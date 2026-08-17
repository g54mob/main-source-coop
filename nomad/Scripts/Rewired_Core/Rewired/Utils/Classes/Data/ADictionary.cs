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
	internal class ADictionary<TKey, TValue> : IDictionary<TKey, TValue>, ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable, IDictionary, ICollection, Rewired.Utils.Interfaces.IReadOnlyDictionary<TKey, TValue>
	{
		[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
		[CustomObfuscation(rename = false)]
		internal struct Entry
		{
			public int hashCode;

			public int next;

			public TKey key;

			public TValue value;
		}

		[Serializable]
		[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
		[CustomObfuscation(rename = false)]
		public struct Enumerator : IEnumerator<KeyValuePair<TKey, TValue>>, IEnumerator, IDisposable, IDictionaryEnumerator
		{
			private ADictionary<TKey, TValue> SyqgUbfMCPnQoqztMcwSLYdzfmcn;

			private int FZuFoUGQChNzjfaLKGWKGYPtaNRA;

			private int TawOIXiRJDIhPWwkgrARpZxGtKpX;

			private KeyValuePair<TKey, TValue> WOxHlzFmDuIXJAVxHgtQDPYCIljcB;

			private int ZHWgUWkQCGzOvffANfSCbGaTNCDA;

			internal const int DictEntry = 1;

			internal const int KeyValuePair = 2;

			KeyValuePair<TKey, TValue> IEnumerator<KeyValuePair<TKey, TValue>>.Current => WOxHlzFmDuIXJAVxHgtQDPYCIljcB;

			object IEnumerator.Current
			{
				get
				{
					if (TawOIXiRJDIhPWwkgrARpZxGtKpX == 0 || TawOIXiRJDIhPWwkgrARpZxGtKpX == SyqgUbfMCPnQoqztMcwSLYdzfmcn._count + 1)
					{
						throw new Exception();
					}
					if (ZHWgUWkQCGzOvffANfSCbGaTNCDA == 1)
					{
						return new DictionaryEntry(WOxHlzFmDuIXJAVxHgtQDPYCIljcB.Key, WOxHlzFmDuIXJAVxHgtQDPYCIljcB.Value);
					}
					return new KeyValuePair<TKey, TValue>(WOxHlzFmDuIXJAVxHgtQDPYCIljcB.Key, WOxHlzFmDuIXJAVxHgtQDPYCIljcB.Value);
				}
			}

			DictionaryEntry IDictionaryEnumerator.Entry
			{
				get
				{
					if (TawOIXiRJDIhPWwkgrARpZxGtKpX == 0 || TawOIXiRJDIhPWwkgrARpZxGtKpX == SyqgUbfMCPnQoqztMcwSLYdzfmcn._count + 1)
					{
						throw new Exception();
					}
					return new DictionaryEntry(WOxHlzFmDuIXJAVxHgtQDPYCIljcB.Key, WOxHlzFmDuIXJAVxHgtQDPYCIljcB.Value);
				}
			}

			object IDictionaryEnumerator.Key
			{
				get
				{
					if (TawOIXiRJDIhPWwkgrARpZxGtKpX == 0 || TawOIXiRJDIhPWwkgrARpZxGtKpX == SyqgUbfMCPnQoqztMcwSLYdzfmcn._count + 1)
					{
						throw new Exception();
					}
					return WOxHlzFmDuIXJAVxHgtQDPYCIljcB.Key;
				}
			}

			object IDictionaryEnumerator.Value
			{
				get
				{
					if (TawOIXiRJDIhPWwkgrARpZxGtKpX == 0 || TawOIXiRJDIhPWwkgrARpZxGtKpX == SyqgUbfMCPnQoqztMcwSLYdzfmcn._count + 1)
					{
						throw new Exception();
					}
					return WOxHlzFmDuIXJAVxHgtQDPYCIljcB.Value;
				}
			}

			internal Enumerator(ADictionary<TKey, TValue> P_0, int P_1)
			{
				SyqgUbfMCPnQoqztMcwSLYdzfmcn = P_0;
				FZuFoUGQChNzjfaLKGWKGYPtaNRA = P_0.hKwuYVFlLYJjAwmAHIsXqLflDexp;
				TawOIXiRJDIhPWwkgrARpZxGtKpX = 0;
				ZHWgUWkQCGzOvffANfSCbGaTNCDA = P_1;
				WOxHlzFmDuIXJAVxHgtQDPYCIljcB = default(KeyValuePair<TKey, TValue>);
			}

			public bool MoveNext()
			{
				if (FZuFoUGQChNzjfaLKGWKGYPtaNRA != SyqgUbfMCPnQoqztMcwSLYdzfmcn.hKwuYVFlLYJjAwmAHIsXqLflDexp)
				{
					throw new Exception();
				}
				while ((uint)TawOIXiRJDIhPWwkgrARpZxGtKpX < (uint)SyqgUbfMCPnQoqztMcwSLYdzfmcn._count)
				{
					if (SyqgUbfMCPnQoqztMcwSLYdzfmcn._entries[TawOIXiRJDIhPWwkgrARpZxGtKpX].hashCode >= 0)
					{
						WOxHlzFmDuIXJAVxHgtQDPYCIljcB = new KeyValuePair<TKey, TValue>(SyqgUbfMCPnQoqztMcwSLYdzfmcn._entries[TawOIXiRJDIhPWwkgrARpZxGtKpX].key, SyqgUbfMCPnQoqztMcwSLYdzfmcn._entries[TawOIXiRJDIhPWwkgrARpZxGtKpX].value);
						TawOIXiRJDIhPWwkgrARpZxGtKpX++;
						return true;
					}
					TawOIXiRJDIhPWwkgrARpZxGtKpX++;
				}
				TawOIXiRJDIhPWwkgrARpZxGtKpX = SyqgUbfMCPnQoqztMcwSLYdzfmcn._count + 1;
				WOxHlzFmDuIXJAVxHgtQDPYCIljcB = default(KeyValuePair<TKey, TValue>);
				return false;
			}

			bool IEnumerator.MoveNext()
			{
				//ILSpy generated this explicit interface implementation from .override directive in MoveNext
				return this.MoveNext();
			}

			public void Dispose()
			{
			}

			void IDisposable.Dispose()
			{
				//ILSpy generated this explicit interface implementation from .override directive in Dispose
				this.Dispose();
			}

			void IEnumerator.Reset()
			{
				if (FZuFoUGQChNzjfaLKGWKGYPtaNRA != SyqgUbfMCPnQoqztMcwSLYdzfmcn.hKwuYVFlLYJjAwmAHIsXqLflDexp)
				{
					throw new Exception();
				}
				TawOIXiRJDIhPWwkgrARpZxGtKpX = 0;
				WOxHlzFmDuIXJAVxHgtQDPYCIljcB = default(KeyValuePair<TKey, TValue>);
			}
		}

		[Serializable]
		[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
		[CustomObfuscation(rename = false)]
		public sealed class KeyCollection : ICollection<TKey>, IEnumerable<TKey>, IEnumerable, ICollection
		{
			[Serializable]
			[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
			[CustomObfuscation(rename = false)]
			public struct Enumerator : IEnumerator<TKey>, IEnumerator, IDisposable
			{
				private ADictionary<TKey, TValue> XzIbLJgAyfEjSoxopbbtNWSsmUNx;

				private int FrSSlmpIgHliTJfeWyOtGDApRUSi;

				private int uSxnMZYEoRQscDcIaxInEvlWUdcn;

				private TKey ZDrEykZTPsxPqjagJQyyhOYiudZI;

				TKey IEnumerator<TKey>.Current => ZDrEykZTPsxPqjagJQyyhOYiudZI;

				object IEnumerator.Current
				{
					get
					{
						if (FrSSlmpIgHliTJfeWyOtGDApRUSi == 0 || FrSSlmpIgHliTJfeWyOtGDApRUSi == XzIbLJgAyfEjSoxopbbtNWSsmUNx._count + 1)
						{
							throw new Exception();
						}
						return ZDrEykZTPsxPqjagJQyyhOYiudZI;
					}
				}

				internal Enumerator(ADictionary<TKey, TValue> P_0)
				{
					XzIbLJgAyfEjSoxopbbtNWSsmUNx = P_0;
					uSxnMZYEoRQscDcIaxInEvlWUdcn = P_0.hKwuYVFlLYJjAwmAHIsXqLflDexp;
					FrSSlmpIgHliTJfeWyOtGDApRUSi = 0;
					ZDrEykZTPsxPqjagJQyyhOYiudZI = default(TKey);
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
					if (uSxnMZYEoRQscDcIaxInEvlWUdcn != XzIbLJgAyfEjSoxopbbtNWSsmUNx.hKwuYVFlLYJjAwmAHIsXqLflDexp)
					{
						throw new Exception();
					}
					while ((uint)FrSSlmpIgHliTJfeWyOtGDApRUSi < (uint)XzIbLJgAyfEjSoxopbbtNWSsmUNx._count)
					{
						if (XzIbLJgAyfEjSoxopbbtNWSsmUNx._entries[FrSSlmpIgHliTJfeWyOtGDApRUSi].hashCode >= 0)
						{
							ZDrEykZTPsxPqjagJQyyhOYiudZI = XzIbLJgAyfEjSoxopbbtNWSsmUNx._entries[FrSSlmpIgHliTJfeWyOtGDApRUSi].key;
							FrSSlmpIgHliTJfeWyOtGDApRUSi++;
							return true;
						}
						FrSSlmpIgHliTJfeWyOtGDApRUSi++;
					}
					FrSSlmpIgHliTJfeWyOtGDApRUSi = XzIbLJgAyfEjSoxopbbtNWSsmUNx._count + 1;
					ZDrEykZTPsxPqjagJQyyhOYiudZI = default(TKey);
					return false;
				}

				bool IEnumerator.MoveNext()
				{
					//ILSpy generated this explicit interface implementation from .override directive in MoveNext
					return this.MoveNext();
				}

				void IEnumerator.Reset()
				{
					if (uSxnMZYEoRQscDcIaxInEvlWUdcn != XzIbLJgAyfEjSoxopbbtNWSsmUNx.hKwuYVFlLYJjAwmAHIsXqLflDexp)
					{
						throw new Exception();
					}
					FrSSlmpIgHliTJfeWyOtGDApRUSi = 0;
					ZDrEykZTPsxPqjagJQyyhOYiudZI = default(TKey);
				}
			}

			private ADictionary<TKey, TValue> eLuCOKFwIyZPIDuvvMeMWLHVgweEb;

			int ICollection<TKey>.Count => eLuCOKFwIyZPIDuvvMeMWLHVgweEb.Count;

			bool ICollection<TKey>.IsReadOnly => true;

			bool ICollection.IsSynchronized => false;

			object ICollection.SyncRoot => ((ICollection)eLuCOKFwIyZPIDuvvMeMWLHVgweEb).SyncRoot;

			public KeyCollection(ADictionary<TKey, TValue> P_0)
			{
				if (P_0 == null)
				{
					throw new ArgumentNullException("dictionary");
				}
				eLuCOKFwIyZPIDuvvMeMWLHVgweEb = P_0;
			}

			public Enumerator GetEnumerator()
			{
				return new Enumerator(eLuCOKFwIyZPIDuvvMeMWLHVgweEb);
			}

			public void CopyTo(TKey[] array, int index)
			{
				if (array == null)
				{
					throw new ArgumentNullException("array");
				}
				if (index < 0 || index > array.Length)
				{
					throw new ArgumentOutOfRangeException("index");
				}
				if (array.Length - index < eLuCOKFwIyZPIDuvvMeMWLHVgweEb.Count)
				{
					throw new Exception();
				}
				int count = eLuCOKFwIyZPIDuvvMeMWLHVgweEb._count;
				Entry[] entries = eLuCOKFwIyZPIDuvvMeMWLHVgweEb._entries;
				for (int i = 0; i < count; i++)
				{
					if (entries[i].hashCode >= 0)
					{
						array[index++] = entries[i].key;
					}
				}
			}

			void ICollection<TKey>.CopyTo(TKey[] array, int index)
			{
				//ILSpy generated this explicit interface implementation from .override directive in CopyTo
				this.CopyTo(array, index);
			}

			private void wzbvkwvutfWkgIVawTINluxLuMoC(TKey P_0)
			{
				throw new Exception();
			}

			void ICollection<TKey>.Add(TKey P_0)
			{
				//ILSpy generated this explicit interface implementation from .override directive in wzbvkwvutfWkgIVawTINluxLuMoC
				this.wzbvkwvutfWkgIVawTINluxLuMoC(P_0);
			}

			private void vayqZJzdlLWbRjJPCvfrZdEJxGOR()
			{
				throw new Exception();
			}

			void ICollection<TKey>.Clear()
			{
				//ILSpy generated this explicit interface implementation from .override directive in vayqZJzdlLWbRjJPCvfrZdEJxGOR
				this.vayqZJzdlLWbRjJPCvfrZdEJxGOR();
			}

			private bool xsWqWhPYXtrxoqYqfKiYcTjpIQiE(TKey P_0)
			{
				return eLuCOKFwIyZPIDuvvMeMWLHVgweEb.ContainsKey(P_0);
			}

			bool ICollection<TKey>.Contains(TKey P_0)
			{
				//ILSpy generated this explicit interface implementation from .override directive in xsWqWhPYXtrxoqYqfKiYcTjpIQiE
				return this.xsWqWhPYXtrxoqYqfKiYcTjpIQiE(P_0);
			}

			private bool kwHOFpYZjHLziaruqhEGmBObdyeI(TKey P_0)
			{
				throw new Exception();
			}

			bool ICollection<TKey>.Remove(TKey P_0)
			{
				//ILSpy generated this explicit interface implementation from .override directive in kwHOFpYZjHLziaruqhEGmBObdyeI
				return this.kwHOFpYZjHLziaruqhEGmBObdyeI(P_0);
			}

			private IEnumerator<TKey> NvyJLgISbwoZRFUWpeoaTiFfjQjW()
			{
				return new Enumerator(eLuCOKFwIyZPIDuvvMeMWLHVgweEb);
			}

			IEnumerator<TKey> IEnumerable<TKey>.GetEnumerator()
			{
				//ILSpy generated this explicit interface implementation from .override directive in NvyJLgISbwoZRFUWpeoaTiFfjQjW
				return this.NvyJLgISbwoZRFUWpeoaTiFfjQjW();
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return new Enumerator(eLuCOKFwIyZPIDuvvMeMWLHVgweEb);
			}

			void ICollection.CopyTo(Array array, int index)
			{
				if (array == null)
				{
					throw new ArgumentNullException("array");
				}
				if (array.Rank != 1)
				{
					throw new Exception();
				}
				if (array.GetLowerBound(0) != 0)
				{
					throw new Exception();
				}
				if (index < 0 || index > array.Length)
				{
					throw new Exception();
				}
				if (array.Length - index < eLuCOKFwIyZPIDuvvMeMWLHVgweEb.Count)
				{
					throw new Exception();
				}
				if (array is TKey[] array2)
				{
					CopyTo(array2, index);
					return;
				}
				if (!(array is object[] array3))
				{
					throw new Exception();
				}
				int count = eLuCOKFwIyZPIDuvvMeMWLHVgweEb._count;
				Entry[] entries = eLuCOKFwIyZPIDuvvMeMWLHVgweEb._entries;
				try
				{
					for (int i = 0; i < count; i++)
					{
						if (entries[i].hashCode >= 0)
						{
							array3[index++] = entries[i].key;
						}
					}
				}
				catch (ArrayTypeMismatchException)
				{
					throw new Exception();
				}
			}
		}

		[Serializable]
		[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
		[CustomObfuscation(rename = false)]
		public sealed class ValueCollection : ICollection<TValue>, IEnumerable<TValue>, IEnumerable, ICollection
		{
			[Serializable]
			[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
			[CustomObfuscation(rename = false)]
			public struct Enumerator : IEnumerator<TValue>, IEnumerator, IDisposable
			{
				private ADictionary<TKey, TValue> GZMZAfkGpDZagyzCMQxXlfflWZKL;

				private int RnTzAdqoPrLxptWcuamYLATTtMie;

				private int amEHeFITDsCmpViiMILcssUgEmYV;

				private TValue amYYJGbHYMhzVdjuClcQypLnERSfA;

				TValue IEnumerator<TValue>.Current => amYYJGbHYMhzVdjuClcQypLnERSfA;

				object IEnumerator.Current
				{
					get
					{
						if (RnTzAdqoPrLxptWcuamYLATTtMie == 0 || RnTzAdqoPrLxptWcuamYLATTtMie == GZMZAfkGpDZagyzCMQxXlfflWZKL._count + 1)
						{
							throw new Exception();
						}
						return amYYJGbHYMhzVdjuClcQypLnERSfA;
					}
				}

				internal Enumerator(ADictionary<TKey, TValue> P_0)
				{
					GZMZAfkGpDZagyzCMQxXlfflWZKL = P_0;
					amEHeFITDsCmpViiMILcssUgEmYV = P_0.hKwuYVFlLYJjAwmAHIsXqLflDexp;
					RnTzAdqoPrLxptWcuamYLATTtMie = 0;
					amYYJGbHYMhzVdjuClcQypLnERSfA = default(TValue);
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
					if (amEHeFITDsCmpViiMILcssUgEmYV != GZMZAfkGpDZagyzCMQxXlfflWZKL.hKwuYVFlLYJjAwmAHIsXqLflDexp)
					{
						throw new Exception();
					}
					while ((uint)RnTzAdqoPrLxptWcuamYLATTtMie < (uint)GZMZAfkGpDZagyzCMQxXlfflWZKL._count)
					{
						if (GZMZAfkGpDZagyzCMQxXlfflWZKL._entries[RnTzAdqoPrLxptWcuamYLATTtMie].hashCode >= 0)
						{
							amYYJGbHYMhzVdjuClcQypLnERSfA = GZMZAfkGpDZagyzCMQxXlfflWZKL._entries[RnTzAdqoPrLxptWcuamYLATTtMie].value;
							RnTzAdqoPrLxptWcuamYLATTtMie++;
							return true;
						}
						RnTzAdqoPrLxptWcuamYLATTtMie++;
					}
					RnTzAdqoPrLxptWcuamYLATTtMie = GZMZAfkGpDZagyzCMQxXlfflWZKL._count + 1;
					amYYJGbHYMhzVdjuClcQypLnERSfA = default(TValue);
					return false;
				}

				bool IEnumerator.MoveNext()
				{
					//ILSpy generated this explicit interface implementation from .override directive in MoveNext
					return this.MoveNext();
				}

				void IEnumerator.Reset()
				{
					if (amEHeFITDsCmpViiMILcssUgEmYV != GZMZAfkGpDZagyzCMQxXlfflWZKL.hKwuYVFlLYJjAwmAHIsXqLflDexp)
					{
						throw new Exception();
					}
					RnTzAdqoPrLxptWcuamYLATTtMie = 0;
					amYYJGbHYMhzVdjuClcQypLnERSfA = default(TValue);
				}
			}

			private ADictionary<TKey, TValue> pFMAEUjcpYekdZcmTFPyHTOatRkt;

			int ICollection<TValue>.Count => pFMAEUjcpYekdZcmTFPyHTOatRkt.Count;

			bool ICollection<TValue>.IsReadOnly => true;

			bool ICollection.IsSynchronized => false;

			object ICollection.SyncRoot => ((ICollection)pFMAEUjcpYekdZcmTFPyHTOatRkt).SyncRoot;

			public ValueCollection(ADictionary<TKey, TValue> P_0)
			{
				if (P_0 == null)
				{
					throw new ArgumentNullException("dictionary");
				}
				pFMAEUjcpYekdZcmTFPyHTOatRkt = P_0;
			}

			public Enumerator GetEnumerator()
			{
				return new Enumerator(pFMAEUjcpYekdZcmTFPyHTOatRkt);
			}

			public void CopyTo(TValue[] array, int index)
			{
				if (array == null)
				{
					throw new ArgumentNullException("array");
				}
				if (index < 0 || index > array.Length)
				{
					throw new Exception();
				}
				if (array.Length - index < pFMAEUjcpYekdZcmTFPyHTOatRkt.Count)
				{
					throw new Exception();
				}
				int count = pFMAEUjcpYekdZcmTFPyHTOatRkt._count;
				Entry[] entries = pFMAEUjcpYekdZcmTFPyHTOatRkt._entries;
				for (int i = 0; i < count; i++)
				{
					if (entries[i].hashCode >= 0)
					{
						array[index++] = entries[i].value;
					}
				}
			}

			void ICollection<TValue>.CopyTo(TValue[] array, int index)
			{
				//ILSpy generated this explicit interface implementation from .override directive in CopyTo
				this.CopyTo(array, index);
			}

			private void IPrVRAUdgTdDlAEihGWNvfhSjCdkA(TValue P_0)
			{
				throw new Exception();
			}

			void ICollection<TValue>.Add(TValue P_0)
			{
				//ILSpy generated this explicit interface implementation from .override directive in IPrVRAUdgTdDlAEihGWNvfhSjCdkA
				this.IPrVRAUdgTdDlAEihGWNvfhSjCdkA(P_0);
			}

			private bool FKBivuLcycMJCsrJKcxPlqUTCfom(TValue P_0)
			{
				throw new Exception();
			}

			bool ICollection<TValue>.Remove(TValue P_0)
			{
				//ILSpy generated this explicit interface implementation from .override directive in FKBivuLcycMJCsrJKcxPlqUTCfom
				return this.FKBivuLcycMJCsrJKcxPlqUTCfom(P_0);
			}

			private void nxtPWcCOPvLXyBHQqMsCRWKdllTi()
			{
				throw new Exception();
			}

			void ICollection<TValue>.Clear()
			{
				//ILSpy generated this explicit interface implementation from .override directive in nxtPWcCOPvLXyBHQqMsCRWKdllTi
				this.nxtPWcCOPvLXyBHQqMsCRWKdllTi();
			}

			private bool PbLhrRakDIFcUdBtiiTCzFGIxRIW(TValue P_0)
			{
				return pFMAEUjcpYekdZcmTFPyHTOatRkt.ContainsValue(P_0);
			}

			bool ICollection<TValue>.Contains(TValue P_0)
			{
				//ILSpy generated this explicit interface implementation from .override directive in PbLhrRakDIFcUdBtiiTCzFGIxRIW
				return this.PbLhrRakDIFcUdBtiiTCzFGIxRIW(P_0);
			}

			private IEnumerator<TValue> VipCxmRosgBtgYyXBMNgCIToBGxl()
			{
				return new Enumerator(pFMAEUjcpYekdZcmTFPyHTOatRkt);
			}

			IEnumerator<TValue> IEnumerable<TValue>.GetEnumerator()
			{
				//ILSpy generated this explicit interface implementation from .override directive in VipCxmRosgBtgYyXBMNgCIToBGxl
				return this.VipCxmRosgBtgYyXBMNgCIToBGxl();
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return new Enumerator(pFMAEUjcpYekdZcmTFPyHTOatRkt);
			}

			void ICollection.CopyTo(Array array, int index)
			{
				if (array == null)
				{
					throw new ArgumentNullException("array");
				}
				if (array.Rank != 1)
				{
					throw new Exception();
				}
				if (array.GetLowerBound(0) != 0)
				{
					throw new Exception();
				}
				if (index < 0 || index > array.Length)
				{
					throw new Exception();
				}
				if (array.Length - index < pFMAEUjcpYekdZcmTFPyHTOatRkt.Count)
				{
					throw new Exception();
				}
				if (array is TValue[] array2)
				{
					CopyTo(array2, index);
					return;
				}
				if (!(array is object[] array3))
				{
					throw new Exception();
				}
				int count = pFMAEUjcpYekdZcmTFPyHTOatRkt._count;
				Entry[] entries = pFMAEUjcpYekdZcmTFPyHTOatRkt._entries;
				try
				{
					for (int i = 0; i < count; i++)
					{
						if (entries[i].hashCode >= 0)
						{
							array3[index++] = entries[i].value;
						}
					}
				}
				catch (ArrayTypeMismatchException)
				{
					throw new Exception();
				}
			}
		}

		private int[] xRkhrglEXuAgFgSoGwTyTFEPvyIp;

		internal Entry[] _entries;

		internal int _count;

		private int hKwuYVFlLYJjAwmAHIsXqLflDexp;

		private int qHpGeTdrYGFCSRZwGQCAjcEWXPSj;

		private int yrSnzAGBfUjDDAkMKWBhTeGSIsyFb;

		private int qITeBvbhRnYoBmesYjlswodqzcBB;

		private IEqualityComparer<TKey> hBvPRJRSALacoJtFKbmSXebOJLON;

		private IEqualityComparer<TValue> FzeVYpsDiFYwVBddujvNDbeItGxm;

		private KeyCollection LpXxuMwiUiuECQxUZLmYoSsXPBxA;

		private ValueCollection UWBWnVcmZlrKvJlhApqNUkJepJGA;

		private readonly object VKemjrGvsGwIRHcfCnIzagxaEhlu = new object();

		private static readonly bool LneAGfiktqzEMAMqmEmTRxFzgvVZ = ReflectionTools.IsValueType(typeof(TKey));

		private static readonly bool hrhBNLTLaaFUxTcFpCNLRRBXoDEn = ReflectionTools.IsValueType(typeof(TValue));

		private const string GwEdSCghymVOnUjoNmgugedTFFqt = "Version";

		private const string EFlEHuHNhebdmeulYkyMrmvOqVRJA = "HashSize";

		private const string ZYsAYuVEIBVkTcuUiORVVODNURAb = "KeyValuePairs";

		private const string dZPMhOqtKksfRLVZuFLAeNWIEmqkA = "Comparer";

		int ICollection<KeyValuePair<TKey, TValue>>.Count => _count - qITeBvbhRnYoBmesYjlswodqzcBB;

		public int TotalCount => _count;

		public KeyCollection Keys
		{
			get
			{
				if (LpXxuMwiUiuECQxUZLmYoSsXPBxA == null)
				{
					LpXxuMwiUiuECQxUZLmYoSsXPBxA = new KeyCollection(this);
				}
				return LpXxuMwiUiuECQxUZLmYoSsXPBxA;
			}
		}

		public ValueCollection Values
		{
			get
			{
				if (UWBWnVcmZlrKvJlhApqNUkJepJGA == null)
				{
					UWBWnVcmZlrKvJlhApqNUkJepJGA = new ValueCollection(this);
				}
				return UWBWnVcmZlrKvJlhApqNUkJepJGA;
			}
		}

		public IEqualityComparer<TKey> KeyComparer
		{
			get
			{
				return hBvPRJRSALacoJtFKbmSXebOJLON;
			}
			set
			{
				if (value == null)
				{
					value = EqualityComparerNoAlloc<TKey>.Default;
				}
				hBvPRJRSALacoJtFKbmSXebOJLON = value;
			}
		}

		public IEqualityComparer<TValue> ValueComparer
		{
			get
			{
				return FzeVYpsDiFYwVBddujvNDbeItGxm;
			}
			set
			{
				if (value == null)
				{
					value = EqualityComparerNoAlloc<TValue>.Default;
				}
				FzeVYpsDiFYwVBddujvNDbeItGxm = value;
			}
		}

		TValue Rewired.Utils.Interfaces.IReadOnlyDictionary<TKey, TValue>.this[TKey key]
		{
			get
			{
				int num = IndexOfKey(key);
				if (num < 0)
				{
					TKey val = key;
					throw new KeyNotFoundException("Key \"" + val?.ToString() + " does not exist.");
				}
				return _entries[num].value;
			}
			set
			{
				FXISjjmVzEUSaMFNKIJcEulIbsobA(key, value, false);
			}
		}

		public int IndexOfFirst
		{
			get
			{
				for (int i = 0; i < _count; i++)
				{
					if (_entries[i].hashCode >= 0)
					{
						return i;
					}
				}
				return -1;
			}
		}

		public int IndexOfLast
		{
			get
			{
				for (int num = _count - 1; num >= 0; num--)
				{
					if (_entries[num].hashCode >= 0)
					{
						return num;
					}
				}
				return -1;
			}
		}

		ICollection<TKey> IDictionary<TKey, TValue>.Keys
		{
			get
			{
				if (LpXxuMwiUiuECQxUZLmYoSsXPBxA == null)
				{
					LpXxuMwiUiuECQxUZLmYoSsXPBxA = new KeyCollection(this);
				}
				return LpXxuMwiUiuECQxUZLmYoSsXPBxA;
			}
		}

		ICollection<TValue> IDictionary<TKey, TValue>.Values
		{
			get
			{
				if (UWBWnVcmZlrKvJlhApqNUkJepJGA == null)
				{
					UWBWnVcmZlrKvJlhApqNUkJepJGA = new ValueCollection(this);
				}
				return UWBWnVcmZlrKvJlhApqNUkJepJGA;
			}
		}

		bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly => false;

		bool ICollection.IsSynchronized => false;

		object ICollection.SyncRoot => VKemjrGvsGwIRHcfCnIzagxaEhlu;

		bool IDictionary.IsFixedSize => false;

		bool IDictionary.IsReadOnly => false;

		ICollection IDictionary.Keys => Keys;

		ICollection IDictionary.Values => Values;

		object IDictionary.this[object key]
		{
			get
			{
				if (bTsQArYgKKUsvJSYJjKLCxpjQJZH(key))
				{
					int num = IndexOfKey((TKey)key);
					if (num >= 0)
					{
						return _entries[num].value;
					}
				}
				return null;
			}
			set
			{
				if (key == null)
				{
					throw new ArgumentNullException("key");
				}
				nlViugDJMjCTEBlnDWrytTOyuXjJA<TValue>(value, "value");
				try
				{
					TKey val = (TKey)key;
					try
					{
						this[val] = (TValue)value;
					}
					catch (InvalidCastException)
					{
						throw new Exception();
					}
				}
				catch (InvalidCastException)
				{
					throw new Exception();
				}
			}
		}

		ICollection<TKey> Rewired.Utils.Interfaces.IReadOnlyDictionary<TKey, TValue>.Keys => Keys;

		ICollection<TValue> Rewired.Utils.Interfaces.IReadOnlyDictionary<TKey, TValue>.Values => Values;

		public ADictionary()
			: this(0, (IEqualityComparer<TKey>)null, (IEqualityComparer<TValue>)null)
		{
		}

		public ADictionary(IEqualityComparer<TKey> P_0)
			: this(0, P_0, (IEqualityComparer<TValue>)null)
		{
		}

		public ADictionary(IEqualityComparer<TKey> P_0, IEqualityComparer<TValue> P_1)
			: this(0, P_0, P_1)
		{
		}

		public ADictionary(int P_0)
			: this(P_0, (IEqualityComparer<TKey>)null, (IEqualityComparer<TValue>)null)
		{
		}

		public ADictionary(int P_0, IEqualityComparer<TKey> P_1)
			: this(P_0, P_1, (IEqualityComparer<TValue>)null)
		{
		}

		public ADictionary(int P_0, IEqualityComparer<TKey> P_1, IEqualityComparer<TValue> P_2)
		{
			if (P_0 < 0)
			{
				throw new ArgumentOutOfRangeException("capacity");
			}
			if (P_0 > 0)
			{
				YXYbjHwUcPfrnJIRxHkkVpSIIfkl(P_0);
			}
			hBvPRJRSALacoJtFKbmSXebOJLON = P_1 ?? EqualityComparerNoAlloc<TKey>.Default;
			FzeVYpsDiFYwVBddujvNDbeItGxm = P_2 ?? EqualityComparerNoAlloc<TValue>.Default;
		}

		public ADictionary(IDictionary<TKey, TValue> P_0)
			: this(P_0, (IEqualityComparer<TKey>)null, (IEqualityComparer<TValue>)null)
		{
		}

		public ADictionary(IDictionary<TKey, TValue> P_0, IEqualityComparer<TKey> P_1)
			: this(P_0, P_1, (IEqualityComparer<TValue>)null)
		{
		}

		public ADictionary(IDictionary<TKey, TValue> P_0, IEqualityComparer<TKey> P_1, IEqualityComparer<TValue> P_2)
			: this(P_0?.Count ?? 0, P_1)
		{
			if (P_0 == null)
			{
				throw new ArgumentNullException("dictionary");
			}
			foreach (KeyValuePair<TKey, TValue> item in P_0)
			{
				Add(item.Key, item.Value);
			}
		}

		public void Add(TKey key, TValue value)
		{
			FXISjjmVzEUSaMFNKIJcEulIbsobA(key, value, true);
		}

		void IDictionary<TKey, TValue>.Add(TKey key, TValue value)
		{
			//ILSpy generated this explicit interface implementation from .override directive in Add
			this.Add(key, value);
		}

		public void Clear()
		{
			if (_count > 0)
			{
				for (int i = 0; i < xRkhrglEXuAgFgSoGwTyTFEPvyIp.Length; i++)
				{
					xRkhrglEXuAgFgSoGwTyTFEPvyIp[i] = -1;
				}
				Array.Clear(_entries, 0, _count);
				yrSnzAGBfUjDDAkMKWBhTeGSIsyFb = -1;
				_count = 0;
				qITeBvbhRnYoBmesYjlswodqzcBB = 0;
				hKwuYVFlLYJjAwmAHIsXqLflDexp++;
				qHpGeTdrYGFCSRZwGQCAjcEWXPSj++;
			}
		}

		void ICollection<KeyValuePair<TKey, TValue>>.Clear()
		{
			//ILSpy generated this explicit interface implementation from .override directive in Clear
			this.Clear();
		}

		void IDictionary.Clear()
		{
			//ILSpy generated this explicit interface implementation from .override directive in Clear
			this.Clear();
		}

		public bool ContainsKey(TKey key)
		{
			return IndexOfKey(key) >= 0;
		}

		bool IDictionary<TKey, TValue>.ContainsKey(TKey key)
		{
			//ILSpy generated this explicit interface implementation from .override directive in ContainsKey
			return this.ContainsKey(key);
		}

		bool Rewired.Utils.Interfaces.IReadOnlyDictionary<TKey, TValue>.ContainsKey(TKey key)
		{
			//ILSpy generated this explicit interface implementation from .override directive in ContainsKey
			return this.ContainsKey(key);
		}

		public bool ContainsValue(TValue value)
		{
			return IndexOfValue(value) >= 0;
		}

		public Enumerator GetEnumerator()
		{
			return new Enumerator(this, 2);
		}

		public bool Remove(TKey key)
		{
			if (!LneAGfiktqzEMAMqmEmTRxFzgvVZ && key == null)
			{
				throw new ArgumentNullException("key");
			}
			if (xRkhrglEXuAgFgSoGwTyTFEPvyIp != null)
			{
				int num = hBvPRJRSALacoJtFKbmSXebOJLON.GetHashCode(key) & 0x7FFFFFFF;
				int num2 = num % xRkhrglEXuAgFgSoGwTyTFEPvyIp.Length;
				int num3 = -1;
				for (int num4 = xRkhrglEXuAgFgSoGwTyTFEPvyIp[num2]; num4 >= 0; num4 = _entries[num4].next)
				{
					if (_entries[num4].hashCode == num && hBvPRJRSALacoJtFKbmSXebOJLON.Equals(_entries[num4].key, key))
					{
						if (num3 < 0)
						{
							xRkhrglEXuAgFgSoGwTyTFEPvyIp[num2] = _entries[num4].next;
						}
						else
						{
							_entries[num3].next = _entries[num4].next;
						}
						_entries[num4].hashCode = -1;
						_entries[num4].next = yrSnzAGBfUjDDAkMKWBhTeGSIsyFb;
						_entries[num4].key = default(TKey);
						_entries[num4].value = default(TValue);
						yrSnzAGBfUjDDAkMKWBhTeGSIsyFb = num4;
						qITeBvbhRnYoBmesYjlswodqzcBB++;
						hKwuYVFlLYJjAwmAHIsXqLflDexp++;
						return true;
					}
					num3 = num4;
				}
			}
			return false;
		}

		bool IDictionary<TKey, TValue>.Remove(TKey key)
		{
			//ILSpy generated this explicit interface implementation from .override directive in Remove
			return this.Remove(key);
		}

		public bool TryGetValue(TKey key, out TValue value)
		{
			int num = IndexOfKey(key);
			if (num >= 0)
			{
				value = _entries[num].value;
				return true;
			}
			value = default(TValue);
			return false;
		}

		bool IDictionary<TKey, TValue>.TryGetValue(TKey key, out TValue value)
		{
			//ILSpy generated this explicit interface implementation from .override directive in TryGetValue
			return this.TryGetValue(key, out value);
		}

		bool Rewired.Utils.Interfaces.IReadOnlyDictionary<TKey, TValue>.TryGetValue(TKey key, out TValue value)
		{
			//ILSpy generated this explicit interface implementation from .override directive in TryGetValue
			return this.TryGetValue(key, out value);
		}

		public TValue GetValueSafe(TKey key)
		{
			int num = IndexOfKey(key);
			if (num >= 0)
			{
				return _entries[num].value;
			}
			return default(TValue);
		}

		public int IndexOfKey(TKey key)
		{
			if (!LneAGfiktqzEMAMqmEmTRxFzgvVZ && key == null)
			{
				throw new ArgumentNullException("key");
			}
			if (xRkhrglEXuAgFgSoGwTyTFEPvyIp != null)
			{
				int num = hBvPRJRSALacoJtFKbmSXebOJLON.GetHashCode(key) & 0x7FFFFFFF;
				for (int num2 = xRkhrglEXuAgFgSoGwTyTFEPvyIp[num % xRkhrglEXuAgFgSoGwTyTFEPvyIp.Length]; num2 >= 0; num2 = _entries[num2].next)
				{
					if (_entries[num2].hashCode == num && hBvPRJRSALacoJtFKbmSXebOJLON.Equals(_entries[num2].key, key))
					{
						return num2;
					}
				}
			}
			return -1;
		}

		public int IndexOfValue(TValue value)
		{
			Entry[] entries = _entries;
			if (!hrhBNLTLaaFUxTcFpCNLRRBXoDEn && value == null)
			{
				for (int i = 0; i < _count; i++)
				{
					if (entries[i].hashCode >= 0 && entries[i].value == null)
					{
						return i;
					}
				}
			}
			else
			{
				IEqualityComparer<TValue> fzeVYpsDiFYwVBddujvNDbeItGxm = FzeVYpsDiFYwVBddujvNDbeItGxm;
				for (int j = 0; j < _count; j++)
				{
					if (entries[j].hashCode >= 0 && fzeVYpsDiFYwVBddujvNDbeItGxm.Equals(entries[j].value, value))
					{
						return j;
					}
				}
			}
			return -1;
		}

		public bool IsValidAt(int index)
		{
			if ((uint)index >= (uint)_count)
			{
				return false;
			}
			return _entries[index].hashCode >= 0;
		}

		public TKey GetKeyAt(int index)
		{
			if ((uint)index >= (uint)_count)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			if (_entries[index].hashCode < 0)
			{
				throw new ArgumentException("index points to an invalid entry.");
			}
			return _entries[index].key;
		}

		public TValue GetValueAt(int index)
		{
			if ((uint)index >= (uint)_count)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			if (_entries[index].hashCode < 0)
			{
				throw new ArgumentException("index points to an invalid entry.");
			}
			return _entries[index].value;
		}

		public KeyValuePair<TKey, TValue> GetEntryAt(int index)
		{
			if ((uint)index >= (uint)_count)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			if (_entries[index].hashCode < 0)
			{
				throw new ArgumentException("index points to an invalid entry.");
			}
			return new KeyValuePair<TKey, TValue>(_entries[index].key, _entries[index].value);
		}

		public bool TryGetKeyAt(int index, out TKey key)
		{
			if ((uint)index >= (uint)_count || _entries[index].hashCode < 0)
			{
				key = default(TKey);
				return false;
			}
			key = _entries[index].key;
			return true;
		}

		public bool TryGetValueAt(int index, out TValue value)
		{
			if ((uint)index >= (uint)_count || _entries[index].hashCode < 0)
			{
				value = default(TValue);
				return false;
			}
			value = _entries[index].value;
			return true;
		}

		public bool TryGetEntryAt(int index, out KeyValuePair<TKey, TValue> entry)
		{
			if ((uint)index >= (uint)_count || _entries[index].hashCode < 0)
			{
				entry = default(KeyValuePair<TKey, TValue>);
				return false;
			}
			entry = new KeyValuePair<TKey, TValue>(_entries[index].key, _entries[index].value);
			return true;
		}

		public bool GetNextIndex(ref int index)
		{
			index++;
			if ((uint)index >= (uint)_count)
			{
				return false;
			}
			while (index < _count)
			{
				if (_entries[index].hashCode >= 0)
				{
					return true;
				}
				index++;
			}
			return false;
		}

		public int GetNextIndex(int index)
		{
			index++;
			if ((uint)index >= (uint)_count)
			{
				return -1;
			}
			while (index < _count)
			{
				if (_entries[index].hashCode >= 0)
				{
					return index;
				}
				index++;
			}
			return -1;
		}

		public bool GetNextKey(ref int index, out TKey key)
		{
			index++;
			if ((uint)index >= (uint)_count)
			{
				key = default(TKey);
				return false;
			}
			while (index < _count)
			{
				if (_entries[index].hashCode >= 0)
				{
					key = _entries[index].key;
					return true;
				}
				index++;
			}
			key = default(TKey);
			return false;
		}

		public bool GetNextValue(ref int index, out TValue value)
		{
			index++;
			if ((uint)index >= (uint)_count)
			{
				value = default(TValue);
				return false;
			}
			while (index < _count)
			{
				if (_entries[index].hashCode >= 0)
				{
					value = _entries[index].value;
					return true;
				}
				index++;
			}
			value = default(TValue);
			return false;
		}

		public bool GetNextEntry(ref int index, out KeyValuePair<TKey, TValue> entry)
		{
			index++;
			if ((uint)index >= (uint)_count)
			{
				entry = default(KeyValuePair<TKey, TValue>);
				return false;
			}
			while (index < _count)
			{
				if (_entries[index].hashCode >= 0)
				{
					entry = new KeyValuePair<TKey, TValue>(_entries[index].key, _entries[index].value);
					return true;
				}
				index++;
			}
			entry = default(KeyValuePair<TKey, TValue>);
			return false;
		}

		public bool GetPreviousIndex(ref int index)
		{
			index--;
			if ((uint)index >= (uint)_count)
			{
				return false;
			}
			while (index >= 0)
			{
				if (_entries[index].hashCode >= 0)
				{
					return true;
				}
				index--;
			}
			return false;
		}

		public int GetPreviousIndex(int index)
		{
			index--;
			if ((uint)index >= (uint)_count)
			{
				return -1;
			}
			while (index >= 0)
			{
				if (_entries[index].hashCode >= 0)
				{
					return index;
				}
				index--;
			}
			return -1;
		}

		public bool GetPreviousKey(ref int index, out TKey key)
		{
			index--;
			if ((uint)index >= (uint)_count)
			{
				key = default(TKey);
				return false;
			}
			while (index >= 0)
			{
				if (_entries[index].hashCode >= 0)
				{
					key = _entries[index].key;
					return true;
				}
				index--;
			}
			key = default(TKey);
			return false;
		}

		public bool GetPreviousValue(ref int index, out TValue value)
		{
			index--;
			if ((uint)index >= (uint)_count)
			{
				value = default(TValue);
				return false;
			}
			while (index >= 0)
			{
				if (_entries[index].hashCode >= 0)
				{
					value = _entries[index].value;
					return true;
				}
				index--;
			}
			value = default(TValue);
			return false;
		}

		public bool GetPreviousEntry(ref int index, out KeyValuePair<TKey, TValue> entry)
		{
			index--;
			if ((uint)index >= (uint)_count)
			{
				entry = default(KeyValuePair<TKey, TValue>);
				return false;
			}
			while (index >= 0)
			{
				if (_entries[index].hashCode >= 0)
				{
					entry = new KeyValuePair<TKey, TValue>(_entries[index].key, _entries[index].value);
					return true;
				}
				index--;
			}
			entry = default(KeyValuePair<TKey, TValue>);
			return false;
		}

		public bool RemoveAt(int index)
		{
			if ((uint)index >= (uint)_count)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			if (_entries[index].hashCode < 0)
			{
				return false;
			}
			Remove(_entries[index].key);
			return true;
		}

		private void CopyTo(KeyValuePair<TKey, TValue>[] array, int index)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (index < 0 || index > array.Length)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			if (array.Length - index < this.Count)
			{
				throw new Exception();
			}
			int count = _count;
			Entry[] entries = _entries;
			for (int i = 0; i < count; i++)
			{
				if (entries[i].hashCode >= 0)
				{
					array[index++] = new KeyValuePair<TKey, TValue>(entries[i].key, entries[i].value);
				}
			}
		}

		private void YXYbjHwUcPfrnJIRxHkkVpSIIfkl(int P_0)
		{
			int num = FwDgckZMFTcSVcrWDjGBiayeyZyTA.YysFyNpCxhfwuaLNxoQFhwWzMyGt(P_0);
			xRkhrglEXuAgFgSoGwTyTFEPvyIp = new int[num];
			for (int i = 0; i < xRkhrglEXuAgFgSoGwTyTFEPvyIp.Length; i++)
			{
				xRkhrglEXuAgFgSoGwTyTFEPvyIp[i] = -1;
			}
			_entries = new Entry[num];
			yrSnzAGBfUjDDAkMKWBhTeGSIsyFb = -1;
		}

		private void FXISjjmVzEUSaMFNKIJcEulIbsobA(TKey P_0, TValue P_1, bool P_2)
		{
			if (!LneAGfiktqzEMAMqmEmTRxFzgvVZ && P_0 == null)
			{
				throw new ArgumentNullException("key");
			}
			if (xRkhrglEXuAgFgSoGwTyTFEPvyIp == null)
			{
				YXYbjHwUcPfrnJIRxHkkVpSIIfkl(0);
			}
			int num = hBvPRJRSALacoJtFKbmSXebOJLON.GetHashCode(P_0) & 0x7FFFFFFF;
			int num2 = num % xRkhrglEXuAgFgSoGwTyTFEPvyIp.Length;
			for (int num3 = xRkhrglEXuAgFgSoGwTyTFEPvyIp[num2]; num3 >= 0; num3 = _entries[num3].next)
			{
				if (_entries[num3].hashCode == num && hBvPRJRSALacoJtFKbmSXebOJLON.Equals(_entries[num3].key, P_0))
				{
					if (P_2)
					{
						throw new ArgumentException("An element with the same key already exists in the dictionary.");
					}
					_entries[num3].value = P_1;
					hKwuYVFlLYJjAwmAHIsXqLflDexp++;
					return;
				}
			}
			int count;
			if (qITeBvbhRnYoBmesYjlswodqzcBB > 0)
			{
				count = yrSnzAGBfUjDDAkMKWBhTeGSIsyFb;
				yrSnzAGBfUjDDAkMKWBhTeGSIsyFb = _entries[count].next;
				qITeBvbhRnYoBmesYjlswodqzcBB--;
			}
			else
			{
				if (_count == _entries.Length)
				{
					TSydjdkkbOLbCFymNzSLspYkmpY();
					num2 = num % xRkhrglEXuAgFgSoGwTyTFEPvyIp.Length;
				}
				count = _count;
				_count++;
			}
			_entries[count].hashCode = num;
			_entries[count].next = xRkhrglEXuAgFgSoGwTyTFEPvyIp[num2];
			_entries[count].key = P_0;
			_entries[count].value = P_1;
			xRkhrglEXuAgFgSoGwTyTFEPvyIp[num2] = count;
			hKwuYVFlLYJjAwmAHIsXqLflDexp++;
			qHpGeTdrYGFCSRZwGQCAjcEWXPSj++;
		}

		private void TSydjdkkbOLbCFymNzSLspYkmpY()
		{
			FcfAxXbARextucCiDpMxEngGubUSc(FwDgckZMFTcSVcrWDjGBiayeyZyTA.iBDFGvcHTeHUtZgDEIwsPMRLLNShb(_count), false);
		}

		private void FcfAxXbARextucCiDpMxEngGubUSc(int P_0, bool P_1)
		{
			int[] array = new int[P_0];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = -1;
			}
			Entry[] array2 = new Entry[P_0];
			Array.Copy(_entries, 0, array2, 0, _count);
			if (P_1)
			{
				for (int j = 0; j < _count; j++)
				{
					if (array2[j].hashCode != -1)
					{
						array2[j].hashCode = hBvPRJRSALacoJtFKbmSXebOJLON.GetHashCode(array2[j].key) & 0x7FFFFFFF;
					}
				}
			}
			for (int k = 0; k < _count; k++)
			{
				if (array2[k].hashCode >= 0)
				{
					int num = array2[k].hashCode % P_0;
					array2[k].next = array[num];
					array[num] = k;
				}
			}
			xRkhrglEXuAgFgSoGwTyTFEPvyIp = array;
			_entries = array2;
		}

		private IEnumerator<KeyValuePair<TKey, TValue>> HcjNhrOTVpoIKGqBZdvOzXeDBlzS()
		{
			return new Enumerator(this, 2);
		}

		IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
		{
			//ILSpy generated this explicit interface implementation from .override directive in HcjNhrOTVpoIKGqBZdvOzXeDBlzS
			return this.HcjNhrOTVpoIKGqBZdvOzXeDBlzS();
		}

		private void LxTAtPChiBAqTmncOeyZFwKfWUvib(KeyValuePair<TKey, TValue> P_0)
		{
			Add(P_0.Key, P_0.Value);
		}

		void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> P_0)
		{
			//ILSpy generated this explicit interface implementation from .override directive in LxTAtPChiBAqTmncOeyZFwKfWUvib
			this.LxTAtPChiBAqTmncOeyZFwKfWUvib(P_0);
		}

		private bool TXeeXywTNPLTrCoTubimKaYqmzVqA(KeyValuePair<TKey, TValue> P_0)
		{
			int num = IndexOfKey(P_0.Key);
			if (num >= 0 && FzeVYpsDiFYwVBddujvNDbeItGxm.Equals(_entries[num].value, P_0.Value))
			{
				return true;
			}
			return false;
		}

		bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> P_0)
		{
			//ILSpy generated this explicit interface implementation from .override directive in TXeeXywTNPLTrCoTubimKaYqmzVqA
			return this.TXeeXywTNPLTrCoTubimKaYqmzVqA(P_0);
		}

		private bool HTxDEbZjPmFfiipodnlpnabJbkvFb(KeyValuePair<TKey, TValue> P_0)
		{
			int num = IndexOfKey(P_0.Key);
			if (num >= 0 && FzeVYpsDiFYwVBddujvNDbeItGxm.Equals(_entries[num].value, P_0.Value))
			{
				Remove(P_0.Key);
				return true;
			}
			return false;
		}

		bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> P_0)
		{
			//ILSpy generated this explicit interface implementation from .override directive in HTxDEbZjPmFfiipodnlpnabJbkvFb
			return this.HTxDEbZjPmFfiipodnlpnabJbkvFb(P_0);
		}

		private void wLVADLftvUOlExolpSSnvUhbsQne(KeyValuePair<TKey, TValue>[] P_0, int P_1)
		{
			CopyTo(P_0, P_1);
		}

		void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] P_0, int P_1)
		{
			//ILSpy generated this explicit interface implementation from .override directive in wLVADLftvUOlExolpSSnvUhbsQne
			this.wLVADLftvUOlExolpSSnvUhbsQne(P_0, P_1);
		}

		void ICollection.CopyTo(Array array, int index)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (array.Rank != 1)
			{
				throw new Exception();
			}
			if (array.GetLowerBound(0) != 0)
			{
				throw new Exception();
			}
			if (index < 0 || index > array.Length)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			if (array.Length - index < this.Count)
			{
				throw new Exception();
			}
			if (array is KeyValuePair<TKey, TValue>[] array2)
			{
				CopyTo(array2, index);
				return;
			}
			if (array is DictionaryEntry[])
			{
				DictionaryEntry[] array3 = array as DictionaryEntry[];
				Entry[] entries = _entries;
				for (int i = 0; i < _count; i++)
				{
					if (entries[i].hashCode >= 0)
					{
						array3[index++] = new DictionaryEntry(entries[i].key, entries[i].value);
					}
				}
				return;
			}
			if (!(array is object[] array4))
			{
				throw new Exception();
			}
			try
			{
				int count = _count;
				Entry[] entries2 = _entries;
				for (int j = 0; j < count; j++)
				{
					if (entries2[j].hashCode >= 0)
					{
						array4[index++] = new KeyValuePair<TKey, TValue>(entries2[j].key, entries2[j].value);
					}
				}
			}
			catch (ArrayTypeMismatchException)
			{
				throw new Exception();
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return new Enumerator(this, 2);
		}

		void IDictionary.Add(object key, object value)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			nlViugDJMjCTEBlnDWrytTOyuXjJA<TValue>(value, "value");
			try
			{
				TKey key2 = (TKey)key;
				try
				{
					Add(key2, (TValue)value);
				}
				catch (InvalidCastException)
				{
					throw new Exception();
				}
			}
			catch (InvalidCastException)
			{
				throw new Exception();
			}
		}

		bool IDictionary.Contains(object key)
		{
			if (bTsQArYgKKUsvJSYJjKLCxpjQJZH(key))
			{
				return ContainsKey((TKey)key);
			}
			return false;
		}

		IDictionaryEnumerator IDictionary.GetEnumerator()
		{
			return new Enumerator(this, 1);
		}

		void IDictionary.Remove(object key)
		{
			if (bTsQArYgKKUsvJSYJjKLCxpjQJZH(key))
			{
				Remove((TKey)key);
			}
		}

		private static bool bTsQArYgKKUsvJSYJjKLCxpjQJZH(object P_0)
		{
			if (P_0 == null)
			{
				throw new ArgumentNullException("key");
			}
			return P_0 is TKey;
		}

		private static void nlViugDJMjCTEBlnDWrytTOyuXjJA<_0001>(object P_0, string P_1)
		{
			if (P_0 == null && default(_0001) != null)
			{
				throw new ArgumentNullException(P_1);
			}
		}
	}
}
