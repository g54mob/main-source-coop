using System;
using Photon.Pun;
using TMPro;
using UnityEngine;

public class EndGameScreen : MonoBehaviour
{
	private Canvas canvas;

	public Transform firstGo;

	public Transform secondGo;

	public Transform thirdGo;

	public Transform dreamGo;

	private PhotonView view;

	private float watchedFor;

	private Action m_OnDoneAction;

	private void Start()
	{
		view = GetComponent<PhotonView>();
		canvas = GetComponent<Canvas>();
	}

	private void Update()
	{
		watchedFor += Time.deltaTime;
	}

	public void StartWatching(Action a = null)
	{
		view = GetComponent<PhotonView>();
		canvas = GetComponent<Canvas>();
		m_OnDoneAction = a;
		int num = UnityEngine.Random.Range(0, firstGo.childCount);
		int num2 = UnityEngine.Random.Range(0, secondGo.childCount);
		int num3 = UnityEngine.Random.Range(0, thirdGo.childCount);
		view.RPC("RPCA_StartWatching", RpcTarget.All, num, num2, num3);
	}

	[PunRPC]
	public void RPCA_StartWatching(int first, int second, int third)
	{
		view = GetComponent<PhotonView>();
		canvas = GetComponent<Canvas>();
		watchedFor = 0f;
		for (int i = 0; i < firstGo.childCount; i++)
		{
			firstGo.GetChild(i).gameObject.SetActive(value: false);
		}
		for (int j = 0; j < secondGo.childCount; j++)
		{
			secondGo.GetChild(j).gameObject.SetActive(value: false);
		}
		for (int k = 0; k < thirdGo.childCount; k++)
		{
			thirdGo.GetChild(k).gameObject.SetActive(value: false);
		}
		if (Enum.TryParse<LocalizationKeys.Keys>("Endscreen1_" + (first + 1), out var result))
		{
			string localizedString = LocalizationKeys.GetLocalizedString(result);
			firstGo.GetChild(first).GetComponent<TextMeshProUGUI>().text = localizedString;
		}
		if (Enum.TryParse<LocalizationKeys.Keys>("Endscreen2_" + (second + 1), out result))
		{
			string localizedString2 = LocalizationKeys.GetLocalizedString(result);
			secondGo.GetChild(second).GetComponent<TextMeshProUGUI>().text = localizedString2;
		}
		if (Enum.TryParse<LocalizationKeys.Keys>("Endscreen3_" + (third + 1), out result))
		{
			string localizedString3 = LocalizationKeys.GetLocalizedString(result);
			thirdGo.GetChild(third).GetComponent<TextMeshProUGUI>().text = localizedString3;
		}
		string localizedString4 = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.EndscreenDream);
		dreamGo.GetComponent<TextMeshProUGUI>().text = localizedString4;
		firstGo.GetChild(first).gameObject.SetActive(value: true);
		secondGo.GetChild(second).gameObject.SetActive(value: true);
		thirdGo.GetChild(third).gameObject.SetActive(value: true);
		GetComponent<Animator>().Play("EndScreen", 0, 0f);
		canvas.enabled = true;
	}

	private void Skip()
	{
		DoneWatching();
	}

	public void DoneWatching()
	{
		m_OnDoneAction?.Invoke();
	}
}
