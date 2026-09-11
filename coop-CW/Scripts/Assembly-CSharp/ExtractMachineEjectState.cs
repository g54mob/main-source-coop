using System.Collections.Generic;
using UnityEngine;

public class ExtractMachineEjectState : VideoExtractMachineState
{
	public ExtractMachineEjectState(ExtractVideoMachine machine)
		: base(machine)
	{
	}

	public override void FixedUpdate()
	{
		base.FixedUpdate();
		if (!base.Machine.Hatch.m_opened)
		{
			base.Machine.Hatch.Open();
		}
		List<(Item, Pickup)> list = base.Machine.Detector.CheckForItems();
		if (list.Count == 0)
		{
			base.Machine.StateMachine.SwitchState<ExtractMachineIdleState>();
		}
		else
		{
			if (!base.Machine.Hatch.IsHalfwayOpen())
			{
				return;
			}
			foreach (var item2 in list)
			{
				Pickup item = item2.Item2;
				if (!(item.Rigidbody.linearVelocity.y > 0.1f))
				{
					item.Rigidbody.AddTorque(Vector3.right * 3f, ForceMode.Impulse);
					item.Rigidbody.AddForce(Vector3.up * 25f + -base.Machine.transform.forward * 6f, ForceMode.Impulse);
				}
			}
		}
	}
}
