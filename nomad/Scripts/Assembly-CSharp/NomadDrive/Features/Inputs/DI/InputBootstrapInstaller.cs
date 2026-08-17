using System;
using EvilCore.DI.Core;
using EvilCore.Inputs;
using UnityEngine;
using VContainer;

namespace NomadDrive.Features.Inputs.DI
{
	[Serializable]
	public class InputBootstrapInstaller : MonoInstaller
	{
		[SerializeField]
		private ActionMapsManager actionMapsManagerReference;

		public override void Install(IContainerBuilder builder)
		{
			RegisterIfNotNull(builder, actionMapsManagerReference, delegate(RegistrationBuilder c)
			{
				c.As<IActionMapsManager>();
			});
			builder.Register<InputMapCoActivationProvider>(Lifetime.Singleton).As<IInputCoActivationProvider>();
		}
	}
}
