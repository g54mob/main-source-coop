using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace Den.Tools.Tasks
{
	public static class CoroutineManager
	{
		public class Task
		{
			public string name;

			public int priority;

			public List<Action> actions;

			public int actionNum;

			public IEnumerator routine;

			public bool Enqueued => queue.Contains(this);

			public bool Active => active == this;

			public void Add(Action action)
			{
				if (actions == null)
				{
					actions = new List<Action>();
				}
				actions.Add(action);
			}

			public void Start()
			{
				Enqueue(this);
			}

			public void Stop()
			{
				CoroutineManager.Stop(this);
			}
		}

		private static List<Task> queue;

		private static Task active;

		public static float timePerFrame;

		private static Stopwatch timer;

		public static long updateNum;

		public static bool IsWorking
		{
			get
			{
				if (queue.Count == 0)
				{
					return active != null;
				}
				return true;
			}
		}

		public static bool IsQueueEmpty => queue.Count == 0;

		static CoroutineManager()
		{
			queue = new List<Task>();
			active = null;
			timePerFrame = 3f;
			timer = new Stopwatch();
			updateNum = 0L;
		}

		public static Task Enqueue(Action action, int priority = 0, string name = null)
		{
			Task obj = new Task
			{
				actions = new List<Action> { action },
				priority = priority,
				name = name
			};
			Enqueue(obj);
			return obj;
		}

		public static Task Enqueue(IEnumerator routine, int priority = 0, string name = null)
		{
			Task obj = new Task
			{
				routine = routine,
				priority = priority,
				name = name
			};
			Enqueue(obj);
			return obj;
		}

		public static void Enqueue(Task task)
		{
			lock (queue)
			{
				if (!queue.Contains(task))
				{
					if (active == task)
					{
						active.routine = null;
						active = null;
					}
					queue.Add(task);
				}
			}
		}

		public static void Dequeue(Task task)
		{
			lock (queue)
			{
				if (queue.Contains(task))
				{
					queue.Remove(task);
				}
			}
		}

		public static void Stop(Task task)
		{
			lock (queue)
			{
				if (queue.Contains(task))
				{
					queue.Remove(task);
				}
			}
			if (active == task)
			{
				active = null;
			}
		}

		public static void Update()
		{
			updateNum++;
			timer.Reset();
			while ((float)timer.ElapsedMilliseconds < timePerFrame || !timer.IsRunning)
			{
				if (!timer.IsRunning)
				{
					timer.Start();
				}
				if (active == null)
				{
					lock (queue)
					{
						if (queue.Count == 0)
						{
							break;
						}
						int maxPriorityNum = GetMaxPriorityNum(queue);
						active = queue[maxPriorityNum];
						queue.RemoveAt(maxPriorityNum);
						goto IL_008c;
					}
				}
				goto IL_008c;
				IL_008c:
				bool flag = false;
				if (active.actions != null && active.actions.Count != 0 && active.actionNum < active.actions.Count)
				{
					try
					{
						active.actions[active.actionNum]();
					}
					catch (Exception ex)
					{
						throw new Exception("Routine error: " + ex);
					}
					finally
					{
						active.actionNum++;
						flag = true;
					}
				}
				if (!flag && active.routine != null)
				{
					flag = active.routine.MoveNext();
				}
				if (!flag)
				{
					if (active != null)
					{
						active.routine = null;
					}
					active = null;
				}
			}
			timer.Stop();
		}

		public static int GetMaxPriorityNum(List<Task> list)
		{
			int num = -2147483648;
			int result = -1;
			_ = list.Count;
			for (int num2 = list.Count - 1; num2 >= 0; num2--)
			{
				int priority = list[num2].priority;
				if (priority > num)
				{
					num = priority;
					result = num2;
				}
			}
			return result;
		}

		public static void Abort()
		{
			lock (queue)
			{
				queue.Clear();
			}
			if (active == null)
			{
				return;
			}
			try
			{
				if (active.routine != null)
				{
					active.routine.Reset();
				}
			}
			catch (Exception)
			{
			}
			active.routine = null;
		}

		private static bool AbortOnExit()
		{
			Abort();
			return true;
		}

		public static bool IsNameEnqueued(string name)
		{
			lock (queue)
			{
				if (queue.FindIndex((Task c) => c.name == name) >= 0)
				{
					return true;
				}
			}
			return false;
		}

		public static bool IsNameActive(string name)
		{
			if (active != null && active.name == name)
			{
				return true;
			}
			return false;
		}

		public static string DebugState()
		{
			string text = "";
			lock (queue)
			{
				text = queue.ToStringMemberwise((Task t) => t.name);
			}
			return "Coroutine active: " + ((active == null) ? "null" : active.name) + "\nCoroutine queue: " + text;
		}
	}
}
