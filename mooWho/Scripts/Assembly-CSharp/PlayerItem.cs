using System;
using System.Collections;
using Dissonance;
using Dissonance.Integrations.MirrorIgnorance;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerItem : MonoBehaviour
{
	[Header("UI")]
	public Image profileImage;

	public TextMeshProUGUI usernameText;

	public CanvasGroup canvasGroup;

	[Header("Scale")]
	public Vector3 hunterScale = new Vector3(1.2f, 1.2f, 1.2f);

	public Vector3 animalScale = Vector3.one;

	[Header("Ölüm")]
	public float deadAlpha = 0.5f;

	[Header("Ölüm Animasyonu (pop + kırmızı flaş)")]
	[Tooltip("Flaş rengi bu Image üstünde oynar — boşsa profileImage kullanılır")]
	public Image deathFlashImage;

	public Color deathFlashColor = Color.red;

	[Tooltip("Pop darbesinin tepe ölçek çarpanı (rol ölçeğinin üstüne)")]
	public float deathPopScale = 1.3f;

	[Tooltip("Pop + flaşın toplam süresi (sn) — büyüyüp/kızarıp geri döner, sonra dim alpha'ya geçer")]
	public float deathAnimDuration = 0.4f;

	[Header("Ses (kim konuşuyor)")]
	[Tooltip("Bu oyuncu voice chat'te konuşurken açılır, susunca kapanır — özellikle ölüler artık hem canlıları hem ölüleri duyduğu için (bkz. SpectatorController) kimin konuştuğunu ayırt etmesine yardımcı olur")]
	public Image speakerImage;

	private MyClient _client;

	private PlayerRoleData _roleData;

	private MirrorIgnorancePlayer _voicePlayer;

	private Health _health;

	private static DissonanceComms _comms;

	private bool _speakerVisible;

	private Vector3 _roleScale = Vector3.one;

	private Color _normalColor = Color.white;

	private bool _wasDead;

	private Coroutine _deathAnimRoutine;

	public MyClient Client => _client;

	public bool IsHunter { get; private set; }

	private Image FlashTarget
	{
		get
		{
			if (!(deathFlashImage != null))
			{
				return profileImage;
			}
			return deathFlashImage;
		}
	}

	private void Awake()
	{
		Image flashTarget = FlashTarget;
		if (flashTarget != null)
		{
			_normalColor = flashTarget.color;
		}
	}

	public void Setup(MyClient client, PlayerRoleData roleData, Sprite profileSprite, bool isHunter)
	{
		_client = client;
		_roleData = roleData;
		IsHunter = isHunter;
		_voicePlayer = ((client != null) ? client.GetComponent<MirrorIgnorancePlayer>() : null);
		_health = ((client != null) ? client.GetComponent<Health>() : null);
		_speakerVisible = false;
		if (speakerImage != null)
		{
			speakerImage.enabled = false;
		}
		if (usernameText != null)
		{
			usernameText.text = ((client != null) ? client.playerInfo.username : "?");
		}
		if (profileImage != null && profileSprite != null)
		{
			profileImage.sprite = profileSprite;
		}
		_roleScale = (isHunter ? hunterScale : animalScale);
		base.transform.localScale = _roleScale;
		if (canvasGroup == null)
		{
			canvasGroup = GetComponent<CanvasGroup>();
		}
		if (canvasGroup != null)
		{
			canvasGroup.alpha = 1f;
		}
	}

	private void Update()
	{
		if (speakerImage == null || _voicePlayer == null || string.IsNullOrEmpty(_voicePlayer.PlayerId))
		{
			return;
		}
		if (_comms == null)
		{
			_comms = UnityEngine.Object.FindObjectOfType<DissonanceComms>();
		}
		if (!(_comms == null))
		{
			bool num = !(_health != null) || !_health.IsDead || SpectatorController.IsSpectating;
			bool flag = false;
			if (num)
			{
				flag = _comms.FindPlayer(_voicePlayer.PlayerId)?.IsSpeaking ?? false;
			}
			if (flag != _speakerVisible)
			{
				_speakerVisible = flag;
				speakerImage.enabled = flag;
			}
		}
	}

	public void RefreshUsername()
	{
		if (usernameText != null && _client != null)
		{
			usernameText.text = _client.playerInfo.username;
		}
	}

	public void SetDead(bool dead, bool animate = true)
	{
		if (canvasGroup == null)
		{
			canvasGroup = GetComponent<CanvasGroup>();
		}
		bool num = animate && dead && !_wasDead;
		_wasDead = dead;
		if (num)
		{
			if (_deathAnimRoutine != null)
			{
				StopCoroutine(_deathAnimRoutine);
			}
			_deathAnimRoutine = StartCoroutine(PlayDeathAnim());
		}
		else if (!dead || _deathAnimRoutine == null)
		{
			if (_deathAnimRoutine != null)
			{
				StopCoroutine(_deathAnimRoutine);
				_deathAnimRoutine = null;
			}
			Image flashTarget = FlashTarget;
			if (flashTarget != null)
			{
				flashTarget.color = _normalColor;
			}
			base.transform.localScale = _roleScale;
			if (canvasGroup != null)
			{
				canvasGroup.alpha = (dead ? deadAlpha : 1f);
			}
		}
	}

	private IEnumerator PlayDeathAnim()
	{
		Image img = FlashTarget;
		if (canvasGroup != null)
		{
			canvasGroup.alpha = 1f;
		}
		float t = 0f;
		while (t < deathAnimDuration)
		{
			t += Time.deltaTime;
			float num = Mathf.Sin(Mathf.Clamp01(t / deathAnimDuration) * MathF.PI);
			base.transform.localScale = _roleScale * (1f + (deathPopScale - 1f) * num);
			if (img != null)
			{
				img.color = Color.Lerp(_normalColor, deathFlashColor, num);
			}
			yield return null;
		}
		base.transform.localScale = _roleScale;
		if (img != null)
		{
			img.color = _normalColor;
		}
		if (canvasGroup != null)
		{
			canvasGroup.alpha = deadAlpha;
		}
		_deathAnimRoutine = null;
	}
}
