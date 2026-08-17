using System;
using Rewired.Interfaces;
using UnityEngine;

namespace Rewired.Data
{
	[Serializable]
	[CustomClassObfuscation(renamePubIntMembers = false, renamePrivateMembers = true)]
	[CustomObfuscation(rename = false)]
	internal sealed class ControllerTemplateElementIdentifier_Editor : ControllerTemplateElementIdentifier, IControllerElementIdentifierCommon_Internal, IControllerTemplateElementIdentifier, IControllerTemplateElementIdentifier_Editor
	{
		[SerializeField]
		[CustomObfuscation(rename = false)]
		private string _scriptingName;

		[CustomObfuscation(rename = false)]
		[SerializeField]
		private string _alternateScriptingName;

		[CustomObfuscation(rename = false)]
		[SerializeField]
		private bool _excludeFromExport;

		[CustomObfuscation(rename = false)]
		[SerializeField]
		private bool _useEditorElementTypeOverride;

		[CustomObfuscation(rename = false)]
		[SerializeField]
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

		internal override bool useEditorElementTypeOverride => _useEditorElementTypeOverride;

		internal override ControllerElementType editorElementTypeOverride => _editorElementTypeOverride;

		internal ControllerTemplateElementType effectiveElementType
		{
			get
			{
				if (!_useEditorElementTypeOverride)
				{
					return base.elementType;
				}
				return OzpbBOHoTkskKeBaOXWaTXQCmjjBA.yYbzPczOHmktyhazvMykohFtarNh(_editorElementTypeOverride, false);
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
