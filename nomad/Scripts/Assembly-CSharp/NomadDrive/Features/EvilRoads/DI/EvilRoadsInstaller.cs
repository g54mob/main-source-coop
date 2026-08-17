using System;
using EvilCore.DI.Core;
using NomadDrive.Features.EvilRoads.Cable;
using UnityEngine;
using VContainer;

namespace NomadDrive.Features.EvilRoads.DI
{
	[Serializable]
	public class EvilRoadsInstaller : MonoInstaller
	{
		[SerializeField]
		private CableManager cableManagerReference;

		public override void Install(IContainerBuilder builder)
		{
			RegisterIfNotNull(builder, cableManagerReference, delegate(RegistrationBuilder c)
			{
				c.As<ICableManager>();
			});
		}
	}
}
