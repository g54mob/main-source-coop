using System;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace PaintCore
{
	public abstract class CwPaintableTextureMonitorMask : CwPaintableTextureMonitor
	{
		[FormerlySerializedAs("mesh")]
		[SerializeField]
		private Mesh maskMesh;

		[SerializeField]
		private int maskSubmesh;

		[SerializeField]
		private Texture maskTexture;

		[SerializeField]
		private CwChannel maskChannel = CwChannel.Alpha;

		[SerializeField]
		private bool calculateTotal = true;

		[SerializeField]
		protected int total;

		[NonSerialized]
		private CwReader maskReader;

		[SerializeField]
		protected NativeArray<byte> maskPixels;

		public Mesh MaskMesh
		{
			get
			{
				return maskMesh;
			}
			set
			{
				maskMesh = value;
			}
		}

		public int MaskSubmesh
		{
			get
			{
				return maskSubmesh;
			}
			set
			{
				maskSubmesh = value;
			}
		}

		public Texture MaskTexture
		{
			get
			{
				return maskTexture;
			}
			set
			{
				maskTexture = value;
			}
		}

		public CwChannel MaskChannel
		{
			get
			{
				return maskChannel;
			}
			set
			{
				maskChannel = value;
			}
		}

		public bool CalculateTotal
		{
			get
			{
				return calculateTotal;
			}
			set
			{
				calculateTotal = value;
			}
		}

		public int Total => total;

		public CwReader MaskReader => maskReader;

		public void MarkMaskReaderAsDirty()
		{
			if (maskReader != null)
			{
				maskReader.MarkAsDirty();
			}
		}

		private void HandleCompleteMask(NativeArray<Color32> pixels)
		{
			if (maskPixels.IsCreated && maskPixels.Length != pixels.Length)
			{
				maskPixels.Dispose();
			}
			if (!maskPixels.IsCreated)
			{
				maskPixels = new NativeArray<byte>(pixels.Length, Allocator.Persistent);
			}
			if (maskTexture != null)
			{
				switch (maskChannel)
				{
				case CwChannel.Red:
				{
					for (int l = 0; l < pixels.Length; l++)
					{
						maskPixels[l] = pixels[l].r;
					}
					break;
				}
				case CwChannel.Green:
				{
					for (int j = 0; j < pixels.Length; j++)
					{
						maskPixels[j] = pixels[j].g;
					}
					break;
				}
				case CwChannel.Blue:
				{
					for (int k = 0; k < pixels.Length; k++)
					{
						maskPixels[k] = pixels[k].b;
					}
					break;
				}
				case CwChannel.Alpha:
				{
					for (int i = 0; i < pixels.Length; i++)
					{
						maskPixels[i] = pixels[i].a;
					}
					break;
				}
				}
			}
			else
			{
				for (int m = 0; m < pixels.Length; m++)
				{
					maskPixels[m] = pixels[m].r;
				}
			}
			HandleComplete(maskReader.DownsampleBoost);
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			if (maskReader == null)
			{
				maskReader = new CwReader();
				maskReader.OnComplete += HandleCompleteMask;
			}
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			if (maskReader != null)
			{
				maskReader.OnComplete -= HandleCompleteMask;
				maskReader.Release();
			}
			if (maskPixels.IsCreated)
			{
				maskPixels.Dispose();
			}
		}

		protected override void Start()
		{
			base.Start();
			if (base.CurrentReader.Dirty)
			{
				maskReader.MarkAsDirty();
			}
		}

		protected override void Update()
		{
			base.Update();
			if (!maskReader.Requested && registeredPaintableTexture != null && registeredPaintableTexture.Activated && CwReader.NeedsUpdating(maskReader, maskPixels, registeredPaintableTexture.Current, downsampleSteps))
			{
				RenderTextureDescriptor descriptor = registeredPaintableTexture.Current.descriptor;
				descriptor.useMipMap = false;
				RenderTexture renderTexture = CwCommon.GetRenderTexture(descriptor);
				if (maskTexture != null)
				{
					CwBlit.Blit(renderTexture, CwCommon.GetQuadMesh(), 0, maskTexture, CwCoord.First);
				}
				else
				{
					CwBlit.White(renderTexture, maskMesh, maskSubmesh, registeredPaintableTexture.Coord);
				}
				maskReader.Request(renderTexture, base.DownsampleSteps, base.Async);
				CwCommon.ReleaseRenderTexture(renderTexture);
			}
		}
	}
}
