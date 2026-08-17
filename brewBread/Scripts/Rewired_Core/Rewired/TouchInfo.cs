using UnityEngine;

namespace Rewired
{
	[CustomClassObfuscation(renamePubIntMembers = false, renamePrivateMembers = true)]
	[CustomObfuscation(rename = false)]
	internal struct TouchInfo
	{
		private bool OfwbAYIoZdGfvbuKLWzFcjGDPogfc;

		private int cbhTGwoPawLMNgkrUcThjMKhXMgb;

		private Vector2 YhkGrOwMthZjYyDkmdHlaTJCFbKTA;

		private Vector2 cnALrlPJLorkcHUbsRgUFMSZODoO;

		private Vector2 iOZPrrOYBzNkiNZqjjeGGgikWgcM;

		private Vector2 aPiacfeLWyGcfcTmDjVctdXKnCzq;

		private float cAeWiqWOHSuNdZnxspIafgeRMAnP;

		private int stCBfsysloFbGCawUgXXVZYZftzgA;

		public bool isValid
		{
			get
			{
				return OfwbAYIoZdGfvbuKLWzFcjGDPogfc;
			}
			internal set
			{
				OfwbAYIoZdGfvbuKLWzFcjGDPogfc = value;
			}
		}

		public int touchId
		{
			get
			{
				return cbhTGwoPawLMNgkrUcThjMKhXMgb;
			}
			internal set
			{
				cbhTGwoPawLMNgkrUcThjMKhXMgb = value;
			}
		}

		public Vector2 touchPos
		{
			get
			{
				return YhkGrOwMthZjYyDkmdHlaTJCFbKTA;
			}
			internal set
			{
				YhkGrOwMthZjYyDkmdHlaTJCFbKTA = value;
			}
		}

		public Vector2 touchPosRaw
		{
			get
			{
				return cnALrlPJLorkcHUbsRgUFMSZODoO;
			}
			internal set
			{
				cnALrlPJLorkcHUbsRgUFMSZODoO = value;
			}
		}

		public Vector2 deltaPos
		{
			get
			{
				return iOZPrrOYBzNkiNZqjjeGGgikWgcM;
			}
			internal set
			{
				iOZPrrOYBzNkiNZqjjeGGgikWgcM = value;
			}
		}

		public Vector2 deltaPosRaw
		{
			get
			{
				return aPiacfeLWyGcfcTmDjVctdXKnCzq;
			}
			internal set
			{
				aPiacfeLWyGcfcTmDjVctdXKnCzq = value;
			}
		}

		public float deltaTime
		{
			get
			{
				return cAeWiqWOHSuNdZnxspIafgeRMAnP;
			}
			internal set
			{
				cAeWiqWOHSuNdZnxspIafgeRMAnP = value;
			}
		}

		public int tapCount
		{
			get
			{
				return stCBfsysloFbGCawUgXXVZYZftzgA;
			}
			internal set
			{
				stCBfsysloFbGCawUgXXVZYZftzgA = value;
			}
		}

		internal static TouchInfo Invalid => new TouchInfo
		{
			OfwbAYIoZdGfvbuKLWzFcjGDPogfc = false
		};

		internal TouchInfo(bool P_0, int P_1, Vector2 P_2, Vector2 P_3, Vector2 P_4, Vector2 P_5, float P_6, int P_7)
		{
			OfwbAYIoZdGfvbuKLWzFcjGDPogfc = P_0;
			cbhTGwoPawLMNgkrUcThjMKhXMgb = P_1;
			YhkGrOwMthZjYyDkmdHlaTJCFbKTA = P_2;
			cnALrlPJLorkcHUbsRgUFMSZODoO = P_3;
			iOZPrrOYBzNkiNZqjjeGGgikWgcM = P_4;
			aPiacfeLWyGcfcTmDjVctdXKnCzq = P_5;
			cAeWiqWOHSuNdZnxspIafgeRMAnP = P_6;
			stCBfsysloFbGCawUgXXVZYZftzgA = P_7;
		}
	}
}
