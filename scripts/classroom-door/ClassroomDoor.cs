using Godot;
using System;

[Icon("res://assets/icons/classroom_door.png")]
public partial class ClassroomDoor : Node3D {
	[Export]
	CollisionShape3D DoorCollider;
	[Export]
	Timer Cooldown;

	[Export]
	Sprite3D Door;

	[Export]
	ClickObjectModule ClickModule;

	[Export]
	AudioStreamPlayer3D OpenSound;
	[Export]
	AudioStreamPlayer3D CloseSound;
	
	bool IsOpen = false;

	public override void _Ready() {
		Cooldown.Timeout += Close;
		ClickModule.Clicked += _OnClick;
	}

	public void Open() {
		IsOpen = true;
		Cooldown.Start();
		if (OpenSound != null) { OpenSound.Play(); }
		Door.Frame = 1;
		DoorCollider.SetDeferred("disabled", true);
	}

	public void Close() {
		IsOpen = false;
		if (CloseSound != null) { CloseSound.Play(); }
		Door.Frame = 0;
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
}
