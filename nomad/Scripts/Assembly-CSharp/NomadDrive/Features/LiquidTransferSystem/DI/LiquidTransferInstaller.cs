using System;
using EvilCore.DI.Core;
using NomadDrive.Features.LiquidTransferSystem.UI;
using UnityEngine;
using VContainer;

namespace NomadDrive.Features.LiquidTransferSystem.DI
{
	[Serializable]
	public class LiquidTransferInstaller : MonoInstaller
	{
		[SerializeField]
		private LiquidContainerInfoPanel liquidContainerInfoPanelReference;

		public override void Install(IContainerBuilder builder)
		{
			builder.Register<LiquidTransferProcessorFactory>(Lifetime.Singleton).As<ILiquidTransferProcessorFactory>();
			RegisterIfNotNull(builder, liquidContainerInfoPanelReference, delegate(RegistrationBuilder c)
			{
				c.As<LiquidContainerInfoPanel>();
			});
		}
	}
}
