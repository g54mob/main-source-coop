using System.Linq;

namespace EvilCore.Extensions
{
	public static class ObjectExtensions
	{
		public static bool EqualsToAll(this object obj, params object[] objects)
		{
			return objects.All((object o) => o.Equals(obj));
		}

		public static bool EqualsToAny(this object obj, params object[] objects)
		{
			return objects.Any((object o) => o.Equals(obj));
		}
	}
}
