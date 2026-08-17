using System;
using UnityEngine;

namespace NWH.VehiclePhysics2
{
	[Serializable]
	[CreateAssetMenu(fileName = "NWH Vehicle Physics 2", menuName = "NWH/Vehicle Physics 2/LOD", order = 1)]
	public class LOD : ScriptableObject
	{
		public float distance;
	}
}
