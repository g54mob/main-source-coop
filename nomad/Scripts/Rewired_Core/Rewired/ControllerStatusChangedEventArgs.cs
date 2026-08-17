using System;

namespace Rewired
{
	public sealed class ControllerStatusChangedEventArgs : EventArgs
	{
		private string MyeIhrCuJtMyQSohQkdOXhbWOMmD;

		private int qxKfxebGHavOtJrMpPyeCtufbiBQc;

		private ControllerType XzRYUiQazwSYVyBbnjEFfvIoqMeQ;

		public string name => MyeIhrCuJtMyQSohQkdOXhbWOMmD;

		public int controllerId => qxKfxebGHavOtJrMpPyeCtufbiBQc;

		public ControllerType controllerType => XzRYUiQazwSYVyBbnjEFfvIoqMeQ;

		public Controller controller
		{
			get
			{
				if (!ReInput.isReady)
				{
					return null;
				}
				return ReInput.controllers.GetController(XzRYUiQazwSYVyBbnjEFfvIoqMeQ, qxKfxebGHavOtJrMpPyeCtufbiBQc);
			}
		}

		public ControllerStatusChangedEventArgs(string P_0, int P_1, ControllerType P_2)
		{
			MyeIhrCuJtMyQSohQkdOXhbWOMmD = P_0;
			qxKfxebGHavOtJrMpPyeCtufbiBQc = P_1;
			XzRYUiQazwSYVyBbnjEFfvIoqMeQ = P_2;
		}
	}
}
