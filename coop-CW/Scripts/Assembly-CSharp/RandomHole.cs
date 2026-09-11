using UnityEngine;

public class RandomHole : MonoBehaviour
{
	public float probability = 0.25f;

	public float childProbability = 1f;

	private void Start()
	{
		if (GameAPI.GetSeededPositionProbability(base.transform.position + Vector3.forward * base.transform.GetSiblingIndex(), probability))
		{
			base.transform.GetChild(1).gameObject.SetActive(value: false);
			base.transform.GetChild(0).gameObject.SetActive(value: true);
		}
		else
		{
			base.transform.GetChild(0).gameObject.SetActive(value: false);
			base.transform.GetChild(1).gameObject.SetActive(value: true);
		}
	}
}
