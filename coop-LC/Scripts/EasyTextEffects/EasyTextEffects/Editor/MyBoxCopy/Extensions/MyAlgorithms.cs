using System;
using JetBrains.Annotations;

namespace EasyTextEffects.Editor.MyBoxCopy.Extensions
{
	[PublicAPI]
	public static class MyAlgorithms
	{
		public static T Cast<T>(this IConvertible source)
		{
			return (T)Convert.ChangeType(source, typeof(T));
		}

		public static bool Is<T>(this object source)
		{
			return source is T;
		}

		public static T As<T>(this object source) where T : class
		{
			return source as T;
		}

		public static T Pipe<T>(this T argument, Action<T> action)
		{
			action(argument);
			return argument;
		}

		public static TResult Pipe<T, TResult>(this T argument, Func<T, TResult> function)
		{
			return function(argument);
		}

		public static T PipeKeep<T, TResult>(this T argument, Func<T, TResult> function)
		{
			function(argument);
			return argument;
		}
	}
}
