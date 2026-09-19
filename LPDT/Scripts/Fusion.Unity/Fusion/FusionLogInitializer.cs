using System.Threading;
using Fusion.Photon.Realtime;
using Photon.Client;
using Photon.Realtime;
using UnityEngine;

namespace Fusion
{
	public static class FusionLogInitializer
	{
		private static FusionUnityLogger CreateLogger(bool isDarkMode)
		{
			return new FusionUnityLogger(Thread.CurrentThread, isDarkMode);
		}

		[RuntimeInitializeOnLoadMethod]
		public static void Initialize()
		{
			bool isDarkMode = false;
			LogLevel logLevel = LogLevel.Info;
			TraceChannels traceChannels = (TraceChannels)0;
			InitializePartial(logLevel, traceChannels);
			if (!Log.IsInitialized)
			{
				FusionUnityLogger fusionUnityLogger = CreateLogger(isDarkMode);
				Log.Initialize(logLevel, fusionUnityLogger.CreateLogStream, traceChannels);
			}
		}

		private static void InitializePartial(LogLevel logLevel, TraceChannels traceChannels)
		{
			LogLevel logLevel2 = (traceChannels.HasFlag(TraceChannels.Realtime) ? logLevel : LogLevel.None);
			if (PhotonAppSettings.TryGetGlobal(out var settings))
			{
				FusionAppSettings appSettings = settings.AppSettings;
				FusionAppSettings appSettings2 = settings.AppSettings;
				appSettings.ClientLogging = (appSettings2.NetworkLogging = logLevel2 switch
				{
					LogLevel.Info => global::Photon.Client.LogLevel.Info, 
					LogLevel.Debug => global::Photon.Client.LogLevel.Debug, 
					LogLevel.Warn => global::Photon.Client.LogLevel.Warning, 
					LogLevel.Error => global::Photon.Client.LogLevel.Error, 
					LogLevel.None => global::Photon.Client.LogLevel.Off, 
					_ => global::Photon.Client.LogLevel.Off, 
				});
			}
			global::Photon.Realtime.Log.Init(delegate
			{
			}, delegate
			{
			}, delegate
			{
			}, delegate
			{
			}, delegate
			{
			});
		}
	}
}
