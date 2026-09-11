using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Feedback : MonoBehaviour
{
	public static UI_Feedback instance;

	public Dictionary<Graphic, Color> originalColors = new Dictionary<Graphic, Color>();

	private Coroutine dieCor;

	public Color offColor;

	public ParticleSystem healParticle;

	private Coroutine takeDamageCor;

	public Color takeDamageColor;

	public Color healColor;

	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		Graphic[] componentsInChildren = GetComponentsInChildren<Graphic>();
		foreach (Graphic graphic in componentsInChildren)
		{
			originalColors.Add(graphic, graphic.color);
		}
	}

	public void Revive()
	{
		ResetColors();
	}

	public void Die()
	{
		Graphic[] graphics;
		List<Color> cols;
		if (dieCor == null)
		{
			graphics = GetComponentsInChildren<Graphic>();
			cols = new List<Color>();
			for (int i = 0; i < graphics.Length; i++)
			{
				cols.Add(graphics[i].color);
			}
			dieCor = StartCoroutine(IDieFeedback());
		}
		IEnumerator IDieFeedback()
		{
			for (int j = 0; j < 6; j++)
			{
				float p = (float)j / 5f;
				SetColor(graphics, offColor);
				yield return new WaitForSeconds(Random.Range(0.1f, 0.2f) * p);
				SetColors(graphics, cols);
				yield return new WaitForSeconds(Random.Range(0.1f, 0.2f) * (1f - p));
			}
			SetColor(graphics, offColor);
			yield return new WaitForSeconds(2f);
			SetColors(graphics, cols);
			dieCor = null;
		}
	}

	public void HealFeedback()
	{
		TakeDamage(isHealing: true);
		healParticle.Play();
	}

	public void TakeDamage(bool isHealing = false)
	{
		Graphic[] graphics;
		List<Color> cols;
		if (takeDamageCor == null)
		{
			graphics = GetComponentsInChildren<Graphic>();
			cols = new List<Color>();
			for (int i = 0; i < graphics.Length; i++)
			{
				cols.Add(graphics[i].color);
			}
			takeDamageCor = StartCoroutine(ITakeDamageFeedback(isHealing));
		}
		IEnumerator ITakeDamageFeedback(bool flag = false)
		{
			SetColor(graphics, flag ? healColor : takeDamageColor);
			yield return new WaitForSeconds(0.2f);
			SetColors(graphics, cols);
			takeDamageCor = null;
		}
	}

	public void ResetColors()
	{
		foreach (KeyValuePair<Graphic, Color> originalColor in originalColors)
		{
			if (!originalColor.Key.gameObject.CompareTag("DontBlink"))
			{
				originalColor.Key.color = originalColor.Value;
			}
		}
	}

	private void SetColors(Graphic[] graphics, List<Color> cols)
	{
		for (int i = 0; i < graphics.Length; i++)
		{
			if (!(graphics[i] == null) && !(graphics[i].gameObject.tag == "DontBlink"))
			{
				graphics[i].color = cols[i];
			}
		}
	}

	private void SetColor(Graphic[] graphics, Color col)
	{
		for (int i = 0; i < graphics.Length; i++)
		{
			if (!(graphics[i] == null) && !(graphics[i].gameObject.tag == "DontBlink"))
			{
				graphics[i].color = col;
			}
		}
	}
}
