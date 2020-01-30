using Godot;
using System;

public class Rafinery : Building
{
	/*
	....
	*/

	protected int oil;
	protected int fuel;
	protected int OilInMachine;
	private int capacity;

	private Timer timer;

	public Rafinery() : base (100)
	{
		this.oil = 0;
		this.fuel = 0;
		this.capacity = 5000;
		this.OilInMachine = 0;
	}

	///...
	public void AddOil (int oil)
	{
		if (oil <= capacity)
		{
			if (oil + OilInMachine <= capacity)
			{
				OilInMachine += oil;
			}
			OilInMachine = capacity;
			oil -= capacity;
		}
	}


	public override void _EnterTree()
	{
		timer = GetNode<Timer>("Timer");
	}


	/* Equivalent a la fonction _Process
		delta => timer.WaitTime
	*/
	public void _on_Timer_timeout()
	{
		if (OilInMachine >= 10)
		{
			fuel += 2;
			OilInMachine -= 10;
		}
	}
}
