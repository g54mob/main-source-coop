using Features.ScreenShakeModule.Scripts;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.GrabModule.Scripts
{
	[NetworkBehaviourWeaved(0)]
	public class LocalScreenShakeOnGrab : NetworkBehaviour
	{
		[SerializeField]
		private SimplePointGrabable _simplePointGrabable;

		[SerializeField]
		private ScreenShakeData _grabScreenShakeData;

		[SerializeField]
		private ScreenShakeData _ungrabScreenShakeData;

		private IScreenShakeService _screenShakeService;

		[Inject]
		private void InjectDependencies(IScreenShakeService screenShakeService)
		{
			_screenShakeService = screenShakeService;
		}

		private void Start()
		{
			_simplePointGrabable.OnGrab += ShakeScreenOnGrab;
			_simplePointGrabable.OnUnGrab += ShakeScreenOnUnGrab;
		}

		private void OnDestroy()
		{
			NetworkBehaviourUtils.InternalOnDestroy(this);
			_simplePointGrabable.OnGrab -= ShakeScreenOnGrab;
			_simplePointGrabable.OnUnGrab -= ShakeScreenOnUnGrab;
		}

		private void ShakeScreenOnUnGrab()
		{
			_screenShakeService.TriggerLocalScreenShake(_grabScreenShakeData);
		}

		private void ShakeScreenOnGrab()
		{
			_screenShakeService.TriggerLocalScreenShake(_ungrabScreenShakeData);
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
