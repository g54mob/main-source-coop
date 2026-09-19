using System.Collections.Generic;
using UnityEngine;

namespace RSG.Muffin.InputDeviceSubmodule.InputDeviceModule.Scripts
{
	[CreateAssetMenu(fileName = "InputDevicesPriorityConfiguration_Default", menuName = "Configurations/InputModule/InputDevicesPriorityConfiguration")]
	public class InputDevicesPriorityConfiguration : ScriptableObject
	{
		[field: SerializeField]
		public List<InputDeviceTypeWithIcons> InputDeviceTypesPriorityWithIcons { get; private set; }

		[field: SerializeField]
		public List<InputDeviceVisualizationConfiguration> InputDeviceVisualizationConfigurations { get; private set; }
	}
}
