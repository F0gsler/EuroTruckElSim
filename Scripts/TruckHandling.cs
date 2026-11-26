using Godot;

// Sådan tilføjer du en ny lastbil til shoppen:
// 1. Højreklik på mappen "Items" i FileSystem
// 2. Vælg Create New -> Resource... -> TruckHandling
// 3. Gem filen i "Items"
// 4. Udfyld navn, pris, billede og fart i Inspectoren
[GlobalClass]
public partial class TruckHandling : Resource
{
	[Export] public string Name = "";
	[Export] public int Price;
	[Export] public int co2;
	[Export] public Texture2D Image;
	[Export] public float Speed = 400f;
}
