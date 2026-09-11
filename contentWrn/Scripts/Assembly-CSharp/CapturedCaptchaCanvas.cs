using System;
using System.Collections;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI.ProceduralImage;

public class CapturedCaptchaCanvas : MonoBehaviour
{
	public GameObject beforePlayerInTerminal;

	public TextMeshProUGUI beforeTimer;

	public GameObject afterPlayerInTerminal;

	public ProceduralImage gameTimer;

	public TextMeshProUGUI triesText;

	public GameObject triesRoot;

	private GameObject tryDot;

	public Color trySuccessColor = new Color(0.58f, 1f, 0.24f);

	private Color tryDefaultColor;

	public GameObject root;

	public GameObject playingGo;

	public GameObject failureGo;

	public float showFailureTimeForTime = 0.5f;

	public void Awake()
	{
		root.SetActive(value: false);
	}

	private IEnumerator FailScreen(bool shake)
	{
		float elapsed = showFailureTimeForTime;
		failureGo.SetActive(value: true);
		playingGo.SetActive(value: false);
		if (shake)
		{
			GamefeelHandler.instance.perlin.AddShake(MainCamera.instance.transform.position, 1f, 0.3f);
		}
		while (elapsed > 0f)
		{
			elapsed -= Time.deltaTime;
			yield return null;
		}
		failureGo.SetActive(value: false);
		playingGo.SetActive(value: true);
	}

	public void DoFailStuff(bool shake)
	{
		StartCoroutine(FailScreen(shake));
	}

	public void GameWaitingToStart(float timeToStart)
	{
		root.SetActive(value: true);
		beforePlayerInTerminal.SetActive(value: true);
		afterPlayerInTerminal.SetActive(value: false);
		beforeTimer.text = timeToStart.ToString(CultureInfo.InvariantCulture);
	}

	public void GameStarted(int captchaLength)
	{
		root.SetActive(value: true);
		beforePlayerInTerminal.SetActive(value: false);
		afterPlayerInTerminal.SetActive(value: true);
		tryDot = triesRoot.transform.GetChild(0).gameObject;
		tryDefaultColor = tryDot.GetComponent<ProceduralImage>().color;
		tryDot.SetActive(value: false);
		int childCount = triesRoot.transform.childCount;
		for (int num = childCount - 1; num >= 0; num--)
		{
			if (num != 0)
			{
				UnityEngine.Object.Destroy(triesRoot.transform.GetChild(num).gameObject);
			}
		}
		childCount = triesRoot.transform.childCount;
		Debug.Log("ChildCount Bafore " + childCount);
		for (int i = 0; i < captchaLength; i++)
		{
			UnityEngine.Object.Instantiate(tryDot, triesRoot.transform).SetActive(value: true);
		}
		Debug.Log("ChildCount after " + childCount);
	}

	public void SetProgress(int progress)
	{
		for (int i = 0; i < triesRoot.transform.childCount; i++)
		{
			if (i != 0)
			{
				triesRoot.transform.GetChild(i).GetComponent<ProceduralImage>().color = ((i <= progress) ? trySuccessColor : tryDefaultColor);
			}
		}
	}

	public void SetBeforeTimeLeft(float timeLeft)
	{
		TimeSpan timeSpan = TimeSpan.FromSeconds(timeLeft);
		beforeTimer.text = $"{timeSpan.Minutes}:{timeSpan.Seconds}";
	}

	public void SetGameTimer(float timeLeft, float maxTime)
	{
		gameTimer.fillAmount = Mathf.InverseLerp(0f, maxTime, timeLeft);
	}

	public void SetTries(int fails, float maxTries)
	{
		triesText.text = $"{maxTries - (float)fails}";
		SetProgress(0);
	}
}
