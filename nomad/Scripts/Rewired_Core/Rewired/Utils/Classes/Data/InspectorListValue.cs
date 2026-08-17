using System.Collections.Generic;
using System.ComponentModel;

namespace Rewired.Utils.Classes.Data
{
	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public sealed class InspectorListValue<T>
	{
		private IList<T> UGsZNPmqNXELgcIJhPeXcERfhkgNc;

		private readonly List<T> bhJNfrcHlgdWbKNxUTcRRjgCjMZU = new List<T>();

		private bool DIBvqUQxbaVbGAVRyACpeGqRdvVaA;

		public bool isSet => DIBvqUQxbaVbGAVRyACpeGqRdvVaA;

		public IList<T> value
		{
			get
			{
				return UGsZNPmqNXELgcIJhPeXcERfhkgNc;
			}
			set
			{
				UGsZNPmqNXELgcIJhPeXcERfhkgNc = value;
				DIBvqUQxbaVbGAVRyACpeGqRdvVaA = true;
				bhJNfrcHlgdWbKNxUTcRRjgCjMZU.Clear();
				if (UGsZNPmqNXELgcIJhPeXcERfhkgNc != null)
				{
					bhJNfrcHlgdWbKNxUTcRRjgCjMZU.AddRange(UGsZNPmqNXELgcIJhPeXcERfhkgNc);
				}
			}
		}

		public bool SetIfChanged(IList<T> value)
		{
			if (!DIBvqUQxbaVbGAVRyACpeGqRdvVaA)
			{
				this.value = value;
				return false;
			}
			if (UGsZNPmqNXELgcIJhPeXcERfhkgNc != value)
			{
				this.value = value;
				return true;
			}
			if (!pVPglOUNQXzSXiehAwVnrWXaYOuf(value, bhJNfrcHlgdWbKNxUTcRRjgCjMZU))
			{
				this.value = value;
				return true;
			}
			return false;
		}

		public void Clear()
		{
			DIBvqUQxbaVbGAVRyACpeGqRdvVaA = false;
			UGsZNPmqNXELgcIJhPeXcERfhkgNc = null;
			bhJNfrcHlgdWbKNxUTcRRjgCjMZU.Clear();
		}

		private static bool pVPglOUNQXzSXiehAwVnrWXaYOuf(IList<T> P_0, IList<T> P_1)
		{
			if (P_0 == P_1)
			{
				return true;
			}
			if (P_0 == null != (P_1 == null))
			{
				return false;
			}
			if (P_0.Count != P_1.Count)
			{
				return false;
			}
			for (int i = 0; i < P_0.Count; i++)
			{
				if (!EqualityComparer<T>.Default.Equals(P_0[i], P_1[i]))
				{
					return false;
				}
			}
			return true;
		}
	}
}
