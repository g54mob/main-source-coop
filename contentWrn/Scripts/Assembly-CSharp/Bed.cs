using Photon.Pun;
using UnityEngine;

public class Bed : Interactable
{
	private Player playerInBed;

	private PhotonView view;

	private float sinceSleep;

	private Vector3 m_up2 = Vector3.up * 2f;

	private string m_SleepText;

	private string m_NotSleepyText;

	private string m_StartGameTitleText;

	private string m_StartGameBody;

	private string m_StartGameConfirmText;

	private string m_CancelText;

	private Vector2 targetLook;

	private Vector2 lookBefore;

	private void Start()
	{
		view = GetComponent<PhotonView>();
		m_SleepText = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.Sleep);
		m_NotSleepyText = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.NotSleepy);
		hoverText = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.StartGame);
		m_StartGameTitleText = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.StartGameTitle);
		m_StartGameBody = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.StartGameBody);
		m_StartGameConfirmText = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.StartGameConfirm);
		m_CancelText = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.Cancel);
	}

	private void Update()
	{
		hoverText = ((TimeOfDayHandler.TimeOfDay == TimeOfDay.Evening) ? m_SleepText : m_NotSleepyText);
		if ((bool)playerInBed && playerInBed == Player.localPlayer)
		{
			sinceSleep += Time.deltaTime;
			playerInBed.data.playerLookValues = Vector2.Lerp(playerInBed.data.playerLookValues, targetLook, Time.deltaTime * 5f);
		}
		if ((bool)playerInBed && !(Player.localPlayer == null) && !(Player.localPlayer != playerInBed) && Player.localPlayer.TryingToLeavePose())
		{
			LeaveBed();
		}
	}

	private void FixedUpdate()
	{
		if ((bool)playerInBed)
		{
			if (HelperFunctions.FlatDistance(base.transform.position, playerInBed.Center()) > 1f)
			{
				playerInBed.refs.ragdoll.AddForce((base.transform.position + m_up2 - playerInBed.Center()) * 20f, ForceMode.Acceleration);
			}
			else
			{
				playerInBed.refs.ragdoll.AddForce((base.transform.position - playerInBed.Center()) * 10f, ForceMode.Acceleration);
			}
			playerInBed.data.sinceGrounded = Mathf.Clamp(playerInBed.data.sinceGrounded, 0f, 1f);
		}
	}

	private void LeaveBed()
	{
		view.RPC("RPCA_LeaveBed", RpcTarget.All);
	}

	[PunRPC]
	public void RPCA_LeaveBed()
	{
		if ((bool)playerInBed)
		{
			playerInBed.refs.animationHandler.StopSleep();
			playerInBed.data.currentBed = null;
			playerInBed = null;
		}
	}

	public override void Interact(Player player)
	{
		bool hasStarted = SurfaceNetworkHandler.HasStarted;
		if (TimeOfDayHandler.TimeOfDay != TimeOfDay.Evening || (bool)playerInBed)
		{
			return;
		}
		if (!hasStarted)
		{
			Modal.Show(m_StartGameTitleText, m_StartGameBody, new ModalOption[2]
			{
				new ModalOption(m_StartGameConfirmText, delegate
				{
					SurfaceNetworkHandler.Instance.RequestStartGame();
				}),
				new ModalOption(m_CancelText)
			});
		}
		else
		{
			RequestSleep(player);
		}
	}

	public void RequestSleep(Player player)
	{
		view.RPC("RPCM_RequestSleep", RpcTarget.MasterClient, player.refs.view.ViewID);
	}

	[PunRPC]
	public void RPCM_RequestSleep(int playerID)
	{
		if (!playerInBed)
		{
			view.RPC("RPCA_AcceptSleep", RpcTarget.All, playerID);
		}
	}

	[PunRPC]
	public void RPCA_AcceptSleep(int playerID)
	{
		Player player = PlayerHandler.instance.TryGetPlayerFromViewID(playerID);
		if (!(player == null))
		{
			player.data.currentBed = this;
			playerInBed = player;
			player.refs.animationHandler.StartSleep();
			if (player == Player.localPlayer)
			{
				player.data.clampLookValues();
				lookBefore = player.data.playerLookValues;
				player.SetLookDirection(Vector3.up + base.transform.forward * 0.2f);
				targetLook = player.data.playerLookValues;
				player.data.playerLookValues = lookBefore;
				sinceSleep = 0f;
			}
		}
	}
}
