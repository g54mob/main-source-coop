using Photon.Pun;

public class DealRewardMeta_2 : DealRewardBase
{
	private string Reward_Text_Localized;

	public override bool UseInGame => true;

	public override DealRewardBase CreateNew()
	{
		return new DealRewardMeta_2();
	}

	public override string GetRewardDescription()
	{
		return Reward_Text_Localized.Replace("{amount}", GetAmount().ToString());
	}

	public int GetAmount()
	{
		return BigNumbers.GetMetaBaseScoreFromQuest(difficulty);
	}

	public override bool ClaimReward()
	{
		MetaProgressionHandler.AddMetaCoins(GetAmount());
		if (PhotonNetwork.IsMasterClient && SaveSystem.CurrentSave != null)
		{
			SurgeryOnASave.PerformSurgery(SaveSystem.CurrentSave);
		}
		return true;
	}

	public override byte GetIndex()
	{
		return GetIndex<DealRewardMeta_2>();
	}

	public override string GetRewardClaimDescription()
	{
		return $"We've sent you <b><u>{GetAmount()} meta points</b>, click the button to claim them.";
	}

	public override void MakeLocalized()
	{
		Reward_Text_Localized = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.Deal_Reward_Meta);
	}
}
