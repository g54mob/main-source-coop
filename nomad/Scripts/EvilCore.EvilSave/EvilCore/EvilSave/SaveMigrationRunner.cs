using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace EvilCore.EvilSave
{
	public static class SaveMigrationRunner
	{
		private static List<ISaveMigration> _migrations;

		public static void Run(SaveData data, int targetVersion)
		{
			if (data.Version >= targetVersion)
			{
				return;
			}
			foreach (ISaveMigration item in (from m in GetMigrations()
				where m.FromVersion >= data.Version && m.ToVersion <= targetVersion
				orderby m.FromVersion
				select m).ToList())
			{
				if (item.FromVersion == data.Version)
				{
					item.Migrate(data);
					data.Version = item.ToVersion;
				}
			}
			_ = data.Version;
			_ = targetVersion;
		}

		private static List<ISaveMigration> GetMigrations()
		{
			if (_migrations != null)
			{
				return _migrations;
			}
			_migrations = new List<ISaveMigration>();
			Type typeFromHandle = typeof(ISaveMigration);
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			foreach (Assembly assembly in assemblies)
			{
				try
				{
					Type[] types = assembly.GetTypes();
					foreach (Type type in types)
					{
						if (!type.IsAbstract && !type.IsInterface && typeFromHandle.IsAssignableFrom(type))
						{
							ISaveMigration item = (ISaveMigration)Activator.CreateInstance(type);
							_migrations.Add(item);
						}
					}
				}
				catch
				{
				}
			}
			_migrations = _migrations.OrderBy((ISaveMigration m) => m.FromVersion).ToList();
			return _migrations;
		}

		internal static void ClearCache()
		{
			_migrations = null;
		}
	}
}
