using Godot;
using System;

public partial class Demo : Node2D
{

	Node2D SurfaceGD, SurfaceCS;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready(){
		SurfaceGD = GetNode<Node2D>(nameof(SurfaceGD));
		SurfaceCS = GetNode<Node2D>(nameof(SurfaceCS));
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta){
		SurfaceGD.Call("render_string_fb", 0, 1, "[CS->GD]", Colors.YellowGreen, Colors.Black);
		SurfaceGD.Call("queue_redraw");
		if(SurfaceCS is Surface s)
		{
			s.Clear();
			s.Print(0, 1, "[CS->CS]", Colors.SpringGreen, Colors.Black);
			s.QueueRedraw();
		}
		
	}
}
