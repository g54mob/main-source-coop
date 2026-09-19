using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.MimicEnemy
{
	public class MimicHandGrabbedStateInitializer : MonoBehaviour
	{
		private static readonly int _isGrabbedHash = Animator.StringToHash("IsGrabbed");

		[SerializeField]
		private Animator _animator;

		[SerializeField]
		private bool _isGrabbed = true;

		private void Awake()
		{
			_animator.SetBool(_isGrabbedHash, _isGrabbed);
		}
	}
}
