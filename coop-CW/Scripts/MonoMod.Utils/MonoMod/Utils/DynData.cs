using System;
using System.Collections.Generic;
using System.Reflection;

namespace MonoMod.Utils
{
	public sealed class DynData<TTarget> : IDisposable where TTarget : class
	{
		private class _Data_ : IDisposable
		{
			public readonly Dictionary<string, Func<TTarget, object>> Getters = new Dictionary<string, Func<TTarget, object>>();

			public readonly Dictionary<string, Action<TTarget, object>> Setters = new Dictionary<string, Action<TTarget, object>>();

			public readonly Dictionary<string, object> Data = new Dictionary<string, object>();

			public readonly HashSet<string> Disposable = new HashSet<string>();

			~_Data_()
			{
				Dispose();
			}

			public void Dispose()
			{
				lock (Data)
				{
					if (Data.Count == 0)
					{
						return;
					}
					foreach (string item in Disposable)
					{
						if (Data.TryGetValue(item, out var value) && value is IDisposable disposable)
						{
							disposable.Dispose();
						}
					}
					Disposable.Clear();
					Data.Clear();
				}
			}
		}

		private static int CreationsInProgress;

		private static readonly object[] _NoArgs;

		private static readonly _Data_ _DataStatic;

		private static readonly Dictionary<WeakReference, _Data_> _DataMap;

		private static readonly HashSet<WeakReference> _DataMapDead;

		private static readonly Dictionary<string, Func<TTarget, object>> _SpecialGetters;

		private static readonly Dictionary<string, Action<TTarget, object>> _SpecialSetters;

		private readonly WeakReference Weak;

		private TTarget KeepAlive;

		private readonly _Data_ _Data;

		public Dictionary<string, Func<TTarget, object>> Getters => _Data.Getters;

		public Dictionary<string, Action<TTarget, object>> Setters => _Data.Setters;

		public Dictionary<string, object> Data => _Data.Data;

		public bool IsAlive
		{
			get
			{
				if (Weak != null)
				{
					return Weak.SafeGetIsAlive();
				}
				return true;
			}
		}

		public TTarget Target => Weak?.SafeGetTarget() as TTarget;

		public object this[string name]
		{
			get
			{
				if (_SpecialGetters.TryGetValue(name, out var value) || Getters.TryGetValue(name, out value))
				{
					return value(Weak?.SafeGetTarget() as TTarget);
				}
				if (Data.TryGetValue(name, out var value2))
				{
					return value2;
				}
				return null;
			}
			set
			{
				if (_SpecialSetters.TryGetValue(name, out var value2) || Setters.TryGetValue(name, out value2))
				{
					value2(Weak?.SafeGetTarget() as TTarget, value);
					return;
				}
				object obj;
				if (_Data.Disposable.Contains(name) && (obj = this[name]) != null && obj is IDisposable disposable)
				{
					disposable.Dispose();
				}
				Data[name] = value;
			}
		}

		public static event Action<DynData<TTarget>, TTarget> OnInitialize;

		static DynData()
		{
			CreationsInProgress = 0;
			_NoArgs = new object[0];
			_DataStatic = new _Data_();
			_DataMap = new Dictionary<WeakReference, _Data_>(new WeakReferenceComparer());
			_DataMapDead = new HashSet<WeakReference>();
			_SpecialGetters = new Dictionary<string, Func<TTarget, object>>();
			_SpecialSetters = new Dictionary<string, Action<TTarget, object>>();
			GCListener.OnCollect += delegate
			{
				if (CreationsInProgress != 0)
				{
					return;
				}
				lock (_DataMap)
				{
					foreach (KeyValuePair<WeakReference, _Data_> item in _DataMap)
					{
						if (!item.Key.SafeGetIsAlive())
						{
							_DataMapDead.Add(item.Key);
							item.Value.Dispose();
						}
					}
					foreach (WeakReference item2 in _DataMapDead)
					{
						_DataMap.Remove(item2);
					}
					_DataMapDead.Clear();
				}
			};
			FieldInfo[] fields = typeof(TTarget).GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			foreach (FieldInfo field in fields)
			{
				string name = field.Name;
				_SpecialGetters[name] = (TTarget obj) => field.GetValue(obj);
				_SpecialSetters[name] = delegate(TTarget obj, object value)
				{
					field.SetValue(obj, value);
				};
			}
			PropertyInfo[] properties = typeof(TTarget).GetProperties(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			foreach (PropertyInfo propertyInfo in properties)
			{
				string name2 = propertyInfo.Name;
				MethodInfo get = propertyInfo.GetGetMethod(nonPublic: true);
				if ((object)get != null)
				{
					_SpecialGetters[name2] = (TTarget obj) => get.Invoke(obj, _NoArgs);
				}
				MethodInfo set = propertyInfo.GetSetMethod(nonPublic: true);
				if ((object)set != null)
				{
					_SpecialSetters[name2] = delegate(TTarget obj, object value)
					{
						set.Invoke(obj, new object[1] { value });
					};
				}
			}
		}

		public DynData()
			: this((TTarget)null, false)
		{
		}

		public DynData(TTarget obj)
			: this(obj, true)
		{
		}

		public DynData(TTarget obj, bool keepAlive)
		{
			if (obj != null)
			{
				WeakReference weakReference = new WeakReference(obj);
				WeakReference key = weakReference;
				CreationsInProgress++;
				lock (_DataMap)
				{
					if (!_DataMap.TryGetValue(key, out _Data))
					{
						_Data = new _Data_();
						_DataMap.Add(key, _Data);
					}
				}
				CreationsInProgress--;
				Weak = weakReference;
				if (keepAlive)
				{
					KeepAlive = obj;
				}
			}
			else
			{
				_Data = _DataStatic;
			}
			DynData<TTarget>.OnInitialize?.Invoke(this, obj);
		}

		public T Get<T>(string name)
		{
			return (T)this[name];
		}

		public void Set<T>(string name, T value)
		{
			this[name] = value;
		}

		public void RegisterProperty(string name, Func<TTarget, object> getter, Action<TTarget, object> setter)
		{
			Getters[name] = getter;
			Setters[name] = setter;
		}

		public void UnregisterProperty(string name)
		{
			Getters.Remove(name);
			Setters.Remove(name);
		}

		private void Dispose(bool disposing)
		{
			KeepAlive = null;
		}

		~DynData()
		{
			Dispose(disposing: false);
		}

		public void Dispose()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}
	}
}
