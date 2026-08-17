using System.Linq;
using Cysharp.Threading.Tasks;
using EvilCore.EvilPack.EvilLogger;
using Mirror;
using Mirror.RemoteCalls;
using NomadDrive.Features.Interaction;
using NomadDrive.Features.ObjectPlacement;
using NomadDrive.Features.Objectives;
using NomadDrive.Features.Vehicle;
using UnityEngine;
using UnityEngine.Events;

namespace NomadDrive.Features.Cooking
{
	[RequireComponent(typeof(SnappingPlanesManager))]
	public class Stove : HeldItem
	{
		private SnappingPlanesManager _snappingPlanesManager;

		public UnityEvent<ushort, uint> OnPanAttached { get; } = new UnityEvent<ushort, uint>();

		public UnityEvent<ushort, uint> OnPanRemoved { get; } = new UnityEvent<ushort, uint>();

		public InteractableHandle[] InteractableHandles { get; private set; }

		private SnappingPlane[] SnappingPlanes => _snappingPlanesManager.SnappingPlanes;

		protected override void Awake()
		{
			base.Awake();
			_snappingPlanesManager = GetComponent<SnappingPlanesManager>();
		}

		protected override void Start()
		{
			base.Start();
		}

		public override void OnStartServer()
		{
			base.OnStartServer();
		}

		public override void OnStartClient()
		{
			base.OnStartClient();
			Init();
		}

		public override void OnStopClient()
		{
			base.OnStopClient();
			OnPanAttached.RemoveListener(OnPanAttachedActions);
			OnPanRemoved.RemoveListener(OnPanRemovedActions);
		}

		private void Init()
		{
			OnPanAttached.AddListener(OnPanAttachedActions);
			OnPanRemoved.AddListener(OnPanRemovedActions);
			SetupSnappingPlanesAsync().Forget();
			InteractableHandles = GetComponentsInChildren<InteractableHandle>();
			InteractableHandles = InteractableHandles.OrderBy((InteractableHandle h) => h.transform.GetSiblingIndex()).ToArray();
			InteractableHandle[] interactableHandles = InteractableHandles;
			foreach (InteractableHandle interactableHandle in interactableHandles)
			{
				int handleID = InteractableHandles.ToList().IndexOf(interactableHandle);
				interactableHandle.onHandleRotateTarget.AddListener(delegate
				{
					OnHandleRotateRightActions((ushort)handleID);
				});
				interactableHandle.onHandleRotateInitial.AddListener(delegate
				{
					OnHandleRotateLeftActions((ushort)handleID);
				});
			}
			SetForLateJoin();
		}

		private async UniTaskVoid SetupSnappingPlanesAsync()
		{
			SnappingPlane[] snappingPlanes = SnappingPlanes;
			foreach (SnappingPlane snappingPlane in snappingPlanes)
			{
				(bool, GameObject) tuple = await networkManager.TryGetNetworkObjectByIdAsync(snappingPlane.netId, 30, 100, this.GetCancellationTokenOnDestroy());
				if (tuple.Item1 && tuple.Item2 != null)
				{
					ushort planeID = _snappingPlanesManager.GetSnappingPlaneIDBySnappingPlaneReference(snappingPlane);
					snappingPlane.OnObjectPlaced.AddListener(delegate(GameObject go)
					{
						OnPanAttached.Invoke(planeID, go.GetComponent<NetworkIdentity>().netId);
					});
					snappingPlane.OnObjectRemoved.AddListener(delegate(GameObject go)
					{
						OnPanRemoved.Invoke(planeID, go.GetComponent<NetworkIdentity>().netId);
					});
				}
			}
		}

		private void SetForLateJoin()
		{
			foreach (CookingPotSlot item in SnappingPlanes.Cast<CookingPotSlot>())
			{
				if (item.SlotState == CookingPotSlotState.On && item.IsAnyObjectPlaced && networkManager.TryGetNetworkObjectById(item.SingleSlotPlacedEntityNetworkID, out var networkObject))
				{
					networkObject.GetComponent<Pan>().StartCookingProcess();
				}
			}
		}

		private void OnPanRemovedActions(ushort planeLocalID, uint panNetID)
		{
			DetachPanFromSlot(planeLocalID, panNetID);
			Pan payload = null;
			if (networkManager.TryGetNetworkObjectById(panNetID, out var networkObject))
			{
				payload = networkObject.GetComponent<Pan>();
			}
			ObjectivesEventBus.Raise(ObjectiveSignal.PanRemovedFromStove, payload);
		}

		private void OnPanAttachedActions(ushort planeLocalID, uint panNetID)
		{
			AttachPanToSlot(planeLocalID, panNetID);
			Pan payload = null;
			if (networkManager.TryGetNetworkObjectById(panNetID, out var networkObject))
			{
				payload = networkObject.GetComponent<Pan>();
			}
			ObjectivesEventBus.Raise(ObjectiveSignal.PanAttachedToStove, payload);
		}

		private void OnHandleRotateRightActions(ushort handleLocalID)
		{
			Ignite(handleLocalID);
		}

		private void OnHandleRotateLeftActions(ushort handleLocalID)
		{
			Extinguish(handleLocalID);
		}

		private void AttachPanToSlot(ushort slotLocalID, uint panNetID)
		{
			if ((SnappingPlanes[slotLocalID] as CookingPotSlot).SlotState == CookingPotSlotState.On && networkManager.TryGetNetworkObjectById(panNetID, out var networkObject))
			{
				networkObject.GetComponent<Pan>().StartCookingProcess();
			}
		}

		private void DetachPanFromSlot(ushort planeLocalID, uint panNetID)
		{
			if ((SnappingPlanes[planeLocalID] as CookingPotSlot).SlotState == CookingPotSlotState.On && networkManager.TryGetNetworkObjectById(panNetID, out var networkObject))
			{
				Pan component = networkObject.GetComponent<Pan>();
				if (!(component == null))
				{
					component.StopCookingProcess();
				}
			}
		}

		private void Ignite(ushort handleLocalIndex)
		{
			if (handleLocalIndex < InteractableHandles.Length && !(InteractableHandles[handleLocalIndex] == null))
			{
				CmdIgnite(handleLocalIndex);
			}
		}

		[Command(requiresAuthority = false)]
		private void CmdIgnite(ushort handleLocalIndex)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteUShort(handleLocalIndex);
			SendCommandInternal("System.Void NomadDrive.Features.Cooking.Stove::CmdIgnite(System.UInt16)", -190803812, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		private void Extinguish(ushort handleLocalIndex)
		{
			if (handleLocalIndex < InteractableHandles.Length && !(InteractableHandles[handleLocalIndex] == null))
			{
				if (TryGetPanByLocalIndex(handleLocalIndex, out var pan))
				{
					pan.StopCookingProcess();
				}
				CmdExtinguish(handleLocalIndex);
			}
		}

		[Command(requiresAuthority = false)]
		private void CmdExtinguish(ushort handleLocalIndex)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteUShort(handleLocalIndex);
			SendCommandInternal("System.Void NomadDrive.Features.Cooking.Stove::CmdExtinguish(System.UInt16)", 913433338, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		public void UpdateAllCookingPotSlots()
		{
			SnappingPlane[] snappingPlanes = SnappingPlanes;
			for (int i = 0; i < snappingPlanes.Length; i++)
			{
				_ = (CookingPotSlot)snappingPlanes[i];
			}
		}

		private CookingPotSlotState GetCookingPotSlotState(ushort planeLocalIndex)
		{
			return (SnappingPlanes[planeLocalIndex] as CookingPotSlot).SlotState;
		}

		private CookingPotSlotState GetSlotStateByHandleLocalID(ushort handleLocalIndex)
		{
			return (SnappingPlanes[handleLocalIndex] as CookingPotSlot).SlotState;
		}

		private bool TryGetPanByLocalIndex(ushort localIndex, out Pan pan)
		{
			pan = null;
			if (localIndex >= SnappingPlanes.Length)
			{
				return false;
			}
			if (!(SnappingPlanes[localIndex] is CookingPotSlot cookingPotSlot))
			{
				return false;
			}
			if (!cookingPotSlot.IsAnyObjectPlaced)
			{
				return false;
			}
			if (!cookingPotSlot.IsSingleUseSlot)
			{
				EvilLogger.LogError("[" + base.gameObject.name + "] Slot is not a single use slot", "TryGetPanByLocalIndex", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\Cooking\\Scripts\\Stove.cs", 293);
				return false;
			}
			uint singleSlotPlacedEntityNetworkID = cookingPotSlot.SingleSlotPlacedEntityNetworkID;
			if (singleSlotPlacedEntityNetworkID == 0)
			{
				return false;
			}
			if (!networkManager.TryGetNetworkObjectById(singleSlotPlacedEntityNetworkID, out var networkObject))
			{
				return false;
			}
			pan = networkObject.GetComponent<Pan>();
			if (pan == null)
			{
				return false;
			}
			return true;
		}

		private void SetSlotStateByHandleIndex(ushort handleIndex, CookingPotSlotState slotState)
		{
			(SnappingPlanes[handleIndex] as CookingPotSlot).SetSlotState(slotState);
		}

		public override bool Weaved()
		{
			return true;
		}

		protected void UserCode_CmdIgnite__UInt16(ushort handleLocalIndex)
		{
			SetSlotStateByHandleIndex(handleLocalIndex, CookingPotSlotState.On);
			if (TryGetPanByLocalIndex(handleLocalIndex, out var pan))
			{
				pan.StartCookingProcess();
			}
		}

		protected static void InvokeUserCode_CmdIgnite__UInt16(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdIgnite called on client.");
			}
			else
			{
				((Stove)obj).UserCode_CmdIgnite__UInt16(reader.ReadUShort());
			}
		}

		protected void UserCode_CmdExtinguish__UInt16(ushort handleLocalIndex)
		{
			SetSlotStateByHandleIndex(handleLocalIndex, CookingPotSlotState.Off);
		}

		protected static void InvokeUserCode_CmdExtinguish__UInt16(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdExtinguish called on client.");
			}
			else
			{
				((Stove)obj).UserCode_CmdExtinguish__UInt16(reader.ReadUShort());
			}
		}

		static Stove()
		{
			RemoteProcedureCalls.RegisterCommand(typeof(Stove), "System.Void NomadDrive.Features.Cooking.Stove::CmdIgnite(System.UInt16)", InvokeUserCode_CmdIgnite__UInt16, requiresAuthority: false);
			RemoteProcedureCalls.RegisterCommand(typeof(Stove), "System.Void NomadDrive.Features.Cooking.Stove::CmdExtinguish(System.UInt16)", InvokeUserCode_CmdExtinguish__UInt16, requiresAuthority: false);
		}
	}
}
