using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Features.UINavigationModuleRealization.Scripts
{
	public class NavigationPresetApplier : MonoBehaviour
	{
		[SerializeField]
		private Selectable _target;

		[SerializeField]
		private List<NavigationPreset> _presets = new List<NavigationPreset>();

		public int PresetCount => _presets.Count;

		public void ApplyNavigationByIndex(int index)
		{
			if (!(_target == null) && index >= 0 && index < _presets.Count)
			{
				_target.navigation = _presets[index].ToNavigation(_target);
			}
		}
	}
}
