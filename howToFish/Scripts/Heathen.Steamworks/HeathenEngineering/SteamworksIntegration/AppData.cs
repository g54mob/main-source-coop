using System;
using HeathenEngineering.SteamworksIntegration.API;
using Steamworks;
using UnityEngine;

namespace HeathenEngineering.SteamworksIntegration
{
	[Serializable]
	public struct AppData : IEquatable<AppId_t>, IEquatable<CGameID>, IEquatable<uint>, IEquatable<ulong>, IEquatable<AppData>, IComparable<AppData>, IComparable<AppId_t>, IComparable<uint>, IComparable<ulong>
	{
		[SerializeField]
		private uint id;

		public static AppData Me => App.Client.Id;

		public readonly AppId_t AppId => new AppId_t(id);

		public readonly bool IsMe => AppId == App.Client.Id;

		public readonly uint Id => AppId.m_AppId;

		public readonly string Name
		{
			get
			{
				App.Web.GetAppName(AppId, out var name);
				return name;
			}
		}

		public static bool NamesLoaded => App.Web.IsAppsListLoaded;

		public readonly void OpenSteamStore(EOverlayToStoreFlag flag = EOverlayToStoreFlag.k_EOverlayToStoreFlag_None)
		{
			SteamFriends.ActivateGameOverlayToStore(this, flag);
		}

		public static AppData Get()
		{
			return Me;
		}

		public static AppData Get(CGameID gameId)
		{
			return gameId;
		}

		public static AppData Get(ulong gameId)
		{
			return gameId;
		}

		public static AppData Get(uint appId)
		{
			return appId;
		}

		public static AppData Get(AppId_t appId)
		{
			return appId;
		}

		public static AppData Get(DlcData dlcData)
		{
			return dlcData;
		}

		public readonly bool GetName(out string name)
		{
			return App.Web.GetAppName(AppId, out name);
		}

		public readonly void GetName(Action<string, bool> callback)
		{
			App.Web.GetAppName(AppId, callback);
		}

		public static void LoadNames(Action callback)
		{
			App.Web.LoadAppNames(callback);
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
			return AppId.CompareTo(other.AppId);
		}

		public readonly int CompareTo(AppId_t other)
		{
			return AppId.CompareTo(other);
		}

		public readonly int CompareTo(ulong other)
		{
			return AppId.CompareTo(new CGameID(other).AppID());
		}

		public readonly int CompareTo(uint other)
		{
			return AppId.m_AppId.CompareTo(other);
		}

		public override readonly string ToString()
		{
			return AppId.ToString();
		}

		public readonly bool Equals(AppData other)
		{
			return AppId.Equals(other.AppId);
		}

		public readonly bool Equals(AppId_t other)
		{
			return AppId.Equals(other);
		}

		public readonly bool Equals(uint other)
		{
			return AppId.m_AppId.Equals(other);
		}

		public readonly bool Equals(CGameID other)
		{
			return AppId.Equals(other.AppID());
		}

		public readonly bool Equals(ulong other)
		{
			return AppId.Equals(new CGameID(other).AppID());
		}

		public override readonly bool Equals(object obj)
		{
			return AppId.m_AppId.Equals(obj);
		}

		public override readonly int GetHashCode()
		{
			return AppId.GetHashCode();
		}

		public static bool operator ==(AppData l, AppData r)
		{
			return l.AppId == r.AppId;
		}

		public static bool operator ==(AppId_t l, AppData r)
		{
			return l == r.AppId;
		}

		public static bool operator ==(AppData l, AppId_t r)
		{
			return l.AppId == r;
		}

		public static bool operator !=(AppData l, AppData r)
		{
			return l.AppId != r.AppId;
		}

		public static bool operator !=(AppId_t l, AppData r)
		{
			return l != r.AppId;
		}

		public static bool operator !=(AppData l, AppId_t r)
		{
			return l.AppId != r;
		}

		public static implicit operator AppData(CGameID id)
		{
			return new AppData
			{
				id = id.AppID().m_AppId
			};
		}

		public static implicit operator uint(AppData c)
		{
			return c.AppId.m_AppId;
		}

		public static implicit operator AppData(ulong id)
		{
			return new AppData
			{
				id = new CGameID(id).AppID().m_AppId
			};
		}

		public static implicit operator AppData(uint id)
		{
			return new AppData
			{
				id = id
			};
		}

		public static implicit operator AppId_t(AppData c)
		{
			return c.AppId;
		}

		public static implicit operator AppData(AppId_t id)
		{
			return new AppData
			{
				id = id.m_AppId
			};
		}

		public static implicit operator AppData(GameData id)
		{
			return new AppData
			{
				id = id.App.Id
			};
		}
	}
}
