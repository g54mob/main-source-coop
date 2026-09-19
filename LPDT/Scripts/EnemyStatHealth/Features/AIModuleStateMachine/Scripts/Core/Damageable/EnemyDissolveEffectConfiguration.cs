using FMODUnity;
using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.Core.Damageable
{
	[CreateAssetMenu(fileName = "EnemyDissolveEffectConfiguration_Default", menuName = "Configurations/EnemyDissolveEffectConfiguration")]
	public class EnemyDissolveEffectConfiguration : ScriptableObject
	{
		[field: SerializeField]
		public EventReference DissolveSound { get; private set; }

		[field: SerializeField]
		public Color SpawnEdgeColor { get; private set; } = new Color(12f, 0f, 24f, 0f);

		[field: SerializeField]
		public Color DespawnEdgeColor { get; private set; } = new Color(12f, 0f, 24f, 0f);
	}
}
