using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NetworkLongClip_6 : NetworkDealBase
{
	private int totalViews;

	public override List<DIFFICULTY> AllowedDifficulties => new List<DIFFICULTY>
	{
		DIFFICULTY.easy,
		DIFFICULTY.medium,
		DIFFICULTY.hard,
		DIFFICULTY.veryHard
	};

	public override string DealName => "Slow Net";

	public override string EmailTitle => "We need a slow video";

	public override string EmailAddress => "";

	public override List<string> PersistentIdOfItem { get; }

	public int TotalViews
	{
		get
		{
			return totalViews;
		}
		set
		{
			if (totalViews != value)
			{
				totalViews = value;
				base.ProgressInt = Mathf.RoundToInt(Mathf.Clamp01((float)TotalViews / (float)RequiredAmount()) * 100f);
			}
		}
	}

	public override bool UseInGame => false;

	public override string IconPath => "Video-Edit-Clock--Streamline-Ultimate";

	public override RARITY Rarity => RARITY.common;

	public override byte GetIndex()
	{
		return GetIndex<NetworkLongClip_6>();
	}

	public override float GetProgress()
	{
		return (float)base.ProgressInt / (float)RequiredAmount();
	}

	public override string DealDescription()
	{
		int num = RequiredAmount();
		return $"Reach day {num} without uploading a video with a clip shorter than 10 seconds";
	}

	public override void OnDestroy()
	{
	}

	public override NetworkDealBase CreateNew()
	{
		return new NetworkLongClip_6();
	}

	public override void ReviewUploadContent(ContentBuffer contentBuffer)
	{
		base.ReviewUploadContent(contentBuffer);
		Debug.Log("Running By Bingo Bongo");
		List<ContentEvent> list = contentBuffer.buffer.Select((ContentBuffer.BufferedContent content) => content.frame.contentEvent).ToList();
		foreach (ContentBuffer.BufferedContent item in contentBuffer.buffer)
		{
			Debug.Log("Content " + item.frame.contentEvent.GetName());
			Debug.Log($"Time {item.frame.time}");
		}
		foreach (ContentEvent item2 in list)
		{
			Debug.Log(item2.GetName());
			Debug.Log(item2.GetID());
		}
		foreach (ContentBuffer.BufferedContent item3 in contentBuffer.buffer)
		{
			if (item3.frame.contentEvent is PlayerDeadContentEvent)
			{
				Debug.Log($"failed at {item3.frame.contentEvent.GetName()} ID: {item3.frame.contentEvent.GetID()}");
				base.State = DEAL_STATE.failed;
			}
		}
	}

	public override string GetSuccessEmailBody()
	{
		return "LONG CLIPS";
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
			DIFFICULTY.easy => 1, 
			DIFFICULTY.medium => 2, 
			DIFFICULTY.hard => 3, 
			DIFFICULTY.veryHard => 4, 
			_ => throw new ArgumentOutOfRangeException(), 
		};
	}

	public override void Update()
	{
	}
}
