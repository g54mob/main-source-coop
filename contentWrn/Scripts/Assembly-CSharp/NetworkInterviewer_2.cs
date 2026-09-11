using System;
using System.Collections.Generic;

public class NetworkInterviewer_2 : NetworkDealBase
{
	[Serializable]
	public class SerializedNetworkInterviewer_2 : SerializedNetworkDeal
	{
		public List<int> interviewedMonsterIDs = new List<int>();
	}

	public List<int> interviewedMonsterIDs = new List<int>();

	public override List<DIFFICULTY> AllowedDifficulties => new List<DIFFICULTY>
	{
		DIFFICULTY.easy,
		DIFFICULTY.medium,
		DIFFICULTY.hard,
		DIFFICULTY.veryHard
	};

	public override string DealName => "Reporter Net";

	public override string EmailTitle => "Media inquiry: Monster deep dive.";

	public override string EmailAddress => "Outreach@ReporterNet.nw";

	public override List<string> PersistentIdOfItem { get; }

	public override bool UseInGame => true;

	public override string IconPath => "Microphone-Podcast-Man--Streamline-Ultimate";

	public override RARITY Rarity => RARITY.common;

	public override byte GetIndex()
	{
		return GetIndex<NetworkInterviewer_2>();
	}

	public override string DealDescription()
	{
		string text = BigNumbers.ViewsToString(RequiredAmount());
		string description_Localized = base.Description_Localized;
		if (string.IsNullOrEmpty(description_Localized))
		{
			return "upload video of you interviewing " + text + " different monsters with the reporter mic";
		}
		return description_Localized.Replace("{Views}", text);
	}

	public override void OnDestroy()
	{
	}

	public override NetworkDealBase CreateNew()
	{
		return new NetworkInterviewer_2();
	}

	public override void ReviewUploadContent(ContentBuffer contentBuffer)
	{
		base.ReviewUploadContent(contentBuffer);
		foreach (ContentBuffer.BufferedContent item in contentBuffer.buffer)
		{
			if (item.frame.contentEvent is InterviewEvent interviewEvent && !interviewedMonsterIDs.Contains(interviewEvent.monsterID))
			{
				interviewedMonsterIDs.Add(interviewEvent.monsterID);
				base.ProgressInt++;
			}
		}
	}

	public override string GetSuccessEmailBody()
	{
		string successEmailBody_Localized = base.SuccessEmailBody_Localized;
		if (!string.IsNullOrEmpty(successEmailBody_Localized))
		{
			return successEmailBody_Localized;
		}
		return "Thank you for interviewing the creatures of the old world!";
	}

	public override string GetFailedEmailBody()
	{
		return "";
	}

	public override int RequiredAmount()
	{
		int num = 0;
		return difficulty switch
		{
			DIFFICULTY.easy => 5, 
			DIFFICULTY.medium => 10, 
			DIFFICULTY.hard => 15, 
			DIFFICULTY.veryHard => 20, 
			_ => throw new ArgumentOutOfRangeException(), 
		};
	}

	public override void Update()
	{
	}

	public override SerializedNetworkDeal GetSerialized()
	{
		SerializedNetworkInterviewer_2 serializedNetworkInterviewer_ = new SerializedNetworkInterviewer_2
		{
			interviewedMonsterIDs = interviewedMonsterIDs
		};
		FillSerializedData(serializedNetworkInterviewer_);
		return serializedNetworkInterviewer_;
	}
}
