using TMPro;
using UnityEngine;

public class PopupManager : MonoBehaviour
{
	public static PopupManager instance;

	public TMP_Text text_title;

	public GameObject popUp;

	private void Awake()
	{
		instance = this;
	}

	public void Popup_Show(string title)
	{
		text_title.text = title;
		popUp.SetActive(value: true);
	}

	public void Popup_Close()
	{
		popUp.SetActive(value: false);
	}
}
