using System;
using Photon.Pun;
using UnityEngine;

public class SimplePlayer : MonoBehaviour
{
	[Serializable]
	public class PlayerInput
	{
		public Vector2 movementInput;

		public Vector2 lookInput;

		public bool jumpWasPressed;

		public bool jumpIsPressed;

		public bool sprintIsPressed;

		public bool interactWasPressed;

		public bool interactIsPressed;

		internal void Update()
		{
			if (GlobalInputHandler.CanTakeInput())
			{
				lookInput.x = Input.GetAxis("Mouse X");
				lookInput.y = Input.GetAxis("Mouse Y");
			}
			movementInput *= 0f;
			if (Input.GetKey(KeyCode.A))
			{
				movementInput.x -= 1f;
			}
			if (Input.GetKey(KeyCode.D))
			{
				movementInput.x += 1f;
			}
			if (Input.GetKey(KeyCode.S))
			{
				movementInput.y -= 1f;
			}
			if (Input.GetKey(KeyCode.W))
			{
				movementInput.y += 1f;
			}
			movementInput.Normalize();
			jumpWasPressed = Input.GetKeyDown(KeyCode.Space);
			jumpIsPressed = Input.GetKey(KeyCode.Space);
			interactWasPressed = Input.GetKeyDown(KeyCode.E);
			interactIsPressed = Input.GetKey(KeyCode.E);
			sprintIsPressed = Input.GetKey(KeyCode.LeftShift);
		}
	}

	[Serializable]
	public class PlayerData
	{
		public Vector2 playerLookValues;

		public Vector3 playerLookForward;

		public Vector3 playerLookRight;

		public Vector3 playerLookUp;

		public bool isLocalPlayer;

		public Vector3 movementVector;

		public bool isGrounded;

		public float sinceGrounded;

		public float groundHeight;

		public Vector3 groundPos;

		public Vector3 groundNormal;

		public Vector3 relativeVel;
	}

	[Serializable]
	public class PlayerRefs
	{
		public SimplePlayerController controller;

		public Transform head;

		public Transform cameraPoint;

		public Transform modelRoot;

		public Rigidbody rig;

		public PhotonView view;
	}

	public static SimplePlayer localPlayer;

	public PlayerInput input;

	public PlayerData data;

	public PlayerRefs refs;

	private void Start()
	{
		refs.view = GetComponent<PhotonView>();
		refs.rig = GetComponent<Rigidbody>();
		refs.controller = GetComponent<SimplePlayerController>();
		refs.head = base.transform.Find("Head");
		refs.cameraPoint = refs.head.Find("CameraPoint");
		refs.modelRoot = base.transform.Find("Model");
		refs.controller.Start();
		if (refs.view.IsMine)
		{
			localPlayer = this;
			data.isLocalPlayer = true;
		}
	}

	private void Update()
	{
		if (data.isLocalPlayer)
		{
			input.Update();
		}
	}
}
