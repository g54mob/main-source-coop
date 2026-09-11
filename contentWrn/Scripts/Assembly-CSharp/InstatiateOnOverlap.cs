using UnityEngine;

public class InstatiateOnOverlap : MonoBehaviour
{
	private float t;

	public LayerMask layer;

	public GameObject[] hit;

	private void Update()
	{
		if (hit.Length == 0)
		{
			return;
		}
		if (Physics.OverlapSphere(base.transform.position, 0.5f, layer).Length != 0)
		{
			if (t < 0.001f)
			{
				for (int i = 0; i < hit.Length; i++)
				{
					Object.Instantiate(hit[i], base.transform.position, base.transform.rotation);
				}
			}
			t = 0.1f;
		}
		else
		{
			t -= Time.deltaTime;
		}
		t = Mathf.Clamp(t, 0f, 1f);
	}
}
