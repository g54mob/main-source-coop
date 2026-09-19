using TMPro;
using UnityEngine;

namespace solidocean
{
	public class HintManager : MonoBehaviour
	{
		[Header("Settings")]
		[Tooltip("Add your game hints, tips and tricks here to show them on the loading screen.")]
		public string[] hints;

		[Header("References")]
		[SerializeField]
		private TextMeshProUGUI hintText;

		private void Start()
		{
			hintText = GetComponent<TextMeshProUGUI>();
		}

		private void OnEnable()
		{
			string text = hints[Random.Range(0, hints.Length)];
			hintText.text = text;
		}
	}
}
