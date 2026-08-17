using System;
using EvilCore.DI.Core;
using EvilCore.UI.Scripts;
using UnityEngine;
using UnityEngine.Serialization;
using VContainer;

namespace EvilCore.DI.Installers
{
	[Serializable]
	public class GameUIInstaller : MonoInstaller
	{
		[FormerlySerializedAs("guiManagerReference")]
		[SerializeField]
		private GameUIManager gameUIManagerReference;

		[SerializeField]
		private CrosshairPanel crosshairPanelReference;

		[SerializeField]
		private UIFeedbackManager uiFeedbackManagerReference;

		[SerializeField]
		private SavingOverlayPanel savingOverlayPanelReference;

		public override void Install(IContainerBuilder builder)
		{
			RegisterIfNotNull(builder, gameUIManagerReference, delegate(RegistrationBuilder c)
			{
				c.As<IGameUIManager>();
			});
			RegisterIfNotNull(builder, crosshairPanelReference, delegate(RegistrationBuilder c)
			{
				c.As<CrosshairPanel>();
			});
			RegisterIfNotNull(builder, uiFeedbackManagerReference, delegate(RegistrationBuilder c)
			{
				c.As<UIFeedbackManager>();
			});
			RegisterIfNotNull(builder, savingOverlayPanelReference, delegate(RegistrationBuilder c)
			{
				c.As<SavingOverlayPanel>();
			});
		}
	}
}
