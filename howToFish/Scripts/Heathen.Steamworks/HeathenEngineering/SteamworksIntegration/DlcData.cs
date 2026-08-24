using System;
using System.IO;
using HeathenEngineering.SteamworksIntegration.API;
using Steamworks;
using UnityEngine;

namespace HeathenEngineering.SteamworksIntegration
{
	[Serializable]
	public struct DlcData : IEquatable<AppId_t>, IEquatable<uint>, IEquatable<AppData>, IComparable<AppData>, IComparable<AppId_t>, IComparable<uint>
	{
		[SerializeField]
		private uint id;

		public readonly AppId_t AppId => new AppId_t(id);

		public readonly uint Id => id;

		public readonly bool Available
		{
			get
			{
				if (App.dlcAppCash.ContainsKey(Id))
				{
					return App.dlcAppCash[Id].available;
				}
				bool result = false;
				int dLCCount = SteamApps.GetDLCCount();
				for (int i = 0; i < dLCCount; i++)
				{
					if (SteamApps.BGetDLCDataByIndex(i, out var pAppID, out var pbAvailable, out var pchName, 512))
					{
						if (App.dlcAppCash.ContainsKey(pAppID.m_AppId))
						{
							App.dlcAppCash[pAppID.m_AppId] = (pchName, pbAvailable);
						}
						else
						{
							App.dlcAppCash.Add(pAppID.m_AppId, (pchName, pbAvailable));
						}
						if (pAppID.m_AppId == id)
						{
							result = pbAvailable;
						}
					}
				}
				return result;
			}
		}

		public readonly string Name
		{
			get
			{
				if (App.dlcAppCash.ContainsKey(Id))
				{
					return App.dlcAppCash[Id].name;
				}
				string result = "None Found";
				int dLCCount = SteamApps.GetDLCCount();
				for (int i = 0; i < dLCCount; i++)
				{
					if (SteamApps.BGetDLCDataByIndex(i, out var pAppID, out var pbAvailable, out var pchName, 512))
					{
						if (App.dlcAppCash.ContainsKey(pAppID.m_AppId))
						{
							App.dlcAppCash[pAppID.m_AppId] = (pchName, pbAvailable);
						}
						else
						{
							App.dlcAppCash.Add(pAppID.m_AppId, (pchName, pbAvailable));
						}
						if (pAppID.m_AppId == id)
						{
							result = pchName;
						}
					}
				}
				return result;
			}
		}

		public readonly bool IsSubscribed => SteamApps.BIsSubscribedApp(this);

		public readonly bool IsInstalled => SteamApps.BIsDlcInstalled(this);

		public readonly DirectoryInfo InstallDirectory
		{
			get
			{
				if (SteamApps.GetAppInstallDir(this, out var pchFolder, 2048u) != 0)
				{
					return new DirectoryInfo(pchFolder.Trim());
				}
				return null;
			}
		}

		public readonly float DownloadProgress
		{
			get
			{
				if (SteamApps.GetDlcDownloadProgress(this, out var punBytesDownloaded, out var punBytesTotal))
				{
					return Convert.ToSingle((double)punBytesDownloaded / (double)punBytesTotal);
				}
				return 0f;
			}
		}

		public readonly DateTime EarliestPurchaseTime
		{
			get
			{
				uint earliestPurchaseUnixTime = SteamApps.GetEarliestPurchaseUnixTime(this);
				return new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(earliestPurchaseUnixTime);
			}
		}

		public readonly void Install()
		{
			SteamApps.InstallDLC(this);
		}

		public readonly void Uninstall()
		{
			SteamApps.UninstallDLC(this);
		}

		public readonly void OpenStore(EOverlayToStoreFlag flag = EOverlayToStoreFlag.k_EOverlayToStoreFlag_None)
		{
			SteamFriends.ActivateGameOverlayToStore(this, flag);
		}

		public DlcData(AppId_t id, bool available, string name)
		{
			this.id = id.m_AppId;
			if (App.dlcAppCash.ContainsKey(id.m_AppId))
			{
				App.dlcAppCash[id.m_AppId] = (name, available);
			}
			else
			{
				App.dlcAppCash.Add(id.m_AppId, (name, available));
			}
		}

		public static DlcData Get(uint appId)
		{
			return appId;
		}

		public static DlcData Get(AppId_t appId)
		{
			return appId;
		}

		public static DlcData Get(AppData appData)
		{
			return appData.AppId;
		}

		public readonly int CompareTo(AppData other)
		{
			return id.CompareTo(other.AppId);
		}

		public readonly int CompareTo(AppId_t other)
		{
			return id.CompareTo(other);
		}

		public readonly int CompareTo(uint other)
		{
			return id.CompareTo(other);
		}

		public readonly bool Equals(uint other)
		{
			return id.Equals(other);
		}

		public override readonly bool Equals(object obj)
		{
			return id.Equals(obj);
		}

		public override readonly int GetHashCode()
		{
			return id.GetHashCode();
		}

		public readonly bool Equals(AppId_t other)
		{
			return id.Equals(other);
		}

		public readonly bool Equals(AppData other)
		{
			return id.Equals(other.AppId);
		}

		public static bool operator ==(DlcData l, DlcData r)
		{
			return l.id == r.id;
		}

		public static bool operator ==(DlcData l, AppData r)
		{
			return l.id == r.Id;
		}

		public static bool operator ==(DlcData l, AppId_t r)
		{
			return l.id == r.m_AppId;
		}

		public static bool operator ==(AppId_t l, DlcData r)
		{
			return l.m_AppId == r.id;
		}

		public static bool operator !=(DlcData l, DlcData r)
		{
			return l.id != r.id;
		}

		public static bool operator !=(DlcData l, AppData r)
		{
			return l.id != r.AppId.m_AppId;
		}

		public static bool operator !=(DlcData l, AppId_t r)
		{
			return l.id != r.m_AppId;
		}

		public static bool operator !=(AppId_t l, DlcData r)
		{
			return l.m_AppId != r.id;
		}

		public static implicit operator uint(DlcData c)
		{
			return c.id;
		}

		public static implicit operator DlcData(uint id)
		{
			return new DlcData
			{
				id = id
			};
		}

		public static implicit operator AppId_t(DlcData c)
		{
			return c.AppId;
		}

		public static implicit operator DlcData(AppId_t id)
		{
			return new DlcData
			{
				id = id.m_AppId
			};
		}

		public static implicit operator DlcData(AppData id)
		{
			return new DlcData
			{
				id = id.Id
			};
		}

		public static implicit operator AppData(DlcData id)
		{
			return AppData.Get(id.id);
		}
	}
}
