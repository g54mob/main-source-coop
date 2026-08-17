using System;
using Rewired.Interfaces;
using UnityEngine;

namespace Rewired
{
	[Serializable]
	[CustomClassObfuscation(renamePubIntMembers = false, renamePrivateMembers = true)]
	public sealed class ControllerElementIdentifier : IControllerElementIdentifierCommon_Internal
	{
		[SerializeField]
		[CustomObfuscation(rename = false)]
		private int _id;

		[SerializeField]
		[CustomObfuscation(rename = false)]
		private string _name;

		[CustomObfuscation(rename = false)]
		[SerializeField]
		private string _positiveName;

		[SerializeField]
		[CustomObfuscation(rename = false)]
		private string _negativeName;

		[SerializeField]
		[CustomObfuscation(rename = false)]
		private ControllerElementType _elementType;

		[CustomObfuscation(rename = false)]
		[SerializeField]
		private CompoundControllerElementType _compoundElementType;

		internal readonly bool isMappableOnPlatform;

		private bool TxSJwJoaEWVxcucVapNSpYPnWYhO;

		private static ControllerElementIdentifier TCiwSGTHwRAlWXdcLyuCBNCcQIzT;

		public int id => _id;

		public string name
		{
			get
			{
				return _name;
			}
			internal set
			{
				jkZRClFCcxCwnCTMVClsLjAEHDHAb();
				_name = value;
			}
		}

		public string positiveName
		{
			get
			{
				return _positiveName;
			}
			internal set
			{
				jkZRClFCcxCwnCTMVClsLjAEHDHAb();
				_positiveName = value;
			}
		}

		public string negativeName
		{
			get
			{
				return _negativeName;
			}
			internal set
			{
				jkZRClFCcxCwnCTMVClsLjAEHDHAb();
				_negativeName = value;
			}
		}

		public ControllerElementType elementType => _elementType;

		public CompoundControllerElementType compoundElementType => _compoundElementType;

		internal bool isCompoundElement => _elementType == ControllerElementType.CompoundElement;

		object IControllerElementIdentifierCommon_Internal.elementType => _elementType;

		bool IControllerElementIdentifierCommon_Internal.useEditorElementTypeOverride => false;

		ControllerElementType IControllerElementIdentifierCommon_Internal.editorElementTypeOverride => _elementType;

		internal static ControllerElementIdentifier BlankReadOnly
		{
			get
			{
				if (TCiwSGTHwRAlWXdcLyuCBNCcQIzT == null)
				{
					ControllerElementIdentifier obj = new ControllerElementIdentifier
					{
						_id = -1,
						TxSJwJoaEWVxcucVapNSpYPnWYhO = true
					};
					TCiwSGTHwRAlWXdcLyuCBNCcQIzT = obj;
					return obj;
				}
				return TCiwSGTHwRAlWXdcLyuCBNCcQIzT;
			}
		}

		public ControllerElementIdentifier()
		{
		}

		public ControllerElementIdentifier(ControllerElementIdentifier P_0)
		{
			isMappableOnPlatform = P_0.isMappableOnPlatform;
			_id = P_0._id;
			_name = P_0._name;
			_positiveName = P_0._positiveName;
			_negativeName = P_0._negativeName;
			_elementType = P_0._elementType;
			_compoundElementType = P_0._compoundElementType;
		}

		internal ControllerElementIdentifier(int P_0, string P_1, string P_2, string P_3, ControllerElementType P_4, CompoundControllerElementType P_5, bool P_6)
		{
			_id = P_0;
			_name = P_1;
			_positiveName = P_2;
			_negativeName = P_3;
			_elementType = P_4;
			_compoundElementType = P_5;
			isMappableOnPlatform = P_6;
		}

		internal ControllerElementIdentifier(int P_0, string P_1, string P_2, string P_3, ControllerElementType P_4, bool P_5)
		{
			_id = P_0;
			_name = P_1;
			_positiveName = P_2;
			_negativeName = P_3;
			_elementType = P_4;
			_compoundElementType = CompoundControllerElementType.Axis2D;
			isMappableOnPlatform = P_5;
		}

		internal ControllerElementIdentifier(ControllerElementIdentifier P_0, bool P_1, ControllerElementType P_2)
			: this(P_0)
		{
			_elementType = P_2;
			isMappableOnPlatform = P_1;
		}

		public ControllerElementIdentifier Clone()
		{
			return new ControllerElementIdentifier(this);
		}

		public string GetDisplayName(ControllerElementType actualElementType, AxisRange axisRange)
		{
			switch (actualElementType)
			{
			case ControllerElementType.Axis:
				switch (axisRange)
				{
				case AxisRange.Full:
					return name;
				case AxisRange.Positive:
					if (string.IsNullOrEmpty(positiveName))
					{
						return name + " +";
					}
					return positiveName;
				case AxisRange.Negative:
					if (string.IsNullOrEmpty(negativeName))
					{
						return name + " -";
					}
					return negativeName;
				default:
					throw new NotImplementedException();
				}
			case ControllerElementType.Button:
				return name;
			case ControllerElementType.CompoundElement:
				return name;
			default:
				throw new NotImplementedException();
			}
		}

		public string GetDisplayName(AxisRange axisRange)
		{
			return GetDisplayName(_elementType, axisRange);
		}

		private void jkZRClFCcxCwnCTMVClsLjAEHDHAb()
		{
			if (TxSJwJoaEWVxcucVapNSpYPnWYhO)
			{
				throw new Exception("The object is marked readonly and you are trying to modify its values.");
			}
		}
	}
}
