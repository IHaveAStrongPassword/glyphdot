@tool
extends Node2D
class_name SurfaceGD
@export var font:SurfaceFontGD = preload("res://addons/glyphdot/IBMCGA+.tres")
@export var grid_width:int = 4
@export var grid_height:int = 4
var glyph_count:int:
	get:
		return grid_width * grid_height
var grid_size: Vector2i: get = get_grid_size
func get_grid_size(): return Vector2i(grid_width, grid_height)
var canvas_size: Vector2i: get = get_canvas_size
func get_canvas_size(): return grid_size * font.char_size
class DrawRect:
	var src:Rect2i
	var fore:Color
	var back:Color
	func _init(src:Rect2i, fore:Color, back:Color):
		self.src = src
		self.fore = fore
		self.back = back

var grid : Array[DrawRect] = []

@export var fore:Color = Color.WHITE
@export var back:Color = Color.BLACK

@onready var empty:Rect2i
# Called when the node enters the scene tree for the first time.
func _ready():
	var size = grid_size * font.char_size
	
	empty = font.get_src(' '.to_ascii_buffer()[0])
	grid.resize(grid_width * grid_height)
	

	if Engine.is_editor_hint():
		visibility_changed.connect(func():
			queue_redraw()
			)	
# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass
func _draw():
	if Engine.is_editor_hint():
		#var char_size = Vector2i(8, 8)
		var char_size = (font as SurfaceFontGD).char_size
		draw_rect(Rect2i(Vector2i.ZERO, grid_size * char_size), back, true)
		
		for i in range(glyph_count):
			var x = i % grid_width
			var y = floor(i / grid_width)
			var pos = Vector2i(x, y)
			
			draw_rect(Rect2i(pos * char_size, char_size), back, true)
			draw_rect(Rect2i(pos * char_size, char_size), fore, false)
			
		return
	for i in range(len(grid)):
		var r := grid[i]
		if r == null:
			continue
		var x = i % grid_width
		var y = floor(i / grid_width)
		var pos = Vector2i(x, y)
		
		draw_rect(Rect2i(pos * font.char_size, font.char_size), r.back)
		draw_texture_rect_region(font.texture, Rect2i(pos * font.char_size, font.char_size), r.src, r.fore)

func clear():
	for i in range(len(grid)):
		grid[i] = DrawRect.new(empty, fore, back)
func render_string(x:int, y:int, str:String):
	render_string_fb(x, y, str, fore, back)
func render_string_fb(x:int, y:int, str:String, f:Color, b:Color):
	for c:int in str.to_ascii_buffer():
		render_char_fb(x, y, c, f, b)
		x += 1
		if x == grid_width:
			x = 0
			y += 1
func render_char(x:int, y:int, char:int) -> void:
	render_char_fb(x, y, char, fore, back)
func render_char_fb(x:int, y:int, char:int, f:Color, b:Color) -> void:
	#var dest = Rect2i(Vector2i(x, y) * font.char_size, font.char_size)
	#var src = font.get_src(char)
	#draw_rect(dest, b)
	#draw_texture_rect_region(font.texture, dest, src, f)
	var index = y * grid_width + x
	grid[index] = DrawRect.new(font.get_src(char), f, b)
	
