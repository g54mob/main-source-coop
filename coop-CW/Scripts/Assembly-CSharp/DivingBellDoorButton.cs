using System.Collections;
using UnityEngine;

public class DivingBellDoorButton : Interactable
{
	public DivingBell divingBell;

	public bool canOnlyOpen;

	private bool onCooldown;

	private bool isOpen;

	private float sinceSwitch = 10f;

	private string m_OpenText;

	private string m_CloseText;

	public Transform lever;

	private float currentAngle = 50f;

	private void Start()
	{
		m_OpenText = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.Open);
		m_CloseText = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.Close);
	}

	private void ToggleSwitch()
	{
		if (divingBell.opened)
		{
			isOpen = true;
		}
		else
		{
			isOpen = false;
		}
		sinceSwitch = 0f;
	}

	public override void Interact(Player player)
	{
		if (!onCooldown)
		{
			divingBell.AttemptSetOpen(!divingBell.opened);
			StartCoroutine(Cooldown());
		}
	}

	private IEnumerator Cooldown()
	{
		onCooldown = true;
		yield return new WaitForSeconds(0.5f);
		onCooldown = false;
	}

	public override bool IsValid(Player player)
	{
		return !divingBell.locked;
	}

	private void Update()
	{
		ToggleSwitch();
		hoverText = (divingBell.opened ? m_CloseText : m_OpenText);
		sinceSwitch += Time.deltaTime;
		if (sinceSwitch < 5f)
		{
			if (isOpen)
			{
				currentAngle = Mathf.Lerp(currentAngle, -50f, Time.deltaTime * 10f);
			}
			else
			{
				currentAngle = Mathf.Lerp(currentAngle, 50f, Time.deltaTime * 10f);
			}
			lever.transform.localEulerAngles = new Vector3(currentAngle, 0f, 0f);
		}
	}
}
