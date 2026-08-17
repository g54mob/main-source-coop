using System;

namespace Rewired
{
	public sealed class MouseMap : ControllerMapWithAxes
	{
		public MouseMap()
		{
			_controllerType = ControllerType.Mouse;
			_controllerId = 0;
		}

		public MouseMap(MouseMap P_0)
			: base(P_0)
		{
		}

		internal void OPieIRwFQUNEFaYLpKUnHAyPmaUs(Guid P_0, int P_1, int P_2)
		{
			_hardwareGuid = P_0;
			_categoryId = P_1;
			_layoutId = P_2;
		}

		internal static MouseMap JGdwEkTXDISoEvRBhOWRtTDetjzA(Guid P_0, int P_1, int P_2)
		{
			return new MouseMap
			{
				_hardwareGuid = P_0,
				_categoryId = P_1,
				_layoutId = P_2,
				_sourceMapId = -1
			};
		}
	}
}
