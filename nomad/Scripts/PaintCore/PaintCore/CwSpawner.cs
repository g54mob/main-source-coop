using System.Collections.Generic;
using UnityEngine;

namespace PaintCore
{
	[HelpURL("https://carloswilkes.com/Documentation/PaintCore#CwSpawner")]
	[AddComponentMenu("CW/Paint Core/Hit/CW Spawner")]
	public class CwSpawner : MonoBehaviour, IHitPoint, IHit
	{
		[SerializeField]
		private List<GameObject> prefabs;

		[SerializeField]
		private float radius;

		[SerializeField]
		private Vector3 velocity;

		[SerializeField]
		private float offsetNormal;

		[SerializeField]
		private Vector3 offsetWorld;

		[SerializeField]
		private GameObject prefab;

		public List<GameObject> Prefabs
		{
			get
			{
				if (prefabs == null)
				{
					prefabs = new List<GameObject>();
				}
				return prefabs;
			}
		}

		public float Radius
		{
			get
			{
				return radius;
			}
			set
			{
				radius = value;
			}
		}

		public Vector3 Velocity
		{
			get
			{
				return velocity;
			}
			set
			{
				velocity = value;
			}
		}

		public float OffsetNormal
		{
			get
			{
				return offsetNormal;
			}
			set
			{
				offsetNormal = value;
			}
		}

		public Vector3 OffsetWorld
		{
			get
			{
				return offsetWorld;
			}
			set
			{
				offsetWorld = value;
			}
		}

		public void Spawn()
		{
			Spawn(base.transform.position, base.transform.rotation);
		}

		public void Spawn(Vector3 position, Vector3 normal)
		{
			Spawn(position, Quaternion.LookRotation(normal));
		}

		public void Spawn(Vector3 position, Quaternion rotation)
		{
			UpgradeLegacy();
			if (prefabs == null || prefabs.Count <= 0)
			{
				return;
			}
			GameObject gameObject = prefabs[Random.Range(0, prefabs.Count)];
			if (gameObject != null)
			{
				position += Random.insideUnitSphere * radius;
				GameObject obj = Object.Instantiate(gameObject, position, rotation, null);
				Rigidbody component = obj.GetComponent<Rigidbody>();
				if (component != null)
				{
					component.linearVelocity = rotation * velocity;
				}
				obj.SetActive(value: true);
			}
		}

		public void HandleHitPoint(bool preview, int priority, float pressure, int seed, Vector3 position, Quaternion rotation)
		{
			Spawn(position + rotation * Vector3.forward * offsetNormal + offsetWorld, rotation);
		}

		public void UpgradeLegacy()
		{
			if (prefab != null && Prefabs.Count == 0)
			{
				prefabs.Add(prefab);
				prefab = null;
			}
		}
	}
}
