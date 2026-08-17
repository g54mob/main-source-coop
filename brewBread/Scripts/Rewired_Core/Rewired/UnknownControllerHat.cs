using Rewired.Utils;

namespace Rewired
{
	[CustomClassObfuscation(renamePubIntMembers = false, renamePrivateMembers = true)]
	[CustomObfuscation(rename = false)]
	internal class UnknownControllerHat
	{
		[CustomObfuscation(rename = false)]
		[CustomClassObfuscation(renamePubIntMembers = false, renamePrivateMembers = true)]
		public class HatButtons
		{
			private int[] rgghiTwPYQtsKoFgKRoUtPldWus;

			public int this[int index] => rgghiTwPYQtsKoFgKRoUtPldWus[index];

			public HatButtons(int[] P_0)
			{
				rgghiTwPYQtsKoFgKRoUtPldWus = P_0;
			}

			public void GetNeighbors(int button, out int neighbor1, out int neighbor2)
			{
				int num = IndexOf(button);
				if (num < 0)
				{
					neighbor1 = -1;
					neighbor2 = -1;
					return;
				}
				if (num > 0)
				{
					neighbor1 = rgghiTwPYQtsKoFgKRoUtPldWus[num - 1];
				}
				else
				{
					neighbor1 = rgghiTwPYQtsKoFgKRoUtPldWus[rgghiTwPYQtsKoFgKRoUtPldWus.Length - 1];
				}
				if (num >= rgghiTwPYQtsKoFgKRoUtPldWus.Length - 1)
				{
					neighbor2 = rgghiTwPYQtsKoFgKRoUtPldWus[0];
				}
				else
				{
					neighbor2 = rgghiTwPYQtsKoFgKRoUtPldWus[num + 1];
				}
			}

			public bool IsCardinal(int button)
			{
				int num = IndexOf(button);
				if (num < 0)
				{
					return false;
				}
				return MathTools.IsEven(num);
			}

			public bool IsCorner(int button)
			{
				int num = IndexOf(button);
				if (num < 0)
				{
					return false;
				}
				return !MathTools.IsEven(num);
			}

			public int IndexOf(int button)
			{
				for (int i = 0; i < rgghiTwPYQtsKoFgKRoUtPldWus.Length; i++)
				{
					if (rgghiTwPYQtsKoFgKRoUtPldWus[i] == button)
					{
						return i;
					}
				}
				return -1;
			}

			public bool Contains(int button)
			{
				return IndexOf(button) >= 0;
			}
		}

		private HatButtons rgghiTwPYQtsKoFgKRoUtPldWus;

		public UnknownControllerHat(HatButtons P_0)
		{
			rgghiTwPYQtsKoFgKRoUtPldWus = P_0;
		}

		public bool ContainsButtonIndex(int index)
		{
			for (int i = 0; i < 8; i++)
			{
				if (rgghiTwPYQtsKoFgKRoUtPldWus.Contains(index))
				{
					return true;
				}
			}
			return false;
		}

		public bool IsButtonIndexCardinal(int index)
		{
			for (int i = 0; i < 8; i++)
			{
				if (rgghiTwPYQtsKoFgKRoUtPldWus.IsCardinal(index))
				{
					return true;
				}
			}
			return false;
		}

		public HatButtons GetButtons()
		{
			return rgghiTwPYQtsKoFgKRoUtPldWus;
		}
	}
}
