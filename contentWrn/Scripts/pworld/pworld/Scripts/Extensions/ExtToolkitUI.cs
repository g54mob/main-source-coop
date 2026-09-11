using UnityEngine.UIElements;

namespace pworld.Scripts.Extensions
{
	public static class ExtToolkitUI
	{
		public static VisualElement AddAndFetch(this VisualElement me, VisualElement toAdd)
		{
			me.Add(toAdd);
			return toAdd;
		}
	}
}
