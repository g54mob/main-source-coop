using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
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
			private sealed class KpKOwspLQmNeJTAqHzvkTzXndMo : IDisposable, IEnumerable, IEnumerator, IEnumerable<int>, IEnumerator<int>
			{
				private int RxAoyfYzYDsYonLGXsvUgwChukLk;

				private int VqEePGSMyrGKIqkWibsjeHcWPSIx;

				private int wLBvvzlDcbAEPyDfgnprolEZBsgN;

				public Entry TtytLoUfsgUyhsklaKccrnoMiiek;

				private int hWZNbLCFLBBWXNZxiKeGtzgrnTsg;

				int IEnumerator<int>.Current
				{
					[DebuggerHidden]
					get
					{
						return VqEePGSMyrGKIqkWibsjeHcWPSIx;
					}
				}

				object IEnumerator.Current
				{
					[DebuggerHidden]
					get
					{
						return VqEePGSMyrGKIqkWibsjeHcWPSIx;
					}
				}

				[DebuggerHidden]
				public KpKOwspLQmNeJTAqHzvkTzXndMo(int P_0)
				{
					RxAoyfYzYDsYonLGXsvUgwChukLk = P_0;
					wLBvvzlDcbAEPyDfgnprolEZBsgN = Thread.CurrentThread.ManagedThreadId;
				}

				[DebuggerHidden]
				void IDisposable.Dispose()
				{
				}

				private bool MoveNext()
				{
					int rxAoyfYzYDsYonLGXsvUgwChukLk = RxAoyfYzYDsYonLGXsvUgwChukLk;
					Entry ttytLoUfsgUyhsklaKccrnoMiiek = TtytLoUfsgUyhsklaKccrnoMiiek;
					switch (rxAoyfYzYDsYonLGXsvUgwChukLk)
					{
					default:
						return false;
					case 0:
						RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
						if (ttytLoUfsgUyhsklaKccrnoMiiek.actionIds == null)
						{
							return false;
						}
						hWZNbLCFLBBWXNZxiKeGtzgrnTsg = 0;
						break;
					case 1:
						RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
						hWZNbLCFLBBWXNZxiKeGtzgrnTsg++;
						break;
					}
					if (hWZNbLCFLBBWXNZxiKeGtzgrnTsg < ttytLoUfsgUyhsklaKccrnoMiiek.actionIds.Count)
					{
						VqEePGSMyrGKIqkWibsjeHcWPSIx = ttytLoUfsgUyhsklaKccrnoMiiek.actionIds[hWZNbLCFLBBWXNZxiKeGtzgrnTsg];
						RxAoyfYzYDsYonLGXsvUgwChukLk = 1;
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
					KpKOwspLQmNeJTAqHzvkTzXndMo kpKOwspLQmNeJTAqHzvkTzXndMo;
					if (RxAoyfYzYDsYonLGXsvUgwChukLk == -2 && wLBvvzlDcbAEPyDfgnprolEZBsgN == Thread.CurrentThread.ManagedThreadId)
					{
						RxAoyfYzYDsYonLGXsvUgwChukLk = 0;
						kpKOwspLQmNeJTAqHzvkTzXndMo = this;
					}
					else
					{
						kpKOwspLQmNeJTAqHzvkTzXndMo = new KpKOwspLQmNeJTAqHzvkTzXndMo(0);
						kpKOwspLQmNeJTAqHzvkTzXndMo.TtytLoUfsgUyhsklaKccrnoMiiek = TtytLoUfsgUyhsklaKccrnoMiiek;
					}
					return kpKOwspLQmNeJTAqHzvkTzXndMo;
				}

				[DebuggerHidden]
				IEnumerator IEnumerable.GetEnumerator()
				{
					return ((IEnumerable<int>)this).GetEnumerator();
				}
			}

			public int categoryId;

			public List<int> actionIds;

			public IEnumerable<int> ActionIds => new KpKOwspLQmNeJTAqHzvkTzXndMo(-2)
			{
				TtytLoUfsgUyhsklaKccrnoMiiek = this
			};

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

		private sealed class YKoEIaasnOPnuZcGBXgOnNhocKZB : IDisposable, IEnumerable, IEnumerator, IEnumerable<int>, IEnumerator<int>
		{
			private int RxAoyfYzYDsYonLGXsvUgwChukLk;

			private int VqEePGSMyrGKIqkWibsjeHcWPSIx;

			private int wLBvvzlDcbAEPyDfgnprolEZBsgN;

			public ActionCategoryMap TtytLoUfsgUyhsklaKccrnoMiiek;

			private int IHGOXTDxkGUFIdzxXaNUTxjYneo;

			public int lIKkctcWVTwoXwHsmpFUIPKIHahI;

			private IEnumerator<int> hSeONskmQJhVsjVUHGUASYbDGJrU;

			int IEnumerator<int>.Current
			{
				[DebuggerHidden]
				get
				{
					return VqEePGSMyrGKIqkWibsjeHcWPSIx;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return VqEePGSMyrGKIqkWibsjeHcWPSIx;
				}
			}

			[DebuggerHidden]
			public YKoEIaasnOPnuZcGBXgOnNhocKZB(int P_0)
			{
				RxAoyfYzYDsYonLGXsvUgwChukLk = P_0;
				wLBvvzlDcbAEPyDfgnprolEZBsgN = Thread.CurrentThread.ManagedThreadId;
			}

			[DebuggerHidden]
			void IDisposable.Dispose()
			{
				int rxAoyfYzYDsYonLGXsvUgwChukLk = RxAoyfYzYDsYonLGXsvUgwChukLk;
				if (rxAoyfYzYDsYonLGXsvUgwChukLk == -3 || rxAoyfYzYDsYonLGXsvUgwChukLk == 1)
				{
					try
					{
					}
					finally
					{
						uvbwjgAeXsWySvtbvsLpuFVuAgJB();
					}
				}
			}

			private bool MoveNext()
			{
				try
				{
					int rxAoyfYzYDsYonLGXsvUgwChukLk = RxAoyfYzYDsYonLGXsvUgwChukLk;
					ActionCategoryMap ttytLoUfsgUyhsklaKccrnoMiiek = TtytLoUfsgUyhsklaKccrnoMiiek;
					switch (rxAoyfYzYDsYonLGXsvUgwChukLk)
					{
					default:
						return false;
					case 0:
					{
						RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
						if (ttytLoUfsgUyhsklaKccrnoMiiek.list == null)
						{
							return false;
						}
						int num = ttytLoUfsgUyhsklaKccrnoMiiek.IndexOfCategory(IHGOXTDxkGUFIdzxXaNUTxjYneo);
						if (num < 0)
						{
							return false;
						}
						hSeONskmQJhVsjVUHGUASYbDGJrU = ttytLoUfsgUyhsklaKccrnoMiiek.list[num].ActionIds.GetEnumerator();
						RxAoyfYzYDsYonLGXsvUgwChukLk = -3;
						break;
					}
					case 1:
						RxAoyfYzYDsYonLGXsvUgwChukLk = -3;
						break;
					}
					if (hSeONskmQJhVsjVUHGUASYbDGJrU.MoveNext())
					{
						int current = hSeONskmQJhVsjVUHGUASYbDGJrU.Current;
						VqEePGSMyrGKIqkWibsjeHcWPSIx = current;
						RxAoyfYzYDsYonLGXsvUgwChukLk = 1;
						return true;
					}
					uvbwjgAeXsWySvtbvsLpuFVuAgJB();
					hSeONskmQJhVsjVUHGUASYbDGJrU = null;
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

			private void uvbwjgAeXsWySvtbvsLpuFVuAgJB()
			{
				RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
				if (hSeONskmQJhVsjVUHGUASYbDGJrU != null)
				{
					hSeONskmQJhVsjVUHGUASYbDGJrU.Dispose();
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
				YKoEIaasnOPnuZcGBXgOnNhocKZB yKoEIaasnOPnuZcGBXgOnNhocKZB;
				if (RxAoyfYzYDsYonLGXsvUgwChukLk == -2 && wLBvvzlDcbAEPyDfgnprolEZBsgN == Thread.CurrentThread.ManagedThreadId)
				{
					RxAoyfYzYDsYonLGXsvUgwChukLk = 0;
					yKoEIaasnOPnuZcGBXgOnNhocKZB = this;
				}
				else
				{
					yKoEIaasnOPnuZcGBXgOnNhocKZB = new YKoEIaasnOPnuZcGBXgOnNhocKZB(0);
					yKoEIaasnOPnuZcGBXgOnNhocKZB.TtytLoUfsgUyhsklaKccrnoMiiek = TtytLoUfsgUyhsklaKccrnoMiiek;
				}
				yKoEIaasnOPnuZcGBXgOnNhocKZB.IHGOXTDxkGUFIdzxXaNUTxjYneo = lIKkctcWVTwoXwHsmpFUIPKIHahI;
				return yKoEIaasnOPnuZcGBXgOnNhocKZB;
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

		public IEnumerable<int> ActionIdsInCategory(int categoryId)
		{
			return new YKoEIaasnOPnuZcGBXgOnNhocKZB(-2)
			{
				TtytLoUfsgUyhsklaKccrnoMiiek = this,
				lIKkctcWVTwoXwHsmpFUIPKIHahI = categoryId
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
