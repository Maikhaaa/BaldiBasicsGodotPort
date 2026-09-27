using Godot;
using System;

public partial class npc : CharacterBody3D {

	[Export]
	NavigationAgent3D NavAgent;

	public override void _Ready() {
	}

	public override void _PhysicsProcess(double delta) {
	}
}
