using System;
using FMODUnity;
using UnityEngine;

namespace Features.ChestScreamerModule.Scripts.Presets
{
	[Serializable]
	public class ChestScreamerPreset
	{
		[SerializeField]
		private ChestScreamerType _type;

		[SerializeField]
		private ChestScreamerLifetime _screamerPrefab;

		[SerializeField]
		private EventReference _swarmSound;

		[SerializeField]
		private EventReference _lidSlamSound;

		[SerializeField]
		private float _minOpenAngleDegrees = 12f;

		[SerializeField]
		private float _minGrabHoldSeconds = 0.75f;

		[SerializeField]
		private float _openImpulsePerMass = 2f;

		[SerializeField]
		private float _openImpulseForwardBias = 0.35f;

		[SerializeField]
		private float _holdOpenAngleDegrees = 120f;

		[SerializeField]
		private float _unblockGrabTimer = 5f;

		[SerializeField]
		private bool _isBlockedGrabbleAfterScreamer;

		[SerializeField]
		private float _weight = 1f;

		public ChestScreamerType Type => _type;

		public ChestScreamerLifetime ScreamerPrefab => _screamerPrefab;

		public EventReference SwarmSound => _swarmSound;

		public EventReference LidSlamSound => _lidSlamSound;

		public float MinOpenAngleDegrees => _minOpenAngleDegrees;

		public float MinGrabHoldSeconds => _minGrabHoldSeconds;

		public float OpenImpulsePerMass => _openImpulsePerMass;

		public float OpenImpulseForwardBias => _openImpulseForwardBias;

		public float HoldOpenAngleDegrees => _holdOpenAngleDegrees;

		public float UnblockGrabTimer => _unblockGrabTimer;

		public bool IsBlockedGrabbleAfterScreamer => _isBlockedGrabbleAfterScreamer;

		public float Weight => _weight;
	}
}
