extends Resource
class_name SurfaceFontGD
@export var texture: Texture2D
@export var char_height:int
@export var char_padding:int
@export var char_width:int

@export var columns:int
@export var name:String
@export var solid_char_index:int
var char_size:Vector2i: get = get_char_size
func get_char_size(): return Vector2i(char_width, char_height)
func get_src(index:int) -> Rect2i:
	var y = floor(index / columns)
	var x = index % columns
	return Rect2i(Vector2i(x, y) * char_size, char_size)
