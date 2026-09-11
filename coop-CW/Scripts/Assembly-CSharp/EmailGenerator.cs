using Photon.Pun;
using Photon.Realtime;

public static class EmailGenerator
{
	public static string GetEmails()
	{
		Photon.Realtime.Player[] playerList = PhotonNetwork.PlayerList;
		string text = "";
		bool flag = true;
		Photon.Realtime.Player[] array = playerList;
		foreach (Photon.Realtime.Player player in array)
		{
			if (!flag)
			{
				text += ", ";
			}
			text = text + player.NickName.ToLower() + "@mailo.nw";
			flag = false;
		}
		return text;
	}
}
