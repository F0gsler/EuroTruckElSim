using Godot;

public partial class ShopUI : VBoxContainer
{
    [Signal] public delegate void ItemBoughtEventHandler(ShopItem item);

    public override void _Ready()
    {
        foreach (string file in DirAccess.GetFilesAt("res://Items"))
        {
            var item = GD.Load<ShopItem>("res://Items/" + file.Replace(".remap", ""));

            var button = new Button();
            button.Text = item.Name + " - " + item.Price + " kr";
            button.Pressed += () => EmitSignal(SignalName.ItemBought, item);
            AddChild(button);
        }
    }
}
