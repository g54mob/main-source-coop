using UnityEngine;

public class PhotoItem : MonoBehaviour
{
	private bool _collected;

	private Animator _animator;

	[SerializeField]
	private GameObject[] _particlesToDelete;

	[SerializeField]
	private GameObject _photoPreviewPrefab;

	private void Awake()
	{
		_animator = GetComponent<Animator>();
	}

	private void SavePhoto()
	{
	}

	private void LoadPhoto()
	{
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.layer == 9 && !_collected)
		{
			_collected = true;
			_animator.SetTrigger("Collect");
		}
	}

	public void PhotoCollected()
	{
		Vector3 cameraPosition = StaticInstance<CameraController>.Instance.GetCameraPosition();
		cameraPosition.z = 0f;
		Object.Instantiate(_photoPreviewPrefab, cameraPosition, Quaternion.identity);
		GameObject[] particlesToDelete = _particlesToDelete;
		for (int i = 0; i < particlesToDelete.Length; i++)
		{
			Object.Destroy(particlesToDelete[i]);
		}
		SavePhoto();
	}

	public void PhotoShown()
	{
		Object.Destroy(base.gameObject);
	}
}
