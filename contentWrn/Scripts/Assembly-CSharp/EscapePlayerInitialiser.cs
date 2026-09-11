using UnityEngine;

public class EscapePlayerInitialiser : MonoBehaviour
{
	public EscapePlayerHandler m_handler;

	private void Awake()
	{
		if (m_handler == null)
		{
			Debug.LogError("Missing EscapePlayerHandler");
		}
		else
		{
			m_handler.Initialise();
		}
	}
}
