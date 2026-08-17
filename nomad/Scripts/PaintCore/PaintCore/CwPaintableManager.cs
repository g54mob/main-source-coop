using System;
using System.Collections.Generic;
using CW.Common;
using UnityEngine;

namespace PaintCore
{
	[DefaultExecutionOrder(100)]
	[DisallowMultipleComponent]
	[HelpURL("https://carloswilkes.com/Documentation/PaintCore#CwPaintableManager")]
	[AddComponentMenu("CW/Paint Core/CW Paintable Manager")]
	public class CwPaintableManager : MonoBehaviour
	{
		private static LinkedList<CwPaintableManager> instances = new LinkedList<CwPaintableManager>();

		private LinkedListNode<CwPaintableManager> instancesNode;

		[SerializeField]
		private int readPixelsBudget = 4096;

		public static object LastPaintingObject;

		private static int activePaintCount;

		private static List<CwReader> tempReaders = new List<CwReader>();

		public static LinkedList<CwPaintableManager> Instances => instances;

		public int ReadPixelsBudget
		{
			get
			{
				return readPixelsBudget;
			}
			set
			{
				readPixelsBudget = value;
			}
		}

		public static bool IsActivelyPainting => activePaintCount > 0;

		public static event Action<object> OnBeginPainting;

		public static void InvokeOnBeginPainting(object link)
		{
			if (CwPaintableManager.OnBeginPainting != null)
			{
				CwPaintableManager.OnBeginPainting(link);
			}
			LastPaintingObject = link;
		}

		public static void MarkActivelyPainting()
		{
			activePaintCount += 2;
		}

		public static CwPaintableManager GetOrCreateInstance()
		{
			if (instances.Count == 0)
			{
				new GameObject(typeof(CwPaintableManager).Name).AddComponent<CwPaintableManager>();
			}
			return instances.First.Value;
		}

		public static void SubmitAll(CwCommand command, Vector3 position, float radius, int layerMask, CwGroup group, CwModel targetModel, CwPaintableTexture targetTexture)
		{
			DoSubmitAll(command, position, radius, layerMask, group, targetModel, targetTexture);
			CwClone.BuildCloners();
			for (int i = 0; i < CwClone.ClonerCount; i++)
			{
				for (int j = 0; j < CwClone.MatrixCount; j++)
				{
					CwCommand cwCommand = command.SpawnCopy();
					CwClone.Clone(cwCommand, i, j);
					DoSubmitAll(cwCommand, position, radius, layerMask, group, targetModel, targetTexture);
					cwCommand.Pool();
				}
			}
		}

		private static void DoSubmitAll(CwCommand command, Vector3 position, float radius, int layerMask, CwGroup group, CwModel targetModel, CwPaintableTexture targetTexture)
		{
			if (targetModel != null)
			{
				if (targetTexture != null)
				{
					Submit(command, targetModel, targetTexture);
				}
				else
				{
					SubmitAll(command, targetModel, group);
				}
			}
			else if (targetTexture != null)
			{
				Submit(command, targetTexture.Model, targetTexture);
			}
			else
			{
				SubmitAll(command, position, radius, layerMask, group);
			}
		}

		private static void SubmitAll(CwCommand command, Vector3 position, float radius, int layerMask, CwGroup group)
		{
			List<CwModel> list = CwModel.FindOverlap(position, radius, layerMask);
			for (int num = list.Count - 1; num >= 0; num--)
			{
				SubmitAll(command, list[num], group);
			}
		}

		private static void SubmitAll(CwCommand command, CwModel model, CwGroup group)
		{
			List<CwPaintableTexture> list = model.FindPaintableTextures(group);
			for (int num = list.Count - 1; num >= 0; num--)
			{
				Submit(command, model, list[num]);
			}
		}

		public static CwCommand Submit(CwCommand command, CwModel model, CwPaintableTexture paintableTexture)
		{
			if (!command.Preview && CwStateManager.PotentiallyStoreStates)
			{
				CwStateManager.StoreAllStates();
			}
			CwCommand cwCommand = command.SpawnCopy();
			cwCommand.Apply(paintableTexture);
			cwCommand.Model = model;
			cwCommand.Submesh = paintableTexture.Slot.Index;
			paintableTexture.AddCommand(cwCommand);
			return cwCommand;
		}

		protected virtual void OnEnable()
		{
			instancesNode = instances.AddLast(this);
		}

		protected virtual void OnDisable()
		{
			instances.Remove(instancesNode);
			instancesNode = null;
		}

		protected virtual void LateUpdate()
		{
			if (this == instances.First.Value && CwModel.Instances.Count > 0)
			{
				ClearAll();
				UpdateAll();
			}
			else
			{
				CwHelper.Destroy(base.gameObject);
			}
			if (activePaintCount > 1)
			{
				activePaintCount = 1;
			}
			else if (activePaintCount == 1)
			{
				activePaintCount = 0;
			}
			int pixelBudget = readPixelsBudget;
			tempReaders.Clear();
			tempReaders.AddRange(CwReader.Instances);
			foreach (CwReader tempReader in tempReaders)
			{
				tempReader.UpdateRequest(ref pixelBudget);
			}
		}

		private void ClearAll()
		{
			foreach (CwModel instance in CwModel.Instances)
			{
				instance.Prepared = false;
			}
		}

		private void UpdateAll()
		{
			foreach (CwPaintableTexture instance in CwPaintableTexture.Instances)
			{
				instance.ExecuteCommands(sendNotifications: true, doSort: true);
			}
		}
	}
}
