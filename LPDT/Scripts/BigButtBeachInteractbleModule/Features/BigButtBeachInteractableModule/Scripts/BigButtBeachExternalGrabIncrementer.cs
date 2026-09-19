using System.Collections.Generic;
using Features.GrabModule.Scripts;
using Features.MultiplayerSessionServices.Scripts;
using UnityEngine;
using Zenject;

namespace Features.BigButtBeachInteractableModule.Scripts
{
	public class BigButtBeachExternalGrabIncrementer : MonoBehaviour
	{
		[SerializeField]
		private List<SimplePointGrabable> _simplePointGrabables;

		private MultiplayerModel _multiplayerModel;

		private BigButtBeachExternalGrabEventClass _externalGrabEventClass;

		[Inject]
		public void InjectDependencies(MultiplayerModel multiplayerModel, BigButtBeachExternalGrabEventClass externalGrabEventClass)
		{
			_multiplayerModel = multiplayerModel;
			_externalGrabEventClass = externalGrabEventClass;
		}

		private void OnEnable()
		{
			foreach (SimplePointGrabable simplePointGrabable in _simplePointGrabables)
			{
				if (!(simplePointGrabable == null))
				{
					simplePointGrabable.LocalOnGrab += HandleLocalGrab;
				}
			}
		}

		private void OnDisable()
		{
			foreach (SimplePointGrabable simplePointGrabable in _simplePointGrabables)
			{
				if (!(simplePointGrabable == null))
				{
					simplePointGrabable.LocalOnGrab -= HandleLocalGrab;
				}
			}
		}

		private void HandleLocalGrab(int i)
		{
			if (_multiplayerModel.NetworkRunner == null)
			{
				return;
			}
			int playerId = _multiplayerModel.NetworkRunner.LocalPlayer.PlayerId;
			foreach (SimplePointGrabable simplePointGrabable in _simplePointGrabables)
			{
				if (!(simplePointGrabable == null) && simplePointGrabable.GrabbedByPlayers.Contains(playerId))
				{
					_externalGrabEventClass.InvokeExternalGrab();
					break;
				}
			}
		}
	}
}
