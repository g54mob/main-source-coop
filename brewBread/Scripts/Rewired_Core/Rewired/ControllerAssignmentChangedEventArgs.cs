using System;

namespace Rewired
{
	public sealed class ControllerAssignmentChangedEventArgs : EventArgs
	{
		private bool IcXfJYgXoAwFSLAcAfqjnBioHaOP;

		private int DCGJXzigEdTcwzFynDKUcORiHGSOA;

		private int UceKlChkUHGjQaNwpAzeqaieFMpt;

		private ControllerType KHkewZTAeuvHVDDJaWvnQRRxxDiF;

		public bool state => IcXfJYgXoAwFSLAcAfqjnBioHaOP;

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

		public Player player
		{
			get
			{
				if (!ReInput.isReady)
				{
					return null;
				}
				return ReInput.players.GetPlayer(DCGJXzigEdTcwzFynDKUcORiHGSOA);
			}
		}

		internal ControllerAssignmentChangedEventArgs(int P_0, int P_1, ControllerType P_2, bool P_3)
		{
			IcXfJYgXoAwFSLAcAfqjnBioHaOP = P_3;
			DCGJXzigEdTcwzFynDKUcORiHGSOA = P_0;
			UceKlChkUHGjQaNwpAzeqaieFMpt = P_1;
			KHkewZTAeuvHVDDJaWvnQRRxxDiF = P_2;
		}
	}
}
