using UnityEngine;

public class SeededActivationChance : MonoBehaviour
{
	public float probability = 0.25f;

	public float childProbability = 1f;

	private void Start()
	{
		if (!GameAPI.GetSeededPositionProbability(base.transform.position + Vector3.forward * base.transform.GetSiblingIndex(), probability))
		{
			base.gameObject.SetActive(value: false);
		}
		for (int i = 0; i < base.transform.childCount; i++)
		{
			if (!GameAPI.GetSeededPositionProbability(base.transform.GetChild(i).position + Vector3.forward * base.transform.GetSiblingIndex(), childProbability))
			{
				base.transform.GetChild(i).gameObject.SetActive(value: false);
			}
		}
	}
}
