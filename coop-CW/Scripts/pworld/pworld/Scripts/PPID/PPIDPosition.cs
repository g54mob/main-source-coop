using UnityEngine;

namespace pworld.Scripts.PPID
{
	public class PPIDPosition : MonoBehaviour
	{
		public float proportion;

		public float integral;

		public float derivative;

		public Transform target;

		private PPIDVector pid;

		private void Awake()
		{
			pid = new PPIDVector(proportion, integral, derivative);
		}

		private void Start()
		{
		}

		private void FixedUpdate()
		{
			pid.UpdateValues(proportion, integral, derivative);
			base.transform.position += pid.GetOutput(target.position - base.transform.position, Time.fixedDeltaTime);
		}

		public void UpdateValues()
		{
			pid.UpdateValues(proportion, integral, derivative);
		}
	}
}
