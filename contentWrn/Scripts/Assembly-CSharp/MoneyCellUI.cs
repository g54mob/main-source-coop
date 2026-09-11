using System.Collections;
using Sirenix.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zorro.Core;

public class MoneyCellUI : MonoBehaviour
{
	public enum MoneyCellType
	{
		HospitalBill = 0,
		Revenue = 1,
		MetaCoins = 2
	}

	public Graphic[] m_rebootMe;

	public TextMeshProUGUI m_moneyText;

	public TextMeshProUGUI m_messageText;

	public AnimationCurve m_scaleInCurve;

	public AnimationCurve m_fadeOutCurve;

	public Graphic outline;

	public Graphic fill;

	public Color goodColor;

	public Color goodOutlineColor;

	private CanvasGroup m_canvasGroup;

	public SFX_Instance cash;

	public SFX_Instance metaCoins;

	public SFX_Instance noCash;

	public void Init(string message, string money, MoneyCellType cellType)
	{
		m_canvasGroup = GetComponent<CanvasGroup>();
		m_messageText.text = message;
		m_moneyText.text = money;
		bool num = cellType != MoneyCellType.HospitalBill;
		if (!num)
		{
			noCash.Play(Camera.main.transform.position);
		}
		if (num)
		{
			if (cellType == MoneyCellType.MetaCoins)
			{
				metaCoins.Play(Camera.main.transform.position);
			}
			else
			{
				cash.Play(Camera.main.transform.position);
			}
			outline.color = goodOutlineColor;
			fill.color = goodColor;
		}
		StartCoroutine(Show());
	}

	private IEnumerator Show()
	{
		yield return m_scaleInCurve.YieldForCurve(delegate(float f)
		{
			base.transform.localScale = new Vector3(1f, f, 1f);
			m_rebootMe.ForEach(delegate(Graphic graphic)
			{
				graphic.enabled = false;
			});
			m_rebootMe.ForEach(delegate(Graphic graphic)
			{
				graphic.enabled = true;
			});
		}, timeScale: false, 5f);
		base.transform.localScale = new Vector3(1f, 1f, 1f);
		m_rebootMe.ForEach(delegate(Graphic graphic)
		{
			graphic.enabled = false;
		});
		m_rebootMe.ForEach(delegate(Graphic graphic)
		{
			graphic.enabled = true;
		});
	}

	public void Clear()
	{
		StartCoroutine(ClearRoutine());
		IEnumerator ClearRoutine()
		{
			yield return m_fadeOutCurve.YieldForCurve(delegate(float f)
			{
				m_canvasGroup.alpha = f;
			}, timeScale: false, 4f);
			Object.Destroy(base.gameObject);
		}
	}
}
