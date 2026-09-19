using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Features.NetworkServices.Scripts.PriorityManagement
{
	[CreateAssetMenu(fileName = "DistancePriorityConfiguration_Default", menuName = "Configurations/PriorityManagementModule/DistancePriorityConfiguration")]
	public class DistancePriorityConfiguration : ScriptableObject
	{
		public List<DistancePriorityData> DistancePriorityData;

		public float BeachBoostRadius;

		public int GetBestUpdatePriority()
		{
			return DistancePriorityData.Min((DistancePriorityData d) => d.UpdatePriority);
		}
	}
}
