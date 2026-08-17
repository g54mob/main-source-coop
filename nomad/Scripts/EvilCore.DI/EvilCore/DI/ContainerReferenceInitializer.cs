using EvilCore.Extensions;
using UnityEngine;
using VContainer;

namespace EvilCore.DI
{
	public class ContainerReferenceInitializer : MonoBehaviour
	{
		[Inject]
		public void Initialize(IObjectResolver container)
		{
			GameObjectExtensions.SetContainerReference(container);
		}
	}
}
