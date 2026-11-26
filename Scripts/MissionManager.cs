using Godot;

public partial class MissionManager : Node2D
{
	private readonly MissionProgress _missions = new();
	private readonly Marker2D[] _stops = new Marker2D[BayStyles.Length];
	private static readonly (string NodeName, Color Color)[] BayStyles =
	{
		("Pickup", Colors.Goldenrod),
		("Delivery", Colors.CornflowerBlue),
		("Charger", Colors.MediumSeaGreen)
	};
	private Truck _truck;
	private CollisionShape2D _truckShape;
	private Label _status;
	private Label _money;
	private ProgressBar _battery;
	private Polygon2D _cargo;
	private bool _interactRequested;

	public override void _Ready()
	{
		_truck = GetNode<Truck>("../Truck");
		_truckShape = _truck.GetNode<CollisionShape2D>("CollisionShape2D");

		for (int i = 0; i < _stops.Length; i++)
		{
			_stops[i] = GetNode<Marker2D>(BayStyles[i].NodeName);
			_stops[i].AddChild(new Label
			{
				Text = BayStyles[i].NodeName.ToUpperInvariant(),
				Position = new Vector2(-100, -150),
				Size = new Vector2(200, 30),
				HorizontalAlignment = HorizontalAlignment.Center
			});
		}
		_cargo = new Polygon2D
		{
			Polygon = new[] { new Vector2(-30, -100), new Vector2(30, -100), new Vector2(30, -40), new Vector2(-30, -40) },
			Color = Colors.Goldenrod, Visible = false
		};
		_truck.AddChild(_cargo);
		CreateHud();
	}

	private void CreateHud()
	{
		var hud = new CanvasLayer();
		AddChild(hud);
		var panel = new PanelContainer { Position = new Vector2(16, 16) };
		hud.AddChild(panel);
		var rows = new VBoxContainer();
		panel.AddChild(rows);
		_money = new Label();
		_money.AddThemeFontSizeOverride("font_size", 28);
		rows.AddChild(_money);
		rows.AddChild(new Label { Text = "A/D eller pile: kør | E: hent / aflever / shop" });
		_status = new Label();
		rows.AddChild(_status);
		_battery = new ProgressBar { CustomMinimumSize = new Vector2(400, 24), MaxValue = 100 };
		rows.AddChild(_battery);
		UpdateHud();
	}

	public override void _UnhandledKeyInput(InputEvent @event)
	{
		if (@event is InputEventKey key && key.Pressed && !key.Echo && key.PhysicalKeycode == Key.E)
		{
			_interactRequested = true;
			GetViewport().SetInputAsHandled();
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		// Input also prevents an interaction from racing a movement tick.
		bool parked = _truck.Velocity.LengthSquared() < 1f
			&& Mathf.IsZeroApprox(Input.GetAxis("move_left", "move_right"));
		if (_interactRequested)
		{
			for (int i = 0; i < _stops.Length; i++)
			{
				if (AtStop(i))
					_truck.Money += _missions.Interact((MissionProgress.Stage)i, parked, _truck.Handling.Cargo);
			}
			_interactRequested = false;
		}
		_missions.TickCharge(delta, parked && AtStop((int)MissionProgress.Stage.Charge));
		_cargo.Visible = _missions.HasCargo;
		UpdateHud();
		QueueRedraw();
	}

	// The whole 388x164 truck fits in a 520x200 bay within these bounds.
	private bool AtStop(int index)
	{
		Vector2 offset = _truckShape.GlobalPosition - _stops[index].GlobalPosition;
		return Mathf.Abs(offset.X) <= 60 && Mathf.Abs(offset.Y) <= 20;
	}

	private void UpdateHud()
	{
		_money.Text = "Pung: " + _truck.Money + " kr";

		string objective = _missions.Current switch
		{
			MissionProgress.Stage.Pickup => "1/3 Hent: parkér i det gule felt og tryk E.",
			MissionProgress.Stage.Delivery => "2/3 Aflever: parkér i det blå felt og tryk E.",
			_ => "3/3 Lad op: hold stille i det grønne felt til 80%."
		};
		string direction;
		int target = (int)_missions.Current;
		float distance = _stops[target].GlobalPosition.X - _truckShape.GlobalPosition.X;
		if (AtStop(target))
			direction = " | Du er fremme";
		else
			direction = distance > 0 ? " | Kør HØJRE >>" : " | << Kør VENSTRE";

		_status.Text = objective + direction
			+ "\nLast: " + _missions.Crates + "/" + _truck.Handling.Cargo + " kasser"
			+ " (" + MissionProgress.PayPerCrate + " kr pr. kasse)"
			+ " | Batteri: " + Mathf.Round(_missions.BatteryPercent) + "%"
			+ " | Ture: " + _missions.Trips;
		_battery.Value = _missions.BatteryPercent;
	}

	public override void _Draw()
	{
		for (int i = 0; i < _stops.Length; i++)
		{
			if (_stops[i] == null) continue;
			var bay = new Rect2(_stops[i].Position - new Vector2(260, 100), new Vector2(520, 200));
			Color color = BayStyles[i].Color;
			DrawRect(bay, new Color(color, 0.18f));
			DrawRect(bay, color, false, (int)_missions.Current == i ? 5f : 2f);
		}
	}
}
