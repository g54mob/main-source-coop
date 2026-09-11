using UnityEngine;

public class AggroObectToggler : MonoBehaviour
{
	private Bot bot;

	public GameObject target;

	public float enableDelay;

	private float enabledFor;

	private void Start()
	{
		bot = base.transform.root.GetComponentInChildren<Bot>();
	}

	private void Update()
	{
		if (bot.aggro)
		{
			if (!target.activeSelf && enabledFor > enableDelay)
			{
				target.SetActive(value: true);
			}
			enabledFor += Time.deltaTime;
		}
		else
		{
			if (target.activeSelf)
			{
				target.SetActive(value: false);
			}
			enabledFor = 0f;
		}
	}
}
