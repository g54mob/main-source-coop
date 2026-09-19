using Features.AudioDevicesModule.Scripts;
using Features.AudioModule.Scripts;
using Features.BootstrapModule.Scripts.Bootstraps;
using Features.BootstrapModule.Scripts.Systems;
using Features.CollectingModule.Scripts.Installers;
using Features.ConfirmExitPopupService.Scripts;
using Features.CurrencyModule.Scripts.Installers;
using Features.DisconnectHandlerModule.Scripts;
using Features.DisconnectHandlerModule.Scripts.Installers;
using Features.EnumHelpersModule.Scripts;
using Features.ExtendedLogger.Scripts.Installers;
using Features.GoogleFormModule.Scripts;
using Features.ItemCollisionModule.Scripts.Installers;
using Features.NetworkRandomModule.Scripts.Installers;
using Features.NetworkTelemetry.Scripts.Installers;
using Features.ObjectDespawnModule.Installers;
using Features.PhysicsUtilsModule.Scripts.Installers;
using Features.PlayerCustomization.Scripts.Installers;
using Features.PlayerMuteModule.Scripts;
using Features.PlayersPingModule.Scripts;
using Features.RunningSessionModule.Scripts.Installers;
using Features.SelfMicrophonePlayerModule.Installers;
using Features.SettingsMenuModule.Installers;
using Features.SettingsMenuModule.Scripts;
using Features.StrechArmsModule.Scripts.Installers;
using Features.TeleportModule.Scripts.Installers;
using Features.TutorialModule.Scripts.SpecialTagsProcessor;
using Features.TutorialModule.Scripts.TutorialCheckpointSystem;
using Features.TutorialModule.Scripts.TutorialStepsSystem.Di;
using Features.TutorialModule.Scripts.TutorialStepsSystem.OverlayUI.Installers;
using Features.UINavigationModuleRealization.Scripts.Installers;
using Features.UserReport.Di;
using Features.UsersStatsModule.Scripts.Installers;
using Features.ViewSystemModule.Scripts.Installers;
using Features.VoiceControlModule.Scripts;
using Features.VoiceSpeakersModule.Scripts.MimicVoice;
using NetworkServices.InterestManagement.Intsallers;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using RSG.Muffin.ScreenKeyboardSubmodule.ScreenKeyboardModule.Scripts;
using RSG.Muffin.ZenjectMockableRealizationSubmodule.ZenjectMockableRealizationModule;
using UnityEngine;
using Zenject;

namespace Features.BootstrapModule.Scripts.Installers
{
	[CreateAssetMenu(menuName = "Configurations/GameBootstrap/GlobalContextInstaller", fileName = "GlobalContextInstaller_Default", order = 0)]
	public class GlobalContextInstaller : ScriptableObjectInstaller<GlobalContextInstaller>
	{
		public override void InstallBindings()
		{
			Installer<UIByContextInstaller>.Install(base.Container);
			Installer<GlobalUIInstaller>.Install(base.Container);
			base.Container.BindInterfacesTo<StartGameBootstrapSystem>().AsSingle();
			base.Container.BindInterfacesAndSelfTo<MasterOrphanAvatarCleanupSystem>().AsSingle().NonLazy();
			base.Container.BindInterfacesAndSelfTo<PlayerStateLifecycleService>().AsSingle().NonLazy();
			base.Container.BindInterfacesAndSelfTo<PlayerReboundBroadcastSystem>().AsSingle().NonLazy();
			base.Container.BindInterfacesAndSelfTo<PlayerSpawnCoordinationSystem>().AsSingle().NonLazy();
			Installer<GlobalNetworkPrefabInjectionInstaller>.Install(base.Container);
			Installer<NavigationInstaller>.Install(base.Container);
			Installer<SettingsMenuInstallers>.Install(base.Container);
			Installer<RunningSessionInstaller>.Install(base.Container);
			Installer<GameDisconnectHandlerInstaller>.Install(base.Container);
			base.Container.BindInterfacesAndSelfTo<LobbyHostMigrationSystem>().AsSingle().NonLazy();
			Installer<PlayerCustomizationInstaller>.Install(base.Container);
			Installer<CollectingModuleInstaller>.Install(base.Container);
			Installer<CurrencyInstaller>.Install(base.Container);
			Installer<ItemCollisionInstaller>.Install(base.Container);
			Installer<ObjectDespawnInstaller>.Install(base.Container);
			Installer<VoiceControllerInstaller>.Install(base.Container);
			Installer<ExtendedLoggerInstaller>.Install(base.Container);
			base.Container.BindInterfacesAndSelfTo<MouseVisibilityInGameStartSystem>().AsSingle();
			Installer<TeleportInstaller>.Install(base.Container);
			Installer<StretchArmInstaller>.Install(base.Container);
			Installer<PhysicsUtilsInstaller>.Install(base.Container);
			Installer<MusicSystemInstaller>.Install(base.Container);
			Installer<GoogleFormInstaller>.Install(base.Container);
			Installer<InterestManagementInstaller>.Install(base.Container);
			base.Container.BindInterfacesAndSelfTo<FmodSharedMicCapture>().AsSingle();
			base.Container.BindInterfacesAndSelfTo<MimicVoiceArchiveConverter>().AsSingle();
			base.Container.BindInterfacesAndSelfTo<MimicVoiceCaptureService>().AsSingle();
			base.Container.BindInterfacesAndSelfTo<MimicVoicePlaybackService>().AsSingle();
			base.Container.BindInterfacesAndSelfTo<MimicRemoteVoiceCaptureSystem>().AsSingle();
			Installer<SelfMicrophonePlayerModuleInstaller>.Install(base.Container);
			base.Container.BindInterfacesAndSelfTo<MicrophoneSystem>().AsSingle();
			base.Container.BindInterfacesAndSelfTo<MimicLocalVoiceCaptureSystem>().AsSingle();
			base.Container.BindInterfacesAndSelfTo<PushToTalkSystem>().AsSingle();
			base.Container.BindInterfacesAndSelfTo<PlayersVoiceEnabledBroadcastSystem>().AsSingle();
			base.Container.BindInterfacesAndSelfTo<PlayerVoiceActivityBroadcastSystem>().AsSingle();
			base.Container.BindInterfacesAndSelfTo<PlayersPingsSystem>().AsSingle();
			base.Container.BindInterfacesAndSelfTo<SettingsSaveSystem>().AsSingle();
			Installer<PlayersVoiceVolumeSystemInstaller>.Install(base.Container);
			Installer<UserReportInstaller>.Install(base.Container);
			Installer<EnumHelpersInstaller>.Install(base.Container);
			Installer<NetworkTelemetryInstaller>.Install(base.Container);
			Installer<ConfirmExitServiceInstaller>.Install(base.Container);
			Installer<NetworkRandomModuleInstaller>.Install(base.Container);
			base.Container.BindAndInstall<IScreenKeyboardServiceInstaller>();
			Installer<UserStatsInstaller>.Install(base.Container);
			Installer<SpecialTagsProcessorInstaller>.Install(base.Container);
			Installer<TutorialCheckpointInstaller>.Install(base.Container);
			Installer<TutorialSystemsInstaller>.Install(base.Container);
			Installer<OverlayUIInstaller>.Install(base.Container);
		}
	}
}
