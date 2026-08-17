using System;
using UnityEngine;

namespace GameKit.Utilities.Types
{
	public class DDOL : MonoBehaviour
	{
		private static DDOL _instance;

		[Obsolete("Use GetDDOL().")]
		public static DDOL Instance => GetDDOL();

		public static DDOL GetDDOL()
		{
			if (_instance == null)
			{
				DDOL dDOL = new GameObject
				{
					name = "FirstGearGames DDOL"
				}.AddComponent<DDOL>();
				UnityEngine.Object.DontDestroyOnLoad(dDOL);
				_instance = dDOL;
				return dDOL;
			}
			return _instance;
		}
	}
}
