using Photon.Pun;

public class VideoClipShareJob
{
	public bool isReRequest;

	public VideoHandle VideoHandle { get; private set; }

	public ClipID ClipID { get; private set; }

	public int ClipOwner { get; private set; }

	public bool IsLocal { get; private set; }

	public bool sending { get; set; }

	public VideoClipShareJob(VideoHandle videoHandle, ClipID clipID, int clipOwner, bool isReRequest)
	{
		this.isReRequest = isReRequest;
		VideoHandle = videoHandle;
		ClipID = clipID;
		ClipOwner = clipOwner;
		IsLocal = PhotonNetwork.LocalPlayer.ActorNumber == clipOwner;
	}
}
