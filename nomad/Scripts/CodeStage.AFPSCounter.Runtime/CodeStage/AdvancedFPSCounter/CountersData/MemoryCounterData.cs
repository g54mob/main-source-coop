using System;
using System.Collections;
using CodeStage.AdvancedFPSCounter.Utils;
using UnityEngine;
using UnityEngine.Profiling;

namespace CodeStage.AdvancedFPSCounter.CountersData
{
	[Serializable]
	public class MemoryCounterData : UpdatableCounterData
	{
		public const long MemoryDivider = 1048576L;

		private const string TextStart = "<color=#{0}>";

		private const string LineStartTotal = "MEM TOTAL: ";

		private const string LineStartAllocated = "MEM ALLOC: ";

		private const string LineStartMono = "MEM MONO: ";

		private const string LineEnd = " MB";

		private const string TextEnd = "</color>";

		[Tooltip("Allows to output memory usage more precisely thus using a bit more system resources.")]
		[SerializeField]
		private bool precise = true;

		[Tooltip("Allows to see private memory amount reserved for application. This memory can’t be used by other applications.")]
		[SerializeField]
		private bool total = true;

		[Tooltip("Allows to see amount of memory, currently allocated by application.")]
		[SerializeField]
		private bool allocated = true;

		[Tooltip("Allows to see amount of memory, allocated by managed Mono objects, such as UnityEngine.Object and everything derived from it for example.")]
		[SerializeField]
		private bool monoUsage;

		[Tooltip("Allows to see amount of allocated memory for the graphics driver (dev builds only).")]
		[SerializeField]
		private bool gfx = true;

		public bool Precise
		{
			get
			{
				return precise;
			}
			set
			{
				if (precise != value && Application.isPlaying)
				{
					precise = value;
					if (enabled)
					{
						Refresh();
					}
				}
			}
		}

		public bool Total
		{
			get
			{
				return total;
			}
			set
			{
				if (total != value && Application.isPlaying)
				{
					total = value;
					if (!total)
					{
						LastTotalValue = 0L;
					}
					if (enabled)
					{
						Refresh();
					}
				}
			}
		}

		public bool Allocated
		{
			get
			{
				return allocated;
			}
			set
			{
				if (allocated != value && Application.isPlaying)
				{
					allocated = value;
					if (!allocated)
					{
						LastAllocatedValue = 0L;
					}
					if (enabled)
					{
						Refresh();
					}
				}
			}
		}

		public bool MonoUsage
		{
			get
			{
				return monoUsage;
			}
			set
			{
				if (monoUsage != value && Application.isPlaying)
				{
					monoUsage = value;
					if (!monoUsage)
					{
						LastMonoValue = 0L;
					}
					if (enabled)
					{
						Refresh();
					}
				}
			}
		}

		public bool Gfx
		{
			get
			{
				return gfx;
			}
			set
			{
				if (gfx != value && Application.isPlaying)
				{
					gfx = value;
					if (!gfx)
					{
						LastGfxValue = 0L;
					}
					if (enabled)
					{
						Refresh();
					}
				}
			}
		}

		public long LastTotalValue { get; private set; }

		public long LastAllocatedValue { get; private set; }

		public long LastMonoValue { get; private set; }

		public long LastGfxValue { get; private set; }

		internal MemoryCounterData()
		{
			color = new Color32(234, 238, 101, 255);
			style = FontStyle.Bold;
		}

		internal override void UpdateValue(bool force)
		{
			if (!enabled)
			{
				return;
			}
			if (force)
			{
				if (!inited && HasData())
				{
					Activate();
					return;
				}
				if (inited && !HasData())
				{
					Deactivate();
					return;
				}
			}
			if (total)
			{
				long totalReservedMemoryLong = Profiler.GetTotalReservedMemoryLong();
				long num = 0L;
				bool flag;
				if (precise)
				{
					flag = LastTotalValue != totalReservedMemoryLong;
				}
				else
				{
					num = totalReservedMemoryLong / 1048576;
					flag = LastTotalValue != num;
				}
				if (flag || force)
				{
					LastTotalValue = (precise ? totalReservedMemoryLong : num);
					dirty = true;
				}
			}
			if (allocated)
			{
				long totalAllocatedMemoryLong = Profiler.GetTotalAllocatedMemoryLong();
				long num2 = 0L;
				bool flag2;
				if (precise)
				{
					flag2 = LastAllocatedValue != totalAllocatedMemoryLong;
				}
				else
				{
					num2 = totalAllocatedMemoryLong / 1048576;
					flag2 = LastAllocatedValue != num2;
				}
				if (flag2 || force)
				{
					LastAllocatedValue = (precise ? totalAllocatedMemoryLong : num2);
					dirty = true;
				}
			}
			if (monoUsage)
			{
				long totalMemory = GC.GetTotalMemory(forceFullCollection: false);
				long num3 = 0L;
				bool flag3;
				if (precise)
				{
					flag3 = LastMonoValue != totalMemory;
				}
				else
				{
					num3 = totalMemory / 1048576;
					flag3 = LastMonoValue != num3;
				}
				if (flag3 || force)
				{
					LastMonoValue = (precise ? totalMemory : num3);
					dirty = true;
				}
			}
			if (!dirty || main.OperationMode != OperationMode.Normal)
			{
				return;
			}
			bool flag4 = false;
			text.Length = 0;
			text.Append(colorCached);
			if (total)
			{
				text.Append("MEM TOTAL: ");
				if (precise)
				{
					text.AppendLookup((float)LastTotalValue / 1048576f);
				}
				else
				{
					text.AppendLookup(LastTotalValue);
				}
				text.Append(" MB");
				flag4 = true;
			}
			if (allocated)
			{
				if (flag4)
				{
					text.Append('\n');
				}
				text.Append("MEM ALLOC: ");
				if (precise)
				{
					text.AppendLookup((float)LastAllocatedValue / 1048576f);
				}
				else
				{
					text.AppendLookup(LastAllocatedValue);
				}
				text.Append(" MB");
				flag4 = true;
			}
			if (monoUsage)
			{
				if (flag4)
				{
					text.Append('\n');
				}
				text.Append("MEM MONO: ");
				if (precise)
				{
					text.AppendLookup((float)LastMonoValue / 1048576f);
				}
				else
				{
					text.AppendLookup(LastMonoValue);
				}
				text.Append(" MB");
			}
			text.Append("</color>");
			ApplyTextStyles();
		}

		protected override void PerformActivationActions()
		{
			base.PerformActivationActions();
			if (!HasData())
			{
				return;
			}
			LastTotalValue = 0L;
			LastAllocatedValue = 0L;
			LastMonoValue = 0L;
			if (main.OperationMode != OperationMode.Normal)
			{
				return;
			}
			if (colorCached == null)
			{
				colorCached = $"<color=#{AFPSCounter.Color32ToHex(color)}>";
			}
			text.Append(colorCached);
			if (total)
			{
				if (precise)
				{
					text.Append("MEM TOTAL: ").Append("0.00").Append(" MB");
				}
				else
				{
					text.Append("MEM TOTAL: ").Append(0).Append(" MB");
				}
			}
			if (allocated)
			{
				if (text.Length > 0)
				{
					text.Append('\n');
				}
				if (precise)
				{
					text.Append("MEM ALLOC: ").Append("0.00").Append(" MB");
				}
				else
				{
					text.Append("MEM ALLOC: ").Append(0).Append(" MB");
				}
			}
			if (monoUsage)
			{
				if (text.Length > 0)
				{
					text.Append('\n');
				}
				if (precise)
				{
					text.Append("MEM MONO: ").Append("0.00").Append(" MB");
				}
				else
				{
					text.Append("MEM MONO: ").Append(0).Append(" MB");
				}
			}
			text.Append("</color>");
			ApplyTextStyles();
			dirty = true;
		}

		protected override void PerformDeActivationActions()
		{
			base.PerformDeActivationActions();
			if (text != null)
			{
				text.Length = 0;
			}
			main.MakeDrawableLabelDirty(anchor);
		}

		protected override IEnumerator UpdateCounter()
		{
			while (true)
			{
				UpdateValue();
				main.UpdateTexts();
				float previousUpdateTime = Time.unscaledTime;
				while (Time.unscaledTime < previousUpdateTime + updateInterval)
				{
					yield return null;
				}
			}
		}

		protected override bool HasData()
		{
			if (!total && !allocated && !monoUsage)
			{
				return gfx;
			}
			return true;
		}

		protected override void CacheCurrentColor()
		{
			colorCached = $"<color=#{AFPSCounter.Color32ToHex(color)}>";
		}
	}
}
