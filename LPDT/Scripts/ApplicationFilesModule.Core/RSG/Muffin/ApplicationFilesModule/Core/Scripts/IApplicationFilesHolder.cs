using System;
using System.Collections.Generic;

namespace RSG.Muffin.ApplicationFilesModule.Core.Scripts
{
	public interface IApplicationFilesHolder
	{
		bool TryGetEnumByName(string enumName, out Type type);

		Type GetEnumByName(string enumName);

		bool TryGetTypeByName(string typeName, out Type type);

		Type GetTypeByName(string typeName);

		Type GetFirstOrDefaultType(Func<Type, bool> predicate);

		List<Type> GetAssignableFrom(Type type);
	}
}
