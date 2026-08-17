using EvilCore.DI.Core;
using UnityEngine;
using VContainer;

namespace EvilCore.Recording.DI
{
	public class DirectorInstaller : MonoInstaller
	{
		[SerializeField]
		private DirectorManager directorManagerReference;

		public override void Install(IContainerBuilder builder)
		{
			RegisterIfNotNull(builder, directorManagerReference);
		}
	}
}
