using System;
using System.Collections.Generic;

namespace Rewired.Utils.Classes.Utility
{
	[CustomObfuscation(rename = false)]
	[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
	internal class ThreadedMessageQueue<T> : IDisposable
	{
		private readonly int XnJIMIuNbNKgHeGEHGfuBYetBWTMA;

		private readonly int JEAscFSTyqQQmVTbyvfJaGAmRrNE;

		private readonly int dWxuIvBjiJVveRjiVxYNjfiyOSBA;

		private readonly bool PQGWhyUzGesxKWYIxadXkdsByvTJ;

		private ThreadHelper PEaFUZCtAFWoOGWylKyPJMkCMeHe;

		private Queue<T> NqmYLlOjjoaFGuunDeWDHEOExLth;

		private Queue<T> FVYBJdFcFxBmuDtDwBZXcvSbBiSmA;

		private bool PoywswjINwiqLvuFzYxoZXMNJCPx;

		private bool DhnrvbCOdGfXKNsjvWfXcHjcqlGR;

		private Action<T> opUFNZdapLmjuYYZoIcDqVsIReKx;

		private bool tsxcPtAqQIUhFuOlvkKaaQIsAtOL;

		public ThreadedMessageQueue(int P_0, int P_1, int P_2, bool P_3, Action<T> P_4)
		{
			if (P_4 == null)
			{
				throw new ArgumentNullException("messageReceiverDelegate");
			}
			if (P_0 < 0)
			{
				P_0 = 0;
			}
			if (P_1 < 0)
			{
				P_1 = 0;
			}
			if (P_2 < 0)
			{
				P_2 = 0;
			}
			XnJIMIuNbNKgHeGEHGfuBYetBWTMA = P_0;
			JEAscFSTyqQQmVTbyvfJaGAmRrNE = P_1;
			dWxuIvBjiJVveRjiVxYNjfiyOSBA = P_2;
			PQGWhyUzGesxKWYIxadXkdsByvTJ = P_3;
			opUFNZdapLmjuYYZoIcDqVsIReKx = P_4;
			NqmYLlOjjoaFGuunDeWDHEOExLth = new Queue<T>(P_0);
			FVYBJdFcFxBmuDtDwBZXcvSbBiSmA = new Queue<T>(P_0);
		}

		public void Enqueue(T message)
		{
			if (!gfYWyNNJrzWxRYzbAaVFXSFlDpEH())
			{
				return;
			}
			lock (NqmYLlOjjoaFGuunDeWDHEOExLth)
			{
				if (XnJIMIuNbNKgHeGEHGfuBYetBWTMA > 0)
				{
					while (NqmYLlOjjoaFGuunDeWDHEOExLth.Count >= XnJIMIuNbNKgHeGEHGfuBYetBWTMA)
					{
						NqmYLlOjjoaFGuunDeWDHEOExLth.Dequeue();
					}
				}
				NqmYLlOjjoaFGuunDeWDHEOExLth.Enqueue(message);
			}
		}

		private bool gfYWyNNJrzWxRYzbAaVFXSFlDpEH()
		{
			if (PoywswjINwiqLvuFzYxoZXMNJCPx)
			{
				return false;
			}
			if (!KFydzeQNwjElqDKIGCYkIgtdohfiA())
			{
				return false;
			}
			if (DhnrvbCOdGfXKNsjvWfXcHjcqlGR)
			{
				return true;
			}
			DhnrvbCOdGfXKNsjvWfXcHjcqlGR = true;
			return true;
		}

		private bool KFydzeQNwjElqDKIGCYkIgtdohfiA()
		{
			if (PoywswjINwiqLvuFzYxoZXMNJCPx)
			{
				return false;
			}
			if (PEaFUZCtAFWoOGWylKyPJMkCMeHe == null)
			{
				try
				{
					PEaFUZCtAFWoOGWylKyPJMkCMeHe = ThreadHelper.CreateFixedTimeStep(JEAscFSTyqQQmVTbyvfJaGAmRrNE, dWxuIvBjiJVveRjiVxYNjfiyOSBA);
					PEaFUZCtAFWoOGWylKyPJMkCMeHe.ThreadUpdateEvent += QgVFGTixVTulbLUmKHjEIghbKxCK;
					PEaFUZCtAFWoOGWylKyPJMkCMeHe.Start(PQGWhyUzGesxKWYIxadXkdsByvTJ);
					return true;
				}
				catch (Exception ex)
				{
					Logger.LogError("Exception occurred while creating thread!\n" + ex, requiredThreadSafety: true);
					if (PEaFUZCtAFWoOGWylKyPJMkCMeHe != null)
					{
						PEaFUZCtAFWoOGWylKyPJMkCMeHe.Stop(PQGWhyUzGesxKWYIxadXkdsByvTJ);
					}
					PoywswjINwiqLvuFzYxoZXMNJCPx = true;
					return false;
				}
			}
			if (!PEaFUZCtAFWoOGWylKyPJMkCMeHe.isRunning)
			{
				PEaFUZCtAFWoOGWylKyPJMkCMeHe.Start(PQGWhyUzGesxKWYIxadXkdsByvTJ);
			}
			else if (dWxuIvBjiJVveRjiVxYNjfiyOSBA > 0)
			{
				PEaFUZCtAFWoOGWylKyPJMkCMeHe.ResetTimeout();
			}
			return true;
		}

		private void GdBYlEuRIzIkXPUEiBNwNLQUBRgn()
		{
			lock (NqmYLlOjjoaFGuunDeWDHEOExLth)
			{
				lock (FVYBJdFcFxBmuDtDwBZXcvSbBiSmA)
				{
					MiscTools.Swap(ref NqmYLlOjjoaFGuunDeWDHEOExLth, ref FVYBJdFcFxBmuDtDwBZXcvSbBiSmA);
				}
			}
		}

		private void QgVFGTixVTulbLUmKHjEIghbKxCK()
		{
			GdBYlEuRIzIkXPUEiBNwNLQUBRgn();
			lock (FVYBJdFcFxBmuDtDwBZXcvSbBiSmA)
			{
				while (FVYBJdFcFxBmuDtDwBZXcvSbBiSmA.Count > 0)
				{
					try
					{
						opUFNZdapLmjuYYZoIcDqVsIReKx(FVYBJdFcFxBmuDtDwBZXcvSbBiSmA.Dequeue());
					}
					catch (Exception ex)
					{
						Logger.LogError("An exception occurred while sending message.\nMessage: " + ex.Message, requiredThreadSafety: true);
					}
				}
			}
		}

		public void Dispose()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}

		void IDisposable.Dispose()
		{
			//ILSpy generated this explicit interface implementation from .override directive in Dispose
			this.Dispose();
		}

		~ThreadedMessageQueue()
		{
			Dispose(disposing: false);
		}

		protected void Dispose(bool disposing)
		{
			if (tsxcPtAqQIUhFuOlvkKaaQIsAtOL)
			{
				return;
			}
			if (disposing)
			{
				if (NqmYLlOjjoaFGuunDeWDHEOExLth != null)
				{
					if (FVYBJdFcFxBmuDtDwBZXcvSbBiSmA != null)
					{
						lock (NqmYLlOjjoaFGuunDeWDHEOExLth)
						{
							lock (FVYBJdFcFxBmuDtDwBZXcvSbBiSmA)
							{
								NqmYLlOjjoaFGuunDeWDHEOExLth.Clear();
								FVYBJdFcFxBmuDtDwBZXcvSbBiSmA.Clear();
							}
						}
					}
					else
					{
						lock (NqmYLlOjjoaFGuunDeWDHEOExLth)
						{
							NqmYLlOjjoaFGuunDeWDHEOExLth.Clear();
						}
					}
				}
				else if (FVYBJdFcFxBmuDtDwBZXcvSbBiSmA != null)
				{
					lock (FVYBJdFcFxBmuDtDwBZXcvSbBiSmA)
					{
						FVYBJdFcFxBmuDtDwBZXcvSbBiSmA.Clear();
					}
				}
				if (PEaFUZCtAFWoOGWylKyPJMkCMeHe != null)
				{
					PEaFUZCtAFWoOGWylKyPJMkCMeHe.Dispose();
				}
			}
			tsxcPtAqQIUhFuOlvkKaaQIsAtOL = true;
		}
	}
}
