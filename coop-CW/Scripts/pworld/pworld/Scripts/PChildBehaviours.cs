using UnityEngine;

namespace pworld.Scripts
{
	public class PChildBehaviours : MonoBehaviour
	{
		public enum CHILD_TYPE
		{
			Step = 0
		}

		public CHILD_TYPE childType;

		public Transform parent;

		private void Start()
		{
		}

		private void Update()
		{
			if (childType == CHILD_TYPE.Step)
			{
				StepUpdate();
			}
		}

		private void StepUpdate()
		{
			base.transform.position = parent.position;
			base.transform.rotation = parent.rotation;
		}
	}
}
