using System;

namespace Rewired
{
	public sealed class KeyboardMap : ControllerMap
	{
		public KeyboardMap()
		{
			_controllerType = ControllerType.Keyboard;
			_controllerId = 0;
		}

		public KeyboardMap(KeyboardMap P_0)
			: base(P_0)
		{
		}

		internal void gAUNPMUaaUKLvNZNbIwQEoXbsUzu(Guid P_0, int P_1, int P_2)
		{
			_hardwareGuid = P_0;
			_categoryId = P_1;
			_layoutId = P_2;
		}

		internal static KeyboardMap ZjKfUjGyzyNIUfydHfYlzgEqlodt(Guid P_0, int P_1, int P_2)
		{
			return new KeyboardMap
			{
				_hardwareGuid = P_0,
				_categoryId = P_1,
				_layoutId = P_2,
				_sourceMapId = -1
			};
		}
	}
}
