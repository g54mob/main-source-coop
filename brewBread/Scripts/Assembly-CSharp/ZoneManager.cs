using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ZoneManager : MonoBehaviour
{
	[SerializeField]
	private float _timeShowed;

	[SerializeField]
	private TextMeshPro _zoneTitle;

	[SerializeField]
	private List<SpriteRenderer> _spritesToShow;

	[SerializeField]
	private bool _enableLights;

	private bool _showingTitle;

	private bool _enablingLights;

	private float _lightIntensity;

	private void Start()
	{
		Color color = _zoneTitle.color;
		color.a = 0f;
		_zoneTitle.color = color;
		foreach (SpriteRenderer item in _spritesToShow)
		{
			Color color2 = item.color;
			color2.a = 0f;
			item.color = color2;
		}
		_showingTitle = false;
		_lightIntensity = StaticInstance<Bread>.Instance.LightIntensity;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.layer == 9)
		{
			if (!_showingTitle)
			{
				StopCoroutine(HideTitle());
				StartCoroutine(ShowTitle());
			}
			if (!_enablingLights)
			{
				StartCoroutine(TurnOnOffLights(_enableLights));
			}
		}
	}

	private IEnumerator ShowTitle()
	{
		_showingTitle = true;
		Color whiteAlphaZero = Color.white;
		whiteAlphaZero.a = 0f;
		Color alphaZero = _zoneTitle.color;
		Color alphaOne = _zoneTitle.color;
		alphaOne.a = 1f;
		float t = 0f;
		while (true)
		{
			_zoneTitle.color = Color.Lerp(alphaZero, alphaOne, t);
			foreach (SpriteRenderer item in _spritesToShow)
			{
				item.color = Color.Lerp(whiteAlphaZero, Color.white, t);
			}
			t += Time.deltaTime;
			if (t > 1f)
			{
				break;
			}
			yield return new WaitForEndOfFrame();
		}
		yield return new WaitForSeconds(_timeShowed);
		StartCoroutine(HideTitle());
	}

	private IEnumerator HideTitle()
	{
		Color whiteAlphaZero = Color.white;
		whiteAlphaZero.a = 0f;
		Color alphaZero = _zoneTitle.color;
		alphaZero.a = 0f;
		Color alphaOne = _zoneTitle.color;
		float t = 0f;
		while (true)
		{
			_zoneTitle.color = Color.Lerp(alphaOne, alphaZero, t);
			foreach (SpriteRenderer item in _spritesToShow)
			{
				item.color = Color.Lerp(Color.white, whiteAlphaZero, t);
			}
			t += Time.deltaTime;
			if (t > 1f)
			{
				break;
			}
			yield return new WaitForEndOfFrame();
		}
		_showingTitle = false;
	}

	private IEnumerator TurnOnOffLights(bool on)
	{
		_enablingLights = true;
		float t = ((!on) ? 1 : 0);
		while (true)
		{
			StaticInstance<Bread>.Instance.PointLight.intensity = Mathf.Lerp(0f, _lightIntensity, t);
			StaticInstance<Fred>.Instance.PointLight.intensity = Mathf.Lerp(0f, _lightIntensity, t);
			t += (on ? Time.deltaTime : (0f - Time.deltaTime));
			if (t > 1f)
			{
				break;
			}
			yield return new WaitForEndOfFrame();
		}
		_enablingLights = false;
	}
}
