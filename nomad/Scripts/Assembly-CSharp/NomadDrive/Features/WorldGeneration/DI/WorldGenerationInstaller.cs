using System;
using EvilCore.DI.Core;
using NomadDrive.Features.WorldGeneration.ObjectSpawning;
using UnityEngine;
using VContainer;

namespace NomadDrive.Features.WorldGeneration.DI
{
	[Serializable]
	public class WorldGenerationInstaller : MonoInstaller
	{
		[SerializeField]
		private WorldGenerator worldGeneratorReference;

		[SerializeField]
		private LootRegistry lootRegistryReference;

		public override void Install(IContainerBuilder builder)
		{
			RegisterIfNotNull(builder, worldGeneratorReference, delegate(RegistrationBuilder c)
			{
				c.As<IWorldGenerator>();
			});
			if (lootRegistryReference != null)
			{
				builder.RegisterInstance(lootRegistryReference);
			}
		}
	}
}
