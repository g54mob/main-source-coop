using PaintCore;
using PaintIn3D;
using UnityEngine;

namespace NomadDrive.Features.Restoration
{
	public class MonoCleanTool : MonoRestorationTool
	{
		[Header("Clean Settings")]
		[SerializeField]
		[Range(1f, 20f)]
		private float _hardness = 5f;

		[SerializeField]
		[Range(0f, 1f)]
		private float _opacity = 0.25f;

		[Header("Rust Blur Effect")]
		[Tooltip("Enable soft blur effect on rust when cleaning")]
		[SerializeField]
		private bool _enableRustBlur = true;

		[Tooltip("Blur kernel size in pixels - higher = more spread")]
		[SerializeField]
		[Range(1f, 8f)]
		private float _rustBlurKernel = 2f;

		[Tooltip("Blur effect opacity - how much blur is applied per stroke")]
		[SerializeField]
		[Range(0f, 1f)]
		private float _rustBlurOpacity = 0.15f;

		[Tooltip("Blur sphere hardness - softer edges for more natural effect")]
		[SerializeField]
		[Range(0.5f, 10f)]
		private float _rustBlurHardness = 3f;

		[Header("Target Groups")]
		[SerializeField]
		private int _dirtGroupIndex = 201;

		[SerializeField]
		private int _rustGroupIndex = 202;

		private CwPaintSphere _dirtSphere;

		private CwPaintSphere _rustBlurSphere;

		protected override float GetBaseOpacity()
		{
			return _opacity;
		}

		protected override void ConfigurePaintSpheres()
		{
			if (_paintSpheres == null || _paintSpheres.Length < 1)
			{
				return;
			}
			_dirtSphere = _paintSpheres[0];
			if (_dirtSphere != null)
			{
				_dirtSphere.Group = new CwGroup(_dirtGroupIndex);
				_dirtSphere.Color = Color.white;
				_dirtSphere.Opacity = _opacity;
				_dirtSphere.Hardness = _hardness;
				_dirtSphere.BlendMode = CwBlendMode.Subtractive(new Vector4(1f, 0f, 0f, 0f));
			}
			if (_enableRustBlur && _paintSpheres.Length >= 2)
			{
				_rustBlurSphere = _paintSpheres[1];
				if (_rustBlurSphere != null)
				{
					_rustBlurSphere.Group = new CwGroup(_rustGroupIndex);
					_rustBlurSphere.Color = Color.white;
					_rustBlurSphere.Opacity = _rustBlurOpacity;
					_rustBlurSphere.Hardness = _rustBlurHardness;
					_rustBlurSphere.BlendMode = CwBlendMode.Blur(_rustBlurKernel, new Vector4(1f, 0f, 0f, 0f));
				}
			}
		}

		protected override void SetupPaintSpheresOnToolTip()
		{
			if (!(_toolTip == null))
			{
				CwPaintSphere cwPaintSphere = CreateOrFindSphere("Sphere_Clean");
				if (_enableRustBlur)
				{
					CwPaintSphere cwPaintSphere2 = CreateOrFindSphere("Sphere_RustBlur");
					_paintSpheres = new CwPaintSphere[2] { cwPaintSphere, cwPaintSphere2 };
				}
				else
				{
					_paintSpheres = new CwPaintSphere[1] { cwPaintSphere };
				}
				ConfigurePaintSpheres();
			}
		}

		private CwPaintSphere CreateOrFindSphere(string name)
		{
			Transform transform = _toolTip.Find(name);
			if (transform != null)
			{
				return transform.GetComponent<CwPaintSphere>() ?? transform.gameObject.AddComponent<CwPaintSphere>();
			}
			GameObject obj = new GameObject(name);
			obj.transform.SetParent(_toolTip);
			obj.transform.localPosition = Vector3.zero;
			return obj.AddComponent<CwPaintSphere>();
		}

		public void SetRustBlurKernel(float kernel)
		{
			_rustBlurKernel = Mathf.Clamp(kernel, 1f, 8f);
			if (_rustBlurSphere != null)
			{
				_rustBlurSphere.BlendMode = CwBlendMode.Blur(_rustBlurKernel, new Vector4(1f, 0f, 0f, 0f));
			}
		}

		public void SetRustBlurEnabled(bool enabled)
		{
			_enableRustBlur = enabled;
			if (_rustBlurSphere != null)
			{
				_rustBlurSphere.enabled = enabled;
			}
		}
	}
}
