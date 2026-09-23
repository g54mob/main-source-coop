using System;
using Mimicraft.Networking;
using Mimicraft.Settings;
using Unity.Netcode;
using UnityEngine;

namespace Mimicraft.Gameplay
{
	[RequireComponent(typeof(NetworkObject))]
	public class PlayerCompanion : NetworkBehaviour
	{
		private enum WalkingParameter
		{
			Unknown = 0,
			Present = 1,
			Missing = 2
		}

		[Tooltip("The dog model, as a child of the player object. Switched on while this player is a companion and off the rest of the time - so it should be left DISABLED in the prefab.")]
		[SerializeField]
		private GameObject dogRoot;

		[Tooltip("The dog's Animator. Only one parameter is driven: a bool named below.")]
		[SerializeField]
		private Animator dogAnimator;

		[Tooltip("Name of the Animator bool set while the dog is moving.")]
		[SerializeField]
		private string walkingParameter = "Walking";

		[Tooltip("Where a bark comes out of. On the dog rather than on the player root so it falls off with distance from the animal people can actually see.")]
		[SerializeField]
		private AudioSource barkSource;

		[Tooltip("Barks. One is picked at random per bark, so a dog held down on the key does not machine-gun the same sample.")]
		[SerializeField]
		private AudioClip[] barkClips;

		private const float WalkingSpeedThreshold = 0.35f;

		private const float BarkCooldownSeconds = 0.6f;

		private readonly NetworkVariable<bool> isCompanion = new NetworkVariable<bool>(value: false);

		private PlayerVoxelBody voxelBody;

		private PlayerMovement movement;

		private Vector3 lastPosition;

		private bool hasLastPosition;

		private double nextBarkAllowedTime;

		private WalkingParameter walkingParameterState;

		private double serverNextBarkAllowedTime;

		public bool IsCompanion => isCompanion.Value;

		public override void OnNetworkSpawn()
		{
			voxelBody = GetComponent<PlayerVoxelBody>();
			movement = GetComponent<PlayerMovement>();
			NetworkVariable<bool> networkVariable = isCompanion;
			networkVariable.OnValueChanged = (NetworkVariable<bool>.OnValueChangedDelegate)Delegate.Combine(networkVariable.OnValueChanged, new NetworkVariable<bool>.OnValueChangedDelegate(OnCompanionChanged));
			Apply(isCompanion.Value);
		}

		public override void OnNetworkDespawn()
		{
			NetworkVariable<bool> networkVariable = isCompanion;
			networkVariable.OnValueChanged = (NetworkVariable<bool>.OnValueChangedDelegate)Delegate.Remove(networkVariable.OnValueChanged, new NetworkVariable<bool>.OnValueChangedDelegate(OnCompanionChanged));
		}

		public void ServerSetCompanion(bool value)
		{
			if (base.IsServer && isCompanion.Value != value)
			{
				isCompanion.Value = value;
			}
		}

		private void OnCompanionChanged(bool previous, bool current)
		{
			Apply(current);
		}

		private void Apply(bool companion)
		{
			if (dogRoot != null && dogRoot.activeSelf != companion)
			{
				dogRoot.SetActive(companion);
			}
			if (voxelBody != null)
			{
				voxelBody.SetCompanionMode(companion);
			}
			hasLastPosition = false;
			if (movement != null)
			{
				movement.IsCompanion = companion;
				if (companion && base.IsOwner)
				{
					movement.SpectatorFrozen = false;
				}
			}
		}

		private void Update()
		{
			if (isCompanion.Value)
			{
				DriveWalkingAnimation();
				if (base.IsOwner && GameInput.Companion.WasPressedThisFrame() && !GameMenuState.InputCaptured)
				{
					RequestBark();
				}
			}
		}

		private void DriveWalkingAnimation()
		{
			if (!(dogAnimator == null) && HasWalkingParameter())
			{
				Vector3 position = base.transform.position;
				if (!hasLastPosition)
				{
					lastPosition = position;
					hasLastPosition = true;
					return;
				}
				Vector3 vector = position - lastPosition;
				lastPosition = position;
				vector.y = 0f;
				float num = ((Time.deltaTime > 0f) ? (vector.magnitude / Time.deltaTime) : 0f);
				dogAnimator.SetBool(walkingParameter, num > 0.35f);
			}
		}

		private bool HasWalkingParameter()
		{
			if (walkingParameterState != WalkingParameter.Unknown)
			{
				return walkingParameterState == WalkingParameter.Present;
			}
			walkingParameterState = WalkingParameter.Missing;
			if (!string.IsNullOrEmpty(walkingParameter))
			{
				AnimatorControllerParameter[] parameters = dogAnimator.parameters;
				foreach (AnimatorControllerParameter animatorControllerParameter in parameters)
				{
					if (animatorControllerParameter.type == AnimatorControllerParameterType.Bool && !(animatorControllerParameter.name != walkingParameter))
					{
						walkingParameterState = WalkingParameter.Present;
						break;
					}
				}
			}
			if (walkingParameterState == WalkingParameter.Missing)
			{
				Debug.LogWarning("[PlayerCompanion] Animator'de '" + walkingParameter + "' adinda bir bool parametresi yok - kopek yurume animasyonu oynatilmayacak.");
			}
			return walkingParameterState == WalkingParameter.Present;
		}

		private void RequestBark()
		{
			if (!(Time.timeAsDouble < nextBarkAllowedTime))
			{
				nextBarkAllowedTime = Time.timeAsDouble + 0.6000000238418579;
				BarkServerRpc();
			}
		}

		[Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
		private void BarkServerRpc(RpcParams rpcParams = default(RpcParams))
		{
			NetworkManager networkManager = base.NetworkManager;
			if ((object)networkManager == null || !networkManager.IsListening)
			{
				Debug.LogError("Rpc methods can only be invoked after starting the NetworkManager!");
				return;
			}
			if (__rpc_exec_stage != __RpcExecStage.Execute)
			{
				RpcParams rpcParams2 = rpcParams;
				RpcAttribute.RpcAttributeParams attributeParams = new RpcAttribute.RpcAttributeParams
				{
					InvokePermission = RpcInvokePermission.Everyone
				};
				FastBufferWriter bufferWriter = __beginSendRpc(457282931u, rpcParams2, attributeParams, SendTo.Server, RpcDelivery.Reliable);
				__endSendRpc(ref bufferWriter, 457282931u, rpcParams, attributeParams, SendTo.Server, RpcDelivery.Reliable);
			}
			if (__rpc_exec_stage == __RpcExecStage.Execute)
			{
				__rpc_exec_stage = __RpcExecStage.Send;
				if (rpcParams.Receive.SenderClientId == base.OwnerClientId && isCompanion.Value && !(Time.timeAsDouble < serverNextBarkAllowedTime))
				{
					serverNextBarkAllowedTime = Time.timeAsDouble + 0.6000000238418579;
					BarkClientRpc(PickBarkIndex());
				}
			}
		}

		private int PickBarkIndex()
		{
			if (barkClips != null && barkClips.Length != 0)
			{
				return UnityEngine.Random.Range(0, barkClips.Length);
			}
			return -1;
		}

		[ClientRpc]
		private void BarkClientRpc(int clipIndex)
		{
			NetworkManager networkManager = base.NetworkManager;
			if ((object)networkManager == null || !networkManager.IsListening)
			{
				Debug.LogError("Rpc methods can only be invoked after starting the NetworkManager!");
				return;
			}
			if (__rpc_exec_stage != __RpcExecStage.Execute && (networkManager.IsServer || networkManager.IsHost))
			{
				ClientRpcParams clientRpcParams = default(ClientRpcParams);
				FastBufferWriter bufferWriter = __beginSendClientRpc(3003285744u, clientRpcParams, RpcDelivery.Reliable);
				BytePacker.WriteValueBitPacked(bufferWriter, clipIndex);
				__endSendClientRpc(ref bufferWriter, 3003285744u, clientRpcParams, RpcDelivery.Reliable);
			}
			if (__rpc_exec_stage != __RpcExecStage.Execute || (!networkManager.IsClient && !networkManager.IsHost))
			{
				return;
			}
			__rpc_exec_stage = __RpcExecStage.Send;
			if (!(barkSource == null) && barkClips != null && clipIndex >= 0 && clipIndex < barkClips.Length)
			{
				AudioClip audioClip = barkClips[clipIndex];
				if (audioClip != null)
				{
					barkSource.PlayOneShot(audioClip);
				}
			}
		}

		protected override void __initializeVariables()
		{
			if (isCompanion == null)
			{
				throw new Exception("PlayerCompanion.isCompanion cannot be null. All NetworkVariableBase instances must be initialized.");
			}
			isCompanion.Initialize(this);
			__nameNetworkVariable(isCompanion, "isCompanion");
			NetworkVariableFields.Add(isCompanion);
			base.__initializeVariables();
		}

		protected override void __initializeRpcs()
		{
			__registerRpc(457282931u, __rpc_handler_457282931, "BarkServerRpc", RpcInvokePermission.Everyone);
			__registerRpc(3003285744u, __rpc_handler_3003285744, "BarkClientRpc", RpcInvokePermission.Server);
			base.__initializeRpcs();
		}

		private static void __rpc_handler_457282931(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
		{
			NetworkManager networkManager = target.NetworkManager;
			if ((object)networkManager != null && networkManager.IsListening)
			{
				RpcParams ext = rpcParams.Ext;
				target.__rpc_exec_stage = __RpcExecStage.Execute;
				((PlayerCompanion)target).BarkServerRpc(ext);
				target.__rpc_exec_stage = __RpcExecStage.Send;
			}
		}

		private static void __rpc_handler_3003285744(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
		{
			NetworkManager networkManager = target.NetworkManager;
			if ((object)networkManager != null && networkManager.IsListening)
			{
				ByteUnpacker.ReadValueBitPacked(reader, out int value);
				target.__rpc_exec_stage = __RpcExecStage.Execute;
				((PlayerCompanion)target).BarkClientRpc(value);
				target.__rpc_exec_stage = __RpcExecStage.Send;
			}
		}

		protected internal override string __getTypeName()
		{
			return "PlayerCompanion";
		}
	}
}
