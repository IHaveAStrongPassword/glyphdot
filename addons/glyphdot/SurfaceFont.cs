using Godot;
using System;
[GodotClassName("SurfaceFont")]
public partial class ConsoleFont : Resource {
	[Export] public Texture2D Texture;
	[Export] public int GlyphHeight,
		GlyphPadding,
		GlyphWidth,
		Columns;
	[Export] string Name;
	[Export] public int SolidGlyphIndex;
	public Vector2I GlyphSize => new(GlyphWidth, GlyphHeight);
	public Rect2I GetSrc(int index) {
		int x = index / Columns, y = index % Columns;
		return new(new Vector2I(x, y) * GlyphSize, GlyphSize);
	}
}
