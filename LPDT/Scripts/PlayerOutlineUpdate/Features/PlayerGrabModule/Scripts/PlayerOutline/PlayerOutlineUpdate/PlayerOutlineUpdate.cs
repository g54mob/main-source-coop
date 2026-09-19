using Features.GrabModule.Scripts;
using Fusion;
using QuickOutline.Scripts;
using UnityEngine;
using Zenject;

namespace Features.PlayerGrabModule.Scripts.PlayerOutline.PlayerOutlineUpdate
{
	public class PlayerOutlineUpdate : MonoBehaviour
	{
		[SerializeField]
		private NetworkObject _networkObject;

		[SerializeField]
		private SimplePointGrabable _simplePointGrabableOutline;

		private PLayerGrabOutlineModel _pLayerGrabOutlineModel;

		[Inject]
		private void InjectDependencies(PLayerGrabOutlineModel pLayerGrabOutlineModel)
		{
			_pLayerGrabOutlineModel = pLayerGrabOutlineModel;
		}

		private void OnEnable()
		{
			_pLayerGrabOutlineModel.OnOutlineAdded += UpdateOutline;
		}

		private void OnDisable()
		{
			_pLayerGrabOutlineModel.OnOutlineAdded -= UpdateOutline;
		}

		private void UpdateOutline(int playerId, Outline outline)
		{
			if (_networkObject.InputAuthority.PlayerId == playerId)
			{
				_simplePointGrabableOutline.Outline = outline;
			}
		}
	}
}
