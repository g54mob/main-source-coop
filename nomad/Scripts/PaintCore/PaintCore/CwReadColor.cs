using System;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace PaintCore
{
	[HelpURL("https://carloswilkes.com/Documentation/PaintCore#CwReadColor")]
	[AddComponentMenu("CW/Paint Core/Hit/CW Read Color")]
	public class CwReadColor : MonoBehaviour, IHitCoord, IHit
	{
		public enum ReadType
		{
			Immediate = 0,
			Async = 1
		}

		[Serializable]
		public class ColorEvent : UnityEvent<Color>
		{
		}

		[SerializeField]
		private CwGroup group;

		[SerializeField]
		private bool preview;

		[SerializeField]
		private ReadType read;

		[SerializeField]
		private Color color;

		[SerializeField]
		private ColorEvent onColor;

		[SerializeField]
		private CwReader reader;

		[SerializeField]
		private RenderTexture buffer;

		public CwGroup Group
		{
			get
			{
				return group;
			}
			set
			{
				group = value;
			}
		}

		public bool Preview
		{
			get
			{
				return preview;
			}
			set
			{
				preview = value;
			}
		}

		public ReadType Read
		{
			get
			{
				return read;
			}
			set
			{
				read = value;
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
				color = value;
			}
		}

		public ColorEvent OnColor
		{
			get
			{
				if (onColor == null)
				{
					onColor = new ColorEvent();
				}
				return onColor;
			}
		}

		public void HandleHitCoord(bool preview, int priority, float pressure, int seed, CwHit hit, Quaternion rotation)
		{
			if (preview && !this.preview)
			{
				return;
			}
			CwModel component = hit.Transform.GetComponent<CwModel>();
			if (!(component != null))
			{
				return;
			}
			List<CwPaintableTexture> list = component.FindPaintableTextures(group);
			for (int num = list.Count - 1; num >= 0; num--)
			{
				CwPaintableTexture cwPaintableTexture = list[num];
				Vector2 coord = cwPaintableTexture.GetCoord(ref hit);
				switch (read)
				{
				case ReadType.Immediate:
					color = CwCommon.GetPixel(cwPaintableTexture.Current, coord);
					if (onColor != null)
					{
						onColor.Invoke(color);
					}
					break;
				case ReadType.Async:
					if (reader == null)
					{
						reader = new CwReader();
						reader.OnComplete += HandleComplete;
					}
					if (buffer == null)
					{
						buffer = new RenderTexture(1, 1, 0);
					}
					if (!reader.Requested)
					{
						float num2 = coord.x * (float)cwPaintableTexture.Current.width;
						float num3 = coord.y * (float)cwPaintableTexture.Current.height;
						Graphics.CopyTexture(cwPaintableTexture.Current, 0, 0, (int)num2, (int)num3, 1, 1, buffer, 0, 0, 0, 0);
						reader.Request(buffer, 0, async: true);
					}
					break;
				}
			}
		}

		protected virtual void OnEnable()
		{
			if (reader != null)
			{
				reader.OnComplete += HandleComplete;
			}
		}

		protected virtual void OnDisable()
		{
			if (reader != null)
			{
				reader.OnComplete -= HandleComplete;
			}
		}

		protected virtual void OnDestroy()
		{
			if (reader != null)
			{
				reader.Release();
			}
		}

		private void HandleComplete(NativeArray<Color32> pixels)
		{
			if (onColor != null)
			{
				onColor.Invoke(pixels[0]);
			}
		}
	}
}
