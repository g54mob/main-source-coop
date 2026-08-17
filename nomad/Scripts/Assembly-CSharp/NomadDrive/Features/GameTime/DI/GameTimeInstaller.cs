using System;
using EvilCore.DI.Core;
using NomadDrive.Managers.GameTime;
using UnityEngine;
using VContainer;

namespace NomadDrive.Features.GameTime.DI
{
	[Serializable]
	public class GameTimeInstaller : MonoInstaller
	{
		[SerializeField]
		private BasicTimeManager timeManagerReference;

		public override void Install(IContainerBuilder builder)
		{
			RegisterIfNotNull(builder, timeManagerReference, delegate(RegistrationBuilder c)
			{
				c.As<ITimeManager>();
			});
		}
	}
}
