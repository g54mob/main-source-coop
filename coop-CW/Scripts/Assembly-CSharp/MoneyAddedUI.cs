using System.Collections.Generic;
using UnityEngine;

public class MoneyAddedUI : MonoBehaviour
{
	public GameObject m_moneyAddedPrefab;

	public Transform m_moneyAddedParent;

	private List<MoneyCellUI> m_cells = new List<MoneyCellUI>();

	private float m_lastShowTime;

	public void Show(string header, string money, MoneyCellUI.MoneyCellType cellType)
	{
		m_lastShowTime = Time.time;
		GameObject obj = Object.Instantiate(m_moneyAddedPrefab, m_moneyAddedParent);
		obj.transform.SetAsFirstSibling();
		MoneyCellUI component = obj.GetComponent<MoneyCellUI>();
		component.Init(header, money, cellType);
		m_cells.Add(component);
	}

	private void ClearAll()
	{
		m_cells.ForEach(delegate(MoneyCellUI ui)
		{
			ui.Clear();
		});
		m_cells.Clear();
	}

	private void Update()
	{
		if (m_cells.Count > 0 && m_lastShowTime + 5f < Time.time)
		{
			ClearAll();
		}
	}
}
