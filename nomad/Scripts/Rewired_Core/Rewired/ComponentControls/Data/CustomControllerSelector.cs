using System;
using Rewired.Utils;
using Rewired.Utils.Attributes;
using UnityEngine;

namespace Rewired.ComponentControls.Data
{
	[Serializable]
	[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
	public sealed class CustomControllerSelector
	{
		[Tooltip("If true, the Custom Controller will be searched for by its source controller id. This can be used with Find in Player and/or Find Using Tag to further refine the search parameters.")]
		[SerializeField]
		[CustomObfuscation(rename = false)]
		private bool _findUsingSourceId = true;

		[Tooltip("The source id of the Custom Controller. This is used to find the Custom Controller if Find Using Source Id is True.")]
		[SerializeField]
		[CustomObfuscation(rename = false)]
		[FieldRange(0, 2147483647)]
		private int _sourceId;

		[Tooltip("If true, the Custom Controller will be found using the tag specified here. This can be used with Find in Player and/or Find Using Source Id to further refine the search parameters.")]
		[SerializeField]
		[CustomObfuscation(rename = false)]
		private bool _findUsingTag;

		[Tooltip("The tag on the Custom Controller you wish to use. This is used to find the Custom Controller.")]
		[SerializeField]
		[CustomObfuscation(rename = false)]
		private string _tag;

		[Tooltip("If true, the Custom Controller will be searched for in the Player specified in the Player Id field. This can be used with Find Using Source Id and/or Find Using Tag to further refine the search parameters.")]
		[SerializeField]
		[CustomObfuscation(rename = false)]
		private bool _findInPlayer;

		[Tooltip("The Player Id of the Player that owns the Custom Controller.")]
		[SerializeField]
		[CustomObfuscation(rename = false)]
		private int _playerId;

		public bool findUsingSourceId
		{
			get
			{
				return _findUsingSourceId;
			}
			set
			{
				if (_findUsingSourceId != value)
				{
					_findUsingSourceId = value;
					LvvmfmwapsHsvKcbuvdDrgaLUjSD();
				}
			}
		}

		public int sourceId
		{
			get
			{
				return _sourceId;
			}
			set
			{
				value = MathTools.Max(0, value);
				if (_sourceId != value)
				{
					_sourceId = value;
					LvvmfmwapsHsvKcbuvdDrgaLUjSD();
				}
			}
		}

		public bool findUsingTag
		{
			get
			{
				return _findUsingTag;
			}
			set
			{
				if (_findUsingTag != value)
				{
					_findUsingTag = value;
					LvvmfmwapsHsvKcbuvdDrgaLUjSD();
				}
			}
		}

		public string tag
		{
			get
			{
				return _tag;
			}
			set
			{
				if (!(_tag == value))
				{
					_tag = value;
					LvvmfmwapsHsvKcbuvdDrgaLUjSD();
				}
			}
		}

		public bool findInPlayer
		{
			get
			{
				return _findInPlayer;
			}
			set
			{
				if (_findInPlayer != value)
				{
					_findInPlayer = value;
					LvvmfmwapsHsvKcbuvdDrgaLUjSD();
				}
			}
		}

		public int playerId
		{
			get
			{
				return _playerId;
			}
			set
			{
				if (_playerId != value)
				{
					_playerId = value;
					LvvmfmwapsHsvKcbuvdDrgaLUjSD();
				}
			}
		}

		internal Rewired.CustomController GetCustomController()
		{
			if (!ReInput.isReady)
			{
				return null;
			}
			if (findInPlayer && ReInput.players.GetPlayer(playerId) == null)
			{
				Logger.LogError("Invalid playerId " + playerId);
				return null;
			}
			for (int i = 0; i < ReInput.controllers.customControllerCount; i++)
			{
				Rewired.CustomController customController = ReInput.controllers.CustomControllers[i];
				if ((!findUsingSourceId || customController.sourceControllerId == sourceId) && (!findUsingTag || !(customController.tag != tag)) && (!findInPlayer || ReInput.controllers.IsControllerAssignedToPlayer(customController.type, customController.id, playerId)))
				{
					return customController;
				}
			}
			return null;
		}

		private void LvvmfmwapsHsvKcbuvdDrgaLUjSD()
		{
		}
	}
}
