using System.Collections.Generic;
using UnityEngine;

namespace Features.RumModule.Scripts
{
	[CreateAssetMenu(fileName = "RumConfiguration_Default", menuName = "Configurations/Rum/RumConfiguration")]
	public class RumConfiguration : ScriptableObject
	{
		[field: SerializeField]
		public List<RumData> RumsData { get; private set; }
	}
}
