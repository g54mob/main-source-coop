using PaintCore;
using PaintIn3D;
using UnityEngine;

namespace NomadDrive.Features.Restoration
{
	public class MonoPolishTool : MonoRestorationTool
	{
		[Header("Polish Settings")]
		[SerializeField]
		[Range(1f, 20f)]
		private float _hardness = 10f;

		[SerializeField]
		[Range(0f, 1f)]
		private float _opacity = 0.3f;

		[Header("Target Groups")]
		[SerializeField]
		private int _polishGroupIndex = 203;

		protected override void ConfigurePaintSpheres()
		{
			if (_paintSpheres != null && _paintSpheres.Length >= 1)
			{
				CwPaintSphere cwPaintSphere = _paintSpheres[0];
				if (!(cwPaintSphere == null))
				{
					cwPaintSphere.Group = new CwGroup(_polishGroupIndex);
					cwPaintSphere.Color = Color.white;
					cwPaintSphere.Opacity = _opacity;
					cwPaintSphere.Hardness = _hardness;
					cwPaintSphere.BlendMode = CwBlendMode.Additive(new Vector4(1f, 0f, 0f, 0f));
				}
			}
		}

		protected override void SetupPaintSpheresOnToolTip()
		{
			if (!(_toolTip == null))
			{
				CwPaintSphere cwPaintSphere = CreateOrFindSphere("Sphere_Polish");
				_paintSpheres = new CwPaintSphere[1] { cwPaintSphere };
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
	}
}
