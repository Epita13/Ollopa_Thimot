using Godot;
using System;


public class DBConnection
{
    private const string key = "zir4uvqh115pmu41nfv541o";
    
    private static char GetCharBase36(long n)
    {
        char c = '0';
        if (n <= 9)
        {
            c = (char)(n + '0');
        }
        else
        {
            c = (char)(n + 'a' - 10);
        }

        return c;
    }
    
    private static string IntToBase36(int nb)
    {
        long n = nb;
        if (nb < 0)
        {
            int d = Mathf.Abs(-2147483648 - nb);
            n = 2147483648 + d;
        }
        string res = "";
        while (n > 0)
        {
            long v = n % 36;
            res = GetCharBase36(v) + res;
            n = n/36;
        }
        return res;
    }
    
    public static void SendScore(Node n, string name)
    {
        HTTPRequest h = new HTTPRequest();
        n.AddChild(h);
        h.Request("https://www.ollopa.fr/DBrequest/addScore.php?name=" + name + "&version=" + Game.VERSION +
                  "&seed='" + IntToBase36(World.GetSeed()) + "'&score=" + Mathf.FloorToInt(Game.timePlayed) + "&key="+key);
    }
}
