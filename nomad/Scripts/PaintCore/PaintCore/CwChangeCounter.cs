using System;
using System.Collections.Generic;
using CW.Common;
using Unity.Collections;
using UnityEngine;

namespace PaintCore
{
	[ExecuteInEditMode]
	[HelpURL("https://carloswilkes.com/Documentation/PaintCore#CwChangeCounter")]
	[AddComponentMenu("CW/Paint Core/CW Change Counter")]
	public class CwChangeCounter : CwPaintableTextureMonitorMask
	{
		public static LinkedList<CwChangeCounter> Instances = new LinkedList<CwChangeCounter>();

		private LinkedListNode<CwChangeCounter> instancesNode;

		[Range(0f, 1f)]
		[SerializeField]
		private float threshold = 0.1f;

		[SerializeField]
		private Texture texture;

		[SerializeField]
		private Color color = Color.white;

		[SerializeField]
		private int count;

		[NonSerialized]
		private CwReader changeReader;

		[SerializeField]
		protected NativeArray<Color32> changePixels;

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
					MarkChangeReaderAsDirty();
				}
			}
		}

		public Texture Texture
		{
			get
			{
				return texture;
			}
			set
			{
				if (texture != value)
				{
					texture = value;
					MarkChangeReaderAsDirty();
				}
			}
		}

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
					MarkChangeReaderAsDirty();
				}
			}
		}

		public int Count => count;

		public float Ratio
		{
			get
			{
				if (total <= 0)
				{
					return 0f;
				}
				return (float)count / (float)total;
			}
		}

		public CwReader ChangeReader => changeReader;

		public bool HasRead
		{
			get
			{
				if (base.MaskReader != null && base.MaskReader.ReadCount > 0 && base.CurrentReader != null && base.CurrentReader.ReadCount > 0 && changeReader != null)
				{
					return changeReader.ReadCount > 0;
				}
				return false;
			}
		}

		public void MarkChangeReaderAsDirty()
		{
			if (changeReader != null)
			{
				changeReader.MarkAsDirty();
			}
		}

		public static long GetTotal(ICollection<CwChangeCounter> counters = null)
		{
			long num = 0L;
			foreach (CwChangeCounter item in counters ?? Instances)
			{
				if (item != null)
				{
					num += item.total;
				}
			}
			return num;
		}

		public static long GetCount(ICollection<CwChangeCounter> counters = null)
		{
			long num = 0L;
			foreach (CwChangeCounter item in counters ?? Instances)
			{
				if (item != null)
				{
					num += item.count;
				}
			}
			return num;
		}

		public static float GetRatio(ICollection<CwChangeCounter> counters = null)
		{
			return CwHelper.Divide(GetCount(counters), GetTotal(counters));
		}

		public static bool GetReady(ICollection<CwChangeCounter> counters = null)
		{
			foreach (CwChangeCounter item in counters ?? Instances)
			{
				if (item != null && !item.HasRead)
				{
					return false;
				}
			}
			return true;
		}

		private void HandleCompleteChange(NativeArray<Color32> pixels)
		{
			if (changePixels.IsCreated && changePixels.Length != pixels.Length)
			{
				changePixels.Dispose();
			}
			if (!changePixels.IsCreated)
			{
				changePixels = new NativeArray<Color32>(pixels.Length, Allocator.Persistent);
			}
			if (changePixels.IsCreated)
			{
				NativeArray<Color32>.Copy(pixels, changePixels);
			}
			else
			{
				changePixels = new NativeArray<Color32>(pixels, Allocator.Persistent);
			}
			HandleComplete(changeReader.DownsampleBoost);
		}

		protected override void HandleComplete(int boost)
		{
			if (!currentPixels.IsCreated || !maskPixels.IsCreated || !changePixels.IsCreated || currentPixels.Length != maskPixels.Length || currentPixels.Length != changePixels.Length)
			{
				return;
			}
			byte b = (byte)(threshold * 255f);
			int num = total;
			count = 0;
			total = 0;
			for (int i = 0; i < currentPixels.Length; i++)
			{
				if (maskPixels[i] > 127)
				{
					total++;
					Color32 color = currentPixels[i];
					Color32 color2 = changePixels[i];
					if (0 + Math.Abs(color2.r - color.r) + Math.Abs(color2.g - color.g) + Math.Abs(color2.b - color.b) + Math.Abs(color2.a - color.a) > b)
					{
						count++;
					}
				}
			}
			total *= boost;
			count *= boost;
			if (!base.CalculateTotal)
			{
				total = num;
			}
			InvokeOnUpdated();
		}

		protected override void OnEnable()
		{
			instancesNode = Instances.AddLast(this);
			base.OnEnable();
			if (changeReader == null)
			{
				changeReader = new CwReader();
				changeReader.OnComplete += HandleCompleteChange;
			}
		}

		protected override void OnDisable()
		{
			Instances.Remove(instancesNode);
			instancesNode = null;
			base.OnDisable();
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			if (changeReader != null)
			{
				changeReader.OnComplete -= HandleCompleteChange;
				changeReader.Release();
			}
			if (changePixels.IsCreated)
			{
				changePixels.Dispose();
			}
		}

		protected override void Start()
		{
			base.Start();
			if (base.MaskReader.Dirty)
			{
				changeReader.MarkAsDirty();
			}
		}

		protected override void Update()
		{
			base.Update();
			if (!changeReader.Requested && registeredPaintableTexture != null && registeredPaintableTexture.Activated && CwReader.NeedsUpdating(changeReader, changePixels, registeredPaintableTexture.Current, downsampleSteps))
			{
				RenderTextureDescriptor descriptor = registeredPaintableTexture.Current.descriptor;
				descriptor.useMipMap = false;
				RenderTexture renderTexture = CwCommon.GetRenderTexture(descriptor);
				CwCommandReplace.Blit(renderTexture, texture, color);
				changeReader.Request(renderTexture, base.DownsampleSteps, base.Async);
				CwCommon.ReleaseRenderTexture(renderTexture);
			}
		}
	}
}
