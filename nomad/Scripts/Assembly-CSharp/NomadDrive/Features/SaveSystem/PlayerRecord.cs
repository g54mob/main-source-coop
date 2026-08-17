using System;
using UnityEngine;

namespace NomadDrive.Features.SaveSystem
{
	public class PlayerRecord
	{
		public string Puid = string.Empty;

		public string DisplayName = string.Empty;

		public Vector3 Position;

		public Vector3 EulerAngles;

		public byte[] SurvivalBlob = Array.Empty<byte>();

		public string EquippedItemGuid = string.Empty;

		public string SeatGuid = string.Empty;

		public bool IsDowned;
	}
}
