using PaintCore;
using PaintIn3D;
using UnityEngine;

namespace NomadDrive.Features.Restoration
{
	public class MonoGrinderTool : MonoRestorationTool
	{
		[Header("Grinder Settings")]
		[SerializeField]
		[Range(1f, 30f)]
		private float _hardness = 12f;

		[SerializeField]
		[Range(0f, 1f)]
		private float _opacity = 0.7f;

		[Header("Target Groups")]
		[SerializeField]
		private int _dirtGroupIndex = 201;

		[SerializeField]
		private int _rustGroupIndex = 202;

		[SerializeField]
		private int _polishGroupIndex = 203;

		[SerializeField]
		private int _paintMaskGroupIndex = 204;

		[SerializeField]
		private int _metalMaskGroupIndex = 205;

		protected override float GetBaseOpacity()
		{
			return _opacity;
		}

		protected override void ConfigurePaintSpheres()
		{
			if (_paintSpheres != null && _paintSpheres.Length >= 5)
			{
				ConfigureSphere(_paintSpheres[0], _dirtGroupIndex);
				ConfigureSphere(_paintSpheres[1], _rustGroupIndex);
				ConfigureSphere(_paintSpheres[2], _paintMaskGroupIndex);
				ConfigureSphere(_paintSpheres[3], _polishGroupIndex);
				ConfigureSphere(_paintSpheres[4], _metalMaskGroupIndex);
			}
		}

		private void ConfigureSphere(CwPaintSphere sphere, int groupIndex)
		{
			if (!(sphere == null))
			{
				sphere.Group = new CwGroup(groupIndex);
				sphere.Color = Color.black;
				sphere.Opacity = _opacity;
				sphere.Hardness = _hardness;
				sphere.BlendMode = CwBlendMode.Replace(new Vector4(1f, 0f, 0f, 0f));
			}
		}

		protected override void SetupPaintSpheresOnToolTip()
		{
			if (!(_toolTip == null))
			{
				CwPaintSphere cwPaintSphere = CreateOrFindSphere("Sphere_Dirt");
				CwPaintSphere cwPaintSphere2 = CreateOrFindSphere("Sphere_Rust");
				CwPaintSphere cwPaintSphere3 = CreateOrFindSphere("Sphere_Paint");
				CwPaintSphere cwPaintSphere4 = CreateOrFindSphere("Sphere_Polish");
				CwPaintSphere cwPaintSphere5 = CreateOrFindSphere("Sphere_Metal");
				_paintSpheres = new CwPaintSphere[5] { cwPaintSphere, cwPaintSphere2, cwPaintSphere3, cwPaintSphere4, cwPaintSphere5 };
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
