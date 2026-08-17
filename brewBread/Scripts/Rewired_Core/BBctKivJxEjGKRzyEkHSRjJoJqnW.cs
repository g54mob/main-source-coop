using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Rewired;
using Rewired.Data;

internal class BBctKivJxEjGKRzyEkHSRjJoJqnW
{
	private int gcznhDktyGCftDBMpxtFbLOxFwAg;

	private int yWdrYraBblaXyeFWBvtthunxkAQNA;

	private Player dPIAWppcMaaxGzcMrOGonBXQzXqJ;

	private Player[] ZEETyWEfPMcQgBGMtHMYilOBxUXA;

	private Player[] YsdVcyiGSSvqDbiDKKWiKnCuHAWB;

	private IList<Player> QBTmfnuLmkWqGrYTHtzcouycAfBw;

	private IList<Player> XiZDdKkKiqMSUOtqNKwptXZMJABe;

	private ConfigVars dNgrfdLJdsfeJmlYYJJiwGpIkluS;

	private bool jYTNgflwwEgvgbZuuTHYnPAnuRao;

	public int CQIAQFpzVmISknegYFOIxRunxPpK => gcznhDktyGCftDBMpxtFbLOxFwAg;

	public int jXskRzMcqiBuuZuSFMpqenJeANVp => yWdrYraBblaXyeFWBvtthunxkAQNA;

	public Player[] JfwqcvUYPSLnmJKPxcgsgsLzOVvp => ZEETyWEfPMcQgBGMtHMYilOBxUXA;

	public Player[] DUOCbsXCfPFpdfdngPcfPQmvfCwTA => YsdVcyiGSSvqDbiDKKWiKnCuHAWB;

	public IList<Player> OLLVcigGyhAbFaNDXiyTXurgpLPOA => XiZDdKkKiqMSUOtqNKwptXZMJABe;

	public IList<Player> WEZyqeTcxBEnozDVUGnNzVZBqwqJ => QBTmfnuLmkWqGrYTHtzcouycAfBw;

	public BBctKivJxEjGKRzyEkHSRjJoJqnW(ConfigVars P_0)
	{
		dNgrfdLJdsfeJmlYYJJiwGpIkluS = P_0;
	}

	public void zQQfvDZMmpVqPPLYlLuSJXXpwJcI()
	{
		if (jYTNgflwwEgvgbZuuTHYnPAnuRao)
		{
			return;
		}
		yWdrYraBblaXyeFWBvtthunxkAQNA = ReInput.UserData.playerCount;
		gcznhDktyGCftDBMpxtFbLOxFwAg = yWdrYraBblaXyeFWBvtthunxkAQNA - 1;
		YsdVcyiGSSvqDbiDKKWiKnCuHAWB = new Player[gcznhDktyGCftDBMpxtFbLOxFwAg];
		ZEETyWEfPMcQgBGMtHMYilOBxUXA = new Player[yWdrYraBblaXyeFWBvtthunxkAQNA];
		IList<Player_Editor> list = ReInput.UserData.WEZyqeTcxBEnozDVUGnNzVZBqwqJ;
		if (list == null)
		{
			throw new ArgumentNullException("Players cannot be null!");
		}
		for (int i = 0; i < list.Count; i++)
		{
			Player_Editor player_Editor = list[i];
			ypwUncqYrrgWARbofZIocbdoyJzm ypwUncqYrrgWARbofZIocbdoyJzm2 = player_Editor.bPCRfTsZBWAqvvaTkOFDsaqvDzII();
			ControllerMapLayoutManager.iZBHTLMJEscsCxujGlXwPdwYSOZv iZBHTLMJEscsCxujGlXwPdwYSOZv = player_Editor.controllerMapLayoutManagerSettings.yMMoRTmlFkeyjRZhjJmIJpImkMHJ();
			ControllerMapEnabler.oejthPnlmICeXCzkndFXvfozHuLjA oejthPnlmICeXCzkndFXvfozHuLjA = player_Editor.controllerMapEnablerSettings.yMMoRTmlFkeyjRZhjJmIJpImkMHJ();
			Player player;
			if (i == 0)
			{
				player = (dPIAWppcMaaxGzcMrOGonBXQzXqJ = new Player(true, 9999999, player_Editor.name, player_Editor.descriptiveName, ypwUncqYrrgWARbofZIocbdoyJzm2, iZBHTLMJEscsCxujGlXwPdwYSOZv, oejthPnlmICeXCzkndFXvfozHuLjA));
			}
			else
			{
				player = new Player(false, i - 1, player_Editor.name, player_Editor.descriptiveName, ypwUncqYrrgWARbofZIocbdoyJzm2, iZBHTLMJEscsCxujGlXwPdwYSOZv, oejthPnlmICeXCzkndFXvfozHuLjA);
				YsdVcyiGSSvqDbiDKKWiKnCuHAWB[i - 1] = player;
			}
			ZEETyWEfPMcQgBGMtHMYilOBxUXA[i] = player;
			player.isPlaying = player_Editor.startPlaying;
			player.controllers.hasMouse = player_Editor.assignMouseOnStart;
			player.controllers.hasKeyboard = player_Editor.assignKeyboardOnStart;
			player.controllers.excludeFromControllerAutoAssignment = player_Editor.excludeFromControllerAutoAssignment;
			player.controllers.maps.yvacXXRoQNUSMpwmVarCvJSKggMFA(true);
			player.controllers.maps.pYpQQZKzmZLGCLEsDAiOiozayExPA(true);
		}
		QBTmfnuLmkWqGrYTHtzcouycAfBw = new ReadOnlyCollection<Player>(YsdVcyiGSSvqDbiDKKWiKnCuHAWB);
		XiZDdKkKiqMSUOtqNKwptXZMJABe = new ReadOnlyCollection<Player>(ZEETyWEfPMcQgBGMtHMYilOBxUXA);
		jYTNgflwwEgvgbZuuTHYnPAnuRao = true;
	}

	public void OjIFqmaNarRMdzDSkOpVffgHGfVxA(Joystick P_0)
	{
		if (ReInput.controllerAssigner != null && ReInput.controllerAssigner.CanHandleAssignment(ControllerType.Joystick, P_0))
		{
			ReInput.controllerAssigner.AssignController(ControllerType.Joystick, P_0);
		}
		else if (!dNgrfdLJdsfeJmlYYJJiwGpIkluS.reassignJoystickToPreviousOwnerOnReconnect || !UUwtnKPfQqUWfSXHAtXRwmvBycjI(P_0))
		{
			YBgsiXamMzfxdDSwtGEvuhuGdavDb(P_0);
		}
	}

	public void nhAQUIndynxeHenHwIxuJxrjAQNp(Joystick P_0)
	{
		if (dNgrfdLJdsfeJmlYYJJiwGpIkluS.autoAssignJoysticks)
		{
			OjIFqmaNarRMdzDSkOpVffgHGfVxA(P_0);
		}
	}

	public void elDPsiONERnRCGVdgPltkMSHilkF(ControllerType P_0, int P_1)
	{
		for (int i = 0; i < yWdrYraBblaXyeFWBvtthunxkAQNA; i++)
		{
			ZEETyWEfPMcQgBGMtHMYilOBxUXA[i].controllers.RemoveController(P_0, P_1);
		}
	}

	public Player ynWFEmrsktqGecVuJbofDaNeQFxn(int P_0)
	{
		if (P_0 != 9999999 && (P_0 < 0 || P_0 >= gcznhDktyGCftDBMpxtFbLOxFwAg))
		{
			Logger.LogError("Player id " + P_0 + " does not exist!");
			return null;
		}
		if (P_0 == 9999999)
		{
			return dPIAWppcMaaxGzcMrOGonBXQzXqJ;
		}
		for (int i = 0; i < gcznhDktyGCftDBMpxtFbLOxFwAg; i++)
		{
			if (YsdVcyiGSSvqDbiDKKWiKnCuHAWB[i].id == P_0)
			{
				return YsdVcyiGSSvqDbiDKKWiKnCuHAWB[P_0];
			}
		}
		return null;
	}

	public Player ynWFEmrsktqGecVuJbofDaNeQFxn(string P_0)
	{
		if (P_0 != null && !(P_0 == string.Empty))
		{
			if (dPIAWppcMaaxGzcMrOGonBXQzXqJ.name.Equals(P_0, StringComparison.OrdinalIgnoreCase))
			{
				return dPIAWppcMaaxGzcMrOGonBXQzXqJ;
			}
			for (int i = 0; i < gcznhDktyGCftDBMpxtFbLOxFwAg; i++)
			{
				if (YsdVcyiGSSvqDbiDKKWiKnCuHAWB[i].name.Equals(P_0, StringComparison.OrdinalIgnoreCase))
				{
					return YsdVcyiGSSvqDbiDKKWiKnCuHAWB[i];
				}
			}
		}
		Logger.LogError("Player \"" + P_0 + "\" does not exist!");
		return null;
	}

	public Player blBVoZZhgxiIOOqkchCQmhLeDxsEA()
	{
		return dPIAWppcMaaxGzcMrOGonBXQzXqJ;
	}

	public int ZCqHACovSplPzgbpXgZTFMQjMLOFb(string P_0)
	{
		if (P_0 == null || P_0 == string.Empty)
		{
			return -1;
		}
		if (dPIAWppcMaaxGzcMrOGonBXQzXqJ.name.Equals(P_0, StringComparison.OrdinalIgnoreCase))
		{
			return 9999999;
		}
		for (int i = 0; i < gcznhDktyGCftDBMpxtFbLOxFwAg; i++)
		{
			if (YsdVcyiGSSvqDbiDKKWiKnCuHAWB[i].name.Equals(P_0, StringComparison.OrdinalIgnoreCase))
			{
				return YsdVcyiGSSvqDbiDKKWiKnCuHAWB[i].id;
			}
		}
		return -1;
	}

	public bool fVVcnKcITKFRLpCybjQgHHsavAvUA(int P_0)
	{
		if (P_0 != 9999999 && (P_0 < 0 || P_0 >= gcznhDktyGCftDBMpxtFbLOxFwAg))
		{
			return false;
		}
		return true;
	}

	public Player[] OPhpqQAIYTbZeFSyRthHDHUWaVMl(bool P_0)
	{
		int num = gcznhDktyGCftDBMpxtFbLOxFwAg;
		if (P_0)
		{
			num++;
		}
		Player[] array = new Player[num];
		int num2 = 0;
		if (P_0)
		{
			array[0] = dPIAWppcMaaxGzcMrOGonBXQzXqJ;
			num2 = 1;
		}
		for (int i = 0; i < gcznhDktyGCftDBMpxtFbLOxFwAg; i++)
		{
			array[num2 + i] = YsdVcyiGSSvqDbiDKKWiKnCuHAWB[i];
		}
		return array;
	}

	public string[] CbNUOSNNDybeWCluNpDcdFlpPdlc(bool P_0)
	{
		int num = gcznhDktyGCftDBMpxtFbLOxFwAg;
		if (P_0)
		{
			num++;
		}
		string[] array = new string[num];
		int num2 = 0;
		if (P_0)
		{
			array[0] = dPIAWppcMaaxGzcMrOGonBXQzXqJ.name;
			num2 = 1;
		}
		for (int i = 0; i < gcznhDktyGCftDBMpxtFbLOxFwAg; i++)
		{
			array[num2 + i] = YsdVcyiGSSvqDbiDKKWiKnCuHAWB[i].name;
		}
		return array;
	}

	public string[] CIYITztLXRrClpnNvmgkHPLIqRAt(bool P_0)
	{
		int num = gcznhDktyGCftDBMpxtFbLOxFwAg;
		if (P_0)
		{
			num++;
		}
		string[] array = new string[num];
		int num2 = 0;
		if (P_0)
		{
			array[0] = dPIAWppcMaaxGzcMrOGonBXQzXqJ.descriptiveName;
			num2 = 1;
		}
		for (int i = 0; i < gcznhDktyGCftDBMpxtFbLOxFwAg; i++)
		{
			array[num2 + i] = YsdVcyiGSSvqDbiDKKWiKnCuHAWB[i].descriptiveName;
		}
		return array;
	}

	public int[] WVREjiFkAWoWTPKBmLDbFFyfZMij(bool P_0)
	{
		int num = gcznhDktyGCftDBMpxtFbLOxFwAg;
		if (P_0)
		{
			num++;
		}
		int[] array = new int[num];
		int num2 = 0;
		if (P_0)
		{
			array[0] = dPIAWppcMaaxGzcMrOGonBXQzXqJ.id;
			num2 = 1;
		}
		for (int i = 0; i < gcznhDktyGCftDBMpxtFbLOxFwAg; i++)
		{
			array[num2 + i] = YsdVcyiGSSvqDbiDKKWiKnCuHAWB[i].id;
		}
		return array;
	}

	public bool RaqNthvphttAgGIIabSXRCuGWBIl(Controller P_0)
	{
		if (P_0 == null || ZEETyWEfPMcQgBGMtHMYilOBxUXA == null)
		{
			return false;
		}
		return RaqNthvphttAgGIIabSXRCuGWBIl(P_0.type, P_0.id);
	}

	public bool RaqNthvphttAgGIIabSXRCuGWBIl(ControllerType P_0, int P_1)
	{
		if (ZEETyWEfPMcQgBGMtHMYilOBxUXA == null)
		{
			return false;
		}
		for (int i = 0; i < ZEETyWEfPMcQgBGMtHMYilOBxUXA.Length; i++)
		{
			if (ZEETyWEfPMcQgBGMtHMYilOBxUXA[i].controllers.ContainsController(P_0, P_1))
			{
				return true;
			}
		}
		return false;
	}

	public bool gdVKWEYmfTArKpxaWVepHQidLbuF(ControllerType P_0, int P_1, int P_2)
	{
		return ynWFEmrsktqGecVuJbofDaNeQFxn(P_2)?.controllers.ContainsController(P_0, P_1) ?? false;
	}

	public void LWrxtiSdTIxHkxgXOOIRRipxOuaE(Controller P_0, bool P_1)
	{
		if (P_0 != null)
		{
			if (P_1)
			{
				dPIAWppcMaaxGzcMrOGonBXQzXqJ.controllers.RemoveController(P_0);
			}
			for (int i = 0; i < gcznhDktyGCftDBMpxtFbLOxFwAg; i++)
			{
				YsdVcyiGSSvqDbiDKKWiKnCuHAWB[i].controllers.RemoveController(P_0);
			}
		}
	}

	public void LWrxtiSdTIxHkxgXOOIRRipxOuaE(ControllerType P_0, int P_1, bool P_2)
	{
		Controller controller = ReInput.controllers.GetController(P_0, P_1);
		if (controller != null)
		{
			LWrxtiSdTIxHkxgXOOIRRipxOuaE(controller, P_2);
		}
	}

	public bool voAbaiSenJzcoJXjSwTrqVlucavU(Joystick P_0)
	{
		if (P_0 == null || ZEETyWEfPMcQgBGMtHMYilOBxUXA == null)
		{
			return false;
		}
		for (int i = 0; i < ZEETyWEfPMcQgBGMtHMYilOBxUXA.Length; i++)
		{
			if (ZEETyWEfPMcQgBGMtHMYilOBxUXA[i].controllers.ContainsController(P_0))
			{
				return true;
			}
		}
		return false;
	}

	public bool voAbaiSenJzcoJXjSwTrqVlucavU(int P_0)
	{
		if (ZEETyWEfPMcQgBGMtHMYilOBxUXA == null)
		{
			return false;
		}
		for (int i = 0; i < ZEETyWEfPMcQgBGMtHMYilOBxUXA.Length; i++)
		{
			if (ZEETyWEfPMcQgBGMtHMYilOBxUXA[i].controllers.ContainsController(ControllerType.Joystick, P_0))
			{
				return true;
			}
		}
		return false;
	}

	public bool LmjBCLHxwMetbFeBHpVClYJbrGjJc(int P_0, int P_1)
	{
		return ynWFEmrsktqGecVuJbofDaNeQFxn(P_1)?.controllers.ContainsController(ControllerType.Joystick, P_0) ?? false;
	}

	public void LBrJBEsxmdmIQJAcusUxcQSRcmIG(Joystick P_0, bool P_1)
	{
		if (P_0 != null)
		{
			if (P_1)
			{
				dPIAWppcMaaxGzcMrOGonBXQzXqJ.controllers.GHLFUYDRbbdpOizTVbmQosBYlpOBb(P_0);
			}
			for (int i = 0; i < gcznhDktyGCftDBMpxtFbLOxFwAg; i++)
			{
				YsdVcyiGSSvqDbiDKKWiKnCuHAWB[i].controllers.GHLFUYDRbbdpOizTVbmQosBYlpOBb(P_0);
			}
		}
	}

	public void LBrJBEsxmdmIQJAcusUxcQSRcmIG(int P_0, bool P_1)
	{
		Joystick joystick = ReInput.controllers.GetJoystick(P_0);
		if (joystick != null)
		{
			LBrJBEsxmdmIQJAcusUxcQSRcmIG(joystick, P_1);
		}
	}

	public bool kziJJZFacpjCcYYBwzoCbEKEKuzq(CustomController P_0)
	{
		if (P_0 == null || ZEETyWEfPMcQgBGMtHMYilOBxUXA == null)
		{
			return false;
		}
		for (int i = 0; i < ZEETyWEfPMcQgBGMtHMYilOBxUXA.Length; i++)
		{
			if (ZEETyWEfPMcQgBGMtHMYilOBxUXA[i].controllers.ContainsController(P_0))
			{
				return true;
			}
		}
		return false;
	}

	public bool kziJJZFacpjCcYYBwzoCbEKEKuzq(int P_0)
	{
		if (ZEETyWEfPMcQgBGMtHMYilOBxUXA == null)
		{
			return false;
		}
		for (int i = 0; i < ZEETyWEfPMcQgBGMtHMYilOBxUXA.Length; i++)
		{
			if (ZEETyWEfPMcQgBGMtHMYilOBxUXA[i].controllers.ContainsController(ControllerType.Custom, P_0))
			{
				return true;
			}
		}
		return false;
	}

	public bool zvWNUbsyYlpZeNcbcfzBpxHFvdSJ(int P_0, int P_1)
	{
		return ynWFEmrsktqGecVuJbofDaNeQFxn(P_1)?.controllers.ContainsController(ControllerType.Custom, P_0) ?? false;
	}

	public void YyoVnjzGVxqGIqdlPQgQqQnVhvGu(CustomController P_0, bool P_1)
	{
		if (P_0 != null)
		{
			if (P_1)
			{
				dPIAWppcMaaxGzcMrOGonBXQzXqJ.controllers.iuacuYVnEcyuHqNzXCqSIHTRrksK(P_0);
			}
			for (int i = 0; i < gcznhDktyGCftDBMpxtFbLOxFwAg; i++)
			{
				YsdVcyiGSSvqDbiDKKWiKnCuHAWB[i].controllers.iuacuYVnEcyuHqNzXCqSIHTRrksK(P_0);
			}
		}
	}

	public void YyoVnjzGVxqGIqdlPQgQqQnVhvGu(int P_0, bool P_1)
	{
		CustomController customController = ReInput.controllers.GetCustomController(P_0);
		if (customController != null)
		{
			YyoVnjzGVxqGIqdlPQgQqQnVhvGu(customController, P_1);
		}
	}

	private bool UUwtnKPfQqUWfSXHAtXRwmvBycjI(Joystick P_0)
	{
		if (dNgrfdLJdsfeJmlYYJJiwGpIkluS.distributeJoysticksEvenly)
		{
			int num = kAOEfGSdYeaKKrAaiuheIvcNJHDy();
			if (num < 0)
			{
				return false;
			}
			int num2 = inMwkcxWcioKpnsbFJekimNUhjEj(P_0.id);
			if (num2 < 0)
			{
				return false;
			}
			Player player = YsdVcyiGSSvqDbiDKKWiKnCuHAWB[num];
			Player player2 = YsdVcyiGSSvqDbiDKKWiKnCuHAWB[num2];
			if (num2 >= 0 && player2.controllers.joystickCount <= player.controllers.joystickCount)
			{
				YsdVcyiGSSvqDbiDKKWiKnCuHAWB[num2].controllers.qckZxShqtYgKZSGQmXJLvbtBGAUy(P_0, true);
				return true;
			}
			return false;
		}
		int num3 = inMwkcxWcioKpnsbFJekimNUhjEj(P_0.id);
		if (num3 < 0)
		{
			return false;
		}
		YsdVcyiGSSvqDbiDKKWiKnCuHAWB[num3].controllers.qckZxShqtYgKZSGQmXJLvbtBGAUy(P_0, true);
		return true;
	}

	private bool YBgsiXamMzfxdDSwtGEvuhuGdavDb(Joystick P_0)
	{
		if (dNgrfdLJdsfeJmlYYJJiwGpIkluS.distributeJoysticksEvenly)
		{
			int num = kAOEfGSdYeaKKrAaiuheIvcNJHDy();
			if (num >= 0)
			{
				YsdVcyiGSSvqDbiDKKWiKnCuHAWB[num].controllers.qckZxShqtYgKZSGQmXJLvbtBGAUy(P_0, true);
				return true;
			}
		}
		else
		{
			for (int i = 0; i < gcznhDktyGCftDBMpxtFbLOxFwAg; i++)
			{
				Player player = YsdVcyiGSSvqDbiDKKWiKnCuHAWB[i];
				if (!player.controllers.excludeFromControllerAutoAssignment && (!dNgrfdLJdsfeJmlYYJJiwGpIkluS.assignJoysticksToPlayingPlayersOnly || player.isPlaying) && player.controllers.joystickCount < dNgrfdLJdsfeJmlYYJJiwGpIkluS.maxJoysticksPerPlayer)
				{
					player.controllers.qckZxShqtYgKZSGQmXJLvbtBGAUy(P_0, true);
					return true;
				}
			}
		}
		return false;
	}

	private int kAOEfGSdYeaKKrAaiuheIvcNJHDy()
	{
		int num = -1;
		int num2 = 0;
		for (int i = 0; i < gcznhDktyGCftDBMpxtFbLOxFwAg; i++)
		{
			Player player = YsdVcyiGSSvqDbiDKKWiKnCuHAWB[i];
			if (!player.controllers.excludeFromControllerAutoAssignment && (!dNgrfdLJdsfeJmlYYJJiwGpIkluS.assignJoysticksToPlayingPlayersOnly || player.isPlaying))
			{
				int joystickCount = player.controllers.joystickCount;
				if (joystickCount < dNgrfdLJdsfeJmlYYJJiwGpIkluS.maxJoysticksPerPlayer && (num == -1 || joystickCount < num2))
				{
					num = i;
					num2 = joystickCount;
				}
			}
		}
		return num;
	}

	public int inMwkcxWcioKpnsbFJekimNUhjEj(int P_0)
	{
		int num = -1;
		double num2 = 0.0;
		for (int i = 0; i < gcznhDktyGCftDBMpxtFbLOxFwAg; i++)
		{
			Player player = YsdVcyiGSSvqDbiDKKWiKnCuHAWB[i];
			if (!player.controllers.excludeFromControllerAutoAssignment && (!dNgrfdLJdsfeJmlYYJJiwGpIkluS.assignJoysticksToPlayingPlayersOnly || player.isPlaying) && player.controllers.joystickCount < dNgrfdLJdsfeJmlYYJJiwGpIkluS.maxJoysticksPerPlayer)
			{
				double num3 = player.controllers.oMjeuAGTUybXgfnehjivQDLBZIzw(P_0);
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
