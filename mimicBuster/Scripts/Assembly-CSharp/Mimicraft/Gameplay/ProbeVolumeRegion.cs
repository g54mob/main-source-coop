using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Mimicraft.Gameplay
{
	public class ProbeVolumeRegion : MonoBehaviour
	{
		[Tooltip("Bu bölgeyi aydınlatan APV Baking Set. Sahne bu set ile bakelenmiş olmalı.")]
		[SerializeField]
		private ProbeVolumeBakingSet bakingSet;

		[Tooltip("Bölgenin kutusu - bu objenin yerel uzayında. Kamerası burada olan oyuncu yukarıdaki seti kullanır.")]
		[SerializeField]
		private Vector3 center;

		[SerializeField]
		private Vector3 size = new Vector3(64f, 32f, 64f);

		[Tooltip("Birden fazla bölge çakışırsa YÜKSEK olan kazanır. Avcı odası haritanın içine gömülüyse odaya daha yüksek bir değer ver.")]
		[SerializeField]
		private int priority;

		private static readonly List<ProbeVolumeRegion> active = new List<ProbeVolumeRegion>();

		public static IReadOnlyList<ProbeVolumeRegion> Active => active;

		public ProbeVolumeBakingSet BakingSet => bakingSet;

		public int Priority => priority;

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetStatics()
		{
			active.Clear();
		}

		private void OnEnable()
		{
			active.Add(this);
			ProbeVolumeRegionSwitcher.Ensure();
		}

		private void OnDisable()
		{
			active.Remove(this);
		}

		public bool Contains(Vector3 worldPoint)
		{
			return new Bounds(center, size).Contains(base.transform.InverseTransformPoint(worldPoint));
		}

		public static ProbeVolumeRegion For(Vector3 worldPoint)
		{
			ProbeVolumeRegion probeVolumeRegion = null;
			foreach (ProbeVolumeRegion item in active)
			{
				if (!(item == null) && !(item.bakingSet == null) && item.Contains(worldPoint) && (probeVolumeRegion == null || item.priority > probeVolumeRegion.priority))
				{
					probeVolumeRegion = item;
				}
			}
			return probeVolumeRegion;
		}

		private void OnDrawGizmosSelected()
		{
			Gizmos.matrix = base.transform.localToWorldMatrix;
			Gizmos.color = ((bakingSet != null) ? new Color(0.4f, 0.7f, 1f, 0.8f) : new Color(0.9f, 0.5f, 0.2f, 0.8f));
			Gizmos.DrawWireCube(center, size);
		}
	}
}
