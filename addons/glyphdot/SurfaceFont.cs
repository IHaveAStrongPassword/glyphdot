using Godot;
using System;
[GodotClassName("SurfaceFont")]
[GlobalClass]
[Tool] //https://github.com/godotengine/godot/issues/85459#issuecomment-1870680268
public partial class SurfaceFont : Resource {
	[Export] public Texture2D Texture;
	[Export] public int GlyphHeight,
		GlyphPadding,
		GlyphWidth,
		Columns;
	[Export] string Name;
	[Export] public int SolidGlyphIndex;
	public Vector2I GlyphSize => new(GlyphWidth, GlyphHeight);
	public Rect2I GetSrc(int index) {
		int x = index % Columns, y = index / Columns;
		return new(new Vector2I(x, y) * GlyphSize, GlyphSize);
	}
}
