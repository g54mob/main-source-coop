using Features.GrabModule.Scripts;
using Features.InteractModule.Scripts;
using Fusion;
using UnityEngine;

namespace Features.CrocodileGameModule.Scripts
{
	[NetworkBehaviourWeaved(0)]
	public class CrocodileToothInteractable : InteractableBase
	{
		[SerializeField]
		private CrocodileGameBehaviour _crocodileGame;

		[SerializeField]
		private int _toothIndex;

		[SerializeField]
		private CrocodileToothView _toothView;

		[SerializeField]
		private SimplePointGrabable _simplePointGrabable;

		private bool _wasLocalPlayerGrabbing;

		public int ToothIndex => _toothIndex;

		public CrocodileToothView ToothView => _toothView;

		public override void Spawned()
		{
			base.Spawned();
			if (_simplePointGrabable == null)
			{
				_simplePointGrabable = GetComponent<SimplePointGrabable>();
			}
			if (_simplePointGrabable != null)
			{
				_simplePointGrabable.OnGrabbedPlayersChanged += OnGrabbedPlayersChanged;
			}
			_toothView?.Initialize();
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			base.Despawned(runner, hasState);
			if (_simplePointGrabable != null)
			{
				_simplePointGrabable.OnGrabbedPlayersChanged -= OnGrabbedPlayersChanged;
			}
		}

		private void OnGrabbedPlayersChanged()
		{
			bool flag = _simplePointGrabable.GrabbedByPlayers.Contains(base.Runner.LocalPlayer.PlayerId);
			if (flag && !_wasLocalPlayerGrabbing)
			{
				HandleLocalPlayerGrabbed();
			}
			_wasLocalPlayerGrabbing = flag;
		}

		private void HandleLocalPlayerGrabbed()
		{
			if (IsInteractable && !(_crocodileGame == null))
			{
				if (!_crocodileGame.CanPressTooth(_toothIndex))
				{
					_simplePointGrabable.LocalGrabBlocked = true;
					ReleaseLocalGrab();
				}
				else
				{
					_simplePointGrabable.LocalGrabBlocked = true;
					_crocodileGame.RequestPressTooth(_toothIndex, base.Runner.LocalPlayer);
					ReleaseLocalGrab();
				}
			}
		}

		public void SetLocalGrabBlocked(bool blocked)
		{
			if (!(_simplePointGrabable == null))
			{
				_simplePointGrabable.LocalGrabBlocked = blocked;
			}
		}

		public void SyncNetworkGrabBlocked(bool blocked)
		{
			if (!(_simplePointGrabable == null))
			{
				if (blocked)
				{
					_simplePointGrabable.BlockGrabRPC();
				}
				else
				{
					_simplePointGrabable.EnableGrabRPC();
				}
			}
		}

		public void ApplyPressState(bool isPressed, bool syncNetworkGrabBlocked)
		{
			_toothView?.SetPressed(isPressed);
			SetLocalGrabBlocked(isPressed);
			if (syncNetworkGrabBlocked)
			{
				SyncNetworkGrabBlocked(isPressed);
			}
		}

		private void ReleaseLocalGrab()
		{
			if (!(_simplePointGrabable == null) && !(base.Runner == null) && _simplePointGrabable.GrabbedByPlayers.Contains(base.Runner.LocalPlayer.PlayerId))
			{
				_simplePointGrabable.UnGrabbedByPlayer(base.Runner.LocalPlayer.PlayerId);
				_wasLocalPlayerGrabbing = false;
			}
		}

		public override void Interact()
		{
		}

		private void OnValidate()
		{
			if (_toothView == null)
			{
				_toothView = GetComponent<CrocodileToothView>();
			}
			if (_simplePointGrabable == null)
			{
				_simplePointGrabable = GetComponent<SimplePointGrabable>();
			}
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			base.CopyBackingFieldsToState(P_0);
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			base.CopyStateToBackingFields();
		}
	}
}
