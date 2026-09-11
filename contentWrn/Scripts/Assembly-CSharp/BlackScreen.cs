using UnityEngine;

public class BlackScreen : MonoBehaviour
{
	public static BlackScreen instance;

	public float blackScreenForSeconds;

	public CanvasGroup gr;

	private Canvas canvas;

	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		gr = GetComponent<CanvasGroup>();
		canvas = GetComponent<Canvas>();
	}

	private void Update()
	{
		blackScreenForSeconds -= Time.deltaTime;
		if (blackScreenForSeconds > 0f)
		{
			gr.alpha = Mathf.MoveTowards(gr.alpha, 1f, Time.deltaTime * 5f);
			canvas.enabled = true;
			return;
		}
		gr.alpha = Mathf.MoveTowards(gr.alpha, 0f, Time.deltaTime * 5f);
		if (gr.alpha < 0.01f)
		{
			canvas.enabled = false;
		}
	}

	public void SetBlackScreen(float seconds)
	{
		if (seconds > blackScreenForSeconds)
		{
			blackScreenForSeconds = seconds;
		}
	}
}
