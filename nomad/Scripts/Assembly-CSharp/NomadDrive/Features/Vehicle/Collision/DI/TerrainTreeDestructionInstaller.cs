using System;
using EvilCore.DI.Core;
using UnityEngine;
using VContainer;

namespace NomadDrive.Features.Vehicle.Collision.DI
{
	[Serializable]
	public class TerrainTreeDestructionInstaller : MonoInstaller
	{
		[SerializeField]
		private TerrainTreeDestructionManager managerReference;

		public override void Install(IContainerBuilder builder)
		{
			RegisterIfNotNull(builder, managerReference, delegate(RegistrationBuilder c)
			{
				c.As<ITerrainTreeDestructionManager>();
			});
		}
	}
}
