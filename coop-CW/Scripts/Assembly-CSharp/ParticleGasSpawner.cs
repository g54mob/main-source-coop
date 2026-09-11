using UnityEngine;

public class ParticleGasSpawner : MonoBehaviour
{
	public GameObject gasTrigger;

	private float counter;

	private ParticleSystem part;

	private void Start()
	{
		part = GetComponent<ParticleSystem>();
	}

	private void Update()
	{
		counter += Time.deltaTime;
		if (part.isPlaying && counter > 0.5f)
		{
			counter = 0f;
			Emit();
		}
	}

	private void Emit()
	{
		Object.Instantiate(gasTrigger, part.transform.position, part.transform.rotation);
	}
}
