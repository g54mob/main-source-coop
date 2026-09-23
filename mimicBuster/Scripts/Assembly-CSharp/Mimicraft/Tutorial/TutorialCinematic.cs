using System;
using System.Collections;
using System.Collections.Generic;
using Mimicraft.Cameras;
using Mimicraft.VoxelEditor;
using Mimicraft.VoxelEditor.Core;
using UnityEngine;

namespace Mimicraft.Tutorial
{
	public class TutorialCinematic : MonoBehaviour
	{
		private const float ApproachSeconds = 4f;

		private const float HoldSeconds = 1.2f;

		private const float CompareSeconds = 3f;

		private const float ReturnSeconds = 1f;

		private const float StartDistance = 3.5f;

		private const float EndDistance = 1.1f;

		private const float EyeHeight = 1.6f;

		private static readonly Color32 BareColor = new Color32(139, 90, 43, byte.MaxValue);

		private TutorialContext context;

		private readonly List<Behaviour> pausedDrivers = new List<Behaviour>();

		private readonly List<Renderer> hiddenRenderers = new List<Renderer>();

		private GameObject bareCube;

		private Func<EditorState, bool> gateBefore;

		private Coroutine run;

		public bool Finished { get; private set; }

		public static TutorialCinematic Play(TutorialContext context)
		{
			TutorialCinematic tutorialCinematic = new GameObject("TutorialCinematic").AddComponent<TutorialCinematic>();
			tutorialCinematic.context = context;
			tutorialCinematic.run = tutorialCinematic.StartCoroutine(tutorialCinematic.Run());
			return tutorialCinematic;
		}

		public void Abort()
		{
			if (!Finished)
			{
				if (run != null)
				{
					StopCoroutine(run);
				}
				Restore();
			}
		}

		private IEnumerator Run()
		{
			Camera camera = context.Camera;
			if (camera == null || context.Model == null)
			{
				Finished = true;
				UnityEngine.Object.Destroy(base.gameObject);
				yield break;
			}
			gateBefore = VoxelEditorSettings.TutorialGate;
			VoxelEditorSettings.TutorialGate = (EditorState _) => false;
			PauseDriver(camera.GetComponent<OrbitCamera>());
			PauseDriver(camera.GetComponent<ThirdPersonCamera>());
			Vector3 center = context.WorldCenter();
			float num = context.WorldRadius();
			float y = context.FloorPoint().y;
			Vector3 vector = context.ClearDirection(camera.transform.forward * -1f, 3.5f);
			Vector3 start = center + vector * (num + 3.5f);
			Vector3 end = center + vector * (num + 1.1f);
			start.y = y + 1.6f;
			end.y = y + 1.52f;
			Transform camT = camera.transform;
			for (float t = 0f; t < 4f; t += Time.deltaTime)
			{
				float t2 = Mathf.SmoothStep(0f, 1f, t / 4f);
				camT.position = Vector3.Lerp(start, end, t2);
				camT.LookAt(center);
				yield return null;
			}
			camT.position = end;
			camT.LookAt(center);
			yield return new WaitForSeconds(1.2f);
			ShowBareCube();
			yield return new WaitForSeconds(3f);
			HideBareCube();
			yield return new WaitForSeconds(1f);
			Restore();
		}

		private void PauseDriver(Behaviour driver)
		{
			if (!(driver == null) && driver.enabled)
			{
				driver.enabled = false;
				pausedDrivers.Add(driver);
			}
		}

		private void ShowBareCube()
		{
			VoxelModel model = context.Model;
			foreach (VoxelEditorController item in VoxelFocusManager.GetAllUsable())
			{
				if (!(item.Model == null))
				{
					MeshRenderer component = item.Model.GetComponent<MeshRenderer>();
					if (component != null && component.enabled)
					{
						component.enabled = false;
						hiddenRenderers.Add(component);
					}
				}
			}
			bareCube = new GameObject("TutorialBareCube");
			bareCube.transform.SetParent(model.transform, worldPositionStays: false);
			MeshFilter meshFilter = bareCube.AddComponent<MeshFilter>();
			MeshRenderer meshRenderer = bareCube.AddComponent<MeshRenderer>();
			meshFilter.sharedMesh = VoxelMeshBuilder.BuildMesh(VoxelGrid.CreateDefaultCube(size: Mathf.Max(1, Mathf.RoundToInt(model.InitialWorldSize / model.VoxelSize)), color: BareColor));
			MeshRenderer component2 = model.GetComponent<MeshRenderer>();
			meshRenderer.sharedMaterial = ((component2 != null) ? component2.sharedMaterial : model.LitMaterial);
		}

		private void HideBareCube()
		{
			if (bareCube != null)
			{
				MeshFilter component = bareCube.GetComponent<MeshFilter>();
				if (component != null && component.sharedMesh != null)
				{
					UnityEngine.Object.Destroy(component.sharedMesh);
				}
				UnityEngine.Object.Destroy(bareCube);
				bareCube = null;
			}
			foreach (Renderer hiddenRenderer in hiddenRenderers)
			{
				if (hiddenRenderer != null)
				{
					hiddenRenderer.enabled = true;
				}
			}
			hiddenRenderers.Clear();
		}

		private void Restore()
		{
			HideBareCube();
			foreach (Behaviour pausedDriver in pausedDrivers)
			{
				if (pausedDriver != null)
				{
					pausedDriver.enabled = true;
				}
			}
			pausedDrivers.Clear();
			VoxelEditorSettings.TutorialGate = gateBefore;
			Finished = true;
			UnityEngine.Object.Destroy(base.gameObject);
		}

		private void OnDestroy()
		{
			if (!Finished)
			{
				Restore();
			}
		}
	}
}
