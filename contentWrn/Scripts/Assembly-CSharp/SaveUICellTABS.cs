using System.Collections.Generic;
using UnityEngine;
using Zorro.UI;

public class SaveUICellTABS : TABS<SaveUICell>
{
	private List<SaveUICell> m_cells = new List<SaveUICell>();

	private bool m_initialized;

	protected override void Start()
	{
		base.Start();
		Initialize();
	}

	private void Initialize()
	{
		if (!m_initialized)
		{
			m_initialized = true;
			SaveUICell[] componentsInChildren = GetComponentsInChildren<SaveUICell>(includeInactive: true);
			Save[] savesOnFile = SaveSystem.SavesOnFile;
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				Save currentSave = savesOnFile[i];
				SaveUICell saveUICell = componentsInChildren[i];
				saveUICell.SetSave(currentSave, i);
				m_cells.Add(saveUICell);
			}
		}
	}

	public override void OnSelected(SaveUICell button)
	{
		_ = SaveSystem.SavesOnFile;
	}

	public GameObject GetFirstSelectedGameObject()
	{
		if (m_cells.Count == 0)
		{
			Initialize();
		}
		return m_cells[0].gameObject;
	}
}
