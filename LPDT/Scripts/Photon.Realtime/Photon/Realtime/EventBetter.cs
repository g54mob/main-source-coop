#define SUPPORTED_UNITY
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace Photon.Realtime
{
	public class EventBetter
	{
		public class YieldListener<MessageType> : IEnumerator, IDisposable where MessageType : class
		{
			private Delegate handler;

			internal EventBetter EventBetterInstance;

			internal List<MessageType> Messages { get; private set; }

			internal MessageType First
			{
				get
				{
					if (Messages == null || Messages.Count == 0)
					{
						return null;
					}
					return Messages[0];
				}
			}

			object IEnumerator.Current => null;

			internal YieldListener()
			{
				handler = EventBetterInstance.RegisterInternal(this, delegate(MessageType msg)
				{
					OnMessage(msg);
				}, HandlerFlags.DontInvokeIfAddedInAHandler);
			}

			public void Dispose()
			{
				if ((object)handler != null)
				{
					EventBetterInstance.UnlistenHandler(typeof(MessageType), handler);
					handler = null;
				}
			}

			private void OnMessage(MessageType msg)
			{
				if (Messages == null)
				{
					Messages = new List<MessageType>();
				}
				Messages.Add(msg);
			}

			bool IEnumerator.MoveNext()
			{
				if (Messages != null)
				{
					Dispose();
					return false;
				}
				return true;
			}

			void IEnumerator.Reset()
			{
			}
		}

		private class ManualHandlerDisposable : IDisposable
		{
			private object disposeLock = new object();

			public Type MessageType { get; set; }

			public Delegate Handler { get; set; }

			public EventBetter EventBetterInstance { get; set; }

			public void Dispose()
			{
				lock (disposeLock)
				{
					if ((object)Handler == null)
					{
						return;
					}
					try
					{
						if (EventBetterInstance != null)
						{
							EventBetterInstance.UnlistenHandler(MessageType, Handler);
						}
					}
					finally
					{
						MessageType = null;
						Handler = null;
						EventBetterInstance = null;
					}
				}
			}
		}

		[Flags]
		private enum HandlerFlags
		{
			None = 0,
			OnlyIfActiveAndEnabled = 1,
			Once = 2,
			DontInvokeIfAddedInAHandler = 4,
			IsUnityObject = 8
		}

		private sealed class EventBetterWorker : MonoBehaviour
		{
			private int instanceId;

			public EventBetter EventBetterInstance;

			private void Awake()
			{
				instanceId = GetInstanceID();
			}

			private void LateUpdate()
			{
				EventBetterInstance.RemoveUnusedHandlers();
			}
		}

		private class EventEntry
		{
			public uint invocationCount;

			public bool needsCleanup;

			public readonly List<object> listeners = new List<object>();

			public readonly List<Delegate> handlers = new List<Delegate>();

			public readonly List<HandlerFlags> flags = new List<HandlerFlags>();

			public int Count => listeners.Count;

			public bool HasFlag(int i, HandlerFlags flag)
			{
				return (flags[i] & flag) == flag;
			}

			public void SetFlag(int i, HandlerFlags flag, bool value)
			{
				if (value)
				{
					flags[i] |= flag;
				}
				else
				{
					flags[i] &= ~flag;
				}
			}

			public void Add(object listener, Delegate handler, HandlerFlags flag)
			{
				UnityAssertListSize();
				if (invocationCount == 0)
				{
					flag &= ~HandlerFlags.DontInvokeIfAddedInAHandler;
				}
				listeners.Add(listener);
				handlers.Add(handler);
				flags.Add(flag);
			}

			public void NullifyAt(int i)
			{
				UnityAssertListSize();
				listeners[i] = null;
				handlers[i] = null;
				flags[i] = HandlerFlags.None;
			}

			public void RemoveAt(int i)
			{
				UnityAssertListSize();
				listeners.RemoveAt(i);
				handlers.RemoveAt(i);
				flags.RemoveAt(i);
			}

			[Conditional("SUPPORTED_UNITY")]
			private void UnityAssertListSize()
			{
			}
		}

		private ConcurrentDictionary<Type, EventEntry> s_entries = new ConcurrentDictionary<Type, EventEntry>();

		private List<EventEntry> s_entriesList = new List<EventEntry>();

		private EventBetterWorker s_worker;

		public void Listen<ListenerType, MessageType>(ListenerType listener, Action<MessageType> handler, bool once = false, bool exculdeInactive = false) where ListenerType : UnityEngine.Object
		{
			HandlerFlags handlerFlags = HandlerFlags.IsUnityObject;
			if (once)
			{
				handlerFlags |= HandlerFlags.Once;
			}
			if (exculdeInactive)
			{
				handlerFlags |= HandlerFlags.OnlyIfActiveAndEnabled;
			}
			RegisterInternal(listener, handler, handlerFlags);
		}

		public bool Unlisten<MessageType>(UnityEngine.Object listener)
		{
			if (listener == null)
			{
				throw new ArgumentNullException("listener");
			}
			return UnregisterInternal(typeof(MessageType), listener, (EventEntry eventEntry, int index, UnityEngine.Object referenceListener) => eventEntry.listeners[index] == referenceListener);
		}

		public bool UnlistenAll(UnityEngine.Object listener)
		{
			if (listener == null)
			{
				throw new ArgumentNullException("listener");
			}
			bool flag = false;
			foreach (EventEntry s_entries in s_entriesList)
			{
				flag |= UnregisterInternal(s_entries, listener, (EventEntry eventEntry, int index, UnityEngine.Object referenceListener) => eventEntry.listeners[index] == referenceListener);
			}
			return flag;
		}

		public IDisposable ListenManual<MessageType>(Action<MessageType> handler)
		{
			Delegate handler2 = RegisterInternal<object, MessageType>((object)s_entries, (Action<MessageType>)delegate(MessageType msg)
			{
				handler(msg);
			}, HandlerFlags.None);
			return new ManualHandlerDisposable
			{
				Handler = handler2,
				MessageType = typeof(MessageType),
				EventBetterInstance = this
			};
		}

		public bool Raise<MessageType>(MessageType message)
		{
			return RaiseInternal(message);
		}

		public void Clear()
		{
			s_entries.Clear();
			s_entriesList.Clear();
			if (s_worker != null)
			{
				UnityEngine.Object.Destroy(s_worker.gameObject);
				s_worker = null;
			}
		}

		public void RemoveUnusedHandlers()
		{
			foreach (EventEntry s_entries in s_entriesList)
			{
				RemoveUnusedHandlers(s_entries);
			}
		}

		public YieldListener<MessageType> ListenWait<MessageType>() where MessageType : class
		{
			return new YieldListener<MessageType>();
		}

		private bool RaiseInternal<T>(T message)
		{
			if (!s_entries.TryGetValue(typeof(T), out var value))
			{
				return false;
			}
			bool result = false;
			uint num = ++value.invocationCount;
			try
			{
				int num2 = value.Count;
				for (int i = 0; i < value.Count; i++)
				{
					object aliveTarget = GetAliveTarget(value.listeners[i]);
					bool flag = true;
					if (aliveTarget != null)
					{
						if (value.HasFlag(i, HandlerFlags.OnlyIfActiveAndEnabled) && (aliveTarget is Behaviour { isActiveAndEnabled: false } || aliveTarget is GameObject { activeInHierarchy: false }))
						{
							continue;
						}
						if (i >= num2 && value.HasFlag(i, HandlerFlags.DontInvokeIfAddedInAHandler))
						{
							value.SetFlag(i, HandlerFlags.DontInvokeIfAddedInAHandler, value: false);
							continue;
						}
						if (!value.HasFlag(i, HandlerFlags.Once))
						{
							flag = false;
						}
						((Action<T>)value.handlers[i])(message);
						result = true;
					}
					if (flag)
					{
						if (num == 1)
						{
							value.RemoveAt(i);
							i--;
							num2--;
						}
						else
						{
							value.needsCleanup = true;
							value.NullifyAt(i);
						}
					}
				}
				return result;
			}
			finally
			{
				value.invocationCount--;
				if (num == 1 && value.needsCleanup)
				{
					value.needsCleanup = false;
					RemoveUnusedHandlers(value);
				}
			}
		}

		private Delegate RegisterInternal<ListenerType, MessageType>(ListenerType listener, Action<MessageType> handler, HandlerFlags flags)
		{
			return RegisterInternal((object)listener, handler, flags);
		}

		private Delegate RegisterInternal<T>(object listener, Action<T> handler, HandlerFlags flags)
		{
			if (listener == null)
			{
				throw new ArgumentNullException("listener");
			}
			if (handler == null)
			{
				throw new ArgumentNullException("handler");
			}
			if ((flags & HandlerFlags.IsUnityObject) == HandlerFlags.IsUnityObject)
			{
				EnsureWorkerExistsAndIsActive();
			}
			if (!s_entries.TryGetValue(typeof(T), out var value))
			{
				value = new EventEntry();
				value = s_entries.GetOrAdd(typeof(T), value);
				s_entriesList.Add(value);
			}
			value.Add(listener, handler, flags);
			return handler;
		}

		private bool UnlistenHandler(Type messageType, Delegate handler)
		{
			return UnregisterInternal(messageType, handler, (EventEntry eventEntry, int index, Delegate _handler) => eventEntry.handlers[index] == _handler);
		}

		private bool UnregisterInternal<ParamType>(Type messageType, ParamType param, Func<EventEntry, int, ParamType, bool> predicate)
		{
			if (!s_entries.TryGetValue(messageType, out var value))
			{
				return false;
			}
			return UnregisterInternal(value, param, predicate);
		}

		private bool UnregisterInternal<ParamType>(EventEntry entry, ParamType param, Func<EventEntry, int, ParamType, bool> predicate)
		{
			bool result = false;
			for (int i = 0; i < entry.Count; i++)
			{
				if (entry.listeners[i] != null && (predicate == null || predicate(entry, i, param)))
				{
					result = true;
					if (entry.invocationCount == 0)
					{
						entry.RemoveAt(i);
						i--;
					}
					else
					{
						entry.needsCleanup = true;
						entry.NullifyAt(i);
					}
				}
			}
			return result;
		}

		private static object GetAliveTarget(object target)
		{
			if (target == null)
			{
				return null;
			}
			if (target is UnityEngine.Object obj && !obj)
			{
				return null;
			}
			return target;
		}

		private void RemoveUnusedHandlers(EventEntry entry)
		{
			for (int i = 0; i < entry.Count; i++)
			{
				object obj = entry.listeners[i];
				if (entry.HasFlag(i, HandlerFlags.IsUnityObject))
				{
					if ((UnityEngine.Object)obj != null)
					{
						continue;
					}
				}
				else if (obj != null)
				{
					continue;
				}
				if (entry.invocationCount == 0)
				{
					entry.RemoveAt(i--);
				}
				else
				{
					entry.NullifyAt(i);
				}
			}
		}

		private void EnsureWorkerExistsAndIsActive()
		{
			if (s_worker != null)
			{
				if (!s_worker.isActiveAndEnabled)
				{
					throw new InvalidOperationException("EventBetterWorker is disabled");
				}
				return;
			}
			GameObject gameObject = new GameObject("EventBetterWorker", typeof(EventBetterWorker));
			gameObject.hideFlags = HideFlags.HideAndDontSave;
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
			s_worker = gameObject.GetComponent<EventBetterWorker>();
			if (!s_worker)
			{
				throw new InvalidOperationException("Unable to create EventBetterWorker");
			}
			s_worker.EventBetterInstance = this;
		}
	}
}
