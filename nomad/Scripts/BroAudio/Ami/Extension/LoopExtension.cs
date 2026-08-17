using System;
using UnityEngine;

namespace Ami.Extension
{
	public static class LoopExtension
	{
		public enum Statement
		{
			Continue = 0,
			Break = 1
		}

		public const int MaxIterationTimes = 10000;

		public static void Loop(Func<Statement> method, bool showErrorWhenInfiniteLoopOccurs = true)
		{
			MainLoopLogic((object obj) => true, method, showErrorWhenInfiniteLoopOccurs);
		}

		public static void While(Predicate<object> predicate, Func<Statement> method, bool showErrorWhenInfiniteLoopOccurs = true)
		{
			MainLoopLogic(predicate, method, showErrorWhenInfiniteLoopOccurs);
		}

		private static void MainLoopLogic(Predicate<object> predicate, Func<Statement> method, bool showErrorWhenInfiniteLoopOccurs)
		{
			if (method == null)
			{
				Debug.LogError("Method is null!");
				return;
			}
			for (int i = 0; i < 10000; i++)
			{
				if (showErrorWhenInfiniteLoopOccurs && i == 9999)
				{
					Debug.LogError("There is an infinite loop!");
				}
				if (!predicate(null))
				{
					break;
				}
				Statement statement = method();
				if (statement != Statement.Continue && statement == Statement.Break)
				{
					break;
				}
			}
		}
	}
}
