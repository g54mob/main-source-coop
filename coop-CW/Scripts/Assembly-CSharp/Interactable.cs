using EPOOutline;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
	protected Outlinable m_outline;

	public virtual string hoverText { get; set; }

	protected virtual void Awake()
	{
		m_outline = GetComponent<Outlinable>();
		if (m_outline == null)
		{
			m_outline = base.gameObject.AddComponent<Outlinable>();
		}
		m_outline.enabled = false;
	}

	public virtual bool IsValid(Player player)
	{
		return true;
	}

	public abstract void Interact(Player player);

	public virtual void OnEndHover(Player player)
	{
		m_outline.enabled = false;
	}

	public virtual void OnStartHover(Player player)
	{
		m_outline.enabled = true;
	}
}
