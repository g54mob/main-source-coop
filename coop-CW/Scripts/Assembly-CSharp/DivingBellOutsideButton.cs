using System.Collections;
using UnityEngine;

public class DivingBellOutsideButton : Interactable
{
	public DivingBell divingBell;

	private bool onCooldown;

	private string m_OpenText;

	private string m_CloseText;

	private void Start()
	{
		m_OpenText = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.Open);
		m_CloseText = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.Close);
	}

	public override bool IsValid(Player player)
	{
		hoverText = (divingBell.opened ? m_CloseText : m_OpenText);
		if (Vector3.Angle(-MainCamera.instance.transform.forward, base.transform.forward) > 90f)
		{
			return false;
		}
		return !divingBell.opened;
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
}
