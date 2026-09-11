using System;
using UnityEngine.Events;

[Serializable]
public class ChanceEvent
{
	public UnityEvent eventToCall;

	public float weight = 1f;
}
