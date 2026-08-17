using System.Collections.Generic;
using CW.Common;
using UnityEngine;

namespace PaintCore
{
	[ExecuteInEditMode]
	[HelpURL("https://carloswilkes.com/Documentation/PaintCore#CwChannelCounter")]
	[AddComponentMenu("CW/Paint Core/CW Channel Counter")]
	public class CwChannelCounter : CwPaintableTextureMonitorMask
	{
		public static LinkedList<CwChannelCounter> Instances = new LinkedList<CwChannelCounter>();

		private LinkedListNode<CwChannelCounter> instancesNode;

		[Range(0f, 1f)]
		[SerializeField]
		private float threshold = 0.5f;

		[SerializeField]
		private int countR;

		[SerializeField]
		private int countG;

		[SerializeField]
		private int countB;

		[SerializeField]
		private int countA;

		public float Threshold
		{
			get
			{
				return threshold;
			}
			set
			{
				if (threshold != value)
				{
					threshold = value;
					MarkCurrentReaderAsDirty();
				}
			}
		}

		public int CountR => countR;

		public int CountG => countG;

		public int CountB => countB;

		public int CountA => countA;

		public float RatioR
		{
			get
			{
				if (total <= 0)
				{
					return 0f;
				}
				return (float)countR / (float)total;
			}
		}

		public float RatioG
		{
			get
			{
				if (total <= 0)
				{
					return 0f;
				}
				return (float)countG / (float)total;
			}
		}

		public float RatioB
		{
			get
			{
				if (total <= 0)
				{
					return 0f;
				}
				return (float)countB / (float)total;
			}
		}

		public float RatioA
		{
			get
			{
				if (total <= 0)
				{
					return 0f;
				}
				return (float)countA / (float)total;
			}
		}

		public Vector4 RatioRGBA
		{
			get
			{
				if (total > 0)
				{
					Vector4 result = default(Vector4);
					float num = 1f / (float)total;
					result.x = Mathf.Clamp01((float)countR * num);
					result.y = Mathf.Clamp01((float)countG * num);
					result.z = Mathf.Clamp01((float)countB * num);
					result.w = Mathf.Clamp01((float)countA * num);
					return result;
				}
				return Vector4.zero;
			}
		}

		public bool HasRead
		{
			get
			{
				if (base.MaskReader != null && base.MaskReader.ReadCount > 0 && base.CurrentReader != null)
				{
					return base.CurrentReader.ReadCount > 0;
				}
				return false;
			}
		}

		public static bool GetReady(ICollection<CwChannelCounter> counters = null)
		{
			foreach (CwChannelCounter item in counters ?? Instances)
			{
				if (item != null && !item.HasRead)
				{
					return false;
				}
			}
			return true;
		}

		public static long GetTotal(ICollection<CwChannelCounter> counters = null)
		{
			long num = 0L;
			foreach (CwChannelCounter item in counters ?? Instances)
			{
				if (item != null)
				{
					num += item.total;
				}
			}
			return num;
		}

		public static long GetCountR(ICollection<CwChannelCounter> counters = null)
		{
			long num = 0L;
			foreach (CwChannelCounter item in counters ?? Instances)
			{
				if (item != null)
				{
					num += item.countR;
				}
			}
			return num;
		}

		public static long GetCountG(ICollection<CwChannelCounter> counters = null)
		{
			long num = 0L;
			foreach (CwChannelCounter item in counters ?? Instances)
			{
				if (item != null)
				{
					num += item.countG;
				}
			}
			return num;
		}

		public static long GetCountB(ICollection<CwChannelCounter> counters = null)
		{
			long num = 0L;
			foreach (CwChannelCounter item in counters ?? Instances)
			{
				if (item != null)
				{
					num += item.countB;
				}
			}
			return num;
		}

		public static long GetCountA(ICollection<CwChannelCounter> counters = null)
		{
			long num = 0L;
			foreach (CwChannelCounter item in counters ?? Instances)
			{
				if (item != null)
				{
					num += item.countA;
				}
			}
			return num;
		}

		public static float GetRatioR(ICollection<CwChannelCounter> counters = null)
		{
			return CwHelper.Divide(GetCountR(counters), GetTotal(counters));
		}

		public static float GetRatioG(ICollection<CwChannelCounter> counters = null)
		{
			return CwHelper.Divide(GetCountG(counters), GetTotal(counters));
		}

		public static float GetRatioB(ICollection<CwChannelCounter> counters = null)
		{
			return CwHelper.Divide(GetCountB(counters), GetTotal(counters));
		}

		public static float GetRatioA(ICollection<CwChannelCounter> counters = null)
		{
			return CwHelper.Divide(GetCountA(counters), GetTotal(counters));
		}

		public static Vector4 GetRatioRGBA(ICollection<CwChannelCounter> counters = null)
		{
			if (counters == null)
			{
				counters = Instances;
			}
			if (counters.Count > 0)
			{
				Vector4 zero = Vector4.zero;
				int num = 0;
				foreach (CwChannelCounter counter in counters)
				{
					if (counter != null)
					{
						num++;
						zero.x += counter.RatioR;
						zero.y += counter.RatioG;
						zero.z += counter.RatioB;
						zero.w += counter.RatioA;
					}
				}
				if (num <= 0)
				{
					return zero / num;
				}
				return Vector4.zero;
			}
			return Vector4.zero;
		}

		protected override void OnEnable()
		{
			instancesNode = Instances.AddLast(this);
			base.OnEnable();
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			Instances.Remove(instancesNode);
			instancesNode = null;
		}

		protected override void HandleComplete(int boost)
		{
			if (!currentPixels.IsCreated || !maskPixels.IsCreated || currentPixels.Length != maskPixels.Length)
			{
				return;
			}
			byte b = (byte)(threshold * 255f);
			int num = total;
			countR = 0;
			countG = 0;
			countB = 0;
			countA = 0;
			total = 0;
			for (int i = 0; i < currentPixels.Length; i++)
			{
				if (maskPixels[i] > 127)
				{
					total++;
					Color32 color = currentPixels[i];
					if (color.r >= b)
					{
						countR++;
					}
					if (color.g >= b)
					{
						countG++;
					}
					if (color.b >= b)
					{
						countB++;
					}
					if (color.a >= b)
					{
						countA++;
					}
				}
			}
			countR *= boost;
			countG *= boost;
			countB *= boost;
			countA *= boost;
			total *= boost;
			if (!base.CalculateTotal)
			{
				total = num;
			}
			InvokeOnUpdated();
		}
	}
}
