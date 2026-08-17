using System.Text;
using TMPro;
using UnityEngine;

namespace EvilCore.UI.Scripts
{
	[RequireComponent(typeof(TextMeshProUGUI))]
	public class ProductVersionLabel : MonoBehaviour
	{
		[Header("Target")]
		[SerializeField]
		private TextMeshProUGUI text;

		[Header("Content")]
		[SerializeField]
		private bool showProductName = true;

		[SerializeField]
		private bool showVersion = true;

		[Header("Formatting")]
		[SerializeField]
		private string prefix = "";

		[SerializeField]
		private string separator = " ";

		[SerializeField]
		private string versionPrefix = "";

		[SerializeField]
		private string suffix = "";

		private void Awake()
		{
			if (text == null)
			{
				text = GetComponent<TextMeshProUGUI>();
			}
		}

		private void OnEnable()
		{
			Refresh();
		}

		public void Refresh()
		{
			if (text == null)
			{
				text = GetComponent<TextMeshProUGUI>();
			}
			if (!(text == null))
			{
				text.text = BuildLabel();
			}
		}

		private string BuildLabel()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(prefix);
			if (showProductName)
			{
				stringBuilder.Append(Application.productName);
			}
			if (showProductName && showVersion)
			{
				stringBuilder.Append(separator);
			}
			if (showVersion)
			{
				stringBuilder.Append(versionPrefix);
				stringBuilder.Append(Application.version);
			}
			stringBuilder.Append(suffix);
			return stringBuilder.ToString();
		}
	}
}
