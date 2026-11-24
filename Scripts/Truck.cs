using Godot;

public partial class Truck : CharacterBody2D
{
	[Export] public float Speed = 400f;

	public override void _PhysicsProcess(double delta)
	{
		float retning = Input.GetAxis("move_left", "move_right");

		Velocity = new Vector2(retning * Speed, 0);
		MoveAndSlide();
	}
}
