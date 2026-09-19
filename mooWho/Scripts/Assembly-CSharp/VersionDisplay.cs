using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class VersionDisplay : MonoBehaviour
{
	private void Awake()
	{
		TMP_Text component = GetComponent<TMP_Text>();
		if (component != null)
		{
			component.text = Application.version;
		}
	}
}
