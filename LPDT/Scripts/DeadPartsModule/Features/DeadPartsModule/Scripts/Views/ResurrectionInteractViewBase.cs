using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using TMPro;
using UnityEngine;

namespace Features.DeadPartsModule.Scripts.Views
{
	public class ResurrectionInteractViewBase : ViewBehaviour
	{
		[field: SerializeField]
		public TMP_Text ResurrectionHintText { get; protected set; }
	}
}
