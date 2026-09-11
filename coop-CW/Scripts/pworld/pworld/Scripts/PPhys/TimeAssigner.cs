using UnityEngine;
using pworld.Scripts.Extensions;

namespace pworld.Scripts.PPhys
{
	public class TimeAssigner : MonoBehaviour
	{
		[SerializeReference]
		public ITimeSource timeSource;

		public void AssignTimeSource(ITimeSource source)
		{
			PPhysSpringBase[] componentsInChildren = GetComponentsInChildren<PPhysSpringBase>();
			foreach (PPhysSpringBase obj in componentsInChildren)
			{
				obj.timeSource = source;
				PExt.SaveObj(obj);
			}
		}
	}
}
