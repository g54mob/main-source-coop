using UnityEngine;
using UnityEngine.Events;

namespace PaintCore
{
	[HelpURL("https://carloswilkes.com/Documentation/PaintCore#CwActionOnEnable")]
	[AddComponentMenu("CW/Paint Core/CW Action OnEnable")]
	public class CwActionOnEnable : MonoBehaviour
	{
		[SerializeField]
		public UnityEvent action;

		public UnityEvent Action
		{
			get
			{
				if (action == null)
				{
					action = new UnityEvent();
				}
				return action;
			}
		}

		protected virtual void OnEnable()
		{
			if (action != null)
			{
				action.Invoke();
			}
		}
	}
}
