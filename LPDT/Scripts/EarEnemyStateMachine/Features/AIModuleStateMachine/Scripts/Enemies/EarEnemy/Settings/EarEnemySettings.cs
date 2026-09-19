using System.Collections.Generic;
using FMOD;
using FMODUnity;
using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.Enemies.EarEnemy.Settings
{
	[CreateAssetMenu(fileName = "EarEnemySettings_Default", menuName = "Configurations/AIModuleStateMachine/Ear/EarEnemySettings")]
	public class EarEnemySettings : ScriptableObject
	{
		[Header("Hearing")]
		[SerializeField]
		[Tooltip("FMOD events this Ear ignores. This is Ear-local and does not affect other sound listeners.")]
		private List<EventReference> _ignoredSoundEvents = new List<EventReference>();

		private HashSet<string> _ignoredSoundPaths;

		[Header("Movement")]
		[field: SerializeField]
		public float WanderRadius { get; private set; } = 12f;

		[field: SerializeField]
		public float WanderSpeed { get; private set; } = 3f;

		[field: SerializeField]
		public float AggroSpeed { get; private set; } = 5f;

		[field: SerializeField]
		public float AggroReachTimeout { get; private set; } = 8f;

		[Header("Attack (area, centred on the enemy)")]
		[field: SerializeField]
		public float AttackDuration { get; private set; } = 1.2f;

		[field: SerializeField]
		public float AttackRadius { get; private set; } = 3f;

		[field: SerializeField]
		public float AttackForce { get; private set; } = 8f;

		[field: SerializeField]
		public float AttackKnockbackUpBias { get; private set; } = 0.35f;

		[field: SerializeField]
		public float ItemsDamage { get; private set; } = 10f;

		[Header("Stun (after every attack)")]
		[field: SerializeField]
		public float StunDuration { get; private set; } = 5f;

		[Header("Post Attack Wander")]
		[field: SerializeField]
		public float PostAttackWanderDuration { get; private set; } = 8f;

		private void OnValidate()
		{
			_ignoredSoundPaths = null;
		}

		public bool IsSoundPathIgnored(string soundPath)
		{
			if (string.IsNullOrEmpty(soundPath))
			{
				return false;
			}
			HashSet<string> hashSet = _ignoredSoundPaths ?? BuildIgnoredSoundPaths();
			if (hashSet.Count == _ignoredSoundEvents.Count)
			{
				_ignoredSoundPaths = hashSet;
			}
			return hashSet.Contains(soundPath);
		}

		private HashSet<string> BuildIgnoredSoundPaths()
		{
			HashSet<string> hashSet = new HashSet<string>();
			foreach (EventReference ignoredSoundEvent in _ignoredSoundEvents)
			{
				if (!ignoredSoundEvent.IsNull && RuntimeManager.StudioSystem.getEventByID(ignoredSoundEvent.Guid, out var _event) == RESULT.OK && _event.getPath(out var path) == RESULT.OK && !string.IsNullOrEmpty(path))
				{
					hashSet.Add(path);
				}
			}
			return hashSet;
		}
	}
}
