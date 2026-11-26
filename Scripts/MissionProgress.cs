using System;

// Game rules independent of the scene and input.
public sealed class MissionProgress
{
	public enum Stage { Pickup, Delivery, Charge, Complete }
	public Stage Current { get; private set; } = Stage.Pickup;
	public bool HasCargo => Current == Stage.Delivery;
	public float BatteryPercent { get; private set; } = 40f;
	public const float ChargeTarget = 80f;
	public const float ChargePerSecond = 10f;

	public bool Interact(Stage location, bool parked)
	{
		if (!parked || location != Current) return false;
		if (Current == Stage.Pickup)
		{
			Current = Stage.Delivery;
			return true;
		}
		if (Current == Stage.Delivery)
		{
			Current = Stage.Charge;
			return true;
		}
		return false;
	}

	public void TickCharge(double delta, bool parkedAtCharger)
	{
		if (Current != Stage.Charge || !parkedAtCharger || delta <= 0) return;
		BatteryPercent = Math.Min(ChargeTarget, BatteryPercent + (float)delta * ChargePerSecond);
		if (BatteryPercent >= ChargeTarget) Current = Stage.Complete;
	}
}
