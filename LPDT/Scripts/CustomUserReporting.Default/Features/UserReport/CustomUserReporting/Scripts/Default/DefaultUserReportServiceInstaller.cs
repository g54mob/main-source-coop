using Features.UserReport.Di;
using RSG.Muffin.AssetLoaderModule.Core;
using RSG.Muffin.ZenjectMockableRealizationSubmodule.ZenjectMockableRealizationModule;
using UnityEngine;
using Zenject;

namespace Features.UserReport.CustomUserReporting.Scripts.Default
{
	public class DefaultUserReportServiceInstaller : Installer<DefaultUserReportServiceInstaller>, IUserReportServiceInstaller, IMockableInstaller
	{
		public void CallInstall(DiContainer container)
		{
			Installer<DefaultUserReportServiceInstaller>.Install(container);
		}

		public override async void InstallBindings()
		{
			GameObject prefab = await base.Container.Resolve<IAssetLoaderFacadeService>().LoadAssetAsync<GameObject>("Assets/Features/UserReport/CustomUserReporting/Prefabs/UserReportingPrefab.prefab", AssetLoadSource.Addressables);
			base.Container.Bind<UserReportingObjectMarker>().FromComponentInNewPrefab(prefab).AsSingle()
				.NonLazy();
			base.Container.BindInterfacesTo<UserReportService>().AsSingle();
		}
	}
}
