using System;
using HeathenEngineering.SteamworksIntegration.API;
using Steamworks;
using UnityEngine;

namespace HeathenEngineering.SteamworksIntegration
{
	[Serializable]
	public struct GameData : IEquatable<AppId_t>, IEquatable<CGameID>, IEquatable<uint>, IEquatable<ulong>, IEquatable<AppData>, IComparable<AppData>, IComparable<AppId_t>, IComparable<uint>, IComparable<ulong>
	{
		[SerializeField]
		private ulong id;

		public static GameData Me => HeathenEngineering.SteamworksIntegration.API.App.Client.Id;

		public readonly CGameID GameId => new CGameID(id);

		public readonly ulong Id => GameId.m_GameID;

		public readonly AppData App => GameId.AppID();

		public readonly bool IsMod => GameId.IsMod();

		public readonly bool IsP2PFile => GameId.IsP2PFile();

		public readonly bool IsShortcut => GameId.IsShortcut();

		public readonly bool IsSteamApp => GameId.IsSteamApp();

		public readonly bool IsValid => GameId.IsValid();

		public readonly uint ModID => GameId.ModID();

		public readonly bool IsMe => this == Me;

		public readonly string Name => App.Name;

		public readonly CGameID.EGameIDType Type => GameId.Type();

		public static bool NamesLoaded => HeathenEngineering.SteamworksIntegration.API.App.Web.IsAppsListLoaded;

		public static GameData Get()
		{
			return Me;
		}

		public static GameData Get(CGameID gameId)
		{
			return gameId;
		}

		public static GameData Get(ulong gameId)
		{
			return gameId;
		}

		public static GameData Get(uint appId)
		{
			return appId;
		}

		public static GameData Get(AppId_t appId)
		{
			return appId;
		}

		public readonly bool GetName(out string name)
		{
			return HeathenEngineering.SteamworksIntegration.API.App.Web.GetAppName(App, out name);
		}

		public readonly void GetName(Action<string, bool> callback)
		{
			HeathenEngineering.SteamworksIntegration.API.App.Web.GetAppName(App, callback);
		}

		public static void LoadNames(Action callback)
		{
			HeathenEngineering.SteamworksIntegration.API.App.Web.LoadAppNames(callback);
		}

		public static void OpenSteamStore(AppData app, EOverlayToStoreFlag flag = EOverlayToStoreFlag.k_EOverlayToStoreFlag_None)
		{
			SteamFriends.ActivateGameOverlayToStore(app, flag);
		}

		public static void OpenMySteamStore(EOverlayToStoreFlag flag = EOverlayToStoreFlag.k_EOverlayToStoreFlag_None)
		{
			SteamFriends.ActivateGameOverlayToStore(Me, flag);
		}

		public readonly int CompareTo(AppData other)
		{
			return App.CompareTo(other.AppId);
		}

		public readonly int CompareTo(GameData other)
		{
			return Id.CompareTo(other.Id);
		}

		public readonly int CompareTo(AppId_t other)
		{
			return App.CompareTo(other);
		}

		public readonly int CompareTo(ulong other)
		{
			return GameId.m_GameID.CompareTo(other);
		}

		public readonly int CompareTo(uint other)
		{
			return App.CompareTo(other);
		}

		public override readonly string ToString()
		{
			return GameId.ToString();
		}

		public readonly bool Equals(AppData other)
		{
			return App.Equals(other.AppId);
		}

		public readonly bool Equals(GameData other)
		{
			return Id.Equals(other.Id);
		}

		public readonly bool Equals(AppId_t other)
		{
			return App.Id.Equals(other);
		}

		public readonly bool Equals(uint other)
		{
			return App.Id.Equals(other);
		}

		public readonly bool Equals(CGameID other)
		{
			return GameId.Equals(other.AppID());
		}

		public readonly bool Equals(ulong other)
		{
			return GameId.Equals(new CGameID(other).AppID());
		}

		public override readonly bool Equals(object obj)
		{
			return GameId.m_GameID.Equals(obj);
		}

		public override readonly int GetHashCode()
		{
			return GameId.GetHashCode();
		}

		public static bool operator ==(GameData l, GameData r)
		{
			return l.GameId.m_GameID == r.GameId.m_GameID;
		}

		public static bool operator ==(AppId_t l, GameData r)
		{
			return l == r.App;
		}

		public static bool operator ==(GameData l, AppId_t r)
		{
			return l.App == r;
		}

		public static bool operator !=(GameData l, GameData r)
		{
			return l.GameId.m_GameID != r.GameId.m_GameID;
		}

		public static bool operator !=(AppId_t l, GameData r)
		{
			return l != r.App;
		}

		public static bool operator !=(GameData l, AppId_t r)
		{
			return l.App != r;
		}

		public static implicit operator GameData(CGameID id)
		{
			return new GameData
			{
				id = id.m_GameID
			};
		}

		public static implicit operator uint(GameData c)
		{
			return c.App.Id;
		}

		public static implicit operator ulong(GameData c)
		{
			return c.Id;
		}

		public static implicit operator GameData(ulong id)
		{
			return new GameData
			{
				id = id
			};
		}

		public static implicit operator GameData(uint id)
		{
			return new GameData
			{
				id = new CGameID(new AppId_t(id)).m_GameID
			};
		}

		public static implicit operator AppId_t(GameData c)
		{
			return c.App;
		}

		public static implicit operator GameData(AppId_t id)
		{
			return new CGameID(id);
		}
	}
}
