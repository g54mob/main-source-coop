using Photon.Pun;
using UnityEngine;

public class SpookyMusicHandler : MonoBehaviour
{
	public int level = 1;

	private float sinceDanger = 1000f;

	public float onVolume = 0.2f;

	private AudioSource source;

	private PhotonView view;

	private bool isDanger;

	private void Awake()
	{
		source = GetComponent<AudioSource>();
		view = GetComponent<PhotonView>();
	}

	private void Update()
	{
		sinceDanger += Time.deltaTime;
		if (view.IsMine)
		{
			if (sinceDanger < 10f)
			{
				ToggleDanger(setDanger: true);
			}
			else
			{
				ToggleDanger(setDanger: false);
			}
		}
		if (isDanger)
		{
			source.volume = Mathf.MoveTowards(source.volume, onVolume, Time.deltaTime * 0.05f);
			if (source.volume > 0.001f)
			{
				source.enabled = true;
			}
		}
		else
		{
			source.volume = Mathf.MoveTowards(source.volume, 0f, Time.deltaTime * 0.05f);
			if (source.volume < 0.001f)
			{
				source.enabled = false;
			}
		}
	}

	public void AddDanger(float dangerPercentage, int jumpScareLevel)
	{
		if (jumpScareLevel == level)
		{
			sinceDanger = 0f;
		}
	}

	private void ToggleDanger(bool setDanger)
	{
		if (isDanger != setDanger)
		{
			view.RPC("RPCA_ToggleDanger", RpcTarget.All, setDanger);
		}
	}

	[PunRPC]
	public void RPCA_ToggleDanger(bool setDanger)
	{
		isDanger = setDanger;
	}
}
