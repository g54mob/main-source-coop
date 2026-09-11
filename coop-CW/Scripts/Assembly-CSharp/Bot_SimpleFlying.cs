using UnityEngine;

public class Bot_SimpleFlying : MonoBehaviour
{
	private Transform root;

	public float speed;

	public float drag;

	[Range(0f, 1f)]
	public float perlinDragFacotor;

	public PerlinSampler perlin;

	private Vector3 vel;

	private Bot bot;

	private void Start()
	{
		bot = GetComponent<Bot>();
		root = base.transform.root;
	}

	private void Update()
	{
		vel = FRILerp.Lerp(vel, bot.syncData.movementInput * speed, drag * Mathf.Lerp(1f, perlin.SampleValue(new Vector2(Time.time, 0f)), perlinDragFacotor));
		root.position += vel * Time.deltaTime;
	}
}
