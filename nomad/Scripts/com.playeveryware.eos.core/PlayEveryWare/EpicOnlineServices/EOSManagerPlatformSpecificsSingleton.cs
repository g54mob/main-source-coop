using System;
using UnityEngine;

namespace PlayEveryWare.EpicOnlineServices
{
	public class EOSManagerPlatformSpecificsSingleton
	{
		private static IPlatformSpecifics s_platformSpecifics;

		public static IPlatformSpecifics Instance => s_platformSpecifics;

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void InitOnPlayMode()
		{
			s_platformSpecifics = null;
		}

		public static void SetEOSManagerPlatformSpecificsInterface(IPlatformSpecifics platformSpecifics)
		{
			if (s_platformSpecifics != null)
			{
				throw new Exception(string.Format("Trying to set the EOSManagerPlatformSpecificsSingleton twice: {0} => {1}", s_platformSpecifics.GetType().Name, (platformSpecifics == null) ? "NULL" : platformSpecifics.GetType().Name));
			}
			s_platformSpecifics = platformSpecifics;
		}
	}
}
