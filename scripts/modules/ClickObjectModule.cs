using Godot;
using System;

[GlobalClass, Icon("res://assets/icons/cursors/cursor_sprite.png")]
public partial class ClickObjectModule : RayCast3D {
	[Signal]
	public delegate void ClickedEventHandler();

	[Export]
	public MouseButton ButtonIndex { get; set; }

	[Export]
	public float Length = 10;
	[Export]
	public Node ClickableObject;

	public override void _UnhandledInput(InputEvent @event) {
		if (@event is InputEventMouseButton) {
			InputEventMouseButton ev = (InputEventMouseButton)@event;
			if (ev.IsPressed() && ev.ButtonIndex == ButtonIndex) {
				_ShootRay();
				var collider = GetCollider();
				if (collider == ClickableObject) {
				}
				EmitSignal(SignalName.Clicked);
			}
		}
	}

	private void _ShootRay() {
		var current_camera = GetViewport().GetCamera3D();
		GlobalPosition = current_camera.GlobalPosition;
		Vector3 dir = current_camera.GlobalRotation.Normalized();
		TargetPosition = dir * Length;
		GD.Print(TargetPosition);
	}
}
