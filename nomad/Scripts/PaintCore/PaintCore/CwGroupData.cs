using UnityEngine;

namespace PaintCore
{
	public class CwGroupData : ScriptableObject
	{
		[SerializeField]
		private int index;

		public int Index
		{
			get
			{
				return index;
			}
			set
			{
				index = value;
			}
		}

		public string GetName(bool prefixNumber)
		{
			if (prefixNumber)
			{
				return index + ": " + base.name;
			}
			return base.name;
		}
	}
}
