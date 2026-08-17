using System;
using System.Collections;
using System.Runtime.InteropServices;
using EvilCore.EvilPack.EvilLogger;
using Mirror;
using Mirror.RemoteCalls;
using NomadDrive.Features.Consumables;
using NomadDrive.Features.Interaction;
using NomadDrive.Features.ObjectPlacement;
using NomadDrive.Features.Objectives;
using NomadDrive.Features.Vehicle;
using PrimeTween;
using UnityEngine;
using UnityEngine.Events;

namespace NomadDrive.Features.Cooking
{
	[RequireComponent(typeof(SnappingPlanesManager))]
	public class Pan : HeldItem
	{
		private SnappingPlanesManager _snappingPlanesManager;

		[Header("Cooking Visuals")]
		[SerializeField]
		private GameObject cookingPanSurfaceParticle;

		[SerializeField]
		private float cookingParticleFadeDuration = 0.6f;

		[SerializeField]
		private Ease cookingParticleFadeInEase = Ease.OutSine;

		[SerializeField]
		private Ease cookingParticleFadeOutEase = Ease.InSine;

		private ParticleSystem[] _cookingParticles;

		private ParticleSystemRenderer[] _cookingParticleRenderers;

		private float[] _cookingParticleOriginalRateMultipliers;

		private Color[] _cookingParticleOriginalStartColors;

		private MaterialPropertyBlock _cookingParticlePropertyBlock;

		private Tween _cookingParticleFadeTween;

		private float _cookingParticleCurrentT;

		private static readonly int CookingParticleBaseColorId;

		private static readonly int CookingParticleTintColorId;

		[SyncVar(hook = "OnReadyForCookingConditionChange")]
		private bool _readyForCooking;

		public Action<bool, bool> _Mirror_SyncVarHookDelegate__readyForCooking;

		private UnityEvent<OrganicFood, int> OnFoodAttached { get; } = new UnityEvent<OrganicFood, int>();

		private UnityEvent<OrganicFood, int> OnFoodRemoved { get; } = new UnityEvent<OrganicFood, int>();

		public bool ReadyForCooking
		{
			get
			{
				return _readyForCooking;
			}
			set
			{
				Network_readyForCooking = value;
			}
		}

		private SnappingPlane[] SnappingPlanes => _snappingPlanesManager.SnappingPlanes;

		public bool Network_readyForCooking
		{
			get
			{
				return _readyForCooking;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref _readyForCooking, 512uL, _Mirror_SyncVarHookDelegate__readyForCooking);
			}
		}

		protected override void Awake()
		{
			base.Awake();
			_snappingPlanesManager = GetComponent<SnappingPlanesManager>();
			OnFoodAttached.AddListener(OnFoodAttachedActions);
			OnFoodRemoved.AddListener(OnFoodRemovedActions);
			CacheCookingParticles();
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			OnFoodAttached.RemoveListener(OnFoodAttachedActions);
			OnFoodRemoved.RemoveListener(OnFoodRemovedActions);
			_cookingParticleFadeTween.Stop();
		}

		private void CacheCookingParticles()
		{
			if (cookingPanSurfaceParticle == null)
			{
				return;
			}
			_cookingParticles = cookingPanSurfaceParticle.GetComponentsInChildren<ParticleSystem>(includeInactive: true);
			if (_cookingParticles.Length != 0)
			{
				_cookingParticleRenderers = new ParticleSystemRenderer[_cookingParticles.Length];
				_cookingParticleOriginalRateMultipliers = new float[_cookingParticles.Length];
				_cookingParticleOriginalStartColors = new Color[_cookingParticles.Length];
				for (int i = 0; i < _cookingParticles.Length; i++)
				{
					ParticleSystem particleSystem = _cookingParticles[i];
					_cookingParticleRenderers[i] = particleSystem.GetComponent<ParticleSystemRenderer>();
					_cookingParticleOriginalRateMultipliers[i] = particleSystem.emission.rateOverTimeMultiplier;
					_cookingParticleOriginalStartColors[i] = particleSystem.main.startColor.color;
				}
				_cookingParticlePropertyBlock = new MaterialPropertyBlock();
				_cookingParticleCurrentT = (cookingPanSurfaceParticle.activeSelf ? 1f : 0f);
			}
		}

		public override void OnStartServer()
		{
			base.OnStartServer();
			StartCoroutine(InitPanCo());
		}

		private IEnumerator InitPanCo()
		{
			yield return new WaitForSeconds(0.5f);
			RegisterSnappingPlanesEvents();
		}

		public override void OnStartClient()
		{
			base.OnStartClient();
			RegisterSnappingPlanesEvents();
		}

		private void RegisterSnappingPlanesEvents()
		{
			SnappingPlane[] snappingPlanes = SnappingPlanes;
			foreach (SnappingPlane plane in snappingPlanes)
			{
				plane.OnObjectPlaced.AddListener(delegate(GameObject snappedObject)
				{
					if (snappedObject.TryGetComponent<OrganicFood>(out var component))
					{
						int snappingPlaneIndex = plane.SnappingPlaneIndex;
						OnFoodAttached.Invoke(component, snappingPlaneIndex);
					}
				});
				plane.OnObjectRemoved.AddListener(delegate(GameObject snappedObject)
				{
					if (snappedObject.TryGetComponent<OrganicFood>(out var component))
					{
						int snappingPlaneIndex = plane.SnappingPlaneIndex;
						OnFoodRemoved.Invoke(component, snappingPlaneIndex);
					}
				});
			}
		}

		private void OnFoodAttachedActions(OrganicFood food, int planeIndex)
		{
			if (food == null)
			{
				EvilLogger.LogError("Food is null in OnFoodAttachedActions", "OnFoodAttachedActions", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\Cooking\\Scripts\\Pan.cs", 145);
				return;
			}
			NetworkIdentity component = food.GetComponent<NetworkIdentity>();
			if (component == null)
			{
				EvilLogger.LogError("NetworkIdentity component missing on food", "OnFoodAttachedActions", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\Cooking\\Scripts\\Pan.cs", 152);
				return;
			}
			CmdAttachFood(component.netId, planeIndex);
			ObjectivesEventBus.Raise(ObjectiveSignal.PanFoodAttached, food);
			if (_readyForCooking && food.IsCookable)
			{
				food.StartCooking();
			}
		}

		[Command(requiresAuthority = false)]
		private void CmdAttachFood(uint foodNetId, int planeIndex)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteVarUInt(foodNetId);
			writer.WriteVarInt(planeIndex);
			SendCommandInternal("System.Void NomadDrive.Features.Cooking.Pan::CmdAttachFood(System.UInt32,System.Int32)", 1303153652, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		private void OnFoodRemovedActions(OrganicFood food, int planeIndex)
		{
			if (food == null)
			{
				EvilLogger.LogError("Food is null in OnFoodRemovedActions", "OnFoodRemovedActions", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\Cooking\\Scripts\\Pan.cs", 191);
				return;
			}
			CmdRemoveFood(planeIndex);
			if (food.IsCookingProcessActive)
			{
				food.StopCooking();
			}
		}

		[Command(requiresAuthority = false)]
		private void CmdRemoveFood(int planeIndex, NetworkConnectionToClient sender = null)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteVarInt(planeIndex);
			SendCommandInternal("System.Void NomadDrive.Features.Cooking.Pan::CmdRemoveFood(System.Int32,Mirror.NetworkConnectionToClient)", 1593698770, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		public override void OnEquip()
		{
			base.OnEquip();
			if (ReadyForCooking)
			{
				StopCookingProcess();
			}
		}

		public void StartCookingProcess()
		{
			CmdStartCookingProcess();
		}

		[Command(requiresAuthority = false)]
		private void CmdStartCookingProcess()
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			SendCommandInternal("System.Void NomadDrive.Features.Cooking.Pan::CmdStartCookingProcess()", 748985711, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		public void StopCookingProcess()
		{
			CmdStopCookingProcess();
		}

		[Command(requiresAuthority = false)]
		private void CmdStopCookingProcess()
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			SendCommandInternal("System.Void NomadDrive.Features.Cooking.Pan::CmdStopCookingProcess()", -2082215011, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		protected override void SetForLateJoiner()
		{
			base.SetForLateJoiner();
			TransitionCookingParticles(_readyForCooking, instant: true);
		}

		private void TransitionCookingParticles(bool show, bool instant)
		{
			if (cookingPanSurfaceParticle == null || _cookingParticles == null || _cookingParticles.Length == 0)
			{
				return;
			}
			_cookingParticleFadeTween.Stop();
			if (instant)
			{
				ParticleSystem[] cookingParticles;
				if (show)
				{
					cookingPanSurfaceParticle.SetActive(value: true);
					ApplyCookingParticleFade(1f);
					cookingParticles = _cookingParticles;
					foreach (ParticleSystem particleSystem in cookingParticles)
					{
						if (particleSystem != null)
						{
							particleSystem.Play(withChildren: false);
						}
					}
					return;
				}
				ApplyCookingParticleFade(0f);
				cookingParticles = _cookingParticles;
				foreach (ParticleSystem particleSystem2 in cookingParticles)
				{
					if (particleSystem2 != null)
					{
						particleSystem2.Clear(withChildren: true);
					}
				}
				cookingPanSurfaceParticle.SetActive(value: false);
				return;
			}
			if (show)
			{
				if (!cookingPanSurfaceParticle.activeSelf)
				{
					ApplyCookingParticleFade(0f);
					cookingPanSurfaceParticle.SetActive(value: true);
					ParticleSystem[] cookingParticles = _cookingParticles;
					foreach (ParticleSystem particleSystem3 in cookingParticles)
					{
						if (particleSystem3 != null)
						{
							particleSystem3.Play(withChildren: true);
						}
					}
				}
				_cookingParticleFadeTween = Tween.Custom(this, _cookingParticleCurrentT, 1f, cookingParticleFadeDuration, delegate(Pan pan, float val)
				{
					pan.ApplyCookingParticleFade(val);
				}, cookingParticleFadeInEase);
				return;
			}
			_cookingParticleFadeTween = Tween.Custom(this, _cookingParticleCurrentT, 0f, cookingParticleFadeDuration, delegate(Pan pan, float val)
			{
				pan.ApplyCookingParticleFade(val);
			}, cookingParticleFadeOutEase).OnComplete(this, delegate(Pan pan)
			{
				if (pan._cookingParticles != null)
				{
					ParticleSystem[] cookingParticles2 = pan._cookingParticles;
					foreach (ParticleSystem particleSystem4 in cookingParticles2)
					{
						if (particleSystem4 != null)
						{
							particleSystem4.Clear(withChildren: true);
						}
					}
				}
				if (pan.cookingPanSurfaceParticle != null)
				{
					pan.cookingPanSurfaceParticle.SetActive(value: false);
				}
			});
		}

		private void ApplyCookingParticleFade(float t)
		{
			if (_cookingParticles == null || _cookingParticles.Length == 0)
			{
				return;
			}
			_cookingParticleCurrentT = t;
			for (int i = 0; i < _cookingParticles.Length; i++)
			{
				ParticleSystem particleSystem = _cookingParticles[i];
				if (!(particleSystem == null))
				{
					ParticleSystem.EmissionModule emission = particleSystem.emission;
					emission.rateOverTimeMultiplier = _cookingParticleOriginalRateMultipliers[i] * t;
					Color color = _cookingParticleOriginalStartColors[i];
					color.a = _cookingParticleOriginalStartColors[i].a * t;
					ParticleSystem.MainModule main = particleSystem.main;
					main.startColor = color;
					ParticleSystemRenderer particleSystemRenderer = _cookingParticleRenderers[i];
					if (!(particleSystemRenderer == null))
					{
						particleSystemRenderer.GetPropertyBlock(_cookingParticlePropertyBlock);
						_cookingParticlePropertyBlock.SetColor(CookingParticleBaseColorId, color);
						_cookingParticlePropertyBlock.SetColor(CookingParticleTintColorId, color);
						particleSystemRenderer.SetPropertyBlock(_cookingParticlePropertyBlock);
					}
				}
			}
		}

		private void OnReadyForCookingConditionChange(bool _, bool newValue)
		{
			if (!IsLateJoinCompleted)
			{
				return;
			}
			TransitionCookingParticles(newValue, instant: false);
			SnappingPlane[] snappingPlanes = SnappingPlanes;
			foreach (SnappingPlane snappingPlane in snappingPlanes)
			{
				if (!snappingPlane.IsAnyObjectPlaced || !networkManager.TryGetNetworkObjectById(snappingPlane.SingleSlotPlacedEntityNetworkID, out var networkObject))
				{
					continue;
				}
				OrganicFood component = networkObject.GetComponent<OrganicFood>();
				if (!(component == null))
				{
					if (!newValue && component.IsCookingProcessActive)
					{
						component.StopCooking();
					}
					else if (newValue && !component.IsCookingProcessActive && component.IsCookable)
					{
						component.StartCooking();
					}
				}
			}
		}

		public Pan()
		{
			_Mirror_SyncVarHookDelegate__readyForCooking = OnReadyForCookingConditionChange;
		}

		static Pan()
		{
			CookingParticleBaseColorId = Shader.PropertyToID("_BaseColor");
			CookingParticleTintColorId = Shader.PropertyToID("_TintColor");
			RemoteProcedureCalls.RegisterCommand(typeof(Pan), "System.Void NomadDrive.Features.Cooking.Pan::CmdAttachFood(System.UInt32,System.Int32)", InvokeUserCode_CmdAttachFood__UInt32__Int32, requiresAuthority: false);
			RemoteProcedureCalls.RegisterCommand(typeof(Pan), "System.Void NomadDrive.Features.Cooking.Pan::CmdRemoveFood(System.Int32,Mirror.NetworkConnectionToClient)", InvokeUserCode_CmdRemoveFood__Int32__NetworkConnectionToClient, requiresAuthority: false);
			RemoteProcedureCalls.RegisterCommand(typeof(Pan), "System.Void NomadDrive.Features.Cooking.Pan::CmdStartCookingProcess()", InvokeUserCode_CmdStartCookingProcess, requiresAuthority: false);
			RemoteProcedureCalls.RegisterCommand(typeof(Pan), "System.Void NomadDrive.Features.Cooking.Pan::CmdStopCookingProcess()", InvokeUserCode_CmdStopCookingProcess, requiresAuthority: false);
		}

		public override bool Weaved()
		{
			return true;
		}

		protected void UserCode_CmdAttachFood__UInt32__Int32(uint foodNetId, int planeIndex)
		{
			if (!networkManager.TryGetNetworkObjectById(foodNetId, out var networkObject))
			{
				EvilLogger.LogError($"Food with netId {foodNetId} not found on server", "CmdAttachFood", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\Cooking\\Scripts\\Pan.cs", 171);
				return;
			}
			NetworkIdentity component = networkObject.GetComponent<NetworkIdentity>();
			if (networkObject.GetComponent<OrganicFood>() == null)
			{
				EvilLogger.LogError($"OrganicFood component not found on netId {foodNetId}", "CmdAttachFood", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\Cooking\\Scripts\\Pan.cs", 179);
				return;
			}
			component.RemoveClientAuthority();
			component.AssignClientAuthority(NetworkServer.connections[0]);
		}

		protected static void InvokeUserCode_CmdAttachFood__UInt32__Int32(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdAttachFood called on client.");
			}
			else
			{
				((Pan)obj).UserCode_CmdAttachFood__UInt32__Int32(reader.ReadVarUInt(), reader.ReadVarInt());
			}
		}

		protected void UserCode_CmdRemoveFood__Int32__NetworkConnectionToClient(int planeIndex, NetworkConnectionToClient sender)
		{
			SnappingPlane snappingPlane = SnappingPlanes[planeIndex];
			if (snappingPlane == null)
			{
				EvilLogger.LogError("SnappingPlane is null in CmdRemoveFood", "CmdRemoveFood", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\Cooking\\Scripts\\Pan.cs", 210);
			}
			else if (snappingPlane.IsAnyObjectPlaced && snappingPlane.IsSingleUseSlot)
			{
				if (!networkManager.TryGetNetworkObjectById(snappingPlane.SingleSlotPlacedEntityNetworkID, out var networkObject))
				{
					EvilLogger.LogError("NetworkIdentity is null in CmdRemoveFood", "CmdRemoveFood", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\Cooking\\Scripts\\Pan.cs", 217);
					return;
				}
				NetworkIdentity component = networkObject.GetComponent<NetworkIdentity>();
				component.RemoveClientAuthority();
				component.AssignClientAuthority(sender);
			}
		}

		protected static void InvokeUserCode_CmdRemoveFood__Int32__NetworkConnectionToClient(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdRemoveFood called on client.");
			}
			else
			{
				((Pan)obj).UserCode_CmdRemoveFood__Int32__NetworkConnectionToClient(reader.ReadVarInt(), senderConnection);
			}
		}

		protected void UserCode_CmdStartCookingProcess()
		{
			ReadyForCooking = true;
		}

		protected static void InvokeUserCode_CmdStartCookingProcess(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdStartCookingProcess called on client.");
			}
			else
			{
				((Pan)obj).UserCode_CmdStartCookingProcess();
			}
		}

		protected void UserCode_CmdStopCookingProcess()
		{
			ReadyForCooking = false;
		}

		protected static void InvokeUserCode_CmdStopCookingProcess(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdStopCookingProcess called on client.");
			}
			else
			{
				((Pan)obj).UserCode_CmdStopCookingProcess();
			}
		}

		public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
		{
			base.SerializeSyncVars(writer, forceAll);
			if (forceAll)
			{
				writer.WriteBool(_readyForCooking);
				return;
			}
			writer.WriteVarULong(syncVarDirtyBits);
			if ((syncVarDirtyBits & 0x200L) != 0L)
			{
				writer.WriteBool(_readyForCooking);
			}
		}

		public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
		{
			base.DeserializeSyncVars(reader, initialState);
			if (initialState)
			{
				GeneratedSyncVarDeserialize(ref _readyForCooking, _Mirror_SyncVarHookDelegate__readyForCooking, reader.ReadBool());
				return;
			}
			long num = (long)reader.ReadVarULong();
			if ((num & 0x200L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref _readyForCooking, _Mirror_SyncVarHookDelegate__readyForCooking, reader.ReadBool());
			}
		}
	}
}
