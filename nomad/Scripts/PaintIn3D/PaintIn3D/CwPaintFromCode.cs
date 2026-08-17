using UnityEngine;

namespace PaintIn3D
{
	[AddComponentMenu("CW/Paint in 3D/CW Paint From Code")]
	public class CwPaintFromCode : MonoBehaviour
	{
		public CwPaintDecal MyDecal;

		protected virtual void Update()
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hitInfo = default(RaycastHit);
			if (Physics.Raycast(ray, out hitInfo))
			{
				bool preview = !Input.GetKey(KeyCode.Mouse0);
				int priority = 0;
				float pressure = 1f;
				int seed = 0;
				Quaternion rotation = Quaternion.LookRotation(-hitInfo.normal);
				MyDecal.HandleHitPoint(preview, priority, pressure, seed, hitInfo.point, rotation);
			}
		}
	}
}
