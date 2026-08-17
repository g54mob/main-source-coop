using System;
using UnityEngine;

namespace VContainer.Unity
{
	internal static class AwaitableExtensions
	{
		public static async Awaitable Forget(this Awaitable awaitable, EntryPointExceptionHandler exceptionHandler = null)
		{
			try
			{
				await awaitable;
			}
			catch (Exception ex)
			{
				if (exceptionHandler != null)
				{
					exceptionHandler.Publish(ex);
					return;
				}
				throw;
			}
		}
	}
}
