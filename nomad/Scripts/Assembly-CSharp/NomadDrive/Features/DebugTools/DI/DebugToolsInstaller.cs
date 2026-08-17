using System;
using EvilCore.DI.Core;
using UnityEngine;
using VContainer;

namespace NomadDrive.Features.DebugTools.DI
{
	[Serializable]
	public class DebugToolsInstaller : MonoInstaller
	{
		[SerializeField]
		private DeveloperConsole developerConsoleReference;

		public override void Install(IContainerBuilder builder)
		{
			RegisterIfNotNull(builder, developerConsoleReference, delegate(RegistrationBuilder c)
			{
				c.As<DeveloperConsole>();
			});
		}
	}
}
