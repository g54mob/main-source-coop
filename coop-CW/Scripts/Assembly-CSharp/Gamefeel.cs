using System;
using UnityEngine;

[Serializable]
public abstract class Gamefeel
{
	public float range;

	public abstract void Apply(Vector3 position = default(Vector3), Vector3 direction = default(Vector3), float multiplier = 1f);
}
