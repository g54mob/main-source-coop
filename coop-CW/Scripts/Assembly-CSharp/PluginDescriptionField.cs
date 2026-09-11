using TMPro;
using UnityEngine;

public class PluginDescriptionField : MonoBehaviour
{
	public TMP_InputField inputField;

	public void Enable(string defaultValue = "")
	{
		base.gameObject.SetActive(value: true);
		inputField.SetTextWithoutNotify(defaultValue);
	}

	public string GetValue()
	{
		return inputField.text;
	}
}
