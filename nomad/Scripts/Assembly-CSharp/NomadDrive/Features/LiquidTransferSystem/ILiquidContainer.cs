using Ami.BroAudio;
using Ami.BroAudio.Data;
using UnityEngine;
using UnityEngine.Events;

namespace NomadDrive.Features.LiquidTransferSystem
{
	public interface ILiquidContainer
	{
		Vector3 Position { get; }

		string Name { get; }

		SoundID TransferSound { get; set; }

		AudioParameter TransferActiveParameter { get; }

		LiquidContainerState State { get; }

		LiquidType AllowedLiquidTypes { get; }

		LiquidType CurrentLiquidType { get; set; }

		LiquidTransferType TransferType { get; set; }

		UnityEvent<float, float> OnLiquidAmountChangedEvent { get; set; }

		float CurrentAmount { get; }

		float Capacity { get; }

		float FillRatio { get; }

		float FillingSpeed { get; }

		float Fill(float amount);

		float Drain(float amount);

		void CmdSetAmount(float amount);

		void CmdSetLiquidType(LiquidType liquidType);
	}
}
