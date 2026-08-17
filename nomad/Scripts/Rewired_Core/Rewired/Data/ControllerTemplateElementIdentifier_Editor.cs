using System;
using Rewired.Interfaces;
using UnityEngine;

namespace Rewired.Data
{
	[Serializable]
	[CustomObfuscation(rename = false)]
	[CustomClassObfuscation(renamePubIntMembers = false, renamePrivateMembers = true)]
	internal sealed class ControllerTemplateElementIdentifier_Editor : ControllerTemplateElementIdentifier, IControllerTemplateElementIdentifier_Editor, IControllerTemplateElementIdentifier, IControllerElementIdentifierCommon_Internal
	{
		[SerializeField]
		[CustomObfuscation(rename = false)]
		private string _scriptingName;

		[SerializeField]
		[CustomObfuscation(rename = false)]
		private string _alternateScriptingName;

		[SerializeField]
		[CustomObfuscation(rename = false)]
		private bool _excludeFromExport;

		[SerializeField]
		[CustomObfuscation(rename = false)]
		private bool _useEditorElementTypeOverride;

		[SerializeField]
		[CustomObfuscation(rename = false)]
		private ControllerElementType _editorElementTypeOverride;

		internal string scriptingName
		{
			get
			{
				return _scriptingName;
			}
			set
			{
				_scriptingName = value;
			}
		}

		internal string alternateScriptingName
		{
			get
			{
				return _alternateScriptingName;
			}
			set
			{
				_alternateScriptingName = value;
			}
		}

		internal bool excludeFromExport => _excludeFromExport;

		bool ControllerTemplateElementIdentifier.useEditorElementTypeOverride => _useEditorElementTypeOverride;

		ControllerElementType ControllerTemplateElementIdentifier.editorElementTypeOverride => _editorElementTypeOverride;

		internal ControllerTemplateElementType effectiveElementType
		{
			get
			{
				if (!_useEditorElementTypeOverride)
				{
					return base.Rewired_002EInterfaces_002EIControllerTemplateElementIdentifier_002EelementType;
				}
				return cVDyIiOsEfJNYzVuZSmuEXqylgT.WMwTgbsyrBXDwSyPLcPBVwCFfUPd(_editorElementTypeOverride, false);
			}
		}

		string IControllerTemplateElementIdentifier_Editor.scriptingName => _scriptingName;

		string IControllerTemplateElementIdentifier_Editor.alternateScriptingName => _alternateScriptingName;

		public ControllerTemplateElementIdentifier_Editor()
		{
		}

		public ControllerTemplateElementIdentifier_Editor(ControllerTemplateElementIdentifier_Editor P_0)
			: base(P_0)
		{
			_scriptingName = P_0._scriptingName;
			_alternateScriptingName = P_0._alternateScriptingName;
			_excludeFromExport = P_0._excludeFromExport;
			_editorElementTypeOverride = P_0._editorElementTypeOverride;
			_useEditorElementTypeOverride = P_0._useEditorElementTypeOverride;
		}

		public override ControllerTemplateElementIdentifier Clone()
		{
			return new ControllerTemplateElementIdentifier_Editor(this);
		}
	}
}
