using System;
using System.Collections.Generic;
using System.Reflection;
using EvilCore.EvilPack.EvilLogger;
using UnityEngine;

namespace EvilCore.DI.Core
{
	public static class InstallerAutoInjection
	{
		private static Dictionary<Type, Component> cachedComponents = new Dictionary<Type, Component>();

		public static void ClearCache()
		{
			cachedComponents.Clear();
		}

		public static int AutoInjectInstaller<T>(T installer) where T : MonoInstaller
		{
			if (installer == null)
			{
				EvilLogger.LogError("Cannot auto-inject null installer", "AutoInjectInstaller", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\_Core\\DI\\Core\\InstallerAutoInjection.cs", 34);
				return 0;
			}
			int num = 0;
			FieldInfo[] fields = installer.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			foreach (FieldInfo fieldInfo in fields)
			{
				if (!fieldInfo.FieldType.IsSubclassOf(typeof(Component)) || fieldInfo.GetValue(installer) != null)
				{
					continue;
				}
				Component value = null;
				if (!cachedComponents.TryGetValue(fieldInfo.FieldType, out value))
				{
					value = UnityEngine.Object.FindObjectOfType(fieldInfo.FieldType) as Component;
					if (value != null)
					{
						cachedComponents[fieldInfo.FieldType] = value;
					}
				}
				if (value != null)
				{
					fieldInfo.SetValue(installer, value);
					num++;
				}
			}
			return num;
		}

		public static bool InjectSpecificComponent<T, TComponent>(T installer, string fieldName, TComponent component) where T : MonoInstaller where TComponent : Component
		{
			if (installer == null)
			{
				EvilLogger.LogError("Cannot inject into null installer", "InjectSpecificComponent", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\_Core\\DI\\Core\\InstallerAutoInjection.cs", 95);
				return false;
			}
			if (component == null)
			{
				EvilLogger.LogError("Cannot inject null component into " + installer.GetType().Name + "." + fieldName, "InjectSpecificComponent", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\_Core\\DI\\Core\\InstallerAutoInjection.cs", 101);
				return false;
			}
			Type type = installer.GetType();
			FieldInfo field = type.GetField(fieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			if (field == null)
			{
				EvilLogger.LogError("Field '" + fieldName + "' not found in " + type.Name, "InjectSpecificComponent", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\_Core\\DI\\Core\\InstallerAutoInjection.cs", 110);
				return false;
			}
			if (!field.FieldType.IsAssignableFrom(typeof(TComponent)))
			{
				EvilLogger.LogError("Field '" + fieldName + "' in " + type.Name + " is of type " + field.FieldType.Name + ", cannot assign " + typeof(TComponent).Name, "InjectSpecificComponent", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\_Core\\DI\\Core\\InstallerAutoInjection.cs", 116);
				return false;
			}
			field.SetValue(installer, component);
			return true;
		}

		public static int AutoInjectAllInstallersInScene()
		{
			ClearCache();
			int num = 0;
			MonoInstaller[] array = UnityEngine.Object.FindObjectsOfType<MonoInstaller>();
			foreach (MonoInstaller installer in array)
			{
				num += AutoInjectInstaller(installer);
			}
			return num;
		}

		public static int AutoInjectInstallersInPrefab(GameObject prefab)
		{
			if (prefab == null)
			{
				EvilLogger.LogError("Cannot auto-inject null prefab", "AutoInjectInstallersInPrefab", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\_Core\\DI\\Core\\InstallerAutoInjection.cs", 155);
				return 0;
			}
			ClearCache();
			int num = 0;
			MonoInstaller[] componentsInChildren = prefab.GetComponentsInChildren<MonoInstaller>();
			foreach (MonoInstaller installer in componentsInChildren)
			{
				num += AutoInjectInstaller(installer);
			}
			return num;
		}
	}
}
