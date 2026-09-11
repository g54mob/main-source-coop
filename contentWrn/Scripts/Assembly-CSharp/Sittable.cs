using Photon.Pun;
using UnityEngine;

public class Sittable : Interactable
{
	public string sitAnimationName = "PlayerSit";

	public Player sitter;

	private int seatID;

	private Sittable[] seats;

	private PhotonView view;

	private AnimationCurve lookCurve;

	private float occupiedFor;

	private void Start()
	{
		lookCurve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(0.5f, 1f), new Keyframe(1f, 0f));
		view = GetComponentInParent<PhotonView>();
		seats = view.transform.GetComponentsInChildren<Sittable>();
		seatID = GetSeatID();
		hoverText = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.Sit);
	}

	private void Update()
	{
		if ((bool)sitter)
		{
			occupiedFor += Time.deltaTime;
		}
		else
		{
			occupiedFor = 0f;
		}
		if (occupiedFor > 0.5f && (bool)sitter && sitter.refs.view.IsMine && sitter.TryingToLeavePose())
		{
			Call_UnSit();
		}
		if ((bool)sitter && sitter.IsLocal)
		{
			float num = lookCurve.Evaluate(occupiedFor);
			if (num > 0.01f)
			{
				sitter.SetLookDirection(Vector3.Lerp(sitter.data.lookDirection, base.transform.forward, Time.deltaTime * 10f * num));
			}
		}
	}

	private void FixedUpdate()
	{
		if ((bool)sitter)
		{
			sitter.ClampGravity(0.25f);
			Rigidbody rig = sitter.GetRig(BodypartType.Hip);
			float num = 0.1f;
			if (HelperFunctions.FlatDistance(base.transform.position, rig.position) > 0.25f)
			{
				num = 1f;
			}
			Vector3 vector = base.transform.position - rig.position + Vector3.up * num;
			rig.AddForce(vector * 200f, ForceMode.Acceleration);
			rig.linearVelocity *= 0.9f;
		}
	}

	public override void Interact(Player player)
	{
		if (!(sitter != null))
		{
			Player.localPlayer.refs.view.RPC("RPCM_RequestSit", RpcTarget.MasterClient, view.ViewID, seatID);
		}
	}

	private void Call_UnSit()
	{
		Player.localPlayer.refs.view.RPC("RPCA_UnSit", RpcTarget.All, view.ViewID, seatID);
	}

	public void UnSit()
	{
		sitter.refs.animationHandler.PlayAnimation("Idle");
		sitter.data.currentSeat = null;
		sitter = null;
	}

	private int GetSeatID()
	{
		for (int i = 0; i < seats.Length; i++)
		{
			if (seats[i] == this)
			{
				return i;
			}
		}
		return -1;
	}

	public void PlayerSit(Player player)
	{
		player.refs.animationHandler.PlayAnimation(sitAnimationName);
		player.data.currentSeat = this;
		sitter = player;
	}
}
