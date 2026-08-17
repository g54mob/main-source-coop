using System;
using EvilCore.DI.Core;
using UnityEngine;
using VContainer;

namespace NomadDrive.Features.FloatingOrigin.DI
{
	[Serializable]
	public class FloatingOriginInstaller : MonoInstaller
	{
		[SerializeField]
		private FloatingOriginManager floatingOriginManagerReference;

		public override void Install(IContainerBuilder builder)
		{
			RegisterIfNotNull(builder, floatingOriginManagerReference, delegate(RegistrationBuilder c)
			{
				c.As<FloatingOriginManager>();
			});
		}
	}
}
