using UnityEngine;

namespace Features.NetworkedModelCodegen.Scripts
{
	public abstract class NetworkedSignalBase
	{
		private string _label;

		protected void RetainLabel(string label)
		{
			if (label != null)
			{
				_label = label;
			}
		}

		protected void ReportDetachedRaise()
		{
			Debug.LogError("[NetworkedModel] " + (_label ?? "NetworkedSignal") + " was raised on a detached model; the signal was dropped (raise only while the model is attached).");
		}
	}
}
