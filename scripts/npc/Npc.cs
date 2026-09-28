using Godot;
using System;

[Icon("res://assets/icons/characters/placeface_icon.png")]
public partial class Npc : CharacterBody3D {
	[Export]
	public NavigationAgent3D NavAgent;

	[Export]
	public AudioStreamPlayer3D SlapAudio;

	[Export]
	public float Speed;

	[Export]
	public Node3D Target;
	private Vector3 targetPosition;

	public override void _Ready() {
	}

	public override void _PhysicsProcess(double delta) {
		var velocity = Velocity;
		if (Target != null) {
			NavAgent.TargetPosition = Target.GlobalPosition;
			Vector3 direction = GlobalPosition.DirectionTo(NavAgent.GetNextPathPosition());
			direction.Y = 0.0f;
			velocity = direction * Speed;

			if(NavAgent.IsNavigationFinished()){
				velocity = Vector3.Zero;
			}
		}
		Velocity = velocity;
		MoveAndSlide();
	}
}
