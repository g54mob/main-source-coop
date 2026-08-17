using System;
using EvilCore.Audio;
using EvilCore.Audio.BroAdapter;
using EvilCore.CustomPass;
using EvilCore.DI.Core;
using EvilCore.DynamicCasting;
using EvilCore.EvilSave;
using EvilCore.GraphicsQuality;
using EvilCore.Inputs;
using EvilCore.Localization;
using EvilCore.Managers;
using EvilCore.Networking;
using EvilCore.Particles;
using EvilCore.Settings;
using UnityEngine;
using UnityEngine.Serialization;
using VContainer;

namespace EvilCore.DI.Installers
{
	[Serializable]
	public class BootstrapManagersInstaller : MonoInstaller
	{
		[SerializeField]
		private EvilSaveManager evilSaveManagerReference;

		[Header("Audio (BroAudio)")]
		[SerializeField]
		[Tooltip("Active IAudioManager implementation backed by BroAudio.")]
		private BroAudioManager broAudioManagerReference;

		[SerializeField]
		[Tooltip("Mirror RPC layer. Routes through IAudioManager so it stays backend-agnostic.")]
		private NetworkedAudioManager networkedAudioManagerReference;

		[SerializeField]
		private CustomPassManager customPassManagerReference;

		[SerializeField]
		private CastingManager castingManagerReference;

		[Header("Online Services")]
		[SerializeField]
		private EOSAuthManager eosAuthManagerReference;

		[SerializeField]
		private EOSLobbyManager eosLobbyManagerReference;

		[SerializeField]
		private EOSManagerBridge eosManagerBridgeReference;

		[SerializeField]
		private SteamManagerBridge steamManagerBridgeReference;

		[Header("Network")]
		[FormerlySerializedAs("networkManagerReference")]
		[FormerlySerializedAs("customNetworkManagerReference")]
		[SerializeField]
		private MirrorNetworkManager mirrorNetworkManagerReference;

		[SerializeField]
		private NetworkObjectSpawnWatcher networkObjectSpawnWatcherReference;

		[Header("Loading")]
		[SerializeField]
		private GameLoadingManager gameLoadingManagerReference;

		[Header("Graphics")]
		[SerializeField]
		private GraphicsQualityManager graphicsQualityManagerReference;

		[Header("Input")]
		[SerializeField]
		private InputGlyphService inputGlyphServiceReference;

		[SerializeField]
		private InputRemappingService inputRemappingServiceReference;

		[Header("Particles")]
		[SerializeField]
		private ParticlesManager particlesManagerReference;

		[SerializeField]
		private NetworkedParticlesManager networkedParticlesManagerReference;

		[Header("Localization")]
		[SerializeField]
		private LocalizationService localizationServiceReference;

		[Header("Settings")]
		[SerializeField]
		private SettingsManager settingsManagerReference;

		[Header("Managers")]
		[SerializeField]
		private SceneFlowManager sceneFlowManagerReference;

		[SerializeField]
		private GameManager gameManagerReference;

		public override void Install(IContainerBuilder builder)
		{
			RegisterIfNotNull(builder, evilSaveManagerReference, delegate(RegistrationBuilder c)
			{
				c.As<ISaveManager>().As<IEvilSaveManager>();
			});
			RegisterIfNotNull(builder, broAudioManagerReference, delegate(RegistrationBuilder c)
			{
				c.As<IAudioManager>();
			});
			RegisterIfNotNull<NetworkedAudioManager>(builder, networkedAudioManagerReference, (Action<RegistrationBuilder>)delegate(RegistrationBuilder c)
			{
				c.As<INetworkedAudioManager>();
			}, (string)null);
			RegisterIfNotNull<CustomPassManager>(builder, customPassManagerReference, (Action<RegistrationBuilder>)delegate(RegistrationBuilder c)
			{
				c.As<ICustomPassManager>();
			}, (string)null);
			RegisterIfNotNull(builder, castingManagerReference, delegate(RegistrationBuilder c)
			{
				c.As<ICastingManager>();
			});
			RegisterIfNotNull(builder, eosAuthManagerReference, delegate(RegistrationBuilder c)
			{
				c.As<IOnlineService>().As<IAuthService>();
			});
			RegisterIfNotNull(builder, eosLobbyManagerReference, delegate(RegistrationBuilder c)
			{
				c.As<IOnlineService>().As<IEOSLobbyManager>();
			});
			RegisterIfNotNull(builder, eosManagerBridgeReference, delegate(RegistrationBuilder c)
			{
				c.As<IOnlineService>();
			});
			RegisterIfNotNull(builder, steamManagerBridgeReference, delegate(RegistrationBuilder c)
			{
				c.As<IOnlineService>();
			});
			RegisterIfNotNull<MirrorNetworkManager>(builder, mirrorNetworkManagerReference, (Action<RegistrationBuilder>)delegate(RegistrationBuilder c)
			{
				c.As<INetworkManager>();
			}, (string)null);
			RegisterIfNotNull(builder, networkObjectSpawnWatcherReference, delegate(RegistrationBuilder c)
			{
				c.As<INetworkObjectSpawnWatcher>();
			});
			RegisterSingletonAs<IRegionService, RegionService>(builder);
			RegisterIfNotNull(builder, gameLoadingManagerReference, delegate(RegistrationBuilder c)
			{
				c.As<IGameLoadingManager>();
			});
			RegisterIfNotNull(builder, graphicsQualityManagerReference, delegate(RegistrationBuilder c)
			{
				c.As<IGraphicsQualityManager>();
			});
			RegisterIfNotNull(builder, inputGlyphServiceReference, delegate(RegistrationBuilder c)
			{
				c.As<IInputGlyphService>();
			});
			RegisterIfNotNull(builder, inputRemappingServiceReference, delegate(RegistrationBuilder c)
			{
				c.As<IInputRemappingService>();
			});
			RegisterIfNotNull(builder, particlesManagerReference, delegate(RegistrationBuilder c)
			{
				c.As<IParticlesManager>();
			});
			RegisterIfNotNull<NetworkedParticlesManager>(builder, networkedParticlesManagerReference, (Action<RegistrationBuilder>)delegate(RegistrationBuilder c)
			{
				c.As<INetworkedParticlesManager>();
			}, (string)null);
			RegisterIfNotNull(builder, localizationServiceReference, delegate(RegistrationBuilder c)
			{
				c.As<ILocalizationService>();
			});
			RegisterIfNotNull(builder, settingsManagerReference, delegate(RegistrationBuilder c)
			{
				c.As<ISettingsManager>();
			});
			RegisterIfNotNull(builder, sceneFlowManagerReference, delegate(RegistrationBuilder c)
			{
				c.As<ISceneFlowManager>();
			});
			RegisterIfNotNull(builder, gameManagerReference, delegate(RegistrationBuilder c)
			{
				c.As<IGameManager>();
			});
			RegisterSingletonAs<IWorldSeedProvider, WorldSeedProvider>(builder);
			RegisterSingletonAs<INetworkTelemetry, EvilAnalyticsNetworkTelemetry>(builder);
			RegisterSingletonAs<INetworkErrorService, NetworkErrorService>(builder);
		}
	}
}
