using Godot;
using System;

[GlobalClass, Icon("res://assets/icons/cursors/cursor_sprite.png")]
public partial class ClickObjectModule : RayCast3D {
	[Signal]
	public delegate void ClickedEventHandler();

	[Export]
	public MouseButton ButtonIndex { get; set; }

	[Export]
	public float Length = 10.0f;

	[Export]
	public Node ClickableObject;

	public override void _Ready() {
		TopLevel = true;
		TargetPosition = new Vector3(0.0f, 0.0f, -Length);
	}

	public override void _UnhandledInput(InputEvent @event) {
		if (@event is InputEventMouseButton) {
			InputEventMouseButton ev = (InputEventMouseButton)@event;
			if (ev.IsPressed() && ev.ButtonIndex == ButtonIndex) {
				var collider = GetCollider();
				if (collider == ClickableObject) {
					EmitSignal(SignalName.Clicked);
				}
			}
		}
	}

	public override void _PhysicsProcess(double delta) {
		var current_camera = GetViewport().GetCamera3D();
		GlobalPosition = current_camera.GlobalPosition;
		GlobalRotation = current_camera.GlobalRotation;
	}
}
