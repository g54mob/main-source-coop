using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Zorro.Recorder;

public class VideoCaptureSRPFeature : ScriptableRendererFeature
{
	public class CameraRegistrationEntry
	{
		public bool isValid;

		public Camera camera;

		public YuvTextureTriplet[] texturePool;

		public RecordingSession session;

		public int currentTextureNum;

		public CameraRegistrationEntry(int numPoolEntries)
		{
			int vIDEO_RENDER_TEXTURE_WIDTH = RecordingConstants.VIDEO_RENDER_TEXTURE_WIDTH;
			int vIDEO_RENDER_TEXTURE_HEIGHT = RecordingConstants.VIDEO_RENDER_TEXTURE_HEIGHT;
			texturePool = new YuvTextureTriplet[numPoolEntries];
			for (int i = 0; i < texturePool.Length; i++)
			{
				texturePool[i] = new YuvTextureTriplet(vIDEO_RENDER_TEXTURE_WIDTH, vIDEO_RENDER_TEXTURE_HEIGHT);
			}
			Reset();
		}

		public void Reset()
		{
			isValid = false;
			camera = null;
			session = null;
			currentTextureNum = 0;
		}

		public YuvTextureTriplet GetNextTextures()
		{
			YuvTextureTriplet result = texturePool[currentTextureNum];
			currentTextureNum = (currentTextureNum + 1) % texturePool.Length;
			return result;
		}
	}

	public class CameraRegistrationManager
	{
		private CameraRegistrationEntry[] m_registeredCameras;

		public CameraRegistrationManager(int numEntries, int numPoolEntries)
		{
			m_registeredCameras = new CameraRegistrationEntry[numEntries];
			for (int i = 0; i < m_registeredCameras.Length; i++)
			{
				m_registeredCameras[i] = new CameraRegistrationEntry(numPoolEntries);
			}
		}

		public Tuple<int, CameraRegistrationEntry> FindRegistrationBySession(RecordingSession session)
		{
			for (int i = 0; i < m_registeredCameras.Length; i++)
			{
				if (m_registeredCameras[i].session == session)
				{
					return new Tuple<int, CameraRegistrationEntry>(i, m_registeredCameras[i]);
				}
			}
			return null;
		}

		public Tuple<int, CameraRegistrationEntry> FindRegistrationByCamera(Camera camera)
		{
			for (int i = 0; i < m_registeredCameras.Length; i++)
			{
				if (m_registeredCameras[i].camera == camera)
				{
					return new Tuple<int, CameraRegistrationEntry>(i, m_registeredCameras[i]);
				}
			}
			return null;
		}

		public bool RegisterNewEntry(Camera camera, RecordingSession session)
		{
			int num = FindEmptyRegistrationIndex();
			if (num == -1)
			{
				return false;
			}
			m_registeredCameras[num].isValid = true;
			m_registeredCameras[num].camera = camera;
			m_registeredCameras[num].session = session;
			m_registeredCameras[num].currentTextureNum = 0;
			Debug.Log($"[VideoCaptureSRP] Associating camera {camera.GetHashCode()} with session {session.GetHashCode()}, index {num}");
			return true;
		}

		private int FindEmptyRegistrationIndex()
		{
			for (int i = 0; i < m_registeredCameras.Length; i++)
			{
				if (!m_registeredCameras[i].isValid)
				{
					return i;
				}
			}
			return -1;
		}

		public bool HasAnyValidTargets()
		{
			for (int i = 0; i < m_registeredCameras.Length; i++)
			{
				if (m_registeredCameras[i].isValid)
				{
					return true;
				}
			}
			return false;
		}

		public void CleanAssociationMap()
		{
			for (int i = 0; i < m_registeredCameras.Length; i++)
			{
				if (m_registeredCameras[i].isValid && m_registeredCameras[i].camera == null)
				{
					Debug.Log($"Found orphaned registration entry, freeing index {i}");
					m_registeredCameras[i].Reset();
				}
			}
		}

		public YuvTextureTriplet GetNextTexturesForCamera(Camera camera)
		{
			return (FindRegistrationByCamera(camera) ?? throw new Exception("Programming error, cannot request textures for a camera that has no registration")).Item2.GetNextTextures();
		}
	}

	public class VideoCaptureSRPFeatureData
	{
		private readonly int k_maxActiveCameras = 2;

		private readonly int k_texturePoolSize = 4;

		public CameraRegistrationManager RegistrationManager;

		public VideoCaptureSRPFeatureData()
		{
			RegistrationManager = new CameraRegistrationManager(k_maxActiveCameras, k_texturePoolSize);
		}

		public void SetRecordingSessionForCamera(Camera camera, RecordingSession session)
		{
			if (session == null || camera == null)
			{
				throw new Exception("Programming error, cannot create a camera -> session mapping if either is null");
			}
			RegistrationManager.CleanAssociationMap();
			Tuple<int, CameraRegistrationEntry> tuple = RegistrationManager.FindRegistrationByCamera(camera);
			if (tuple != null)
			{
				tuple.Item2.session = session;
				Debug.Log("[VideoCaptureSRP] Replacing association between camera " + $"{camera.GetHashCode()} , new session {session.GetHashCode()}");
			}
			else if (!RegistrationManager.RegisterNewEntry(camera, session))
			{
				Debug.LogError("[VideoCaptureSRP] Cannot create camera association as we're out of slots in the SRP plugin");
			}
		}

		public void ClearRegistrationEntryByRecordingSession(RecordingSession session)
		{
			if (session == null)
			{
				throw new Exception("Programming error, trying to remove a camera / session with a null session");
			}
			RegistrationManager.CleanAssociationMap();
			Tuple<int, CameraRegistrationEntry> tuple = RegistrationManager.FindRegistrationBySession(session);
			if (tuple != null)
			{
				int item = tuple.Item1;
				CameraRegistrationEntry item2 = tuple.Item2;
				Debug.Log("[VideoCaptureSRP] Removing association between camera " + $"{item2.camera.GetHashCode()} with session " + $"{item2.session.GetHashCode()}, index {item}");
				item2.Reset();
			}
		}

		public bool HasAnyValidTargets()
		{
			return RegistrationManager.HasAnyValidTargets();
		}
	}

	private class CaptureRenderPass : ScriptableRenderPass
	{
		private Material m_blitMaterial;

		private VideoCaptureSRPFeatureData m_data;

		public CaptureRenderPass(VideoCaptureSRPFeatureData Data)
		{
			Shader shader = Shader.Find("Hidden/SingleChannelPlanarYUV420Output");
			m_blitMaterial = new Material(shader);
			m_data = Data;
		}

		public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
		{
			m_data.RegistrationManager.CleanAssociationMap();
			Camera camera = renderingData.cameraData.camera;
			Tuple<int, CameraRegistrationEntry> tuple = m_data.RegistrationManager.FindRegistrationByCamera(camera);
			if (tuple != null)
			{
				CameraRegistrationEntry item = tuple.Item2;
				CommandBuffer commandBuffer = CommandBufferPool.Get("Video Capture YUV Render");
				RenderTexture targetTexture = camera.targetTexture;
				YuvTextureTriplet nextTexturesForCamera = m_data.RegistrationManager.GetNextTexturesForCamera(camera);
				commandBuffer.SetRenderTarget(nextTexturesForCamera.Y);
				commandBuffer.ClearRenderTarget(clearDepth: true, clearColor: true, Color.black);
				commandBuffer.SetRenderTarget(nextTexturesForCamera.U);
				commandBuffer.ClearRenderTarget(clearDepth: true, clearColor: true, Color.black);
				commandBuffer.SetRenderTarget(nextTexturesForCamera.V);
				commandBuffer.ClearRenderTarget(clearDepth: true, clearColor: true, Color.black);
				commandBuffer.Blit(targetTexture, nextTexturesForCamera.Y, m_blitMaterial, 0);
				commandBuffer.Blit(targetTexture, nextTexturesForCamera.U, m_blitMaterial, 1);
				commandBuffer.Blit(targetTexture, nextTexturesForCamera.V, m_blitMaterial, 2);
				context.ExecuteCommandBuffer(commandBuffer);
				context.Submit();
				CommandBufferPool.Release(commandBuffer);
				item.session.QueueFrame(nextTexturesForCamera);
			}
		}
	}

	public static VideoCaptureSRPFeatureData Data;

	private CaptureRenderPass m_captureRenderPass;

	public override void Create()
	{
		if (Data == null)
		{
			Data = new VideoCaptureSRPFeatureData();
		}
		m_captureRenderPass = new CaptureRenderPass(Data);
		m_captureRenderPass.renderPassEvent = RenderPassEvent.AfterRendering;
	}

	public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
	{
		if (Data.HasAnyValidTargets())
		{
			renderer.EnqueuePass(m_captureRenderPass);
		}
	}
}
