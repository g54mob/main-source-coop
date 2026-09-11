using System;
using System.Collections.Generic;
using Zorro.Core;

public class CameraHandler : Singleton<CameraHandler>
{
	private Dictionary<Guid, VideoCamera> m_cameras = new Dictionary<Guid, VideoCamera>();

	public static void RegisterCamera(Guid id, VideoCamera camera)
	{
		if (!(Singleton<CameraHandler>.Instance == null))
		{
			Singleton<CameraHandler>.Instance.m_cameras.Add(id, camera);
		}
	}

	public static void UnregisterCamera(Guid id)
	{
		if (!(Singleton<CameraHandler>.Instance == null))
		{
			Singleton<CameraHandler>.Instance.m_cameras.Remove(id);
		}
	}

	public static bool TryGetCamera(Guid instanceDataGuid, out VideoCamera videoCamera)
	{
		if (Singleton<CameraHandler>.Instance == null)
		{
			videoCamera = null;
			return false;
		}
		return Singleton<CameraHandler>.Instance.m_cameras.TryGetValue(instanceDataGuid, out videoCamera);
	}
}
