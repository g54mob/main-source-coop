using UnityEngine;

namespace pworld.Scripts
{
	public class PMouseLock : MonoBehaviour
	{
		private bool shouldLock;

		private void Update()
		{
			if (Input.GetKey(KeyCode.LeftShift) || (Input.GetKey(KeyCode.Mouse3) && !shouldLock))
			{
				Cursor.lockState = CursorLockMode.None;
				Cursor.visible = true;
			}
			else
			{
				Cursor.lockState = CursorLockMode.Locked;
				Cursor.visible = false;
			}
		}

		private void OnEnable()
		{
			shouldLock = true;
		}

		private void OnDisable()
		{
			shouldLock = false;
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}
	}
}
