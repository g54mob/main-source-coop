using UnityEngine;

public class OxygenDisplay : MonoBehaviour
{
	private static readonly int Fill = Shader.PropertyToID("_Fill");

	private SkinnedMeshRenderer rend;

	private Material mat;

	private Player player;

	private void Start()
	{
		player = GetComponentInParent<Player>();
		rend = GetComponent<SkinnedMeshRenderer>();
		mat = rend.materials[2];
	}

	private void Update()
	{
		if (Time.frameCount % 3 != 0)
		{
			mat.SetFloat(Fill, player.data.OxygenDisplayPercentage());
		}
	}
}
