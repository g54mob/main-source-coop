using EvilCore.Extensions;
using NomadDrive.Features.Vehicle.Networking;
using UnityEngine;

namespace NomadDrive.Features.Vehicle
{
	public static class VehicleStuckDetector
	{
		private const float UprightDotThreshold = 0.5f;

		private const float MinPenetrationDepth = 0.12f;

		public static bool IsStuck(NetworkedNWHVehicle vehicle, LayerMask worldMask)
		{
			if (vehicle == null)
			{
				return false;
			}
			if (Vector3.Dot(vehicle.transform.up, Vector3.up) < 0.5f)
			{
				return true;
			}
			Collider[] array = CollectBodyColliders(vehicle);
			if (array.Length == 0)
			{
				return false;
			}
			if (array.GetPenetrationsInLayerUnified(worldMask, out var correction))
			{
				return correction.magnitude >= 0.12f;
			}
			return false;
		}

		private static Collider[] CollectBodyColliders(NetworkedNWHVehicle vehicle)
		{
			Collider[] componentsInChildren = vehicle.GetComponentsInChildren<Collider>();
			int num = 0;
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				if (componentsInChildren[i] != null && !componentsInChildren[i].isTrigger)
				{
					num++;
				}
			}
			if (num == componentsInChildren.Length)
			{
				return componentsInChildren;
			}
			Collider[] array = new Collider[num];
			int num2 = 0;
			for (int j = 0; j < componentsInChildren.Length; j++)
			{
				if (componentsInChildren[j] != null && !componentsInChildren[j].isTrigger)
				{
					array[num2++] = componentsInChildren[j];
				}
			}
			return array;
		}
	}
}
