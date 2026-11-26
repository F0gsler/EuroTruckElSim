using Godot;

public partial class ShopUI : VBoxContainer
{
	[Export] public int Money = 5000;
	[Export] public Truck Truck;

	private AcceptDialog _popup = new AcceptDialog();

	public override void _Ready()
	{
		_popup.Title = "Shop";
		AddChild(_popup);

		foreach (string file in DirAccess.GetFilesAt("res://Items"))
		{
			var item = GD.Load<TruckHandling>("res://Items/" + file.Replace(".remap", ""));

			var button = new Button();
			button.Text = item.Name + " - " + item.Price + " kr (fart " + item.Speed + ")";
			button.Pressed += () => Buy(item);
			AddChild(button);
		}
	}

	private void Buy(TruckHandling item)
	{
		if (Truck.Handling == item)
		{
			_popup.DialogText = "Du kører allerede i " + item.Name;
		}
		else if (Money < item.Price)
		{
			_popup.DialogText = "Du har ikke råd til " + item.Name;
		}
		else
		{
			Money -= item.Price;
			Truck.SetHandling(item);
			_popup.DialogText = "Du købte " + item.Name + "\nPenge tilbage: " + Money + " kr";
		}

		_popup.PopupCentered();
	}
}
