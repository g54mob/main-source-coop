using System;
using System.Collections.Generic;
using CW.Common;
using UnityEngine;

namespace PaintCore
{
	[ExecuteInEditMode]
	[HelpURL("https://carloswilkes.com/Documentation/PaintCore#CwColorCounter")]
	[AddComponentMenu("CW/Paint Core/CW Color Counter")]
	public class CwColorCounter : CwPaintableTextureMonitorMask
	{
		public class Contribution
		{
			public CwColor Color;

			public int Count;

			public float Ratio;

			public byte R;

			public byte G;

			public byte B;

			public byte A;

			public static Stack<Contribution> Pool = new Stack<Contribution>();
		}

		public static LinkedList<CwColorCounter> Instances = new LinkedList<CwColorCounter>();

		private LinkedListNode<CwColorCounter> instancesNode;

		[Range(0f, 1f)]
		[SerializeField]
		private float threshold = 0.1f;

		[NonSerialized]
		private List<Contribution> contributions = new List<Contribution>();

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

		public List<Contribution> Contributions => contributions;

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

		public static long GetTotal(ICollection<CwColorCounter> counters = null)
		{
			long num = 0L;
			foreach (CwColorCounter item in counters ?? Instances)
			{
				if (item != null)
				{
					num += item.total;
				}
			}
			return num;
		}

		public static long GetCount(CwColor color, ICollection<CwColorCounter> counters = null)
		{
			long num = 0L;
			foreach (CwColorCounter item in counters ?? Instances)
			{
				if (item != null)
				{
					num += item.Count(color);
				}
			}
			return num;
		}

		public static float GetRatio(CwColor color, ICollection<CwColorCounter> counters = null)
		{
			return CwHelper.Divide(GetCount(color, counters), GetTotal(counters));
		}

		public static bool GetReady(ICollection<CwColorCounter> counters = null)
		{
			foreach (CwColorCounter item in counters ?? Instances)
			{
				if (item != null && !item.HasRead)
				{
					return false;
				}
			}
			return true;
		}

		public int Count(CwColor color)
		{
			foreach (Contribution contribution in contributions)
			{
				if (contribution.Color == color)
				{
					return contribution.Count;
				}
			}
			return 0;
		}

		public float Ratio(CwColor color)
		{
			if (total > 0)
			{
				return (float)Count(color) / (float)total;
			}
			return 0f;
		}

		public static void MarkAllDirty()
		{
			foreach (CwColorCounter instance in Instances)
			{
				instance.MarkCurrentReaderAsDirty();
			}
		}

		protected override void OnEnable()
		{
			instancesNode = Instances.AddLast(this);
			base.OnEnable();
		}

		protected override void OnDisable()
		{
			Instances.Remove(instancesNode);
			instancesNode = null;
			base.OnDisable();
			Contribute(0);
		}

		protected override void HandleComplete(int boost)
		{
			if (!currentPixels.IsCreated || !maskPixels.IsCreated || currentPixels.Length != maskPixels.Length)
			{
				return;
			}
			byte b = (byte)(threshold * 255f);
			int num = total;
			PrepareContributions();
			total = 0;
			for (int i = 0; i < currentPixels.Length; i++)
			{
				if (maskPixels[i] <= 127)
				{
					continue;
				}
				total++;
				Color32 color = currentPixels[i];
				int num2 = -1;
				int num3 = b;
				for (int j = 0; j < CwColor.Instances.Count; j++)
				{
					Contribution contribution = contributions[j];
					int num4 = 0;
					num4 += Math.Abs(contribution.R - color.r);
					num4 += Math.Abs(contribution.G - color.g);
					num4 += Math.Abs(contribution.B - color.b);
					num4 += Math.Abs(contribution.A - color.a);
					if (num4 <= num3)
					{
						num2 = j;
						num3 = num4;
					}
				}
				if (num2 >= 0)
				{
					contributions[num2].Count++;
				}
			}
			total *= boost;
			if (!base.CalculateTotal)
			{
				total = num;
			}
			Contribute(boost);
			InvokeOnUpdated();
		}

		private void ClearContributions()
		{
			for (int num = contributions.Count - 1; num >= 0; num--)
			{
				Contribution.Pool.Push(contributions[num]);
			}
			contributions.Clear();
		}

		private void PrepareContributions()
		{
			ClearContributions();
			foreach (CwColor instance in CwColor.Instances)
			{
				Contribution contribution = ((Contribution.Pool.Count > 0) ? Contribution.Pool.Pop() : new Contribution());
				Color32 color = instance.Color.linear;
				contribution.Color = instance;
				contribution.Count = 0;
				contribution.R = color.r;
				contribution.G = color.g;
				contribution.B = color.b;
				contribution.A = color.a;
				contributions.Add(contribution);
			}
			total = 0;
		}

		private void Contribute(int scale)
		{
			float num = ((total > 0) ? (1f / (float)total) : 1f);
			for (int num2 = contributions.Count - 1; num2 >= 0; num2--)
			{
				Contribution contribution = contributions[num2];
				contribution.Count *= scale;
				contribution.Ratio = (float)contribution.Count * num;
				if (contribution.Color != null)
				{
					contribution.Color.Contribute(this, contribution.Count);
				}
				if (contribution.Count <= 0)
				{
					Contribution.Pool.Push(contribution);
					contributions.RemoveAt(num2);
				}
			}
		}
	}
}
