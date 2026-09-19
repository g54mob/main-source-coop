using System.Collections.Generic;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.DeadPartsModule.Scripts
{
	[NetworkBehaviourWeaved(0)]
	public abstract class DeadPartCracksRenderer : NetworkBehaviour
	{
		private static readonly int DestructionFactorProperty = Shader.PropertyToID("_DestructionFactor");

		[SerializeField]
		private List<Renderer> _deadPartRenderers;

		private PlayerDeadPartsConfiguration _playerDeadPartsConfiguration;

		public DeadPartType DeadPartType { get; protected set; }

		[Inject]
		public void InjectDependencies(PlayerDeadPartsConfiguration playerDeadPartsConfiguration)
		{
			_playerDeadPartsConfiguration = playerDeadPartsConfiguration;
		}

		protected void UpdateCracksBasedOnUsage(int usageCount)
		{
			if (DeadPartType == DeadPartType.None)
			{
				return;
			}
			int num = _playerDeadPartsConfiguration.MaxDeadPartUsageCount[DeadPartType];
			float value = Mathf.Clamp01((float)usageCount / (float)num);
			foreach (Renderer deadPartRenderer in _deadPartRenderers)
			{
				deadPartRenderer.material.SetFloat(DestructionFactorProperty, value);
			}
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
		}
	}
}
