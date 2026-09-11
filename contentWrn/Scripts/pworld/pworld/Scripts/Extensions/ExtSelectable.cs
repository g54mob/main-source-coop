using System.Collections.Generic;
using UnityEngine.UI;

namespace pworld.Scripts.Extensions
{
	public static class ExtSelectable
	{
		public static bool CanNavigateFrom(this Selectable me)
		{
			bool flag = false;
			flag = me.FindSelectableOnUp() != null || flag;
			flag = me.FindSelectableOnDown() != null || flag;
			flag = me.FindSelectableOnRight() != null || flag;
			return me.FindSelectableOnLeft() != null || flag;
		}

		public static void PSetUpVerticalNavigation(this List<Selectable> me, bool saveInEditor = false, bool reverse = false, bool loop = false)
		{
			List<Selectable> list = new List<Selectable>(me);
			if (reverse)
			{
				list.Reverse();
			}
			for (int i = 0; i < me.Count; i++)
			{
				Selectable selectable = me[i];
				Navigation navigation = selectable.navigation;
				navigation.mode = Navigation.Mode.Explicit;
				object selectOnUp;
				if (i != 0)
				{
					selectOnUp = me[i - 1];
				}
				else if (!loop)
				{
					selectOnUp = null;
				}
				else
				{
					selectOnUp = me[me.Count - 1];
				}
				navigation.selectOnUp = (Selectable)selectOnUp;
				navigation.selectOnDown = ((i != me.Count - 1) ? me[i + 1] : (loop ? me[0] : null));
				selectable.navigation = navigation;
				if (saveInEditor)
				{
					PExt.SaveObj(selectable);
				}
			}
		}

		public static void PSetUpHorizontalNavigation(this List<Selectable> me, bool saveInEditor = false, bool reverse = false, bool loop = false)
		{
			if (reverse)
			{
				me.Reverse();
			}
			for (int i = 0; i < me.Count; i++)
			{
				Selectable selectable = me[i];
				Navigation navigation = selectable.navigation;
				navigation.mode = Navigation.Mode.Explicit;
				object selectOnLeft;
				if (i != 0)
				{
					selectOnLeft = me[i - 1];
				}
				else if (!loop)
				{
					selectOnLeft = null;
				}
				else
				{
					selectOnLeft = me[me.Count - 1];
				}
				navigation.selectOnLeft = (Selectable)selectOnLeft;
				navigation.selectOnRight = ((i != me.Count - 1) ? me[i + 1] : (loop ? me[0] : null));
				selectable.navigation = navigation;
				if (saveInEditor)
				{
					PExt.SaveObj(selectable);
				}
			}
		}
	}
}
