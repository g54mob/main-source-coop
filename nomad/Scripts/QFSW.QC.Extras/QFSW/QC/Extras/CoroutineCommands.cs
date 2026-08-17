using System;
using System.Collections;
using UnityEngine;

namespace QFSW.QC.Extras
{
	[AddComponentMenu("")]
	public class CoroutineCommands : MonoBehaviour
	{
		[Command("start-coroutine", "starts the supplied command as a coroutine", MonoTargetType.Singleton, Platform.AllPlatforms)]
		private void StartCoroutineCommand(string coroutineCommand)
		{
			object obj = QuantumConsoleProcessor.InvokeCommand(coroutineCommand);
			if (obj is IEnumerator)
			{
				StartCoroutine(obj as IEnumerator);
				return;
			}
			throw new ArgumentException(coroutineCommand + " is not a coroutine");
		}
	}
}
