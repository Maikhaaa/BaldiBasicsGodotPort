using Godot;
using System;

[Icon("res://assets/icons/classroom_door.png")]
public partial class Door : Node3D {
	[Export]
	CollisionShape3D DoorCollider;
	[Export]
	Timer Cooldown;

	[Export]
	Sprite3D DoorIn;
	[Export]
	Sprite3D DoorOut;

	[Export]
	ClickObjectModule ClickModule;

	[Export]
	Area3D Proximity;

	[Export]
	AudioStreamPlayer3D OpenSound;
	[Export]
	AudioStreamPlayer3D CloseSound;
	
	bool IsOpen = false;

	public override void _Ready() {
		Cooldown.Timeout += Close;
		if (ClickModule != null) { ClickModule.Clicked += _OnClick; };
		if (Proximity != null) { Proximity.BodyEntered += _OnBodyEntered; };
	}

	public void Open() {
		IsOpen = true;
		Cooldown.Start();
		if (OpenSound != null) { OpenSound.Play(); }
		DoorIn.Frame = 1;
		DoorOut.Frame = 1;
		DoorCollider.SetDeferred("disabled", true);
	}

	public void Close() {
		IsOpen = false;
		if (CloseSound != null) { CloseSound.Play(); }
		DoorIn.Frame = 0;
		DoorOut.Frame = 0;
		DoorCollider.Visible = true;
		DoorCollider.SetDeferred("disabled", false);
	}

	private void _OnClick() {
		if (IsOpen) {
			Cooldown.Start();
			return;
		}
		Open();
	}

	private void _OnBodyEntered(Node3D body) {
		if (IsOpen) {
			Cooldown.Start();
			return;
		}
		Open();
	}
}
