extends Node2D
func _ready():
	$SurfaceGD.render_string_fb(0, 1, "[GD->GD]", Color.RED, Color.BLACK)
	$SurfaceCS.Clear()
	$SurfaceCS.Print(0, 1, "[GD->CS]", Color.ORANGE, Color.BLACK)
	$SurfaceCS.queue_redraw()
func _process(delta):
	pass
