using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Rewired.Utils.Interfaces;

namespace Rewired.Utils.Classes.Data
{
	[DefaultMember("Item")]
	[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
	[CustomObfuscation(rename = false)]
	internal sealed class ReadOnlyList<T> : Rewired.Utils.Interfaces.IReadOnlyList<T>, IReadOnlyList, IEnumerable<T>, IEnumerable
	{
		private readonly IList<T> gIpFmKFEvoscDpuSBnjsjdPjhiIi;

		int IReadOnlyList.Count => gIpFmKFEvoscDpuSBnjsjdPjhiIi.Count;

		T Rewired.Utils.Interfaces.IReadOnlyList<T>.this[int index] => gIpFmKFEvoscDpuSBnjsjdPjhiIi[index];

		object IReadOnlyList.this[int P_0] => (this as IList)[P_0];

		public ReadOnlyList(IList<T> P_0)
		{
			if (P_0 == null)
			{
				throw new ArgumentNullException();
			}
			gIpFmKFEvoscDpuSBnjsjdPjhiIi = P_0;
		}

		public ReadOnlyList(ReadOnlyList<T> P_0)
		{
			if (P_0 == null)
			{
				throw new ArgumentNullException();
			}
			gIpFmKFEvoscDpuSBnjsjdPjhiIi = new List<T>(P_0.gIpFmKFEvoscDpuSBnjsjdPjhiIi);
		}

		public bool Contains(T value)
		{
			return gIpFmKFEvoscDpuSBnjsjdPjhiIi.Contains(value);
		}

		bool Rewired.Utils.Interfaces.IReadOnlyList<T>.Contains(T value)
		{
			//ILSpy generated this explicit interface implementation from .override directive in Contains
			return this.Contains(value);
		}

		public int IndexOf(T value)
		{
			return gIpFmKFEvoscDpuSBnjsjdPjhiIi.IndexOf(value);
		}

		int Rewired.Utils.Interfaces.IReadOnlyList<T>.IndexOf(T value)
		{
			//ILSpy generated this explicit interface implementation from .override directive in IndexOf
			return this.IndexOf(value);
		}

		public void CopyTo(IList<T> destination)
		{
			if (destination == null)
			{
				throw new ArgumentNullException();
			}
			for (int i = 0; i < gIpFmKFEvoscDpuSBnjsjdPjhiIi.Count; i++)
			{
				destination.Add(gIpFmKFEvoscDpuSBnjsjdPjhiIi[i]);
			}
		}

		private int rqalgraZLnQwOkMiYuPmkibdkTJn(object P_0)
		{
			return (this as IList).IndexOf(P_0);
		}

		int IReadOnlyList.IndexOf(object P_0)
		{
			//ILSpy generated this explicit interface implementation from .override directive in rqalgraZLnQwOkMiYuPmkibdkTJn
			return this.rqalgraZLnQwOkMiYuPmkibdkTJn(P_0);
		}

		private bool qEDYJzgMZYhdTBedDztQLvbmjcgJ(object P_0)
		{
			return (this as IList).Contains(P_0);
		}

		bool IReadOnlyList.Contains(object P_0)
		{
			//ILSpy generated this explicit interface implementation from .override directive in qEDYJzgMZYhdTBedDztQLvbmjcgJ
			return this.qEDYJzgMZYhdTBedDztQLvbmjcgJ(P_0);
		}

		private IEnumerator<T> ORleQxObwwyfJVTEtYTELiAcesBK()
		{
			return gIpFmKFEvoscDpuSBnjsjdPjhiIi.GetEnumerator();
		}

		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			//ILSpy generated this explicit interface implementation from .override directive in ORleQxObwwyfJVTEtYTELiAcesBK
			return this.ORleQxObwwyfJVTEtYTELiAcesBK();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return gIpFmKFEvoscDpuSBnjsjdPjhiIi.GetEnumerator();
		}
	}
}
