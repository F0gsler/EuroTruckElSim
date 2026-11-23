using Godot;

public partial class Main : Node
{
    [Export] public int Money = 5000;

    public void OnItemBought(ShopItem item)
    {
        if (Money < item.Price)
        {
            GD.Print("Ikke råd til " + item.Name);
            return;
        }

        Money -= item.Price;
        GD.Print("Købte " + item.Name + ". Penge tilbage: " + Money);
    }
}
