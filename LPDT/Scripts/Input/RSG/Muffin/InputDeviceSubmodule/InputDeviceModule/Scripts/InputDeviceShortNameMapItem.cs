using System;
using Global.SerializableDictionary;

namespace RSG.Muffin.InputDeviceSubmodule.InputDeviceModule.Scripts
{
	[Serializable]
	public class InputDeviceShortNameMapItem
	{
		public string InputControlPath;

		public InputDeviceIconCustomizationData DefaultCustomizationData;

		public SerializableDictionary<int, InputDeviceIconCustomizationData> CustomizationsData;
	}
}
