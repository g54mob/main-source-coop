using System;
using System.Collections.Generic;
using Rewired.Utils;
using Rewired.Utils.Classes.Data;

namespace Rewired.HID
{
	[CustomObfuscation(rename = false)]
	[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
	internal class HIDTouchpad : HIDControllerElement
	{
		[CustomObfuscation(rename = false)]
		[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
		internal class TouchpadInfo
		{
			public int maxTouches;

			public int minX;

			public int maxX;

			public int minY;

			public int maxY;

			public bool invertY;

			public bool reverseY;

			public TouchpadInfo(int P_0, int P_1, int P_2, int P_3, int P_4, bool P_5, bool P_6)
			{
				maxTouches = P_0;
				minX = P_1;
				maxX = P_2;
				minY = P_3;
				maxY = P_4;
				invertY = P_5;
				reverseY = P_6;
			}

			public void CalculateTouch(ref TouchData data)
			{
				int num = (reverseY ? (maxY - data.positionRawY) : data.positionRawY);
				data.positionX = MathTools.ValueInNewRange(data.positionRawX, minX, maxX, 0f, 1f);
				data.positionY = MathTools.ValueInNewRange(num, minY, maxY, 0f, 1f);
				data.positionAbsX = data.positionRawX;
				data.positionAbsY = num;
				if (data.positionAbsX > maxX)
				{
					data.positionAbsX = maxX;
				}
				if (data.positionAbsY > maxY)
				{
					data.positionAbsY = maxY;
				}
				if (data.positionAbsX < minX)
				{
					data.positionAbsX = minX;
				}
				if (data.positionAbsY < minY)
				{
					data.positionAbsY = minY;
				}
				if (invertY)
				{
					data.positionY *= -1f;
					data.positionAbsY *= -1;
				}
			}
		}

		[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
		[CustomObfuscation(rename = false)]
		internal struct TouchData
		{
			public int touchId;

			public float timeStamp;

			public bool isTouching;

			public int positionRawX;

			public int positionRawY;

			public float positionX;

			public float positionY;

			public int positionAbsX;

			public int positionAbsY;

			public void Clear()
			{
				touchId = -1;
				timeStamp = 0f;
				isTouching = false;
				positionRawX = 0;
				positionRawY = 0;
				positionX = 0f;
				positionY = 0f;
				positionAbsX = 0;
				positionAbsY = 0;
			}
		}

		private TouchpadInfo bkgcrrWCRFOtLkjgQGzzaVDQJynbA;

		private Queue<TouchData> vjJobbFPBxnJgBfARdvgGkHYYiOo;

		private TouchData[] WlrJRZSVkpoDpvqaHjjafQFxxvboA;

		private Action<NativeBuffer, TouchData[]> yYsIzhEAunEeadgbIpStWaJjrpBe;

		public TouchData[] values;

		public HIDTouchpad(byte P_0, TouchpadInfo P_1, HIDInfo P_2, Action<NativeBuffer, TouchData[]> P_3)
			: base(P_0, P_2)
		{
			bkgcrrWCRFOtLkjgQGzzaVDQJynbA = P_1;
			yYsIzhEAunEeadgbIpStWaJjrpBe = P_3;
			vjJobbFPBxnJgBfARdvgGkHYYiOo = new Queue<TouchData>(10);
			WlrJRZSVkpoDpvqaHjjafQFxxvboA = new TouchData[P_1.maxTouches];
			values = new TouchData[P_1.maxTouches];
			for (int i = 0; i < values.Length; i++)
			{
				values[i].Clear();
			}
		}

		public override void UpdateValue(NativeBuffer inputReport, double timestamp)
		{
			if (yYsIzhEAunEeadgbIpStWaJjrpBe == null)
			{
				return;
			}
			yYsIzhEAunEeadgbIpStWaJjrpBe(inputReport, WlrJRZSVkpoDpvqaHjjafQFxxvboA);
			lock (vjJobbFPBxnJgBfARdvgGkHYYiOo)
			{
				for (int i = 0; i < bkgcrrWCRFOtLkjgQGzzaVDQJynbA.maxTouches; i++)
				{
					vjJobbFPBxnJgBfARdvgGkHYYiOo.Enqueue(WlrJRZSVkpoDpvqaHjjafQFxxvboA[i]);
				}
			}
			ProcessQueue();
		}

		public void ProcessQueue()
		{
			int num = 0;
			for (int i = 0; i < values.Length; i++)
			{
				values[i].Clear();
			}
			lock (vjJobbFPBxnJgBfARdvgGkHYYiOo)
			{
				int num2 = vjJobbFPBxnJgBfARdvgGkHYYiOo.Count;
				while (num2 > 0)
				{
					TouchData data = vjJobbFPBxnJgBfARdvgGkHYYiOo.Dequeue();
					num2--;
					bkgcrrWCRFOtLkjgQGzzaVDQJynbA.CalculateTouch(ref data);
					values[num] = data;
					num++;
				}
			}
		}

		public bool IsTouching(int touchId)
		{
			for (int i = 0; i < values.Length; i++)
			{
				if (values[i].isTouching && values[i].touchId == touchId)
				{
					return true;
				}
			}
			return false;
		}
	}
}
