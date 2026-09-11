using UnityEngine.Events;

public class GenericInteractible : Interactable
{
	public UnityEvent interactEvent;

	public override void Interact(Player player)
	{
		interactEvent.Invoke();
	}
}
