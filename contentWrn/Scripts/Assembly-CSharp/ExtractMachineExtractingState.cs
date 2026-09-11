using System.Collections.Generic;
using UnityEngine;

public class ExtractMachineExtractingState : VideoExtractMachineState
{
	public ExtractMachineExtractingState(ExtractVideoMachine machine)
		: base(machine)
	{
	}

	public override void Enter()
	{
		base.Enter();
		if (base.Machine.m_hasCDInRom.IsSome)
		{
			Debug.LogError("Already has CD in rom");
			base.Machine.StateMachine.SwitchState<ExtractMachineEjectState>();
			return;
		}
		if (base.Machine.AlreadyExtracting)
		{
			Debug.LogError("Already extracting");
			base.Machine.StateMachine.SwitchState<ExtractMachineEjectState>();
			return;
		}
		List<(Item, Pickup)> list = base.Machine.Detector.CheckForItems();
		if (list.Count != 1 || list[0].Item1.itemType != Item.ItemType.Camera)
		{
			base.Machine.StateMachine.SwitchState<ExtractMachineEjectState>();
			return;
		}
		Pickup item = list[0].Item2;
		ItemInstanceData o;
		VideoInfoEntry t;
		if (!item.itemInstance.m_guid.IsSome)
		{
			Debug.LogError("Camera Pickup has no guid");
			base.Machine.StateMachine.SwitchState<ExtractMachineEjectState>();
		}
		else if (!ItemInstanceDataHandler.TryGetInstanceData(item.itemInstance.m_guid.Value, out o))
		{
			Debug.LogError("No data found for camera");
			base.Machine.StateMachine.SwitchState<ExtractMachineEjectState>();
		}
		else if (!o.TryGetEntry<VideoInfoEntry>(out t))
		{
			Debug.LogError("No VideoInfoEntry found for camera");
			base.Machine.StateMachine.SwitchState<ExtractMachineEjectState>();
		}
		else if (t.videoID.Equals(VideoHandle.Invalid))
		{
			Debug.LogError("VideoID is invalid");
			base.Machine.StateMachine.SwitchState<ExtractMachineEjectState>();
		}
		else
		{
			item.RPC_Remove();
			bool isBrokenCamera = item.itemInstance.item.name.ToLower().Contains("camerabroken");
			base.Machine.StartExtract(t.videoID, isBrokenCamera);
		}
	}
}
