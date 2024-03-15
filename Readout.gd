extends Node2D


# Called when the node enters the scene tree for the first time.
func _ready():
	pass # Replace with function body.
# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	$Surface.render_string_fb(0, 0, "Hello" + str(Time.get_ticks_usec()), Color.RED, Color.BLACK)
	$Surface.queue_redraw()
	pass
