using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Photon.Pun;
using Photon.Realtime;
using Portningsbolaget.Platforms;
using TMPro;
using UnityEngine;
using UnityEngine.Video;
using Zorro.Core;

public class UploadCompleteUI : MonoBehaviour
{
	public SaveVideoToDesktopInteractable m_saveToDesktopInteractable;

	public CanvasGroup m_saveToDesktopGroup;

	public AnimationCurve m_viewsCurve;

	public AnimationCurve m_saveToDesktopCurve;

	public TextMeshProUGUI m_views;

	public VideoPlayer m_videoPlayer;

	public GameObject m_commentsPrefab;

	public Transform m_commentsParent;

	public List<CommentUI> m_comments = new List<CommentUI>();

	private CameraRecording m_replayVideo;

	private IPlayableVideo m_replayLostFootage;

	private int m_replayViews;

	private Comment[] m_replayComments;

	private Action m_onPlayed;

	private PhotonView photonView;

	private string m_ViewsText;

	private bool m_canWatchVideo = true;

	private void Awake()
	{
		photonView = GetComponent<PhotonView>();
		PhotonNetwork.AllocateViewID(photonView);
	}

	public void PlayVideo(IPlayableVideo playableVideo, int views, Comment[] comments, Action onPlayed)
	{
		m_saveToDesktopGroup.gameObject.SetActive(value: false);
		m_replayComments = comments;
		m_replayVideo = null;
		m_replayLostFootage = playableVideo;
		m_replayViews = views;
		m_onPlayed = onPlayed;
		m_ViewsText = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.Views);
		m_views.text = m_ViewsText + ": 0";
		if (playableVideo.TryGetVideoPath(out var path))
		{
			if (!File.Exists(path))
			{
				Debug.LogError("Failed To Get Video Path: " + path);
				Modal.ShowError(LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.Error_VideoPath), path);
			}
			m_canWatchVideo = CanWatchVideo();
			m_videoPlayer.url = path;
			m_videoPlayer.time = 0.0;
			m_videoPlayer.GetTargetAudioSource(0).enabled = CanHearAudio();
			if (m_canWatchVideo)
			{
				m_videoPlayer.Play();
			}
			CameraRecording cameraRecording = playableVideo as CameraRecording;
			StartCoroutine(DisplayVideoEval(views, comments));
		}
		else
		{
			Debug.LogError("Failed To Get Video Path: " + path);
		}
		StartCoroutine(VideoDone());
		IEnumerator VideoDone()
		{
			while (!m_videoPlayer.isPrepared)
			{
				yield return null;
			}
			while (m_videoPlayer.frame + 1 < (long)m_videoPlayer.frameCount)
			{
				yield return null;
			}
			m_onPlayed?.Invoke();
			m_onPlayed = null;
			m_saveToDesktopGroup.gameObject.SetActive(value: true);
			yield return m_saveToDesktopCurve.YieldForCurve(delegate(float f)
			{
				m_saveToDesktopGroup.alpha = f;
			});
		}
	}

	public void PlayVideos(CameraRecording playableVideo, int views, Comment[] comments, Action onPlayed)
	{
		StartCoroutine(PlayVideosAsync(playableVideo, views, comments, onPlayed));
	}

	public IEnumerator PlayVideosAsync(CameraRecording playableVideo, int views, Comment[] comments, Action onPlayed)
	{
		m_saveToDesktopGroup.gameObject.SetActive(value: false);
		m_replayComments = comments;
		m_replayViews = views;
		m_onPlayed = onPlayed;
		m_replayVideo = playableVideo;
		m_replayLostFootage = null;
		m_ViewsText = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.Views);
		m_views.text = m_ViewsText + ": 0";
		StartCoroutine(DisplayVideoEval(views, comments));
		m_saveToDesktopInteractable.SetRecording(playableVideo);
		for (int i = 0; i < playableVideo.m_clips.Count; i++)
		{
			string text = Path.Combine(playableVideo.m_clips[i].GetClipDirectory(), "output.webm");
			if (text != null)
			{
				if (!File.Exists(text))
				{
					Debug.LogError("Failed To Get Video Path: " + text);
					Modal.ShowError(LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.Error_VideoPath), text);
					continue;
				}
				m_canWatchVideo = CanWatchVideo();
				m_videoPlayer.url = text;
				m_videoPlayer.time = 0.0;
				m_videoPlayer.Prepare();
				m_videoPlayer.GetTargetAudioSource(0).enabled = CanHearAudio();
				if (m_canWatchVideo)
				{
					m_videoPlayer.Play();
				}
				while (!m_videoPlayer.isPrepared)
				{
					yield return null;
				}
				while (m_videoPlayer.frame + 1 < (long)m_videoPlayer.frameCount)
				{
					yield return null;
				}
			}
			else
			{
				Debug.LogError("Failed To Get Video Path: " + text);
			}
		}
		m_views.text = m_ViewsText + ": " + BigNumbers.ViewsToString(Mathf.FloorToInt(views)).ToString();
		if (PhotonNetwork.IsMasterClient)
		{
			PhotonGameLobbyHandler.Instance.SetCurrentObjective(new GoToBedSuccessObjective());
		}
		m_onPlayed?.Invoke();
		m_onPlayed = null;
		m_saveToDesktopGroup.gameObject.SetActive(value: true);
		yield return m_saveToDesktopCurve.YieldForCurve(delegate(float f)
		{
			m_saveToDesktopGroup.alpha = f;
		});
	}

	private bool CanWatchVideo()
	{
		if (!m_canWatchVideo)
		{
			return false;
		}
		List<Player> players = PlayerHandler.instance.players;
		if (players.Count == 1)
		{
			return true;
		}
		PlayerVoiceHandler instance = PlayerVoiceHandler.Instance;
		if (instance == null)
		{
			Debug.LogError("Can't watch video: Player voice handler is null");
			return false;
		}
		if (!instance.AllowedToCommunicate)
		{
			Debug.Log("Can't watch video: Not allowed to communicate");
			return false;
		}
		Player localPlayer = Player.localPlayer;
		if (localPlayer == null)
		{
			Debug.LogError("Can't watch video: Failed to get local player");
			return false;
		}
		for (int num = players.Count - 1; num >= 0; num--)
		{
			Player player = players[num];
			if (player == null)
			{
				Debug.LogError($"Can't watch video: Failed to get player {num}");
			}
			else if (!(player == localPlayer))
			{
				if (!player.TryGetGlobalPlayerData(out var d))
				{
					Photon.Realtime.Player owner = player.photonView.Owner;
					Debug.LogError($"Can't watch video: Failed to get global player data for {num} {owner.NickName}");
					return false;
				}
				if (d.isBlocked)
				{
					Debug.Log("Can't watch video: Blocked by a player");
					return false;
				}
			}
		}
		return true;
	}

	private bool CanHearAudio()
	{
		if (!m_canWatchVideo)
		{
			return false;
		}
		Player localPlayer = Player.localPlayer;
		if (localPlayer == null)
		{
			Debug.LogError("Can't hear audio: Failed to get local player");
			return false;
		}
		PlayerHandler instance = PlayerHandler.instance;
		if (instance == null)
		{
			Debug.LogError("Can't hear audio: Failed to get player handler");
			return false;
		}
		for (int num = instance.players.Count - 1; num >= 0; num--)
		{
			Player player = instance.players[num];
			if (player == null)
			{
				Debug.LogError($"Can't hear audio: Failed to get player {num}");
				return false;
			}
			if (!(localPlayer == player))
			{
				if (!player.TryGetGlobalPlayerData(out var d))
				{
					Debug.LogError($"Can't hear audio: Failed to get global player data for player {num} {player.photonView.Owner?.NickName}");
					return false;
				}
				if (!d.canCommunicateWith || d.isBlocked)
				{
					Debug.Log("Can't hear audio: Not allowed or blocked by player");
					return false;
				}
				if (d.isMuted || d.localVoiceVolume == 0f)
				{
					Debug.Log("Can't hear audio: Muted");
					return false;
				}
			}
		}
		return true;
	}

	private IEnumerator DisplayVideoEval(int views, Comment[] comments)
	{
		foreach (CommentUI comment3 in m_comments)
		{
			UnityEngine.Object.Destroy(comment3.gameObject);
		}
		m_comments.Clear();
		float t = 0f;
		yield return 2;
		while (!m_videoPlayer.isPrepared)
		{
			yield return null;
		}
		float time = (float)m_videoPlayer.length;
		if (comments.Length != 0)
		{
			float time2 = comments[^1].Time;
			if (time2 > time)
			{
				time = time2;
			}
			Debug.Log($"Last Time: {time2}");
		}
		Debug.Log($"Video Length: {m_videoPlayer.length}\nPlay Time: {time}");
		int commentIndex = 0;
		while (t < time)
		{
			t += Time.deltaTime;
			for (int i = commentIndex; i < comments.Length; i++)
			{
				Comment comment = comments[i];
				if (!(comment.Time < t))
				{
					break;
				}
				DisplayComment(comment);
				commentIndex++;
			}
			m_views.text = m_ViewsText + ": " + BigNumbers.ViewsToString(Mathf.FloorToInt(m_viewsCurve.Evaluate(t / time) * (float)views)).ToString();
			yield return null;
		}
		for (int j = commentIndex; j < comments.Length; j++)
		{
			Comment comment2 = comments[j];
			DisplayComment(comment2);
		}
		yield return null;
	}

	private void DisplayComment(Comment comment)
	{
		foreach (CommentUI comment2 in m_comments)
		{
			comment2.Move(-100f);
		}
		Debug.Log("Displaying comment: " + comment.Text + " From Event: " + ContentEventIDMapper.GetContentEvent(comment.EventID).ToString());
		CommentUI component = UnityEngine.Object.Instantiate(m_commentsPrefab, m_commentsParent).GetComponent<CommentUI>();
		component.transform.SetAsFirstSibling();
		component.Setup(comment);
		m_comments.Add(component);
		OnCheckCommentEventID(comment.EventID);
	}

	private void OnCheckCommentEventID(ushort eventID)
	{
		ContentEvent contentEvent = ContentEventIDMapper.GetContentEvent(eventID);
		if (!(contentEvent is FlickerContentEvent))
		{
			if (!(contentEvent is BigSlapAgroContentEvent))
			{
				if (contentEvent is StreamerContentEvent)
				{
					PlatformManager.UnlockAchievement(Achievements.ACH_FILM_STREAMER);
				}
			}
			else
			{
				PlatformManager.UnlockAchievement(Achievements.ACH_FILM_BIGSLAP);
			}
		}
		else
		{
			PlatformManager.UnlockAchievement(Achievements.ACH_FILM_FLICKER);
		}
	}

	private void OnDisable()
	{
		m_saveToDesktopInteractable.gameObject.SetActive(value: false);
	}

	public void Replay()
	{
		photonView.RPC("RPCA_Replay", RpcTarget.All);
	}

	[PunRPC]
	private void RPCA_Replay()
	{
		if (base.gameObject.activeSelf)
		{
			if (m_replayVideo != null)
			{
				PlayVideos(m_replayVideo, m_replayViews, m_replayComments, null);
			}
			else
			{
				PlayVideo(m_replayLostFootage, m_replayViews, m_replayComments, null);
			}
		}
	}
}
