using UnityEngine;

namespace Rewired
{
	[CustomObfuscation(rename = false)]
	[CustomClassObfuscation(renamePubIntMembers = false, renamePrivateMembers = true)]
	internal struct TouchInfo
	{
		private bool YvpLwpAcGzKqjIAnTYSBmCWbBKgz;

		private int SpOlPfVjIUftIDgfzBfdoHpuhKvIA;

		private Vector2 EsSkaddwGqcsxHnXFUHyxFWITxTT;

		private Vector2 qySRkpbnbtRUjEafEzzbyLBuBndi;

		private Vector2 HyOfLgkdanRMNlgRYEjkbgGWpVqxA;

		private Vector2 mHePycOIpplZSonScMoiZUIcLPJB;

		private float tmZlTwDtJyKyEsvBBoifxBPqXTjR;

		private int kfgApQsbiRKYXDZkwObDAEKKQrIO;

		public bool isValid
		{
			get
			{
				return YvpLwpAcGzKqjIAnTYSBmCWbBKgz;
			}
			internal set
			{
				YvpLwpAcGzKqjIAnTYSBmCWbBKgz = value;
			}
		}

		public int touchId
		{
			get
			{
				return SpOlPfVjIUftIDgfzBfdoHpuhKvIA;
			}
			internal set
			{
				SpOlPfVjIUftIDgfzBfdoHpuhKvIA = value;
			}
		}

		public Vector2 touchPos
		{
			get
			{
				return EsSkaddwGqcsxHnXFUHyxFWITxTT;
			}
			internal set
			{
				EsSkaddwGqcsxHnXFUHyxFWITxTT = value;
			}
		}

		public Vector2 touchPosRaw
		{
			get
			{
				return qySRkpbnbtRUjEafEzzbyLBuBndi;
			}
			internal set
			{
				qySRkpbnbtRUjEafEzzbyLBuBndi = value;
			}
		}

		public Vector2 deltaPos
		{
			get
			{
				return HyOfLgkdanRMNlgRYEjkbgGWpVqxA;
			}
			internal set
			{
				HyOfLgkdanRMNlgRYEjkbgGWpVqxA = value;
			}
		}

		public Vector2 deltaPosRaw
		{
			get
			{
				return mHePycOIpplZSonScMoiZUIcLPJB;
			}
			internal set
			{
				mHePycOIpplZSonScMoiZUIcLPJB = value;
			}
		}

		public float deltaTime
		{
			get
			{
				return tmZlTwDtJyKyEsvBBoifxBPqXTjR;
			}
			internal set
			{
				tmZlTwDtJyKyEsvBBoifxBPqXTjR = value;
			}
		}

		public int tapCount
		{
			get
			{
				return kfgApQsbiRKYXDZkwObDAEKKQrIO;
			}
			internal set
			{
				kfgApQsbiRKYXDZkwObDAEKKQrIO = value;
			}
		}

		internal static TouchInfo Invalid => new TouchInfo
		{
			YvpLwpAcGzKqjIAnTYSBmCWbBKgz = false
		};

		internal TouchInfo(bool P_0, int P_1, Vector2 P_2, Vector2 P_3, Vector2 P_4, Vector2 P_5, float P_6, int P_7)
		{
			YvpLwpAcGzKqjIAnTYSBmCWbBKgz = P_0;
			SpOlPfVjIUftIDgfzBfdoHpuhKvIA = P_1;
			EsSkaddwGqcsxHnXFUHyxFWITxTT = P_2;
			qySRkpbnbtRUjEafEzzbyLBuBndi = P_3;
			HyOfLgkdanRMNlgRYEjkbgGWpVqxA = P_4;
			mHePycOIpplZSonScMoiZUIcLPJB = P_5;
			tmZlTwDtJyKyEsvBBoifxBPqXTjR = P_6;
			kfgApQsbiRKYXDZkwObDAEKKQrIO = P_7;
		}
	}
}
