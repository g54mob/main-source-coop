using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class SoundPlayerItem : ItemInstanceBehaviour
{
	public InputActionReference m_cameraZoomIn;

	public InputActionReference m_cameraZoomOut;

	public SFX_Instance[] sounds;

	private IntRangeEntry selectionEntry;

	private OnOffEntry playingEntry;

	public TextMeshPro numberText;

	public TextMeshPro text;

	public Transform spinner;

	private float counter = 1f;

	private int currentNr;

	private bool ready;

	private bool on;

	public override void ConfigItem(ItemInstanceData data, PhotonView playerView)
	{
		ready = true;
		if (!data.TryGetEntry<IntRangeEntry>(out selectionEntry))
		{
			selectionEntry = new IntRangeEntry
			{
				selectedValue = 0,
				maxValue = sounds.Length
			};
			data.AddDataEntry(selectionEntry);
		}
		if (!data.TryGetEntry<OnOffEntry>(out playingEntry))
		{
			playingEntry = new OnOffEntry
			{
				on = false
			};
			data.AddDataEntry(playingEntry);
		}
		else
		{
			on = playingEntry.on;
		}
	}

	private void Update()
	{
		if (!ready)
		{
			return;
		}
		counter += Time.deltaTime;
		if (counter > 0.25f && isHeldByMe && !Player.localPlayer.HasLockedInput() && GlobalInputHandler.CanTakeInput())
		{
			if (Player.localPlayer.input.clickWasPressed)
			{
				counter = 0f;
				playingEntry.on = !playingEntry.on;
				selectionEntry.SetDirty();
			}
			int selectedValue = selectionEntry.selectedValue;
			float axisRaw = Input.GetAxisRaw("Mouse ScrollWheel");
			if (axisRaw > 0f || m_cameraZoomOut.action.WasPressedThisFrame())
			{
				selectionEntry.selectedValue--;
			}
			else if (axisRaw < 0f || m_cameraZoomIn.action.WasPressedThisFrame())
			{
				selectionEntry.selectedValue++;
			}
			if (selectionEntry.selectedValue != selectedValue)
			{
				selectionEntry.selectedValue = Mathf.Clamp(selectionEntry.selectedValue, 0, selectionEntry.maxValue - 1);
				selectionEntry.SetDirty();
			}
		}
		if (currentNr != selectionEntry.selectedValue)
		{
			currentNr = selectionEntry.selectedValue;
		}
		numberText.text = currentNr + 1 + "/" + selectionEntry.maxValue;
		text.text = sounds[selectionEntry.selectedValue].name;
		if (playingEntry != null && on != playingEntry.on)
		{
			sounds[selectionEntry.selectedValue].Play(base.transform.position, local: false, 1f, base.transform);
			on = playingEntry.on;
		}
		spinner.transform.localEulerAngles = new Vector3(-90 - currentNr * 10, 0f, 0f);
	}
}
