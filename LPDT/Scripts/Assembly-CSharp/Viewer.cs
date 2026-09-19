using UnityEngine;
using UnityEngine.UI;

public class Viewer : MonoBehaviour
{
	public ParticleSystem[] Particles;

	public int showNum;

	public GameObject ShowPos;

	[Header("UI Elements")]
	public Text _fxNameText;

	public Button _PlayButton;

	public Button _PauseButton;

	public Text _PauseText;

	public float Rotspeed;

	public GameObject _Cam;

	private bool isPaused;

	private void Start()
	{
		for (int i = 0; i < Particles.Length; i++)
		{
			Particles[i].gameObject.SetActive(value: false);
		}
		isPaused = false;
		_PauseButton.gameObject.SetActive(value: false);
		_PlayButton.gameObject.SetActive(value: true);
	}

	private void Update()
	{
		showNum = Mathf.Clamp(showNum, 0, Particles.Length - 1);
		if (!isPaused)
		{
			_fxNameText.text = Particles[showNum].gameObject.name;
			_PauseText.text = "Pause";
		}
		else
		{
			_fxNameText.text = Particles[showNum].gameObject.name + "-Paused";
			_PauseText.text = "Resume";
		}
		if (Particles[showNum].IsAlive())
		{
			_PauseButton.gameObject.SetActive(value: true);
			_PlayButton.gameObject.SetActive(value: false);
		}
		else
		{
			_PauseButton.gameObject.SetActive(value: false);
			_PlayButton.gameObject.SetActive(value: true);
		}
		if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			Previous();
		}
		if (Input.GetKeyDown(KeyCode.RightArrow))
		{
			Next();
		}
		if ((Input.GetKeyDown(KeyCode.Space) && !Particles[showNum].IsAlive()) || (Input.GetKeyDown(KeyCode.Space) && isPaused))
		{
			PlayFX();
		}
		else if (Input.GetKeyDown(KeyCode.Space) && Particles[showNum].IsAlive() && !isPaused)
		{
			PauseFX();
		}
		if (Input.GetMouseButtonDown(0) && Physics.Raycast(_Cam.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition), out var hitInfo, float.PositiveInfinity))
		{
			GameObject gameObject = Object.Instantiate(Particles[showNum].gameObject, hitInfo.point, Quaternion.FromToRotation(Particles[showNum].transform.up, hitInfo.normal));
			gameObject.gameObject.SetActive(value: true);
			gameObject.GetComponent<ParticleSystem>().Play();
			Object.Destroy(gameObject, gameObject.GetComponent<ParticleSystem>().main.duration);
		}
		if (Input.GetMouseButton(1))
		{
			float angle = Input.GetAxis("Mouse X") * Time.deltaTime * Rotspeed;
			float num = Input.GetAxis("Mouse Y") * Time.deltaTime * Rotspeed;
			Vector3 position = ShowPos.transform.position;
			_Cam.transform.RotateAround(position, Vector3.up, angle);
			_Cam.transform.RotateAround(position, Vector3.right, 0f - num);
			_Cam.transform.LookAt(position);
		}
	}

	public void Next()
	{
		isPaused = false;
		if (showNum != Particles.Length - 1)
		{
			Particles[showNum].gameObject.SetActive(value: false);
			showNum++;
			Particles[showNum].gameObject.transform.position = ShowPos.transform.position;
			Particles[showNum].gameObject.SetActive(value: true);
		}
		else
		{
			Particles[showNum].gameObject.SetActive(value: false);
			showNum = 0;
			Particles[showNum].gameObject.transform.position = ShowPos.transform.position;
			Particles[showNum].gameObject.SetActive(value: true);
		}
	}

	public void Previous()
	{
		isPaused = false;
		if (showNum != 0)
		{
			Particles[showNum].gameObject.SetActive(value: false);
			showNum--;
			Particles[showNum].gameObject.transform.position = ShowPos.transform.position;
			Particles[showNum].gameObject.SetActive(value: true);
		}
		else
		{
			Particles[showNum].gameObject.SetActive(value: false);
			showNum = Particles.Length - 1;
			Particles[showNum].gameObject.transform.position = ShowPos.transform.position;
			Particles[showNum].gameObject.SetActive(value: true);
		}
	}

	public void PlayFX()
	{
		if (!Particles[showNum].IsAlive())
		{
			Particles[showNum].gameObject.transform.position = ShowPos.transform.position;
			Particles[showNum].gameObject.SetActive(value: true);
			Particles[showNum].Play();
		}
		if (isPaused)
		{
			Particles[showNum].Play();
			isPaused = false;
		}
	}

	public void PauseFX()
	{
		if (!isPaused)
		{
			isPaused = true;
			Particles[showNum].Pause();
		}
		else if (isPaused)
		{
			isPaused = false;
			Particles[showNum].Play();
		}
	}
}
