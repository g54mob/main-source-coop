using System;
using Unity.Collections;
using UnityEngine;

namespace PaintCore
{
	public abstract class CwPaintableTextureMonitor : MonoBehaviour
	{
		[SerializeField]
		private CwPaintableTexture paintableTexture;

		[SerializeField]
		private bool waitUntilNotPainting;

		[SerializeField]
		private float interval;

		[SerializeField]
		private bool async = true;

		[SerializeField]
		private bool readAtStart = true;

		[SerializeField]
		protected int downsampleSteps = 3;

		[SerializeField]
		protected CwPaintableTexture registeredPaintableTexture;

		[SerializeField]
		private float cooldown;

		[NonSerialized]
		private CwReader currentReader;

		[SerializeField]
		protected NativeArray<Color32> currentPixels;

		public CwPaintableTexture PaintableTexture
		{
			get
			{
				return paintableTexture;
			}
			set
			{
				paintableTexture = value;
				Register();
			}
		}

		public bool WaitUntilNotPainting
		{
			get
			{
				return waitUntilNotPainting;
			}
			set
			{
				waitUntilNotPainting = value;
			}
		}

		public float Interval
		{
			get
			{
				return interval;
			}
			set
			{
				interval = value;
			}
		}

		public bool Async
		{
			get
			{
				return async;
			}
			set
			{
				async = value;
			}
		}

		public bool ReadAtStart
		{
			get
			{
				return readAtStart;
			}
			set
			{
				readAtStart = value;
			}
		}

		public int DownsampleSteps
		{
			get
			{
				return downsampleSteps;
			}
			set
			{
				downsampleSteps = value;
			}
		}

		public bool Registered => registeredPaintableTexture != null;

		public CwReader CurrentReader => currentReader;

		public event Action OnUpdated;

		public void MarkCurrentReaderAsDirty()
		{
			if (currentReader != null)
			{
				currentReader.MarkAsDirty();
			}
		}

		[ContextMenu("Register")]
		public void Register()
		{
			Unregister();
			if (paintableTexture != null)
			{
				paintableTexture.OnModified += HandleModified;
				registeredPaintableTexture = paintableTexture;
			}
		}

		[ContextMenu("Unregister")]
		public void Unregister()
		{
			if (registeredPaintableTexture != null)
			{
				registeredPaintableTexture.OnModified -= HandleModified;
				registeredPaintableTexture = null;
			}
		}

		protected void InvokeOnUpdated()
		{
			if (this.OnUpdated != null)
			{
				this.OnUpdated();
			}
		}

		protected virtual void OnEnable()
		{
			Register();
			if (currentReader == null)
			{
				currentReader = new CwReader();
				currentReader.OnComplete += HandleCompleteCurrent;
			}
		}

		protected virtual void OnDisable()
		{
			Unregister();
		}

		protected virtual void OnDestroy()
		{
			if (currentReader != null)
			{
				currentReader.OnComplete -= HandleCompleteCurrent;
				currentReader.Release();
			}
			if (currentPixels.IsCreated)
			{
				currentPixels.Dispose();
			}
		}

		protected virtual void Start()
		{
			if (readAtStart)
			{
				currentReader.MarkAsDirty();
			}
		}

		protected virtual void Update()
		{
			cooldown -= Time.deltaTime;
			if (currentReader.Dirty)
			{
				bool flag = cooldown <= 0f;
				if (waitUntilNotPainting && CwPaintableManager.IsActivelyPainting)
				{
					flag = false;
				}
				if (flag && !currentReader.Requested && registeredPaintableTexture != null && registeredPaintableTexture.Activated && CwReader.NeedsUpdating(currentReader, currentPixels, registeredPaintableTexture.Current, downsampleSteps))
				{
					cooldown = interval;
					currentReader.Request(registeredPaintableTexture.Current, downsampleSteps, async);
				}
			}
		}

		private void HandleCompleteCurrent(NativeArray<Color32> pixels)
		{
			if (currentPixels.IsCreated && currentPixels.Length != pixels.Length)
			{
				currentPixels.Dispose();
			}
			if (currentPixels.IsCreated)
			{
				NativeArray<Color32>.Copy(pixels, currentPixels);
			}
			else
			{
				currentPixels = new NativeArray<Color32>(pixels, Allocator.Persistent);
			}
			HandleComplete(currentReader.DownsampleBoost);
		}

		private void HandleModified(bool preview)
		{
			if (!preview)
			{
				MarkCurrentReaderAsDirty();
			}
		}

		protected abstract void HandleComplete(int boost);
	}
}
