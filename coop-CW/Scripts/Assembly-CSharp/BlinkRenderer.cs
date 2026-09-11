using UnityEngine;

public class BlinkRenderer : MonoBehaviour
{
	private float counter;

	public float rate = 0.5f;

	public FootstepHandler step;

	private void Start()
	{
		GetComponentInParent<Player>().refs.bodyMeshRenderer.enabled = false;
	}

	private void Update()
	{
		counter += Time.deltaTime;
		if (counter < rate && (bool)step)
		{
			step.step = false;
		}
		if (counter > rate)
		{
			counter = 0f;
			Go();
			if ((bool)step)
			{
				step.step = true;
			}
		}
	}

	private void Go()
	{
		Transform child = base.transform.GetChild(0);
		GameObject gameObject = Object.Instantiate(child.gameObject, child.position, child.rotation);
		gameObject.AddComponent<RemoveAfterSeconds>().seconds = rate;
		gameObject.transform.localScale = child.lossyScale;
		PlayerVisual component = gameObject.GetComponent<PlayerVisual>();
		for (int i = 0; i < component.followerConfig.Length; i++)
		{
			component.followerConfig[i].main.SetParent(gameObject.transform, worldPositionStays: true);
			component.followerConfig[i].main.position = component.followerConfig[i].target.position;
			component.followerConfig[i].main.rotation = component.followerConfig[i].target.rotation;
		}
		Object.DestroyImmediate(component);
		gameObject.SetActive(value: true);
	}
}
