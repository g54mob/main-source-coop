using UnityEngine;

namespace NomadDrive.Features.LiquidTransferSystem
{
	public interface ILiquidSnapTarget
	{
		Transform FillAnchor { get; }

		LiquidSnapTargetType TargetType { get; }

		uint SnapNetId { get; }
	}
}
