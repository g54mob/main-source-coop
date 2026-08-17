using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace NWH.VehiclePhysics2
{
	[Serializable]
	[CreateAssetMenu(fileName = "NWH Vehicle Physics 2", menuName = "NWH/Vehicle Physics 2/State Settings", order = 1)]
	public class StateSettings : ScriptableObject
	{
		public List<StateDefinition> definitions = new List<StateDefinition>();

		public List<LOD> LODs = new List<LOD>();

		private int _lodCount = -1;

		public int LODCount
		{
			get
			{
				if (_lodCount < 0)
				{
					_lodCount = LODs.Count;
				}
				return _lodCount;
			}
		}

		public StateDefinition GetDefinition(string fullComponentTypeName)
		{
			return definitions.Find((StateDefinition d) => d.fullName == fullComponentTypeName);
		}

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		public void Reload()
		{
			List<string> fullNames = (from t in AppDomain.CurrentDomain.GetAssemblies().SelectMany((Assembly assembly) => assembly.GetTypes())
				where !t.IsAbstract && t.IsSubclassOf(typeof(VehicleComponent))
				select t.FullName).ToList();
			foreach (string item in fullNames)
			{
				if (GetDefinition(item) == null)
				{
					definitions.Add(new StateDefinition(item, isEnabled: true, -1));
				}
			}
			definitions.RemoveAll((StateDefinition d) => fullNames.All((string n) => n != d.fullName));
			definitions = definitions.OrderBy((StateDefinition d) => d.fullName).ToList();
		}
	}
}
