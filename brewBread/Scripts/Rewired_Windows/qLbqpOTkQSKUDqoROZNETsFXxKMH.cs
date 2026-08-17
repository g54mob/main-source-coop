internal class qLbqpOTkQSKUDqoROZNETsFXxKMH
{
	public static class CraGIyBcrwwXcunOaTrTaLxneLiIA
	{
		public enum GaTHNNvxYqJyyWKQSzWCJvSKDBKD : ushort
		{
			Undefined = 0,
			Pointer = 1,
			Mouse = 2,
			Reserved = 3,
			Joystick = 4,
			GamePad = 5,
			Keyboard = 6,
			Keypad = 7,
			MultiAxisController = 8,
			TabletPCSystemControls = 9,
			X = 48,
			Y = 49,
			Z = 50,
			Rx = 51,
			Ry = 52,
			Rz = 53,
			Slider = 54,
			Dial = 55,
			Wheel = 56,
			HatSwitch = 57,
			CountedBuffer = 58,
			ByteCount = 59,
			MotionWakeup = 60,
			Start = 61,
			Select = 62,
			Vx = 64,
			Vy = 65,
			Vz = 66,
			Vbrx = 67,
			Vbry = 68,
			Vbrz = 69,
			Vno = 70,
			SystemControl = 128,
			SystemPowerDown = 129,
			SystemSleep = 130,
			SystemWakeUp = 131,
			SystemContextMenu = 132,
			SystemMainMenu = 133,
			SystemAppMenu = 134,
			SystemMenuHelp = 135,
			SystemMenuExit = 136,
			SystemMenuSelect = 137,
			SystemMenuRight = 138,
			SystemMenuLeft = 139,
			SystemMenuUp = 140,
			SystemMenuDown = 141,
			SystemColdRestart = 142,
			SystemWarmRestart = 143,
			DPadUp = 144,
			DPadDown = 145,
			DPadRight = 146,
			DPadLeft = 147,
			SystemDock = 160,
			SystemUndock = 161,
			SystemSetup = 162,
			SystemBreak = 163,
			SystemDebuggerBreak = 164,
			ApplicationBreak = 166,
			ApplicationDebuggerBreak = 166,
			SystemSpeakerMute = 167,
			SystemHibernate = 168,
			SystemDisplayInvert = 176,
			SystemDisplayInternal = 177,
			SystemDisplayExternal = 178,
			SystemDisplayBoth = 179,
			SystemDisplayDual = 180,
			SystemDisplayToggleIntExt = 181,
			SystemDisplaySwapPrimarySecondary = 182,
			SystemDisplayLCDAutoscale = 183
		}

		public static bool eQZBqkGlvQvrLHtkptlwJUiQhhRA(ushort P_0)
		{
			if ((uint)(P_0 - 4) <= 1u || P_0 == 8)
			{
				return true;
			}
			return false;
		}

		public static bool pZzGxPeBYbVdXxNuCKzndrnnqdDQ(ushort P_0)
		{
			if (P_0 < 48 || P_0 > 70)
			{
				return false;
			}
			if ((P_0 >= 48 && P_0 <= 56) || (P_0 >= 64 && P_0 <= 70))
			{
				return true;
			}
			return false;
		}

		public static bool abCHewFKwbvVtsTgPnjeKGBGAEYkA(ushort P_0)
		{
			if (P_0 < 61 || P_0 > 147)
			{
				return false;
			}
			switch (P_0)
			{
			case 61:
			case 62:
			case 144:
			case 145:
			case 146:
			case 147:
				return true;
			default:
				return false;
			}
		}
	}

	public static class QlDPyihPegWFqgbbfDiYCvRNpNJpA
	{
		public enum LscWhgofJVNHsWOxFKVJXkyaJLZx : ushort
		{
			Aileron = 176,
			AileronTrim = 177,
			AntiTorqueControl = 178,
			AutopilotEnable = 179,
			ChaffRelease = 180,
			CollectiveControl = 181,
			DriveBrake = 182,
			ElectronicCountermeasures = 183,
			Elevator = 184,
			ElevatorTrim = 185,
			Rudder = 186,
			Throttle = 187,
			FlightCommunication = 188,
			FlareRelease = 189,
			LandingGear = 190,
			ToeBrake = 191,
			Trigger = 192,
			WeaponsAim = 193,
			WeaponsSelect = 194,
			WingFlaps = 195,
			Accelerator = 196,
			Brake = 197,
			Clutch = 198,
			Shifter = 199,
			Steering = 200,
			TurretDirection = 201,
			BarrelElevation = 202,
			DivePlane = 203,
			Ballast = 204,
			BicycleCrank = 205,
			HandleBars = 206,
			FrontBrake = 207,
			RearBrake = 208
		}

		public static bool pZzGxPeBYbVdXxNuCKzndrnnqdDQ(ushort P_0)
		{
			if (P_0 < 176 || P_0 > 208)
			{
				return false;
			}
			if (P_0 < 176 || P_0 > 178)
			{
				switch (P_0)
				{
				default:
					switch (P_0)
					{
					case 191:
					case 195:
					case 196:
					case 197:
					case 198:
					case 199:
					case 200:
					case 201:
					case 202:
					case 203:
					case 204:
					case 205:
					case 206:
					case 207:
					case 208:
						break;
					default:
						return false;
					}
					break;
				case 181:
				case 182:
				case 184:
				case 185:
				case 186:
				case 187:
					break;
				}
			}
			return true;
		}

		public static bool abCHewFKwbvVtsTgPnjeKGBGAEYkA(ushort P_0)
		{
			switch (P_0)
			{
			default:
				return false;
			case 181:
			case 182:
			case 184:
			case 185:
			case 186:
			case 187:
			case 191:
			case 192:
			case 193:
			case 194:
				if (P_0 < 192 || P_0 > 194)
				{
					break;
				}
				goto case 179;
			case 179:
			case 180:
			case 183:
			case 188:
			case 189:
			case 190:
				return true;
			}
			return false;
		}
	}

	public static class EyxxFBfOcVEoRUAnHqFUPyKjteYN
	{
		public enum tuLVqmcBWAKNovBOuHBKgLPsybPt : ushort
		{
			ConsumerControl = 1,
			NumericKeyPad = 2,
			ProgrammableButtons = 3,
			Plus10 = 32,
			Plus100 = 33,
			AMOrPM = 34,
			Power = 48,
			Reset = 49,
			Sleep = 50,
			SleepAfter = 51,
			SleepMode = 52,
			Illumination = 53,
			FunctionButtons = 54,
			Menu = 64,
			MenuPick = 65,
			MenuUp = 66,
			MenuDown = 67,
			MenuLeft = 68,
			MenuRight = 69,
			MenuEscape = 70,
			MenuValueIncrease = 71,
			MenuValueDecrease = 72,
			DataOnScreen = 96,
			ClosedCaption = 97,
			ClosedCaptionSelect = 98,
			VCROrTV = 99,
			BroadcastMode = 100,
			Snapshot = 101,
			Still = 102,
			Selection = 128,
			Assign = 129,
			ModeStep = 130,
			RecallLast = 131,
			EnterChannel = 132,
			OrderMovie = 133,
			Channel = 134,
			MediaSelection = 135,
			MediaSelectComputer = 136,
			MediaSelectTV = 137,
			MediaSelectWWW = 138,
			MediaSelectDVD = 139,
			MediaSelectTelephone = 140,
			MediaSelectProgramGuide = 141,
			MediaSelectVideoPhone = 142,
			MediaSelectGames = 143,
			MediaSelectMessages = 144,
			MediaSelectCD = 145,
			MediaSelectVCR = 146,
			MediaSelectTuner = 147,
			Quit = 148,
			Help = 149,
			MediaSelectTape = 150,
			MediaSelectCable = 151,
			MediaSelectSatellite = 152,
			MediaSelectSecurity = 153,
			MediaSelectHome = 154,
			MediaSelectCall = 155,
			ChannelIncrement = 156,
			ChannelDecrement = 157,
			Media = 158,
			VCRPlus = 160,
			Once = 161,
			Daily = 162,
			Weekly = 163,
			Monthly = 164,
			Play = 176,
			Pause = 177,
			Record = 178,
			FastForward = 179,
			Rewind = 180,
			ScanNextTrack = 181,
			ScanPreviousTrack = 182,
			Stop = 183,
			Eject = 184,
			RandomPlay = 185,
			SelectDisc = 186,
			EnterDisc = 187,
			Repeat = 188,
			Tracking = 189,
			TrackNormal = 190,
			SlowTracking = 191,
			FrameForward = 192,
			FrameBack = 193,
			Mark = 194,
			ClearMark = 195,
			RepeatFromMark = 196,
			ReturnToMark = 197,
			SearchMarkForward = 198,
			SearchMarkBackwards = 199,
			CounterReset = 200,
			ShowCounter = 201,
			TrackingIncrement = 202,
			TrackingDecrement = 203,
			StopOrEject = 204,
			PlayOrPause = 205,
			PlayOrSkip = 206,
			Volume = 224,
			Balance = 225,
			Mute = 226,
			Bass = 227,
			Treble = 228,
			BassBoost = 229,
			SurroundMode = 230,
			Loudness = 231,
			MPX = 232,
			VolumeIncrement = 233,
			VolumeDecrement = 234,
			Speed = 240,
			PlaybackSpeed = 241,
			StandardPlay = 242,
			LongPlay = 243,
			ExtendedPlay = 244,
			Slow = 245,
			FanEnable = 256,
			FanSpeed = 257,
			LightEnable = 258,
			LightIlluminationLevel = 259,
			ClimateControlEnable = 260,
			RoomTemperature = 261,
			SecurityEnable = 262,
			FireAlarm = 263,
			PoliceAlarm = 264,
			BalanceRight = 336,
			BalanceLeft = 337,
			BassIncrement = 338,
			BassDecrement = 339,
			TrebleIncrement = 340,
			TrebleDecrement = 341,
			SpeakerSystem = 352,
			ChannelLeft = 353,
			ChannelRight = 354,
			ChannelCenter = 355,
			ChannelFront = 356,
			ChannelCenterFront = 357,
			ChannelSide = 358,
			ChannelSurround = 359,
			ChannelLowFrequencyEnhancement = 360,
			ChannelTop = 361,
			ChannelUnknown = 362,
			SubChannel = 368,
			SubChannelIncrement = 369,
			SubChannelDecrement = 370,
			AlternateAudioIncrement = 371,
			AlternateAudioDecrement = 372,
			ApplicationLaunchButtons = 384,
			ALLaunchButtonConfigurationTool = 385,
			ALProgrammableButtonConfiguration = 386,
			ALConsumerControlConfiguration = 387,
			ALWordProcessor = 388,
			ALTextEditor = 389,
			ALSpreadsheet = 390,
			ALGraphicsEditor = 391,
			ALPresentationApp = 392,
			ALDatabaseApp = 393,
			ALEmailReader = 394,
			ALNewsreader = 395,
			ALVoicemail = 396,
			ALContactsOrAddressBook = 397,
			ALCalendarOrSchedule = 398,
			ALTaskOrProjectManager = 399,
			ALLogOrJournalOrTimecard = 400,
			ALCheckbookOrFinance = 401,
			ALCalculator = 402,
			ALAOrVCaptureOrPlayback = 403,
			ALLocalMachineBrowser = 404,
			ALLANOrWANBrowser = 405,
			ALInternetBrowser = 406,
			ALRemoteNetworkingOrISPConnect = 407,
			ALNetworkConference = 408,
			ALNetworkChat = 409,
			ALTelephonyOrDialer = 410,
			ALLogon = 411,
			ALLogoff = 412,
			ALLogonOrLogoff = 413,
			ALTerminalLockOrScreensaver = 414,
			ALControlPanel = 415,
			ALCommandLineProcessorOrRun = 416,
			ALProcessOrTaskManager = 417,
			AL = 418,
			ALNextTaskOrApplication = 323,
			ALPreviousTaskOrApplication = 420,
			ALPreemptiveHaltTaskOrApplication = 421,
			GenericGUIApplicationControls = 512,
			ACNew = 513,
			ACOpen = 514,
			ACClose = 515,
			ACExit = 516,
			ACMaximize = 517,
			ACMinimize = 518,
			ACSave = 519,
			ACPrint = 520,
			ACProperties = 521,
			ACUndo = 538,
			ACCopy = 539,
			ACCut = 540,
			ACPaste = 541,
			AC = 542,
			ACFind = 543,
			ACFindandReplace = 544,
			ACSearch = 545,
			ACGoTo = 546,
			ACHome = 547,
			ACBack = 548,
			ACForward = 549,
			ACStop = 550,
			ACRefresh = 551,
			ACPreviousLink = 552,
			ACNextLink = 553,
			ACBookmarks = 554,
			ACHistory = 555,
			ACSubscriptions = 556,
			ACZoomIn = 557,
			ACZoomOut = 558,
			ACZoom = 559,
			ACFullScreenView = 560,
			ACNormalView = 561,
			ACViewToggle = 562,
			ACScrollUp = 563,
			ACScrollDown = 564,
			ACScroll = 565,
			ACPanLeft = 566,
			ACPanRight = 567,
			ACPan = 568,
			ACNewWindow = 569,
			ACTileHorizontally = 570,
			ACTileVertically = 571,
			ACFormat = 572,
			Reserved = ushort.MaxValue
		}

		public static bool pZzGxPeBYbVdXxNuCKzndrnnqdDQ(ushort P_0)
		{
			return false;
		}

		public static bool abCHewFKwbvVtsTgPnjeKGBGAEYkA(ushort P_0)
		{
			if ((uint)P_0 <= 264u)
			{
				if ((uint)P_0 <= 206u)
				{
					switch (P_0)
					{
					case 32:
					case 33:
					case 34:
					case 48:
					case 49:
					case 50:
					case 51:
					case 52:
					case 53:
					case 64:
					case 65:
					case 66:
					case 67:
					case 68:
					case 69:
					case 70:
					case 71:
					case 72:
					case 96:
					case 97:
					case 98:
					case 99:
					case 100:
					case 101:
					case 102:
					case 129:
					case 130:
					case 131:
					case 132:
					case 133:
					case 135:
					case 136:
					case 137:
					case 138:
					case 139:
					case 140:
					case 141:
					case 142:
					case 143:
					case 144:
					case 145:
					case 146:
					case 147:
					case 148:
					case 149:
					case 150:
					case 151:
					case 152:
					case 153:
					case 154:
					case 155:
					case 156:
					case 157:
					case 158:
					case 160:
					case 161:
					case 162:
					case 163:
					case 164:
					case 176:
					case 177:
					case 178:
					case 179:
					case 180:
					case 181:
					case 182:
					case 183:
					case 184:
					case 185:
					case 187:
					case 188:
					case 190:
					case 192:
					case 193:
					case 194:
					case 195:
					case 196:
					case 197:
					case 198:
					case 199:
					case 200:
					case 201:
					case 202:
					case 203:
					case 204:
					case 205:
					case 206:
						break;
					default:
						goto IL_04ce;
					}
				}
				else
				{
					switch (P_0)
					{
					case 226:
					case 229:
					case 230:
					case 231:
					case 232:
					case 233:
					case 234:
					case 240:
					case 242:
					case 243:
					case 244:
					case 245:
					case 256:
					case 258:
					case 260:
					case 262:
					case 263:
					case 264:
						break;
					default:
						goto IL_04ce;
					}
				}
			}
			else
			{
				switch (P_0)
				{
				case 323:
				case 336:
				case 337:
				case 338:
				case 339:
				case 340:
				case 341:
				case 369:
				case 370:
				case 371:
				case 372:
				case 385:
				case 386:
				case 387:
				case 388:
				case 389:
				case 390:
				case 391:
				case 392:
				case 393:
				case 394:
				case 395:
				case 396:
				case 397:
				case 398:
				case 399:
				case 400:
				case 401:
				case 402:
				case 403:
				case 404:
				case 405:
				case 406:
				case 407:
				case 408:
				case 409:
				case 410:
				case 411:
				case 412:
				case 413:
				case 414:
				case 415:
				case 416:
				case 417:
				case 418:
				case 420:
				case 421:
				case 513:
				case 514:
				case 515:
				case 516:
				case 517:
				case 518:
				case 519:
				case 520:
				case 521:
				case 538:
				case 539:
				case 540:
				case 541:
				case 542:
				case 543:
				case 544:
				case 545:
				case 546:
				case 547:
				case 548:
				case 549:
				case 550:
				case 551:
				case 552:
				case 553:
				case 554:
				case 555:
				case 556:
				case 557:
				case 558:
				case 559:
				case 560:
				case 561:
				case 562:
				case 563:
				case 564:
				case 565:
				case 566:
				case 567:
				case 568:
				case 569:
				case 570:
				case 571:
				case 572:
					break;
				default:
					goto IL_04ce;
				}
			}
			return true;
			IL_04ce:
			return false;
		}
	}

	public static class AoMPgcQQkIFGtwIKARLLHaRufjlEA
	{
		public enum eLSenDikwRkmUEBFcNJWJjFkHUYvA : ushort
		{
			_3DGameController = 1,
			PinballDevice = 2,
			GunDevice = 3,
			PointofView = 32,
			TurnRightOrLeft = 33,
			PitchUpOrDown = 34,
			RollRightOrLeft = 35,
			MoveRightOrLeft = 36,
			MoveForwardOrBackward = 37,
			MoveUpOrDown = 38,
			LeanRightOrLeft = 39,
			LeanForwardOrBackward = 40,
			HeightOfPOV = 41,
			Flipper = 42,
			SecondaryFlipper = 43,
			Bump = 44,
			NewGame = 45,
			ShootBall = 46,
			Player = 47,
			GunBolt = 48,
			GunClip = 49,
			Gun = 50,
			GunSingleShot = 51,
			GunBurst = 52,
			GunAutomatic = 53,
			GunSafety = 54,
			GamepadFireOrJump = 55,
			GamepadTrigger = 57,
			Reserved = ushort.MaxValue
		}

		public static bool pZzGxPeBYbVdXxNuCKzndrnnqdDQ(ushort P_0)
		{
			if ((uint)(P_0 - 33) <= 8u)
			{
				return true;
			}
			return false;
		}

		public static bool abCHewFKwbvVtsTgPnjeKGBGAEYkA(ushort P_0)
		{
			if ((uint)(P_0 - 42) <= 12u)
			{
				return true;
			}
			return false;
		}
	}

	public const ushort pQpATsAfPDVeKGkxtbQoedpoEDpMA = 1;

	public const ushort SuOaIZoAMmwqXVpFeOSovAUEQAnf = 2;

	public const ushort xHvEtXuFsYfYLNhJZdxVnELjNCpn = 5;

	public const ushort MdkGPxmkgueZmadoLvxxShrQGBHv = 9;

	public const ushort hyryCtUyTTkwOQNKKovYPKUOfUwe = 12;

	public static bool pZzGxPeBYbVdXxNuCKzndrnnqdDQ(ushort P_0, ushort P_1)
	{
		return P_0 switch
		{
			1 => CraGIyBcrwwXcunOaTrTaLxneLiIA.pZzGxPeBYbVdXxNuCKzndrnnqdDQ(P_1), 
			2 => QlDPyihPegWFqgbbfDiYCvRNpNJpA.pZzGxPeBYbVdXxNuCKzndrnnqdDQ(P_1), 
			5 => AoMPgcQQkIFGtwIKARLLHaRufjlEA.pZzGxPeBYbVdXxNuCKzndrnnqdDQ(P_1), 
			12 => EyxxFBfOcVEoRUAnHqFUPyKjteYN.pZzGxPeBYbVdXxNuCKzndrnnqdDQ(P_1), 
			_ => false, 
		};
	}

	public static bool qilNivTPMJaTfIdYCqINhpBecCbMB(ushort P_0, ushort P_1)
	{
		if (!pZzGxPeBYbVdXxNuCKzndrnnqdDQ(P_0, P_1))
		{
			return abCHewFKwbvVtsTgPnjeKGBGAEYkA(P_0, P_1);
		}
		return true;
	}

	public static bool abCHewFKwbvVtsTgPnjeKGBGAEYkA(ushort P_0, ushort P_1)
	{
		return P_0 switch
		{
			1 => CraGIyBcrwwXcunOaTrTaLxneLiIA.abCHewFKwbvVtsTgPnjeKGBGAEYkA(P_1), 
			2 => QlDPyihPegWFqgbbfDiYCvRNpNJpA.abCHewFKwbvVtsTgPnjeKGBGAEYkA(P_1), 
			5 => AoMPgcQQkIFGtwIKARLLHaRufjlEA.abCHewFKwbvVtsTgPnjeKGBGAEYkA(P_1), 
			9 => true, 
			12 => EyxxFBfOcVEoRUAnHqFUPyKjteYN.abCHewFKwbvVtsTgPnjeKGBGAEYkA(P_1), 
			_ => false, 
		};
	}

	public static bool sVplMQHZCEuTYnRqklEpurjJolrd(ushort P_0)
	{
		if (!orpTfrsrJUgipeOltDCqbHsMZymP(P_0))
		{
			return OCIXZaNopFQRwFarUbzCmbFHFjtO(P_0);
		}
		return true;
	}

	public static bool orpTfrsrJUgipeOltDCqbHsMZymP(ushort P_0)
	{
		if (P_0 == 1 || P_0 == 2 || P_0 == 5)
		{
			return true;
		}
		return false;
	}

	public static bool OCIXZaNopFQRwFarUbzCmbFHFjtO(ushort P_0)
	{
		if (P_0 == 9 || P_0 == 12 || P_0 == 2 || P_0 == 5)
		{
			return true;
		}
		return false;
	}

	public static bool cHQIatSCbCvMjdMjoZRFvBiVexmFA(ushort P_0, ushort P_1)
	{
		if (P_0 != 1)
		{
			return false;
		}
		return P_1 == 57;
	}
}
