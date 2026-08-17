using System;
using System.Collections.Generic;

namespace Rewired.Utils.Classes.Utility
{
	[CustomObfuscation(rename = false)]
	[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
	internal abstract class ValueWatcher
	{
		public enum XbZpuJXfjSgeJhLNFTihdNrWdKZAA
		{
			ValueChanged = 0
		}

		public abstract bool changed { get; }

		public abstract bool autoTriggerEvent { get; set; }

		public abstract bool Update();

		public abstract bool Use();

		public abstract bool TriggerEvent();

		public abstract void AddEventListener(XbZpuJXfjSgeJhLNFTihdNrWdKZAA eventType, Delegate listener);

		public abstract void RemoveEventListener(XbZpuJXfjSgeJhLNFTihdNrWdKZAA eventType, Delegate listener);
	}
	[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
	[CustomObfuscation(rename = false)]
	internal sealed class ValueWatcher<T> : ValueWatcher
	{
		private static IEqualityComparer<T> suMDhezeJHkJqletOMyLOpnVArbx = EqualityComparerNoAlloc<T>.Default;

		private bool YdQriBdGGvAqsKFtyxZnRoVXpGIB;

		private T ckUkzuSDJGuJonmPceZrDLTKWbwc;

		private bool iwKVgRFJRKamjQxYRpyVLqqRWXxJ;

		private Func<T> XwzDgvMWltiflzVyqEhvijYXnoqzA;

		private Action<T> FFXtbgAfoebxOqlQslBSuuhHySjm;

		public override bool changed => YdQriBdGGvAqsKFtyxZnRoVXpGIB;

		public override bool autoTriggerEvent
		{
			get
			{
				return iwKVgRFJRKamjQxYRpyVLqqRWXxJ;
			}
			set
			{
				iwKVgRFJRKamjQxYRpyVLqqRWXxJ = value;
			}
		}

		public Func<T> getValueDelegate
		{
			get
			{
				return XwzDgvMWltiflzVyqEhvijYXnoqzA;
			}
			set
			{
				XwzDgvMWltiflzVyqEhvijYXnoqzA = value;
			}
		}

		public T value => ckUkzuSDJGuJonmPceZrDLTKWbwc;

		public event Action<T> ChangedEvent
		{
			add
			{
				FFXtbgAfoebxOqlQslBSuuhHySjm = (Action<T>)Delegate.Combine(FFXtbgAfoebxOqlQslBSuuhHySjm, value);
			}
			remove
			{
				FFXtbgAfoebxOqlQslBSuuhHySjm = (Action<T>)Delegate.Remove(FFXtbgAfoebxOqlQslBSuuhHySjm, value);
			}
		}

		public ValueWatcher(T P_0, bool P_1)
		{
			ckUkzuSDJGuJonmPceZrDLTKWbwc = P_0;
			iwKVgRFJRKamjQxYRpyVLqqRWXxJ = P_1;
		}

		public ValueWatcher(T P_0, Func<T> P_1, bool P_2)
			: this(P_0, P_2)
		{
			XwzDgvMWltiflzVyqEhvijYXnoqzA = P_1;
		}

		public override bool Update()
		{
			if (XwzDgvMWltiflzVyqEhvijYXnoqzA == null)
			{
				return false;
			}
			try
			{
				return Set(XwzDgvMWltiflzVyqEhvijYXnoqzA());
			}
			catch (Exception ex)
			{
				Logger.LogError("An exception was thrown by getValueDelegate.\n" + ex);
				return false;
			}
		}

		public override bool Use()
		{
			if (!YdQriBdGGvAqsKFtyxZnRoVXpGIB)
			{
				return false;
			}
			YdQriBdGGvAqsKFtyxZnRoVXpGIB = false;
			return true;
		}

		public override bool TriggerEvent()
		{
			if (!YdQriBdGGvAqsKFtyxZnRoVXpGIB)
			{
				return false;
			}
			if (FFXtbgAfoebxOqlQslBSuuhHySjm == null)
			{
				return true;
			}
			try
			{
				Use();
				FFXtbgAfoebxOqlQslBSuuhHySjm(ckUkzuSDJGuJonmPceZrDLTKWbwc);
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
			if (suMDhezeJHkJqletOMyLOpnVArbx.Equals(ckUkzuSDJGuJonmPceZrDLTKWbwc, value))
			{
				return false;
			}
			ckUkzuSDJGuJonmPceZrDLTKWbwc = value;
			YdQriBdGGvAqsKFtyxZnRoVXpGIB = true;
			if (iwKVgRFJRKamjQxYRpyVLqqRWXxJ)
			{
				TriggerEvent();
			}
			return true;
		}

		public override void AddEventListener(XbZpuJXfjSgeJhLNFTihdNrWdKZAA eventType, Delegate listener)
		{
			if (eventType == XbZpuJXfjSgeJhLNFTihdNrWdKZAA.ValueChanged)
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

		public override void RemoveEventListener(XbZpuJXfjSgeJhLNFTihdNrWdKZAA eventType, Delegate listener)
		{
			if (eventType == XbZpuJXfjSgeJhLNFTihdNrWdKZAA.ValueChanged)
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
