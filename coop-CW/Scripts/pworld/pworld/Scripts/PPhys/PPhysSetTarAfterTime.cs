using UnityEngine;
using pworld.Scripts.Extensions;

namespace pworld.Scripts.PPhys
{
	public class PPhysSetTarAfterTime : MonoBehaviour
	{
		public Vector3 target;

		public float time;

		private PAlarm alarm;

		private void Awake()
		{
			alarm = new PAlarm(this, null, delegate
			{
				GetComponent<PPhysSpringBase>().Target = target;
			});
			alarm.Set(time);
		}
	}
}
