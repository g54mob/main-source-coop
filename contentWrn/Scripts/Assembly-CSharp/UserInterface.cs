using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;
using Zorro.Core;

public class UserInterface : Singleton<UserInterface>
{
	public MoneyAddedUI moneyAddedUI;

	public EquippedUI equippedUI;

	public InteractionUI interactionUI;

	public HotbarUI hotbarUI;

	public NextStepUI nextStepUI;

	public float force = 100f;

	public float movementForce = 100f;

	[FormerlySerializedAs("translationtForce")]
	public float translationForce = 100f;

	public float damp = 0.95f;

	public float m_maxPivotMovement = 150f;

	public Transform m_pivot;

	private int2 m_currentResolution;

	private void LateUpdate()
	{
		if (Time.frameCount % 5 != 0)
		{
			return;
		}
		int2 int5 = new int2(Screen.width, Screen.height);
		if (!m_currentResolution.Equals(int5))
		{
			AspectRatio aspectRatio = AspectRatioUtility.GetAspectRatio((float)Screen.width / (float)Screen.height);
			LoadAspectRatio(aspectRatio);
			m_currentResolution = int5;
		}
		bool num = Player.localPlayer != null;
		Player player = Player.localPlayer;
		if (num)
		{
			hotbarUI.Set(player);
			interactionUI.SetData(player.refs.interaction.CurrentInteractable);
			UpdateInventory();
			Objective currentObjective = PhotonGameLobbyHandler.CurrentObjective;
			if (currentObjective != null)
			{
				nextStepUI.SetData(currentObjective);
			}
			else
			{
				nextStepUI.Hide();
			}
		}
		void UpdateInventory()
		{
			if (player.TryGetInventory(out var o) && o.TryGetSlot(player.data.selectedItemSlot, out var slot))
			{
				equippedUI.SetData(slot.ItemInSlot);
			}
			else
			{
				equippedUI.SetData(ItemDescriptor.Empty);
			}
		}
	}

	public void LoadAspectRatio(AspectRatio aspectRatio)
	{
		if (aspectRatio == AspectRatio._16x10)
		{
			aspectRatio = AspectRatio._4x3;
		}
		AspectRatioUISetter[] componentsInChildren = GetComponentsInChildren<AspectRatioUISetter>(includeInactive: true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].LoadAspectPosition(aspectRatio);
		}
	}

	public static void ShowMoneyNotification(string header, string money, MoneyCellUI.MoneyCellType cellType)
	{
		Singleton<UserInterface>.Instance.moneyAddedUI.Show(header, money, cellType);
	}
}
