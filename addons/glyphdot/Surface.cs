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
	[Export] public Resource
		font = ResourceLoader.Load("res://addons/glyphdot/IBMCGA+Sharp.tres");
	[Export] public int
		GridWidth = 1,
		GridHeight = 1;
	[Export] Color
		fore = Colors.White,
		back = Colors.Black;
	private SurfaceFont _font =>	(SurfaceFont) font;
	public Vector2I GlyphSize =>	_font.GlyphSize;
	public int GlyphCount =>		GridWidth * GridHeight;
	public Vector2I GridSize =>		new(GridWidth, GridHeight);
	public Vector2I CanvasSize =>	GridSize * _font.GlyphSize;
	public Vector2 ScaledSize =>	CanvasSize * Scale;
	private Rect2I empty => _font.GetSrc(' ');
	private interface ICell {
		public void Draw (Node2D Surface, SurfaceFont font, Vector2I pos);
	}
	public record ColorCell (Color back) : ICell {
		public void Draw (Node2D Surface, SurfaceFont font, Vector2I pos) {
			var GlyphSize = font.GlyphSize;
			Surface.DrawRect(new Rect2I(pos * GlyphSize, GlyphSize), back);
		}
	}
	private record GlyphCell (Rect2I src, Color fore, Color back, char glyph) : ICell {

		public static readonly GlyphCell empty = new GlyphCell(new Rect2I(), Colors.Transparent, Colors.Transparent, ' ');
		public void Draw(Node2D Surface, SurfaceFont font, Vector2I pos) {
			var GlyphSize = font.GlyphSize;
			Surface.DrawRect(new Rect2I(pos * GlyphSize, GlyphSize), back);
			Surface.DrawTextureRectRegion(font.Texture, new Rect2I(pos * GlyphSize, GlyphSize), src, fore);
		}
	};
	private GlyphCell[] grid = new GlyphCell[1];
	public override void _Ready() {
		ResetGrid();
		//Debug.Print($"{Resource.IsInstanceValid(_font)} {_font}");
	}
	public void ResetGrid () => grid = new GlyphCell[GlyphCount];
	public void SetSize(int w, int h) {
		GridWidth = w;
		GridHeight = h;
		ResetGrid();
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
		foreach(var index in Enumerable.Range(0, grid.Length)) {
			if(grid[index] is not { }rect) continue;
			rect.Draw(this, _font, IndexToPos(index));
		}
	}
	public void Clear(){
		foreach(var index in Enumerable.Range(0, grid.Length))
			grid[index] = null;
	}
	public void Print(int x, int y, string s) => Print(x, y, s, fore, back);
	public void Print(int x, int y, string s, Color f, Color b){
		foreach(var c in s){
			Print(x, y, c, f, b);
			if((x += 1) == GridWidth){
				x = 0;
				y++;
			}
		}
	}
	public void Print(int x, int y, char c) => Print(x, y, c, fore, back);
	public void Print(int x, int y, char c, Color f, Color b){
		//Debug.Print($"{grid.Length} {x} {y} {Resource.IsInstanceValid(_font)} {_font}");
		grid[PosToIndex(x, y)] =
			new GlyphCell(_font.GetSrc(c), f, b, c);
	}

	public Vector2I IndexToPos (int index) => new Vector2I(index % GridWidth, index / GridWidth);
	public int PosToIndex (int x, int y) => y * GridWidth + x;
	public Color GetCellForeground (int x, int y) => grid[PosToIndex(x, y)].fore;
	public Color GetCellBackground (int x, int y) => grid[PosToIndex(x, y)].back;
	public char GetGlyph (int x, int y) => grid[PosToIndex(x, y)].glyph;

	public void SetCellForeground(int x, int y, Color foreground) {
		var index = PosToIndex(x, y);
		grid[index] = (grid[index] ?? GlyphCell.empty) with { fore = foreground };
	}
	public void SetCellBackground(int x, int y, Color background) {
		var index = PosToIndex(x, y);
		grid[index] = (grid[index] ?? GlyphCell.empty) with { back = background };
	}
	public void SetGlyph (int x, int y, char c) {
		var index = PosToIndex(x, y);
		grid[index] = (grid[index] ?? GlyphCell.empty) with { glyph = c };
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
