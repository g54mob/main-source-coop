using System;
using System.Threading;
using GameAnalyticsSDK.Net.Logging;

namespace GameAnalyticsSDK.Net.Threading
{
	public class GAThreading
	{
		private static bool endThread = false;

		private static DateTime threadDeadline;

		private static readonly GAThreading _instance = new GAThreading();

		private const int ThreadWaitTimeInMs = 1000;

		private readonly PriorityQueue<long, TimedBlock> blocks = new PriorityQueue<long, TimedBlock>();

		private readonly object threadLock = new object();

		private TimedBlock scheduledBlock;

		private bool hasScheduledBlockRun;

		private Thread thread;

		private static GAThreading Instance => _instance;

		private GAThreading()
		{
			threadDeadline = DateTime.Now;
			hasScheduledBlockRun = true;
		}

		~GAThreading()
		{
			StopThread();
		}

		private static void RunBlocks()
		{
			TimedBlock nextBlock;
			while ((nextBlock = GetNextBlock()) != null)
			{
				nextBlock.block();
			}
			if ((nextBlock = GetScheduledBlock()) != null)
			{
				nextBlock.block();
			}
		}

		public static void Run()
		{
			GALogger.D("Starting GA thread");
			try
			{
				while (!endThread && threadDeadline.CompareTo(DateTime.Now) > 0)
				{
					RunBlocks();
					Thread.Sleep(1000);
				}
				RunBlocks();
				if (!endThread)
				{
					GALogger.D("Ending GA thread");
				}
			}
			catch (Exception)
			{
			}
		}

		public static void PerformTaskOnGAThread(string blockName, Action taskBlock)
		{
			PerformTaskOnGAThread(blockName, taskBlock, 0L);
		}

		public static void PerformTaskOnGAThread(string blockName, Action taskBlock, long delayInSeconds)
		{
			if (endThread)
			{
				return;
			}
			lock (Instance.threadLock)
			{
				DateTime deadline = DateTime.Now.AddSeconds(delayInSeconds);
				TimedBlock timedBlock = new TimedBlock(deadline, taskBlock, blockName);
				Instance.AddTimedBlock(timedBlock);
				threadDeadline = deadline.AddSeconds(10.0);
				if (IsThreadFinished())
				{
					if (Instance.thread != null)
					{
						Instance.thread.Join();
					}
					StartThread();
				}
			}
		}

		public static void ScheduleTimer(double interval, string blockName, Action callback)
		{
			if (endThread)
			{
				return;
			}
			lock (Instance.threadLock)
			{
				if (!Instance.hasScheduledBlockRun)
				{
					return;
				}
				DateTime deadline = DateTime.Now.AddSeconds(interval);
				Instance.scheduledBlock = new TimedBlock(deadline, callback, blockName);
				Instance.hasScheduledBlockRun = false;
				threadDeadline = deadline.AddSeconds(2.0);
				if (IsThreadFinished())
				{
					if (Instance.thread != null)
					{
						Instance.thread.Join();
					}
					StartThread();
				}
			}
		}

		private void AddTimedBlock(TimedBlock timedBlock)
		{
			PriorityQueue<long, TimedBlock> priorityQueue = blocks;
			DateTime deadline = timedBlock.deadline;
			priorityQueue.Enqueue(deadline.Ticks, timedBlock);
		}

		private static TimedBlock GetNextBlock()
		{
			lock (Instance.threadLock)
			{
				DateTime now = DateTime.Now;
				if (Instance.blocks.HasItems)
				{
					DateTime deadline = Instance.blocks.Peek().deadline;
					if (deadline.CompareTo(now) <= 0)
					{
						return Instance.blocks.Dequeue();
					}
				}
				return null;
			}
		}

		private static TimedBlock GetScheduledBlock()
		{
			lock (Instance.threadLock)
			{
				DateTime now = DateTime.Now;
				if (!Instance.hasScheduledBlockRun && Instance.scheduledBlock != null)
				{
					DateTime deadline = Instance.scheduledBlock.deadline;
					if (deadline.CompareTo(now) <= 0)
					{
						Instance.hasScheduledBlockRun = true;
						return Instance.scheduledBlock;
					}
				}
				return null;
			}
		}

		public static void StartThread()
		{
			Instance.thread = new Thread(Run);
			Instance.thread.Priority = ThreadPriority.Lowest;
			Instance.thread.Start();
		}

		public static void StopThread()
		{
			endThread = true;
		}

		public static bool IsThreadFinished()
		{
			if (Instance.thread != null)
			{
				return !Instance.thread.IsAlive;
			}
			return true;
		}
	}
}
