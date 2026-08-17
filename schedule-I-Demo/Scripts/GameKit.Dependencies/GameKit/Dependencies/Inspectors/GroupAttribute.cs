using UnityEngine;

namespace GameKit.Dependencies.Inspectors
{
	public class GroupAttribute : PropertyAttribute
	{
		public string name;

		public bool foldEverything;

		public GroupAttribute(string name, bool foldEverything = false)
		{
			this.foldEverything = foldEverything;
			this.name = name;
		}
	}
}
