using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Rewired.Platforms.Custom
{
	public abstract class CustomInputSource : IDisposable
	{
		public abstract class Controller
		{
			protected bool _isConnected;

			protected string _deviceName;

			protected string _customName;

			public string customName => _customName;

			public bool isConnected
			{
				get
				{
					return _isConnected;
				}
				set
				{
					if (value != _isConnected)
					{
						_ = _isConnected;
						_isConnected = value;
					}
				}
			}

			public string deviceName => _deviceName;

			protected Controller(string P_0)
			{
				_deviceName = P_0;
			}

			public void Disconnect()
			{
				if (_isConnected)
				{
					_isConnected = false;
				}
			}

			public void Connect()
			{
				if (!_isConnected)
				{
					_isConnected = true;
				}
			}

			public abstract void Update();
		}

		public abstract class Joystick : Controller
		{
			private long? orGeZnHyIQImwrQbTiysYDyAvZUR;

			private int xmEXVfyENtoyjKLfjizpdurnqqpi;

			private readonly Axis[] gmjcdmbtDXKatrtMKrVjuasYTyOh;

			private readonly Button[] YXuCILbbSuGPNMGgRTZoPeQmcMPsA;

			private readonly ReadOnlyCollection<Axis> cBSHMOCbWzIiiaZpZEDfrfjPCNNab;

			private readonly ReadOnlyCollection<Button> tnwAAWkpEQQDOblRgOpzbinWPPbqc;

			private bool OlbVRqsSBEtRqSittUWERZaqzxXk;

			private Rewired.Controller.Extension ieNssgrFeJiIpjsZwbsJdGwhvyJU;

			public long? systemId
			{
				get
				{
					return orGeZnHyIQImwrQbTiysYDyAvZUR;
				}
				protected set
				{
					orGeZnHyIQImwrQbTiysYDyAvZUR = value;
				}
			}

			public int unityId
			{
				get
				{
					return xmEXVfyENtoyjKLfjizpdurnqqpi;
				}
				protected set
				{
					xmEXVfyENtoyjKLfjizpdurnqqpi = value;
				}
			}

			public IList<Axis> Axes => cBSHMOCbWzIiiaZpZEDfrfjPCNNab;

			public IList<Button> Buttons => tnwAAWkpEQQDOblRgOpzbinWPPbqc;

			public bool supportsVibration
			{
				get
				{
					return OlbVRqsSBEtRqSittUWERZaqzxXk;
				}
				set
				{
					OlbVRqsSBEtRqSittUWERZaqzxXk = value;
				}
			}

			public Rewired.Controller.Extension extension
			{
				get
				{
					return ieNssgrFeJiIpjsZwbsJdGwhvyJU;
				}
				set
				{
					ieNssgrFeJiIpjsZwbsJdGwhvyJU = value;
				}
			}

			public int buttonCount => YXuCILbbSuGPNMGgRTZoPeQmcMPsA.Length;

			public int axisCount => gmjcdmbtDXKatrtMKrVjuasYTyOh.Length;

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
				orGeZnHyIQImwrQbTiysYDyAvZUR = P_1;
				xmEXVfyENtoyjKLfjizpdurnqqpi = P_2;
				gmjcdmbtDXKatrtMKrVjuasYTyOh = new Axis[P_3];
				YXuCILbbSuGPNMGgRTZoPeQmcMPsA = new Button[P_4];
				for (int i = 0; i < P_3; i++)
				{
					gmjcdmbtDXKatrtMKrVjuasYTyOh[i] = new Axis();
				}
				for (int j = 0; j < P_4; j++)
				{
					YXuCILbbSuGPNMGgRTZoPeQmcMPsA[j] = new Button();
				}
				cBSHMOCbWzIiiaZpZEDfrfjPCNNab = new ReadOnlyCollection<Axis>(gmjcdmbtDXKatrtMKrVjuasYTyOh);
				tnwAAWkpEQQDOblRgOpzbinWPPbqc = new ReadOnlyCollection<Button>(YXuCILbbSuGPNMGgRTZoPeQmcMPsA);
			}

			public virtual float GetAxisValue(int index)
			{
				if (index < 0 || index >= gmjcdmbtDXKatrtMKrVjuasYTyOh.Length)
				{
					return 0f;
				}
				return gmjcdmbtDXKatrtMKrVjuasYTyOh[index].value;
			}

			public virtual bool GetButtonValue(int index)
			{
				if (index < 0 || index >= YXuCILbbSuGPNMGgRTZoPeQmcMPsA.Length)
				{
					return false;
				}
				return YXuCILbbSuGPNMGgRTZoPeQmcMPsA[index].value;
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
			public bool value;
		}

		private readonly InputSource mKGFJudopEcewqKpuxRtaCtIVgIn;

		private readonly List<Joystick> lacEMsSikdfQQWUospJtpSoUfDdS;

		private readonly ReadOnlyCollection<Joystick> STDYSyyforIsYvzkZoBubRXRkbEE;

		private bool CJusUpshQKVHuGVwEaiCDFqiDoUFA = true;

		[CompilerGenerated]
		private Action m_nOYSRzstOJbWcLYZLMjrCoJoGBEN;

		[CompilerGenerated]
		private Action m_SLDNNFtlUUCYOMZYUcmjzNuvBZei;

		private bool AeWaeWamxRrERkciQkpFDWbRfZMkA;

		public bool useApproximateMatching
		{
			get
			{
				return CJusUpshQKVHuGVwEaiCDFqiDoUFA;
			}
			protected set
			{
				CJusUpshQKVHuGVwEaiCDFqiDoUFA = value;
			}
		}

		internal InputSource twtzIKfuPcwLuTdbfRDPmfUOUduI => mKGFJudopEcewqKpuxRtaCtIVgIn;

		public abstract bool isReady { get; }

		private event Action nOYSRzstOJbWcLYZLMjrCoJoGBEN
		{
			[CompilerGenerated]
			add
			{
				Action action = this.m_nOYSRzstOJbWcLYZLMjrCoJoGBEN;
				Action action2;
				do
				{
					action2 = action;
					Action value2 = (Action)Delegate.Combine(action2, b);
					action = Interlocked.CompareExchange(ref this.m_nOYSRzstOJbWcLYZLMjrCoJoGBEN, value2, action2);
				}
				while ((object)action != action2);
			}
			[CompilerGenerated]
			remove
			{
				Action action = this.m_nOYSRzstOJbWcLYZLMjrCoJoGBEN;
				Action action2;
				do
				{
					action2 = action;
					Action value2 = (Action)Delegate.Remove(action2, value3);
					action = Interlocked.CompareExchange(ref this.m_nOYSRzstOJbWcLYZLMjrCoJoGBEN, value2, action2);
				}
				while ((object)action != action2);
			}
		}

		private event Action SLDNNFtlUUCYOMZYUcmjzNuvBZei
		{
			[CompilerGenerated]
			add
			{
				Action action = this.m_SLDNNFtlUUCYOMZYUcmjzNuvBZei;
				Action action2;
				do
				{
					action2 = action;
					Action value2 = (Action)Delegate.Combine(action2, b);
					action = Interlocked.CompareExchange(ref this.m_SLDNNFtlUUCYOMZYUcmjzNuvBZei, value2, action2);
				}
				while ((object)action != action2);
			}
			[CompilerGenerated]
			remove
			{
				Action action = this.m_SLDNNFtlUUCYOMZYUcmjzNuvBZei;
				Action action2;
				do
				{
					action2 = action;
					Action value2 = (Action)Delegate.Remove(action2, value3);
					action = Interlocked.CompareExchange(ref this.m_SLDNNFtlUUCYOMZYUcmjzNuvBZei, value2, action2);
				}
				while ((object)action != action2);
			}
		}

		internal event Action IjBZhVnpMDVJOoHiocMFmfzWUjXD
		{
			add
			{
				nOYSRzstOJbWcLYZLMjrCoJoGBEN += action;
			}
			remove
			{
				nOYSRzstOJbWcLYZLMjrCoJoGBEN -= action;
			}
		}

		internal event Action bEQVXqVyYwXaKGcXFdnuHwwjmrcf
		{
			add
			{
				SLDNNFtlUUCYOMZYUcmjzNuvBZei += action;
			}
			remove
			{
				SLDNNFtlUUCYOMZYUcmjzNuvBZei -= action;
			}
		}

		public CustomInputSource(int P_0)
		{
			if (!Enum.IsDefined(typeof(InputSource), P_0))
			{
				Logger.LogError("Unknown InputSource (" + P_0 + ")!");
			}
			mKGFJudopEcewqKpuxRtaCtIVgIn = (InputSource)P_0;
			lacEMsSikdfQQWUospJtpSoUfDdS = new List<Joystick>();
			STDYSyyforIsYvzkZoBubRXRkbEE = new ReadOnlyCollection<Joystick>(lacEMsSikdfQQWUospJtpSoUfDdS);
		}

		public void AddJoystick(Joystick joystick)
		{
			if (joystick != null)
			{
				if (lacEMsSikdfQQWUospJtpSoUfDdS.Contains(joystick))
				{
					Logger.LogWarning("The joystick is already in the list. Cannot add again.");
				}
				else
				{
					lacEMsSikdfQQWUospJtpSoUfDdS.Add(joystick);
				}
			}
		}

		public void RemoveJoystick(Joystick joystick)
		{
			if (joystick != null)
			{
				if (!lacEMsSikdfQQWUospJtpSoUfDdS.Contains(joystick))
				{
					Logger.LogWarning("The joystick was not found in the list. Cannot remove.");
				}
				else
				{
					lacEMsSikdfQQWUospJtpSoUfDdS.Remove(joystick);
				}
			}
		}

		public IList<Joystick> GetJoysticks()
		{
			return STDYSyyforIsYvzkZoBubRXRkbEE;
		}

		protected virtual void OnJoystickConnected()
		{
			if (this.nOYSRzstOJbWcLYZLMjrCoJoGBEN != null)
			{
				this.nOYSRzstOJbWcLYZLMjrCoJoGBEN();
			}
		}

		protected virtual void OnJoystickDisconnected()
		{
			if (this.SLDNNFtlUUCYOMZYUcmjzNuvBZei != null)
			{
				this.SLDNNFtlUUCYOMZYUcmjzNuvBZei();
			}
		}

		internal Joystick[] VLkpLxcdERXBKIOsaWtDTHIzOMAS()
		{
			List<Joystick> list = new List<Joystick>(lacEMsSikdfQQWUospJtpSoUfDdS.Count);
			for (int i = 0; i < lacEMsSikdfQQWUospJtpSoUfDdS.Count; i++)
			{
				Joystick joystick = lacEMsSikdfQQWUospJtpSoUfDdS[i];
				if (joystick != null && joystick.isConnected)
				{
					list.Add(joystick);
				}
			}
			return list.ToArray();
		}

		public virtual void Dispose()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}

		~CustomInputSource()
		{
			Dispose(disposing: false);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!AeWaeWamxRrERkciQkpFDWbRfZMkA)
			{
				AeWaeWamxRrERkciQkpFDWbRfZMkA = true;
			}
		}

		public abstract void Update();
	}
}
