using UnityEngine;

public class SecondTutorialSpawner : MonoBehaviour
{
	[SerializeField]
	private GameObject _charlie2;

	private void OnTriggerExit2D(Collider2D collision)
	{
		_charlie2.SetActive(value: true);
	}
}
