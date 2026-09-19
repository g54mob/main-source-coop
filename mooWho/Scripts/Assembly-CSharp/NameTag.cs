using Dissonance;
using Dissonance.Integrations.MirrorIgnorance;
using UnityEngine;

public class NameTag : MonoBehaviour
{
	[Header("Anchor (kafanın üstü)")]
	public Transform anchor;

	public float heightOffset = 2.2f;

	private MyClient _myClient;

	private Health _health;

	private PlayerVoiceMonitor _voiceMonitor;

	private MirrorIgnorancePlayer _voicePlayer;

	private DissonanceComms _comms;

	private bool _registered;

	public NameTagUI UI { get; private set; }

	public PlayerRoleData RoleData { get; private set; }

	public bool IsLocalPlayerTag
	{
		get
		{
			if (_myClient != null)
			{
				return _myClient.isLocalPlayer;
			}
			return false;
		}
	}

	public bool IsDead
	{
		get
		{
			if (_health != null)
			{
				return _health.IsDead;
			}
			return false;
		}
	}

	public int BuzzWarningSeconds
	{
		get
		{
			if (!(_voiceMonitor != null))
			{
				return -1;
			}
			return _voiceMonitor.BuzzWarningSeconds;
		}
	}

	public bool IsSpeaking
	{
		get
		{
			if (_voicePlayer == null || string.IsNullOrEmpty(_voicePlayer.PlayerId))
			{
				return false;
			}
			if (_comms == null)
			{
				_comms = Object.FindObjectOfType<DissonanceComms>();
			}
			if (_comms == null)
			{
				return false;
			}
			return _comms.FindPlayer(_voicePlayer.PlayerId)?.IsSpeaking ?? false;
		}
	}

	private void Awake()
	{
		_myClient = GetComponent<MyClient>();
		RoleData = GetComponent<PlayerRoleData>();
		_health = GetComponent<Health>();
		_voiceMonitor = GetComponent<PlayerVoiceMonitor>();
		_voicePlayer = GetComponent<MirrorIgnorancePlayer>();
	}

	private void OnEnable()
	{
		TryRegister();
	}

	private void Update()
	{
		if (!_registered)
		{
			TryRegister();
		}
	}

	private void TryRegister()
	{
		if (!_registered && !(NameTagManager.Instance == null) && !(NameTagManager.Instance.tagPrefab == null))
		{
			UI = Object.Instantiate(NameTagManager.Instance.tagPrefab, NameTagManager.Instance.canvas.transform);
			UI.gameObject.SetActive(value: false);
			NameTagManager.Instance.Register(this);
			_registered = true;
		}
	}

	private void OnDisable()
	{
		Cleanup();
	}

	private void OnDestroy()
	{
		Cleanup();
	}

	private void Cleanup()
	{
		if (NameTagManager.Instance != null)
		{
			NameTagManager.Instance.Unregister(this);
		}
		_registered = false;
	}

	public Vector3 GetAnchorPosition()
	{
		if (!(anchor != null))
		{
			return base.transform.position + Vector3.up * heightOffset;
		}
		return anchor.position;
	}

	public string GetUsername()
	{
		if (_myClient == null)
		{
			_myClient = GetComponent<MyClient>();
		}
		string text = ((_myClient != null) ? _myClient.playerInfo.username : null);
		if (UI != null && !string.IsNullOrEmpty(text) && UI.label != null && UI.label.text != text)
		{
			UI.SetText(text);
		}
		return text;
	}
}
