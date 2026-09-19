using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NameTagUI : MonoBehaviour
{
	public TextMeshProUGUI label;

	public RectTransform rect;

	[Header("Ölüm")]
	public CanvasGroup canvasGroup;

	public float deadAlpha = 0.5f;

	[Header("Buzzing (sinek uyarısı)")]
	public GameObject buzzingImage;

	[Tooltip("Sinek evresine kalan süreyi 0 (henüz uzak) → 1 (yakın/aktif) dolum olarak gösterir")]
	public Image buzzingFillImage;

	[Tooltip("Fill 0 rengi (henüz uzak)")]
	public Color buzzingFillColorFar = new Color32(59, 117, 129, byte.MaxValue);

	[Tooltip("Fill 1 rengi (sinek evresi çok yakın / aktif)")]
	public Color buzzingFillColorNear = new Color32(183, 47, 20, byte.MaxValue);

	[Header("Konuşma Göstergesi")]
	public GameObject speakingImage;

	private Vector3 _smoothedWorldPos;

	private bool _hasWorldPos;

	private void Awake()
	{
		if (rect == null)
		{
			rect = GetComponent<RectTransform>();
		}
		if (canvasGroup == null)
		{
			canvasGroup = GetComponent<CanvasGroup>();
		}
	}

	public void SetDeadAlpha(bool dead)
	{
		if (canvasGroup != null)
		{
			canvasGroup.alpha = (dead ? deadAlpha : 1f);
		}
	}

	public void SetBuzzWarning(int value)
	{
		bool flag = value != -1;
		if (buzzingImage != null && buzzingImage.activeSelf != flag)
		{
			buzzingImage.SetActive(flag);
		}
		if (!(buzzingFillImage == null))
		{
			float num;
			if (!flag)
			{
				num = 0f;
			}
			else if (value == -2)
			{
				num = 1f;
			}
			else
			{
				float num2 = ((GameManager.Instance != null) ? ((float)GameManager.Instance.ConfiguredBuzzingInterval / 3f) : 10f);
				num = ((num2 > 0f) ? (1f - Mathf.Clamp01((float)value / num2)) : 1f);
			}
			buzzingFillImage.fillAmount = num;
			buzzingFillImage.color = Color.Lerp(buzzingFillColorFar, buzzingFillColorNear, num);
		}
	}

	private void OnDisable()
	{
		_hasWorldPos = false;
	}

	public void SetSpeaking(bool speaking)
	{
		if (speakingImage != null && speakingImage.activeSelf != speaking)
		{
			speakingImage.SetActive(speaking);
		}
	}

	public void SetText(string text)
	{
		if (label != null)
		{
			label.text = text;
		}
	}

	public void UpdateWorldPosition(Vector3 targetWorldPos, Camera cam, float smoothing)
	{
		if (!_hasWorldPos || smoothing <= 0f)
		{
			_smoothedWorldPos = targetWorldPos;
			_hasWorldPos = true;
		}
		else
		{
			float t = 1f - Mathf.Exp((0f - smoothing) * Time.deltaTime);
			_smoothedWorldPos = Vector3.Lerp(_smoothedWorldPos, targetWorldPos, t);
		}
		if (!(rect == null) && !(cam == null))
		{
			Vector3 vector = cam.WorldToScreenPoint(_smoothedWorldPos);
			rect.position = new Vector3(vector.x, vector.y, 0f);
		}
	}

	public void SetScale(float scale)
	{
		if (rect != null)
		{
			rect.localScale = new Vector3(scale, scale, 1f);
		}
	}
}
