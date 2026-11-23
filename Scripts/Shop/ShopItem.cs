using Godot;

// Sådan tilføjer du en ny ting til shoppen:
// 1. Højreklik på mappen "Items" i FileSystem
// 2. Vælg New -> Resource... -> ShopItem
// 3. Gem filen i "Items"
// 4. Skriv navn og pris i Inspectoren
[GlobalClass]
public partial class ShopItem : Resource
{
	[Export] public string Name;
	[Export] public int Price;
}
