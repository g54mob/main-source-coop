using UnityEngine;

namespace pworld.Scripts
{
	public class PMoveHere : MonoBehaviour
	{
		public Transform moveHere;

		public void MoveIt()
		{
			base.transform.position = moveHere.position;
		}
	}
}
