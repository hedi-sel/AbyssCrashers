using Godot;
using System;

public partial class SummonStation : Node2D
{
	[Export] private Node2D _p2;
	[Export] private Node2D _p3;
	[Export] private Node2D _p4;

	public Vector2 GetPlayerPosition(int playerIndex)
	{
		return playerIndex switch
		{
			1 => GlobalPosition,
			2 => _p2.GlobalPosition,
			3 => _p3.GlobalPosition,
			4 => _p4.GlobalPosition,
			_ => throw new Exception("Player index must be 1, 2, 3 or 4")
		};
	}
}
