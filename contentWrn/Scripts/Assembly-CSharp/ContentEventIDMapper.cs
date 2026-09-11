using System;
using DefaultNamespace.ContentEvents;

public static class ContentEventIDMapper
{
	private static ContentEvent GetContentEventGenerated(ushort id)
	{
		return id switch
		{
			1012 => new BarnacleBallContentEvent(), 
			1002 => new BigSlapAgroContentEvent(), 
			1001 => new BigSlapPeacefulContentEvent(), 
			1043 => new BlackHoleBotContentEvent(), 
			1030 => new BombContentEvent(), 
			1017 => new BombsContentEvent(), 
			1026 => new CamCreepContentEvent(), 
			1024 => new DogContentEvent(), 
			1008 => new EarContentEvent(), 
			1025 => new EyeGuyContentEvent(), 
			1041 => new FireMonsterContentEvent(), 
			1004 => new FlickerContentEvent(), 
			1027 => new GoodCatchContentEvent(), 
			1037 => new HarpoonerContentEvent(), 
			1033 => new InterviewEvent(), 
			1005 => new JelloContentEvent(), 
			1006 => new KnifoContentEvent(), 
			1018 => new LarvaContentEvent(), 
			1045 => new MimeContentEvent(), 
			1009 => new MouthContentEvent(), 
			1029 => new MultiMonsterEvent(), 
			1000 => new PlayerContentEvent(), 
			1020 => new PlayerDeadContentEvent(), 
			1023 => new PlayerEmoteContentEvent(), 
			1022 => new PlayerFallingContentEvent(), 
			1032 => new PlayerHoldingMicContentEvent(), 
			1021 => new PlayerRagdollContentEvent(), 
			1028 => new PlayerTookDamageContentEvent(), 
			1044 => new PlayerWearingHatContentEvent(), 
			1042 => new PuffoContentEvent(), 
			1035 => new RobotButtonContentEvent(), 
			1031 => new PlayerShroomContentEvent(), 
			1010 => new SlurperContentEvent(), 
			1040 => new SnailSpawnerContentEvent(), 
			1011 => new SnatchoContentEvent(), 
			1019 => new SpiderContentEvent(), 
			1038 => new StreamerContentEvent(), 
			1034 => new TauntEvent(), 
			1013 => new ToolkitWhiskContentEvent(), 
			1036 => new WalloContentEvent(), 
			1007 => new WeepingContentEvent(), 
			1014 => new WeepingContentEventCaptured(), 
			1015 => new WeepingContentEventFail(), 
			1016 => new WeepingContentEventSuccess(), 
			1039 => new WormContentEvent(), 
			1003 => new ZombieContentEvent(), 
			_ => null, 
		};
	}

	public static ContentEvent GetContentEvent(ushort id)
	{
		ContentEvent contentEventGenerated = GetContentEventGenerated(id);
		if (contentEventGenerated != null)
		{
			return contentEventGenerated;
		}
		PropContent entryFromID = PropContentDatabase.GetEntryFromID(id);
		if (entryFromID != null)
		{
			if (entryFromID.isArtifact)
			{
				return new ArtifactContentEvent
				{
					content = entryFromID
				};
			}
			return new PropContentEvent
			{
				content = entryFromID
			};
		}
		throw new Exception("Unknown content event id: " + id);
	}
}
