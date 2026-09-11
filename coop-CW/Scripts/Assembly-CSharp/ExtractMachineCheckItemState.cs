using System.Collections.Generic;
using UnityEngine;
using Zorro.Core;

public class ExtractMachineCheckItemState : VideoExtractMachineState
{
	private Optionable<float> m_timeBeenSearchingForItem;

	public ExtractMachineCheckItemState(ExtractVideoMachine machine)
		: base(machine)
	{
	}

	public override void Enter()
	{
		base.Enter();
		m_timeBeenSearchingForItem = Optionable<float>.None;
	}

	public override void Update()
	{
		base.Update();
		if (base.Machine.Hatch.m_opened)
		{
			base.Machine.Hatch.Close();
		}
		if (!base.Machine.Hatch.IsFullyClosed())
		{
			return;
		}
		if (m_timeBeenSearchingForItem.IsNone)
		{
			m_timeBeenSearchingForItem = Optionable<float>.Some(0f);
		}
		m_timeBeenSearchingForItem = Optionable<float>.Some(m_timeBeenSearchingForItem.Value + Time.deltaTime);
		if (!(m_timeBeenSearchingForItem.Value > 0.3f))
		{
			return;
		}
		if (m_timeBeenSearchingForItem.Value < 1.5f)
		{
			List<(Item, Pickup)> list = base.Machine.Detector.CheckForItems();
			if (list.Count == 1 && list[0].Item1.itemType == Item.ItemType.Camera)
			{
				Debug.Log("Video extraction machine found camera");
				base.Machine.StateMachine.SwitchState<ExtractMachineExtractingState>();
			}
			else
			{
				Debug.Log("Video extraction machine found something else");
				base.Machine.StateMachine.SwitchState<ExtractMachineEjectState>();
			}
		}
		else
		{
			base.Machine.StateMachine.SwitchState<ExtractMachineEjectState>();
		}
	}
}
