using System;
using Features.Extensions;
using RSG.Muffin.SteamSubmodule.SteamModule.Scripts.API;
using Steamworks;

namespace Features.PlayerCustomization.Scripts
{
	public class PlayerCustomizationService : IPlayerCustomizationService
	{
		private readonly SteamModel _steamModel;

		public PlayerCustomizationService(SteamModel steamModel)
		{
			_steamModel = steamModel;
		}

		public string GetPlayerDefaultNickName(PlayerCustomizationType customizationType)
		{
			switch (customizationType)
			{
			case PlayerCustomizationType.Default:
				return GetRandomDummyName();
			case PlayerCustomizationType.Steam:
				if (!_steamModel.IsSteamInitialized)
				{
					return GetRandomDummyName();
				}
				return SteamFriends.GetPersonaName();
			default:
				throw new ArgumentException("Unknown player customization type: " + customizationType);
			}
		}

		private string GetRandomDummyName()
		{
			return HubMenuConstants.DummyNames.SelectRandom();
		}
	}
}
