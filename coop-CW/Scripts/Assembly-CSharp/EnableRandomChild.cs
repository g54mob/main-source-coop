using UnityEngine;

public class EnableRandomChild : MonoBehaviour
{
	public void EnableRandom()
	{
		base.transform.GetChild(Random.Range(0, base.transform.childCount)).gameObject.SetActive(value: true);
	}
}
