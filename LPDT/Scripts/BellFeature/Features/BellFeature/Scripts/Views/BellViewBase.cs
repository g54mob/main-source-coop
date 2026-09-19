using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using UnityEngine;

namespace Features.BellFeature.Scripts.Views
{
	public abstract class BellViewBase : ViewBehaviour
	{
		private static readonly int IsActiveHash = Animator.StringToHash("IsActive");

		[SerializeField]
		private Animator _animator;

		public void SetIsActive(bool isActive)
		{
			_animator.SetBool(IsActiveHash, isActive);
		}
	}
}
