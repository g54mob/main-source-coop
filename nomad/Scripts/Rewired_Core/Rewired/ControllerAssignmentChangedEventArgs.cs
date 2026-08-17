using System;

namespace Rewired
{
	public sealed class ControllerAssignmentChangedEventArgs : EventArgs
	{
		private bool zIFftPeKnQSQLIcYXkDAzodzpwUXA;

		private int YCLFriwRPvsCCNMhWOmprVhOKahm;

		private int hCsATwgXFueEeNqHOAMFjEdzreQq;

		private ControllerType MZekAnhLsmsJEtJXVwbsVhpKWkOF;

		public bool state => zIFftPeKnQSQLIcYXkDAzodzpwUXA;

		public Controller controller
		{
			get
			{
				if (!ReInput.isReady)
				{
					return null;
				}
				return ReInput.controllers.GetController(MZekAnhLsmsJEtJXVwbsVhpKWkOF, hCsATwgXFueEeNqHOAMFjEdzreQq);
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
				return ReInput.players.GetPlayer(YCLFriwRPvsCCNMhWOmprVhOKahm);
			}
		}

		internal ControllerAssignmentChangedEventArgs(int P_0, int P_1, ControllerType P_2, bool P_3)
		{
			zIFftPeKnQSQLIcYXkDAzodzpwUXA = P_3;
			YCLFriwRPvsCCNMhWOmprVhOKahm = P_0;
			hCsATwgXFueEeNqHOAMFjEdzreQq = P_1;
			MZekAnhLsmsJEtJXVwbsVhpKWkOF = P_2;
		}
	}
}
