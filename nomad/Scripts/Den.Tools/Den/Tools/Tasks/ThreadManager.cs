using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Den.Tools.Tasks
{
	public static class ThreadManager
	{
		public class Task
		{
			public string name;

			public int priority;

			public Action action;

			public Thread thread;

			public bool wasActiveBeforePause;

			public bool Enqueued => queue.Contains(this);

			public bool Active
			{
				get
				{
					if (!active.Contains(this))
					{
						return paused.Contains(this);
					}
					return true;
				}
			}

			public bool IsAlive
			{
				get
				{
					if (!queue.Contains(this) && !active.Contains(this))
					{
						return paused.Contains(this);
					}
					return true;
				}
			}
		}

		private static List<Task> queue;

		private static List<Task> active;

		private static List<Task> paused;

		public static int maxThreads;

		public static int processorThreads;

		public static bool autoMaxThreads;

		public static bool useMultithreading;

		public static int Count => queue.Count + active.Count;

		public static bool IsWorking
		{
			get
			{
				if (queue.Count == 0)
				{
					return active.Count != 0;
				}
				return true;
			}
		}

		static ThreadManager()
		{
			queue = new List<Task>();
			active = new List<Task>();
			paused = new List<Task>();
			maxThreads = 3;
			processorThreads = -1;
			autoMaxThreads = true;
			useMultithreading = true;
		}

		public static Task Enqueue(Action action, int priority = 0, string name = null)
		{
			Task obj = new Task
			{
				action = action,
				priority = priority,
				name = name
			};
			Enqueue(obj);
			return obj;
		}

		public static void Enqueue(ref Task task, Action action, int priority = 0, string name = null)
		{
			task = new Task
			{
				action = action,
				priority = priority,
				name = name
			};
			Enqueue(task);
		}

		public static void Enqueue(Task task)
		{
			lock (active)
			{
				if (active.Contains(task))
				{
					return;
				}
			}
			lock (queue)
			{
				if (queue.Contains(task))
				{
					return;
				}
				queue.Add(task);
			}
			LaunchThreads();
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

		public static void Pause(Task task)
		{
			lock (paused)
			{
				if (paused.Contains(task))
				{
					return;
				}
				paused.Add(task);
				lock (active)
				{
					if (active.Contains(task))
					{
						active.Remove(task);
						task.wasActiveBeforePause = true;
					}
				}
				lock (queue)
				{
					if (queue.Contains(task))
					{
						queue.Remove(task);
						task.wasActiveBeforePause = false;
					}
				}
			}
		}

		public static void Resume(Task task)
		{
			lock (paused)
			{
				paused.Remove(task);
				if (task.wasActiveBeforePause)
				{
					lock (active)
					{
						active.Add(task);
						return;
					}
				}
				lock (queue)
				{
					queue.Add(task);
				}
			}
		}

		public static void LaunchThreads()
		{
			lock (active)
			{
				while (true)
				{
					int num = maxThreads;
					if (autoMaxThreads)
					{
						if (processorThreads < 0)
						{
							processorThreads = SystemInfo.processorCount;
						}
						num = processorThreads - 1;
					}
					if (active.Count >= num)
					{
						break;
					}
					Task task;
					lock (queue)
					{
						if (queue.Count == 0)
						{
							break;
						}
						int maxPriorityNum = GetMaxPriorityNum(queue);
						task = queue[maxPriorityNum];
						queue.RemoveAt(maxPriorityNum);
					}
					active.Add(task);
					if (useMultithreading)
					{
						Thread thread = new Thread(task.TaskThreadAction);
						lock (task)
						{
							task.thread = thread;
						}
						thread.Start();
					}
					else
					{
						task.TaskThreadAction();
					}
				}
			}
		}

		public static void TaskThreadAction(this Task task)
		{
			try
			{
				task.action();
			}
			catch (ThreadAbortException)
			{
			}
			catch (Exception ex2)
			{
				Debug.LogError("Thread failed: " + ex2);
			}
			finally
			{
				lock (active)
				{
					active.Remove(task);
				}
				LaunchThreads();
			}
		}

		public static int GetMaxPriorityNum(List<Task> list)
		{
			int num = -2147483648;
			int result = -1;
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
			List<Task> list;
			lock (active)
			{
				list = new List<Task>(active);
				active.Clear();
			}
			for (int i = 0; i < list.Count; i++)
			{
				Task task = list[i];
				lock (task)
				{
					if (task.thread != null)
					{
						task.thread.Abort();
					}
				}
			}
		}

		private static bool AbortOnExit()
		{
			Abort();
			return true;
		}

		public static Task GetCurrentTask()
		{
			Thread thread = Thread.CurrentThread;
			Task task = null;
			task = active.Find(IsCurrent);
			if (task == null)
			{
				task = queue.Find(IsCurrent);
			}
			if (task == null)
			{
				task = paused.Find(IsCurrent);
			}
			return task;
			bool IsCurrent(Task task2)
			{
				return task2.thread == thread;
			}
		}

		public static string DebugState()
		{
			string text;
			lock (active)
			{
				text = active.ToStringMemberwise((Task t) => t.name);
			}
			string text2;
			lock (queue)
			{
				text2 = queue.ToStringMemberwise((Task t) => t.name);
			}
			return "Thread active: " + text + "\nThread queue: " + text2;
		}
	}
}
