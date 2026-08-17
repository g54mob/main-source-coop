using System;
using EvilCore.DI.Core;
using UnityEngine;
using VContainer;

namespace NomadDrive.Features.LiquidDrinking.DI
{
	[Serializable]
	public class LiquidDrinkingInstaller : MonoInstaller
	{
		[SerializeField]
		private LiquidStatEffectsConfig liquidStatEffectsConfigReference;

		public override void Install(IContainerBuilder builder)
		{
			if (liquidStatEffectsConfigReference != null)
			{
				builder.RegisterInstance(liquidStatEffectsConfigReference);
			}
		}
	}
}
