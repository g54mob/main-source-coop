using Photon.Pun;

public abstract class MonsterContentProvider : ContentProvider
{
	private PhotonView photonView;

	private Bot hiddenBot;

	private void Awake()
	{
		photonView = GetComponent<PhotonView>();
		hiddenBot = base.transform.root.GetComponentInChildren<Bot>();
	}

	public T GetContentEvent<T>() where T : MonsterContentEvent, new()
	{
		T val = new T();
		int viewID = 0;
		if (photonView != null)
		{
			viewID = photonView.ViewID;
		}
		val.viewID = viewID;
		val.worldPosition = hiddenBot.Center();
		return val;
	}
}
