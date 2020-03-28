using Godot;
using System;
using System.Collections.Generic;
using Array = Godot.Collections.Array;
using System.Diagnostics;
using System.Runtime.InteropServices;

public class LiquidMove : TileMap
{
    /*Il reste un petit soucis, c'est que de l'eau se teleporte plus loin lorsque on ne la limite plus a une cuvette*/


    /*Pour utiliser l'eau, il suffit d'appeler la fonction DrawWaterLevel(), pour les niveaux, le niveau max est
     défini par capacity. Pour fonctionner correctement le TileSet associé doit contenir au minimum un sprite pour chaque
     niveau. Numeroté de 1 a capacity. le sprite 0 doit OBLIGATOIREMENT etre un sprite transparent*/

    /*Ne prend pas en compte le y = 0 de la TileMap Watermap parce que ya des soucis avec le changement de coordonnées sinon*/


    private List<Tuple<int,int,int>> map = new List<Tuple<int,int,int>>{};
    private List<Tuple<int,int,int>> ToRemove = new List<Tuple<int,int,int>>{};
    private List<Tuple<int,int,int>> ToFusion = new List<Tuple<int,int,int>>{};
    private static Tuple<int, int, int> ToFind;
    private const int Capacity = Liquid.Capacity;
    private int width;                             //Hauteur et largeur de la matrice qui gere l'eau
    private readonly int height;
    public readonly Liquid.Type type;
    private TileMap mapdraw;

    private Predicate<Tuple<int, int, int>> predicat = Predicat;


    private int test = 0;
    
    
    public void Move()
    {
        World.IsInitWorldTest("Liquid." + type);
        width = World.size * Chunk.size;
        DrawWaterLevel();
        test++;
    }
 
    public LiquidMove(Liquid.Type type)
    {
        height = Chunk.height;
        this.type = type;
        mapdraw = new TileMap();
    }
 
 
    public void PlaceWater(int x, int y)
    {
        if (Block.GetIDTile(World.GetBlock(x, y).GetType) == -1)
            map.Add(new Tuple<int, int, int>( x, y, 8));
    }
 
    private void DrawWaterLevel()
    {
        //Récupere les niveaux et emplacement d'eau puis calcule les nouveux niveau verticaux puis horizontaux
        Update();
        VerticalWater();
        Update();
        DrawWater();
        //HorizontalWater();
        Update();
        DrawWater();
    }

    private void Remove()
    {
        foreach (Tuple<int,int,int> block in ToRemove)
        {
            Liquid.Watermap.SetCell(block.Item1, height - block.Item2, -1);
            map.Remove(block);
        }
        ToRemove.Clear();
    }
    private void Update()
    {
        for (int i = 0; i < map.Count; i++)
        {
            Tuple<int, int, int> block = map[i];
            if (block.Item3 <= 0 || block.Item1 < 0 || block.Item2 < 0 || Block.GetIDTile(World.GetBlock(block.Item1, block.Item2).GetType) != -1)
                ToRemove.Add(block);
        }
        Remove();
    }
 
    private void HorizontalWater()
    {
        //Calcule la difference d'eau avec les tuiles voisines d'une tuiles contenant de l'eau.
        //Redefinit ensuite le nouveau niveau en fonction des blocks deja present
        int lgr = map.Count;
        for(int i = 0; i < lgr; i++)
        {
            Tuple<int, int, int> block = map[i];
            Tuple<int, int, int> blockinf = Find(block.Item1, block.Item2 - 1, lgr);

            int differenceLeft = Difference(block, 'L', lgr);
            int differenceRight = Difference(block, 'R', lgr);
            
            if (block.Item2 > 0 && Block.GetIDTile(World.GetBlock(block.Item1 - 1, block.Item2).GetType) == -1 &&
                Block.GetIDTile(World.GetBlock(block.Item1 + 1, block.Item2).GetType) == -1 &&
                (blockinf.Item3 == 8 || blockinf.Item3 == -1))
            {
                //Cas standard, pas de bloc ou mur ni a gauche, ni a droite
                if (differenceLeft > differenceRight)
                    Mouvement(block, 'L', lgr, i);
                else if (differenceLeft < differenceRight)
                    Mouvement(block, 'R', lgr, i);
                else if (differenceLeft == differenceRight && differenceLeft != 0)
                    Mouvement(block, 'R', lgr, i);
            }
            else if (block.Item2 > 0 && Block.GetIDTile(World.GetBlock(block.Item1 - 1, block.Item2).GetType) != -1 &&
                     Block.GetIDTile(World.GetBlock(block.Item1 + 1, block.Item2).GetType) == 0 && differenceRight != 0 &&
                     (blockinf.Item3 == 8 || blockinf.Item3 == -1))
                //Cas bloc ou mur a gauche et PAS a gauche
                Mouvement(block, 'R', lgr, i);
            else if (block.Item2 > 0 && Block.GetIDTile(World.GetBlock(block.Item1 + 1, block.Item2).GetType) != -1 &&
                     Block.GetIDTile(World.GetBlock(block.Item1 - 1, block.Item2).GetType) == -1 && differenceLeft != 0 &&
                     (blockinf.Item3 == 8 || blockinf.Item3 == -1))
                //Cas block ou mur a gauche et PAS a droite
                Mouvement(block, 'L', lgr, i);
            
            Remove();
        }
    }
 
    private void VerticalWater()
    {
        //Calcule pour toutes les tuiles vide la difference d'eau avec la tuile au dessus.
        //Transfert l'eau si possible
        int lgr = map.Count;
 
        for(int i = 0; i < lgr; i++)
        {
            Tuple<int, int, int> block = map[i];
            Tuple<int,int,int> blockinf = Find(block.Item1, block.Item2 - 1, lgr);
            int h2 = blockinf.Item3;
            int h = block.Item3;
            int difference = Capacity;

            if (blockinf.Item1 == -1 && blockinf.Item2 == -1)
            {
                blockinf = new Tuple<int, int, int>(block.Item1, block.Item2 - 1, -1);
            }
                
 
            if (Block.GetIDTile(World.GetBlock(block.Item1, block.Item2 - 1).GetType) == -1 && block.Item2 > 0)
            {
                if (h2 > 0)
                    difference -= h2;

                if (difference != 0)
                {
                    if (block.Item3 < difference)
                    {
                        h2 += block.Item3;
                        h = 0;
                    }
                    else
                    {
                        h2 = Capacity;
                        h -= difference;
                    }

                    map[i] = new Tuple<int, int, int>(block.Item1, block.Item2, h);
                    ToFind = blockinf;
                    if (map.BinarySearch(blockinf) < 0)
                    {
                        map.Add(new Tuple<int, int, int>(blockinf.Item1, blockinf.Item2, h2));
                    }
                    else
                    {
                        map[map.BinarySearch(blockinf)] = new Tuple<int, int, int>(blockinf.Item1, blockinf.Item2, h2);
                    }
                }
            }
        }
        
        
        for (int i = 0; i < map.Count && Double(map) ; i++)
        {
            int h = 0;
            int j = 1;
            ToFind = map[i];
            ToFusion = map.FindAll(predicat);
            foreach (Tuple<int,int,int> block in ToFusion)
            {
                h += block.Item3;
            }
          
            map.RemoveAll(predicat);
            if (h > 8)
            {
                while (h > 8)
                {
                    map.Add(new Tuple<int, int, int>(ToFind.Item1, ToFind.Item2 + j, 8));
                    j++;
                    h -= 8;
                }
                map.Add(new Tuple<int, int, int>(ToFind.Item1, ToFind.Item2, h));
            }
            else
            {
                map.Add(new Tuple<int, int, int>(ToFind.Item1,ToFind.Item2,h));
            }
              
              
        }



    }
 
 
    private void DrawWater()
    {
        //Dessine sur la Tilemap les niveaux d'eau correspondant à la matrice
 
        foreach (Tuple<int, int, int> block in map)
        {
            Liquid.Watermap.SetCell(block.Item1, height - block.Item2, block.Item3);
        }
    }
 
    private int Difference(Tuple<int,int,int> block, char side, int lgr)
    {
        //Calcule la difference d'eau avec le block de droite ou de gauche selon side
        int dif = 0;
        int x = block.Item1;
        int y = block.Item2;
        
        switch (side)
        {
            case 'R':
            {
                Tuple<int, int, int> blockinf = Find(x - 1, y, lgr);
 
                if (blockinf.Item3 > 0 && block.Item3 > blockinf.Item3)
                    dif = block.Item3 - blockinf.Item3;
                else if (block.Item3 > blockinf.Item3)
                    dif = block.Item3;
 
                break;
            }
            case 'L':
            {
                Tuple<int, int, int> blocksup = Find(x + 1, y, lgr);
 
                if (blocksup.Item3 > 0 && block.Item3 > blocksup.Item3)
                    dif = block.Item3 - blocksup.Item3;
                else if (block.Item3 > blocksup.Item3)
                    dif = block.Item3;
 
                break;
            }
            default:
                throw new ArgumentException("Character different of 'R' or 'L' from Difference");
        }
 
        if (dif < 0)
            dif = 0;
 
        return dif;
    }
 
    private void Mouvement(Tuple<int,int,int> block, char side, int lgr, int index)
    {
        //Deplace horizontalement 1 d'eau selon side
        int x = block.Item1;
        int y = block.Item2;
        int h = block.Item3;
 
        switch (side)
        {
            case 'R':
            {
                Tuple<int, int, int> blocksup = Find(x + 1, y, lgr);
                if (blocksup.Item1 == -1 && blocksup.Item2 == -1)
                {
                    blocksup = new Tuple<int, int, int>(block.Item1 + 1, block.Item2, -1);
                }
                
                if (map.BinarySearch(blocksup) < 0)
                    map.Add(new Tuple<int, int, int>(x + 1, y, 1));
                else
                {
                    ToRemove.Add(blocksup);
                    map[map.BinarySearch(blocksup)] = new Tuple<int, int, int>(x + 1, y, blocksup.Item3 + 1);
                }
 
                break;
            }
            case 'L':
            {
                Tuple<int, int, int> blockinf = Find(x - 1, y, lgr);
                if (blockinf.Item1 == -1 && blockinf.Item2 == -1)
                {
                    blockinf = new Tuple<int, int, int>(block.Item1 - 1, block.Item2, -1);
                }

                if (map.BinarySearch(blockinf) < 0)
                    map.Add(new Tuple<int, int, int>(x + 1, y, 1));
                else
                {
                    ToRemove.Add(blockinf);
                    map[map.BinarySearch(blockinf)] = new Tuple<int, int, int>(x + 1, y, blockinf.Item3 + 1);
                }
 
                break;
            }
            default:
                throw new ArgumentException("Character different of 'R' or 'L' from Mouvement");
        }
 
        ToRemove.Add(block);
        map[index] = new Tuple<int, int, int>(x, y, block.Item3 - 1);
    }

    private Tuple<int, int, int> Find(int x, int y, int lgr = -1, int start = 0)
    {
        if (lgr == -1)
            lgr = map.Count;

        Tuple<int, int, int> res;
        int i = 0;
        do
        {
            res = map[i];
            i++;
        } while ((res.Item1 != x || res.Item2 != y) && i < lgr);
 
        if (i >= lgr)
            res = new Tuple<int, int, int>(-1, -1, -1);
 
        return res;
    }

    
    private static bool Predicat(Tuple<int, int, int> block)
    {
        return block.Item1 == ToFind.Item1 && block.Item2 == ToFind.Item2;
    }

    private static bool Double(List<Tuple<int, int, int>> water)
    {
        bool res = false;
        int i = 0;

        while (!res && i < water.Count)
        {
            ToFind = water[i];
            if (water.FindAll(Predicat).Count > 1)
                res = true;
            i++;
        }
        
        return res;
    }
}