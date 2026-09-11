using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
	private Player player;

	private Interactable m_currentInteractable;

	private bool simulatedEPress;

	private float sinceSimulatedEPress;

	public Interactable CurrentInteractable => m_currentInteractable;

	private void Start()
	{
		player = GetComponentInParent<Player>();
	}

	private void Update()
	{
		if (!player.refs.view.IsMine)
		{
			return;
		}
		sinceSimulatedEPress += Time.deltaTime;
		if (sinceSimulatedEPress > 0.3f)
		{
			simulatedEPress = false;
		}
		if (player.data.dead)
		{
			return;
		}
		Interactable interactable = FindBestInteractible();
		if (!player.data.CanInteract())
		{
			interactable = null;
		}
		if (interactable != m_currentInteractable)
		{
			if ((bool)m_currentInteractable)
			{
				m_currentInteractable.OnEndHover(player);
			}
			if ((bool)interactable)
			{
				interactable.OnStartHover(player);
			}
			m_currentInteractable = interactable;
		}
		if ((bool)m_currentInteractable)
		{
			if (player.input.interactWasPressed || simulatedEPress)
			{
				interactable.Interact(player);
				simulatedEPress = false;
			}
			MainCamera.instance.outliner.enabled = true;
		}
		else
		{
			MainCamera.instance.outliner.enabled = false;
		}
	}

	private Interactable FindBestInteractible()
	{
		RaycastHit[] array = HelperFunctions.LineCheckAll(MainCamera.instance.transform.position, MainCamera.instance.transform.position + MainCamera.instance.transform.forward * 3f, HelperFunctions.LayerType.All);
		LayerMask mask = HelperFunctions.GetMask(HelperFunctions.LayerType.Interactable);
		Interactable result = null;
		float num = float.PositiveInfinity;
		RaycastHit[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			RaycastHit raycastHit = array2[i];
			if (!raycastHit.transform)
			{
				continue;
			}
			Debug.DrawLine(MainCamera.instance.transform.position, raycastHit.point, Color.red, 0.1f);
			Collider[] array3 = Physics.OverlapSphere(raycastHit.point, 0.35f, mask);
			for (int j = 0; j < array3.Length; j++)
			{
				if (array3[j].isTrigger)
				{
					continue;
				}
				Debug.DrawLine(raycastHit.point, array3[j].ClosestPoint(raycastHit.point), Color.green, 0.1f);
				Interactable componentInParent = array3[j].transform.GetComponentInParent<Interactable>();
				if ((bool)componentInParent && componentInParent.IsValid(player))
				{
					float num2 = Vector3.Distance(raycastHit.point, componentInParent.transform.position);
					if (num2 < num)
					{
						_ = array3[j];
						num = num2;
						result = componentInParent;
					}
				}
			}
		}
		return result;
	}

	public void PressE()
	{
		simulatedEPress = true;
		sinceSimulatedEPress = 0f;
	}
}
