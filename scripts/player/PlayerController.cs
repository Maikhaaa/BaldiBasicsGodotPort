using Godot;
using System;

[Icon("res://assets/icons/player.png")]
public partial class PlayerController : CharacterBody3D
{

	[Export]
	public float MouseSensitivity = 1.0f;
	[Export]
	public Camera3D Camera;

	[Export]
	public float WalkSpeed;
	[Export]
	public float RunSpeed;
	
	private float speed;
	
	[Export]
	public float MaxStamina = 100.0f;
	[Export]
	public float StaminaRate = 10.0f;

	private float stamina;

	public override void _Ready() {
		speed = WalkSpeed;
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
		var magnitude = Vector3.Zero.DistanceTo(velocity);
		if (magnitude > 0.1f) {
			if (Input.IsActionPressed("sprint")) {
				stamina -= StaminaRate * (float)delta;
			}
			if (stamina < 0f & stamina > 5f){
				stamina = -5f;
			}
		} else if (stamina < MaxStamina) {
			stamina += StaminaRate * (float)delta;
		}
	
		speed = (Input.IsActionPressed("sprint")) ? RunSpeed : WalkSpeed;
	
		Vector2 inputDir = Input.GetVector("left", "right", "forward", "backward");
		Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
		
		if (direction != Vector3.Zero) {
			velocity.X = direction.X * speed;
			velocity.Z = direction.Z * speed;
		}
		else {
			velocity.X = Mathf.MoveToward(Velocity.X, 0, speed);
			velocity.Z = Mathf.MoveToward(Velocity.Z, 0, speed);
		}

		Velocity = velocity;
		MoveAndSlide();

		if (Input.IsActionPressed("look_behind")) {
			Camera.Rotation = new Vector3(0.0f, Mathf.Pi, 0.0f);
		}else {
			Camera.Rotation = new Vector3(0.0f, 0.0f, 0.0f);
		}
	}


	private void _LockMouse() {
		Input.MouseMode = Input.MouseModeEnum.Captured;
	}
}
