using System.ComponentModel;
using UnityEngine;

namespace Rewired.Internal
{
	[Browsable(false)]
	[ExecuteInEditMode]
	[RequireComponent(typeof(InputManager_Base))]
	[AddComponentMenu("")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public class OnGUIHelper : MonoBehaviour
	{
		private InputManager_Base QMmaBzbGhLRYBQhUiAbwUcRQwInF;

		[CustomObfuscation(rename = false)]
		private void Awake()
		{
			QMmaBzbGhLRYBQhUiAbwUcRQwInF = GetComponent<InputManager_Base>();
		}

		[CustomObfuscation(rename = false)]
		private void OnGUI()
		{
			if (!(QMmaBzbGhLRYBQhUiAbwUcRQwInF == null))
			{
				QMmaBzbGhLRYBQhUiAbwUcRQwInF.OnGUIUpdate();
			}
		}
	}
}
