using System;
using System.Collections.Generic;

namespace Rewired.Utils.Classes.Utility
{
	[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
	[CustomObfuscation(rename = false)]
	internal abstract class ValueWatcher
	{
		public enum nSneEBjUCsFZYRbmbjspiWiieSEfb
		{
			ValueChanged = 0
		}

		public abstract bool changed { get; }

		public abstract bool autoTriggerEvent { get; set; }

		public abstract bool Update();

		public abstract bool Use();

		public abstract bool TriggerEvent();

		public abstract void AddEventListener(nSneEBjUCsFZYRbmbjspiWiieSEfb eventType, Delegate listener);

		public abstract void RemoveEventListener(nSneEBjUCsFZYRbmbjspiWiieSEfb eventType, Delegate listener);
	}
	[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
	[CustomObfuscation(rename = false)]
	internal sealed class ValueWatcher<T> : ValueWatcher
	{
		private static IEqualityComparer<T> AHYtglhpUoiOvzIeyCIFkpaPRGqP = EqualityComparerNoAlloc<T>.Default;

		private bool MTodEZdpIHHSbnrCXCvzqCfdesYeA;

		private T GwIHrSsCflLHgIzJvGdhJodKHgiI;

		private bool ISVuSVerBswRapBeKiOelGxnfNfbA;

		private Func<T> JjRRBeHClBgvddhdTaJwLggqMVLtA;

		private Action<T> BrZQkRGnKZvSTfmBOxJwDkIPFeyL;

		bool ValueWatcher.changed => MTodEZdpIHHSbnrCXCvzqCfdesYeA;

		bool ValueWatcher.autoTriggerEvent
		{
			get
			{
				return ISVuSVerBswRapBeKiOelGxnfNfbA;
			}
			set
			{
				ISVuSVerBswRapBeKiOelGxnfNfbA = value;
			}
		}

		public Func<T> getValueDelegate
		{
			get
			{
				return JjRRBeHClBgvddhdTaJwLggqMVLtA;
			}
			set
			{
				JjRRBeHClBgvddhdTaJwLggqMVLtA = value;
			}
		}

		public T value => GwIHrSsCflLHgIzJvGdhJodKHgiI;

		public event Action<T> ChangedEvent
		{
			add
			{
				BrZQkRGnKZvSTfmBOxJwDkIPFeyL = (Action<T>)Delegate.Combine(BrZQkRGnKZvSTfmBOxJwDkIPFeyL, value);
			}
			remove
			{
				BrZQkRGnKZvSTfmBOxJwDkIPFeyL = (Action<T>)Delegate.Remove(BrZQkRGnKZvSTfmBOxJwDkIPFeyL, value);
			}
		}

		public ValueWatcher(T P_0, bool P_1)
		{
			GwIHrSsCflLHgIzJvGdhJodKHgiI = P_0;
			ISVuSVerBswRapBeKiOelGxnfNfbA = P_1;
		}

		public ValueWatcher(T P_0, Func<T> P_1, bool P_2)
			: this(P_0, P_2)
		{
			JjRRBeHClBgvddhdTaJwLggqMVLtA = P_1;
		}

		public override bool Update()
		{
			if (JjRRBeHClBgvddhdTaJwLggqMVLtA == null)
			{
				return false;
			}
			try
			{
				return Set(JjRRBeHClBgvddhdTaJwLggqMVLtA());
			}
			catch (Exception ex)
			{
				Logger.LogError("An exception was thrown by getValueDelegate.\n" + ex);
				return false;
			}
		}

		public override bool Use()
		{
			if (!MTodEZdpIHHSbnrCXCvzqCfdesYeA)
			{
				return false;
			}
			MTodEZdpIHHSbnrCXCvzqCfdesYeA = false;
			return true;
		}

		public override bool TriggerEvent()
		{
			if (!MTodEZdpIHHSbnrCXCvzqCfdesYeA)
			{
				return false;
			}
			if (BrZQkRGnKZvSTfmBOxJwDkIPFeyL == null)
			{
				return true;
			}
			try
			{
				Use();
				BrZQkRGnKZvSTfmBOxJwDkIPFeyL(GwIHrSsCflLHgIzJvGdhJodKHgiI);
				return true;
			}
			catch (Exception ex)
			{
				Logger.LogError("An exception was thrown by ValueChangedEvent handler.\n" + ex);
				return false;
			}
		}

		public bool Set(T value)
		{
			if (AHYtglhpUoiOvzIeyCIFkpaPRGqP.Equals(GwIHrSsCflLHgIzJvGdhJodKHgiI, value))
			{
				return false;
			}
			GwIHrSsCflLHgIzJvGdhJodKHgiI = value;
			MTodEZdpIHHSbnrCXCvzqCfdesYeA = true;
			if (ISVuSVerBswRapBeKiOelGxnfNfbA)
			{
				TriggerEvent();
			}
			return true;
		}

		public override void AddEventListener(nSneEBjUCsFZYRbmbjspiWiieSEfb eventType, Delegate listener)
		{
			if (eventType == nSneEBjUCsFZYRbmbjspiWiieSEfb.ValueChanged)
			{
				if (!(listener is Action<T>))
				{
					throw new ArgumentException("listener must be of type Action<" + typeof(T).Name + ">");
				}
				ChangedEvent += (Action<T>)listener;
				return;
			}
			throw new NotImplementedException();
		}

		public override void RemoveEventListener(nSneEBjUCsFZYRbmbjspiWiieSEfb eventType, Delegate listener)
		{
			if (eventType == nSneEBjUCsFZYRbmbjspiWiieSEfb.ValueChanged)
			{
				if (!(listener is Action<T>))
				{
					throw new ArgumentException("listener must be of type Action<" + typeof(T).Name + ">");
				}
				ChangedEvent -= (Action<T>)listener;
				return;
			}
			throw new NotImplementedException();
		}
	}
}
