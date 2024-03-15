using Godot;
using Godot.Collections;
using Godot.NativeInterop;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Surface : Node2D
{
	[Export] public ConsoleFont font;
	[Export] public int GridWidth, GridHeight;
	public Vector2I GridSize => new(GridWidth, GridHeight);
	public int GlyphCount => GridWidth * GridHeight;
	public Vector2I GlyphSize => font.GlyphSize;
	public Vector2I CanvasSize => GridSize * font.GlyphSize;

	record PrintRect(Rect2I src, Color fore, Color back);
	private List<PrintRect> grid;

	[Export] Color fore = Colors.White, back = Colors.Black;
	public override void _Ready() {
		grid.EnsureCapacity(GlyphCount);
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
				y = index / GridHeight;
			var pos = new Vector2I(x, y);
			if(rect == null) {
				continue;
			}
			DrawRect(new Rect2I(pos * GlyphSize, GlyphSize), rect.back);
			DrawTextureRectRegion(font.Texture, new Rect2I(pos * GlyphSize, GlyphSize), rect.src, rect.fore);
        };
	}
	Rect2I empty => font.GetSrc(' ');
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
	public void Print(int x, int y, char c, Color f, Color b) =>
		grid[y * GridHeight + x] = new PrintRect(font.GetSrc(c), f, b);
}
