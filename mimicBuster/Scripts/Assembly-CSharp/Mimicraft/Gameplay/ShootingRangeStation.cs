using System.Collections.Generic;
using UnityEngine;

namespace Mimicraft.Gameplay
{
	public class ShootingRangeStation : MonoBehaviour
	{
		[Tooltip("Hareket eden hedef. Boş bırakılırsa bu objenin ilk çocuğu kullanılır.")]
		[SerializeField]
		private Transform target;

		[Tooltip("Hedefin durabileceği noktalar - sırayla. Her butona bir indeks veriyorsun, yani 5m/10m/20m gibi mesafeleri buraya elle koyduğun boş objelerle belirliyorsun.")]
		[SerializeField]
		private Transform[] stops = new Transform[0];

		[Tooltip("Hedefin hızı, m/sn. Anında ışınlanmıyor - rayda kayıyor.")]
		[SerializeField]
		[Min(0.1f)]
		private float moveSpeed = 8f;

		[Tooltip("Başlangıçta hangi durakta duracağı.")]
		[SerializeField]
		[Min(0f)]
		private int startStop;

		private static readonly List<ShootingRangeStation> all = new List<ShootingRangeStation>();

		private int destination;

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetStatics()
		{
			all.Clear();
		}

		private void OnEnable()
		{
			all.Add(this);
			if (target == null && base.transform.childCount > 0)
			{
				target = base.transform.GetChild(0);
			}
			destination = Mathf.Clamp(startStop, 0, Mathf.Max(0, stops.Length - 1));
			if (target != null && TryGetStop(destination, out var position))
			{
				target.position = position;
			}
		}

		private void OnDisable()
		{
			all.Remove(this);
		}

		public void GoTo(int stop)
		{
			if (stop >= 0 && stop < stops.Length && stops[stop] != null)
			{
				destination = stop;
			}
		}

		private void Update()
		{
			if (!(target == null) && TryGetStop(destination, out var position))
			{
				target.position = Vector3.MoveTowards(target.position, position, moveSpeed * Time.deltaTime);
			}
		}

		private bool TryGetStop(int stop, out Vector3 position)
		{
			position = default(Vector3);
			if (stop < 0 || stop >= stops.Length || stops[stop] == null)
			{
				return false;
			}
			position = stops[stop].position;
			return true;
		}

		public static ShootingRangeStation Nearest(Vector3 point, float within)
		{
			ShootingRangeStation result = null;
			float num = within * within;
			foreach (ShootingRangeStation item in all)
			{
				if (!(item == null))
				{
					float sqrMagnitude = (item.transform.position - point).sqrMagnitude;
					if (!(sqrMagnitude > num))
					{
						num = sqrMagnitude;
						result = item;
					}
				}
			}
			return result;
		}

		private void OnDrawGizmosSelected()
		{
			if (stops == null)
			{
				return;
			}
			Gizmos.color = new Color(0.3f, 0.9f, 0.5f, 0.9f);
			Vector3 position = base.transform.position;
			for (int i = 0; i < stops.Length; i++)
			{
				if (!(stops[i] == null))
				{
					Gizmos.DrawWireCube(stops[i].position, Vector3.one * 0.4f);
					Gizmos.DrawLine(position, stops[i].position);
					position = stops[i].position;
				}
			}
		}
	}
}
