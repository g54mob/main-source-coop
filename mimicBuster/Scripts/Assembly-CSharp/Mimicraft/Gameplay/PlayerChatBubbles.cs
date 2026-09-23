using System.Collections.Generic;
using Mimicraft.Localization;
using Mimicraft.Networking;
using Mimicraft.UI;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

namespace Mimicraft.Gameplay
{
	[RequireComponent(typeof(NetworkObject))]
	public class PlayerChatBubbles : NetworkBehaviour
	{
		private class Bubble
		{
			public TextMeshPro Label;

			public float ExpiresAt;

			public float TargetHeight;

			public float TargetScale;

			public float Height;

			public float Scale;

			public Color Tint;
		}

		private const int MaxBubbles = 3;

		private const float LifetimeSeconds = 7f;

		private const float FadeSeconds = 0.8f;

		private const float GapAboveNameTag = 0.3f;

		private const float LinePitch = 0.34f;

		private const float FontSize = 2.4f;

		private const float LineWidth = 4.5f;

		private const float MaxVisibleDistance = 25f;

		private static readonly float[] DepthScale = new float[3] { 1f, 0.8f, 0.64f };

		private const float SlideSpeed = 9f;

		[Tooltip("Doğru bilinen Gartic tahmininin rengi.")]
		[SerializeField]
		private Color correctColor = new Color(0.42f, 0.92f, 0.46f);

		[Tooltip("Yaklaşan tahminin rengi. Yalnızca tahmini yapan oyuncu görür.")]
		[SerializeField]
		private Color closeColor = new Color(0.95f, 0.79f, 0.35f);

		[Tooltip("Yanlış tahminin ve sıradan mesajların rengi.")]
		[SerializeField]
		private Color messageColor = Color.white;

		private readonly List<Bubble> live = new List<Bubble>();

		private readonly List<Bubble> spare = new List<Bubble>();

		private Transform root;

		private PlayerVoxelBody voxelBody;

		private bool resolved;

		public void ServerShow(string text, ChatBubbleKind kind)
		{
			if (base.IsServer)
			{
				ShowBubbleRpc(Clip(text), (byte)kind);
			}
		}

		public void ServerShowTo(ulong clientId, string text, ChatBubbleKind kind)
		{
			if (base.IsServer)
			{
				ShowBubbleRpc(Clip(text), (byte)kind, base.RpcTarget.Single(clientId, RpcTargetUse.Temp));
			}
		}

		public void ServerShowExcept(ulong clientId, string text, ChatBubbleKind kind)
		{
			if (base.IsServer)
			{
				ShowBubbleRpc(Clip(text), (byte)kind, base.RpcTarget.Not(clientId, RpcTargetUse.Temp));
			}
		}

		private static FixedString128Bytes Clip(string text)
		{
			FixedString128Bytes fs = default(FixedString128Bytes);
			FixedStringMethods.CopyFromTruncated(ref fs, text ?? "");
			return fs;
		}

		[Rpc(SendTo.ClientsAndHost, AllowTargetOverride = true)]
		private void ShowBubbleRpc(FixedString128Bytes text, byte kind, RpcParams rpcParams = default(RpcParams))
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
					AllowTargetOverride = true
				};
				FastBufferWriter bufferWriter = __beginSendRpc(2873990156u, rpcParams, attributeParams, SendTo.ClientsAndHost, RpcDelivery.Reliable);
				bufferWriter.WriteValueSafe(in text, default(FastBufferWriter.ForFixedStrings));
				bufferWriter.WriteValueSafe(in kind, default(FastBufferWriter.ForPrimitives));
				__endSendRpc(ref bufferWriter, 2873990156u, rpcParams, attributeParams, SendTo.ClientsAndHost, RpcDelivery.Reliable);
			}
			if (__rpc_exec_stage == __RpcExecStage.Execute)
			{
				__rpc_exec_stage = __RpcExecStage.Send;
				Push(text.ToString(), (ChatBubbleKind)kind);
			}
		}

		private void Push(string text, ChatBubbleKind kind)
		{
			string text2 = ChatFilter.Apply(kind switch
			{
				ChatBubbleKind.GarticCorrect => Loc.Get("Chat.Bubble.Correct"), 
				ChatBubbleKind.GarticClose => Loc.Get("Chat.Bubble.Close"), 
				ChatBubbleKind.GarticWrong => Loc.Format("Chat.Bubble.Wrong", text), 
				_ => text, 
			});
			if (!string.IsNullOrWhiteSpace(text2))
			{
				Color tint = kind switch
				{
					ChatBubbleKind.GarticCorrect => correctColor, 
					ChatBubbleKind.GarticClose => closeColor, 
					_ => messageColor, 
				};
				Bubble bubble = Take();
				bubble.Label.text = text2;
				bubble.Tint = tint;
				bubble.ExpiresAt = Time.unscaledTime + 7f;
				bubble.Height = 0f;
				bubble.Scale = DepthScale[0];
				live.Insert(0, bubble);
				while (live.Count > 3)
				{
					List<Bubble> list = live;
					Bubble bubble2 = list[list.Count - 1];
					live.RemoveAt(live.Count - 1);
					Release(bubble2);
				}
				Layout();
			}
		}

		private Bubble Take()
		{
			if (spare.Count > 0)
			{
				List<Bubble> list = spare;
				Bubble bubble = list[list.Count - 1];
				spare.RemoveAt(spare.Count - 1);
				bubble.Label.gameObject.SetActive(value: true);
				return bubble;
			}
			GameObject obj = new GameObject("ChatBubble");
			obj.transform.SetParent(Root(), worldPositionStays: false);
			TextMeshPro textMeshPro = obj.AddComponent<TextMeshPro>();
			textMeshPro.fontSize = 2.4f;
			textMeshPro.alignment = TextAlignmentOptions.Bottom;
			textMeshPro.textWrappingMode = TextWrappingModes.Normal;
			textMeshPro.rectTransform.sizeDelta = new Vector2(4.5f, 1f);
			textMeshPro.rectTransform.pivot = new Vector2(0.5f, 0f);
			return new Bubble
			{
				Label = textMeshPro
			};
		}

		private void Release(Bubble bubble)
		{
			bubble.Label.text = string.Empty;
			bubble.Label.gameObject.SetActive(value: false);
			spare.Add(bubble);
		}

		private Transform Root()
		{
			if (root != null)
			{
				return root;
			}
			GameObject gameObject = new GameObject("ChatBubbles");
			gameObject.transform.SetParent(base.transform, worldPositionStays: false);
			root = gameObject.transform;
			return root;
		}

		private void Layout()
		{
			float num = 0f;
			for (int i = 0; i < live.Count; i++)
			{
				float num2 = DepthScale[Mathf.Min(i, DepthScale.Length - 1)];
				live[i].TargetScale = num2;
				live[i].TargetHeight = num;
				num += 0.34f * num2;
			}
		}

		private void LateUpdate()
		{
			if (live.Count != 0)
			{
				Resolve();
				Camera main = Camera.main;
				bool visible = main != null && ShouldShow(main);
				Expire();
				Follow(main, visible);
			}
		}

		private void Resolve()
		{
			if (!resolved)
			{
				resolved = true;
				voxelBody = GetComponent<PlayerVoxelBody>();
			}
		}

		private bool ShouldShow(Camera cam)
		{
			if (voxelBody != null && voxelBody.HasVoxelBody)
			{
				return false;
			}
			return (cam.transform.position - base.transform.position).sqrMagnitude <= 625f;
		}

		private void Expire()
		{
			for (int num = live.Count - 1; num >= 0; num--)
			{
				if (!(Time.unscaledTime < live[num].ExpiresAt))
				{
					Release(live[num]);
					live.RemoveAt(num);
				}
			}
			if (live.Count > 0)
			{
				Layout();
			}
		}

		private void Follow(Camera cam, bool visible)
		{
			float num = 2.3999999f;
			foreach (Bubble item in live)
			{
				item.Label.gameObject.SetActive(visible);
				if (visible)
				{
					item.Height = Mathf.Lerp(item.Height, item.TargetHeight, 1f - Mathf.Exp(-9f * Time.unscaledDeltaTime));
					item.Scale = Mathf.Lerp(item.Scale, item.TargetScale, 1f - Mathf.Exp(-9f * Time.unscaledDeltaTime));
					Transform obj = item.Label.transform;
					obj.localPosition = Vector3.up * (num + item.Height);
					obj.localScale = Vector3.one * item.Scale;
					obj.rotation = Quaternion.LookRotation(obj.position - cam.transform.position);
					float a = Mathf.Clamp01((item.ExpiresAt - Time.unscaledTime) / 0.8f);
					item.Label.color = new Color(item.Tint.r, item.Tint.g, item.Tint.b, a);
				}
			}
		}

		public override void OnNetworkDespawn()
		{
			foreach (Bubble item in live)
			{
				if (item.Label != null)
				{
					Object.Destroy(item.Label.gameObject);
				}
			}
			foreach (Bubble item2 in spare)
			{
				if (item2.Label != null)
				{
					Object.Destroy(item2.Label.gameObject);
				}
			}
			live.Clear();
			spare.Clear();
		}

		protected override void __initializeVariables()
		{
			base.__initializeVariables();
		}

		protected override void __initializeRpcs()
		{
			__registerRpc(2873990156u, __rpc_handler_2873990156, "ShowBubbleRpc", RpcInvokePermission.Everyone);
			base.__initializeRpcs();
		}

		private static void __rpc_handler_2873990156(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
		{
			NetworkManager networkManager = target.NetworkManager;
			if ((object)networkManager != null && networkManager.IsListening)
			{
				reader.ReadValueSafe(out FixedString128Bytes value, default(FastBufferWriter.ForFixedStrings));
				reader.ReadValueSafe(out byte value2, default(FastBufferWriter.ForPrimitives));
				RpcParams ext = rpcParams.Ext;
				target.__rpc_exec_stage = __RpcExecStage.Execute;
				((PlayerChatBubbles)target).ShowBubbleRpc(value, value2, ext);
				target.__rpc_exec_stage = __RpcExecStage.Send;
			}
		}

		protected internal override string __getTypeName()
		{
			return "PlayerChatBubbles";
		}
	}
}
