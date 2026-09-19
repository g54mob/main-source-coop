using Features.DeviceModule.Scripts.DeviceData;
using UnityEngine;

namespace Features.DeviceModule.Scripts
{
	[CreateAssetMenu(fileName = "DeviceEditorSwitcherConfiguration_Default", menuName = "Configurations/DeviceModule/DeviceEditorSwitcherConfiguration")]
	public class DeviceEditorSwitcherConfiguration : ScriptableObject
	{
		[field: SerializeField]
		public Features.DeviceModule.Scripts.DeviceData.DeviceType EditorDeviceType { get; private set; }
	}
}
