using UnityEngine;

namespace Zorro.Core
{
	public class TextureCurve : ScriptableObject
	{
		[SerializeField]
		private AnimationCurve _red = AnimationCurve.Linear(0f, 0f, 1f, 1f);

		[SerializeField]
		private AnimationCurve _green = AnimationCurve.Linear(0f, 0f, 1f, 1f);

		[SerializeField]
		private AnimationCurve _blue = AnimationCurve.Linear(0f, 0f, 1f, 1f);

		[SerializeField]
		private AnimationCurve _alpha = AnimationCurve.Linear(0f, 0f, 1f, 1f);

		private Texture2D _texture;

		[SerializeField]
		private int _resolution = 512;

		[SerializeField]
		private TextureWrapMode _wrapMode = TextureWrapMode.Clamp;

		[SerializeField]
		private FilterMode _filterMode = FilterMode.Bilinear;

		public Texture2D Texture => _texture;

		public int Resolution => _resolution;

		public void OnEnable()
		{
			if (_texture == null)
			{
				_texture = new Texture2D(_resolution, 1, TextureFormat.ARGB32, mipChain: false, linear: true);
			}
		}

		public Texture Bake()
		{
			if (_texture == null)
			{
				return null;
			}
			if (_texture.width != _resolution)
			{
				_texture.Reinitialize(_resolution, 1);
			}
			_texture.wrapMode = _wrapMode;
			_texture.filterMode = _filterMode;
			Color[] array = new Color[_resolution];
			for (int i = 0; i < _resolution; i++)
			{
				float time = (float)i / (float)_resolution;
				array[i].r = _red.Evaluate(time);
				array[i].g = _green.Evaluate(time);
				array[i].b = _blue.Evaluate(time);
				array[i].a = _alpha.Evaluate(time);
			}
			_texture.SetPixels(array);
			_texture.Apply(updateMipmaps: false);
			return _texture;
		}
	}
}
