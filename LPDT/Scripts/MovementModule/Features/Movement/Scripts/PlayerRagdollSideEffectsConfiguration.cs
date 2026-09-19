using System.Collections.Generic;
using Features.RagdollModule.Scripts;
using UnityEngine;

namespace Features.Movement.Scripts
{
	[CreateAssetMenu(fileName = "PlayerRagdollSideEffectsConfiguration_Default", menuName = "Configurations/Movement/PlayerRagdollSideEffectsConfiguration")]
	public class PlayerRagdollSideEffectsConfiguration : ScriptableObject
	{
		[field: SerializeField]
		public List<RagdollSimulationReasonEnum> ReasonsToIgnoreSideEffects { get; private set; }
	}
}
