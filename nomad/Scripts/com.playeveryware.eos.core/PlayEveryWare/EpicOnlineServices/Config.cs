using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PlayEveryWare.Common;
using PlayEveryWare.EpicOnlineServices.Utility;
using UnityEngine;

namespace PlayEveryWare.EpicOnlineServices
{
	public abstract class Config
	{
		private class MemberInfo
		{
			public Type MemberType;

			public object MemberValue;

			public bool Equals(MemberInfo a, MemberInfo b)
			{
				if (a.MemberType != b.MemberType)
				{
					return false;
				}
				if (a.MemberValue == null && b.MemberValue == null)
				{
					return true;
				}
				if (a.MemberType.IsValueType)
				{
					return a.MemberValue == b.MemberValue;
				}
				if (a.MemberType == typeof(List<string>))
				{
					if (a.MemberValue != null || ((List<string>)b.MemberValue).Count != 0)
					{
						if (((List<string>)a.MemberValue).Count == 0)
						{
							return b.MemberValue == null;
						}
						return false;
					}
					return true;
				}
				if (a.MemberType == typeof(string))
				{
					if (string.IsNullOrEmpty(b.MemberValue as string))
					{
						return string.IsNullOrEmpty(a.MemberValue as string);
					}
					return false;
				}
				return a.MemberValue == b.MemberValue;
			}

			public int GetHashCode(MemberInfo memberInfo)
			{
				return HashCode.Combine(memberInfo.MemberType, memberInfo.MemberValue);
			}
		}

		protected static IDictionary<Type, Config> s_cachedConfigs = new Dictionary<Type, Config>();

		private static Dictionary<Type, Func<Config>> s_factories = new Dictionary<Type, Func<Config>>();

		protected readonly string Filename;

		protected readonly string Directory;

		private string _lastReadJsonString;

		private readonly bool _allowDefaultIfFileNotFound;

		private static readonly Version CURRENT_SCHEMA_VERSION = new Version(1, 0);

		[JsonProperty]
		private Version schemaVersion;

		[JsonIgnore]
		public string FilePath => FileSystemUtility.CombinePaths(Directory, Filename);

		protected Config(string filename, bool allowDefault = false)
			: this(filename, FileSystemUtility.CombinePaths(Application.streamingAssetsPath, "EOS"), allowDefault)
		{
		}

		protected Config(string filename, string directory, bool allowDefault = false)
		{
			Filename = filename;
			Directory = directory;
			_allowDefaultIfFileNotFound = allowDefault;
		}

		private async Task MigrateConfigIfNeededAsync()
		{
			MigrateConfigIfNeededInternal();
		}

		private void MigrateConfigIfNeeded()
		{
			MigrateConfigIfNeededInternal();
		}

		private void MigrateConfigIfNeededInternal()
		{
			if (NeedsMigration())
			{
				MigrateConfig();
			}
		}

		protected virtual bool NeedsMigration()
		{
			if (schemaVersion == null)
			{
				return true;
			}
			if (VersionUtility.AreVersionsEqual(schemaVersion, CURRENT_SCHEMA_VERSION))
			{
				return false;
			}
			Debug.LogWarning($"Config file with schemaVersion \"{CURRENT_SCHEMA_VERSION}\"" + " has been read into memory, and needs to be migrated to " + $"schemaVersion \"{CURRENT_SCHEMA_VERSION}\".");
			return true;
		}

		protected virtual void MigrateConfig()
		{
		}

		protected static void RegisterFactory<T>(Func<T> factory) where T : Config
		{
			s_factories[typeof(T)] = factory;
		}

		private static bool TryGetFactory<T>(out Func<Config> factory) where T : Config
		{
			RuntimeHelpers.RunClassConstructor(typeof(T).TypeHandle);
			if (!s_factories.TryGetValue(typeof(T), out factory))
			{
				throw new InvalidOperationException("No factory method has been registered for type \"" + typeof(T).FullName + "\". Please make sure that \"" + typeof(T).FullName + "\" registers its constructor with the base Config class via a static constructor.");
			}
			return true;
		}

		public static async Task<T> GetAsync<T>() where T : Config
		{
			if (s_cachedConfigs.TryGetValue(typeof(T), out var value))
			{
				return (T)value;
			}
			TryGetFactory<T>(out var factory);
			T instance = (T)factory();
			await instance.ReadAsync();
			s_cachedConfigs.Add(typeof(T), instance);
			await instance.MigrateConfigIfNeededAsync();
			return instance;
		}

		public static T Get<T>() where T : Config
		{
			if (s_cachedConfigs.TryGetValue(typeof(T), out var value))
			{
				return (T)value;
			}
			TryGetFactory<T>(out var factory);
			T val = (T)factory();
			val.Read();
			s_cachedConfigs.Add(typeof(T), val);
			val.MigrateConfigIfNeeded();
			return val;
		}

		protected virtual async Task ReadAsync()
		{
			await EnsureConfigFileExistsAsync();
			if (await FileSystemUtility.FileExistsAsync(FilePath))
			{
				_lastReadJsonString = await FileSystemUtility.ReadAllTextAsync(FilePath);
				JsonUtility.FromJsonOverwrite(_lastReadJsonString, this);
				OnReadCompleted();
			}
		}

		protected virtual void Read()
		{
			if (FileSystemUtility.FileExists(FilePath))
			{
				_lastReadJsonString = FileSystemUtility.ReadAllText(FilePath);
				JsonUtility.FromJsonOverwrite(_lastReadJsonString, this);
				OnReadCompleted();
			}
		}

		protected virtual void OnReadCompleted()
		{
		}

		protected virtual async Task EnsureConfigFileExistsAsync()
		{
			if (!(await FileSystemUtility.FileExistsAsync(FilePath)) && !_allowDefaultIfFileNotFound)
			{
				throw new FileNotFoundException("Config file \"" + FilePath + "\" does not exist.");
			}
		}

		public bool IsDefault()
		{
			return IsDefault(this);
		}

		public object Clone()
		{
			return MemberwiseClone();
		}

		private static bool IsDefault<T>(T configInstance) where T : Config
		{
			return IteratePropertiesAndFields(configInstance).All((MemberInfo mInfo) => GetDefaultValue(mInfo.MemberType) == mInfo.MemberValue);
		}

		private static object GetDefaultValue(Type type)
		{
			if (type.IsValueType)
			{
				return Activator.CreateInstance(type);
			}
			if (type == typeof(List<string>))
			{
				return new List<string>();
			}
			if (type == typeof(string))
			{
				return "";
			}
			return null;
		}

		public override bool Equals(object obj)
		{
			if (obj == null || GetType() != obj.GetType())
			{
				return false;
			}
			Config instance = obj as Config;
			IEnumerable<MemberInfo> first = IteratePropertiesAndFields(this);
			IEnumerable<MemberInfo> second = IteratePropertiesAndFields(instance);
			return first.SequenceEqual(second);
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(IteratePropertiesAndFields(this));
		}

		public static bool operator ==(Config left, Config right)
		{
			if ((object)left == right)
			{
				return true;
			}
			if ((object)left == null || (object)right == null)
			{
				return false;
			}
			return left.Equals(right);
		}

		public static bool operator !=(Config left, Config right)
		{
			return !(left == right);
		}

		private static IEnumerable<MemberInfo> IteratePropertiesAndFields<T>(T instance, BindingFlags bindingAttr = BindingFlags.Instance | BindingFlags.Public)
		{
			PropertyInfo[] properties = typeof(T).GetProperties(bindingAttr);
			foreach (PropertyInfo propertyInfo in properties)
			{
				yield return new MemberInfo
				{
					MemberType = propertyInfo.PropertyType,
					MemberValue = propertyInfo.GetValue(instance)
				};
			}
			FieldInfo[] fields = typeof(T).GetFields(bindingAttr);
			foreach (FieldInfo fieldInfo in fields)
			{
				yield return new MemberInfo
				{
					MemberType = fieldInfo.FieldType,
					MemberValue = fieldInfo.GetValue(instance)
				};
			}
		}
	}
}
