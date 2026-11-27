using Godot;

public partial class Truck : CharacterBody2D
{
	[Export] public TruckHandling Handling;
	[Export] public int Money = 0; // Din pung

	public override void _Ready()
	{
		SetHandling(Handling);
	}

	// Skifter lastbilen (bruges også af shoppen)
	public void SetHandling(TruckHandling handling)
	{
		Handling = handling;
		GetNode<Sprite2D>("Sprite2D").Texture = handling.Image;
	}

	public override void _PhysicsProcess(double delta)
	{
		float retning = Input.GetAxis("move_left", "move_right");

		// Vend billedet efter hvilken vej man kører
		if (retning < 0)
		{
			GetNode<Sprite2D>("Sprite2D").FlipH = true;
		}
		else if (retning > 0)
		{
			GetNode<Sprite2D>("Sprite2D").FlipH = false;
		}

		Velocity = new Vector2(retning * Handling.Speed, 0);
		MoveAndSlide();
	}
}
