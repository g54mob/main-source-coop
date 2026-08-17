using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Rewired.Data.Mapping;
using Rewired.Internal.Localization;
using Rewired.Utils;
using UnityEngine;

namespace Rewired.Data
{
	[Serializable]
	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
	public sealed class CustomController_Editor
	{
		[Serializable]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public abstract class Element
		{
			public int elementIdentifierId;

			public string name;

			public Element()
			{
			}

			public Element(string P_0, int P_1)
			{
				name = P_0;
				elementIdentifierId = P_1;
			}

			public abstract Element Clone();
		}

		[Serializable]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public sealed class Button : Element
		{
			public Button()
			{
			}

			public Button(string P_0)
				: base(P_0, -1)
			{
			}

			public Button(string P_0, int P_1)
				: base(P_0, P_1)
			{
			}

			public Button(Button P_0)
				: base(P_0.name, P_0.elementIdentifierId)
			{
			}

			public override Element Clone()
			{
				return new Button(this);
			}
		}

		[Serializable]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public sealed class Axis : Element
		{
			public AxisRange range;

			public bool invert;

			public float deadZone;

			public float zero;

			public float min;

			public float max;

			public bool doNotCalibrateRange;

			public AxisSensitivityType sensitivityType;

			public float sensitivity = 1f;

			public AnimationCurve sensitivityCurve;

			public HardwareAxisInfo axisInfo = HardwareAxisInfo.Default;

			public Axis()
			{
			}

			public Axis(string P_0)
				: base(P_0, -1)
			{
				range = AxisRange.Full;
				invert = false;
				deadZone = 0f;
				zero = 0f;
				min = -1f;
				max = 1f;
				sensitivity = 1f;
				sensitivityType = AxisSensitivityType.Multiplier;
				sensitivityCurve = AnimationCurve.Linear(-1f, 1f, 1f, 1f);
				axisInfo = new HardwareAxisInfo(AxisCoordinateMode.Absolute, false, -1f, SpecialAxisType.None);
			}

			[Obsolete("This constructor should not longer be used.", false)]
			public Axis(string P_0, string P_1, string P_2, int P_3, AxisRange P_4, bool P_5, float P_6, float P_7, float P_8, float P_9, bool P_10, HardwareAxisInfo P_11)
				: base(P_0, P_3)
			{
				range = P_4;
				invert = P_5;
				deadZone = P_6;
				zero = P_7;
				min = P_8;
				max = P_9;
				doNotCalibrateRange = P_10;
				axisInfo = MiscTools.DeepClone(P_11) ?? HardwareAxisInfo.Default;
				sensitivity = 1f;
				sensitivityType = AxisSensitivityType.Multiplier;
				sensitivityCurve = AnimationCurve.Linear(-1f, 1f, 1f, 1f);
			}

			public Axis(Axis P_0)
				: base(P_0.name, P_0.elementIdentifierId)
			{
				range = P_0.range;
				invert = P_0.invert;
				deadZone = P_0.deadZone;
				zero = P_0.zero;
				min = P_0.min;
				max = P_0.max;
				doNotCalibrateRange = P_0.doNotCalibrateRange;
				sensitivity = P_0.sensitivity;
				sensitivityType = P_0.sensitivityType;
				sensitivityCurve = UnityTools.Copy(P_0.sensitivityCurve);
				axisInfo = MiscTools.DeepClone(P_0.axisInfo) ?? HardwareAxisInfo.Default;
			}

			public override Element Clone()
			{
				return new Axis(this);
			}
		}

		private sealed class rwWgCwZtnSJqiuzwhRWgymUgcjzC : IEnumerable<ControllerElementIdentifier>, IEnumerable, IEnumerator<ControllerElementIdentifier>, IEnumerator, IDisposable
		{
			private int ianNreCPPDOiwCqyUXsIDopvveHo;

			private ControllerElementIdentifier hnrUKQyskVaTUCnFHiRNHrnsJFRw;

			private int nirewusrlUuEiRqHLmeOMOGKzjT;

			public CustomController_Editor pPdHhRcSZlRXmxcscSXsCwmPiYvT;

			private int SJaCRvXWEwmDiCGeSrAyiqwZCaNKA;

			ControllerElementIdentifier IEnumerator<ControllerElementIdentifier>.Current
			{
				[DebuggerHidden]
				get
				{
					return hnrUKQyskVaTUCnFHiRNHrnsJFRw;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return hnrUKQyskVaTUCnFHiRNHrnsJFRw;
				}
			}

			[DebuggerHidden]
			public rwWgCwZtnSJqiuzwhRWgymUgcjzC(int P_0)
			{
				ianNreCPPDOiwCqyUXsIDopvveHo = P_0;
				nirewusrlUuEiRqHLmeOMOGKzjT = Environment.CurrentManagedThreadId;
			}

			[DebuggerHidden]
			void IDisposable.Dispose()
			{
				ianNreCPPDOiwCqyUXsIDopvveHo = -2;
			}

			private bool MoveNext()
			{
				int num = ianNreCPPDOiwCqyUXsIDopvveHo;
				CustomController_Editor customController_Editor = pPdHhRcSZlRXmxcscSXsCwmPiYvT;
				switch (num)
				{
				default:
					return false;
				case 0:
					ianNreCPPDOiwCqyUXsIDopvveHo = -1;
					if (customController_Editor._elementIdentifiers == null)
					{
						return false;
					}
					SJaCRvXWEwmDiCGeSrAyiqwZCaNKA = 0;
					break;
				case 1:
					ianNreCPPDOiwCqyUXsIDopvveHo = -1;
					SJaCRvXWEwmDiCGeSrAyiqwZCaNKA++;
					break;
				}
				if (SJaCRvXWEwmDiCGeSrAyiqwZCaNKA < customController_Editor._elementIdentifiers.Count)
				{
					hnrUKQyskVaTUCnFHiRNHrnsJFRw = customController_Editor._elementIdentifiers[SJaCRvXWEwmDiCGeSrAyiqwZCaNKA];
					ianNreCPPDOiwCqyUXsIDopvveHo = 1;
					return true;
				}
				return false;
			}

			bool IEnumerator.MoveNext()
			{
				//ILSpy generated this explicit interface implementation from .override directive in MoveNext
				return this.MoveNext();
			}

			[DebuggerHidden]
			void IEnumerator.Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator<ControllerElementIdentifier> IEnumerable<ControllerElementIdentifier>.GetEnumerator()
			{
				rwWgCwZtnSJqiuzwhRWgymUgcjzC rwWgCwZtnSJqiuzwhRWgymUgcjzC2;
				if (ianNreCPPDOiwCqyUXsIDopvveHo == -2 && nirewusrlUuEiRqHLmeOMOGKzjT == Environment.CurrentManagedThreadId)
				{
					ianNreCPPDOiwCqyUXsIDopvveHo = 0;
					rwWgCwZtnSJqiuzwhRWgymUgcjzC2 = this;
				}
				else
				{
					rwWgCwZtnSJqiuzwhRWgymUgcjzC2 = new rwWgCwZtnSJqiuzwhRWgymUgcjzC(0);
					rwWgCwZtnSJqiuzwhRWgymUgcjzC2.pPdHhRcSZlRXmxcscSXsCwmPiYvT = pPdHhRcSZlRXmxcscSXsCwmPiYvT;
				}
				return rwWgCwZtnSJqiuzwhRWgymUgcjzC2;
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return ((IEnumerable<ControllerElementIdentifier>)this).GetEnumerator();
			}
		}

		[SerializeField]
		[CustomObfuscation(rename = false)]
		private string _name;

		[SerializeField]
		[CustomObfuscation(rename = false)]
		private string _descriptiveName;

		[SerializeField]
		[CustomObfuscation(rename = false)]
		private int _id;

		[SerializeField]
		[CustomObfuscation(rename = false)]
		private string _typeGuidString;

		[SerializeField]
		[CustomObfuscation(rename = false)]
		private string _key;

		[SerializeField]
		[CustomObfuscation(rename = false)]
		private List<ControllerElementIdentifier> _elementIdentifiers;

		[SerializeField]
		[CustomObfuscation(rename = false)]
		private List<Axis> _axes;

		[SerializeField]
		[CustomObfuscation(rename = false)]
		private List<Button> _buttons;

		[SerializeField]
		[CustomObfuscation(rename = false)]
		private int _elementIdentifierIdCounter;

		public string name
		{
			get
			{
				return _name;
			}
			internal set
			{
				_name = value;
			}
		}

		public string descriptiveName
		{
			get
			{
				return _descriptiveName;
			}
			internal set
			{
				_descriptiveName = value;
			}
		}

		public int id
		{
			get
			{
				return _id;
			}
			internal set
			{
				_id = value;
			}
		}

		public Guid typeGuid
		{
			get
			{
				return StringTools.ToGuid(_typeGuidString);
			}
			internal set
			{
				_typeGuidString = value.ToString();
			}
		}

		internal string typeGuidString
		{
			get
			{
				return _typeGuidString;
			}
			set
			{
				_typeGuidString = value;
			}
		}

		public string key
		{
			get
			{
				return _key;
			}
			set
			{
				_key = value;
			}
		}

		public List<ControllerElementIdentifier> elementIdentifiers
		{
			get
			{
				return _elementIdentifiers;
			}
			internal set
			{
				_elementIdentifiers = value;
			}
		}

		public List<Axis> axes => _axes;

		public List<Button> buttons => _buttons;

		public int buttonCount
		{
			get
			{
				if (buttons == null)
				{
					return 0;
				}
				return buttons.Count;
			}
		}

		public int axisCount
		{
			get
			{
				if (axes == null)
				{
					return 0;
				}
				return axes.Count;
			}
		}

		public IEnumerable<ControllerElementIdentifier> ElementIdentifiers
		{
			[IteratorStateMachine(typeof(rwWgCwZtnSJqiuzwhRWgymUgcjzC))]
			get
			{
				return new rwWgCwZtnSJqiuzwhRWgymUgcjzC(-2)
				{
					pPdHhRcSZlRXmxcscSXsCwmPiYvT = this
				};
			}
		}

		public CustomController_Editor()
		{
			_axes = new List<Axis>();
			_buttons = new List<Button>();
			_elementIdentifiers = new List<ControllerElementIdentifier>();
		}

		public CustomController_Editor(CustomController_Editor P_0)
		{
			_name = P_0._name;
			_descriptiveName = P_0._descriptiveName;
			_id = P_0._id;
			_typeGuidString = P_0._typeGuidString;
			_key = P_0._key;
			if (P_0._elementIdentifiers != null)
			{
				_elementIdentifiers = new List<ControllerElementIdentifier>(P_0._elementIdentifiers.Count);
				for (int i = 0; i < P_0._elementIdentifiers.Count; i++)
				{
					_elementIdentifiers.Add(P_0._elementIdentifiers[i].Clone());
				}
			}
			if (P_0._axes != null)
			{
				_axes = new List<Axis>(P_0._axes.Count);
				for (int j = 0; j < P_0._axes.Count; j++)
				{
					_axes.Add((Axis)P_0._axes[j].Clone());
				}
			}
			if (P_0._buttons != null)
			{
				_buttons = new List<Button>(P_0._buttons.Count);
				for (int k = 0; k < P_0._buttons.Count; k++)
				{
					_buttons.Add((Button)P_0._buttons[k].Clone());
				}
			}
			_elementIdentifierIdCounter = P_0._elementIdentifierIdCounter;
		}

		public CustomController_Editor Clone()
		{
			return new CustomController_Editor(this);
		}

		public string[] GetElementIdentifierNames()
		{
			int num = ((_elementIdentifiers != null) ? _elementIdentifiers.Count : 0);
			string[] array = new string[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = _elementIdentifiers[i].nonLocalizedName;
			}
			return array;
		}

		public int[] GetElementIdentifierIds()
		{
			int num = ((_elementIdentifiers != null) ? _elementIdentifiers.Count : 0);
			int[] array = new int[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = _elementIdentifiers[i].Rewired_002EInterfaces_002EIControllerElementIdentifierCommon_Internal_002Eid;
			}
			return array;
		}

		public string[] GetElementIdentifierNamesTypeSorted()
		{
			List<string> list = new List<string>();
			List<string> list2 = new List<string>();
			int num = axisCount;
			for (int i = 0; i < num; i++)
			{
				int num2 = IndexOfElementIdentifier(axes[i].elementIdentifierId);
				if (num2 >= 0)
				{
					list2.Add(_elementIdentifiers[num2].nonLocalizedName);
				}
			}
			int num3 = buttonCount;
			for (int j = 0; j < num3; j++)
			{
				int num4 = IndexOfElementIdentifier(buttons[j].elementIdentifierId);
				if (num4 >= 0)
				{
					list.Add(_elementIdentifiers[num4].nonLocalizedName);
				}
			}
			return ListTools.Combine(list2, list).ToArray();
		}

		public int[] GetElementIdentifierIdsTypeSorted()
		{
			List<int> list = new List<int>();
			List<int> list2 = new List<int>();
			int num = axisCount;
			for (int i = 0; i < num; i++)
			{
				list2.Add(axes[i].elementIdentifierId);
			}
			int num2 = buttonCount;
			for (int j = 0; j < num2; j++)
			{
				list.Add(buttons[j].elementIdentifierId);
			}
			return ListTools.Combine(list2, list).ToArray();
		}

		public ControllerElementIdentifier[] GetElementIdentifiersTypeSorted()
		{
			List<ControllerElementIdentifier> list = new List<ControllerElementIdentifier>();
			List<ControllerElementIdentifier> list2 = new List<ControllerElementIdentifier>();
			int num = axisCount;
			for (int i = 0; i < num; i++)
			{
				int num2 = IndexOfElementIdentifier(axes[i].elementIdentifierId);
				if (num2 >= 0)
				{
					list2.Add(_elementIdentifiers[num2]);
				}
			}
			int num3 = buttonCount;
			for (int j = 0; j < num3; j++)
			{
				int num4 = IndexOfElementIdentifier(buttons[j].elementIdentifierId);
				if (num4 >= 0)
				{
					list.Add(_elementIdentifiers[num4]);
				}
			}
			return ListTools.Combine(list2, list).ToArray();
		}

		public bool ContainsElementIdentifier(int id)
		{
			int num = ((_elementIdentifiers != null) ? _elementIdentifiers.Count : 0);
			for (int i = 0; i < num; i++)
			{
				if (_elementIdentifiers[i].Rewired_002EInterfaces_002EIControllerElementIdentifierCommon_Internal_002Eid == id)
				{
					return true;
				}
			}
			return false;
		}

		public int IndexOfElementIdentifier(int id)
		{
			int num = ((_elementIdentifiers != null) ? _elementIdentifiers.Count : 0);
			for (int i = 0; i < num; i++)
			{
				if (_elementIdentifiers[i].Rewired_002EInterfaces_002EIControllerElementIdentifierCommon_Internal_002Eid == id)
				{
					return i;
				}
			}
			return -1;
		}

		public ControllerElementIdentifier GetElementIdentifier(int id)
		{
			int num = IndexOfElementIdentifier(id);
			if (num < 0)
			{
				return null;
			}
			return _elementIdentifiers[num];
		}

		internal ControllerElementType GetEffectiveElementIdentifierType(int elementIdentifierId)
		{
			ControllerElementIdentifier elementIdentifier = GetElementIdentifier(elementIdentifierId);
			if (elementIdentifier == null)
			{
				return ControllerElementType.Axis;
			}
			for (int i = 0; i < axisCount; i++)
			{
				if (axes[i].elementIdentifierId == elementIdentifier.Rewired_002EInterfaces_002EIControllerElementIdentifierCommon_Internal_002Eid)
				{
					return ControllerElementType.Axis;
				}
			}
			for (int j = 0; j < buttonCount; j++)
			{
				if (buttons[j].elementIdentifierId == elementIdentifier.Rewired_002EInterfaces_002EIControllerElementIdentifierCommon_Internal_002Eid)
				{
					return ControllerElementType.Button;
				}
			}
			return elementIdentifier.elementType;
		}

		internal bool GetEffectiveAxisRange(int elementIdentifierId, out AxisRange axisRange)
		{
			ControllerElementIdentifier elementIdentifier = GetElementIdentifier(elementIdentifierId);
			if (elementIdentifier == null)
			{
				axisRange = AxisRange.Full;
				return false;
			}
			for (int i = 0; i < axisCount; i++)
			{
				if (axes[i].elementIdentifierId == elementIdentifier.Rewired_002EInterfaces_002EIControllerElementIdentifierCommon_Internal_002Eid)
				{
					axisRange = axes[i].range;
					if (axes[i].invert)
					{
						axisRange = InputTools.InvertAxisRange(axisRange);
					}
					return true;
				}
			}
			axisRange = AxisRange.Full;
			return false;
		}

		public string[] GetButtonNames()
		{
			int num = ((_buttons != null) ? _buttons.Count : 0);
			string[] array = new string[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = _buttons[i].name;
			}
			return array;
		}

		public int[] GetButtonElementIdentifierIds()
		{
			int num = ((_buttons != null) ? _buttons.Count : 0);
			int[] array = new int[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = _buttons[i].elementIdentifierId;
			}
			return array;
		}

		public string[] GetAxisNames()
		{
			int num = ((_axes != null) ? _axes.Count : 0);
			string[] array = new string[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = _axes[i].name;
			}
			return array;
		}

		public int[] GetAxisElementIdentifierIds()
		{
			int num = ((_axes != null) ? _axes.Count : 0);
			int[] array = new int[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = _axes[i].elementIdentifierId;
			}
			return array;
		}

		public string[] GetElementNames<T>() where T : Element
		{
			if ((object)typeof(T) == typeof(Axis))
			{
				return GetAxisNames();
			}
			if ((object)typeof(T) == typeof(Button))
			{
				return GetButtonNames();
			}
			throw new NotImplementedException();
		}

		public string[] GetElementNames(ControllerElementType type)
		{
			return type switch
			{
				ControllerElementType.Axis => GetAxisNames(), 
				ControllerElementType.Button => GetButtonNames(), 
				_ => throw new NotImplementedException(), 
			};
		}

		public int[] GetElementElementIdentifierIds(ControllerElementType type)
		{
			return type switch
			{
				ControllerElementType.Axis => GetAxisElementIdentifierIds(), 
				ControllerElementType.Button => GetButtonElementIdentifierIds(), 
				_ => throw new NotImplementedException(), 
			};
		}

		public T GetElement<T>(int index) where T : Element
		{
			if (index < 0)
			{
				return null;
			}
			if ((object)typeof(T) == typeof(Axis))
			{
				if (index >= axisCount)
				{
					return null;
				}
				return _axes[index] as T;
			}
			if ((object)typeof(T) == typeof(Button))
			{
				if (index >= buttonCount)
				{
					return null;
				}
				return _buttons[index] as T;
			}
			throw new NotImplementedException();
		}

		public void AddElement(ControllerElementType type)
		{
			if (type == ControllerElementType.Axis)
			{
				AddAxis();
			}
			else
			{
				AddButton();
			}
		}

		public void AddAxis()
		{
			axes.Add((Axis)spoBaDnVJiNphwistZqFljFkIwrr(ControllerElementType.Axis));
		}

		public void AddButton()
		{
			buttons.Add((Button)spoBaDnVJiNphwistZqFljFkIwrr(ControllerElementType.Button));
		}

		public void InsertElement(ControllerElementType type, int index)
		{
			if (type == ControllerElementType.Axis)
			{
				InsertAxis(index);
			}
			else
			{
				InsertButton(index);
			}
		}

		public void InsertAxis(int index)
		{
			if (index < 0 || index >= axes.Count)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			axes.Insert(index, (Axis)spoBaDnVJiNphwistZqFljFkIwrr(ControllerElementType.Axis));
		}

		public void InsertButton(int index)
		{
			if (index < 0 || index >= buttons.Count)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			buttons.Insert(index, (Button)spoBaDnVJiNphwistZqFljFkIwrr(ControllerElementType.Button));
		}

		public void DeleteElement(ControllerElementType type, int index)
		{
			switch (type)
			{
			case ControllerElementType.Axis:
				DeleteElement<Axis>(index);
				break;
			case ControllerElementType.Button:
				DeleteElement<Button>(index);
				break;
			default:
				throw new NotImplementedException();
			}
		}

		public void DeleteElement<T>(int index) where T : Element
		{
			if (index < 0)
			{
				return;
			}
			T val;
			if ((object)typeof(T) == typeof(Axis))
			{
				if (index >= axisCount)
				{
					return;
				}
				val = _axes[index] as T;
				_axes.RemoveAt(index);
			}
			else
			{
				if ((object)typeof(T) != typeof(Button))
				{
					throw new NotImplementedException();
				}
				if (index >= buttonCount)
				{
					return;
				}
				val = _buttons[index] as T;
				_buttons.RemoveAt(index);
			}
			if (_elementIdentifiers == null)
			{
				return;
			}
			for (int num = _elementIdentifiers.Count - 1; num >= 0; num--)
			{
				if (_elementIdentifiers[num].Rewired_002EInterfaces_002EIControllerElementIdentifierCommon_Internal_002Eid == val.elementIdentifierId)
				{
					_elementIdentifiers.RemoveAt(num);
				}
			}
		}

		public bool ReorderElement(ControllerElementType type, int index, bool offsetDown, bool offsetNow)
		{
			switch (type)
			{
			case ControllerElementType.Axis:
			{
				List<Axis> list2 = _axes;
				if (list2 == null || index < 0 || index >= list2.Count)
				{
					return false;
				}
				return ListTools.OffsetAtIndex(list2, index, offsetDown, offsetNow);
			}
			case ControllerElementType.Button:
			{
				List<Button> list = _buttons;
				if (list == null || index < 0 || index >= list.Count)
				{
					return false;
				}
				return ListTools.OffsetAtIndex(list, index, offsetDown, offsetNow);
			}
			default:
				throw new NotImplementedException();
			}
		}

		public void DuplicateElement(ControllerElementType type, int index)
		{
			switch (type)
			{
			case ControllerElementType.Axis:
				hQDXxWqlxFCYMSrbeGTDWGrszwDH(index, axes);
				break;
			case ControllerElementType.Button:
				hQDXxWqlxFCYMSrbeGTDWGrszwDH(index, buttons);
				break;
			default:
				throw new NotImplementedException();
			}
		}

		private void hQDXxWqlxFCYMSrbeGTDWGrszwDH<_0001>(int P_0, List<_0001> P_1) where _0001 : Element
		{
			if (P_1 == null || P_0 < 0 || P_0 >= P_1.Count)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			_0001 val = P_1[P_0];
			string text = StringTools.IterateName(val.name, -1, GetElementNames<_0001>());
			ControllerElementIdentifier controllerElementIdentifier = hXPyLuFqyAgCIlebnevQOIRWoAMU(val.elementIdentifierId, text);
			if (controllerElementIdentifier == null)
			{
				Logger.LogError("Element identifier is missing! Element cannot be duplicated!");
				return;
			}
			_0001 val2 = (_0001)val.Clone();
			val2.elementIdentifierId = controllerElementIdentifier.Rewired_002EInterfaces_002EIControllerElementIdentifierCommon_Internal_002Eid;
			val2.name = text;
			if (P_0 == P_1.Count - 1)
			{
				P_1.Add(val2);
			}
			else
			{
				P_1.Insert(P_0 + 1, val2);
			}
		}

		private ControllerElementIdentifier hXPyLuFqyAgCIlebnevQOIRWoAMU(int P_0, string P_1)
		{
			if (!ContainsElementIdentifier(P_0))
			{
				return null;
			}
			int num = IndexOfElementIdentifier(P_0);
			int elementIdentifierIdCounter = _elementIdentifierIdCounter;
			_elementIdentifierIdCounter++;
			ControllerElementIdentifier controllerElementIdentifier = new ControllerElementIdentifier(new ControllerElementIdentifier.lVegNbJXhHfEVYqBGHidPgoEtALWA
			{
				id = elementIdentifierIdCounter,
				name = P_1,
				positiveName = _elementIdentifiers[num].Rewired_002EInterfaces_002EIControllerElementIdentifierCommon_Internal_002EpositiveName,
				negativeName = _elementIdentifiers[num].Rewired_002EInterfaces_002EIControllerElementIdentifierCommon_Internal_002EnegativeName,
				key = _elementIdentifiers[num].key,
				positiveKey = _elementIdentifiers[num].positiveKey,
				negativeKey = _elementIdentifiers[num].negativeKey,
				elementType = _elementIdentifiers[num].elementType,
				compoundElementType = _elementIdentifiers[num].compoundElementType,
				role = _elementIdentifiers[num].role
			});
			if (num == _elementIdentifiers.Count - 1)
			{
				_elementIdentifiers.Add(controllerElementIdentifier);
			}
			else
			{
				_elementIdentifiers.Insert(num + 1, controllerElementIdentifier);
			}
			return controllerElementIdentifier;
		}

		private Element spoBaDnVJiNphwistZqFljFkIwrr(ControllerElementType P_0)
		{
			switch (P_0)
			{
			case ControllerElementType.Axis:
			{
				string text2 = StringTools.IterateName("Axis ", -1, GetAxisNames());
				ControllerElementIdentifier controllerElementIdentifier2 = gmDUOGIFlwAGGtTeqfShikVyomJg(P_0, text2, string.Empty, string.Empty, key, string.Empty, string.Empty);
				return new Axis(text2)
				{
					elementIdentifierId = controllerElementIdentifier2.Rewired_002EInterfaces_002EIControllerElementIdentifierCommon_Internal_002Eid
				};
			}
			case ControllerElementType.Button:
			{
				string text = StringTools.IterateName("Button ", -1, GetButtonNames());
				ControllerElementIdentifier controllerElementIdentifier = gmDUOGIFlwAGGtTeqfShikVyomJg(P_0, text, string.Empty, string.Empty, key, string.Empty, string.Empty);
				return new Button(text)
				{
					elementIdentifierId = controllerElementIdentifier.Rewired_002EInterfaces_002EIControllerElementIdentifierCommon_Internal_002Eid
				};
			}
			default:
				throw new NotImplementedException();
			}
		}

		private ControllerElementIdentifier gmDUOGIFlwAGGtTeqfShikVyomJg(ControllerElementType P_0, string P_1, string P_2, string P_3, string P_4, string P_5, string P_6)
		{
			int elementIdentifierIdCounter = _elementIdentifierIdCounter;
			_elementIdentifierIdCounter++;
			ControllerElementIdentifier controllerElementIdentifier = new ControllerElementIdentifier(new ControllerElementIdentifier.lVegNbJXhHfEVYqBGHidPgoEtALWA
			{
				id = elementIdentifierIdCounter,
				name = P_1,
				positiveName = P_2,
				negativeName = P_3,
				key = P_4,
				positiveKey = P_5,
				negativeKey = P_6,
				elementType = P_0
			});
			_elementIdentifiers.Add(controllerElementIdentifier);
			return controllerElementIdentifier;
		}

		internal HardwareControllerMap_Game CreateGameHardwareMap()
		{
			int num = axisCount;
			int num2 = buttonCount;
			int[] array = new int[num2];
			int[] array2 = new int[num];
			AxisCalibrationData[] array3 = new AxisCalibrationData[num];
			AxisRange[] array4 = new AxisRange[num];
			HardwareAxisInfo[] array5 = new HardwareAxisInfo[num];
			HardwareButtonInfo[] array6 = new HardwareButtonInfo[num2];
			for (int i = 0; i < num2; i++)
			{
				array[i] = _buttons[i].elementIdentifierId;
				array6[i] = new HardwareButtonInfo();
			}
			for (int j = 0; j < num; j++)
			{
				array2[j] = _axes[j].elementIdentifierId;
				array3[j] = new AxisCalibrationData(true, _axes[j].deadZone, _axes[j].zero, _axes[j].min, _axes[j].max, _axes[j].invert, !_axes[j].doNotCalibrateRange, _axes[j].sensitivityType, _axes[j].sensitivity, UnityTools.Copy(_axes[j].sensitivityCurve));
				array4[j] = _axes[j].range;
				array5[j] = MiscTools.DeepClone(_axes[j].axisInfo) ?? HardwareAxisInfo.Default;
			}
			ControllerElementIdentifier[] elementIdentifiersTypeSorted = GetElementIdentifiersTypeSorted();
			ControllerElementIdentifier[] array7 = new ControllerElementIdentifier[elementIdentifiersTypeSorted.Length];
			for (int k = 0; k < array7.Length; k++)
			{
				if (elementIdentifiersTypeSorted[k] != null)
				{
					array7[k] = new ControllerElementIdentifier(elementIdentifiersTypeSorted[k]);
				}
			}
			List<string> list = new List<string> { _key };
			DeviceLocalizationInfo deviceLocalizationInfo = new DeviceLocalizationInfo(ControllerType.Custom, false, typeGuid, list, null);
			return new HardwareControllerMap_Game(_name, deviceLocalizationInfo, _id, array7, array, array2, array3, array4, array5, array6, null);
		}
	}
}
