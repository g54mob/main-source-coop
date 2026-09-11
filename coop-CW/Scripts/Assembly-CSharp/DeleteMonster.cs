using Photon.Pun;
using UnityEngine;

public class DeleteMonster : MonoBehaviour
{
	public float seconds = 10f;

	private ParticleSystem part;

	private PhotonView view;

	private Player player;

	private bool deletedMonster;

	private void Start()
	{
		player = GetComponentInParent<Player>();
		view = GetComponentInParent<PhotonView>();
		part = GetComponent<ParticleSystem>();
	}

	private void Update()
	{
		seconds -= Time.deltaTime;
		if (seconds < -3f)
		{
			Object.Destroy(base.gameObject);
		}
		if (!deletedMonster && seconds < 0f)
		{
			GameObject gameObject = base.transform.root.gameObject;
			base.transform.SetParent(null);
			ParticleSystem.ShapeModule shape = part.shape;
			shape.skinnedMeshRenderer = player.refs.bodyMeshRenderer;
			part.Emit(50);
			if (view.IsMine)
			{
				MonoFunctions.instance.PhotonDestroy(gameObject, 1f);
			}
			gameObject.SetActive(value: false);
			deletedMonster = true;
		}
	}
}
