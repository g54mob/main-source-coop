using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using Mirror.Examples.Common.Controllers.Player;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Mirror.Examples.AdditiveLevels
{
	public class Portal : NetworkBehaviour
	{
		[CompilerGenerated]
		private sealed class _003CSendPlayerToNewScene_003Ed__8 : IEnumerator<object>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private object _003C_003E2__current;

			public GameObject player;

			public Portal _003C_003E4__this;

			private NetworkConnectionToClient _003Cconn_003E5__2;

			object IEnumerator<object>.Current
			{
				[DebuggerHidden]
				get
				{
					return _003C_003E2__current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return _003C_003E2__current;
				}
			}

			[DebuggerHidden]
			public _003CSendPlayerToNewScene_003Ed__8(int _003C_003E1__state)
			{
				this._003C_003E1__state = _003C_003E1__state;
			}

			[DebuggerHidden]
			void IDisposable.Dispose()
			{
			}

			private bool MoveNext()
			{
				int num = _003C_003E1__state;
				Portal portal = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
				{
					_003C_003E1__state = -1;
					if (!player.TryGetComponent<NetworkIdentity>(out var component2))
					{
						return false;
					}
					_003Cconn_003E5__2 = component2.connectionToClient;
					if (_003Cconn_003E5__2 == null)
					{
						return false;
					}
					_003Cconn_003E5__2.Send(new SceneMessage
					{
						sceneName = portal.gameObject.scene.path,
						sceneOperation = SceneOperation.UnloadAdditive,
						customHandling = true
					});
					_003C_003E2__current = new WaitForSeconds(AdditiveLevelsNetworkManager.singleton.fadeInOut.GetFadeInTime());
					_003C_003E1__state = 1;
					return true;
				}
				case 1:
					_003C_003E1__state = -1;
					NetworkServer.RemovePlayerForConnection(_003Cconn_003E5__2, RemovePlayerOptions.Unspawn);
					_003C_003E2__current = null;
					_003C_003E1__state = 2;
					return true;
				case 2:
				{
					_003C_003E1__state = -1;
					player.transform.position = portal.startPosition;
					player.transform.LookAt(Vector3.up);
					SceneManager.MoveGameObjectToScene(player, SceneManager.GetSceneByPath(portal.destinationScene));
					_003Cconn_003E5__2.Send(new SceneMessage
					{
						sceneName = portal.destinationScene,
						sceneOperation = SceneOperation.LoadAdditive,
						customHandling = true
					});
					NetworkServer.AddPlayerForConnection(_003Cconn_003E5__2, player);
					if (NetworkClient.localPlayer != null && NetworkClient.localPlayer.TryGetComponent<PlayerControllerBase>(out var component))
					{
						component.enabled = true;
					}
					return false;
				}
				}
			}

			bool IEnumerator.MoveNext()
			{
				//ILSpy generated this explicit interface implementation from .override directive in MoveNext
				return this.MoveNext();
			}

			[DebuggerHidden]
			void IEnumerator.Reset()
			{
				throw new NotSupportedException();
			}
		}

		[Scene]
		[Tooltip("Which scene to send player from here")]
		public string destinationScene;

		[Tooltip("Where to spawn player in Destination Scene")]
		public Vector3 startPosition;

		[Tooltip("Reference to child TextMesh label")]
		public TextMesh label;

		[SyncVar(hook = "OnLabelTextChanged")]
		public string labelText;

		public Action<string, string> _Mirror_SyncVarHookDelegate_labelText;

		public string NetworklabelText
		{
			get
			{
				return labelText;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref labelText, 1uL, _Mirror_SyncVarHookDelegate_labelText);
			}
		}

		public void OnLabelTextChanged(string _, string newValue)
		{
			label.text = labelText;
		}

		public override void OnStartServer()
		{
			NetworklabelText = Path.GetFileNameWithoutExtension(destinationScene).Replace("MirrorAdditiveLevels", "");
			NetworklabelText = Regex.Replace(labelText, "\\B[A-Z0-9]+", " $0");
		}

		public override void OnStartClient()
		{
			if (label.TryGetComponent<LookAtMainCamera>(out var component))
			{
				component.enabled = true;
			}
		}

		private void OnTriggerEnter(Collider other)
		{
			if (other is CapsuleCollider && other.CompareTag("Player"))
			{
				if (other.TryGetComponent<PlayerControllerBase>(out var component))
				{
					component.enabled = false;
				}
				if (base.isServer)
				{
					StartCoroutine(SendPlayerToNewScene(other.gameObject));
				}
			}
		}

		[IteratorStateMachine(typeof(_003CSendPlayerToNewScene_003Ed__8))]
		[ServerCallback]
		private IEnumerator SendPlayerToNewScene(GameObject player)
		{
			if (!NetworkServer.active)
			{
				return null;
			}
			return new _003CSendPlayerToNewScene_003Ed__8(0)
			{
				_003C_003E4__this = this,
				player = player
			};
		}

		public Portal()
		{
			_Mirror_SyncVarHookDelegate_labelText = OnLabelTextChanged;
		}

		public override bool Weaved()
		{
			return true;
		}

		public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
		{
			base.SerializeSyncVars(writer, forceAll);
			if (forceAll)
			{
				writer.WriteString(labelText);
				return;
			}
			writer.WriteVarULong(syncVarDirtyBits);
			if ((syncVarDirtyBits & 1L) != 0L)
			{
				writer.WriteString(labelText);
			}
		}

		public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
		{
			base.DeserializeSyncVars(reader, initialState);
			if (initialState)
			{
				GeneratedSyncVarDeserialize(ref labelText, _Mirror_SyncVarHookDelegate_labelText, reader.ReadString());
				return;
			}
			long num = (long)reader.ReadVarULong();
			if ((num & 1L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref labelText, _Mirror_SyncVarHookDelegate_labelText, reader.ReadString());
			}
		}
	}
}
