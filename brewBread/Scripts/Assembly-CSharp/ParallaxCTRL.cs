using Cinemachine;
using UnityEngine;

public class ParallaxCTRL : MonoBehaviour
{
	[SerializeField]
	private Camera mainCamera;

	[SerializeField]
	private CinemachineVirtualCamera _cinemachineVirtual;

	public Transform group;

	private CinemachineBasicMultiChannelPerlin _perlin;

	[SerializeField]
	[Space(10f)]
	private Vector2 offset;

	private Vector2 startPosition;

	private float startZ;

	private float _initialCameraZ;

	private Vector2 travel => (Vector2)mainCamera.transform.position - startPosition;

	private float distFromSubject => base.transform.position.z - group.position.z;

	private float clippingPlane => _initialCameraZ + ((distFromSubject > 0f) ? mainCamera.farClipPlane : mainCamera.nearClipPlane);

	private float parallaxFactor => Mathf.Abs(distFromSubject) / clippingPlane;

	private void Start()
	{
		startPosition = base.transform.position;
		startZ = base.transform.position.z;
		_perlin = _cinemachineVirtual.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
		_initialCameraZ = mainCamera.transform.position.z;
	}

	private void Update()
	{
		Vector2 vector = startPosition + travel * parallaxFactor;
		base.transform.position = new Vector3(vector.x + offset.x, vector.y + offset.y, startZ);
	}
}
