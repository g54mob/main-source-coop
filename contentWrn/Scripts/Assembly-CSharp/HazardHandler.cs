using System.Collections.Generic;
using UnityEngine;

public class HazardHandler : MonoBehaviour
{
	public List<Hazard> hazards = new List<Hazard>();

	public static HazardHandler instance;

	private void Awake()
	{
		instance = this;
	}

	public Hazard GetNearbyHazard(float range, Vector3 pos)
	{
		Hazard result = null;
		for (int i = 0; i < hazards.Count; i++)
		{
			float num = Vector3.Distance(pos, hazards[i].transform.position);
			if (num < range)
			{
				result = hazards[i];
				range = num;
			}
		}
		return result;
	}
}
