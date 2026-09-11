using System;
using UnityEngine;

public class ShopInteractableRightArrow : Interactable
{
	private Action m_OnAction;

	protected override void Awake()
	{
		base.Awake();
		base.gameObject.layer = LayerMask.NameToLayer("Interactable");
	}

	private void Start()
	{
		hoverText = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.Right);
	}

	public override void Interact(Player player)
	{
		m_OnAction?.Invoke();
	}

	public void AddOnAction(Action a)
	{
		m_OnAction = (Action)Delegate.Combine(m_OnAction, a);
	}
}
