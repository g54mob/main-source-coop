using System;
using Features.ContextHierarchy.Scripts.GlobalContextScene;
using Infrastructure.Injection.Factories;
using UnityEngine;
using Zenject;

namespace Features.GlobalFactories.Di
{
	public class CustomInjectionInstaller : Installer<CustomInjectionInstaller>
	{
		public override void InstallBindings()
		{
			BindCoreFactories();
		}

		private void BindCoreFactories()
		{
			base.Container.BindFactory<Type, GameObject, Component, InjectedComponentFactory>().FromFactory<CustomInjectedComponentFactory>();
			base.Container.BindFactory<Type, GameObject, Component, SafeInjectedComponentFactory>().FromFactory<SafeCustomInjectedComponentFactory>();
			base.Container.BindFactory<GameObject, Transform, GameObject, InjectedPrefabFactory>().FromFactory<CustomInjectedPrefabFactory>();
		}
	}
}
