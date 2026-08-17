using UnityEngine;

namespace Enviro
{
	[ExecuteInEditMode]
	[AddComponentMenu("Enviro 3/Effect Removal Zone")]
	public class EnviroEffectRemovalZone : MonoBehaviour
	{
		public enum Mode
		{
			Spherical = 0,
			Cubical = 1
		}

		public Mode type;

		[Range(-10f, 0f)]
		public float density = -10f;

		public float radius = 1f;

		public float stretch = 2f;

		[Range(0f, 1f)]
		public float feather = 0.7f;

		public Vector3 size = Vector3.one * 10f;

		private void OnEnable()
		{
			if (EnviroManager.instance != null)
			{
				AddToZoneToManager();
			}
		}

		private void OnDisable()
		{
			if (EnviroManager.instance != null)
			{
				RemoveZoneFromManager();
			}
		}

		private void OnDestroy()
		{
			if (EnviroManager.instance != null)
			{
				RemoveZoneFromManager();
			}
		}

		private void AddToZoneToManager()
		{
			bool flag = false;
			for (int i = 0; i < EnviroManager.instance.removalZones.Count; i++)
			{
				if (EnviroManager.instance.removalZones[i] == this)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				EnviroManager.instance.AddRemovalZone(this);
			}
		}

		private void RemoveZoneFromManager()
		{
			for (int i = 0; i < EnviroManager.instance.removalZones.Count; i++)
			{
				if (EnviroManager.instance.removalZones[i] == this)
				{
					EnviroManager.instance.RemoveRemovaleZone(EnviroManager.instance.removalZones[i]);
				}
			}
		}

		private void Update()
		{
			base.transform.localScale = size;
		}

		private void OnDrawGizmosSelected()
		{
			if (type == Mode.Spherical)
			{
				Matrix4x4 identity = Matrix4x4.identity;
				Transform transform = base.transform;
				identity.SetTRS(transform.position, transform.rotation, new Vector3(1f, stretch, 1f));
				Gizmos.matrix = identity;
				Gizmos.DrawWireSphere(Vector3.zero, radius);
			}
			else
			{
				Matrix4x4 identity2 = Matrix4x4.identity;
				Transform transform2 = base.transform;
				identity2.SetTRS(transform2.position, transform2.rotation, new Vector3(1f, 1f, 1f));
				Gizmos.matrix = identity2;
				Gizmos.DrawWireCube(Vector3.zero, transform2.localScale);
			}
		}
	}
}
