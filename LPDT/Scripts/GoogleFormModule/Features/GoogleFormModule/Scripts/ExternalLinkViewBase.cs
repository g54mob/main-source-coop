using System;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using UnityEngine;

namespace Features.GoogleFormModule.Scripts
{
	public class ExternalLinkViewBase : ViewBehaviour
	{
		[field: SerializeField]
		public ExternalLinkSource ExternalLinkSource { get; private set; }

		public event Action<ExternalLinkType> OnFeedbackButtonClick;

		protected void InvokeExternalLinkButtonClick(ExternalLinkType externalLinkType)
		{
			this.OnFeedbackButtonClick?.Invoke(externalLinkType);
		}
	}
}
