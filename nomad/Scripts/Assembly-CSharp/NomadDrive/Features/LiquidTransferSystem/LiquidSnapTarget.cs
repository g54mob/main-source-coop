using EvilCore.EvilPack.EvilLogger;
using EvilCore.Extensions;
using Mirror;
using NomadDrive.Features.Player;
using UnityEngine;
using VContainer;

namespace NomadDrive.Features.LiquidTransferSystem
{
	public class LiquidSnapTarget : MonoBehaviour, ILiquidSnapTarget
	{
		[SerializeField]
		private Transform fillAnchor;

		[SerializeField]
		private LiquidSnapTargetType targetType;

		[Inject]
		private IPlayerService _playerService;

		private NetworkIdentity _identity;

		public Transform FillAnchor => fillAnchor;

		public LiquidSnapTargetType TargetType => targetType;

		public uint SnapNetId
		{
			get
			{
				if (!(_identity != null))
				{
					return 0u;
				}
				return _identity.netId;
			}
		}

		private void Awake()
		{
			base.gameObject.InjectGameObject();
			_identity = GetComponent<NetworkIdentity>();
			if (_identity == null)
			{
				EvilLogger.LogError("LiquidSnapTarget on " + base.name + " must share its GameObject with the container's NetworkIdentity so remote clients can resolve the snap anchor.", "Awake", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\LiquidTransferSystem\\Scripts\\Core\\LiquidSnapTarget.cs", 36);
			}
		}

		public void SetSnap(bool active)
		{
			LiquidSnapTrigger.Set(_playerService, this, active);
		}
	}
}
