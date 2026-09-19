using System;
using UnityEngine;
using UnityEngine.UI;

namespace Features.UINavigationModuleRealization.Scripts
{
	[Serializable]
	public class NavigationPreset
	{
		[SerializeField]
		private Navigation.Mode _mode = Navigation.Mode.Automatic;

		[SerializeField]
		private Selectable _selectOnUp;

		[SerializeField]
		private Selectable _selectOnDown;

		[SerializeField]
		private Selectable _selectOnLeft;

		[SerializeField]
		private Selectable _selectOnRight;

		[SerializeField]
		private bool _savePreviousSelectable;

		public bool SavePreviousSelectable => _savePreviousSelectable;

		public Navigation ToNavigation(Selectable target)
		{
			return ToNavigation(target, _savePreviousSelectable);
		}

		public Navigation ToNavigation(Selectable target, bool savePreviousSelectable)
		{
			Navigation navigation = ((target != null) ? target.navigation : default(Navigation));
			return new Navigation
			{
				mode = _mode,
				selectOnUp = ResolveSelectable(_selectOnUp, navigation.selectOnUp, savePreviousSelectable),
				selectOnDown = ResolveSelectable(_selectOnDown, navigation.selectOnDown, savePreviousSelectable),
				selectOnLeft = ResolveSelectable(_selectOnLeft, navigation.selectOnLeft, savePreviousSelectable),
				selectOnRight = ResolveSelectable(_selectOnRight, navigation.selectOnRight, savePreviousSelectable)
			};
		}

		private static Selectable ResolveSelectable(Selectable presetValue, Selectable previousValue, bool savePreviousSelectable)
		{
			if (!(presetValue != null))
			{
				if (!savePreviousSelectable)
				{
					return null;
				}
				return previousValue;
			}
			return presetValue;
		}
	}
}
