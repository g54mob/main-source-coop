using System;
using UnityEngine;

[Serializable]
public class EnviroModule : ScriptableObject
{
	public bool showModuleInspector;

	public bool showSaveLoad;

	public bool active = true;

	public virtual void Enable()
	{
	}

	public virtual void Disable()
	{
	}

	public virtual void UpdateModule()
	{
	}
}
