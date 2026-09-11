using System;

namespace Zorro.Settings
{
	public interface ISettingsSaveLoad
	{
		bool TryLoadInt(Type type, out int o);

		void SaveInt(Type type, int value);

		void WriteToDisk();

		bool TryLoadFloat(Type type, out float value);

		void SaveFloat(Type type, float value);

		void SaveString(Type type, string value);

		bool TryGetString(Type type, out string value);

		bool TryLoadBool(Type type, out bool o);

		void SaveBool(Type type, bool value);

		bool TryLoadEnum<T>(Type type, out T value) where T : unmanaged, Enum;

		void SaveEnum<T>(Type type, T value) where T : unmanaged, Enum;
	}
}
