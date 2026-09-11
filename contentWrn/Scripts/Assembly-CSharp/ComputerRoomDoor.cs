using UnityEngine;

public class ComputerRoomDoor : MonoBehaviour
{
	public Animator doorAnimator;

	private bool m_doorOpen;

	public void OpenDoor()
	{
		if (!m_doorOpen)
		{
			m_doorOpen = true;
			Debug.Log("Door opened");
			doorAnimator.Play("DoorAnim2");
		}
	}
}
