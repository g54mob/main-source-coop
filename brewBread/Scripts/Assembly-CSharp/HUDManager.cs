using System.Collections;
using Cinemachine;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class HUDManager : MonoBehaviour
{
	[Header("Height")]
	[SerializeField]
	private TextMeshProUGUI heightText;

	[SerializeField]
	private CinemachineTargetGroup targetGroup;

	[SerializeField]
	private int heightOffset;

	[SerializeField]
	private float punchFactor = 0.2f;

	private float punchFactorAux = 0.2f;

	[Header("Timer")]
	[SerializeField]
	private GameObject timer;

	[SerializeField]
	private TextMeshProUGUI timerText;

	[SerializeField]
	private TextMeshProUGUI timermsText;

	private GameManager manager;

	private int auxHeight;

	private bool changingColor;

	private float colorTimeChange;

	private void Start()
	{
		manager = Object.FindObjectOfType<GameManager>();
		DOTween.Init();
		punchFactorAux = punchFactor;
		StartCoroutine(CalculateHeight());
	}

	private IEnumerator CalculateHeight()
	{
		int num = -100;
		for (int i = 0; i < targetGroup.m_Targets.Length; i++)
		{
			if (targetGroup.m_Targets[i].target.transform.position.y > (float)num)
			{
				num = (int)targetGroup.m_Targets[i].target.transform.position.y;
			}
		}
		if (num != auxHeight)
		{
			heightText.transform.DOPunchScale(new Vector3(0f, punchFactor, 0f), 0.1f).From();
			auxHeight = num;
		}
		heightText.text = (num + heightOffset).ToString();
		yield return new WaitForSeconds(0.1f);
		StartCoroutine(CalculateHeight());
	}
}
