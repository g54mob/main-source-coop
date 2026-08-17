using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Rewired.Utils;
using UnityEngine;

namespace Rewired.Data.Mapping
{
	[Serializable]
	public sealed class ActionCategoryMap
	{
		[Serializable]
		public class Entry
		{
			private sealed class gpRerkKCwuDirSplKBpbiNsgdhJPB : IEnumerable<int>, IEnumerable, IEnumerator<int>, IEnumerator, IDisposable
			{
				private int jpgjMKVggskNYwgiKpOgUdhorVXl;

				private int zbvZaMIuYHjzHLiJVbWnmmNmCjOQ;

				private int SIBteLGbQdDbwOqdCGlGGhqHxPYN;

				public Entry HJAqHKcGouDtrDOjSOJCHtBiovveb;

				private int iIfbmntQaeGcynNDaydvkhTciskE;

				int IEnumerator<int>.Current
				{
					[DebuggerHidden]
					get
					{
						return zbvZaMIuYHjzHLiJVbWnmmNmCjOQ;
					}
				}

				object IEnumerator.Current
				{
					[DebuggerHidden]
					get
					{
						return zbvZaMIuYHjzHLiJVbWnmmNmCjOQ;
					}
				}

				[DebuggerHidden]
				public gpRerkKCwuDirSplKBpbiNsgdhJPB(int P_0)
				{
					jpgjMKVggskNYwgiKpOgUdhorVXl = P_0;
					SIBteLGbQdDbwOqdCGlGGhqHxPYN = Environment.CurrentManagedThreadId;
				}

				[DebuggerHidden]
				void IDisposable.Dispose()
				{
					jpgjMKVggskNYwgiKpOgUdhorVXl = -2;
				}

				private bool MoveNext()
				{
					int num = jpgjMKVggskNYwgiKpOgUdhorVXl;
					Entry hJAqHKcGouDtrDOjSOJCHtBiovveb = HJAqHKcGouDtrDOjSOJCHtBiovveb;
					switch (num)
					{
					default:
						return false;
					case 0:
						jpgjMKVggskNYwgiKpOgUdhorVXl = -1;
						if (hJAqHKcGouDtrDOjSOJCHtBiovveb.actionIds == null)
						{
							return false;
						}
						iIfbmntQaeGcynNDaydvkhTciskE = 0;
						break;
					case 1:
						jpgjMKVggskNYwgiKpOgUdhorVXl = -1;
						iIfbmntQaeGcynNDaydvkhTciskE++;
						break;
					}
					if (iIfbmntQaeGcynNDaydvkhTciskE < hJAqHKcGouDtrDOjSOJCHtBiovveb.actionIds.Count)
					{
						zbvZaMIuYHjzHLiJVbWnmmNmCjOQ = hJAqHKcGouDtrDOjSOJCHtBiovveb.actionIds[iIfbmntQaeGcynNDaydvkhTciskE];
						jpgjMKVggskNYwgiKpOgUdhorVXl = 1;
						return true;
					}
					return false;
				}

				bool IEnumerator.MoveNext()
				{
					//ILSpy generated this explicit interface implementation from .override directive in MoveNext
					return this.MoveNext();
				}

				[DebuggerHidden]
				void IEnumerator.Reset()
				{
					throw new NotSupportedException();
				}

				[DebuggerHidden]
				IEnumerator<int> IEnumerable<int>.GetEnumerator()
				{
					gpRerkKCwuDirSplKBpbiNsgdhJPB gpRerkKCwuDirSplKBpbiNsgdhJPB2;
					if (jpgjMKVggskNYwgiKpOgUdhorVXl == -2 && SIBteLGbQdDbwOqdCGlGGhqHxPYN == Environment.CurrentManagedThreadId)
					{
						jpgjMKVggskNYwgiKpOgUdhorVXl = 0;
						gpRerkKCwuDirSplKBpbiNsgdhJPB2 = this;
					}
					else
					{
						gpRerkKCwuDirSplKBpbiNsgdhJPB2 = new gpRerkKCwuDirSplKBpbiNsgdhJPB(0);
						gpRerkKCwuDirSplKBpbiNsgdhJPB2.HJAqHKcGouDtrDOjSOJCHtBiovveb = HJAqHKcGouDtrDOjSOJCHtBiovveb;
					}
					return gpRerkKCwuDirSplKBpbiNsgdhJPB2;
				}

				[DebuggerHidden]
				IEnumerator IEnumerable.GetEnumerator()
				{
					return ((IEnumerable<int>)this).GetEnumerator();
				}
			}

			public int categoryId;

			public List<int> actionIds;

			public IEnumerable<int> ActionIds
			{
				[IteratorStateMachine(typeof(gpRerkKCwuDirSplKBpbiNsgdhJPB))]
				get
				{
					return new gpRerkKCwuDirSplKBpbiNsgdhJPB(-2)
					{
						HJAqHKcGouDtrDOjSOJCHtBiovveb = this
					};
				}
			}

			public Entry()
			{
				actionIds = new List<int>();
			}

			public Entry(int P_0)
				: this()
			{
				categoryId = P_0;
			}

			public Entry(Entry P_0)
			{
				actionIds = ListTools.ShallowCopy(P_0.actionIds);
			}

			public void AddAction(int actionId)
			{
				if (!actionIds.Contains(actionId))
				{
					actionIds.Add(actionId);
				}
			}

			public bool InsertAction(int actionId, int index)
			{
				if (index < 0)
				{
					return false;
				}
				if (actionIds.Contains(actionId))
				{
					return true;
				}
				if (index >= actionIds.Count)
				{
					actionIds.Add(actionId);
				}
				else
				{
					actionIds.Insert(index, actionId);
				}
				return true;
			}

			public bool ReorderAction(int actionId, bool offsetDown, bool offsetNow)
			{
				int num = IndexOfAction(actionId);
				if (num < 0)
				{
					return false;
				}
				if (!offsetDown && num == 0)
				{
					return false;
				}
				if (offsetDown && num >= actionIds.Count - 1)
				{
					return false;
				}
				if (!offsetNow)
				{
					return true;
				}
				int value = actionIds[num];
				if (offsetDown)
				{
					actionIds[num] = actionIds[num + 1];
					actionIds[num + 1] = value;
				}
				else
				{
					actionIds[num] = actionIds[num - 1];
					actionIds[num - 1] = value;
				}
				return true;
			}

			public void RemoveAction(int actionId)
			{
				int num = IndexOfAction(actionId);
				if (num >= 0)
				{
					actionIds.RemoveAt(num);
				}
			}

			public int IndexOfAction(int id)
			{
				if (actionIds == null)
				{
					return -1;
				}
				for (int i = 0; i < actionIds.Count; i++)
				{
					if (actionIds[i] == id)
					{
						return i;
					}
				}
				return -1;
			}

			public bool ContainsAction(int id)
			{
				return IndexOfAction(id) >= 0;
			}

			public Entry Clone()
			{
				return new Entry(this);
			}
		}

		private sealed class qNSapEaENaiwuddDDjDmBESuUqJWB : IEnumerable<int>, IEnumerable, IEnumerator<int>, IEnumerator, IDisposable
		{
			private int DtRfpdAgOmpMFTCLpVcJCnrsfhUg;

			private int HsHLDXzqPHdypgdXBvrberUReeUyA;

			private int hjUgSajyCtvrOPUqSrmNEJdPrMDwA;

			public ActionCategoryMap QLuFduBDNHPNACcBIyfnWoJENQvgA;

			private int YrEVUybTDsvXKMeavsQIQmCbNVOr;

			public int vetmeXnwrZTfEqXTICiZeeDgdYBN;

			private IEnumerator<int> UOqDGbkqWdsPXachObXmqHuAuEHH;

			int IEnumerator<int>.Current
			{
				[DebuggerHidden]
				get
				{
					return HsHLDXzqPHdypgdXBvrberUReeUyA;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return HsHLDXzqPHdypgdXBvrberUReeUyA;
				}
			}

			[DebuggerHidden]
			public qNSapEaENaiwuddDDjDmBESuUqJWB(int P_0)
			{
				DtRfpdAgOmpMFTCLpVcJCnrsfhUg = P_0;
				hjUgSajyCtvrOPUqSrmNEJdPrMDwA = Environment.CurrentManagedThreadId;
			}

			[DebuggerHidden]
			void IDisposable.Dispose()
			{
				int dtRfpdAgOmpMFTCLpVcJCnrsfhUg = DtRfpdAgOmpMFTCLpVcJCnrsfhUg;
				if (dtRfpdAgOmpMFTCLpVcJCnrsfhUg == -3 || dtRfpdAgOmpMFTCLpVcJCnrsfhUg == 1)
				{
					try
					{
					}
					finally
					{
						pkQohoPlsCafGcvFDAiKUBWlzBxRA();
					}
				}
				UOqDGbkqWdsPXachObXmqHuAuEHH = null;
				DtRfpdAgOmpMFTCLpVcJCnrsfhUg = -2;
			}

			private bool MoveNext()
			{
				try
				{
					int dtRfpdAgOmpMFTCLpVcJCnrsfhUg = DtRfpdAgOmpMFTCLpVcJCnrsfhUg;
					ActionCategoryMap qLuFduBDNHPNACcBIyfnWoJENQvgA = QLuFduBDNHPNACcBIyfnWoJENQvgA;
					switch (dtRfpdAgOmpMFTCLpVcJCnrsfhUg)
					{
					default:
						return false;
					case 0:
					{
						DtRfpdAgOmpMFTCLpVcJCnrsfhUg = -1;
						if (qLuFduBDNHPNACcBIyfnWoJENQvgA.list == null)
						{
							return false;
						}
						int num = qLuFduBDNHPNACcBIyfnWoJENQvgA.IndexOfCategory(YrEVUybTDsvXKMeavsQIQmCbNVOr);
						if (num < 0)
						{
							return false;
						}
						UOqDGbkqWdsPXachObXmqHuAuEHH = qLuFduBDNHPNACcBIyfnWoJENQvgA.list[num].ActionIds.GetEnumerator();
						DtRfpdAgOmpMFTCLpVcJCnrsfhUg = -3;
						break;
					}
					case 1:
						DtRfpdAgOmpMFTCLpVcJCnrsfhUg = -3;
						break;
					}
					if (UOqDGbkqWdsPXachObXmqHuAuEHH.MoveNext())
					{
						int current = UOqDGbkqWdsPXachObXmqHuAuEHH.Current;
						HsHLDXzqPHdypgdXBvrberUReeUyA = current;
						DtRfpdAgOmpMFTCLpVcJCnrsfhUg = 1;
						return true;
					}
					pkQohoPlsCafGcvFDAiKUBWlzBxRA();
					UOqDGbkqWdsPXachObXmqHuAuEHH = null;
					return false;
				}
				catch
				{
					//try-fault
					((IDisposable)this).Dispose();
					throw;
				}
			}

			bool IEnumerator.MoveNext()
			{
				//ILSpy generated this explicit interface implementation from .override directive in MoveNext
				return this.MoveNext();
			}

			private void pkQohoPlsCafGcvFDAiKUBWlzBxRA()
			{
				DtRfpdAgOmpMFTCLpVcJCnrsfhUg = -1;
				if (UOqDGbkqWdsPXachObXmqHuAuEHH != null)
				{
					UOqDGbkqWdsPXachObXmqHuAuEHH.Dispose();
				}
			}

			[DebuggerHidden]
			void IEnumerator.Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator<int> IEnumerable<int>.GetEnumerator()
			{
				qNSapEaENaiwuddDDjDmBESuUqJWB qNSapEaENaiwuddDDjDmBESuUqJWB2;
				if (DtRfpdAgOmpMFTCLpVcJCnrsfhUg == -2 && hjUgSajyCtvrOPUqSrmNEJdPrMDwA == Environment.CurrentManagedThreadId)
				{
					DtRfpdAgOmpMFTCLpVcJCnrsfhUg = 0;
					qNSapEaENaiwuddDDjDmBESuUqJWB2 = this;
				}
				else
				{
					qNSapEaENaiwuddDDjDmBESuUqJWB2 = new qNSapEaENaiwuddDDjDmBESuUqJWB(0);
					qNSapEaENaiwuddDDjDmBESuUqJWB2.QLuFduBDNHPNACcBIyfnWoJENQvgA = QLuFduBDNHPNACcBIyfnWoJENQvgA;
				}
				qNSapEaENaiwuddDDjDmBESuUqJWB2.YrEVUybTDsvXKMeavsQIQmCbNVOr = vetmeXnwrZTfEqXTICiZeeDgdYBN;
				return qNSapEaENaiwuddDDjDmBESuUqJWB2;
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return ((IEnumerable<int>)this).GetEnumerator();
			}
		}

		[SerializeField]
		[CustomObfuscation(rename = false)]
		private List<Entry> list;

		[IteratorStateMachine(typeof(qNSapEaENaiwuddDDjDmBESuUqJWB))]
		public IEnumerable<int> ActionIdsInCategory(int categoryId)
		{
			return new qNSapEaENaiwuddDDjDmBESuUqJWB(-2)
			{
				QLuFduBDNHPNACcBIyfnWoJENQvgA = this,
				vetmeXnwrZTfEqXTICiZeeDgdYBN = categoryId
			};
		}

		public ActionCategoryMap()
		{
			list = new List<Entry>();
		}

		public ActionCategoryMap(ActionCategoryMap P_0)
		{
			if (P_0.list != null)
			{
				list = new List<Entry>(P_0.list.Count);
				for (int i = 0; i < P_0.list.Count; i++)
				{
					list[i] = P_0.list[i].Clone();
				}
			}
		}

		public void AddCategory(int id)
		{
			list.Add(new Entry(id));
		}

		public void RemoveCategory(int id)
		{
			int num = IndexOfCategory(id);
			if (num >= 0)
			{
				list.RemoveAt(num);
			}
		}

		public bool ReorderCategory(int id, bool offsetDown)
		{
			int num = IndexOfCategory(id);
			if (num < 0)
			{
				return false;
			}
			if (!offsetDown && num == 0)
			{
				return false;
			}
			if (offsetDown && num >= list.Count - 1)
			{
				return false;
			}
			Entry value = list[num];
			if (offsetDown)
			{
				list[num] = list[num + 1];
				list[num + 1] = value;
			}
			else
			{
				list[num] = list[num - 1];
				list[num - 1] = value;
			}
			return true;
		}

		public bool ChangeCategory(int actionId, int newCategoryId)
		{
			if (list == null)
			{
				return false;
			}
			bool result = false;
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].ContainsAction(actionId))
				{
					list[i].RemoveAction(actionId);
				}
			}
			for (int j = 0; j < list.Count; j++)
			{
				if (list[j].categoryId == newCategoryId)
				{
					list[j].AddAction(actionId);
					result = true;
				}
			}
			return result;
		}

		public int IndexOfCategory(int id)
		{
			if (list == null)
			{
				return -1;
			}
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].categoryId == id)
				{
					return i;
				}
			}
			return -1;
		}

		public bool AddAction(int categoryId, int actionId)
		{
			if (list == null)
			{
				return false;
			}
			int num = IndexOfCategory(categoryId);
			if (num < 0)
			{
				return false;
			}
			list[num].AddAction(actionId);
			return true;
		}

		public bool InsertAction(int categoryId, int actionId, int index)
		{
			if (index < 0)
			{
				return false;
			}
			int num = IndexOfCategory(categoryId);
			if (num < 0)
			{
				return false;
			}
			return list[num].InsertAction(actionId, index);
		}

		public bool ReorderAction(int categoryId, int actionId, bool offsetDown, bool offsetNow)
		{
			int num = IndexOfCategory(categoryId);
			if (num < 0)
			{
				return false;
			}
			return list[num].ReorderAction(actionId, offsetDown, offsetNow);
		}

		public void RemoveAction(int categoryId, int actionId)
		{
			int num = IndexOfCategory(categoryId);
			if (num >= 0)
			{
				list[num].RemoveAction(actionId);
			}
		}

		public int IndexOfAction(int categoryId, int actionId)
		{
			int num = IndexOfCategory(categoryId);
			if (num < 0)
			{
				return -1;
			}
			return list[num].IndexOfAction(actionId);
		}

		public ActionCategoryMap Clone()
		{
			return new ActionCategoryMap(this);
		}
	}
}
