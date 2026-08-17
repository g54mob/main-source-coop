using System;
using EvilCore.EvilPack.EvilLogger;
using EvilCore.Managers;
using Mirror;

namespace EvilCore.DI
{
	public class GameEntryPoint : IGameEntryPoint
	{
		private readonly ISaveManager _saveManager;

		private readonly IGameSaveService _gameSaveService;

		protected GameEntryPoint(ISaveManager saveManager, IGameSaveService gameSaveService)
		{
			_saveManager = saveManager;
			_gameSaveService = gameSaveService;
		}

		public void Initialize()
		{
			try
			{
			}
			catch (Exception ex)
			{
				EvilLogger.LogError("Project Initialization failed: " + ex.Message, "Initialize", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\_Core\\DI\\GameEntryPoint.cs", 43);
			}
		}

		public void OnHostStarted()
		{
			if (NetworkServer.active && NetworkClient.active)
			{
				_gameSaveService?.NotifyHostStarted();
			}
		}

		public void OnClientStarted()
		{
			if (!NetworkServer.active)
			{
				_ = NetworkClient.active;
			}
		}
	}
}
