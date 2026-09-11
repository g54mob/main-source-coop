using TMPro;
using UnityEngine;

public class PluginNameField : MonoBehaviour
{
	public TMP_InputField inputField;

	public void Enable(string defaultName)
	{
		base.gameObject.SetActive(value: true);
		inputField.SetTextWithoutNotify(defaultName);
	}

	public string GetValue()
	{
		return inputField.text;
	}
}
