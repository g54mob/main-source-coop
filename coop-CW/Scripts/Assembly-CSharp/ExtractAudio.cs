using UnityEngine;

public class ExtractAudio : MonoBehaviour
{
	public ExtractVideoMachine extract;

	public GameObject extractSound;

	public GameObject errorSound;

	public ExtractVideoStationHatch hatch;

	public SFX_Instance hatchClose;

	public SFX_Instance hatchOpen;

	private bool prevOpenHatch;

	public CDRom rom;

	private bool prevOpenRom;

	public SFX_Instance romClose;

	public SFX_Instance romOpen;

	private void Update()
	{
		if (extract.m_failedStateUI.activeSelf)
		{
			errorSound.SetActive(value: true);
		}
		else
		{
			errorSound.SetActive(value: false);
		}
		if (extract.m_extractionStateUI.activeSelf || extract.m_loadingStateUI.activeSelf)
		{
			extractSound.SetActive(value: true);
		}
		else
		{
			extractSound.SetActive(value: false);
		}
		if (prevOpenRom != rom.m_opened)
		{
			if (!rom.m_opened)
			{
				romClose.Play(base.transform.position);
			}
			if (rom.m_opened)
			{
				romOpen.Play(base.transform.position);
			}
		}
		if (prevOpenHatch != hatch.m_opened)
		{
			if (!hatch.m_opened)
			{
				hatchClose.Play(base.transform.position);
			}
			if (hatch.m_opened)
			{
				hatchOpen.Play(base.transform.position);
			}
		}
		prevOpenRom = rom.m_opened;
		prevOpenHatch = hatch.m_opened;
	}
}
