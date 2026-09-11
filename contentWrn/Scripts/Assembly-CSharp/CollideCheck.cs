using UnityEngine;

public class CollideCheck : MonoBehaviour
{
	private void Test()
	{
		Debug.Log(GetComponentInParent<PropSpawner>().Collides(base.gameObject));
	}
}
