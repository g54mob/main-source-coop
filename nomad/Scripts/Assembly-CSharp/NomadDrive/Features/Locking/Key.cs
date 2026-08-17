using NomadDrive.Features.Interaction;
using UnityEngine;

namespace NomadDrive.Features.Locking
{
	public class Key : HeldItem
	{
		[Header("Key Tier")]
		[SerializeField]
		private ChestType keyType;

		public ChestType KeyType => keyType;

		public override bool Weaved()
		{
			return true;
		}
	}
}
