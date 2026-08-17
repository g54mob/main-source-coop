using System;
using System.Collections.Generic;
using CW.Common;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Rendering;

namespace PaintCore
{
	[Serializable]
	public class CwReader
	{
		private enum RequestType
		{
			None = 0,
			Async = 1,
			Syncronous = 2
		}

		[SerializeField]
		private AsyncGPUReadbackRequest readback;

		[SerializeField]
		private int currentLine;

		[SerializeField]
		private NativeArray<Color32> currentColors;

		[SerializeField]
		private bool dirty;

		[SerializeField]
		private RequestType requested;

		[SerializeField]
		private RenderTexture buffer;

		[SerializeField]
		private Vector2Int originalSize;

		[SerializeField]
		private Vector2Int downsampledSize;

		[SerializeField]
		private int downsampleSteps;

		[SerializeField]
		private int downsampleBoost;

		[SerializeField]
		private Texture2D tempTexture;

		[SerializeField]
		private int readCount;

		public static LinkedList<CwReader> Instances = new LinkedList<CwReader>();

		private LinkedListNode<CwReader> node;

		public static List<CwReader> PendingReaders = new List<CwReader>();

		public bool Dirty => dirty;

		public bool Requested => requested != RequestType.None;

		public Vector2Int OriginalSize => originalSize;

		public int DownsampleSteps => downsampleSteps;

		public Vector2Int DownsampledSize => downsampledSize;

		public int DownsampleBoost => downsampleBoost;

		public int ReadCount => readCount;

		public event Action<NativeArray<Color32>> OnComplete;

		public CwReader()
		{
			node = Instances.AddLast(this);
		}

		public void MarkAsDirty()
		{
			dirty = true;
		}

		public void UpdateRequest(ref int pixelBudget)
		{
			if (requested == RequestType.Async)
			{
				if (readback.hasError)
				{
					requested = RequestType.Syncronous;
					currentLine = 0;
				}
				else if (readback.done)
				{
					FinishRequest();
					this.OnComplete(readback.GetData<Color32>());
				}
			}
			if (requested == RequestType.Syncronous && pixelBudget > 0)
			{
				if (!currentColors.IsCreated)
				{
					currentColors = new NativeArray<Color32>(buffer.width * buffer.height, Allocator.Persistent);
				}
				int b = buffer.height - currentLine;
				int num = pixelBudget / buffer.width;
				if (num == 0)
				{
					num = 1;
				}
				ReadLines(Mathf.Min(num, b));
				if (currentLine >= buffer.height)
				{
					FinishRequest();
					this.OnComplete(currentColors);
					currentColors.Dispose();
				}
			}
		}

		private void FinishRequest()
		{
			requested = RequestType.None;
			buffer = CwCommon.ReleaseRenderTexture(buffer);
			readCount++;
			PendingReaders.Remove(this);
		}

		private void ReadLines(int count)
		{
			if (tempTexture == null)
			{
				tempTexture = new Texture2D(1, 1, TextureFormat.RGBA32, mipChain: false);
			}
			tempTexture.Reinitialize(buffer.width, count);
			CwHelper.BeginActive(buffer);
			tempTexture.ReadPixels(new Rect(0f, currentLine, buffer.width, count), 0, 0);
			CwHelper.EndActive();
			tempTexture.Apply();
			NativeArray<Color32> rawTextureData = tempTexture.GetRawTextureData<Color32>();
			int num = buffer.width * currentLine;
			for (int i = 0; i < rawTextureData.Length; i++)
			{
				currentColors[num + i] = rawTextureData[i];
			}
			currentLine += count;
		}

		public static bool NeedsUpdating<T>(CwReader reader, NativeArray<T> array, RenderTexture texture, int downsampleSteps) where T : struct
		{
			if (!array.IsCreated || reader.dirty || reader.DownsampledSize.x * reader.DownsampledSize.y != array.Length)
			{
				return true;
			}
			Vector2Int zero = Vector2Int.zero;
			Vector2Int zero2 = Vector2Int.zero;
			int x = (zero2.x = texture.width);
			zero.x = x;
			x = (zero2.y = texture.height);
			zero.y = x;
			for (int i = 0; i < downsampleSteps; i++)
			{
				if (zero2.x > 2)
				{
					zero2.x /= 2;
				}
				if (zero2.y > 2)
				{
					zero2.y /= 2;
				}
			}
			if (!(reader.OriginalSize != zero))
			{
				return reader.DownsampledSize != zero2;
			}
			return true;
		}

		public void Request(RenderTexture texture, int downsample, bool async)
		{
			if (texture == null)
			{
				Debug.LogError("Texture null.");
				return;
			}
			if (requested != RequestType.None)
			{
				Debug.LogError("Already requested.");
				return;
			}
			if (buffer != null)
			{
				Debug.LogError("Buffer exists.");
				return;
			}
			ref Vector2Int reference = ref originalSize;
			int x = (downsampledSize.x = texture.width);
			reference.x = x;
			ref Vector2Int reference2 = ref originalSize;
			x = (downsampledSize.y = texture.height);
			reference2.y = x;
			for (int i = 0; i < downsample; i++)
			{
				if (downsampledSize.x > 2)
				{
					downsampledSize.x /= 2;
				}
				if (downsampledSize.y > 2)
				{
					downsampledSize.y /= 2;
				}
			}
			downsampleSteps = downsample;
			downsampleBoost = originalSize.x / downsampledSize.x * (originalSize.y / downsampledSize.y);
			RenderTextureDescriptor descriptor = texture.descriptor;
			descriptor.useMipMap = false;
			descriptor.width = downsampledSize.x;
			descriptor.height = downsampledSize.y;
			buffer = CwCommon.GetRenderTexture(descriptor);
			CwCommandReplace.Blit(buffer, texture, Color.white);
			if (async && SystemInfo.supportsAsyncGPUReadback)
			{
				requested = RequestType.Async;
				readback = AsyncGPUReadback.Request(buffer, 0, TextureFormat.RGBA32);
			}
			else
			{
				requested = RequestType.Syncronous;
				currentLine = 0;
			}
			dirty = false;
		}

		public void Release()
		{
			buffer = CwCommon.ReleaseRenderTexture(buffer);
			tempTexture = CwHelper.Destroy(tempTexture);
			Instances.Remove(node);
			node = null;
		}
	}
}
