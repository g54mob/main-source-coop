using System;
using Features.RagdollModule.Scripts;
using UnityEngine;

namespace Features.DamageableTrackModule.Scripts
{
	[Serializable]
	public class DamageData
	{
		public float Damage;

		public Vector3 Position;

		public Vector3 Direction;

		public int DamageDealerPlayerID;

		public float Force;

		public ForceMode ForceMode;

		public bool IsStunning;

		public StunDurationPreset StunDurationPreset;

		public DamageSource? Source;
	}
}
