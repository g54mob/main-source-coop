using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Rewired;
using Rewired.Data;

internal class vVGxMaRQSgzzFjtXaZTAmFOGsEmm
{
	private int SaAPUknZYJjkJPNMEsCcKAPefhmIA;

	private int YNZxkqoFXUeJNFqdRCzgRSvWQlTbA;

	private Player FSqjTMGgVYIJQkaNGaXgJJjuoXyG;

	private Player[] oDFVoflNKcFbyGpcsgPOzMcRGdaJ;

	private Player[] OEVOcQyWrhpclZkiWnLxbRGlXdql;

	private IList<Player> VZbXlvjyGAAasKOcsAiyzAeIzXbe;

	private IList<Player> RwqQDyJmlWuSeNtEzhqVUlaSMdYc;

	private ConfigVars ZThjONALbmmeOCJHZqCXNLXCOMpU;

	private bool CfrrPiGKEEweWUoyCLVyrEtzTSap;

	public int VeldubbvOvGpfoCqfSOeOBOPVmbpA => SaAPUknZYJjkJPNMEsCcKAPefhmIA;

	public int dTPqHqcLdarzLjRMtjGyxsDcgqoq => YNZxkqoFXUeJNFqdRCzgRSvWQlTbA;

	public Player[] UTKiNgXnQuIZVkkphLwgVlBnhTkd => oDFVoflNKcFbyGpcsgPOzMcRGdaJ;

	public Player[] RNneRXekceTIrvqRONeevKWpudcSA => OEVOcQyWrhpclZkiWnLxbRGlXdql;

	public IList<Player> QxMAcoPmpKUqxnkzdWuGJnRqUkqh => RwqQDyJmlWuSeNtEzhqVUlaSMdYc;

	public IList<Player> rBBrAfxShCBQlDmrrfnkdCxzHJalA => VZbXlvjyGAAasKOcsAiyzAeIzXbe;

	public vVGxMaRQSgzzFjtXaZTAmFOGsEmm(ConfigVars P_0)
	{
		ZThjONALbmmeOCJHZqCXNLXCOMpU = P_0;
	}

	public void viMGrxkPlOmAmdnLgTNfCNtNoCBKc()
	{
		if (CfrrPiGKEEweWUoyCLVyrEtzTSap)
		{
			return;
		}
		YNZxkqoFXUeJNFqdRCzgRSvWQlTbA = ReInput.UserData.playerCount;
		SaAPUknZYJjkJPNMEsCcKAPefhmIA = YNZxkqoFXUeJNFqdRCzgRSvWQlTbA - 1;
		OEVOcQyWrhpclZkiWnLxbRGlXdql = new Player[SaAPUknZYJjkJPNMEsCcKAPefhmIA];
		oDFVoflNKcFbyGpcsgPOzMcRGdaJ = new Player[YNZxkqoFXUeJNFqdRCzgRSvWQlTbA];
		IList<Player_Editor> list = ReInput.UserData.JIZWSkNLBmbxmgqTylFTFDyIHEkLB;
		if (list == null)
		{
			throw new ArgumentNullException("Players cannot be null!");
		}
		for (int i = 0; i < list.Count; i++)
		{
			Player_Editor player_Editor = list[i];
			EhUOIcAWtPzjNpBYLAHyNaaERzaE ehUOIcAWtPzjNpBYLAHyNaaERzaE = player_Editor.qjgbGAcyFmSMTzcJABgXEsUgyQTv();
			ControllerMapLayoutManager.ICvEqTkajGQZLXkOyPTomcjaeCWs cvEqTkajGQZLXkOyPTomcjaeCWs = player_Editor.controllerMapLayoutManagerSettings.MJHfMyBLinpbxXjnaGNrevuOWSsCA();
			ControllerMapEnabler.CyXCJRXKNwVfGTMPVLIRUynNmCKI cyXCJRXKNwVfGTMPVLIRUynNmCKI = player_Editor.controllerMapEnablerSettings.YNoLOReakxUorAzAJFCaIyXpqBIS();
			Player player;
			if (i == 0)
			{
				player = (FSqjTMGgVYIJQkaNGaXgJJjuoXyG = new Player(true, 9999999, player_Editor.name, player_Editor.descriptiveName, player_Editor.key, ehUOIcAWtPzjNpBYLAHyNaaERzaE, cvEqTkajGQZLXkOyPTomcjaeCWs, cyXCJRXKNwVfGTMPVLIRUynNmCKI));
			}
			else
			{
				player = new Player(false, i - 1, player_Editor.name, player_Editor.descriptiveName, player_Editor.key, ehUOIcAWtPzjNpBYLAHyNaaERzaE, cvEqTkajGQZLXkOyPTomcjaeCWs, cyXCJRXKNwVfGTMPVLIRUynNmCKI);
				OEVOcQyWrhpclZkiWnLxbRGlXdql[i - 1] = player;
			}
			oDFVoflNKcFbyGpcsgPOzMcRGdaJ[i] = player;
			player.isPlaying = player_Editor.startPlaying;
			player.controllers.hasMouse = player_Editor.assignMouseOnStart;
			player.controllers.hasKeyboard = player_Editor.assignKeyboardOnStart;
			player.controllers.excludeFromControllerAutoAssignment = player_Editor.excludeFromControllerAutoAssignment;
			player.controllers.maps.EBXSWvlskcRzsmNKjtNWCyEjCeir(true);
			player.controllers.maps.ICgkACWSyaPEaoDmKtVxijJGGzNB(true);
		}
		VZbXlvjyGAAasKOcsAiyzAeIzXbe = new ReadOnlyCollection<Player>(OEVOcQyWrhpclZkiWnLxbRGlXdql);
		RwqQDyJmlWuSeNtEzhqVUlaSMdYc = new ReadOnlyCollection<Player>(oDFVoflNKcFbyGpcsgPOzMcRGdaJ);
		CfrrPiGKEEweWUoyCLVyrEtzTSap = true;
	}

	public void aENDJtIKdDBxcWAwSMmhnOuTbqKl(Joystick P_0)
	{
		if (ReInput.controllerAssigner != null && ReInput.controllerAssigner.CanHandleAssignment(ControllerType.Joystick, P_0))
		{
			ReInput.controllerAssigner.AssignController(ControllerType.Joystick, P_0);
		}
		else if (!ZThjONALbmmeOCJHZqCXNLXCOMpU.reassignJoystickToPreviousOwnerOnReconnect || !UJiwMQsRAPPSwyFzCEulcnyCiXdd(P_0))
		{
			WVOHcBkCMCXOFfJZPPUaRKCEdJiq(P_0);
		}
	}

	public void XYpxgsTENovtMXHsttRwttzGtfJB(Joystick P_0)
	{
		if (ZThjONALbmmeOCJHZqCXNLXCOMpU.autoAssignJoysticks)
		{
			aENDJtIKdDBxcWAwSMmhnOuTbqKl(P_0);
		}
	}

	public void DtcMqkBDUFDNlsQzRBvpRrePYJJT(ControllerType P_0, int P_1)
	{
		for (int i = 0; i < YNZxkqoFXUeJNFqdRCzgRSvWQlTbA; i++)
		{
			oDFVoflNKcFbyGpcsgPOzMcRGdaJ[i].controllers.RemoveController(P_0, P_1);
		}
	}

	public Player LcqJOYavcMfniFdJGbmbfGcBCdPHA(int P_0)
	{
		if (P_0 != 9999999 && (P_0 < 0 || P_0 >= SaAPUknZYJjkJPNMEsCcKAPefhmIA))
		{
			Logger.LogError("Player id " + P_0 + " does not exist!");
			return null;
		}
		if (P_0 == 9999999)
		{
			return FSqjTMGgVYIJQkaNGaXgJJjuoXyG;
		}
		for (int i = 0; i < SaAPUknZYJjkJPNMEsCcKAPefhmIA; i++)
		{
			if (OEVOcQyWrhpclZkiWnLxbRGlXdql[i].id == P_0)
			{
				return OEVOcQyWrhpclZkiWnLxbRGlXdql[P_0];
			}
		}
		return null;
	}

	public Player vVgeiNMSEqqjIpQPOUSSbXqKMJKi(string P_0)
	{
		if (P_0 != null && !(P_0 == string.Empty))
		{
			if (FSqjTMGgVYIJQkaNGaXgJJjuoXyG.name.Equals(P_0, StringComparison.OrdinalIgnoreCase))
			{
				return FSqjTMGgVYIJQkaNGaXgJJjuoXyG;
			}
			for (int i = 0; i < SaAPUknZYJjkJPNMEsCcKAPefhmIA; i++)
			{
				if (OEVOcQyWrhpclZkiWnLxbRGlXdql[i].name.Equals(P_0, StringComparison.OrdinalIgnoreCase))
				{
					return OEVOcQyWrhpclZkiWnLxbRGlXdql[i];
				}
			}
		}
		Logger.LogError("Player \"" + P_0 + "\" does not exist!");
		return null;
	}

	public Player KSOsWtiVPgllQavrnvGgcPSdahjn()
	{
		return FSqjTMGgVYIJQkaNGaXgJJjuoXyG;
	}

	public int ETcCPPDiAgogRIOlbLeCrfltHOeic(string P_0)
	{
		if (P_0 == null || P_0 == string.Empty)
		{
			return -1;
		}
		if (FSqjTMGgVYIJQkaNGaXgJJjuoXyG.name.Equals(P_0, StringComparison.OrdinalIgnoreCase))
		{
			return 9999999;
		}
		for (int i = 0; i < SaAPUknZYJjkJPNMEsCcKAPefhmIA; i++)
		{
			if (OEVOcQyWrhpclZkiWnLxbRGlXdql[i].name.Equals(P_0, StringComparison.OrdinalIgnoreCase))
			{
				return OEVOcQyWrhpclZkiWnLxbRGlXdql[i].id;
			}
		}
		return -1;
	}

	public bool XxOxiaujmYdKOWQmRpJTtLcvbtRy(int P_0)
	{
		if (P_0 != 9999999 && (P_0 < 0 || P_0 >= SaAPUknZYJjkJPNMEsCcKAPefhmIA))
		{
			return false;
		}
		return true;
	}

	public Player[] XnbZGlcwZVMINEaeDamnepKlPuCQA(bool P_0)
	{
		int num = SaAPUknZYJjkJPNMEsCcKAPefhmIA;
		if (P_0)
		{
			num++;
		}
		Player[] array = new Player[num];
		int num2 = 0;
		if (P_0)
		{
			array[0] = FSqjTMGgVYIJQkaNGaXgJJjuoXyG;
			num2 = 1;
		}
		for (int i = 0; i < SaAPUknZYJjkJPNMEsCcKAPefhmIA; i++)
		{
			array[num2 + i] = OEVOcQyWrhpclZkiWnLxbRGlXdql[i];
		}
		return array;
	}

	public string[] AiwSECrvmluRBIukiFAqNNkGiatcA(bool P_0)
	{
		int num = SaAPUknZYJjkJPNMEsCcKAPefhmIA;
		if (P_0)
		{
			num++;
		}
		string[] array = new string[num];
		int num2 = 0;
		if (P_0)
		{
			array[0] = FSqjTMGgVYIJQkaNGaXgJJjuoXyG.name;
			num2 = 1;
		}
		for (int i = 0; i < SaAPUknZYJjkJPNMEsCcKAPefhmIA; i++)
		{
			array[num2 + i] = OEVOcQyWrhpclZkiWnLxbRGlXdql[i].name;
		}
		return array;
	}

	public string[] lzghKbuCDVimnIqvQdasXbPaLcryA(bool P_0)
	{
		int num = SaAPUknZYJjkJPNMEsCcKAPefhmIA;
		if (P_0)
		{
			num++;
		}
		string[] array = new string[num];
		int num2 = 0;
		if (P_0)
		{
			array[0] = FSqjTMGgVYIJQkaNGaXgJJjuoXyG.descriptiveName;
			num2 = 1;
		}
		for (int i = 0; i < SaAPUknZYJjkJPNMEsCcKAPefhmIA; i++)
		{
			array[num2 + i] = OEVOcQyWrhpclZkiWnLxbRGlXdql[i].descriptiveName;
		}
		return array;
	}

	public int[] VSZNYLBMdwJKnwACpAGwapcLMJUpA(bool P_0)
	{
		int num = SaAPUknZYJjkJPNMEsCcKAPefhmIA;
		if (P_0)
		{
			num++;
		}
		int[] array = new int[num];
		int num2 = 0;
		if (P_0)
		{
			array[0] = FSqjTMGgVYIJQkaNGaXgJJjuoXyG.id;
			num2 = 1;
		}
		for (int i = 0; i < SaAPUknZYJjkJPNMEsCcKAPefhmIA; i++)
		{
			array[num2 + i] = OEVOcQyWrhpclZkiWnLxbRGlXdql[i].id;
		}
		return array;
	}

	public bool aqhIxgnnmPZVlFbGfmDEGfMBtHtE(Controller P_0)
	{
		if (P_0 == null || oDFVoflNKcFbyGpcsgPOzMcRGdaJ == null)
		{
			return false;
		}
		return NwncrvnyvquUEQJMoESVHNntGqPF(P_0.type, P_0.id);
	}

	public bool NwncrvnyvquUEQJMoESVHNntGqPF(ControllerType P_0, int P_1)
	{
		if (oDFVoflNKcFbyGpcsgPOzMcRGdaJ == null)
		{
			return false;
		}
		for (int i = 0; i < oDFVoflNKcFbyGpcsgPOzMcRGdaJ.Length; i++)
		{
			if (oDFVoflNKcFbyGpcsgPOzMcRGdaJ[i].controllers.ContainsController(P_0, P_1))
			{
				return true;
			}
		}
		return false;
	}

	public bool UKErIMlvBybVbWfLuJdJkBpWtcZM(ControllerType P_0, int P_1, int P_2)
	{
		return LcqJOYavcMfniFdJGbmbfGcBCdPHA(P_2)?.controllers.ContainsController(P_0, P_1) ?? false;
	}

	public void TytuzXrTqkbdWsnyKEArNueZorIJ(Controller P_0, bool P_1)
	{
		if (P_0 != null)
		{
			if (P_1)
			{
				FSqjTMGgVYIJQkaNGaXgJJjuoXyG.controllers.RemoveController(P_0);
			}
			for (int i = 0; i < SaAPUknZYJjkJPNMEsCcKAPefhmIA; i++)
			{
				OEVOcQyWrhpclZkiWnLxbRGlXdql[i].controllers.RemoveController(P_0);
			}
		}
	}

	public void RmrBIoFPNnWxKPKpWAhRbDrtIAlPA(ControllerType P_0, int P_1, bool P_2)
	{
		Controller controller = ReInput.controllers.GetController(P_0, P_1);
		if (controller != null)
		{
			TytuzXrTqkbdWsnyKEArNueZorIJ(controller, P_2);
		}
	}

	public bool srxWbDEXENzLHMKSMWHYNlQvNhsc(Joystick P_0)
	{
		if (P_0 == null || oDFVoflNKcFbyGpcsgPOzMcRGdaJ == null)
		{
			return false;
		}
		for (int i = 0; i < oDFVoflNKcFbyGpcsgPOzMcRGdaJ.Length; i++)
		{
			if (oDFVoflNKcFbyGpcsgPOzMcRGdaJ[i].controllers.ContainsController(P_0))
			{
				return true;
			}
		}
		return false;
	}

	public bool WomCcKkVEQVBDrjsRIxXGYHxVwVN(int P_0)
	{
		if (oDFVoflNKcFbyGpcsgPOzMcRGdaJ == null)
		{
			return false;
		}
		for (int i = 0; i < oDFVoflNKcFbyGpcsgPOzMcRGdaJ.Length; i++)
		{
			if (oDFVoflNKcFbyGpcsgPOzMcRGdaJ[i].controllers.ContainsController(ControllerType.Joystick, P_0))
			{
				return true;
			}
		}
		return false;
	}

	public bool TYBTDZeIKOQqBqGWmCVraQPlywlM(int P_0, int P_1)
	{
		return LcqJOYavcMfniFdJGbmbfGcBCdPHA(P_1)?.controllers.ContainsController(ControllerType.Joystick, P_0) ?? false;
	}

	public void EAGXsKFcPMELlqeigItnhEsvxBZI(Joystick P_0, bool P_1)
	{
		if (P_0 != null)
		{
			if (P_1)
			{
				FSqjTMGgVYIJQkaNGaXgJJjuoXyG.controllers.KMPMiMoEebbjbaespAkXfArEmWZbb(P_0);
			}
			for (int i = 0; i < SaAPUknZYJjkJPNMEsCcKAPefhmIA; i++)
			{
				OEVOcQyWrhpclZkiWnLxbRGlXdql[i].controllers.KMPMiMoEebbjbaespAkXfArEmWZbb(P_0);
			}
		}
	}

	public void YPRiSbxpfuHpjHJUEtbJoGCbsTxn(int P_0, bool P_1)
	{
		Joystick joystick = ReInput.controllers.GetJoystick(P_0);
		if (joystick != null)
		{
			EAGXsKFcPMELlqeigItnhEsvxBZI(joystick, P_1);
		}
	}

	public bool oiJesQfscgWrOhYjmBeynHCUTSdNA(CustomController P_0)
	{
		if (P_0 == null || oDFVoflNKcFbyGpcsgPOzMcRGdaJ == null)
		{
			return false;
		}
		for (int i = 0; i < oDFVoflNKcFbyGpcsgPOzMcRGdaJ.Length; i++)
		{
			if (oDFVoflNKcFbyGpcsgPOzMcRGdaJ[i].controllers.ContainsController(P_0))
			{
				return true;
			}
		}
		return false;
	}

	public bool yeQFYalmclJKXhkjlMKlOQQfgrSM(int P_0)
	{
		if (oDFVoflNKcFbyGpcsgPOzMcRGdaJ == null)
		{
			return false;
		}
		for (int i = 0; i < oDFVoflNKcFbyGpcsgPOzMcRGdaJ.Length; i++)
		{
			if (oDFVoflNKcFbyGpcsgPOzMcRGdaJ[i].controllers.ContainsController(ControllerType.Custom, P_0))
			{
				return true;
			}
		}
		return false;
	}

	public bool mvVxELeVNlCkGSnPKMpeKBsEyIkn(int P_0, int P_1)
	{
		return LcqJOYavcMfniFdJGbmbfGcBCdPHA(P_1)?.controllers.ContainsController(ControllerType.Custom, P_0) ?? false;
	}

	public void zDSWrprkdisFplLdhSLXGgHeEili(CustomController P_0, bool P_1)
	{
		if (P_0 != null)
		{
			if (P_1)
			{
				FSqjTMGgVYIJQkaNGaXgJJjuoXyG.controllers.olyogBEWAdfNkvaMUpyfhXXcrdsH(P_0);
			}
			for (int i = 0; i < SaAPUknZYJjkJPNMEsCcKAPefhmIA; i++)
			{
				OEVOcQyWrhpclZkiWnLxbRGlXdql[i].controllers.olyogBEWAdfNkvaMUpyfhXXcrdsH(P_0);
			}
		}
	}

	public void fpCJjwOokofCeibJTwPioFDvRNEFb(int P_0, bool P_1)
	{
		CustomController customController = ReInput.controllers.GetCustomController(P_0);
		if (customController != null)
		{
			zDSWrprkdisFplLdhSLXGgHeEili(customController, P_1);
		}
	}

	private bool UJiwMQsRAPPSwyFzCEulcnyCiXdd(Joystick P_0)
	{
		if (ZThjONALbmmeOCJHZqCXNLXCOMpU.distributeJoysticksEvenly)
		{
			int num = psgQhSopXYKiATJixkTcDOxSKwKD();
			if (num < 0)
			{
				return false;
			}
			int num2 = GoKzDmuPdLoVPvJucfqHGodbBedwA(P_0.id);
			if (num2 < 0)
			{
				return false;
			}
			Player player = OEVOcQyWrhpclZkiWnLxbRGlXdql[num];
			Player player2 = OEVOcQyWrhpclZkiWnLxbRGlXdql[num2];
			if (num2 >= 0 && player2.controllers.joystickCount <= player.controllers.joystickCount)
			{
				OEVOcQyWrhpclZkiWnLxbRGlXdql[num2].controllers.hhmqdFNEziZipQHXNKUvCsrJCuwl(P_0, true);
				return true;
			}
			return false;
		}
		int num3 = GoKzDmuPdLoVPvJucfqHGodbBedwA(P_0.id);
		if (num3 < 0)
		{
			return false;
		}
		OEVOcQyWrhpclZkiWnLxbRGlXdql[num3].controllers.hhmqdFNEziZipQHXNKUvCsrJCuwl(P_0, true);
		return true;
	}

	private bool WVOHcBkCMCXOFfJZPPUaRKCEdJiq(Joystick P_0)
	{
		if (ZThjONALbmmeOCJHZqCXNLXCOMpU.distributeJoysticksEvenly)
		{
			int num = psgQhSopXYKiATJixkTcDOxSKwKD();
			if (num >= 0)
			{
				OEVOcQyWrhpclZkiWnLxbRGlXdql[num].controllers.hhmqdFNEziZipQHXNKUvCsrJCuwl(P_0, true);
				return true;
			}
		}
		else
		{
			for (int i = 0; i < SaAPUknZYJjkJPNMEsCcKAPefhmIA; i++)
			{
				Player player = OEVOcQyWrhpclZkiWnLxbRGlXdql[i];
				if (!player.controllers.excludeFromControllerAutoAssignment && (!ZThjONALbmmeOCJHZqCXNLXCOMpU.assignJoysticksToPlayingPlayersOnly || player.isPlaying) && player.controllers.joystickCount < ZThjONALbmmeOCJHZqCXNLXCOMpU.maxJoysticksPerPlayer)
				{
					player.controllers.hhmqdFNEziZipQHXNKUvCsrJCuwl(P_0, true);
					return true;
				}
			}
		}
		return false;
	}

	private int psgQhSopXYKiATJixkTcDOxSKwKD()
	{
		int num = -1;
		int num2 = 0;
		for (int i = 0; i < SaAPUknZYJjkJPNMEsCcKAPefhmIA; i++)
		{
			Player player = OEVOcQyWrhpclZkiWnLxbRGlXdql[i];
			if (!player.controllers.excludeFromControllerAutoAssignment && (!ZThjONALbmmeOCJHZqCXNLXCOMpU.assignJoysticksToPlayingPlayersOnly || player.isPlaying))
			{
				int joystickCount = player.controllers.joystickCount;
				if (joystickCount < ZThjONALbmmeOCJHZqCXNLXCOMpU.maxJoysticksPerPlayer && (num == -1 || joystickCount < num2))
				{
					num = i;
					num2 = joystickCount;
				}
			}
		}
		return num;
	}

	public int GoKzDmuPdLoVPvJucfqHGodbBedwA(int P_0)
	{
		int num = -1;
		double num2 = 0.0;
		for (int i = 0; i < SaAPUknZYJjkJPNMEsCcKAPefhmIA; i++)
		{
			Player player = OEVOcQyWrhpclZkiWnLxbRGlXdql[i];
			if (!player.controllers.excludeFromControllerAutoAssignment && (!ZThjONALbmmeOCJHZqCXNLXCOMpU.assignJoysticksToPlayingPlayersOnly || player.isPlaying) && player.controllers.joystickCount < ZThjONALbmmeOCJHZqCXNLXCOMpU.maxJoysticksPerPlayer)
			{
				double num3 = player.controllers.srsDPIieElYApGDgafIUhFdfynUi(P_0);
				if (!(num3 < 0.0) && (num < 0 || num3 > num2))
				{
					num2 = num3;
					num = i;
				}
			}
		}
		return num;
	}
}
