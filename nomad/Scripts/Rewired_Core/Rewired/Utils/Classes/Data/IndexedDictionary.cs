using System;
using System.Collections;
using System.Collections.Generic;
using Rewired.Utils.Interfaces;

namespace Rewired.Utils.Classes.Data
{
	[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
	[CustomObfuscation(rename = false)]
	internal class IndexedDictionary<TKey, TValue> : IDictionary<TKey, TValue>, ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable, IDictionary, ICollection, Rewired.Utils.Interfaces.IReadOnlyList<TValue>, IReadOnlyList
	{
		private struct PLpnOJdgdqHFEgwDpQIfvNWlxDKBA
		{
			public TKey OrRWAMnVDTsUpKngurTdhIIAtaJX;

			public TValue EooecqekyDnZzgRwqPZWbVsdTCSJb;

			public PLpnOJdgdqHFEgwDpQIfvNWlxDKBA(TKey P_0, TValue P_1)
			{
				OrRWAMnVDTsUpKngurTdhIIAtaJX = P_0;
				EooecqekyDnZzgRwqPZWbVsdTCSJb = P_1;
			}

			public KeyValuePair<TKey, TValue> uRwaZRPOZpdFNtvnfYUQtpgnVcVU()
			{
				return new KeyValuePair<TKey, TValue>(OrRWAMnVDTsUpKngurTdhIIAtaJX, EooecqekyDnZzgRwqPZWbVsdTCSJb);
			}
		}

		[Serializable]
		[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
		[CustomObfuscation(rename = false)]
		public struct Enumerator : IEnumerator<KeyValuePair<TKey, TValue>>, IEnumerator, IDisposable, IDictionaryEnumerator
		{
			private IndexedDictionary<TKey, TValue> ijNgrdFjzcMGkfODXsixVSRUmchl;

			private int MKKFXwEoivJMwrLryJUtLvZbHeDG;

			private int UsnUqjALQLkUAMRjsAhiFDuBiyav;

			private KeyValuePair<TKey, TValue> UUBtHmKLROhzMfDsFdIGdhnbifcg;

			private int YkNBoeNTgrFqPDDnuLivMoVLfFooA;

			internal const int DictEntry = 1;

			internal const int KeyValuePair = 2;

			KeyValuePair<TKey, TValue> IEnumerator<KeyValuePair<TKey, TValue>>.Current => UUBtHmKLROhzMfDsFdIGdhnbifcg;

			object IEnumerator.Current
			{
				get
				{
					if (UsnUqjALQLkUAMRjsAhiFDuBiyav == 0 || UsnUqjALQLkUAMRjsAhiFDuBiyav == ijNgrdFjzcMGkfODXsixVSRUmchl.NWhsmTLafNBAmfcZDbqzAzkfnjUv._count + 1)
					{
						throw new Exception();
					}
					if (YkNBoeNTgrFqPDDnuLivMoVLfFooA == 1)
					{
						return new DictionaryEntry(UUBtHmKLROhzMfDsFdIGdhnbifcg.Key, UUBtHmKLROhzMfDsFdIGdhnbifcg.Value);
					}
					return new KeyValuePair<TKey, TValue>(UUBtHmKLROhzMfDsFdIGdhnbifcg.Key, UUBtHmKLROhzMfDsFdIGdhnbifcg.Value);
				}
			}

			DictionaryEntry IDictionaryEnumerator.Entry
			{
				get
				{
					if (UsnUqjALQLkUAMRjsAhiFDuBiyav == 0 || UsnUqjALQLkUAMRjsAhiFDuBiyav == ijNgrdFjzcMGkfODXsixVSRUmchl.NWhsmTLafNBAmfcZDbqzAzkfnjUv._count + 1)
					{
						throw new Exception();
					}
					return new DictionaryEntry(UUBtHmKLROhzMfDsFdIGdhnbifcg.Key, UUBtHmKLROhzMfDsFdIGdhnbifcg.Value);
				}
			}

			object IDictionaryEnumerator.Key
			{
				get
				{
					if (UsnUqjALQLkUAMRjsAhiFDuBiyav == 0 || UsnUqjALQLkUAMRjsAhiFDuBiyav == ijNgrdFjzcMGkfODXsixVSRUmchl.NWhsmTLafNBAmfcZDbqzAzkfnjUv._count + 1)
					{
						throw new Exception();
					}
					return UUBtHmKLROhzMfDsFdIGdhnbifcg.Key;
				}
			}

			object IDictionaryEnumerator.Value
			{
				get
				{
					if (UsnUqjALQLkUAMRjsAhiFDuBiyav == 0 || UsnUqjALQLkUAMRjsAhiFDuBiyav == ijNgrdFjzcMGkfODXsixVSRUmchl.NWhsmTLafNBAmfcZDbqzAzkfnjUv._count + 1)
					{
						throw new Exception();
					}
					return UUBtHmKLROhzMfDsFdIGdhnbifcg.Value;
				}
			}

			internal Enumerator(IndexedDictionary<TKey, TValue> P_0, int P_1)
			{
				ijNgrdFjzcMGkfODXsixVSRUmchl = P_0;
				MKKFXwEoivJMwrLryJUtLvZbHeDG = P_0.NWhsmTLafNBAmfcZDbqzAzkfnjUv.Version;
				UsnUqjALQLkUAMRjsAhiFDuBiyav = 0;
				YkNBoeNTgrFqPDDnuLivMoVLfFooA = P_1;
				UUBtHmKLROhzMfDsFdIGdhnbifcg = default(KeyValuePair<TKey, TValue>);
			}

			public bool MoveNext()
			{
				if (MKKFXwEoivJMwrLryJUtLvZbHeDG != ijNgrdFjzcMGkfODXsixVSRUmchl.NWhsmTLafNBAmfcZDbqzAzkfnjUv.Version)
				{
					throw new Exception();
				}
				if ((uint)UsnUqjALQLkUAMRjsAhiFDuBiyav < (uint)ijNgrdFjzcMGkfODXsixVSRUmchl.NWhsmTLafNBAmfcZDbqzAzkfnjUv._count)
				{
					UUBtHmKLROhzMfDsFdIGdhnbifcg = new KeyValuePair<TKey, TValue>(ijNgrdFjzcMGkfODXsixVSRUmchl.NWhsmTLafNBAmfcZDbqzAzkfnjUv._items[UsnUqjALQLkUAMRjsAhiFDuBiyav].OrRWAMnVDTsUpKngurTdhIIAtaJX, ijNgrdFjzcMGkfODXsixVSRUmchl.NWhsmTLafNBAmfcZDbqzAzkfnjUv._items[UsnUqjALQLkUAMRjsAhiFDuBiyav].EooecqekyDnZzgRwqPZWbVsdTCSJb);
					UsnUqjALQLkUAMRjsAhiFDuBiyav++;
					return true;
				}
				UsnUqjALQLkUAMRjsAhiFDuBiyav = ijNgrdFjzcMGkfODXsixVSRUmchl.NWhsmTLafNBAmfcZDbqzAzkfnjUv._count + 1;
				UUBtHmKLROhzMfDsFdIGdhnbifcg = default(KeyValuePair<TKey, TValue>);
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
				if (MKKFXwEoivJMwrLryJUtLvZbHeDG != ijNgrdFjzcMGkfODXsixVSRUmchl.NWhsmTLafNBAmfcZDbqzAzkfnjUv.Version)
				{
					throw new Exception();
				}
				UsnUqjALQLkUAMRjsAhiFDuBiyav = 0;
				UUBtHmKLROhzMfDsFdIGdhnbifcg = default(KeyValuePair<TKey, TValue>);
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
				private IndexedDictionary<TKey, TValue> jBhGUxWBDCHnxnofzVvzczENFyqO;

				private int yLoXMWxSKPNOYuIxTfYcCCYTpHvR;

				private int HwbSSoXrzqzeMXEXFCjlFhphFXgm;

				private TKey ulYBLgmNJUbFkzpaHgDQIjCtMdRJ;

				TKey IEnumerator<TKey>.Current => ulYBLgmNJUbFkzpaHgDQIjCtMdRJ;

				object IEnumerator.Current
				{
					get
					{
						if (yLoXMWxSKPNOYuIxTfYcCCYTpHvR == 0 || yLoXMWxSKPNOYuIxTfYcCCYTpHvR == jBhGUxWBDCHnxnofzVvzczENFyqO.NWhsmTLafNBAmfcZDbqzAzkfnjUv._count + 1)
						{
							throw new Exception();
						}
						return ulYBLgmNJUbFkzpaHgDQIjCtMdRJ;
					}
				}

				internal Enumerator(IndexedDictionary<TKey, TValue> P_0)
				{
					jBhGUxWBDCHnxnofzVvzczENFyqO = P_0;
					HwbSSoXrzqzeMXEXFCjlFhphFXgm = P_0.NWhsmTLafNBAmfcZDbqzAzkfnjUv.Version;
					yLoXMWxSKPNOYuIxTfYcCCYTpHvR = 0;
					ulYBLgmNJUbFkzpaHgDQIjCtMdRJ = default(TKey);
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
					if (HwbSSoXrzqzeMXEXFCjlFhphFXgm != jBhGUxWBDCHnxnofzVvzczENFyqO.NWhsmTLafNBAmfcZDbqzAzkfnjUv.Version)
					{
						throw new Exception();
					}
					if ((uint)yLoXMWxSKPNOYuIxTfYcCCYTpHvR < (uint)jBhGUxWBDCHnxnofzVvzczENFyqO.NWhsmTLafNBAmfcZDbqzAzkfnjUv._count)
					{
						ulYBLgmNJUbFkzpaHgDQIjCtMdRJ = jBhGUxWBDCHnxnofzVvzczENFyqO.NWhsmTLafNBAmfcZDbqzAzkfnjUv._items[yLoXMWxSKPNOYuIxTfYcCCYTpHvR].OrRWAMnVDTsUpKngurTdhIIAtaJX;
						yLoXMWxSKPNOYuIxTfYcCCYTpHvR++;
						return true;
					}
					yLoXMWxSKPNOYuIxTfYcCCYTpHvR = jBhGUxWBDCHnxnofzVvzczENFyqO.NWhsmTLafNBAmfcZDbqzAzkfnjUv._count + 1;
					ulYBLgmNJUbFkzpaHgDQIjCtMdRJ = default(TKey);
					return false;
				}

				bool IEnumerator.MoveNext()
				{
					//ILSpy generated this explicit interface implementation from .override directive in MoveNext
					return this.MoveNext();
				}

				void IEnumerator.Reset()
				{
					if (HwbSSoXrzqzeMXEXFCjlFhphFXgm != jBhGUxWBDCHnxnofzVvzczENFyqO.NWhsmTLafNBAmfcZDbqzAzkfnjUv.Version)
					{
						throw new Exception();
					}
					yLoXMWxSKPNOYuIxTfYcCCYTpHvR = 0;
					ulYBLgmNJUbFkzpaHgDQIjCtMdRJ = default(TKey);
				}
			}

			private IndexedDictionary<TKey, TValue> MSigxaqxlioYfuMSgqaAKhhWfvvFA;

			int ICollection.Count => MSigxaqxlioYfuMSgqaAKhhWfvvFA.Count;

			bool ICollection<TKey>.IsReadOnly => true;

			bool ICollection.IsSynchronized => false;

			object ICollection.SyncRoot => ((ICollection)MSigxaqxlioYfuMSgqaAKhhWfvvFA).SyncRoot;

			public KeyCollection(IndexedDictionary<TKey, TValue> P_0)
			{
				if (P_0 == null)
				{
					throw new ArgumentNullException("dictionary");
				}
				MSigxaqxlioYfuMSgqaAKhhWfvvFA = P_0;
			}

			public Enumerator GetEnumerator()
			{
				return new Enumerator(MSigxaqxlioYfuMSgqaAKhhWfvvFA);
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
				if (array.Length - index < MSigxaqxlioYfuMSgqaAKhhWfvvFA.Count)
				{
					throw new Exception();
				}
				int count = MSigxaqxlioYfuMSgqaAKhhWfvvFA.NWhsmTLafNBAmfcZDbqzAzkfnjUv._count;
				PLpnOJdgdqHFEgwDpQIfvNWlxDKBA[] items = MSigxaqxlioYfuMSgqaAKhhWfvvFA.NWhsmTLafNBAmfcZDbqzAzkfnjUv._items;
				for (int i = 0; i < count; i++)
				{
					array[index++] = items[i].OrRWAMnVDTsUpKngurTdhIIAtaJX;
				}
			}

			void ICollection<TKey>.CopyTo(TKey[] array, int index)
			{
				//ILSpy generated this explicit interface implementation from .override directive in CopyTo
				this.CopyTo(array, index);
			}

			private void yBQFhNmiAUJttLxvmOXVcJgdlAnu(TKey P_0)
			{
				throw new Exception();
			}

			void ICollection<TKey>.Add(TKey P_0)
			{
				//ILSpy generated this explicit interface implementation from .override directive in yBQFhNmiAUJttLxvmOXVcJgdlAnu
				this.yBQFhNmiAUJttLxvmOXVcJgdlAnu(P_0);
			}

			private void GSznhViBCcTUHqBwbUpxEnzdryrt()
			{
				throw new Exception();
			}

			void ICollection<TKey>.Clear()
			{
				//ILSpy generated this explicit interface implementation from .override directive in GSznhViBCcTUHqBwbUpxEnzdryrt
				this.GSznhViBCcTUHqBwbUpxEnzdryrt();
			}

			private bool eUAciYgFWuJMqLwwKVJlpBhcZJnWA(TKey P_0)
			{
				return MSigxaqxlioYfuMSgqaAKhhWfvvFA.ContainsKey(P_0);
			}

			bool ICollection<TKey>.Contains(TKey P_0)
			{
				//ILSpy generated this explicit interface implementation from .override directive in eUAciYgFWuJMqLwwKVJlpBhcZJnWA
				return this.eUAciYgFWuJMqLwwKVJlpBhcZJnWA(P_0);
			}

			private bool fFHxsWzzgjyJayXbPpQltSaPvvoh(TKey P_0)
			{
				throw new Exception();
			}

			bool ICollection<TKey>.Remove(TKey P_0)
			{
				//ILSpy generated this explicit interface implementation from .override directive in fFHxsWzzgjyJayXbPpQltSaPvvoh
				return this.fFHxsWzzgjyJayXbPpQltSaPvvoh(P_0);
			}

			private IEnumerator<TKey> NUwbDiKHdCnHhYVJrlwvCqNCSkPPc()
			{
				return new Enumerator(MSigxaqxlioYfuMSgqaAKhhWfvvFA);
			}

			IEnumerator<TKey> IEnumerable<TKey>.GetEnumerator()
			{
				//ILSpy generated this explicit interface implementation from .override directive in NUwbDiKHdCnHhYVJrlwvCqNCSkPPc
				return this.NUwbDiKHdCnHhYVJrlwvCqNCSkPPc();
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return new Enumerator(MSigxaqxlioYfuMSgqaAKhhWfvvFA);
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
				if (array.Length - index < MSigxaqxlioYfuMSgqaAKhhWfvvFA.Count)
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
				int count = MSigxaqxlioYfuMSgqaAKhhWfvvFA.NWhsmTLafNBAmfcZDbqzAzkfnjUv._count;
				PLpnOJdgdqHFEgwDpQIfvNWlxDKBA[] items = MSigxaqxlioYfuMSgqaAKhhWfvvFA.NWhsmTLafNBAmfcZDbqzAzkfnjUv._items;
				try
				{
					for (int i = 0; i < count; i++)
					{
						array3[index++] = items[i].OrRWAMnVDTsUpKngurTdhIIAtaJX;
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
				private IndexedDictionary<TKey, TValue> mvNUKFiLshFAPBNbBbvOHITBsmug;

				private int ryQlGkegfOVAiAeBHFPlYJJyhBPv;

				private int MovEprVhwEdXPtDEdLNgFvpCSnB;

				private TValue jfMEAcDFtVckRmpLfrwKEMBHRksTA;

				TValue IEnumerator<TValue>.Current => jfMEAcDFtVckRmpLfrwKEMBHRksTA;

				object IEnumerator.Current
				{
					get
					{
						if (ryQlGkegfOVAiAeBHFPlYJJyhBPv == 0 || ryQlGkegfOVAiAeBHFPlYJJyhBPv == mvNUKFiLshFAPBNbBbvOHITBsmug.NWhsmTLafNBAmfcZDbqzAzkfnjUv._count + 1)
						{
							throw new Exception();
						}
						return jfMEAcDFtVckRmpLfrwKEMBHRksTA;
					}
				}

				internal Enumerator(IndexedDictionary<TKey, TValue> P_0)
				{
					mvNUKFiLshFAPBNbBbvOHITBsmug = P_0;
					MovEprVhwEdXPtDEdLNgFvpCSnB = P_0.NWhsmTLafNBAmfcZDbqzAzkfnjUv.Version;
					ryQlGkegfOVAiAeBHFPlYJJyhBPv = 0;
					jfMEAcDFtVckRmpLfrwKEMBHRksTA = default(TValue);
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
					if (MovEprVhwEdXPtDEdLNgFvpCSnB != mvNUKFiLshFAPBNbBbvOHITBsmug.NWhsmTLafNBAmfcZDbqzAzkfnjUv.Version)
					{
						throw new Exception();
					}
					if ((uint)ryQlGkegfOVAiAeBHFPlYJJyhBPv < (uint)mvNUKFiLshFAPBNbBbvOHITBsmug.NWhsmTLafNBAmfcZDbqzAzkfnjUv._count)
					{
						jfMEAcDFtVckRmpLfrwKEMBHRksTA = mvNUKFiLshFAPBNbBbvOHITBsmug.NWhsmTLafNBAmfcZDbqzAzkfnjUv._items[ryQlGkegfOVAiAeBHFPlYJJyhBPv].EooecqekyDnZzgRwqPZWbVsdTCSJb;
						ryQlGkegfOVAiAeBHFPlYJJyhBPv++;
						return true;
					}
					ryQlGkegfOVAiAeBHFPlYJJyhBPv = mvNUKFiLshFAPBNbBbvOHITBsmug.NWhsmTLafNBAmfcZDbqzAzkfnjUv._count + 1;
					jfMEAcDFtVckRmpLfrwKEMBHRksTA = default(TValue);
					return false;
				}

				bool IEnumerator.MoveNext()
				{
					//ILSpy generated this explicit interface implementation from .override directive in MoveNext
					return this.MoveNext();
				}

				void IEnumerator.Reset()
				{
					if (MovEprVhwEdXPtDEdLNgFvpCSnB != mvNUKFiLshFAPBNbBbvOHITBsmug.NWhsmTLafNBAmfcZDbqzAzkfnjUv.Version)
					{
						throw new Exception();
					}
					ryQlGkegfOVAiAeBHFPlYJJyhBPv = 0;
					jfMEAcDFtVckRmpLfrwKEMBHRksTA = default(TValue);
				}
			}

			private IndexedDictionary<TKey, TValue> xwQJFKkFCRIkrjcGIqUkaclHuGBYB;

			int ICollection<TValue>.Count => xwQJFKkFCRIkrjcGIqUkaclHuGBYB.Count;

			bool ICollection<TValue>.IsReadOnly => true;

			bool ICollection.IsSynchronized => false;

			object ICollection.SyncRoot => ((ICollection)xwQJFKkFCRIkrjcGIqUkaclHuGBYB).SyncRoot;

			public ValueCollection(IndexedDictionary<TKey, TValue> P_0)
			{
				if (P_0 == null)
				{
					throw new ArgumentNullException("dictionary");
				}
				xwQJFKkFCRIkrjcGIqUkaclHuGBYB = P_0;
			}

			public Enumerator GetEnumerator()
			{
				return new Enumerator(xwQJFKkFCRIkrjcGIqUkaclHuGBYB);
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
				if (array.Length - index < xwQJFKkFCRIkrjcGIqUkaclHuGBYB.Count)
				{
					throw new Exception();
				}
				int count = xwQJFKkFCRIkrjcGIqUkaclHuGBYB.NWhsmTLafNBAmfcZDbqzAzkfnjUv._count;
				PLpnOJdgdqHFEgwDpQIfvNWlxDKBA[] items = xwQJFKkFCRIkrjcGIqUkaclHuGBYB.NWhsmTLafNBAmfcZDbqzAzkfnjUv._items;
				for (int i = 0; i < count; i++)
				{
					array[index++] = items[i].EooecqekyDnZzgRwqPZWbVsdTCSJb;
				}
			}

			void ICollection<TValue>.CopyTo(TValue[] array, int index)
			{
				//ILSpy generated this explicit interface implementation from .override directive in CopyTo
				this.CopyTo(array, index);
			}

			private void HovnXhfhpOsGgoJjcEBzgYtcSfMuA(TValue P_0)
			{
				throw new Exception();
			}

			void ICollection<TValue>.Add(TValue P_0)
			{
				//ILSpy generated this explicit interface implementation from .override directive in HovnXhfhpOsGgoJjcEBzgYtcSfMuA
				this.HovnXhfhpOsGgoJjcEBzgYtcSfMuA(P_0);
			}

			private bool KLrQFBHOUBmZVWYJxTlAtTwCAPhl(TValue P_0)
			{
				throw new Exception();
			}

			bool ICollection<TValue>.Remove(TValue P_0)
			{
				//ILSpy generated this explicit interface implementation from .override directive in KLrQFBHOUBmZVWYJxTlAtTwCAPhl
				return this.KLrQFBHOUBmZVWYJxTlAtTwCAPhl(P_0);
			}

			private void YDrksbSkstmSalEVfCSuNouHprPg()
			{
				throw new Exception();
			}

			void ICollection<TValue>.Clear()
			{
				//ILSpy generated this explicit interface implementation from .override directive in YDrksbSkstmSalEVfCSuNouHprPg
				this.YDrksbSkstmSalEVfCSuNouHprPg();
			}

			private bool jOEzwWYpaufAxjaVErdbwmYyAJAD(TValue P_0)
			{
				return xwQJFKkFCRIkrjcGIqUkaclHuGBYB.ContainsValue(P_0);
			}

			bool ICollection<TValue>.Contains(TValue P_0)
			{
				//ILSpy generated this explicit interface implementation from .override directive in jOEzwWYpaufAxjaVErdbwmYyAJAD
				return this.jOEzwWYpaufAxjaVErdbwmYyAJAD(P_0);
			}

			private IEnumerator<TValue> jdnlvumnWleIbbeJvohBEtCfLuMC()
			{
				return new Enumerator(xwQJFKkFCRIkrjcGIqUkaclHuGBYB);
			}

			IEnumerator<TValue> IEnumerable<TValue>.GetEnumerator()
			{
				//ILSpy generated this explicit interface implementation from .override directive in jdnlvumnWleIbbeJvohBEtCfLuMC
				return this.jdnlvumnWleIbbeJvohBEtCfLuMC();
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return new Enumerator(xwQJFKkFCRIkrjcGIqUkaclHuGBYB);
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
				if (array.Length - index < xwQJFKkFCRIkrjcGIqUkaclHuGBYB.Count)
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
				int count = xwQJFKkFCRIkrjcGIqUkaclHuGBYB.NWhsmTLafNBAmfcZDbqzAzkfnjUv._count;
				PLpnOJdgdqHFEgwDpQIfvNWlxDKBA[] items = xwQJFKkFCRIkrjcGIqUkaclHuGBYB.NWhsmTLafNBAmfcZDbqzAzkfnjUv._items;
				try
				{
					for (int i = 0; i < count; i++)
					{
						array3[index++] = items[i].EooecqekyDnZzgRwqPZWbVsdTCSJb;
					}
				}
				catch (ArrayTypeMismatchException)
				{
					throw new Exception();
				}
			}
		}

		private static readonly bool AzUPPgEgHrIoMTIQQKzSkVEgDnTI = ReflectionTools.IsValueType(typeof(TKey));

		private static readonly bool yZQCPiBZbCyHUGKxzkvDdNqaehHv = ReflectionTools.IsValueType(typeof(TValue));

		private IEqualityComparer<TKey> hKlMtciWczsbgdANoPABUCxXtenG = EqualityComparerNoAlloc<TKey>.Default;

		private IEqualityComparer<TValue> TPsWCjYoiUncwThVBIEtSevGWRyw = EqualityComparerNoAlloc<TValue>.Default;

		private readonly AList<PLpnOJdgdqHFEgwDpQIfvNWlxDKBA> NWhsmTLafNBAmfcZDbqzAzkfnjUv;

		private readonly ADictionary<TKey, int> pQAopWSVEMJtHChuDbWhmmEiFEUhA;

		private bool ZOhZuabMvpnoVCdgIaWjfziYFpDs;

		int ICollection<KeyValuePair<TKey, TValue>>.Count => NWhsmTLafNBAmfcZDbqzAzkfnjUv._count;

		public bool ContainsDuplicateKeys
		{
			get
			{
				if (!ZOhZuabMvpnoVCdgIaWjfziYFpDs)
				{
					return false;
				}
				return pQAopWSVEMJtHChuDbWhmmEiFEUhA._count < NWhsmTLafNBAmfcZDbqzAzkfnjUv._count;
			}
		}

		public bool AllowDuplicateKeys
		{
			get
			{
				return ZOhZuabMvpnoVCdgIaWjfziYFpDs;
			}
			set
			{
				if (ZOhZuabMvpnoVCdgIaWjfziYFpDs != value)
				{
					ZOhZuabMvpnoVCdgIaWjfziYFpDs = value;
					if (!value && ContainsDuplicateKeys)
					{
						throw new Exception("The dictionary contains duplicate keys and cannot be changed unless the keys are removed.");
					}
				}
			}
		}

		public TValue this[int index]
		{
			get
			{
				if ((uint)index >= (uint)NWhsmTLafNBAmfcZDbqzAzkfnjUv._count)
				{
					throw new ArgumentOutOfRangeException("index");
				}
				return NWhsmTLafNBAmfcZDbqzAzkfnjUv._items[index].EooecqekyDnZzgRwqPZWbVsdTCSJb;
			}
			set
			{
				if ((uint)index >= (uint)NWhsmTLafNBAmfcZDbqzAzkfnjUv._count)
				{
					throw new ArgumentOutOfRangeException("index");
				}
				NWhsmTLafNBAmfcZDbqzAzkfnjUv._items[index].EooecqekyDnZzgRwqPZWbVsdTCSJb = value;
			}
		}

		public IEqualityComparer<TKey> KeyComparer
		{
			get
			{
				return hKlMtciWczsbgdANoPABUCxXtenG;
			}
			set
			{
				if (value == null)
				{
					value = EqualityComparerNoAlloc<TKey>.Default;
				}
				hKlMtciWczsbgdANoPABUCxXtenG = value;
			}
		}

		public IEqualityComparer<TValue> ValueComparer
		{
			get
			{
				return TPsWCjYoiUncwThVBIEtSevGWRyw;
			}
			set
			{
				if (value == null)
				{
					value = EqualityComparerNoAlloc<TValue>.Default;
				}
				TPsWCjYoiUncwThVBIEtSevGWRyw = value;
			}
		}

		ICollection<TKey> IDictionary<TKey, TValue>.Keys => new KeyCollection(this);

		ICollection<TValue> IDictionary<TKey, TValue>.Values => new ValueCollection(this);

		bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly => false;

		TValue IDictionary<TKey, TValue>.this[TKey P_0]
		{
			get
			{
				int num = IndexOfKey(P_0);
				if (num < 0)
				{
					TKey val = P_0;
					throw new KeyNotFoundException("Key \"" + val?.ToString() + "\" does not exist.");
				}
				return NWhsmTLafNBAmfcZDbqzAzkfnjUv._items[num].EooecqekyDnZzgRwqPZWbVsdTCSJb;
			}
			set
			{
				SetValue(key, value2);
			}
		}

		bool IDictionary.IsFixedSize => false;

		bool IDictionary.IsReadOnly => false;

		ICollection IDictionary.Keys => new KeyCollection(this);

		ICollection IDictionary.Values => new ValueCollection(this);

		object IDictionary.this[object key]
		{
			get
			{
				return ((IDictionary<TKey, TValue>)this)[(TKey)key];
			}
			set
			{
				((IDictionary<TKey, TValue>)this)[(TKey)key] = (TValue)value;
			}
		}

		bool ICollection.IsSynchronized => ((ICollection)NWhsmTLafNBAmfcZDbqzAzkfnjUv).IsSynchronized;

		object ICollection.SyncRoot => ((ICollection)NWhsmTLafNBAmfcZDbqzAzkfnjUv).SyncRoot;

		TValue Rewired.Utils.Interfaces.IReadOnlyList<TValue>.this[int P_0] => this[P_0];

		int IReadOnlyList.Count => this.Count;

		object IReadOnlyList.this[int P_0] => this[P_0];

		public IndexedDictionary()
			: this(0, false)
		{
		}

		public IndexedDictionary(int P_0)
			: this(P_0, false)
		{
		}

		public IndexedDictionary(bool P_0)
			: this(0, P_0)
		{
		}

		public IndexedDictionary(int P_0, bool P_1)
		{
			if (P_0 < 0)
			{
				throw new ArgumentOutOfRangeException("capacity");
			}
			ZOhZuabMvpnoVCdgIaWjfziYFpDs = P_1;
			NWhsmTLafNBAmfcZDbqzAzkfnjUv = new AList<PLpnOJdgdqHFEgwDpQIfvNWlxDKBA>(P_0);
			pQAopWSVEMJtHChuDbWhmmEiFEUhA = new ADictionary<TKey, int>(P_0);
		}

		public IndexedDictionary(IDictionary<TKey, TValue> P_0)
			: this(P_0, false)
		{
		}

		public IndexedDictionary(IDictionary<TKey, TValue> P_0, bool P_1)
			: this(0, P_1)
		{
			if (P_0 == null)
			{
				throw new ArgumentNullException("dictionary");
			}
			if (ReflectionTools.DoesTypeImplement(P_0.GetType(), typeof(IndexedDictionary<TKey, TValue>)))
			{
				IndexedDictionary<TKey, TValue> indexedDictionary = (IndexedDictionary<TKey, TValue>)P_0;
				for (int i = 0; i < indexedDictionary.NWhsmTLafNBAmfcZDbqzAzkfnjUv._count; i++)
				{
					Add(indexedDictionary.NWhsmTLafNBAmfcZDbqzAzkfnjUv._items[i].OrRWAMnVDTsUpKngurTdhIIAtaJX, indexedDictionary.NWhsmTLafNBAmfcZDbqzAzkfnjUv._items[i].EooecqekyDnZzgRwqPZWbVsdTCSJb);
				}
				return;
			}
			foreach (KeyValuePair<TKey, TValue> item in P_0)
			{
				Add(item.Key, item.Value);
			}
		}

		public TValue GetValue(TKey key)
		{
			return NWhsmTLafNBAmfcZDbqzAzkfnjUv._items[pQAopWSVEMJtHChuDbWhmmEiFEUhA[key]].EooecqekyDnZzgRwqPZWbVsdTCSJb;
		}

		public bool TryGetValue(TKey key, out TValue value)
		{
			if (!pQAopWSVEMJtHChuDbWhmmEiFEUhA.TryGetValue(key, out var value2))
			{
				value = default(TValue);
				return false;
			}
			value = NWhsmTLafNBAmfcZDbqzAzkfnjUv._items[value2].EooecqekyDnZzgRwqPZWbVsdTCSJb;
			return true;
		}

		bool IDictionary<TKey, TValue>.TryGetValue(TKey key, out TValue value)
		{
			//ILSpy generated this explicit interface implementation from .override directive in TryGetValue
			return this.TryGetValue(key, out value);
		}

		public TKey GetKeyAt(int index)
		{
			if ((uint)index >= (uint)NWhsmTLafNBAmfcZDbqzAzkfnjUv._count)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			return ((AList<IndexedDictionary<PLpnOJdgdqHFEgwDpQIfvNWlxDKBA, _003F>.PLpnOJdgdqHFEgwDpQIfvNWlxDKBA>)(object)NWhsmTLafNBAmfcZDbqzAzkfnjUv)[index].OrRWAMnVDTsUpKngurTdhIIAtaJX;
		}

		public KeyValuePair<TKey, TValue> GetEntry(TKey key)
		{
			return ((AList<IndexedDictionary<PLpnOJdgdqHFEgwDpQIfvNWlxDKBA, _003F>.PLpnOJdgdqHFEgwDpQIfvNWlxDKBA>)(object)NWhsmTLafNBAmfcZDbqzAzkfnjUv)[pQAopWSVEMJtHChuDbWhmmEiFEUhA[key]].uRwaZRPOZpdFNtvnfYUQtpgnVcVU();
		}

		public KeyValuePair<TKey, TValue> GetEntryAt(int index)
		{
			if ((uint)index >= (uint)NWhsmTLafNBAmfcZDbqzAzkfnjUv._count)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			return ((AList<IndexedDictionary<PLpnOJdgdqHFEgwDpQIfvNWlxDKBA, _003F>.PLpnOJdgdqHFEgwDpQIfvNWlxDKBA>)(object)NWhsmTLafNBAmfcZDbqzAzkfnjUv)[index].uRwaZRPOZpdFNtvnfYUQtpgnVcVU();
		}

		public bool TryGetEntry(TKey key, out KeyValuePair<TKey, TValue> entry)
		{
			if (!pQAopWSVEMJtHChuDbWhmmEiFEUhA.TryGetValue(key, out var value))
			{
				entry = default(KeyValuePair<TKey, TValue>);
				return false;
			}
			entry = ((AList<IndexedDictionary<PLpnOJdgdqHFEgwDpQIfvNWlxDKBA, _003F>.PLpnOJdgdqHFEgwDpQIfvNWlxDKBA>)(object)NWhsmTLafNBAmfcZDbqzAzkfnjUv)[value].uRwaZRPOZpdFNtvnfYUQtpgnVcVU();
			return true;
		}

		public void Add(TKey key, TValue value)
		{
			bool num = pQAopWSVEMJtHChuDbWhmmEiFEUhA.ContainsKey(key);
			if (num && !ZOhZuabMvpnoVCdgIaWjfziYFpDs)
			{
				TKey val = key;
				throw new ArgumentException("Key \"" + val?.ToString() + "\" is already in use.");
			}
			int num2 = NWhsmTLafNBAmfcZDbqzAzkfnjUv.Add(new PLpnOJdgdqHFEgwDpQIfvNWlxDKBA(key, value));
			if (num)
			{
				pQAopWSVEMJtHChuDbWhmmEiFEUhA[key] = num2;
			}
			else
			{
				pQAopWSVEMJtHChuDbWhmmEiFEUhA.Add(key, num2);
			}
		}

		void IDictionary<TKey, TValue>.Add(TKey key, TValue value)
		{
			//ILSpy generated this explicit interface implementation from .override directive in Add
			this.Add(key, value);
		}

		public void SetValue(TKey key, TValue value)
		{
			if (pQAopWSVEMJtHChuDbWhmmEiFEUhA.TryGetValue(key, out var value2))
			{
				NWhsmTLafNBAmfcZDbqzAzkfnjUv._items[value2].EooecqekyDnZzgRwqPZWbVsdTCSJb = value;
				pQAopWSVEMJtHChuDbWhmmEiFEUhA[key] = value2;
			}
			else
			{
				Add(key, value);
			}
		}

		public bool Remove(TKey key)
		{
			pQAopWSVEMJtHChuDbWhmmEiFEUhA.Remove(key);
			if (ZOhZuabMvpnoVCdgIaWjfziYFpDs)
			{
				bool result = false;
				for (int num = NWhsmTLafNBAmfcZDbqzAzkfnjUv._count - 1; num >= 0; num--)
				{
					if (hKlMtciWczsbgdANoPABUCxXtenG.Equals(NWhsmTLafNBAmfcZDbqzAzkfnjUv._items[num].OrRWAMnVDTsUpKngurTdhIIAtaJX, key))
					{
						NWhsmTLafNBAmfcZDbqzAzkfnjUv.RemoveAt(num);
						result = true;
					}
				}
				return result;
			}
			int num2 = IndexOfKey(key);
			if (num2 < 0)
			{
				return false;
			}
			RemoveAt(num2);
			return true;
		}

		bool IDictionary<TKey, TValue>.Remove(TKey key)
		{
			//ILSpy generated this explicit interface implementation from .override directive in Remove
			return this.Remove(key);
		}

		public void RemoveAt(int index)
		{
			if ((uint)index >= (uint)NWhsmTLafNBAmfcZDbqzAzkfnjUv._count)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			TKey orRWAMnVDTsUpKngurTdhIIAtaJX = NWhsmTLafNBAmfcZDbqzAzkfnjUv._items[index].OrRWAMnVDTsUpKngurTdhIIAtaJX;
			if (index < NWhsmTLafNBAmfcZDbqzAzkfnjUv._count - 1)
			{
				for (int i = index + 1; i < ((AList<IndexedDictionary<PLpnOJdgdqHFEgwDpQIfvNWlxDKBA, _003F>.PLpnOJdgdqHFEgwDpQIfvNWlxDKBA>)(object)NWhsmTLafNBAmfcZDbqzAzkfnjUv).Count; i++)
				{
					pQAopWSVEMJtHChuDbWhmmEiFEUhA[NWhsmTLafNBAmfcZDbqzAzkfnjUv._items[i].OrRWAMnVDTsUpKngurTdhIIAtaJX] = i - 1;
				}
			}
			NWhsmTLafNBAmfcZDbqzAzkfnjUv.RemoveAt(index);
			pQAopWSVEMJtHChuDbWhmmEiFEUhA.Remove(orRWAMnVDTsUpKngurTdhIIAtaJX);
		}

		public void RemoveValue(TValue value)
		{
			int num = IndexOfValue(value);
			if (num >= 0)
			{
				_ = ref NWhsmTLafNBAmfcZDbqzAzkfnjUv._items[num];
				RemoveAt(num);
			}
		}

		public int RemoveAll(TValue value)
		{
			int num = 0;
			for (int num2 = NWhsmTLafNBAmfcZDbqzAzkfnjUv._count - 1; num2 >= 0; num2--)
			{
				_ = ref NWhsmTLafNBAmfcZDbqzAzkfnjUv._items[num2];
				if (TPsWCjYoiUncwThVBIEtSevGWRyw.Equals(NWhsmTLafNBAmfcZDbqzAzkfnjUv._items[num2].EooecqekyDnZzgRwqPZWbVsdTCSJb, value))
				{
					RemoveAt(num2);
					num++;
				}
			}
			return num;
		}

		public int IndexOfKey(TKey key)
		{
			if (!AzUPPgEgHrIoMTIQQKzSkVEgDnTI && key == null)
			{
				throw new ArgumentNullException("key");
			}
			int count = NWhsmTLafNBAmfcZDbqzAzkfnjUv._count;
			for (int i = 0; i < count; i++)
			{
				if (hKlMtciWczsbgdANoPABUCxXtenG.Equals(NWhsmTLafNBAmfcZDbqzAzkfnjUv._items[i].OrRWAMnVDTsUpKngurTdhIIAtaJX, key))
				{
					return i;
				}
			}
			return -1;
		}

		public int IndexOfValue(TValue value)
		{
			int count = NWhsmTLafNBAmfcZDbqzAzkfnjUv._count;
			for (int i = 0; i < count; i++)
			{
				if (TPsWCjYoiUncwThVBIEtSevGWRyw.Equals(NWhsmTLafNBAmfcZDbqzAzkfnjUv._items[i].EooecqekyDnZzgRwqPZWbVsdTCSJb, value))
				{
					return i;
				}
			}
			return -1;
		}

		public bool ContainsKey(TKey key)
		{
			return pQAopWSVEMJtHChuDbWhmmEiFEUhA.ContainsKey(key);
		}

		bool IDictionary<TKey, TValue>.ContainsKey(TKey key)
		{
			//ILSpy generated this explicit interface implementation from .override directive in ContainsKey
			return this.ContainsKey(key);
		}

		public bool ContainsValue(TValue value)
		{
			return IndexOfValue(value) >= 0;
		}

		public void Clear()
		{
			NWhsmTLafNBAmfcZDbqzAzkfnjUv.Clear();
			pQAopWSVEMJtHChuDbWhmmEiFEUhA.Clear();
		}

		void IDictionary.Clear()
		{
			//ILSpy generated this explicit interface implementation from .override directive in Clear
			this.Clear();
		}

		void ICollection<KeyValuePair<TKey, TValue>>.Clear()
		{
			//ILSpy generated this explicit interface implementation from .override directive in Clear
			this.Clear();
		}

		public void TrimExcess()
		{
			NWhsmTLafNBAmfcZDbqzAzkfnjUv.TrimExcess();
		}

		private void NPhzHHmosZOfbUqXuMOyGLDPNHwp(KeyValuePair<TKey, TValue> P_0)
		{
			Add(P_0.Key, P_0.Value);
		}

		void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> P_0)
		{
			//ILSpy generated this explicit interface implementation from .override directive in NPhzHHmosZOfbUqXuMOyGLDPNHwp
			this.NPhzHHmosZOfbUqXuMOyGLDPNHwp(P_0);
		}

		private bool zvQmWlqgyCgYKUMoebQStqDUnPgw(KeyValuePair<TKey, TValue> P_0)
		{
			int num = IndexOfKey(P_0.Key);
			if (num < 0)
			{
				return false;
			}
			PLpnOJdgdqHFEgwDpQIfvNWlxDKBA pLpnOJdgdqHFEgwDpQIfvNWlxDKBA = NWhsmTLafNBAmfcZDbqzAzkfnjUv._items[num];
			return TPsWCjYoiUncwThVBIEtSevGWRyw.Equals(P_0.Value, pLpnOJdgdqHFEgwDpQIfvNWlxDKBA.EooecqekyDnZzgRwqPZWbVsdTCSJb);
		}

		bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> P_0)
		{
			//ILSpy generated this explicit interface implementation from .override directive in zvQmWlqgyCgYKUMoebQStqDUnPgw
			return this.zvQmWlqgyCgYKUMoebQStqDUnPgw(P_0);
		}

		private void CNbtWdqCndmNwpWZaFonpLFpmAVh(KeyValuePair<TKey, TValue>[] P_0, int P_1)
		{
			if (P_0 == null)
			{
				throw new ArgumentNullException("array");
			}
			if (P_1 < 0 || P_1 > P_0.Length)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			if (P_0.Length - P_1 < this.Count)
			{
				throw new Exception();
			}
			int count = NWhsmTLafNBAmfcZDbqzAzkfnjUv._count;
			for (int i = 0; i < count; i++)
			{
				P_0[P_1++] = new KeyValuePair<TKey, TValue>(NWhsmTLafNBAmfcZDbqzAzkfnjUv._items[i].OrRWAMnVDTsUpKngurTdhIIAtaJX, NWhsmTLafNBAmfcZDbqzAzkfnjUv._items[i].EooecqekyDnZzgRwqPZWbVsdTCSJb);
			}
		}

		void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] P_0, int P_1)
		{
			//ILSpy generated this explicit interface implementation from .override directive in CNbtWdqCndmNwpWZaFonpLFpmAVh
			this.CNbtWdqCndmNwpWZaFonpLFpmAVh(P_0, P_1);
		}

		private bool wwlNjYmJquHQojHtuMrTHDfrLPLU(KeyValuePair<TKey, TValue> P_0)
		{
			if (ZOhZuabMvpnoVCdgIaWjfziYFpDs)
			{
				bool result = false;
				for (int num = NWhsmTLafNBAmfcZDbqzAzkfnjUv._count - 1; num >= 0; num--)
				{
					PLpnOJdgdqHFEgwDpQIfvNWlxDKBA pLpnOJdgdqHFEgwDpQIfvNWlxDKBA = NWhsmTLafNBAmfcZDbqzAzkfnjUv._items[num];
					if (TPsWCjYoiUncwThVBIEtSevGWRyw.Equals(P_0.Value, pLpnOJdgdqHFEgwDpQIfvNWlxDKBA.EooecqekyDnZzgRwqPZWbVsdTCSJb))
					{
						NWhsmTLafNBAmfcZDbqzAzkfnjUv.RemoveAt(num);
						result = true;
					}
				}
				return result;
			}
			int num2 = IndexOfKey(P_0.Key);
			if (num2 < 0)
			{
				return false;
			}
			PLpnOJdgdqHFEgwDpQIfvNWlxDKBA pLpnOJdgdqHFEgwDpQIfvNWlxDKBA2 = NWhsmTLafNBAmfcZDbqzAzkfnjUv._items[num2];
			if (!TPsWCjYoiUncwThVBIEtSevGWRyw.Equals(P_0.Value, pLpnOJdgdqHFEgwDpQIfvNWlxDKBA2.EooecqekyDnZzgRwqPZWbVsdTCSJb))
			{
				return false;
			}
			RemoveAt(num2);
			return true;
		}

		bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> P_0)
		{
			//ILSpy generated this explicit interface implementation from .override directive in wwlNjYmJquHQojHtuMrTHDfrLPLU
			return this.wwlNjYmJquHQojHtuMrTHDfrLPLU(P_0);
		}

		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
		{
			return new Enumerator(this, 1);
		}

		IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
		{
			//ILSpy generated this explicit interface implementation from .override directive in GetEnumerator
			return this.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return new Enumerator(this, 1);
		}

		void IDictionary.Add(object key, object value)
		{
			Add((TKey)key, (TValue)value);
		}

		bool IDictionary.Contains(object key)
		{
			return ContainsKey((TKey)key);
		}

		IDictionaryEnumerator IDictionary.GetEnumerator()
		{
			return new Enumerator(this, 2);
		}

		void IDictionary.Remove(object key)
		{
			Remove((TKey)key);
		}

		void ICollection.CopyTo(Array array, int index)
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
			int count = NWhsmTLafNBAmfcZDbqzAzkfnjUv._count;
			for (int i = 0; i < count; i++)
			{
				array.SetValue(new KeyValuePair<TKey, TValue>(NWhsmTLafNBAmfcZDbqzAzkfnjUv._items[i].OrRWAMnVDTsUpKngurTdhIIAtaJX, NWhsmTLafNBAmfcZDbqzAzkfnjUv._items[i].EooecqekyDnZzgRwqPZWbVsdTCSJb), index++);
			}
		}

		private int rLZDsvgScnovmEdnOldeVoOdMKRC(TValue P_0)
		{
			return IndexOfValue(P_0);
		}

		int Rewired.Utils.Interfaces.IReadOnlyList<TValue>.IndexOf(TValue P_0)
		{
			//ILSpy generated this explicit interface implementation from .override directive in rLZDsvgScnovmEdnOldeVoOdMKRC
			return this.rLZDsvgScnovmEdnOldeVoOdMKRC(P_0);
		}

		private bool gpJoCyarzKhlfrjMGdfMRrSmoHol(TValue P_0)
		{
			return ContainsValue(P_0);
		}

		bool Rewired.Utils.Interfaces.IReadOnlyList<TValue>.Contains(TValue P_0)
		{
			//ILSpy generated this explicit interface implementation from .override directive in gpJoCyarzKhlfrjMGdfMRrSmoHol
			return this.gpJoCyarzKhlfrjMGdfMRrSmoHol(P_0);
		}

		private int hSgUFkekdjTlioFycurfPjOaMWxp(object P_0)
		{
			return IndexOfValue((TValue)P_0);
		}

		int IReadOnlyList.IndexOf(object P_0)
		{
			//ILSpy generated this explicit interface implementation from .override directive in hSgUFkekdjTlioFycurfPjOaMWxp
			return this.hSgUFkekdjTlioFycurfPjOaMWxp(P_0);
		}

		private bool TDTQMatTwPdNdDclfNRieMcmLyHN(object P_0)
		{
			return ContainsValue((TValue)P_0);
		}

		bool IReadOnlyList.Contains(object P_0)
		{
			//ILSpy generated this explicit interface implementation from .override directive in TDTQMatTwPdNdDclfNRieMcmLyHN
			return this.TDTQMatTwPdNdDclfNRieMcmLyHN(P_0);
		}
	}
}
