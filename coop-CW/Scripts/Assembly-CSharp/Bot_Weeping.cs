using System.Collections;
using DefaultNamespace;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

public class Bot_Weeping : MonoBehaviour
{
	public float deAgrroDistance = 50f;

	public float turnSpeed = 12f;

	public float captureDistance = 3f;

	public float timeUntilKillBecauseCaptchaNotStarted = 20f;

	public float superCloseAggroRange = 10f;

	public Transform rayPoint;

	public Player capturedPlayer;

	public CaptchaGame captchaGame;

	public Player playerInCaptchaGame;

	public Bot bot;

	public CapturedCaptchaCanvas capturedCanvas;

	public CaptchaTerminalCanvas captchaTerminal;

	public float timeSinceCapture;

	public bool captchaGameFinished;

	[HideInInspector]
	public bool debugCapturePlayerOverride;

	public GameObject frontBlocker;

	public bool captchaGameFailed;

	private bool captchaStarted;

	private float timeWaitedToStartCaptcha;

	private PhotonView view;

	[SerializeField]
	private Bot_SimpleMovement m_movement;

	private bool readyToBeRemoved;

	private float timeSpentBeingReadyToBeRemoved;

	public bool HasCapturedPlayer
	{
		get
		{
			if (!(capturedPlayer != null))
			{
				return debugCapturePlayerOverride;
			}
			return true;
		}
	}

	public bool HasPlayerInCaptchaGame => playerInCaptchaGame != null;

	private Vector3 CapturePoint => bot.centerTransform.TransformPoint(Vector3.forward * -0.1f);

	private void Start()
	{
		view = GetComponent<PhotonView>();
		bot = GetComponent<Bot>();
		captchaGame = GetComponent<CaptchaGame>();
		capturedCanvas = base.transform.root.GetComponentInChildren<CapturedCaptchaCanvas>();
		captchaTerminal = base.transform.root.GetComponentInChildren<CaptchaTerminalCanvas>();
	}

	private void Update()
	{
		if (captchaGameFinished)
		{
			if (!view.IsMine)
			{
				return;
			}
			bot.StandStill();
			if (readyToBeRemoved)
			{
				timeSpentBeingReadyToBeRemoved += Time.deltaTime;
				if (timeSpentBeingReadyToBeRemoved > 20f && !AnyoneLookingAtMe(out var _))
				{
					PhotonNetwork.Destroy(base.transform.root.gameObject);
				}
			}
			return;
		}
		m_movement.enabled = false;
		if (HasPlayerInCaptchaGame && playerInCaptchaGame.refs.view.IsMine)
		{
			RunCaptchaGame();
		}
		if (HasCapturedPlayer)
		{
			CaptchaUpdateForEveryone();
		}
		if (!view.IsMine)
		{
			return;
		}
		if (HasCapturedPlayer)
		{
			bot.StandStill();
			if (Vector3.Distance(capturedPlayer.Center(), CapturePoint) > 1.125f)
			{
				view.RPC("RPCA_RelasePlayerAndRestartCaptchaThings", RpcTarget.All);
			}
			return;
		}
		TryCapturePlayer();
		if (AnyoneLookingAtMe(out var firstPlayerLookingAtMe2))
		{
			if (bot.targetPlayer == null)
			{
				bot.SetTargetPlayer(firstPlayerLookingAtMe2);
			}
			bot.StandStill();
			return;
		}
		TargetSomeoneIfTheyAreReallyCloseToMe();
		if (bot.targetPlayer == null)
		{
			bot.StandStill();
			return;
		}
		bool num = !bot.CanSeeTarget(bot.Center(), 360f);
		bool flag = bot.remainingNavDistance > deAgrroDistance;
		if (num && flag && bot.remainingNavDistance < 1000000f)
		{
			Debug.LogError("Lost Target");
			bot.LoseTarget();
		}
		else
		{
			bot.navTargetPos_Set = bot.targetPlayer.CenterGroundPos();
			bot.RotateThenMove(bot.navDirection_Read, turnSpeed);
			m_movement.enabled = true;
		}
	}

	public WeepingContentProvider.WEEPING_CONTENT_STATE GetContentState()
	{
		if (captchaGameFinished)
		{
			if (captchaGameFailed)
			{
				return WeepingContentProvider.WEEPING_CONTENT_STATE.fail;
			}
			return WeepingContentProvider.WEEPING_CONTENT_STATE.success;
		}
		if (HasCapturedPlayer)
		{
			return WeepingContentProvider.WEEPING_CONTENT_STATE.captured;
		}
		return WeepingContentProvider.WEEPING_CONTENT_STATE.idle;
	}

	[PunRPC]
	private void RPCA_RelasePlayerAndRestartCaptchaThings()
	{
		capturedPlayer = null;
		capturedCanvas.root.SetActive(value: false);
		captchaGame.terminalCavnas.root.SetActive(value: false);
		RPCA_LeaveCaptchaGame();
		timeSinceCapture = 0f;
		if (captchaGame.gameState == CaptchaGame.RESULT.playing)
		{
			captchaGame.TurnOffGame();
		}
	}

	public void PlayerInteracted(PhotonView playerView)
	{
		view.RPC("RPCM_PlayerJoinsCaptchaGame", RpcTarget.MasterClient, playerView.ViewID);
	}

	[PunRPC]
	private void RPCM_PlayerJoinsCaptchaGame(int playerViewId)
	{
		if (!(playerInCaptchaGame != null))
		{
			view.RPC("RPCA_JoinCaptchaGame", RpcTarget.All, playerViewId);
		}
	}

	private void CaptchaUpdateForEveryone()
	{
		timeSinceCapture += Time.deltaTime;
		if (!captchaStarted)
		{
			timeWaitedToStartCaptcha += Time.deltaTime;
			capturedCanvas.SetBeforeTimeLeft(timeUntilKillBecauseCaptchaNotStarted - timeWaitedToStartCaptcha);
			if (timeWaitedToStartCaptcha > timeUntilKillBecauseCaptchaNotStarted && view.IsMine)
			{
				view.RPC("RPCA_CaptchaGameFailed", RpcTarget.All);
			}
		}
		else
		{
			captchaGame.RunTimer();
		}
	}

	[PunRPC]
	private void RPCA_InputCharToCaptcha(string input)
	{
		captchaGame.TryText(input[0]);
	}

	[PunRPC]
	private void RPCA_InputButtonToCaptcha(string input)
	{
		captchaGame.TryButton(input[0]);
	}

	private void TryCapturePlayer()
	{
		if (!HasCapturedPlayer && !(bot.targetPlayer == null) && !(Vector3.Distance(CapturePoint, bot.targetPlayer.Center()) > captureDistance))
		{
			view.RPC("RPCA_CapturePlayer", RpcTarget.All, bot.targetPlayer.refs.view.ViewID);
		}
	}

	[PunRPC]
	public void RPCA_CapturePlayer(int playerId)
	{
		timeWaitedToStartCaptcha = 0f;
		timeSinceCapture = 0f;
		capturedPlayer = PlayerHandler.instance.TryGetPlayerFromViewID(playerId);
		captchaGame.CreateGame();
		capturedCanvas.GameWaitingToStart(timeUntilKillBecauseCaptchaNotStarted);
		Vector3 delta = CapturePoint - capturedPlayer.Center();
		capturedPlayer.MoveAllRigsInDirection(delta);
		if (view.IsMine)
		{
			TurnToDirectionWithSpace();
		}
		StartCoroutine(IBlock());
		IEnumerator IBlock()
		{
			frontBlocker.SetActive(value: true);
			yield return new WaitForSeconds(1f);
			frontBlocker.SetActive(value: false);
		}
	}

	private void DebugCapturePlayer()
	{
		if (view.IsMine)
		{
			debugCapturePlayerOverride = true;
			TurnToDirectionWithSpace();
			captchaGame.CreateGame();
			capturedCanvas.GameWaitingToStart(timeUntilKillBecauseCaptchaNotStarted);
		}
	}

	public void TurnToDirectionWithSpace()
	{
		float radius = 0.25f;
		Vector3 vector = bot.Center();
		float num = 5f;
		Vector3 vector2 = base.transform.forward;
		if (!HelperFunctions.SphereLineCheck(vector, vector + vector2 * num, HelperFunctions.LayerType.TerrainProp, radius).transform)
		{
			return;
		}
		int num2 = 20;
		int num3 = 360 / num2;
		Vector3 vector3 = Vector3.zero;
		float num4 = float.MaxValue;
		for (int i = 0; i < num2; i++)
		{
			if (i != 0)
			{
				vector2 = Quaternion.AngleAxis(num3 * i, Vector3.up) * vector2;
				RaycastHit raycastHit = HelperFunctions.SphereLineCheck(vector, vector + vector2 * num, HelperFunctions.LayerType.TerrainProp, radius);
				if (raycastHit.transform == null)
				{
					vector3 = vector2;
					break;
				}
				if (num4 < raycastHit.distance)
				{
					vector3 = vector2;
					num4 = raycastHit.distance;
					Debug.DrawLine(vector, vector + vector2 * num, Color.yellow, 2f);
				}
				else
				{
					Debug.DrawLine(vector, vector + vector2 * num, Color.red, 2f);
				}
			}
		}
		Debug.DrawLine(vector, vector + vector3 * num, Color.green, 2f);
		bot.Look(vector3, 20f);
	}

	private void RunCaptchaGame()
	{
		if (!HasCapturedPlayer)
		{
			return;
		}
		if (Keyboard.current.escapeKey.wasPressedThisFrame)
		{
			view.RPC("RPCA_LeaveCaptchaGame", RpcTarget.All);
			return;
		}
		string inputString = Input.inputString;
		if (CaptchaGame.ValidInput(inputString))
		{
			view.RPC("RPCA_InputCharToCaptcha", RpcTarget.All, inputString[0].ToString());
		}
		if (Gamepad.current != null)
		{
			if (Gamepad.current.buttonEast.wasPressedThisFrame)
			{
				view.RPC("RPCA_LeaveCaptchaGame", RpcTarget.All);
				return;
			}
			string buttonString = captchaTerminal.GetButtonString();
			if (CaptchaGame.ValidButton(buttonString))
			{
				view.RPC("RPCA_InputButtonToCaptcha", RpcTarget.All, buttonString[0].ToString());
			}
		}
		if (capturedPlayer.data.dead || captchaGame.gameState == CaptchaGame.RESULT.failed)
		{
			view.RPC("RPCA_CaptchaGameFailed", RpcTarget.All);
		}
		if (captchaGame.gameState == CaptchaGame.RESULT.completed)
		{
			view.RPC("RPCA_CaptchaGameSuccess", RpcTarget.All);
		}
	}

	[PunRPC]
	private void RPCA_CaptchaGameFailed()
	{
		captchaGameFailed = true;
		debugCapturePlayerOverride = false;
		captchaGameFinished = true;
		captchaGame.gameState = CaptchaGame.RESULT.failed;
		Player playerToKill = capturedPlayer;
		capturedPlayer = null;
		Debug.Log("capthca game failed");
		RPCA_LeaveCaptchaGame();
		if (view.IsMine)
		{
			StartCoroutine(KillAfterAWhile());
		}
		IEnumerator KillAfterAWhile()
		{
			float time = 0f;
			while (time < 1.5f)
			{
				time += Time.deltaTime;
				yield return null;
			}
			if (playerToKill != null)
			{
				Debug.LogError("Killing player because captcha failed!");
				playerToKill.CallDie();
			}
			else
			{
				Debug.LogError("Captured player is null. weeping cant kill it even tho the game has failed.");
			}
			playerToKill = null;
			readyToBeRemoved = true;
		}
	}

	[PunRPC]
	private void RPCA_CaptchaGameSuccess()
	{
		debugCapturePlayerOverride = false;
		captchaGameFinished = true;
		RPCA_LeaveCaptchaGame();
		capturedPlayer = null;
		readyToBeRemoved = true;
	}

	[PunRPC]
	public void RPCA_JoinCaptchaGame(int playerID)
	{
		playerInCaptchaGame = PlayerHandler.instance.TryGetPlayerFromViewID(playerID);
		playerInCaptchaGame.data.hookedIntoTerminal = true;
		captchaStarted = true;
		captchaGame.StartGame();
	}

	[PunRPC]
	public void RPCA_LeaveCaptchaGame()
	{
		if (playerInCaptchaGame != null)
		{
			playerInCaptchaGame.data.hookedIntoTerminal = false;
		}
		playerInCaptchaGame = null;
	}

	private bool AnyoneLookingAtMe(out Player firstPlayerLookingAtMe)
	{
		foreach (Player item in PlayerHandler.instance.playersAlive)
		{
			if ((!(capturedPlayer != null) || !(capturedPlayer == item)) && item.CanSee(rayPoint.position, 70f, bot.targetPlayer == null))
			{
				firstPlayerLookingAtMe = item;
				return true;
			}
		}
		firstPlayerLookingAtMe = null;
		return false;
	}

	public void TargetSomeoneIfTheyAreReallyCloseToMe()
	{
		Player player = null;
		float num = float.MaxValue;
		foreach (Player item in PlayerHandler.instance.playersAlive)
		{
			float num2 = Vector3.Distance(item.HeadPosition(), base.transform.position);
			if (num2 < superCloseAggroRange && num2 < num)
			{
				num = num2;
				player = item;
			}
		}
		if (player != null)
		{
			bot.SetTargetPlayer(player);
		}
	}
}
