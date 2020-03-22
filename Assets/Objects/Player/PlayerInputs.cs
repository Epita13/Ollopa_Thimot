using Godot;
using System;

public class PlayerInputs : Node2D
{

	public static bool playerInputActive = true;
	

	[Signal] delegate void BlockPlaced();

	private Vector2 mousePos;
	private PlayerState.State lastState;
	private Usable.Type lastSelectedUsable;

	public override void _Ready()
	{
		Player.inventoryUsables.Add(Usable.Type.Dirt, 30);
		Player.inventoryUsables.Add(Usable.Type.Grass, 30);
		Player.inventoryUsables.Add(Usable.Type.Stone, 30);
		ConnectSignals();
		Player.inventoryItems.Add(Item.Type.Composite, 12);

	}

	private void ConnectSignals()
	{
		if (GetTree().GetNodesInGroup("ToolBar").Count==1)
			Connect("BlockPlaced", (Node)GetTree().GetNodesInGroup("ToolBar")[0], "SendRefresh"); // Pour Actualisation de la ToolBar
	}

  
	public override void _Process(float delta)
	{
		if (!playerInputActive)
			return;
		
		if (lastState!=PlayerState.GetState() || lastSelectedUsable!=Player.UsableSelected)
		{
			World.UIBlockTilemap.Clear();
			lastSelectedUsable = Player.UsableSelected;
			lastState = PlayerState.GetState();
		}

		mousePos = Convertion.Location2WorldFloor(GetGlobalMousePosition());

		//Affichage
		if (PlayerState.GetState() == PlayerState.State.Normal)
		{
			NormalState();
		}
		else if (PlayerState.GetState() == PlayerState.State.Build)
		{
			BuildState();
		}


		/* Inventory Click */
		if (Input.IsActionJustPressed("inventory"))
		{
			InventoryClick();
		}		
		
		//Inputs
		if (Input.IsActionJustPressed("mouse1"))
		{
			if (PlayerState.GetState() == PlayerState.State.Normal)
			{
				ClickNormalState();
			}
			else if (PlayerState.GetState() == PlayerState.State.Build)
			{
				ClickBuildState();
			}
			
		}
		else if (Input.IsActionJustPressed("mouse2"))
		{
			if (PlayerState.GetState() == PlayerState.State.Normal)
			{
				PlayerState.SetState(PlayerState.State.Build);
			}
			else
			{
				PlayerState.SetState(PlayerState.State.Normal);
			}
		}
	}


	private void NormalState()
	{
		Usable.Type type = Player.UsableSelected;
		Usable.Category cat = Usable.category[(int)type];
		if(cat==Usable.Category.Block)
		{
			World.UIBlockTilemap.Clear();
			int amount = Player.inventoryUsables.GetItemCount(type);
			bool canPlace = PlaceBlock.CanPlace((int)mousePos.x, (int)mousePos.y, out canPlace);
			if (amount>0 && canPlace && MouseInRange(9,false))
			{
				World.UIBlockTilemap.SetCell((int)mousePos.x, -(int)mousePos.y+Chunk.height, 1);
			}
			else
			{
				World.UIBlockTilemap.SetCell((int)mousePos.x, -(int)mousePos.y+Chunk.height, 0);
			}
		}
	}

	private void PrintBatRight(int idtile)
	{
		int j = 0;
		int i = 0;
		while (j < 4 )
		{
			i = 0;
			while (i < 4)
			{
				World.UIBlockTilemap.SetCell((int) mousePos.x+i, -(int) mousePos.y + Chunk.height-j, idtile);
				i++;
			}
			j++;
		}
	}

	private void PrintBatLeft(int idtile)
	{
		int j = 0;
		int i = 0;
		while (j < 4)
		{
			i = 0;
			while (i < 4)
			{
				World.UIBlockTilemap.SetCell((int) mousePos.x - i, -(int) mousePos.y + Chunk.height - j, idtile);
				i++;
			}

			j++;
		}
	}

	private void BuildState()
	{
		Vector2 vec = mousePos;
		int x = (int) mousePos.x;
		int y = (int) mousePos.y;
		World.UIBlockTilemap.Clear();
		Vector2 playerPos = Convertion.Location2World(PlayerMouvements.instance.Position);
		bool right = playerPos.x-1 < mousePos.x;
		if (right)
		{
			if (BasicPlacement.IsPlacableRight(x, y, 4, 4) && MouseInRange(9, true))
			{
				PrintBatRight(1);
			}
			else
			{
				PrintBatRight(0);
			}
		}
		else 
		{
			if (BasicPlacement.IsPlacableLeft(x, y, 4, 4) && MouseInRange(9, true))
			{
				PrintBatLeft(1);
			}
			else
			{
				PrintBatLeft(0);
			}
		}
}


	private void ClickNormalState()
	{
		Usable.Type type = Player.UsableSelected;
		Usable.Category cat = Usable.category[(int)type];
		if (MouseInRange(9,false))
		{
			if(cat==Usable.Category.Block)
			{
				int amount = Player.inventoryUsables.GetItemCount(type);
				if (amount>0)
				{
					bool succeed = PlaceBlock.Place((int)mousePos.x, (int)mousePos.y, Usable.blocks[type]);
					if (succeed)
					{
						Player.inventoryUsables.Remove(type, 1);
						EmitSignal("BlockPlaced");
					}
				}
			}else
			{
				//World.GetChunk((int)mousePos.x).RemoveBlock(Chunk.GetLocaleX((int)mousePos.x), (int)mousePos.y);
			}
		}
	}

	private void ClickBuildState()
	{
		Vector2 playerPos = Convertion.Location2World(PlayerMouvements.instance.Position);
		bool right = playerPos.x-1 < mousePos.x;
		if (MouseInRange(10,true))
		{
			Storage sk = (Storage) Building.prefabs[Building.Type.Storage].Instance();
			BasicPlacement.PlaceWithMouse(sk, GetGlobalMousePosition(),right);
		}
	}

	private bool MouseInRange(int range,bool onPlayer)
	{
		Vector2 playerPos = Convertion.Location2World(PlayerMouvements.instance.Position);
		float xmin = Mathf.Floor(playerPos.x - PlayerMouvements.size.x / 2);
		float ymin = Mathf.Floor(playerPos.y-PlayerMouvements.size.y/2);
		float xmax = Mathf.Floor(playerPos.x+PlayerMouvements.size.x/2);
		float ymax = Mathf.Floor(playerPos.y+PlayerMouvements.size.y/2);
		GD.Print(mousePos);
		GD.Print(xmin, " ", xmax, "  -  ", ymin, " ", ymax);
		if (!onPlayer)
			if (Mathf.Floor(mousePos.x)<=xmax && Mathf.Floor(mousePos.x)>=xmin && Mathf.Floor(mousePos.y)<=ymax && Mathf.Floor(mousePos.y)>=ymin)
				return false;
		int x = Mathf.FloorToInt(mousePos.x);
		int y = Mathf.FloorToInt(mousePos.y);
		float distance = Mathf.Sqrt( Mathf.Pow((x-playerPos.x),2) + Mathf.Pow((y-playerPos.y),2));
		return (distance<range); 
	}


	private void InventoryClick()
	{
		if (PlayerState.GetState() != PlayerState.State.Inventory)
		{
			PlayerState.SetState(PlayerState.State.Inventory);
			UI_PlayerInventory.Open("item");
			GD.Print("okok");
		}
		else
		{
			UI_PlayerInventory.Close();
		}
	}


}
