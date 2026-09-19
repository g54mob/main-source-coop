using UnityEngine;

namespace Features.StrechArmsModule.Scripts.Views.StateVisuals
{
	public abstract class ArmStateVisual : MonoBehaviour
	{
		[SerializeField]
		private ArmState _armState;

		public ArmState ArmState => _armState;

		public abstract void Enable();

		public abstract void Disable();

		public abstract void SetProgress(float progress);
	}
}
