extends Node2D


# Called when the node enters the scene tree for the first time.
func _ready():
	$SurfaceGD.render_string_fb(1, 1, "Yes", Color.GREEN, Color.BLACK)
	$Surface.Print(1, 1, "Yes", Color.GREEN, Color.BLACK)
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass
