using System;
using EvilCore.DI.Core;
using UnityEngine;
using VContainer;

namespace NomadDrive.Features.OutdoorLighting.DI
{
	[Serializable]
	public class OutdoorLightingInstaller : MonoInstaller
	{
		[SerializeField]
		private OutdoorLightingManager outdoorLightingManagerReference;

		public override void Install(IContainerBuilder builder)
		{
			RegisterIfNotNull(builder, outdoorLightingManagerReference, delegate(RegistrationBuilder c)
			{
				c.As<IOutdoorLightingManager>();
			});
		}
	}
}
