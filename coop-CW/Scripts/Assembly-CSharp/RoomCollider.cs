using UnityEngine;

public class RoomCollider : MonoBehaviour
{
	public void TestCollision()
	{
		Debug.Log(GetComponentInParent<Room>().VerifyCollider(GetComponent<Collider>()));
	}
}
