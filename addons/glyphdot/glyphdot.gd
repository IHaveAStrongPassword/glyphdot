@tool
extends EditorPlugin


func _enter_tree():
	add_custom_type("Surface", "Node2D", preload("res://addons/glyphdot/Surface.gd"), preload("res://icon.svg"))
	pass


func _exit_tree():
	# Clean-up of the plugin goes here.
	pass
