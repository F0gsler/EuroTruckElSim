using System;

// Game rules independent of the scene and input.
// Missionen kører i ring: hent -> aflever -> lad op -> hent igen ...
public sealed class MissionProgress
{
	public enum Stage { Pickup, Delivery, Charge }
	public Stage Current { get; private set; } = Stage.Pickup;
	public int Crates { get; private set; }
	public bool HasCargo => Crates > 0;
	public float BatteryPercent { get; private set; } = ChargeTarget;
	public int Trips { get; private set; }
	public const float ChargeTarget = 80f;
	public const float ChargePerSecond = 10f;
	public const float TripBatteryCost = 40f;
	public const int PayPerCrate = 100;

	// Returnerer hvor mange penge man tjente (0 hvis intet skete)
	public int Interact(Stage location, bool parked, int capacity)
	{
		if (!parked || location != Current) return 0;
		if (Current == Stage.Pickup)
		{
			Crates = capacity;
			Current = Stage.Delivery;
			return 0;
		}
		if (Current == Stage.Delivery)
		{
			int pay = Crates * PayPerCrate;
			Crates = 0;
			BatteryPercent = Math.Max(0, BatteryPercent - TripBatteryCost);
			Trips++;
			Current = Stage.Charge;
			return pay;
		}
		return 0;
	}

	public void TickCharge(double delta, bool parkedAtCharger)
	{
		if (Current != Stage.Charge || !parkedAtCharger || delta <= 0) return;
		BatteryPercent = Math.Min(ChargeTarget, BatteryPercent + (float)delta * ChargePerSecond);
		if (BatteryPercent >= ChargeTarget) Current = Stage.Pickup;
	}
}
