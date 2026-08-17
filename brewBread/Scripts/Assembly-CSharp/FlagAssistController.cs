using UnityEngine;

public class FlagAssistController : MonoBehaviour
{
	[SerializeField]
	private LayerMask groundLayer;

	private void Awake()
	{
		FlagAssistController[] array = Object.FindObjectsOfType<FlagAssistController>();
		if (array.Length <= 1)
		{
			return;
		}
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i] != this)
			{
				Object.Destroy(array[i].gameObject);
			}
		}
	}

	private void Start()
	{
		RaycastHit2D raycastHit2D = Physics2D.Raycast(base.transform.position, Vector2.down, 5f, groundLayer);
		if ((bool)raycastHit2D)
		{
			base.transform.position = raycastHit2D.point;
		}
	}
}
