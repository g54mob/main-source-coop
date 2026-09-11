using UnityEngine;

public class AmbienceTrigger : MonoBehaviour
{
	public AudioClip[] clips;

	public Bounds bounds;

	internal float size;

	private void Start()
	{
		bounds = GetComponent<BoxCollider>().bounds;
		size = bounds.size.magnitude;
		base.gameObject.SetActive(value: false);
	}
}
