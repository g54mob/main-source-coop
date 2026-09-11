using System;
using UnityEngine;

namespace Portningsbolaget.Platforms
{
	[Serializable]
	public class PlatformUserData
	{
		public Sprite m_userProfilePicture;

		public string m_userName;

		public bool m_onlineStatus;

		public Action<PlatformUserData> m_onInvite;

		public object m_customData;
	}
}
