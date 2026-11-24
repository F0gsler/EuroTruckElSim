using Godot;

[GlobalClass]
public partial class TruckHandling : Resource
{
	[Export] public string Name = "";
	[Export] public Texture2D Image;
	[Export] public float Speed = 400f;
}
