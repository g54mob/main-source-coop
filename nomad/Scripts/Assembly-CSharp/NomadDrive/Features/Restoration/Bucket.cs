using Cysharp.Threading.Tasks;
using Mirror;
using Mirror.RemoteCalls;
using NomadDrive.Features.LiquidDrinking;
using NomadDrive.Features.LiquidTransferSystem;
using NomadDrive.Features.ObjectPlacement;
using NomadDrive.Features.Vehicle;
using UnityEngine;

namespace NomadDrive.Features.Restoration
{
	[RequireComponent(typeof(SnappingPlanesManager))]
	public class Bucket : DrinkableHeldItem
	{
		[Header("Cleaning Settings")]
		[SerializeField]
		[Range(0.01f, 0.5f)]
		private float _cleaningSpeed = 0.1f;

		[SerializeField]
		[Range(0.1f, 5f)]
		private float _waterPerConditionUnit = 1f;

		private SnappingPlanesManager _snappingPlanesManager;

		private SnappingPlane _cleaningPlane;

		private CleanTool _currentCleanTool;

		private bool _isCleaningActive;

		protected override void Awake()
		{
			base.Awake();
			_snappingPlanesManager = GetComponent<SnappingPlanesManager>();
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			if (base.LiquidContainer != null)
			{
				base.LiquidContainer.OnLiquidAmountChangedEvent.AddListener(OnLiquidAmountChanged);
			}
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			if (base.LiquidContainer != null)
			{
				base.LiquidContainer.OnLiquidAmountChangedEvent.RemoveListener(OnLiquidAmountChanged);
			}
		}

		public override void OnStartServer()
		{
			base.OnStartServer();
			if (base.LiquidContainer != null)
			{
				base.LiquidContainer.SetLiquidType(LiquidType.Water);
				base.LiquidContainer.ServerApplyRandomizedFill();
			}
			RegisterSnappingPlaneEvents();
			ResumeCleaningForExistingToolsAsync().Forget();
		}

		public override void OnStartClient()
		{
			base.OnStartClient();
			RegisterSnappingPlaneEvents();
		}

		private void RegisterSnappingPlaneEvents()
		{
			SnappingPlane[] snappingPlanes = _snappingPlanesManager.SnappingPlanes;
			if (snappingPlanes == null || snappingPlanes.Length == 0)
			{
				return;
			}
			SnappingPlane[] array = snappingPlanes;
			foreach (SnappingPlane snappingPlane in array)
			{
				if (_cleaningPlane == null || snappingPlane is CleaningSnappingPlane)
				{
					_cleaningPlane = snappingPlane;
				}
				snappingPlane.OnObjectPlaced.AddListener(OnToolPlaced);
				snappingPlane.OnObjectRemoved.AddListener(OnToolRemoved);
			}
		}

		private void OnToolPlaced(GameObject obj)
		{
			if (obj.TryGetComponent<CleanTool>(out var _))
			{
				NetworkIdentity component2 = obj.GetComponent<NetworkIdentity>();
				if (component2 != null)
				{
					CmdOnToolPlaced(component2.netId);
				}
			}
		}

		private void OnToolRemoved(GameObject obj)
		{
			if (obj.TryGetComponent<CleanTool>(out var _))
			{
				CmdOnToolRemoved();
			}
		}

		[Command(requiresAuthority = false)]
		private void CmdOnToolPlaced(uint toolNetId)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteVarUInt(toolNetId);
			SendCommandInternal("System.Void NomadDrive.Features.Restoration.Bucket::CmdOnToolPlaced(System.UInt32)", 633543905, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		[Command(requiresAuthority = false)]
		private void CmdOnToolRemoved()
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			SendCommandInternal("System.Void NomadDrive.Features.Restoration.Bucket::CmdOnToolRemoved()", -412990278, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		private void OnLiquidAmountChanged(float oldAmount, float newAmount)
		{
			UpdateState();
			if (base.isServer && _currentCleanTool != null && !_isCleaningActive && newAmount > 0f)
			{
				TryStartCleaning();
			}
		}

		private bool IsToolStillInBucket()
		{
			if (_currentCleanTool != null && _cleaningPlane != null)
			{
				return _cleaningPlane.PlacedEntityNetworkIDs.Contains(_currentCleanTool.netId);
			}
			return false;
		}

		private void TryStartCleaning()
		{
			if (!_isCleaningActive && !(_currentCleanTool == null) && IsToolStillInBucket() && !(_currentCleanTool.Condition >= 1f) && !(base.LiquidContainer == null) && base.LiquidContainer.State != LiquidContainerState.Empty)
			{
				StartCleaningLoop().Forget();
			}
		}

		private async UniTaskVoid StartCleaningLoop()
		{
			if (!base.isServer)
			{
				return;
			}
			_isCleaningActive = true;
			while (_isCleaningActive && _currentCleanTool != null && IsToolStillInBucket() && !(_currentCleanTool.Condition >= 1f) && base.LiquidContainer.State != LiquidContainerState.Empty)
			{
				float num = _cleaningSpeed * Time.deltaTime;
				float num2 = num * _waterPerConditionUnit;
				float num3 = base.LiquidContainer.Drain(num2);
				if (num3 > 0f)
				{
					float num4 = num2 - num3;
					num = ((_waterPerConditionUnit > 0f) ? (num4 / _waterPerConditionUnit) : 0f);
				}
				if (!(num > 0f))
				{
					break;
				}
				_currentCleanTool.ServerRestoreCondition(num);
				await UniTask.Yield(this.GetCancellationTokenOnDestroy());
			}
			_isCleaningActive = false;
		}

		private async UniTaskVoid ResumeCleaningForExistingToolsAsync()
		{
			SnappingPlane[] snappingPlanes = _snappingPlanesManager.SnappingPlanes;
			if (snappingPlanes == null)
			{
				return;
			}
			SnappingPlane[] array = snappingPlanes;
			foreach (SnappingPlane snappingPlane in array)
			{
				foreach (uint placedEntityNetworkID in snappingPlane.PlacedEntityNetworkIDs)
				{
					(bool, GameObject) tuple = await networkManager.TryGetNetworkObjectByIdAsync(placedEntityNetworkID, 30, 100, this.GetCancellationTokenOnDestroy());
					if (tuple.Item1 && tuple.Item2.TryGetComponent<CleanTool>(out var component))
					{
						_currentCleanTool = component;
						TryStartCleaning();
					}
				}
			}
		}

		public override void OnEquip()
		{
			base.OnEquip();
			_isCleaningActive = false;
			_currentCleanTool = null;
		}

		public override bool Weaved()
		{
			return true;
		}

		protected void UserCode_CmdOnToolPlaced__UInt32(uint toolNetId)
		{
			if (networkManager.TryGetNetworkObjectById(toolNetId, out var networkObject) && networkObject.TryGetComponent<CleanTool>(out var component))
			{
				_currentCleanTool = component;
				TryStartCleaning();
			}
		}

		protected static void InvokeUserCode_CmdOnToolPlaced__UInt32(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdOnToolPlaced called on client.");
			}
			else
			{
				((Bucket)obj).UserCode_CmdOnToolPlaced__UInt32(reader.ReadVarUInt());
			}
		}

		protected void UserCode_CmdOnToolRemoved()
		{
			_isCleaningActive = false;
			_currentCleanTool = null;
		}

		protected static void InvokeUserCode_CmdOnToolRemoved(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdOnToolRemoved called on client.");
			}
			else
			{
				((Bucket)obj).UserCode_CmdOnToolRemoved();
			}
		}

		static Bucket()
		{
			RemoteProcedureCalls.RegisterCommand(typeof(Bucket), "System.Void NomadDrive.Features.Restoration.Bucket::CmdOnToolPlaced(System.UInt32)", InvokeUserCode_CmdOnToolPlaced__UInt32, requiresAuthority: false);
			RemoteProcedureCalls.RegisterCommand(typeof(Bucket), "System.Void NomadDrive.Features.Restoration.Bucket::CmdOnToolRemoved()", InvokeUserCode_CmdOnToolRemoved, requiresAuthority: false);
		}
	}
}
