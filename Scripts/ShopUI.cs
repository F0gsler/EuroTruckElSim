using Godot;

public partial class ShopUI : VBoxContainer
{
	[Export] public Truck Truck;
	[Export] public Node2D ShopStop;

	private AcceptDialog _popup = new AcceptDialog();

	public override void _Ready()
	{
		Visible = false;
		_popup.Title = "Shop";
		AddChild(_popup);

		foreach (string file in DirAccess.GetFilesAt("res://Items"))
		{
			var item = GD.Load<TruckHandling>("res://Items/" + file.Replace(".remap", ""));

			var button = new Button();
			button.Text = item.Name + " - " + item.Price + " kr (fart " + item.Speed + ", cargo " + item.Cargo + " kasser)";
			button.Pressed += () => Buy(item);
			AddChild(button);
		}
	}

	// Er lastbilen ved shoppen?
	private bool AtShop()
	{
		return Mathf.Abs(Truck.GlobalPosition.X - ShopStop.GlobalPosition.X) < 150;
	}

	public override void _UnhandledKeyInput(InputEvent @event)
	{
		if (@event is InputEventKey key && key.Pressed && !key.Echo && key.PhysicalKeycode == Key.E && AtShop())
		{
			Visible = !Visible;
			GetViewport().SetInputAsHandled();
		}
	}

	public override void _Process(double delta)
	{
		// Luk shoppen når man kører væk
		if (Visible && !AtShop())
		{
			Visible = false;
		}
	}

	private void Buy(TruckHandling item)
	{
		if (Truck.Handling == item)
		{
			_popup.DialogText = "Du kører allerede i " + item.Name;
		}
		else if (Truck.Money < item.Price)
		{
			_popup.DialogText = "Du har ikke råd til " + item.Name;
		}
		else
		{
			Truck.Money -= item.Price;
			Truck.SetHandling(item);
			_popup.DialogText = "Du købte " + item.Name + "\nPenge tilbage: " + Truck.Money + " kr";
		}

		_popup.PopupCentered();
	}
}
