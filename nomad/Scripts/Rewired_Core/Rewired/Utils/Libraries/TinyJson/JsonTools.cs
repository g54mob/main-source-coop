namespace Rewired.Utils.Libraries.TinyJson
{
	public static class JsonTools
	{
		public static T Clone<T>(T obj) where T : class
		{
			if (obj == null)
			{
				return null;
			}
			return JsonParser.FromJson<T>(JsonWriter.ToJson(obj));
		}
	}
}
