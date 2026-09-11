using TMPro;
using UnityEngine;

public class UI_Money : MonoBehaviour
{
	private TextMeshProUGUI text;

	private int m_money = -1;

	private void Start()
	{
		text = GetComponent<TextMeshProUGUI>();
	}

	private void Update()
	{
		if (SurfaceNetworkHandler.RoomStats != null && m_money != SurfaceNetworkHandler.RoomStats.Money)
		{
			m_money = SurfaceNetworkHandler.RoomStats.Money;
			text.text = "$ " + m_money;
		}
	}
}
