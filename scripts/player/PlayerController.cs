using Godot;
using System;

public partial class PlayerController : CharacterBody3D
{
	[Export]
	public float Speed = 5.0f;
	[Export]
	public float MouseSensitivity = 1.0f;
	[Export]
	public Camera3D Camera;

	public override void _Ready() {
		_LockMouse();
	}

	public override void _UnhandledInput(InputEvent @event) {
		if (@event is InputEventMouseMotion) {
			InputEventMouseMotion ev = (InputEventMouseMotion)@event;
			RotateY(-ev.Relative.X * MouseSensitivity);
		}
	}

	public override void _PhysicsProcess(double delta) {
		Vector3 velocity = Velocity;

		Vector2 inputDir = Input.GetVector("left", "right", "forward", "backward");
		Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
		
		if (direction != Vector3.Zero) {
			velocity.X = direction.X * Speed;
			velocity.Z = direction.Z * Speed;
		}
		else {
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			velocity.Z = Mathf.MoveToward(Velocity.Z, 0, Speed);
		}

		Velocity = velocity;
		MoveAndSlide();

		if (Input.IsActionJustPressed("look_behind")) {
			Camera.Rotation = new Vector3(0.0f, 180.0f, 0.0f);
		}
	}

	private void _LockMouse() {
		Input.MouseMode = Input.MouseModeEnum.Captured;
	}
}
