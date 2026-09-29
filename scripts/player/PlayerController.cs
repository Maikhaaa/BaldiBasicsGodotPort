using Godot;
using System;

[Icon("res://assets/icons/characters/player.png")]
public partial class PlayerController : CharacterBody3D {
	[Export]
	public float MouseSensitivity = 1.0f;
	[Export]
	public Camera3D _Camera;

	[Export]
	public float WalkSpeed;
	[Export]
	public float RunSpeed;
	[Export]
	public float TiredSpeed;
	
	private float speed;
	
	[Export]
	public StaminaModule Stamina;


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
		
		if (Stamina.IsNotDepleted()) {
			speed = (Input.IsActionPressed("sprint")) ? RunSpeed : WalkSpeed;
		} else {
			speed = TiredSpeed;
		}
		
		if (Stamina != null) {
			_handleStamina();
		}

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
			_Camera.Rotation = new Vector3(0.0f, Mathf.Pi, 0.0f);
		}else {
			_Camera.Rotation = new Vector3(0.0f, 0.0f, 0.0f);
		}
	}


	private void _LockMouse() {
		Input.MouseMode = Input.MouseModeEnum.Captured;
	}

	private void _handleStamina() {
		var magnitude = Vector3.Zero.DistanceTo(Velocity);
		if (magnitude > 0.1f) {
			if (Input.IsActionPressed("sprint")) {
				Stamina.Decrease();
			}
		} else if (Stamina.IsNotFull()) {
			Stamina.Increase();
		}
	}
}
