using System;
using System.Collections.Generic;
using CW.Common;
using UnityEngine;

namespace PaintCore
{
	[HelpURL("https://carloswilkes.com/Documentation/PaintCore#CwColor")]
	[AddComponentMenu("CW/Paint Core/CW Color")]
	public class CwColor : MonoBehaviour
	{
		[Serializable]
		private class Contribution
		{
			public CwColorCounter Counter;

			public int Solid;
		}

		[SerializeField]
		private Color color;

		[SerializeField]
		private List<Contribution> contributions;

		private static LinkedList<CwColor> instances = new LinkedList<CwColor>();

		private LinkedListNode<CwColor> instancesNode;

		public Color Color
		{
			get
			{
				return color;
			}
			set
			{
				if (color != value)
				{
					color = value;
					CwColorCounter.MarkAllDirty();
				}
			}
		}

		public static LinkedList<CwColor> Instances => instances;

		public int Total
		{
			get
			{
				int num = 0;
				foreach (CwColorCounter instance in CwColorCounter.Instances)
				{
					num += instance.Total;
				}
				return num;
			}
		}

		public int Solid
		{
			get
			{
				int num = 0;
				if (contributions != null)
				{
					for (int num2 = contributions.Count - 1; num2 >= 0; num2--)
					{
						Contribution contribution = contributions[num2];
						if (CwHelper.Enabled(contribution.Counter))
						{
							num += contribution.Solid;
						}
						else
						{
							contributions.RemoveAt(num2);
						}
					}
				}
				return num;
			}
		}

		public float Ratio
		{
			get
			{
				int total = Total;
				if (total > 0)
				{
					return (float)Solid / (float)total;
				}
				return 0f;
			}
		}

		protected virtual void OnEnable()
		{
			instancesNode = instances.AddLast(this);
			CwColorCounter.MarkAllDirty();
		}

		protected virtual void OnDisable()
		{
			instances.Remove(instancesNode);
			instancesNode = null;
			CwColorCounter.MarkAllDirty();
		}

		public void Contribute(CwColorCounter counter, int solid)
		{
			Contribution contribution = null;
			if (!TryGetContribution(counter, ref contribution))
			{
				if (solid <= 0)
				{
					return;
				}
				contribution = new Contribution();
				contributions.Add(contribution);
				contribution.Counter = counter;
			}
			contribution.Solid = solid;
		}

		private bool TryGetContribution(CwColorCounter counter, ref Contribution contribution)
		{
			if (contributions == null)
			{
				contributions = new List<Contribution>();
			}
			for (int num = contributions.Count - 1; num >= 0; num--)
			{
				contribution = contributions[num];
				if (contribution.Counter == counter)
				{
					return true;
				}
			}
			return false;
		}
	}
}
