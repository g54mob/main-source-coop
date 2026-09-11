using System;
using UnityEngine;

public static class CommandLineArgumentReader
{
	public static void Init()
	{
		string[] commandLineArgs = Environment.GetCommandLineArgs();
		foreach (string text in commandLineArgs)
		{
			Debug.Log("Cmd argument: " + text);
		}
	}
}
