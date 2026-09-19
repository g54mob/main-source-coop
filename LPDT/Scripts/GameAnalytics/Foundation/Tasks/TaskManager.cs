using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Foundation.Tasks
{
	[AddComponentMenu("Foundation/TaskManager")]
	[ExecuteInEditMode]
	public class TaskManager : MonoBehaviour
	{
		public struct LogCommand
		{
			public LogType Type;

			public object Message;
		}

		public struct CoroutineCommand
		{
			public IEnumerator Coroutine;

			public Action OnComplete;
		}

		private static TaskManager _instance;

		private static object syncRoot = new object();

		protected static readonly List<CoroutineCommand> PendingCoroutineInfo = new List<CoroutineCommand>();

		protected static readonly List<IEnumerator> PendingAdd = new List<IEnumerator>();

		protected static readonly List<IEnumerator> PendingRemove = new List<IEnumerator>();

		protected static readonly List<Action> PendingActions = new List<Action>();

		protected static readonly List<LogCommand> PendingLogs = new List<LogCommand>();

		protected static bool IsApplicationQuit;

		public static TaskManager Instance
		{
			get
			{
				ConfirmInit();
				return _instance;
			}
		}

		public static bool IsMainThread => Thread.CurrentThread == MainThread;

		public static Thread MainThread { get; protected set; }

		public static Thread CurrentThread => Thread.CurrentThread;

		public static void ConfirmInit()
		{
			if (!(_instance == null))
			{
				return;
			}
			TaskManager[] array = UnityEngine.Object.FindObjectsOfType<TaskManager>();
			foreach (TaskManager taskManager in array)
			{
				if (Application.isEditor)
				{
					UnityEngine.Object.DestroyImmediate(taskManager.gameObject);
				}
				else
				{
					UnityEngine.Object.Destroy(taskManager.gameObject);
				}
			}
			GameObject obj = new GameObject("_TaskManager");
			UnityEngine.Object.DontDestroyOnLoad(obj);
			_instance = obj.AddComponent<TaskManager>();
			MainThread = CurrentThread;
		}

		public static Coroutine WaitForSeconds(int seconds)
		{
			return Instance.StartCoroutine(Instance.WaitForSecondsInternal(seconds));
		}

		public static Coroutine StartRoutine(IEnumerator coroutine)
		{
			if (IsApplicationQuit)
			{
				return null;
			}
			if (!IsMainThread)
			{
				lock (syncRoot)
				{
					PendingAdd.Add(coroutine);
					return null;
				}
			}
			return Instance.StartCoroutine(coroutine);
		}

		public static void StartRoutine(CoroutineCommand info)
		{
			if (IsApplicationQuit)
			{
				return;
			}
			if (!IsMainThread)
			{
				lock (syncRoot)
				{
					PendingCoroutineInfo.Add(info);
					return;
				}
			}
			Instance.StartCoroutine(Instance.RunCoroutineInfo(info));
		}

		public static void StopRoutine(IEnumerator coroutine)
		{
			if (IsApplicationQuit)
			{
				return;
			}
			if (!IsMainThread)
			{
				lock (syncRoot)
				{
					PendingRemove.Add(coroutine);
					return;
				}
			}
			Instance.StopCoroutine(coroutine);
		}

		public static void RunOnMainThread(Action action)
		{
			if (IsApplicationQuit)
			{
				return;
			}
			if (!IsMainThread)
			{
				lock (syncRoot)
				{
					PendingActions.Add(action);
					return;
				}
			}
			action();
		}

		public static void Log(LogCommand m)
		{
			if (!IsMainThread)
			{
				lock (syncRoot)
				{
					PendingLogs.Add(m);
					return;
				}
			}
			Write(m);
		}

		private static void Write(LogCommand m)
		{
			switch (m.Type)
			{
			case LogType.Warning:
				Debug.LogWarning(m.Message);
				break;
			case LogType.Error:
			case LogType.Exception:
				Debug.LogError(m.Message);
				break;
			case LogType.Assert:
			case LogType.Log:
				Debug.Log(m.Message);
				break;
			}
		}

		protected void Awake()
		{
			if (_instance == null)
			{
				_instance = this;
			}
		}

		protected void Update()
		{
			if (IsApplicationQuit || (PendingAdd.Count == 0 && PendingRemove.Count == 0 && PendingActions.Count == 0 && PendingLogs.Count == 0 && PendingCoroutineInfo.Count == 0))
			{
				return;
			}
			lock (syncRoot)
			{
				for (int i = 0; i < PendingLogs.Count; i++)
				{
					Write(PendingLogs[i]);
				}
				for (int j = 0; j < PendingAdd.Count; j++)
				{
					StartCoroutine(PendingAdd[j]);
				}
				for (int k = 0; k < PendingRemove.Count; k++)
				{
					StopCoroutine(PendingRemove[k]);
				}
				for (int l = 0; l < PendingCoroutineInfo.Count; l++)
				{
					StartCoroutine(RunCoroutineInfo(PendingCoroutineInfo[l]));
				}
				for (int m = 0; m < PendingActions.Count; m++)
				{
					PendingActions[m]();
				}
				PendingAdd.Clear();
				PendingRemove.Clear();
				PendingActions.Clear();
				PendingLogs.Clear();
				PendingCoroutineInfo.Clear();
			}
		}

		private IEnumerator RunCoroutineInfo(CoroutineCommand info)
		{
			yield return StartCoroutine(info.Coroutine);
			if (info.OnComplete != null)
			{
				info.OnComplete();
			}
		}

		protected void OnApplicationQuit()
		{
			IsApplicationQuit = true;
		}

		private IEnumerator WaitForSecondsInternal(int seconds)
		{
			if (seconds > 0)
			{
				float delta = 0f;
				while (delta < (float)seconds)
				{
					delta += Time.unscaledDeltaTime;
					yield return 1;
				}
			}
		}
	}
}
