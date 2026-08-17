using System;

namespace Rewired
{
	public sealed class ControllerStatusChangedEventArgs : EventArgs
	{
		private string hEJFspqWqeBOdedbqHNRpPNEbfBG;

		private int UceKlChkUHGjQaNwpAzeqaieFMpt;

		private ControllerType KHkewZTAeuvHVDDJaWvnQRRxxDiF;

		public string name => hEJFspqWqeBOdedbqHNRpPNEbfBG;

		public int controllerId => UceKlChkUHGjQaNwpAzeqaieFMpt;

		public ControllerType controllerType => KHkewZTAeuvHVDDJaWvnQRRxxDiF;

		public Controller controller
		{
			get
			{
				if (!ReInput.isReady)
				{
					return null;
				}
				return ReInput.controllers.GetController(KHkewZTAeuvHVDDJaWvnQRRxxDiF, UceKlChkUHGjQaNwpAzeqaieFMpt);
			}
		}

		public ControllerStatusChangedEventArgs(string P_0, int P_1, ControllerType P_2)
		{
			hEJFspqWqeBOdedbqHNRpPNEbfBG = P_0;
			UceKlChkUHGjQaNwpAzeqaieFMpt = P_1;
			KHkewZTAeuvHVDDJaWvnQRRxxDiF = P_2;
		}
	}
}
