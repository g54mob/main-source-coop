using System;

namespace Rewired.Utils.Classes.Utility
{
	[CustomObfuscation(rename = false)]
	[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
	internal class DualThreadLowLevelInputEventQueue : IDisposable
	{
		private class OZmFZmDMqTjUZGsaFxDQRvNauewZ : LockedObject<LowLevelInputEvent>, INewEventWrapper, IDisposable
		{
			LowLevelInputEvent INewEventWrapper.Event
			{
				get
				{
					return item;
				}
				set
				{
					item = value;
				}
			}

			public OZmFZmDMqTjUZGsaFxDQRvNauewZ(object P_0)
				: base(P_0)
			{
			}
		}

		[CustomObfuscation(rename = false)]
		[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
		public interface INewEventWrapper : IDisposable
		{
			LowLevelInputEvent Event { get; set; }
		}

		private readonly LowLevelInputEventQueue rZAmAOqCRiyoRRfPckGqRLADRcig;

		private readonly LowLevelInputEventQueue JkmYzfjYepGkGUCsefUbHPjNcNajb;

		private readonly object vAyMTAbpRskjoATyCkTlCJVEXzrf;

		private uint HblTJgHBEJNBjsSaClGsDpSSYCJd;

		private bool NxKHTKOWsUgWedImvaVEmGLkAVvn;

		private int WtFlAcsWHmEbromDVjMXDCyqkGmRA;

		private int UpEiqOGwyGUWQCKzEdIiQDLrfNvU;

		private OZmFZmDMqTjUZGsaFxDQRvNauewZ KAfgxmXryJJXDAPGsuTssindhBtT;

		public LowLevelInputEvent currentEvent;

		private bool ozzynxUsHZOqQsfLvbwUbMUgPDCO;

		public uint lastProcessedEventId => HblTJgHBEJNBjsSaClGsDpSSYCJd;

		public int count
		{
			get
			{
				lock (vAyMTAbpRskjoATyCkTlCJVEXzrf)
				{
					return rZAmAOqCRiyoRRfPckGqRLADRcig.Count;
				}
			}
		}

		public DualThreadLowLevelInputEventQueue(int P_0, int P_1, int P_2, int P_3)
		{
			rZAmAOqCRiyoRRfPckGqRLADRcig = new LowLevelInputEventQueue(P_0, P_1, P_2, P_3);
			JkmYzfjYepGkGUCsefUbHPjNcNajb = new LowLevelInputEventQueue(P_0, P_1, P_2, P_3);
			vAyMTAbpRskjoATyCkTlCJVEXzrf = new object();
			KAfgxmXryJJXDAPGsuTssindhBtT = new OZmFZmDMqTjUZGsaFxDQRvNauewZ(vAyMTAbpRskjoATyCkTlCJVEXzrf);
		}

		public INewEventWrapper T_CreateEvent()
		{
			KAfgxmXryJJXDAPGsuTssindhBtT.Lock();
			KAfgxmXryJJXDAPGsuTssindhBtT.item = JkmYzfjYepGkGUCsefUbHPjNcNajb.CreateEvent();
			return KAfgxmXryJJXDAPGsuTssindhBtT;
		}

		public void Update()
		{
			lock (vAyMTAbpRskjoATyCkTlCJVEXzrf)
			{
				rZAmAOqCRiyoRRfPckGqRLADRcig.CopyNewEventsFrom(JkmYzfjYepGkGUCsefUbHPjNcNajb);
			}
		}

		public void Clear()
		{
			lock (vAyMTAbpRskjoATyCkTlCJVEXzrf)
			{
				StopProcessingEvents();
				rZAmAOqCRiyoRRfPckGqRLADRcig.Clear();
				JkmYzfjYepGkGUCsefUbHPjNcNajb.Clear();
			}
		}

		public bool ProcessNewEvents()
		{
			if (UpEiqOGwyGUWQCKzEdIiQDLrfNvU == 0)
			{
				Update();
				int num = rZAmAOqCRiyoRRfPckGqRLADRcig.FindNextIndex(HblTJgHBEJNBjsSaClGsDpSSYCJd);
				if (num < 0)
				{
					currentEvent = default(LowLevelInputEvent);
					return false;
				}
				UpEiqOGwyGUWQCKzEdIiQDLrfNvU = num;
				NxKHTKOWsUgWedImvaVEmGLkAVvn = true;
				WtFlAcsWHmEbromDVjMXDCyqkGmRA = rZAmAOqCRiyoRRfPckGqRLADRcig.Count;
			}
			if (UpEiqOGwyGUWQCKzEdIiQDLrfNvU >= WtFlAcsWHmEbromDVjMXDCyqkGmRA)
			{
				currentEvent = default(LowLevelInputEvent);
				NxKHTKOWsUgWedImvaVEmGLkAVvn = false;
				UpEiqOGwyGUWQCKzEdIiQDLrfNvU = 0;
				return false;
			}
			if (rZAmAOqCRiyoRRfPckGqRLADRcig.TryGetNext(UpEiqOGwyGUWQCKzEdIiQDLrfNvU, out currentEvent))
			{
				HblTJgHBEJNBjsSaClGsDpSSYCJd = currentEvent.GetId();
				UpEiqOGwyGUWQCKzEdIiQDLrfNvU++;
				return true;
			}
			currentEvent = default(LowLevelInputEvent);
			NxKHTKOWsUgWedImvaVEmGLkAVvn = false;
			UpEiqOGwyGUWQCKzEdIiQDLrfNvU = 0;
			return false;
		}

		public void StopProcessingEvents()
		{
			NxKHTKOWsUgWedImvaVEmGLkAVvn = false;
			UpEiqOGwyGUWQCKzEdIiQDLrfNvU = 0;
		}

		public void ImportAll(DualThreadLowLevelInputEventQueue other)
		{
			if (other == null)
			{
				throw new ArgumentNullException("other");
			}
			if (other == this)
			{
				return;
			}
			lock (vAyMTAbpRskjoATyCkTlCJVEXzrf)
			{
				lock (other.vAyMTAbpRskjoATyCkTlCJVEXzrf)
				{
					rZAmAOqCRiyoRRfPckGqRLADRcig.CopyAllFrom(other.rZAmAOqCRiyoRRfPckGqRLADRcig);
					JkmYzfjYepGkGUCsefUbHPjNcNajb.CopyAllFrom(other.JkmYzfjYepGkGUCsefUbHPjNcNajb);
					HblTJgHBEJNBjsSaClGsDpSSYCJd = other.HblTJgHBEJNBjsSaClGsDpSSYCJd;
					NxKHTKOWsUgWedImvaVEmGLkAVvn = other.NxKHTKOWsUgWedImvaVEmGLkAVvn;
					WtFlAcsWHmEbromDVjMXDCyqkGmRA = other.WtFlAcsWHmEbromDVjMXDCyqkGmRA;
					UpEiqOGwyGUWQCKzEdIiQDLrfNvU = other.UpEiqOGwyGUWQCKzEdIiQDLrfNvU;
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

		~DualThreadLowLevelInputEventQueue()
		{
			Dispose(disposing: false);
		}

		protected void Dispose(bool disposing)
		{
			if (ozzynxUsHZOqQsfLvbwUbMUgPDCO)
			{
				return;
			}
			if (disposing)
			{
				lock (vAyMTAbpRskjoATyCkTlCJVEXzrf)
				{
					rZAmAOqCRiyoRRfPckGqRLADRcig.Dispose();
					JkmYzfjYepGkGUCsefUbHPjNcNajb.Dispose();
				}
			}
			ozzynxUsHZOqQsfLvbwUbMUgPDCO = true;
		}
	}
}
