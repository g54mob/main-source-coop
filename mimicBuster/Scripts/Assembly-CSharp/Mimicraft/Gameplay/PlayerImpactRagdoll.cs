using System.Collections.Generic;
using Mimicraft.Networking;
using Unity.Netcode;
using UnityEngine;

namespace Mimicraft.Gameplay
{
	[RequireComponent(typeof(CharacterController))]
	public class PlayerImpactRagdoll : NetworkBehaviour
	{
		[Header("Üstüne düşmek")]
		[Tooltip("Bu hızdan (m/sn, aşağı doğru) daha sert düşen bir oyuncu, üstüne düştüğü kişiyi de yere serer. İkisi de ragdoll olur.")]
		[SerializeField]
		[Min(1f)]
		private float slamSpeed = 7f;

		[Tooltip("Düşen oyuncunun kendi ragdoll'una uygulanan itmenin gücü.")]
		[SerializeField]
		[Min(0f)]
		private float slamImpulse = 3f;

		[Header("Modelci şarjı")]
		[Tooltip("Varsayılan boyuttaki (1x) bir Modelci'nin birini devirebilmesi için gereken yatay hız, m/sn. Modelci ne kadar büyükse bu eşik o kadar düşer.")]
		[SerializeField]
		[Min(0.5f)]
		private float chargeSpeed = 4.5f;

		[Tooltip("Boyutun eşiği ne kadar düşürebileceğinin sınırı. 0.35 = en büyük Modelci bile eşiğin %35'inin altına inemez, yani durur gibi yürüyerek kimseyi deviremez.")]
		[SerializeField]
		[Range(0.1f, 1f)]
		private float smallestChargeFraction = 0.35f;

		[Tooltip("Devrilen oyuncuya uygulanan itmenin gücü. Modelci'nin boyutuyla ölçeklenir.")]
		[SerializeField]
		[Min(0f)]
		private float chargeImpulse = 5f;

		[Header("Teşhis")]
		[Tooltip("Açıkken, hızlı hareket ederken YAKINDAKİ her oyuncu için hangi koşulun geçtiğini ve hangisinin reddettiğini konsola yazar. Bir çarpışmanın neden işlemediğini bulmanın yolu bu - çalışırken kapalı tut.")]
		[SerializeField]
		private bool logRefusals;

		private const float LandingHeightFraction = 0.5f;

		private const float OverlapSlack = 0.15f;

		private const float RequestIntervalSeconds = 0.5f;

		private static readonly List<PlayerImpactRagdoll> all = new List<PlayerImpactRagdoll>();

		private CharacterController controller;

		private PlayerRagdoll ragdoll;

		private PlayerVoxelBody voxelBody;

		private Vector3 lastPosition;

		private Vector3 velocity;

		private bool hasSample;

		private float nextRequestTime;

		private float nextExplainTime;

		private float recentFallSpeed;

		private float recentGroundSpeed;

		private const float PeakMemorySeconds = 0.5f;

		private const float ServerReachSlack = 1.5f;

		private Vector3 Body
		{
			get
			{
				if (!(ragdoll != null))
				{
					return base.transform.position;
				}
				return ragdoll.BodyPosition;
			}
		}

		private bool IsSlamming => recentFallSpeed >= slamSpeed;

		private bool IsCharging
		{
			get
			{
				if (voxelBody == null || !voxelBody.HasVoxelBody)
				{
					return false;
				}
				return recentGroundSpeed >= RequiredChargeSpeed;
			}
		}

		private float RequiredChargeSpeed
		{
			get
			{
				float num = ((voxelBody != null) ? Mathf.Max(voxelBody.BodySizeRatio, 0.0001f) : 1f);
				return chargeSpeed * Mathf.Max(1f / num, smallestChargeFraction);
			}
		}

		private bool CanBeHit
		{
			get
			{
				if (controller != null && controller.enabled && ragdoll != null && ragdoll.CanRagdoll)
				{
					return ragdoll.State == RagdollState.None;
				}
				return false;
			}
		}

		private bool CanBeHitSource
		{
			get
			{
				if (controller != null && ragdoll != null)
				{
					return ragdoll.State != RagdollState.Dead;
				}
				return false;
			}
		}

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetStatics()
		{
			all.Clear();
		}

		public override void OnNetworkSpawn()
		{
			controller = GetComponent<CharacterController>();
			ragdoll = GetComponent<PlayerRagdoll>();
			voxelBody = GetComponent<PlayerVoxelBody>();
			all.Add(this);
		}

		public override void OnNetworkDespawn()
		{
			all.Remove(this);
		}

		private void LateUpdate()
		{
			TrackVelocity();
			if (!base.IsOwner || all.Count < 2 || Time.time < nextRequestTime)
			{
				return;
			}
			bool isSlamming = IsSlamming;
			bool isCharging = IsCharging;
			if (!isSlamming && !isCharging)
			{
				Explain(null, $"hareket yetersiz - dusus {recentFallSpeed:0.0}/{slamSpeed:0.0}, " + $"yatay {recentGroundSpeed:0.0}/{RequiredChargeSpeed:0.0}" + ((voxelBody != null && voxelBody.HasVoxelBody) ? "" : " (voxel govde yok, sarj kapali)"));
				return;
			}
			foreach (PlayerImpactRagdoll item in all)
			{
				if (item == this || item == null)
				{
					continue;
				}
				if (!item.CanBeHit)
				{
					Explain(item, (item.ragdoll == null) ? "hedefte PlayerRagdoll yok" : ((!item.ragdoll.CanRagdoll) ? "hedefin insan modeli yok (voxel govde giymis olabilir)" : $"hedef zaten {item.ragdoll.State}"));
					continue;
				}
				if (isSlamming && LandedOn(item))
				{
					nextRequestTime = Time.time + 0.5f;
					ReportSlamServerRpc(item.NetworkObjectId);
					break;
				}
				if (isCharging && RanInto(item))
				{
					nextRequestTime = Time.time + 0.5f;
					ReportChargeServerRpc(item.NetworkObjectId);
					break;
				}
				Explain(item, (!HorizontallyOverlaps(item)) ? $"yatayda uzak ({(Body - item.Body).magnitude:0.00} m)" : (isSlamming ? "yatayda ustunde ama yukseklik penceresinde degil" : "carpisma yonu uymuyor"));
			}
		}

		private void Explain(PlayerImpactRagdoll other, string why)
		{
			if (logRefusals && !(Time.unscaledTime < nextExplainTime))
			{
				nextExplainTime = Time.unscaledTime + 1f;
				string arg = ((other != null) ? $"-> {other.OwnerClientId}" : "(hedef yok)");
				Debug.Log($"[Carpisma] {base.OwnerClientId} {arg}: {why}", this);
			}
		}

		private void TrackVelocity()
		{
			Vector3 body = Body;
			if (hasSample && Time.deltaTime > 0f)
			{
				velocity = (body - lastPosition) / Time.deltaTime;
			}
			lastPosition = body;
			hasSample = true;
			float num = Time.deltaTime / 0.5f;
			recentFallSpeed = Mathf.Max(0f - velocity.y, Mathf.MoveTowards(recentFallSpeed, 0f, slamSpeed * num));
			recentGroundSpeed = Mathf.Max(new Vector3(velocity.x, 0f, velocity.z).magnitude, Mathf.MoveTowards(recentGroundSpeed, 0f, chargeSpeed * num));
		}

		private bool LandedOn(PlayerImpactRagdoll other)
		{
			if (!HorizontallyOverlaps(other))
			{
				return false;
			}
			float y = Body.y;
			float y2 = other.Body.y;
			float num = other.controller.height * other.transform.lossyScale.y;
			if (y >= y2 + num * 0.5f)
			{
				return y <= y2 + num;
			}
			return false;
		}

		private bool RanInto(PlayerImpactRagdoll other)
		{
			if (!HorizontallyOverlaps(other) || !VerticallyOverlaps(other))
			{
				return false;
			}
			Vector3 vector = other.Body - Body;
			vector.y = 0f;
			return Vector3.Dot(new Vector3(velocity.x, 0f, velocity.z).normalized, vector.normalized) > 0.5f;
		}

		private bool HorizontallyOverlaps(PlayerImpactRagdoll other)
		{
			Vector3 vector = Body - other.Body;
			vector.y = 0f;
			return vector.magnitude <= Radius() + other.Radius() + 0.15f;
		}

		private bool VerticallyOverlaps(PlayerImpactRagdoll other)
		{
			float y = Body.y;
			float num = y + controller.height * base.transform.lossyScale.y;
			float y2 = other.Body.y;
			float num2 = y2 + other.controller.height * other.transform.lossyScale.y;
			if (y < num2)
			{
				return y2 < num;
			}
			return false;
		}

		private float Radius()
		{
			return controller.radius * Mathf.Max(base.transform.lossyScale.x, base.transform.lossyScale.z);
		}

		[Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
		private void ReportSlamServerRpc(ulong victimId, RpcParams rpcParams = default(RpcParams))
		{
			NetworkManager networkManager = base.NetworkManager;
			if ((object)networkManager == null || !networkManager.IsListening)
			{
				Debug.LogError("Rpc methods can only be invoked after starting the NetworkManager!");
				return;
			}
			if (__rpc_exec_stage != __RpcExecStage.Execute)
			{
				RpcAttribute.RpcAttributeParams attributeParams = new RpcAttribute.RpcAttributeParams
				{
					InvokePermission = RpcInvokePermission.Everyone
				};
				FastBufferWriter bufferWriter = __beginSendRpc(1068173015u, rpcParams, attributeParams, SendTo.Server, RpcDelivery.Reliable);
				BytePacker.WriteValueBitPacked(bufferWriter, victimId);
				__endSendRpc(ref bufferWriter, 1068173015u, rpcParams, attributeParams, SendTo.Server, RpcDelivery.Reliable);
			}
			if (__rpc_exec_stage != __RpcExecStage.Execute)
			{
				return;
			}
			__rpc_exec_stage = __RpcExecStage.Send;
			if (TryResolve(victimId, rpcParams, out var victim) && IsSlamming && ServerNear(victim))
			{
				Vector3 vector = victim.Body - Body;
				vector.y = 0f;
				if (vector.sqrMagnitude < 0.0001f)
				{
					vector = victim.transform.forward;
				}
				ragdoll.ServerBeginKnockdown(Vector3.down * slamImpulse);
				victim.ragdoll.ServerBeginKnockdown(vector.normalized * slamImpulse + Vector3.up * 0.5f);
			}
		}

		[Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
		private void ReportChargeServerRpc(ulong victimId, RpcParams rpcParams = default(RpcParams))
		{
			NetworkManager networkManager = base.NetworkManager;
			if ((object)networkManager == null || !networkManager.IsListening)
			{
				Debug.LogError("Rpc methods can only be invoked after starting the NetworkManager!");
				return;
			}
			if (__rpc_exec_stage != __RpcExecStage.Execute)
			{
				RpcAttribute.RpcAttributeParams attributeParams = new RpcAttribute.RpcAttributeParams
				{
					InvokePermission = RpcInvokePermission.Everyone
				};
				FastBufferWriter bufferWriter = __beginSendRpc(1009033478u, rpcParams, attributeParams, SendTo.Server, RpcDelivery.Reliable);
				BytePacker.WriteValueBitPacked(bufferWriter, victimId);
				__endSendRpc(ref bufferWriter, 1009033478u, rpcParams, attributeParams, SendTo.Server, RpcDelivery.Reliable);
			}
			if (__rpc_exec_stage != __RpcExecStage.Execute)
			{
				return;
			}
			__rpc_exec_stage = __RpcExecStage.Send;
			if (!TryResolve(victimId, rpcParams, out var victim) || !IsCharging || !ServerNear(victim))
			{
				return;
			}
			float num = ((voxelBody != null) ? voxelBody.BodySizeRatio : 1f);
			Vector3 vector = new Vector3(velocity.x, 0f, velocity.z).normalized * (chargeImpulse * num) + Vector3.up * 1.5f;
			if (victim.ragdoll.ServerBeginShove(vector))
			{
				float num2 = victim.controller.height * victim.transform.lossyScale.y;
				Vector3 contact = Vector3.Lerp(Body, victim.Body, 0.5f) + Vector3.up * (num2 * 0.5f);
				Vector3 vector2 = Body - victim.Body;
				vector2.y = 0f;
				if (vector2.sqrMagnitude < 0.0001f)
				{
					vector2 = -vector;
				}
				ChargeImpactClientRpc(contact, vector2.normalized, num);
			}
		}

		[Rpc(SendTo.ClientsAndHost)]
		private void ChargeImpactClientRpc(Vector3 contact, Vector3 normal, float size)
		{
			NetworkManager networkManager = base.NetworkManager;
			if ((object)networkManager == null || !networkManager.IsListening)
			{
				Debug.LogError("Rpc methods can only be invoked after starting the NetworkManager!");
				return;
			}
			if (__rpc_exec_stage != __RpcExecStage.Execute)
			{
				RpcAttribute.RpcAttributeParams attributeParams = default(RpcAttribute.RpcAttributeParams);
				RpcParams rpcParams = default(RpcParams);
				FastBufferWriter bufferWriter = __beginSendRpc(2666009985u, rpcParams, attributeParams, SendTo.ClientsAndHost, RpcDelivery.Reliable);
				bufferWriter.WriteValueSafe(in contact);
				bufferWriter.WriteValueSafe(in normal);
				bufferWriter.WriteValueSafe(in size, default(FastBufferWriter.ForPrimitives));
				__endSendRpc(ref bufferWriter, 2666009985u, rpcParams, attributeParams, SendTo.ClientsAndHost, RpcDelivery.Reliable);
			}
			if (__rpc_exec_stage == __RpcExecStage.Execute)
			{
				__rpc_exec_stage = __RpcExecStage.Send;
				ImpactEffects.SpawnChargeImpactEffect(contact, normal, size);
			}
		}

		private bool ServerNear(PlayerImpactRagdoll victim)
		{
			Vector3 vector = Body - victim.Body;
			float num = Radius() + victim.Radius() + 1.5f;
			float num2 = Mathf.Max(controller.height, victim.controller.height) + 1.5f;
			if (new Vector2(vector.x, vector.z).magnitude <= num)
			{
				return Mathf.Abs(vector.y) <= num2;
			}
			return false;
		}

		private bool TryResolve(ulong victimId, RpcParams rpcParams, out PlayerImpactRagdoll victim)
		{
			victim = null;
			if (!base.IsServer || rpcParams.Receive.SenderClientId != base.OwnerClientId || !CanBeHitSource)
			{
				return false;
			}
			if (!base.NetworkManager.SpawnManager.SpawnedObjects.TryGetValue(victimId, out var value) || value == null)
			{
				return false;
			}
			victim = value.GetComponent<PlayerImpactRagdoll>();
			if (victim != null && victim != this)
			{
				return victim.CanBeHit;
			}
			return false;
		}

		protected override void __initializeVariables()
		{
			base.__initializeVariables();
		}

		protected override void __initializeRpcs()
		{
			__registerRpc(1068173015u, __rpc_handler_1068173015, "ReportSlamServerRpc", RpcInvokePermission.Everyone);
			__registerRpc(1009033478u, __rpc_handler_1009033478, "ReportChargeServerRpc", RpcInvokePermission.Everyone);
			__registerRpc(2666009985u, __rpc_handler_2666009985, "ChargeImpactClientRpc", RpcInvokePermission.Everyone);
			base.__initializeRpcs();
		}

		private static void __rpc_handler_1068173015(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
		{
			NetworkManager networkManager = target.NetworkManager;
			if ((object)networkManager != null && networkManager.IsListening)
			{
				ByteUnpacker.ReadValueBitPacked(reader, out ulong value);
				RpcParams ext = rpcParams.Ext;
				target.__rpc_exec_stage = __RpcExecStage.Execute;
				((PlayerImpactRagdoll)target).ReportSlamServerRpc(value, ext);
				target.__rpc_exec_stage = __RpcExecStage.Send;
			}
		}

		private static void __rpc_handler_1009033478(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
		{
			NetworkManager networkManager = target.NetworkManager;
			if ((object)networkManager != null && networkManager.IsListening)
			{
				ByteUnpacker.ReadValueBitPacked(reader, out ulong value);
				RpcParams ext = rpcParams.Ext;
				target.__rpc_exec_stage = __RpcExecStage.Execute;
				((PlayerImpactRagdoll)target).ReportChargeServerRpc(value, ext);
				target.__rpc_exec_stage = __RpcExecStage.Send;
			}
		}

		private static void __rpc_handler_2666009985(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
		{
			NetworkManager networkManager = target.NetworkManager;
			if ((object)networkManager != null && networkManager.IsListening)
			{
				reader.ReadValueSafe(out Vector3 value);
				reader.ReadValueSafe(out Vector3 value2);
				reader.ReadValueSafe(out float value3, default(FastBufferWriter.ForPrimitives));
				target.__rpc_exec_stage = __RpcExecStage.Execute;
				((PlayerImpactRagdoll)target).ChargeImpactClientRpc(value, value2, value3);
				target.__rpc_exec_stage = __RpcExecStage.Send;
			}
		}

		protected internal override string __getTypeName()
		{
			return "PlayerImpactRagdoll";
		}
	}
}
