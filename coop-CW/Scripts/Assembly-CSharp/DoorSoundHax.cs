using UnityEngine;

public class DoorSoundHax : MonoBehaviour
{
	private void LateUpdate()
	{
		if (!(MainCamera.instance == null))
		{
			float num = Vector3.Dot(MainCamera.instance.transform.position - base.transform.position, base.transform.forward);
			float z = -1f;
			if (num > 0f)
			{
				z = 1f;
			}
			base.transform.localScale = new Vector3(1f, 1f, z);
		}
	}
}
