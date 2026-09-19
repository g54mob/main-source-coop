using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

namespace Mirror.Examples.CouchCoop
{
	public class CouchPlayer : NetworkBehaviour
	{
		public Rigidbody rb;

		public float movementSpeed = 3f;

		public float jumpSpeed = 6f;

		private float movementVelocity;

		private bool isGrounded;

		public CouchPlayerManager couchPlayerManager;

		private KeyCode jumpKey = KeyCode.Space;

		private KeyCode leftKey = KeyCode.LeftArrow;

		private KeyCode rightKey = KeyCode.RightArrow;

		[SyncVar(hook = "OnNumberChangedHook")]
		public int playerNumber;

		public Text textPlayerNumber;

		public static readonly List<GameObject> playersList = new List<GameObject>();

		public Action<int, int> _Mirror_SyncVarHookDelegate_playerNumber;

		public int NetworkplayerNumber
		{
			get
			{
				return playerNumber;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref playerNumber, 1uL, _Mirror_SyncVarHookDelegate_playerNumber);
			}
		}

		public void Start()
		{
			playersList.Add(base.gameObject);
			SetPlayerUI();
		}

		public void OnDestroy()
		{
			playersList.Remove(base.gameObject);
		}

		public override void OnStartAuthority()
		{
			base.enabled = true;
			if (base.isOwned)
			{
				couchPlayerManager = UnityEngine.Object.FindAnyObjectByType<CouchPlayerManager>();
				jumpKey = couchPlayerManager.playerKeyJump[playerNumber];
				leftKey = couchPlayerManager.playerKeyLeft[playerNumber];
				rightKey = couchPlayerManager.playerKeyRight[playerNumber];
			}
		}

		private void Update()
		{
			if (Application.isFocused && base.isOwned)
			{
				if (isGrounded && (Input.GetKey(KeyCode.Space) || Input.GetKeyDown(jumpKey)))
				{
					rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpSpeed);
				}
				movementVelocity = 0f;
				if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(leftKey))
				{
					movementVelocity = 0f - movementSpeed;
				}
				if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(rightKey))
				{
					movementVelocity = movementSpeed;
				}
				rb.linearVelocity = new Vector2(movementVelocity, rb.linearVelocity.y);
			}
		}

		[ClientCallback]
		private void OnCollisionExit(Collision col)
		{
			if (NetworkClient.active && base.isOwned)
			{
				isGrounded = false;
			}
		}

		[ClientCallback]
		private void OnCollisionStay(Collision col)
		{
			if (NetworkClient.active && base.isOwned)
			{
				isGrounded = true;
			}
		}

		private void OnNumberChangedHook(int _old, int _new)
		{
			SetPlayerUI();
		}

		public void SetPlayerUI()
		{
			if (base.isOwned)
			{
				textPlayerNumber.text = "Local: " + playerNumber;
			}
			else
			{
				textPlayerNumber.text = "Remote: " + playerNumber;
			}
		}

		public CouchPlayer()
		{
			_Mirror_SyncVarHookDelegate_playerNumber = OnNumberChangedHook;
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
				writer.WriteVarInt(playerNumber);
				return;
			}
			writer.WriteVarULong(syncVarDirtyBits);
			if ((syncVarDirtyBits & 1L) != 0L)
			{
				writer.WriteVarInt(playerNumber);
			}
		}

		public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
		{
			base.DeserializeSyncVars(reader, initialState);
			if (initialState)
			{
				GeneratedSyncVarDeserialize(ref playerNumber, _Mirror_SyncVarHookDelegate_playerNumber, reader.ReadVarInt());
				return;
			}
			long num = (long)reader.ReadVarULong();
			if ((num & 1L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref playerNumber, _Mirror_SyncVarHookDelegate_playerNumber, reader.ReadVarInt());
			}
		}
	}
}
