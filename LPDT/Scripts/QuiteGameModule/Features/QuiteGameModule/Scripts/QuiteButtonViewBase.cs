using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Features.QuiteGameModule.Scripts
{
	public class QuiteButtonViewBase : ViewBehaviour
	{
		[SerializeField]
		private Button _button;

		public Button Button => _button;
	}
}
