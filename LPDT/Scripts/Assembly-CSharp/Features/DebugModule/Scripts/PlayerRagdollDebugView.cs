using System;
using UnityEngine;
using UnityEngine.UI;

namespace Features.DebugModule.Scripts
{
	public class PlayerRagdollDebugView : PlayerRagdollDebugViewBase
	{
		[SerializeField]
		private Toggle _ragdollSimulationToggle;

		public override event Action<bool> OnRagdollSimulationChanged;

		protected override void OnEnable()
		{
			base.OnEnable();
			_ragdollSimulationToggle.onValueChanged.AddListener(InvokeOnRagdollSimulationChanged);
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			_ragdollSimulationToggle.onValueChanged.RemoveListener(InvokeOnRagdollSimulationChanged);
		}

		private void InvokeOnRagdollSimulationChanged(bool ragdollSimulated)
		{
			OnRagdollSimulationChanged?.Invoke(ragdollSimulated);
		}

		public override void RefreshToggle(bool isEnabled)
		{
			_ragdollSimulationToggle.isOn = isEnabled;
		}
	}
}
