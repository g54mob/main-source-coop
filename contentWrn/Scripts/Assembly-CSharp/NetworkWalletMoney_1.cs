using System;
using System.Collections.Generic;

public class NetworkWalletMoney_1 : NetworkDealBase
{
	public override List<DIFFICULTY> AllowedDifficulties => new List<DIFFICULTY>
	{
		DIFFICULTY.easy,
		DIFFICULTY.medium,
		DIFFICULTY.hard,
		DIFFICULTY.veryHard
	};

	public override string DealName => "MoneyMoney Net";

	public override string EmailTitle => "We're looking for someone rich..";

	public override string EmailAddress => "NewWorldCapital@OldMoney.nw";

	public override RARITY Rarity => RARITY.common;

	public override bool UseInGame => true;

	public override List<string> PersistentIdOfItem => new List<string> { "90ce3757-e4c7-4ced-bb1d-acd0299b395a" };

	public override string IconPath => "Saving-Piggy-Bank--Streamline-Ultimate";

	public override byte GetIndex()
	{
		return GetIndex<NetworkWalletMoney_1>();
	}

	public override void OnDestroy()
	{
	}

	public override void OnStatusUpdated()
	{
		if (SurfaceNetworkHandler.RoomStats != null)
		{
			base.ProgressInt = SurfaceNetworkHandler.RoomStats.Money;
		}
	}

	public override float GetProgress()
	{
		return (float)base.ProgressInt / (float)RequiredAmount();
	}

	public override void OnRemovedMoney(int removedAmount)
	{
		base.ProgressInt = SurfaceNetworkHandler.RoomStats.Money;
	}

	public override string GetSuccessEmailBody()
	{
		string successEmailBody_Localized = base.SuccessEmailBody_Localized;
		if (!string.IsNullOrEmpty(successEmailBody_Localized))
		{
			return successEmailBody_Localized;
		}
		return "Your wealth has been noted and your reward is on the way!";
	}

	public override string GetFailedEmailBody()
	{
		return "";
	}

	public override string DealDescription()
	{
		int num = RequiredAmount();
		string description_Localized = base.Description_Localized;
		if (string.IsNullOrEmpty(description_Localized))
		{
			return $"We have some wealth to share. \nHowever we only want to give it to someone we KNOW is rich.{Environment.NewLine}Have a total of <b>${num}</b> to claim your reward";
		}
		return description_Localized.Replace("{Amount}", num.ToString()).Replace("\\n", Environment.NewLine);
	}

	public override NetworkDealBase CreateNew()
	{
		return new NetworkWalletMoney_1();
	}

	public override int RequiredAmount()
	{
		int num = 0;
		return difficulty switch
		{
			DIFFICULTY.easy => GetAmountByRun(1), 
			DIFFICULTY.medium => GetAmountByRun(2), 
			DIFFICULTY.hard => GetAmountByRun(3), 
			DIFFICULTY.veryHard => GetAmountByRun(4), 
			_ => throw new ArgumentOutOfRangeException(), 
		};
		static int GetAmountByRun(int run)
		{
			return (int)((float)BigNumbers.GetMoneyFromScoreByRun(BigNumbers.GetQuota(run), run) * 1f);
		}
	}

	public override void Update()
	{
		if (SurfaceNetworkHandler.RoomStats != null)
		{
			base.ProgressInt = SurfaceNetworkHandler.RoomStats.Money;
		}
	}
}
