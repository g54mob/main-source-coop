using UnityEngine;

public class FallCharacter : MonoBehaviour
{
	private float max_height;

	private float timer;

	public float fallTime;

	private CharacterCTRL cTRL;

	public GameObject fallparticles;

	private void Start()
	{
		max_height = base.transform.position.y;
		cTRL = GetComponent<CharacterCTRL>();
	}

	private void Update()
	{
		if (cTRL.GetGrounded())
		{
			max_height = base.transform.position.y;
		}
		if (base.transform.position.y < max_height && cTRL._playerState != PlayerState.FALLING && cTRL._playerState != PlayerState.HANGING && cTRL._playerState != PlayerState.ATTACHED)
		{
			timer += Time.deltaTime;
		}
		else
		{
			max_height = base.transform.position.y;
			timer = 0f;
		}
		if (timer >= fallTime)
		{
			cTRL._playerState = PlayerState.FALLING;
			timer = 0f;
		}
	}

	public void SetIdle()
	{
		cTRL._playerState = PlayerState.IDLE;
		Object.Instantiate(fallparticles, base.transform.position, Quaternion.identity);
	}
}
