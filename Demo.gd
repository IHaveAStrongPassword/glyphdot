extends Node2D
func _ready():
	$SurfaceGD.render_string_fb(1, 1, "GD->GD", Color.RED, Color.BLACK)
	$SurfaceCS.Clear()
	$SurfaceCS.Print(1, 1, "GD->CS", Color.ORANGE, Color.BLACK)
func _process(delta):
	pass
