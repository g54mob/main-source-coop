using System;
using EvilCore.DI.Core;
using NomadDrive.Features.WorldGeneration.Thumbleweed;
using UnityEngine;
using VContainer;

namespace NomadDrive.Features.WorldGeneration.DI
{
	[Serializable]
	public class ThumbleweedInstaller : MonoInstaller
	{
		[SerializeField]
		private ThumbleweedManager thumbleweedManagerReference;

		public override void Install(IContainerBuilder builder)
		{
			RegisterIfNotNull(builder, thumbleweedManagerReference, delegate(RegistrationBuilder c)
			{
				c.As<ThumbleweedManager>();
			});
		}
	}
}
