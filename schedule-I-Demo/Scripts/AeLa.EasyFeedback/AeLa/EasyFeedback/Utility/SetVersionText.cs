using AeLa.EasyFeedback.UI;
using UnityEngine;

namespace AeLa.EasyFeedback.Utility
{
	public class SetVersionText : MonoBehaviour
	{
		public string VersionNumber;

		public string Prefix;

		public string Suffix;

		private void Start()
		{
			UIInterop.GetText(base.gameObject).Text = Prefix + VersionNumber + Suffix;
		}
	}
}
