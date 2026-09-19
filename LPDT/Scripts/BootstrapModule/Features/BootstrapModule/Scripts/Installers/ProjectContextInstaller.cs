using Features.ApplicationFilesRealizationModule.Scripts.Installers;
using Features.AssetLoaderRealization.Scripts.Installers;
using Features.AudioDevicesModule.Scripts;
using Features.AudioServiceModule.Scripts.Installers;
using Features.AudioVolumeModule.Scripts;
using Features.BootstrapModule.Scripts.Systems;
using Features.ChineseDetectionModule.Scripts.Installers;
using Features.CommandLineArguments.Scripts.Installers;
using Features.CoroutineUtils.Scripts.Installers;
using Features.CustomFontAssetCreatorModule.Scripts.Installers;
using Features.CustomNetworkEventsModule.Scripts.Installers;
using Features.GameCycle.Scripts.SessionCleanup;
using Features.GameJournalingModule.Scripts.Core;
using Features.GameUpdaterModule;
using Features.GlobalFactories.Di;
using Features.InputDeviceModuleRealization.Scripts;
using Features.LevelLightModule.Scripts.Installers;
using Features.LevelModule.Scripts.Installers;
using Features.MainMenuModule.Scripts;
using Features.MultiplayerSessionServices.Scripts;
using Features.MultiplayerSessionServices.Scripts.Installers;
using Features.NetworkInputModule.Scripts;
using Features.PhysicsVolumeModule.Scripts;
using Features.PlatformStatusRealizationModule.Scripts.Installers;
using Features.PlayerIdentityModule;
using Features.ProgressSavingModule.Scripts.Implementation;
using Features.SceneManagement.Installers;
using Features.SceneTransitionsModule.Installers;
using Features.SettingsMenuModule.Scripts;
using Features.SteamImplementationModule.Scripts;
using Features.SteamInviteModule.Scripts;
using Features.StoreModule.Scripts.Roster;
using Features.SynchronizedModelsModule.Scripts.DataStreamingSynchronizer.Installers;
using Features.SynchronizedModelsModule.Scripts.JsonModelSynchronizer.Installers;
using Features.UIAnimationsModule.Installers;
using Features.VSyncServiceModule.Scripts;
using Features.ViewSystemModule.Scripts.Installers;
using GameplayEvents;
using Global.Modules.Database_Module.Scripts.Installers;
using Global.Modules.Localization_Module.Scripts.Installers;
using Global.StateMachinesModule.Scripts.Installers;
using RSG.Muffin.DeviceSubmodule.DeviceModule.Scripts;
using RSG.Muffin.JsonConvertorSubmodule.JsonConvertorModule.Core.Implementation.JsonConvertor;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using RSG.Muffin.SavingSubmodule.Samples.MainRealizationExample.Scripts;
using RSG.Muffin.SceneLoaderSubmodule.Samples.MainRealizationExample.Installers;
using UnityEngine;
using Zenject;

namespace Features.BootstrapModule.Scripts.Installers
{
	[CreateAssetMenu(menuName = "Configurations/GameBootstrap/ProjectContextInstaller", fileName = "ProjectContextInstaller_Default", order = 0)]
	public class ProjectContextInstaller : ScriptableObjectInstaller<ProjectContextInstaller>
	{
		public override void InstallBindings()
		{
			Installer<CustomInjectionInstaller>.Install(base.Container);
			Installer<ProjectUIInstaller>.Install(base.Container);
			Installer<StateMachinesInstaller>.Install(base.Container);
			Installer<SceneLoaderServiceModuleInstaller>.Install(base.Container);
			Installer<ViewSystemInstaller>.Install(base.Container);
			Installer<UIByContextInstaller>.Install(base.Container);
			Installer<AssetLoaderInstaller>.Install(base.Container);
			Installer<GameplayEventsInstaller>.Install(base.Container);
			Installer<NetworkSceneLoaderInstaller>.Install(base.Container);
			Installer<DataInstaller>.Install(base.Container);
			base.Container.BindInterfacesAndSelfTo<StoreReadyRosterSystem>().AsSingle();
			Installer<SessionCleanupInstaller>.Install(base.Container);
			base.Container.BindInterfacesTo<CameraModelSessionCleanupSystem>().AsSingle();
			Installer<MultiplayerServicesInstaller>.Install(base.Container);
			Installer<ConfigurationInstaller>.Install(base.Container);
			Installer<SavingSystemInstaller>.Install(base.Container);
			Installer<CommandLineArgumentsInstaller>.Install(base.Container);
			Installer<JsonModelSynchronizationInstaller>.Install(base.Container);
			Installer<NetworkSynchronizerInstaller>.Install(base.Container);
			Installer<DataStreamingModelSynchronizationInstaller>.Install(base.Container);
			Installer<LocalInputModuleInstaller>.Install(base.Container);
			Installer<NetworkInputProviderInstaller>.Install(base.Container);
			Installer<NetworkEventsInstaller>.Install(base.Container);
			Installer<MicrophoneModuleInstaller>.Install(base.Container);
			Installer<SettingsServiceInstaller>.Install(base.Container);
			Installer<VSyncInstaller>.Install(base.Container);
			Installer<FmodAudioServiceInstaller>.Install(base.Container);
			Installer<AudioServiceInstaller>.Install(base.Container);
			Installer<TransitionInstaller>.Install(base.Container);
			Installer<ApplicationFilesRealizationInstaller>.Install(base.Container);
			Installer<InputRealizationInstaller>.Install(base.Container);
			Installer<UIAnimationsInstaller>.Install(base.Container);
			base.Container.BindInterfacesTo<JsonConvertor>().AsSingle();
			Installer<DatabaseInstaller>.Install(base.Container);
			Installer<LocalizationInstaller>.Install(base.Container);
			Installer<CustomFontAssetCreatorInstaller>.Install(base.Container);
			Installer<CoroutineRunnerInstaller>.Install(base.Container);
			Installer<LevelModuleInstaller>.Install(base.Container);
			Installer<LevelLightInstaller>.Install(base.Container);
			Installer<JournalingSystemInstaller>.Install(base.Container);
			Installer<SavingServiceInstaller>.Install(base.Container);
			base.Container.Bind<IGameUpdater>().To<GameUpdater>().FromComponentsInHierarchy()
				.AsSingle();
			base.Container.BindInterfacesTo<SessionJoinGuardService>().AsSingle();
			Installer<SteamRealizationInstaller>.Install(base.Container);
			Installer<SteamInviteModuleInstaller>.Install(base.Container);
			Installer<PlatformStatusRealizationInstaller>.Install(base.Container);
			Installer<DeviceModuleInstaller>.Install(base.Container);
			base.Container.Bind<IPersistentPlayerIdProvider>().FromInstance(new FixedPersistentPlayerIdProvider(new PersistentPlayerId(ResolveLocalPlayerId()))).AsSingle();
			base.Container.Bind<ISessionRecoverySource>().To<PrefsSessionRecoverySource>().AsSingle();
			base.Container.BindInterfacesTo<AutoLanguageDetectionSystem>().AsSingle();
			Installer<ChineseDetectionInstaller>.Install(base.Container);
			base.Container.BindInterfacesTo<PhysicsVolumeUpdateSystem>().AsSingle();
			base.Container.Bind<CargoSimulationClusterService>().AsSingle();
			base.Container.BindInterfacesTo<CargoSimulationClusterInstaller>().AsSingle();
		}

		private static string ResolveLocalPlayerId()
		{
			string deviceUniqueIdentifier = SystemInfo.deviceUniqueIdentifier;
			if (!Application.isEditor || Application.isBatchMode)
			{
				return deviceUniqueIdentifier;
			}
			return deviceUniqueIdentifier + "-" + ShortStableHash(Application.dataPath);
		}

		private static string ShortStableHash(string value)
		{
			uint num = 2166136261u;
			foreach (char c in value)
			{
				num ^= c;
				num *= 16777619;
			}
			return num.ToString("x8");
		}
	}
}
