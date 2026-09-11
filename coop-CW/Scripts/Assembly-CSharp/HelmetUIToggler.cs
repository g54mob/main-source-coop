using UnityEngine;

public class HelmetUIToggler : MonoBehaviour
{
	[SerializeField]
	private GameObject canvas;

	private void Update()
	{
		if (canvas.activeInHierarchy == Spectate.spectating)
		{
			canvas.SetActive(!Spectate.spectating);
		}
	}
}
