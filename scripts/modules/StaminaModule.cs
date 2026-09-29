using Godot;
using System;

/// <section>
/// Class <c>StaminaModule</c> managed the state of stmaina value.
/// </section>

[GlobalClass, Icon("res://assets/icons/objects/boots_icon.png")]
public partial class StaminaModule : Node {
	[Signal]
	public delegate void StaminaChangedEventHandler();
	[Signal]
	public delegate void StaminaDeplededEventHandler();

	[Export]
	public float MaxStamina = 100.0f;
	[Export]
	public float StaminaRate = 10.0f;
	

	[Export]
	public ProgressBar ProgressBar;

	private float stamina = 100.0f;

	public override void _Process(double delta) {
		if (ProgressBar != null) {
			ProgressBar.Value = stamina;
		}
	}

	/// <section>
	/// Function <c>Increase</c> Increases the value of stamina by StaminaRate * deltaTime.
	/// </section>
	public void Increase() {
		float delta = (float)GetProcessDeltaTime();
		stamina += StaminaRate * delta * 2.75f;
		stamina = Mathf.Clamp(stamina, 0.0f, MaxStamina);
		EmitSignal(SignalName.StaminaChanged);
	}


	public void Decrease() {
		float delta = (float)GetProcessDeltaTime();
		stamina -= StaminaRate * delta * 2.75f;
		if (stamina <= 0) {
			EmitSignal(SignalName.StaminaDepleded);
			stamina = 0;
		}
		EmitSignal(SignalName.StaminaChanged);
	}

	// TODO: think of better name later
	public bool IsNotFull() {
		return stamina < MaxStamina;
	}
	
	public bool IsNotDepleted() {
		return stamina != 0.0f;
	}

}
