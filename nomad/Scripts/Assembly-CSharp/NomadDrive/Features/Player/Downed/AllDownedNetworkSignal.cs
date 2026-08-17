using EvilCore;
using EvilCore.Extensions;
using EvilCore.UI.Scripts;
using Mirror;
using Mirror.RemoteCalls;
using UnityEngine;
using VContainer;

namespace NomadDrive.Features.Player.Downed
{
	public class AllDownedNetworkSignal : NetworkBehaviour
	{
		[Inject]
		private IGameOverService _gameOverService;

		[Inject]
		private IPlayerService _playerService;

		private IAllDownedPopup _popup;

		private bool _confirmHooked;

		private void Awake()
		{
			base.gameObject.InjectGameObject();
		}

		[Inject]
		private void ResolvePopup(IObjectResolver resolver)
		{
			resolver.TryResolve<IAllDownedPopup>(out _popup);
		}

		public override void OnStartServer()
		{
			base.OnStartServer();
			_gameOverService?.ServerResetGameOver();
			if (_gameOverService != null)
			{
				_gameOverService.OnGameOver += HandleGameOver;
			}
		}

		public override void OnStopServer()
		{
			base.OnStopServer();
			if (_gameOverService != null)
			{
				_gameOverService.OnGameOver -= HandleGameOver;
			}
		}

		public override void OnStartClient()
		{
			base.OnStartClient();
			if (_popup != null && !_confirmHooked)
			{
				_popup.OnConfirmed += OnPopupConfirmed;
				_confirmHooked = true;
			}
		}

		public override void OnStopClient()
		{
			base.OnStopClient();
			UnhookConfirm();
		}

		private void OnDestroy()
		{
			if (_gameOverService != null)
			{
				_gameOverService.OnGameOver -= HandleGameOver;
			}
			UnhookConfirm();
		}

		private void UnhookConfirm()
		{
			if (_popup != null && _confirmHooked)
			{
				_popup.OnConfirmed -= OnPopupConfirmed;
				_confirmHooked = false;
			}
		}

		private void HandleGameOver(GameOverReason reason)
		{
			RpcShowAllDownedPopup();
		}

		[ClientRpc]
		private void RpcShowAllDownedPopup()
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			SendRPCInternal("System.Void NomadDrive.Features.Player.Downed.AllDownedNetworkSignal::RpcShowAllDownedPopup()", -1421835433, writer, 0, includeOwner: true);
			NetworkWriterPool.Return(writer);
		}

		private void OnPopupConfirmed()
		{
			SetLocalDownedLookSuspended(suspended: false);
		}

		private void SetLocalDownedLookSuspended(bool suspended)
		{
			if (_playerService != null && _playerService.IsPlayerSpawned && _playerService.LocalPlayer != null && _playerService.LocalPlayer.TryGetComponent<PlayerDeathController>(out var component))
			{
				component.SuspendDownedLook(suspended);
			}
		}

		public override bool Weaved()
		{
			return true;
		}

		protected void UserCode_RpcShowAllDownedPopup()
		{
			_popup?.Show();
			SetLocalDownedLookSuspended(suspended: true);
		}

		protected static void InvokeUserCode_RpcShowAllDownedPopup(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkClient.active)
			{
				Debug.LogError("RPC RpcShowAllDownedPopup called on server.");
			}
			else
			{
				((AllDownedNetworkSignal)obj).UserCode_RpcShowAllDownedPopup();
			}
		}

		static AllDownedNetworkSignal()
		{
			RemoteProcedureCalls.RegisterRpc(typeof(AllDownedNetworkSignal), "System.Void NomadDrive.Features.Player.Downed.AllDownedNetworkSignal::RpcShowAllDownedPopup()", InvokeUserCode_RpcShowAllDownedPopup);
		}
	}
}
