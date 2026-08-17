using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class CutsceneController : MonoBehaviour
{
	[SerializeField]
	private GameObject _blackFade;

	[SerializeField]
	private GameObject _globalVolume;

	[SerializeField]
	private PlayableDirector _timeline;

	[SerializeField]
	private LineRenderer _ropeRenderer;

	[SerializeField]
	private GameObject _assistPopUp;

	private void Awake()
	{
		_blackFade.SetActive(value: true);
		_globalVolume.SetActive(value: false);
	}

	private void Start()
	{
		StartCoroutine(DisableFade());
		if (StaticInstance<DataBetweenScenes>.Instance.loadGame)
		{
			for (int i = 0; i < base.transform.childCount; i++)
			{
				base.transform.GetChild(i).gameObject.SetActive(value: false);
			}
			StaticInstance<CameraController>.Instance.SetDamping(Vector2.one);
		}
		else
		{
			StaticInstance<Pause>.Instance.SetEnablePause(v: false);
			_ropeRenderer.enabled = false;
			_timeline.Play();
			StaticInstance<Bread>.Instance.DisableInput();
			StaticInstance<Bread>.Instance.SetSpriteRendererActive(value: false);
			StaticInstance<Fred>.Instance.DisableInput();
			StaticInstance<Fred>.Instance.SetSpriteRendererActive(value: false);
		}
	}

	private IEnumerator DisableFade()
	{
		yield return new WaitForEndOfFrame();
		_blackFade.SetActive(value: false);
		_globalVolume.SetActive(value: true);
	}

	public void EndCutscene()
	{
		StaticInstance<Pause>.Instance.SetEnablePause(v: true);
		StaticInstance<Bread>.Instance.EnableInput();
		StaticInstance<Bread>.Instance.SetSpriteRendererActive(value: true);
		StaticInstance<Fred>.Instance.EnableInput();
		StaticInstance<Fred>.Instance.SetSpriteRendererActive(value: true);
		_ropeRenderer.enabled = true;
		Object.Destroy(base.gameObject);
		StaticInstance<CameraController>.Instance.SetDamping(Vector2.one);
		_assistPopUp.SetActive(value: true);
	}
}
