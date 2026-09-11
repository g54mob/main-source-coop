using UnityEngine;

namespace Portningsbolaget
{
	[CreateAssetMenu(menuName = "Portningsbolaget/Platforms Config", fileName = "PlatformsConfig")]
	public class PlatformsConfig : ScriptableObject
	{
		[Header("Versions")]
		[SerializeField]
		private uint m_major = 1u;

		[SerializeField]
		private uint m_minor = 1u;

		[Header("Patches")]
		[SerializeField]
		private uint m_steam;

		[SerializeField]
		private uint m_xboxApp;

		[SerializeField]
		private uint m_xboxOne;

		[SerializeField]
		private uint m_xboxSeries;

		[SerializeField]
		private uint m_playstation4;

		[SerializeField]
		private uint m_playstation5;

		[SerializeField]
		private uint m_switch1;

		[SerializeField]
		private uint m_switch2;

		[Header("Additional")]
		[SerializeField]
		private uint m_playstation4Master;

		[SerializeField]
		private uint m_playstation5Master;

		[SerializeField]
		private uint m_switch1Release;

		[SerializeField]
		private uint m_switch2Release;

		private static PlatformsConfig s_instance;

		public static PlatformsConfig Instance
		{
			get
			{
				if (s_instance == null)
				{
					s_instance = Resources.Load<PlatformsConfig>("PlatformsConfig");
				}
				return s_instance;
			}
		}

		public string BuildVersion => SteamBuildVersion;

		public string AdditionalVersion => string.Empty;

		public string MatchmakingVersion => $"{m_major}.{m_minor}";

		private string SteamBuildVersion => $"{m_major}.{m_minor}.{m_steam}";

		private string XBoxAppBuildVersion => $"{m_major}.{m_minor}.{m_xboxApp}.0";

		private string XBoxOneBuildVersion => $"{m_major}.{m_minor}.{m_xboxOne}.0";

		private string XBoxSeriesBuildVersion => $"{m_major}.{m_minor}.{m_xboxSeries}.0";

		private string Playstation4BuildVersion => $"{m_major}.{m_minor}.{m_playstation4}";

		private string Playstation4AddedVersion => $"{m_major}.{m_playstation4Master}";

		private string Playstation5BuildVersion => $"{m_major}.{m_minor}.{m_playstation5}";

		private string Playstation5AddedVersion => $"{m_major}.{m_playstation5Master}";

		private string Switch1BuildVersion => $"{m_major}.{m_minor}.{m_switch1}";

		private string Switch1AddedVersion => $"{m_switch1Release}";

		private string Switch2BuildVersion => $"{m_major}.{m_minor}.{m_switch2}";

		private string Switch2AddedVersion => $"{m_switch2Release}";

		public static string GetMatchmakingVersion()
		{
			if (s_instance != null)
			{
				return s_instance.MatchmakingVersion;
			}
			string[] array = Application.version.Split('.');
			return array[0] + "." + array[1];
		}
	}
}
