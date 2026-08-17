using System;
using System.Collections.Generic;
using PaintCore;
using UnityEngine;

namespace PaintIn3D
{
	[HelpURL("https://carloswilkes.com/Documentation/PaintIn3D#CwCommandSerialization")]
	[AddComponentMenu("CW/Paint in 3D/CW Command Serialization")]
	public class CwCommandSerialization : MonoBehaviour
	{
		[Serializable]
		public struct CommandData
		{
			public CwPaintableTexture PaintableTexture;

			[SerializeReference]
			public CwCommand LocalCommand;
		}

		[SerializeField]
		private bool listening = true;

		[SerializeField]
		private List<CommandData> commandDatas = new List<CommandData>();

		public bool Listening
		{
			get
			{
				return listening;
			}
			set
			{
				listening = value;
			}
		}

		[ContextMenu("Clear")]
		public void Clear()
		{
			foreach (CommandData commandData in commandDatas)
			{
				commandData.LocalCommand.Pool();
			}
			commandDatas.Clear();
		}

		[ContextMenu("Rebuild Random Command")]
		public void RebuildRandomCommand()
		{
			bool flag = listening;
			listening = false;
			foreach (CwPaintableTexture instance in CwPaintableTexture.Instances)
			{
				instance.Clear();
			}
			if (commandDatas.Count > 0)
			{
				int index = UnityEngine.Random.Range(0, commandDatas.Count);
				CommandData commandData = commandDatas[index];
				if (commandData.PaintableTexture != null)
				{
					CwCommand cwCommand = commandData.LocalCommand.SpawnCopyWorld(commandData.PaintableTexture.transform);
					commandData.PaintableTexture.AddCommand(cwCommand);
					cwCommand.Pool();
				}
			}
			listening = flag;
		}

		[ContextMenu("Serialize And Deserialize")]
		public void SerializeAndDeserialize()
		{
			JsonUtility.FromJsonOverwrite(JsonUtility.ToJson(this), this);
		}

		protected virtual void OnEnable()
		{
			CwPaintableTexture.OnAddCommandGlobal += HandleAddCommandGlobal;
		}

		protected virtual void OnDisable()
		{
			CwPaintableTexture.OnAddCommandGlobal -= HandleAddCommandGlobal;
		}

		private void HandleAddCommandGlobal(CwPaintableTexture paintableTexture, CwCommand command)
		{
			if (listening && !command.Preview)
			{
				CwCommand localCommand = command.SpawnCopyLocal(paintableTexture.transform);
				CommandData item = new CommandData
				{
					PaintableTexture = paintableTexture,
					LocalCommand = localCommand
				};
				commandDatas.Add(item);
			}
		}
	}
}
