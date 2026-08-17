using EvilCore.DI.Core;
using UnityEngine;
using VContainer;

namespace NomadDrive.Features.ColliderLOD.DI
{
	public class ColliderLODInstaller : MonoInstaller
	{
		[SerializeField]
		private ColliderLODManager colliderLodManagerReference;

		public override void Install(IContainerBuilder builder)
		{
			RegisterIfNotNull(builder, colliderLodManagerReference, delegate(RegistrationBuilder c)
			{
				c.As<IColliderLODManager>();
			});
		}
	}
}
