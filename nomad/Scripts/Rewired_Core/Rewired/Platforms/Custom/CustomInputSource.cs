using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Threading;
using Rewired.Interfaces;

namespace Rewired.Platforms.Custom
{
	public abstract class CustomInputSource : IDisposable
	{
		public abstract class Controller
		{
			protected bool _isConnected;

			protected string _deviceName;

			protected string _customName;

			protected object _customIdentifier;

			protected Guid _persistentGuid;

			private Action<bool> OSFVEqSfZplZWQaqjDNTVGbQKlzI;

			public string customName => _customName;

			public bool isConnected
			{
				get
				{
					return _isConnected;
				}
				set
				{
					if (value == _isConnected)
					{
						return;
					}
					_isConnected = value;
					Action<bool> oSFVEqSfZplZWQaqjDNTVGbQKlzI = OSFVEqSfZplZWQaqjDNTVGbQKlzI;
					if (oSFVEqSfZplZWQaqjDNTVGbQKlzI == null)
					{
						return;
					}
					try
					{
						oSFVEqSfZplZWQaqjDNTVGbQKlzI(value);
					}
					catch (Exception exception)
					{
						ReInput.HandleCallbackException("CustomInputSource.Controller.ConnectedStateChangedEvent", exception);
					}
				}
			}

			public string deviceName => _deviceName;

			public object customIdentifier
			{
				get
				{
					return _customIdentifier;
				}
				set
				{
					_customIdentifier = value;
				}
			}

			public Guid deviceInstanceGuid
			{
				get
				{
					return _persistentGuid;
				}
				set
				{
					_persistentGuid = value;
				}
			}

			public event Action<bool> ConnectedStateChangedEvent
			{
				add
				{
					OSFVEqSfZplZWQaqjDNTVGbQKlzI = (Action<bool>)Delegate.Combine(OSFVEqSfZplZWQaqjDNTVGbQKlzI, value);
				}
				remove
				{
					OSFVEqSfZplZWQaqjDNTVGbQKlzI = (Action<bool>)Delegate.Remove(OSFVEqSfZplZWQaqjDNTVGbQKlzI, value);
				}
			}

			protected Controller(string P_0)
			{
				_deviceName = P_0;
			}

			public void Disconnect()
			{
				if (_isConnected)
				{
					isConnected = false;
				}
			}

			public void Connect()
			{
				if (!_isConnected)
				{
					isConnected = true;
				}
			}

			public abstract void Update();
		}

		public abstract class Joystick : Controller
		{
			private long? VVbHLOcLWNMAXnnbwUUUydkruenU;

			private int dRhSpPnQAHlUrTBzhuehWIIUBsGq;

			private readonly Axis[] NPpCIPtBwvCJRcraEQXtkNTQngQQA;

			private readonly Button[] BkRDMLYuLbVaIPmrpCIWbuPqcLgIb;

			private readonly ReadOnlyCollection<Axis> uvbjADSQhLqGkoLknXHTQDPsOrPN;

			private readonly ReadOnlyCollection<Button> FiipFHmMbpSaLcHQOVAnwVbAQVlL;

			private bool QHekWdfanEKIwgoOicxMBeCePirG;

			private Rewired.Controller.Extension LsbwzqeCMDrfamtCQlTZNtMAVndc;

			public long? systemId
			{
				get
				{
					return VVbHLOcLWNMAXnnbwUUUydkruenU;
				}
				protected set
				{
					VVbHLOcLWNMAXnnbwUUUydkruenU = value;
				}
			}

			public int unityId
			{
				get
				{
					return dRhSpPnQAHlUrTBzhuehWIIUBsGq;
				}
				protected set
				{
					dRhSpPnQAHlUrTBzhuehWIIUBsGq = value;
				}
			}

			public IList<Axis> Axes => uvbjADSQhLqGkoLknXHTQDPsOrPN;

			public IList<Button> Buttons => FiipFHmMbpSaLcHQOVAnwVbAQVlL;

			public bool supportsVibration
			{
				get
				{
					return QHekWdfanEKIwgoOicxMBeCePirG;
				}
				set
				{
					QHekWdfanEKIwgoOicxMBeCePirG = value;
				}
			}

			public Rewired.Controller.Extension extension
			{
				get
				{
					return LsbwzqeCMDrfamtCQlTZNtMAVndc;
				}
				set
				{
					LsbwzqeCMDrfamtCQlTZNtMAVndc = value;
					if (LsbwzqeCMDrfamtCQlTZNtMAVndc is IControllerVibrator)
					{
						QHekWdfanEKIwgoOicxMBeCePirG = true;
					}
				}
			}

			public int buttonCount => BkRDMLYuLbVaIPmrpCIWbuPqcLgIb.Length;

			public int axisCount => NPpCIPtBwvCJRcraEQXtkNTQngQQA.Length;

			protected Joystick(string P_0, long P_1, int P_2, int P_3)
				: this(P_0, P_1, 0, P_2, P_3)
			{
			}

			public Joystick(string P_0, long? P_1, int P_2, int P_3, int P_4)
				: base(P_0)
			{
				if (P_3 < 0)
				{
					P_3 = 0;
				}
				if (P_4 < 0)
				{
					P_4 = 0;
				}
				VVbHLOcLWNMAXnnbwUUUydkruenU = P_1;
				dRhSpPnQAHlUrTBzhuehWIIUBsGq = P_2;
				NPpCIPtBwvCJRcraEQXtkNTQngQQA = new Axis[P_3];
				BkRDMLYuLbVaIPmrpCIWbuPqcLgIb = new Button[P_4];
				for (int i = 0; i < P_3; i++)
				{
					NPpCIPtBwvCJRcraEQXtkNTQngQQA[i] = new Axis();
				}
				for (int j = 0; j < P_4; j++)
				{
					BkRDMLYuLbVaIPmrpCIWbuPqcLgIb[j] = new Button();
				}
				uvbjADSQhLqGkoLknXHTQDPsOrPN = new ReadOnlyCollection<Axis>(NPpCIPtBwvCJRcraEQXtkNTQngQQA);
				FiipFHmMbpSaLcHQOVAnwVbAQVlL = new ReadOnlyCollection<Button>(BkRDMLYuLbVaIPmrpCIWbuPqcLgIb);
			}

			public virtual float GetAxisValue(int index)
			{
				if (index < 0 || index >= NPpCIPtBwvCJRcraEQXtkNTQngQQA.Length)
				{
					return 0f;
				}
				return NPpCIPtBwvCJRcraEQXtkNTQngQQA[index].value;
			}

			public virtual bool GetButtonValue(int index)
			{
				if (index < 0 || index >= BkRDMLYuLbVaIPmrpCIWbuPqcLgIb.Length)
				{
					return false;
				}
				return BkRDMLYuLbVaIPmrpCIWbuPqcLgIb[index].boolValue;
			}

			public virtual float GetButtonFloatValue(int index)
			{
				if (index < 0 || index >= BkRDMLYuLbVaIPmrpCIWbuPqcLgIb.Length)
				{
					return 0f;
				}
				return BkRDMLYuLbVaIPmrpCIWbuPqcLgIb[index].floatValue;
			}

			public virtual void SetAxisValue(int index, float value)
			{
				if (index >= 0 && index < NPpCIPtBwvCJRcraEQXtkNTQngQQA.Length)
				{
					NPpCIPtBwvCJRcraEQXtkNTQngQQA[index].value = value;
				}
			}

			public virtual void SetButtonValue(int index, bool value)
			{
				if (index >= 0 && index < BkRDMLYuLbVaIPmrpCIWbuPqcLgIb.Length)
				{
					BkRDMLYuLbVaIPmrpCIWbuPqcLgIb[index].boolValue = value;
				}
			}

			public virtual void SetButtonFloatValue(int index, float value)
			{
				if (index >= 0 && index < BkRDMLYuLbVaIPmrpCIWbuPqcLgIb.Length)
				{
					BkRDMLYuLbVaIPmrpCIWbuPqcLgIb[index].floatValue = value;
				}
			}

			internal void ljiSaOCcXfofdbVaKCGSKyocBRMG(int P_0, out bool P_1, out float P_2)
			{
				if (P_0 < 0 || P_0 >= BkRDMLYuLbVaIPmrpCIWbuPqcLgIb.Length)
				{
					P_1 = false;
					P_2 = 0f;
				}
				else
				{
					P_1 = BkRDMLYuLbVaIPmrpCIWbuPqcLgIb[P_0].AzbAkOjtwWDtNhcTfnsMWsnxPyMZ;
					P_2 = BkRDMLYuLbVaIPmrpCIWbuPqcLgIb[P_0].floatValue;
				}
			}

			internal virtual void qWsfrgdHVSOtLdhBEgSUxhNFQhKA()
			{
				for (int i = 0; i < BkRDMLYuLbVaIPmrpCIWbuPqcLgIb.Length; i++)
				{
					if (BkRDMLYuLbVaIPmrpCIWbuPqcLgIb[i] != null)
					{
						BkRDMLYuLbVaIPmrpCIWbuPqcLgIb[i].BrePWjpXXrJTHEKQrMmBZNYxMlbX();
					}
				}
			}
		}

		public abstract class Element
		{
		}

		public sealed class Axis : Element
		{
			public float value;
		}

		public sealed class Button : Element
		{
			[Obsolete("Deprecated. Use boolValue instead.", false)]
			public bool value;

			private float oWtwHVLevJgjhuTpXkBTxXKPIteV;

			private bool TafkerMHKMlkhueiqGdWXkzcefBS;

			public bool boolValue
			{
				get
				{
					return value;
				}
				set
				{
					_ = this.value;
					if (!this.value && value)
					{
						TafkerMHKMlkhueiqGdWXkzcefBS = true;
					}
					this.value = value;
				}
			}

			public float floatValue
			{
				get
				{
					return oWtwHVLevJgjhuTpXkBTxXKPIteV;
				}
				set
				{
					oWtwHVLevJgjhuTpXkBTxXKPIteV = value;
				}
			}

			internal bool AzbAkOjtwWDtNhcTfnsMWsnxPyMZ
			{
				get
				{
					if (!value)
					{
						return TafkerMHKMlkhueiqGdWXkzcefBS;
					}
					return true;
				}
			}

			internal void BrePWjpXXrJTHEKQrMmBZNYxMlbX()
			{
				TafkerMHKMlkhueiqGdWXkzcefBS = false;
			}
		}

		private readonly InputSource ksFnQGKIXSmvvpvxpjJWEBkvNFqM;

		private readonly List<Joystick> UglIohQjisfVjcxxHTogJQZZldvbA;

		private readonly ReadOnlyCollection<Joystick> nAdhEBeVnyoYsMaczkusZmNUdNucA;

		private bool VPCJgIwCLIAWWIEVBoUMBkLUkgqL = true;

		private IUnifiedKeyboardSource xlIWKgIlcUpoKaNkxzvuwinSbYWU;

		private IUnifiedMouseSource VpUGsxmUWaykGrtssctpOkJHaaiM;

		[CompilerGenerated]
		private Action m_HXWuHyOYnypyEaswgzVulxPMOfcc;

		[CompilerGenerated]
		private Action m_JZHOyikwjlFaUeGfviyQeUKgoPhNA;

		private bool vGEqgUBUqAVznvCQMjpUjvyBpZkE;

		public bool useApproximateMatching
		{
			get
			{
				return VPCJgIwCLIAWWIEVBoUMBkLUkgqL;
			}
			protected set
			{
				VPCJgIwCLIAWWIEVBoUMBkLUkgqL = value;
			}
		}

		internal InputSource ynIdjZCpVlwTpZFWgFUmNBinSjFu => ksFnQGKIXSmvvpvxpjJWEBkvNFqM;

		public abstract bool isReady { get; }

		private event Action HXWuHyOYnypyEaswgzVulxPMOfcc
		{
			[CompilerGenerated]
			add
			{
				Action action = this.m_HXWuHyOYnypyEaswgzVulxPMOfcc;
				Action action2;
				do
				{
					action2 = action;
					Action value2 = (Action)Delegate.Combine(action2, b);
					action = Interlocked.CompareExchange(ref this.m_HXWuHyOYnypyEaswgzVulxPMOfcc, value2, action2);
				}
				while ((object)action != action2);
			}
			[CompilerGenerated]
			remove
			{
				Action action = this.m_HXWuHyOYnypyEaswgzVulxPMOfcc;
				Action action2;
				do
				{
					action2 = action;
					Action value2 = (Action)Delegate.Remove(action2, value3);
					action = Interlocked.CompareExchange(ref this.m_HXWuHyOYnypyEaswgzVulxPMOfcc, value2, action2);
				}
				while ((object)action != action2);
			}
		}

		private event Action JZHOyikwjlFaUeGfviyQeUKgoPhNA
		{
			[CompilerGenerated]
			add
			{
				Action action = this.m_JZHOyikwjlFaUeGfviyQeUKgoPhNA;
				Action action2;
				do
				{
					action2 = action;
					Action value2 = (Action)Delegate.Combine(action2, b);
					action = Interlocked.CompareExchange(ref this.m_JZHOyikwjlFaUeGfviyQeUKgoPhNA, value2, action2);
				}
				while ((object)action != action2);
			}
			[CompilerGenerated]
			remove
			{
				Action action = this.m_JZHOyikwjlFaUeGfviyQeUKgoPhNA;
				Action action2;
				do
				{
					action2 = action;
					Action value2 = (Action)Delegate.Remove(action2, value3);
					action = Interlocked.CompareExchange(ref this.m_JZHOyikwjlFaUeGfviyQeUKgoPhNA, value2, action2);
				}
				while ((object)action != action2);
			}
		}

		internal event Action ELgzSCNUHMcXrkZIbwNubcEZCRLO
		{
			add
			{
				HXWuHyOYnypyEaswgzVulxPMOfcc += action;
			}
			remove
			{
				HXWuHyOYnypyEaswgzVulxPMOfcc -= action;
			}
		}

		internal event Action gyzaOhVNWBapcBNiAHNFidYiUZvDA
		{
			add
			{
				JZHOyikwjlFaUeGfviyQeUKgoPhNA += action;
			}
			remove
			{
				JZHOyikwjlFaUeGfviyQeUKgoPhNA -= action;
			}
		}

		internal IUnifiedKeyboardSource CgmdcXLZYwNvCEAMqCnUidmgmAhq()
		{
			return xlIWKgIlcUpoKaNkxzvuwinSbYWU;
		}

		internal IUnifiedMouseSource HcrPovgnNSheDcZTplDJNufqleGA()
		{
			return VpUGsxmUWaykGrtssctpOkJHaaiM;
		}

		public CustomInputSource(int P_0)
		{
			if (!Enum.IsDefined(typeof(InputSource), P_0))
			{
				Logger.LogError("Unknown InputSource (" + P_0 + ")!");
			}
			ksFnQGKIXSmvvpvxpjJWEBkvNFqM = (InputSource)P_0;
			UglIohQjisfVjcxxHTogJQZZldvbA = new List<Joystick>();
			nAdhEBeVnyoYsMaczkusZmNUdNucA = new ReadOnlyCollection<Joystick>(UglIohQjisfVjcxxHTogJQZZldvbA);
		}

		internal CustomInputSource(int P_0, IUnifiedKeyboardSource P_1, IUnifiedMouseSource P_2)
			: this(P_0)
		{
			xlIWKgIlcUpoKaNkxzvuwinSbYWU = P_1;
			VpUGsxmUWaykGrtssctpOkJHaaiM = P_2;
		}

		internal virtual void BrcOHbBoQMdzRAEoYlowdclvaIsvA()
		{
			OnInitialize();
		}

		protected virtual void OnInitialize()
		{
		}

		public void AddJoystick(Joystick joystick)
		{
			if (joystick == null)
			{
				return;
			}
			if (UglIohQjisfVjcxxHTogJQZZldvbA.Contains(joystick))
			{
				Logger.LogWarning("The joystick is already in the list. Cannot add again.");
				return;
			}
			UglIohQjisfVjcxxHTogJQZZldvbA.Add(joystick);
			joystick.ConnectedStateChangedEvent += IkUXxmRapwVCdLwdGXjEzESiLezD;
			if (joystick.isConnected)
			{
				OnJoystickConnected();
			}
		}

		public void RemoveJoystick(Joystick joystick)
		{
			if (joystick == null)
			{
				return;
			}
			if (!UglIohQjisfVjcxxHTogJQZZldvbA.Contains(joystick))
			{
				Logger.LogWarning("The joystick was not found in the list. Cannot remove.");
				return;
			}
			UglIohQjisfVjcxxHTogJQZZldvbA.Remove(joystick);
			joystick.ConnectedStateChangedEvent -= IkUXxmRapwVCdLwdGXjEzESiLezD;
			if (joystick.isConnected)
			{
				OnJoystickDisconnected();
			}
		}

		public IList<Joystick> GetJoysticks()
		{
			return nAdhEBeVnyoYsMaczkusZmNUdNucA;
		}

		protected virtual void OnJoystickConnected()
		{
			if (this.HXWuHyOYnypyEaswgzVulxPMOfcc != null)
			{
				this.HXWuHyOYnypyEaswgzVulxPMOfcc();
			}
		}

		protected virtual void OnJoystickDisconnected()
		{
			if (this.JZHOyikwjlFaUeGfviyQeUKgoPhNA != null)
			{
				this.JZHOyikwjlFaUeGfviyQeUKgoPhNA();
			}
		}

		private void IkUXxmRapwVCdLwdGXjEzESiLezD(bool P_0)
		{
			if (P_0)
			{
				OnJoystickConnected();
			}
			else
			{
				OnJoystickDisconnected();
			}
		}

		internal Joystick[] YXuIcbyhIzTJDUqKWkzLEETsGHeV()
		{
			List<Joystick> list = new List<Joystick>(UglIohQjisfVjcxxHTogJQZZldvbA.Count);
			for (int i = 0; i < UglIohQjisfVjcxxHTogJQZZldvbA.Count; i++)
			{
				Joystick joystick = UglIohQjisfVjcxxHTogJQZZldvbA[i];
				if (joystick != null && joystick.isConnected)
				{
					list.Add(joystick);
				}
			}
			return list.ToArray();
		}

		internal virtual void nmiHBHNfCudAZvaUpaMnJnprHnMy()
		{
		}

		public virtual void Dispose()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}

		void IDisposable.Dispose()
		{
			//ILSpy generated this explicit interface implementation from .override directive in Dispose
			this.Dispose();
		}

		~CustomInputSource()
		{
			Dispose(disposing: false);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (vGEqgUBUqAVznvCQMjpUjvyBpZkE)
			{
				return;
			}
			if (disposing)
			{
				if (xlIWKgIlcUpoKaNkxzvuwinSbYWU is IDisposable)
				{
					try
					{
						(xlIWKgIlcUpoKaNkxzvuwinSbYWU as IDisposable).Dispose();
					}
					catch (Exception msg)
					{
						Logger.LogError(msg);
					}
				}
				if (VpUGsxmUWaykGrtssctpOkJHaaiM is IDisposable)
				{
					try
					{
						(VpUGsxmUWaykGrtssctpOkJHaaiM as IDisposable).Dispose();
					}
					catch (Exception msg2)
					{
						Logger.LogError(msg2);
					}
				}
			}
			vGEqgUBUqAVznvCQMjpUjvyBpZkE = true;
		}

		public abstract void Update();
	}
}
