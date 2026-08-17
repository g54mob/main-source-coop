using UnityEngine;

namespace NomadDrive.Features.Rope
{
	[ExecuteAlways]
	public class RopeLanyard : MonoBehaviour
	{
		public Transform deadeyeStart;

		public Transform deadeyeEnd;

		public Transform lanyard;

		public Transform startAttach;

		public Transform endAttach;

		private void Start()
		{
		}

		private void Update()
		{
			if ((bool)startAttach)
			{
				deadeyeStart.position = startAttach.position;
			}
			if ((bool)endAttach)
			{
				deadeyeEnd.position = endAttach.position;
			}
			if ((bool)deadeyeStart && (bool)deadeyeEnd)
			{
				deadeyeStart.LookAt(deadeyeEnd);
				deadeyeEnd.LookAt(deadeyeStart);
			}
		}
	}
}
