using Godot;
using Godot.Collections;
using Godot.NativeInterop;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
[GodotClassName("Surface")]
[GlobalClass]
public partial class Surface : Node2D
{
	[Export] public Resource font = ResourceLoader.Load("res://addons/glyphdot/IBMCGA+Sharp.tres");
	private SurfaceFont _font => font is SurfaceFont sf ? sf : null;
	[Export] public int GridWidth = 1, GridHeight = 1;
	public Vector2I GridSize => new(GridWidth, GridHeight);
	public int GlyphCount => GridWidth * GridHeight;
	public Vector2I GlyphSize => _font.GlyphSize;
	public Vector2I CanvasSize => GridSize * _font.GlyphSize;
	public Vector2 ScaledSize => CanvasSize * Scale;
	public record PrintRect(Rect2I src, Color fore, Color back);
	private PrintRect[] grid = new PrintRect[1];

	[Export] Color fore = Colors.White, back = Colors.Black;
	public override void _Ready() {
		ResetGrid();
		//Debug.Print($"{Resource.IsInstanceValid(_font)} {_font}");
	}
	public void SetSize(int w, int h) {
		GridWidth = w;
		GridHeight = h;
		ResetGrid();
	}
	public void ResetGrid () {
		grid = new PrintRect[GlyphCount];
	}

	public override void _Process(double delta){
		base._Process(delta);
		if (Engine.IsEditorHint()){
			QueueRedraw();
			return;
		}
	}
	public override void _Draw(){
		base._Draw();
		if (Engine.IsEditorHint()){
			DrawRect(new(Transform.Origin, GridSize * GlyphSize), back, true);
			return;
		}
        foreach (var (index, rect) in grid.Select((rect, index) => (index, rect))) {
			int x = index % GridWidth,
				y = index / GridWidth;
			var pos = new Vector2I(x, y);
			if(rect == null) {
				continue;
			}
			DrawRect(new Rect2I(pos * GlyphSize, GlyphSize), rect.back);
			DrawTextureRectRegion(_font.Texture, new Rect2I(pos * GlyphSize, GlyphSize), rect.src, rect.fore);
        };
	}
	Rect2I empty => _font.GetSrc(' ');
	public void Clear(){
		foreach(var(index, rect) in grid.Select((rect, index) => (index, rect))){
			grid[index] = new PrintRect(empty, fore, back);
		}
	}
	public void Print(int x, int y, string s)=>
		Print(x, y, s, fore, back);
	public void Print(int x, int y, string s, Color f, Color b){
		foreach(var c in s){
			Print(x, y, c, f, b);
			if((x += 1) == GridWidth){
				x = 0;
				y++;
			}
		}
	}
	public void Print(int x, int y, char c)
	=>
		Print(x, y, c, fore, back);
	public void Print(int x, int y, char c, Color f, Color b){
		//Debug.Print($"{grid.Length} {x} {y} {Resource.IsInstanceValid(_font)} {_font}");
		grid[y * GridWidth + x] = new PrintRect(_font.GetSrc(c), f, b);
	}

	
}

public static class SSurface {
	public static void FullScreen (this Surface s) {
		var ws = DisplayServer.WindowGetSize();
		var v = (ws / (new Vector2I(100, 60) * 8));
		int scale = v[((int)v.MinAxisIndex())];
		s.Scale = new Vector2(scale, scale);
		var font = s.font as SurfaceFont;
		var size = ws / (font.GlyphSize * scale);
		s.SetSize(size.X, size.Y);
		Debug.WriteLine($"{s.Name} resized to {size.X} x {size.Y}");

		s.Position = (ws - s.CanvasSize * scale) / 2;
	}
}
