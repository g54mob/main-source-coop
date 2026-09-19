using System.Collections.Generic;
using UnityEngine;

namespace RSG.Muffin.InputDeviceSubmodule.InputDeviceModule.Scripts
{
	[CreateAssetMenu(fileName = "InputDeviceVisualizationConfiguration_Default", menuName = "Configurations/InputModule/InputDeviceVisualizationConfiguration")]
	public class InputDeviceVisualizationConfiguration : ScriptableObject
	{
		[field: SerializeField]
		public List<string> DeviceNames { get; private set; }

		[field: SerializeField]
		public List<InputDeviceShortNameMapItem> InputDeviceShortNameMapItems { get; private set; }
	}
}
