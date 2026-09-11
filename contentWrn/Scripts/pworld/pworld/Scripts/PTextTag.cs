using TMPro;
using UnityEngine;

namespace pworld.Scripts
{
	public class PTextTag : MonoBehaviour
	{
		private TextMeshProUGUI text;

		public string Text
		{
			get
			{
				return text.text;
			}
			set
			{
				text.text = value;
			}
		}

		protected void Awake()
		{
			text = GetComponent<TextMeshProUGUI>();
		}
	}
}
