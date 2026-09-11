using Photon.Pun;
using UnityEngine;

public class ProjectorMachine : MonoBehaviour
{
	public SFX_Instance offSound;

	public SFX_Instance onSound;

	public AudioLoop loop;

	public SFX_Instance flip;

	public MeshRenderer rend;

	public Texture2D[] textures;

	private int currentID;

	private int prevInt;

	private PhotonView view;

	private void Start()
	{
		view = GetComponent<PhotonView>();
	}

	public void PressLess()
	{
		view.RPC("RPCA_Press", RpcTarget.All, false);
	}

	public void PressMore()
	{
		view.RPC("RPCA_Press", RpcTarget.All, true);
	}

	private void Update()
	{
		if (!rend.enabled)
		{
			if (loop.enabled)
			{
				offSound.Play(loop.transform.position);
			}
			loop.enabled = false;
		}
		if (rend.enabled)
		{
			if (!loop.enabled)
			{
				onSound.Play(loop.transform.position);
			}
			loop.enabled = true;
		}
		if (currentID != prevInt)
		{
			flip.Play(loop.transform.position);
		}
		prevInt = currentID;
	}

	[PunRPC]
	public void RPCA_Press(bool more)
	{
		if (more)
		{
			currentID++;
		}
		else
		{
			currentID--;
		}
		if (currentID < 0)
		{
			currentID = textures.Length - 1;
		}
		if (currentID > textures.Length - 1)
		{
			currentID = 0;
		}
		if (textures[currentID] == null)
		{
			rend.enabled = false;
		}
		else
		{
			rend.enabled = true;
			rend.material.SetTexture("_TextureSample0", textures[currentID]);
		}
		flip.Play();
	}
}
