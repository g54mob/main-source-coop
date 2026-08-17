using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Rewired.Utils.Classes.Data
{
	[Serializable]
	[DefaultMember("Item")]
	[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
	[CustomObfuscation(rename = false)]
	internal sealed class MappedArray<T> : IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable, IList, ICollection
	{
		[Serializable]
		public struct wQNJwVeAfshPXGJRCHaFsNaQlRjXA : IEnumerator<T>, IEnumerator, IDisposable
		{
			private MappedArray<T> array;

			private int index;

			private int version;

			private T current;

			T IEnumerator<T>.Current => current;

			object IEnumerator.Current
			{
				get
				{
					if (index == 0 || index == array.Length + 1)
					{
						throw new InvalidOperationException();
					}
					return this.Current;
				}
			}

			internal wQNJwVeAfshPXGJRCHaFsNaQlRjXA(MappedArray<T> P_0)
			{
				array = P_0;
				index = 0;
				version = P_0.ePElqxFaQlXURnadNSswddoLkOMB;
				current = default(T);
			}

			public void Dispose()
			{
			}

			void IDisposable.Dispose()
			{
				//ILSpy generated this explicit interface implementation from .override directive in Dispose
				this.Dispose();
			}

			public bool MoveNext()
			{
				MappedArray<T> mappedArray = array;
				if (version == mappedArray.ePElqxFaQlXURnadNSswddoLkOMB && (uint)index < (uint)mappedArray.Length)
				{
					current = mappedArray.QZaDDBEottEuMTOosXLmjbtPFLgzA[mappedArray.vGehOZCsmExsWgzPmGnUzsQVNYHh(index)];
					index++;
					return true;
				}
				return VvAFFScibSUPZkygDXQmbsZwtJUR();
			}

			bool IEnumerator.MoveNext()
			{
				//ILSpy generated this explicit interface implementation from .override directive in MoveNext
				return this.MoveNext();
			}

			private bool VvAFFScibSUPZkygDXQmbsZwtJUR()
			{
				if (version != array.ePElqxFaQlXURnadNSswddoLkOMB)
				{
					throw new InvalidOperationException("List was changed.");
				}
				index = array.Length + 1;
				current = default(T);
				return false;
			}

			void IEnumerator.Reset()
			{
				if (version != array.ePElqxFaQlXURnadNSswddoLkOMB)
				{
					throw new InvalidOperationException("List was changed.");
				}
				index = 0;
				current = default(T);
			}
		}

		private T[] QZaDDBEottEuMTOosXLmjbtPFLgzA;

		private int ePElqxFaQlXURnadNSswddoLkOMB;

		private Func<int, int> IVObrEANMynbOwWTPUGNvDUdECpJA;

		public Func<int, int> indexMap
		{
			get
			{
				return IVObrEANMynbOwWTPUGNvDUdECpJA;
			}
			set
			{
				IVObrEANMynbOwWTPUGNvDUdECpJA = value;
				ePElqxFaQlXURnadNSswddoLkOMB++;
			}
		}

		T IList<T>.this[int index]
		{
			get
			{
				return QZaDDBEottEuMTOosXLmjbtPFLgzA[vGehOZCsmExsWgzPmGnUzsQVNYHh(index)];
			}
			set
			{
				QZaDDBEottEuMTOosXLmjbtPFLgzA[vGehOZCsmExsWgzPmGnUzsQVNYHh(index)] = value;
			}
		}

		public int Length => QZaDDBEottEuMTOosXLmjbtPFLgzA.Length;

		int ICollection<T>.Count => QZaDDBEottEuMTOosXLmjbtPFLgzA.Length;

		bool ICollection<T>.IsReadOnly => ((ICollection<T>)QZaDDBEottEuMTOosXLmjbtPFLgzA).IsReadOnly;

		object IList.this[int index]
		{
			get
			{
				return ((IList)QZaDDBEottEuMTOosXLmjbtPFLgzA)[vGehOZCsmExsWgzPmGnUzsQVNYHh(index)];
			}
			set
			{
				((IList)QZaDDBEottEuMTOosXLmjbtPFLgzA)[vGehOZCsmExsWgzPmGnUzsQVNYHh(index)] = value;
			}
		}

		int ICollection.Count => QZaDDBEottEuMTOosXLmjbtPFLgzA.Length;

		bool IList.IsFixedSize => ((IList)QZaDDBEottEuMTOosXLmjbtPFLgzA).IsFixedSize;

		object ICollection.SyncRoot => ((ICollection)QZaDDBEottEuMTOosXLmjbtPFLgzA).SyncRoot;

		bool ICollection.IsSynchronized => ((ICollection)QZaDDBEottEuMTOosXLmjbtPFLgzA).IsSynchronized;

		public MappedArray(T[] P_0, Func<int, int> P_1)
		{
			QZaDDBEottEuMTOosXLmjbtPFLgzA = P_0;
			IVObrEANMynbOwWTPUGNvDUdECpJA = P_1;
		}

		public void Add(T item)
		{
			throw new NotImplementedException();
		}

		void ICollection<T>.Add(T item)
		{
			//ILSpy generated this explicit interface implementation from .override directive in Add
			this.Add(item);
		}

		public void Clear()
		{
			Array.Clear(QZaDDBEottEuMTOosXLmjbtPFLgzA, 0, QZaDDBEottEuMTOosXLmjbtPFLgzA.Length);
		}

		void ICollection<T>.Clear()
		{
			//ILSpy generated this explicit interface implementation from .override directive in Clear
			this.Clear();
		}

		void IList.Clear()
		{
			//ILSpy generated this explicit interface implementation from .override directive in Clear
			this.Clear();
		}

		public bool Contains(T item)
		{
			return QZaDDBEottEuMTOosXLmjbtPFLgzA.Contains(item);
		}

		bool ICollection<T>.Contains(T item)
		{
			//ILSpy generated this explicit interface implementation from .override directive in Contains
			return this.Contains(item);
		}

		public void CopyTo(T[] array, int arrayIndex)
		{
			QZaDDBEottEuMTOosXLmjbtPFLgzA.CopyTo(array, arrayIndex);
		}

		void ICollection<T>.CopyTo(T[] array, int arrayIndex)
		{
			//ILSpy generated this explicit interface implementation from .override directive in CopyTo
			this.CopyTo(array, arrayIndex);
		}

		public IEnumerator<T> GetEnumerator()
		{
			return new wQNJwVeAfshPXGJRCHaFsNaQlRjXA(this);
		}

		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			//ILSpy generated this explicit interface implementation from .override directive in GetEnumerator
			return this.GetEnumerator();
		}

		public int IndexOf(T item)
		{
			return vGehOZCsmExsWgzPmGnUzsQVNYHh(((IList<T>)QZaDDBEottEuMTOosXLmjbtPFLgzA).IndexOf(item));
		}

		int IList<T>.IndexOf(T item)
		{
			//ILSpy generated this explicit interface implementation from .override directive in IndexOf
			return this.IndexOf(item);
		}

		private void JshadtnYIMcKvnyfUKlORhGXeoEQ(int P_0, T P_1)
		{
			throw new NotImplementedException();
		}

		void IList<T>.Insert(int P_0, T P_1)
		{
			//ILSpy generated this explicit interface implementation from .override directive in JshadtnYIMcKvnyfUKlORhGXeoEQ
			this.JshadtnYIMcKvnyfUKlORhGXeoEQ(P_0, P_1);
		}

		private bool cthneaifOLrhKbEISsrElNgYclFX(T P_0)
		{
			throw new NotImplementedException();
		}

		bool ICollection<T>.Remove(T P_0)
		{
			//ILSpy generated this explicit interface implementation from .override directive in cthneaifOLrhKbEISsrElNgYclFX
			return this.cthneaifOLrhKbEISsrElNgYclFX(P_0);
		}

		private void dTjXofFzpdgDjHsTpwpPLlpJSWSc(int P_0)
		{
			throw new NotImplementedException();
		}

		void IList<T>.RemoveAt(int P_0)
		{
			//ILSpy generated this explicit interface implementation from .override directive in dTjXofFzpdgDjHsTpwpPLlpJSWSc
			this.dTjXofFzpdgDjHsTpwpPLlpJSWSc(P_0);
		}

		int IList.Add(object value)
		{
			throw new NotImplementedException();
		}

		bool IList.Contains(object value)
		{
			return ((IList)QZaDDBEottEuMTOosXLmjbtPFLgzA).Contains(value);
		}

		void ICollection.CopyTo(Array array, int index)
		{
			QZaDDBEottEuMTOosXLmjbtPFLgzA.CopyTo(array, index);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return new wQNJwVeAfshPXGJRCHaFsNaQlRjXA(this);
		}

		int IList.IndexOf(object value)
		{
			return ((IList)QZaDDBEottEuMTOosXLmjbtPFLgzA).IndexOf(value);
		}

		void IList.Insert(int index, object value)
		{
			throw new NotImplementedException();
		}

		void IList.Remove(object value)
		{
			throw new NotImplementedException();
		}

		void IList.RemoveAt(int index)
		{
			throw new NotImplementedException();
		}

		private int vGehOZCsmExsWgzPmGnUzsQVNYHh(int P_0)
		{
			if (IVObrEANMynbOwWTPUGNvDUdECpJA == null)
			{
				return P_0;
			}
			if (P_0 < 0 || P_0 >= QZaDDBEottEuMTOosXLmjbtPFLgzA.Length)
			{
				return P_0;
			}
			return IVObrEANMynbOwWTPUGNvDUdECpJA(P_0);
		}
	}
}
