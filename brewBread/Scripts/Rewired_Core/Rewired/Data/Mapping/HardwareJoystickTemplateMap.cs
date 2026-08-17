using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Rewired.Interfaces;
using Rewired.Utils;
using Rewired.Utils.Libraries.TinyJson;
using UnityEngine;

namespace Rewired.Data.Mapping
{
	public sealed class HardwareJoystickTemplateMap : HardwareControllerTemplateMap, IHardwareControllerMap, IHardwareControllerTemplateMap, IHardwareControllerMap_Internal, IHardwareControllerTemplateMap_Internal
	{
		[Serializable]
		public sealed class Entry
		{
			public int id;

			public string name;

			public string joystickGuid;

			public string fileGuid;

			public List<ElementIdentifierMap> elementIdentifierMappings;

			public Guid JoystickGuid
			{
				get
				{
					try
					{
						return new Guid(joystickGuid);
					}
					catch
					{
						Logger.LogWarning("Error converting string to Guid due to invalid characters or bad Guid string format. Guid string = \"" + joystickGuid + "\"");
						return Guid.Empty;
					}
				}
			}

			public int GetJoystickElementId(int templateElementId)
			{
				if (elementIdentifierMappings == null)
				{
					return -1;
				}
				for (int i = 0; i < elementIdentifierMappings.Count; i++)
				{
					if (elementIdentifierMappings[i].templateId == templateElementId)
					{
						return elementIdentifierMappings[i].joystickId;
					}
				}
				return -1;
			}

			public int GetTemplateElementId(int joystickElementId)
			{
				if (elementIdentifierMappings == null)
				{
					return -1;
				}
				for (int i = 0; i < elementIdentifierMappings.Count; i++)
				{
					if (elementIdentifierMappings[i].joystickId == joystickElementId)
					{
						return elementIdentifierMappings[i].templateId;
					}
				}
				return -1;
			}

			public ElementIdentifierMap GetElementIdentifierMap(int templateId)
			{
				if (elementIdentifierMappings == null)
				{
					return null;
				}
				for (int i = 0; i < elementIdentifierMappings.Count; i++)
				{
					if (elementIdentifierMappings[i].templateId == templateId)
					{
						return elementIdentifierMappings[i];
					}
				}
				return null;
			}

			public void GetElementIdentifierMaps(int templateId, List<ElementIdentifierMap> results)
			{
				if (results == null || elementIdentifierMappings == null)
				{
					return;
				}
				for (int i = 0; i < elementIdentifierMappings.Count; i++)
				{
					if (elementIdentifierMappings[i].templateId == templateId)
					{
						results.Add(elementIdentifierMappings[i]);
					}
				}
			}
		}

		[Serializable]
		public sealed class ElementIdentifierMap
		{
			public int templateId;

			public int joystickId;

			public int joystickId2;

			public bool splitAxis;
		}

		[Serializable]
		public sealed class SpecialElementEntry : IControllerTemplateMapSpecialElement_Internal
		{
			public int elementIdentifierId = -1;

			public string data;

			T IControllerTemplateMapSpecialElement_Internal.GetMapping<T>()
			{
				JsonParser.TryFromJson<T>(data, out var value);
				return value;
			}
		}

		private sealed class WHRsxpxEybuaFhhfBXutkRQNiVQX : IDisposable, IEnumerable, IEnumerator, IEnumerable<IControllerElementIdentifierCommon_Internal>, IEnumerator<IControllerElementIdentifierCommon_Internal>
		{
			private int RxAoyfYzYDsYonLGXsvUgwChukLk;

			private IControllerElementIdentifierCommon_Internal VqEePGSMyrGKIqkWibsjeHcWPSIx;

			private int wLBvvzlDcbAEPyDfgnprolEZBsgN;

			public HardwareJoystickTemplateMap TtytLoUfsgUyhsklaKccrnoMiiek;

			private int hWZNbLCFLBBWXNZxiKeGtzgrnTsg;

			IControllerElementIdentifierCommon_Internal IEnumerator<IControllerElementIdentifierCommon_Internal>.Current
			{
				[DebuggerHidden]
				get
				{
					return VqEePGSMyrGKIqkWibsjeHcWPSIx;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return VqEePGSMyrGKIqkWibsjeHcWPSIx;
				}
			}

			[DebuggerHidden]
			public WHRsxpxEybuaFhhfBXutkRQNiVQX(int P_0)
			{
				RxAoyfYzYDsYonLGXsvUgwChukLk = P_0;
				wLBvvzlDcbAEPyDfgnprolEZBsgN = Thread.CurrentThread.ManagedThreadId;
			}

			[DebuggerHidden]
			void IDisposable.Dispose()
			{
			}

			private bool MoveNext()
			{
				int rxAoyfYzYDsYonLGXsvUgwChukLk = RxAoyfYzYDsYonLGXsvUgwChukLk;
				HardwareJoystickTemplateMap ttytLoUfsgUyhsklaKccrnoMiiek = TtytLoUfsgUyhsklaKccrnoMiiek;
				switch (rxAoyfYzYDsYonLGXsvUgwChukLk)
				{
				default:
					return false;
				case 0:
					RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
					if (ttytLoUfsgUyhsklaKccrnoMiiek.elementIdentifiers == null)
					{
						return false;
					}
					hWZNbLCFLBBWXNZxiKeGtzgrnTsg = 0;
					break;
				case 1:
					RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
					hWZNbLCFLBBWXNZxiKeGtzgrnTsg++;
					break;
				}
				if (hWZNbLCFLBBWXNZxiKeGtzgrnTsg < ttytLoUfsgUyhsklaKccrnoMiiek.elementIdentifiers.Length)
				{
					VqEePGSMyrGKIqkWibsjeHcWPSIx = ttytLoUfsgUyhsklaKccrnoMiiek.elementIdentifiers[hWZNbLCFLBBWXNZxiKeGtzgrnTsg];
					RxAoyfYzYDsYonLGXsvUgwChukLk = 1;
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
			IEnumerator<IControllerElementIdentifierCommon_Internal> IEnumerable<IControllerElementIdentifierCommon_Internal>.GetEnumerator()
			{
				WHRsxpxEybuaFhhfBXutkRQNiVQX wHRsxpxEybuaFhhfBXutkRQNiVQX;
				if (RxAoyfYzYDsYonLGXsvUgwChukLk == -2 && wLBvvzlDcbAEPyDfgnprolEZBsgN == Thread.CurrentThread.ManagedThreadId)
				{
					RxAoyfYzYDsYonLGXsvUgwChukLk = 0;
					wHRsxpxEybuaFhhfBXutkRQNiVQX = this;
				}
				else
				{
					wHRsxpxEybuaFhhfBXutkRQNiVQX = new WHRsxpxEybuaFhhfBXutkRQNiVQX(0);
					wHRsxpxEybuaFhhfBXutkRQNiVQX.TtytLoUfsgUyhsklaKccrnoMiiek = TtytLoUfsgUyhsklaKccrnoMiiek;
				}
				return wHRsxpxEybuaFhhfBXutkRQNiVQX;
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return ((IEnumerable<IControllerElementIdentifierCommon_Internal>)this).GetEnumerator();
			}
		}

		private sealed class PdmLmOZATAAgJaEHqmEOUkrITuVtA : IDisposable, IEnumerable, IEnumerator, IEnumerable<ControllerTemplateElementIdentifier>, IEnumerator<ControllerTemplateElementIdentifier>
		{
			private int RxAoyfYzYDsYonLGXsvUgwChukLk;

			private ControllerTemplateElementIdentifier VqEePGSMyrGKIqkWibsjeHcWPSIx;

			private int wLBvvzlDcbAEPyDfgnprolEZBsgN;

			public HardwareJoystickTemplateMap TtytLoUfsgUyhsklaKccrnoMiiek;

			private int hWZNbLCFLBBWXNZxiKeGtzgrnTsg;

			ControllerTemplateElementIdentifier IEnumerator<ControllerTemplateElementIdentifier>.Current
			{
				[DebuggerHidden]
				get
				{
					return VqEePGSMyrGKIqkWibsjeHcWPSIx;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return VqEePGSMyrGKIqkWibsjeHcWPSIx;
				}
			}

			[DebuggerHidden]
			public PdmLmOZATAAgJaEHqmEOUkrITuVtA(int P_0)
			{
				RxAoyfYzYDsYonLGXsvUgwChukLk = P_0;
				wLBvvzlDcbAEPyDfgnprolEZBsgN = Thread.CurrentThread.ManagedThreadId;
			}

			[DebuggerHidden]
			void IDisposable.Dispose()
			{
			}

			private bool MoveNext()
			{
				int rxAoyfYzYDsYonLGXsvUgwChukLk = RxAoyfYzYDsYonLGXsvUgwChukLk;
				HardwareJoystickTemplateMap ttytLoUfsgUyhsklaKccrnoMiiek = TtytLoUfsgUyhsklaKccrnoMiiek;
				switch (rxAoyfYzYDsYonLGXsvUgwChukLk)
				{
				default:
					return false;
				case 0:
					RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
					if (ttytLoUfsgUyhsklaKccrnoMiiek.elementIdentifiers == null)
					{
						return false;
					}
					hWZNbLCFLBBWXNZxiKeGtzgrnTsg = 0;
					break;
				case 1:
					RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
					hWZNbLCFLBBWXNZxiKeGtzgrnTsg++;
					break;
				}
				if (hWZNbLCFLBBWXNZxiKeGtzgrnTsg < ttytLoUfsgUyhsklaKccrnoMiiek.elementIdentifiers.Length)
				{
					VqEePGSMyrGKIqkWibsjeHcWPSIx = ttytLoUfsgUyhsklaKccrnoMiiek.elementIdentifiers[hWZNbLCFLBBWXNZxiKeGtzgrnTsg];
					RxAoyfYzYDsYonLGXsvUgwChukLk = 1;
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
			IEnumerator<ControllerTemplateElementIdentifier> IEnumerable<ControllerTemplateElementIdentifier>.GetEnumerator()
			{
				PdmLmOZATAAgJaEHqmEOUkrITuVtA pdmLmOZATAAgJaEHqmEOUkrITuVtA;
				if (RxAoyfYzYDsYonLGXsvUgwChukLk == -2 && wLBvvzlDcbAEPyDfgnprolEZBsgN == Thread.CurrentThread.ManagedThreadId)
				{
					RxAoyfYzYDsYonLGXsvUgwChukLk = 0;
					pdmLmOZATAAgJaEHqmEOUkrITuVtA = this;
				}
				else
				{
					pdmLmOZATAAgJaEHqmEOUkrITuVtA = new PdmLmOZATAAgJaEHqmEOUkrITuVtA(0);
					pdmLmOZATAAgJaEHqmEOUkrITuVtA.TtytLoUfsgUyhsklaKccrnoMiiek = TtytLoUfsgUyhsklaKccrnoMiiek;
				}
				return pdmLmOZATAAgJaEHqmEOUkrITuVtA;
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return ((IEnumerable<ControllerTemplateElementIdentifier>)this).GetEnumerator();
			}
		}

		[CustomObfuscation(rename = false)]
		[SerializeField]
		private string controllerName;

		[SerializeField]
		[CustomObfuscation(rename = false)]
		private string description;

		[CustomObfuscation(rename = false)]
		[SerializeField]
		private string templateGuid;

		[SerializeField]
		[CustomObfuscation(rename = false)]
		private string className;

		[CustomObfuscation(rename = false)]
		[SerializeField]
		private ControllerTemplateElementIdentifier_Editor[] elementIdentifiers;

		[SerializeField]
		[CustomObfuscation(rename = false)]
		private List<Entry> joysticks;

		[SerializeField]
		[CustomObfuscation(rename = false)]
		private SpecialElementEntry[] specialElements = new SpecialElementEntry[0];

		[SerializeField]
		[CustomObfuscation(rename = false)]
		private int elementIdentifierIdCounter;

		[CustomObfuscation(rename = false)]
		[SerializeField]
		private int joystickIdCounter;

		public override Guid Guid => StringTools.ToGuid(templateGuid);

		public string ControllerName => controllerName;

		public string ClassName => className;

		public IEnumerable<ControllerTemplateElementIdentifier> ElementIdentifiers => new PdmLmOZATAAgJaEHqmEOUkrITuVtA(-2)
		{
			TtytLoUfsgUyhsklaKccrnoMiiek = this
		};

		IEnumerable<IControllerElementIdentifierCommon_Internal> IHardwareControllerMap_Internal.ElementIdentifiers => new WHRsxpxEybuaFhhfBXutkRQNiVQX(-2)
		{
			TtytLoUfsgUyhsklaKccrnoMiiek = this
		};

		string IHardwareControllerTemplateMap_Internal.name => controllerName;

		Guid IHardwareControllerTemplateMap_Internal.typeGuid => Guid;

		[CustomObfuscation(rename = false)]
		public ControllerTemplateElementIdentifier GetElementIdentifier(int id)
		{
			int num = kwBxRrTMOUcPoxanwKoMBibCqnok(id);
			if (num < 0 || num >= elementIdentifiers.Length)
			{
				return null;
			}
			return elementIdentifiers[num];
		}

		[CustomObfuscation(rename = false)]
		public bool ContainsElementIdentifier(int id)
		{
			return kwBxRrTMOUcPoxanwKoMBibCqnok(id) >= 0;
		}

		[CustomObfuscation(rename = false)]
		public string[] GetElementIdentifierNames()
		{
			int num = ((elementIdentifiers != null) ? elementIdentifiers.Length : 0);
			string[] array = new string[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = elementIdentifiers[i].name;
			}
			return array;
		}

		[CustomObfuscation(rename = false)]
		public int[] GetElementIdentifierIds()
		{
			int num = ((elementIdentifiers != null) ? elementIdentifiers.Length : 0);
			int[] array = new int[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = elementIdentifiers[i].id;
			}
			return array;
		}

		[CustomObfuscation(rename = false)]
		internal string[] GetElementIdentifierScriptingNames(bool useAlternate)
		{
			int num = ((elementIdentifiers != null) ? elementIdentifiers.Length : 0);
			string[] array = new string[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = (useAlternate ? elementIdentifiers[i].alternateScriptingName : elementIdentifiers[i].scriptingName);
			}
			return array;
		}

		public int GetMappableElementIdentifierInfo(out string[] names, out int[] ids)
		{
			names = EmptyObjects<string>.array;
			ids = EmptyObjects<int>.array;
			int num = ((elementIdentifiers != null) ? elementIdentifiers.Length : 0);
			if (num == 0)
			{
				return 0;
			}
			List<ControllerTemplateElementIdentifier> list = new List<ControllerTemplateElementIdentifier>();
			for (int i = 0; i < num; i++)
			{
				if (elementIdentifiers[i] != null && InputTools.IsMappableType(elementIdentifiers[i].elementType))
				{
					list.Add(elementIdentifiers[i]);
				}
			}
			int count = list.Count;
			if (count == 0)
			{
				return 0;
			}
			names = new string[count];
			ids = new int[count];
			for (int j = 0; j < count; j++)
			{
				names[j] = list[j].name;
				ids[j] = list[j].id;
			}
			return count;
		}

		public int GetNonMappableElementIdentifierInfo(out string[] names, out int[] ids)
		{
			names = EmptyObjects<string>.array;
			ids = EmptyObjects<int>.array;
			int num = ((elementIdentifiers != null) ? elementIdentifiers.Length : 0);
			if (num == 0)
			{
				return 0;
			}
			List<ControllerTemplateElementIdentifier> list = new List<ControllerTemplateElementIdentifier>();
			for (int i = 0; i < num; i++)
			{
				if (elementIdentifiers[i] != null && !InputTools.IsMappableType(elementIdentifiers[i].elementType))
				{
					list.Add(elementIdentifiers[i]);
				}
			}
			int count = list.Count;
			if (count == 0)
			{
				return 0;
			}
			names = new string[count];
			ids = new int[count];
			for (int j = 0; j < count; j++)
			{
				names[j] = list[j].name;
				ids[j] = list[j].id;
			}
			return count;
		}

		public string[] GetJoystickNames()
		{
			int num = ((joysticks != null) ? joysticks.Count : 0);
			string[] array = new string[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = joysticks[i].name;
			}
			return array;
		}

		public int[] GetJoystickIds()
		{
			int num = ((joysticks != null) ? joysticks.Count : 0);
			int[] array = new int[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = joysticks[i].id;
			}
			return array;
		}

		public Guid GetJoystickGuid(int joystickId)
		{
			if (joysticks == null)
			{
				return Guid.Empty;
			}
			for (int i = 0; i < joysticks.Count; i++)
			{
				if (joysticks[i].id == joystickId)
				{
					return StringTools.ToGuid(joysticks[i].joystickGuid);
				}
			}
			return Guid.Empty;
		}

		public int GetJoystickId(Guid guid)
		{
			for (int i = 0; i < joysticks.Count; i++)
			{
				if (StringTools.ToGuid(joysticks[i].joystickGuid) == guid)
				{
					return joysticks[i].id;
				}
			}
			return -1;
		}

		public string GetJoystickFileGuidString(int joystickId)
		{
			if (joysticks == null)
			{
				return string.Empty;
			}
			for (int i = 0; i < joysticks.Count; i++)
			{
				if (joysticks[i].id == joystickId)
				{
					return joysticks[i].fileGuid;
				}
			}
			return string.Empty;
		}

		internal bool FnsmXdiZXlVuhjyKGKBoTnaakMhL(ControllerMap_Editor P_0, HardwareJoystickMap P_1, Guid P_2, out string P_3)
		{
			if (P_0 == null)
			{
				P_3 = "Template Map was null.";
				return false;
			}
			P_0.hardwareGuidString = P_2.ToString();
			Entry entry = FanGsmXappFogsAqPdlAABpkQqvtA(P_2);
			if (entry == null)
			{
				Guid guid = P_2;
				P_3 = "Hardware guid not found in ControllerDataFiles: " + guid.ToString() + "\nThis error should never happen. Please contact support.";
				return false;
			}
			List<ActionElementMap> actionElementMaps = P_0.actionElementMaps;
			using (TempListPool.TList<ActionElementMap> tList = TempListPool.GetTList<ActionElementMap>())
			{
				List<ActionElementMap> list = tList.list;
				using (TempListPool.TList<ElementIdentifierMap> tList2 = TempListPool.GetTList<ElementIdentifierMap>())
				{
					List<ElementIdentifierMap> list2 = tList2.list;
					for (int i = 0; i < actionElementMaps.Count; i++)
					{
						list2.Clear();
						ActionElementMap actionElementMap = actionElementMaps[i];
						int elementIdentifierId = actionElementMap._elementIdentifierId;
						entry.GetElementIdentifierMaps(elementIdentifierId, list2);
						for (int j = 0; j < list2.Count; j++)
						{
							ElementIdentifierMap elementIdentifierMap = list2[j];
							if (elementIdentifierMap == null || elementIdentifierMap.joystickId < 0)
							{
								continue;
							}
							ActionElementMap actionElementMap2 = new ActionElementMap(actionElementMap);
							ActionElementMap actionElementMap3 = null;
							bool flag = false;
							int num = kwBxRrTMOUcPoxanwKoMBibCqnok(elementIdentifierId);
							if (num >= 0 && num < elementIdentifiers.Length)
							{
								ControllerElementIdentifier elementIdentifier = P_1.GetElementIdentifier(elementIdentifierMap.joystickId);
								ControllerTemplateElementIdentifier_Editor controllerTemplateElementIdentifier_Editor = elementIdentifiers[num];
								if (elementIdentifier != null && controllerTemplateElementIdentifier_Editor != null)
								{
									ControllerTemplateElementType effectiveElementType = controllerTemplateElementIdentifier_Editor.effectiveElementType;
									if (!OzpbBOHoTkskKeBaOXWaTXQCmjjBA.dRTWaRkWIkEMLraxMRHrbdGkCqGCA(effectiveElementType, elementIdentifier.elementType))
									{
										if (effectiveElementType == ControllerTemplateElementType.Axis && elementIdentifier.elementType == ControllerElementType.Button)
										{
											if (elementIdentifierMap.splitAxis)
											{
												if (actionElementMap2.axisType == AxisType.Normal && actionElementMap2._axisRange == AxisRange.Full)
												{
													actionElementMap3 = new ActionElementMap(actionElementMap2);
													actionElementMap3._elementType = ControllerElementType.Button;
													actionElementMap3._elementIdentifierId = elementIdentifierMap.joystickId2;
													if (actionElementMap2._invert)
													{
														actionElementMap2._axisContribution = Pole.Negative;
														actionElementMap3._axisContribution = Pole.Positive;
													}
													else
													{
														actionElementMap2._axisContribution = Pole.Positive;
														actionElementMap3._axisContribution = Pole.Negative;
													}
												}
												else if (actionElementMap2.axisType == AxisType.Split)
												{
													if (actionElementMap2._axisRange == AxisRange.Positive)
													{
														actionElementMap2._elementIdentifierId = elementIdentifierMap.joystickId;
													}
													else if (actionElementMap2._axisRange == AxisRange.Negative)
													{
														actionElementMap2._elementIdentifierId = elementIdentifierMap.joystickId2;
													}
													flag = true;
												}
											}
											actionElementMap2._elementType = ControllerElementType.Button;
										}
										else
										{
											if (effectiveElementType != ControllerTemplateElementType.Button || elementIdentifier.elementType != ControllerElementType.Axis)
											{
												throw new NotImplementedException();
											}
											actionElementMap2._axisRange = AxisRange.Positive;
											actionElementMap2._elementType = ControllerElementType.Axis;
										}
									}
								}
							}
							if (!flag)
							{
								actionElementMap2._elementIdentifierId = elementIdentifierMap.joystickId;
							}
							list.Add(actionElementMap2);
							if (actionElementMap3 != null)
							{
								list.Add(actionElementMap3);
							}
						}
					}
				}
				actionElementMaps.Clear();
				ListTools.CopyTo(list, actionElementMaps);
			}
			P_3 = null;
			return true;
		}

		internal ControllerTemplateElementIdentifier TQKQOcjTcFmNzxDgybZxLvFfTrkC(Guid P_0, int P_1)
		{
			if (P_0 == Guid.Empty || P_1 < 0)
			{
				return null;
			}
			if (joysticks == null)
			{
				return null;
			}
			int num = -1;
			for (int i = 0; i < joysticks.Count; i++)
			{
				if (joysticks[i] != null && joysticks[i].JoystickGuid == P_0)
				{
					num = i;
					break;
				}
			}
			if (num < 0)
			{
				return null;
			}
			Entry entry = joysticks[num];
			if (entry == null)
			{
				return null;
			}
			int templateElementId = entry.GetTemplateElementId(P_1);
			if (templateElementId < 0)
			{
				return null;
			}
			return GetElementIdentifier(templateElementId);
		}

		[CustomObfuscation(rename = false)]
		internal SpecialElementEntry[] GetSpecialElementsOrig()
		{
			return specialElements;
		}

		private Entry FanGsmXappFogsAqPdlAABpkQqvtA(Guid P_0)
		{
			if (joysticks == null)
			{
				return null;
			}
			for (int i = 0; i < joysticks.Count; i++)
			{
				if (joysticks[i].JoystickGuid == P_0)
				{
					return joysticks[i];
				}
			}
			return null;
		}

		private int kwBxRrTMOUcPoxanwKoMBibCqnok(int P_0)
		{
			if (elementIdentifiers == null)
			{
				return -1;
			}
			for (int i = 0; i < elementIdentifiers.Length; i++)
			{
				if (elementIdentifiers[i].id == P_0)
				{
					return i;
				}
			}
			return -1;
		}

		IControllerElementIdentifierCommon_Internal IHardwareControllerMap_Internal.GetElementIdentifier(int id)
		{
			return GetElementIdentifier(id);
		}

		int IHardwareControllerTemplateMap_Internal.GetElementIdentifierCount()
		{
			if (elementIdentifiers == null)
			{
				return 0;
			}
			return elementIdentifiers.Length;
		}

		IControllerTemplateElementIdentifier IHardwareControllerTemplateMap_Internal.GetTemplateElementIdentifier(int index)
		{
			if (elementIdentifiers == null)
			{
				return null;
			}
			return elementIdentifiers[index];
		}

		IControllerTemplateElementIdentifier IHardwareControllerTemplateMap_Internal.GetTemplateElementIdentifierById(int elementIdentifierId)
		{
			return GetElementIdentifier(elementIdentifierId);
		}

		IControllerTemplateMapSpecialElement_Internal IHardwareControllerTemplateMap_Internal.GetSpecialTemplateElementByElementIdentifierId(int id)
		{
			if (specialElements == null)
			{
				return null;
			}
			for (int i = 0; i < specialElements.Length; i++)
			{
				if (specialElements[i] != null && specialElements[i].elementIdentifierId == id)
				{
					return specialElements[i];
				}
			}
			return null;
		}

		HxKJTjnYRTlXmgqyhXzejFNHdCai IHardwareControllerTemplateMap_Internal.GetAxisTarget(Controller controller, int elementIdentifierId)
		{
			ControllerTemplateElementIdentifier elementIdentifier = GetElementIdentifier(elementIdentifierId);
			if (elementIdentifier == null)
			{
				return null;
			}
			if (elementIdentifier.elementType != ControllerTemplateElementType.Axis)
			{
				return null;
			}
			if (controller == null)
			{
				return null;
			}
			Entry entry = FanGsmXappFogsAqPdlAABpkQqvtA(controller.fMxZVPLmyEupjctdQIaGgbDJdlvHA);
			if (entry == null)
			{
				return null;
			}
			List<ElementIdentifierMap> elementIdentifierMappings = entry.elementIdentifierMappings;
			if (elementIdentifierMappings == null)
			{
				return null;
			}
			int count = elementIdentifierMappings.Count;
			for (int i = 0; i < count; i++)
			{
				ElementIdentifierMap elementIdentifierMap = elementIdentifierMappings[i];
				if (elementIdentifierMap != null && elementIdentifierMap.templateId == elementIdentifierId)
				{
					if (elementIdentifierMap.splitAxis)
					{
						return new HxKJTjnYRTlXmgqyhXzejFNHdCai(ControllerTemplateElementType.Axis, true, new cyWFKpvAJpSlVUbktJsNPZKHVsyU(controller, -1, AxisRange.Full), new cyWFKpvAJpSlVUbktJsNPZKHVsyU(controller, elementIdentifierMap.joystickId, AxisRange.Positive), new cyWFKpvAJpSlVUbktJsNPZKHVsyU(controller, elementIdentifierMap.joystickId2, AxisRange.Positive));
					}
					return new HxKJTjnYRTlXmgqyhXzejFNHdCai(ControllerTemplateElementType.Axis, false, new cyWFKpvAJpSlVUbktJsNPZKHVsyU(controller, elementIdentifierMap.joystickId, AxisRange.Full), new cyWFKpvAJpSlVUbktJsNPZKHVsyU(controller, -1, AxisRange.Positive), new cyWFKpvAJpSlVUbktJsNPZKHVsyU(controller, -1, AxisRange.Positive));
				}
			}
			return null;
		}

		HxKJTjnYRTlXmgqyhXzejFNHdCai IHardwareControllerTemplateMap_Internal.GetButtonTarget(Controller controller, int elementIdentifierId)
		{
			ControllerTemplateElementIdentifier elementIdentifier = GetElementIdentifier(elementIdentifierId);
			if (elementIdentifier == null)
			{
				return null;
			}
			if (elementIdentifier.elementType != ControllerTemplateElementType.Button)
			{
				return null;
			}
			if (controller == null)
			{
				return null;
			}
			Entry entry = FanGsmXappFogsAqPdlAABpkQqvtA(controller.fMxZVPLmyEupjctdQIaGgbDJdlvHA);
			if (entry == null)
			{
				return null;
			}
			List<ElementIdentifierMap> elementIdentifierMappings = entry.elementIdentifierMappings;
			if (elementIdentifierMappings == null)
			{
				return null;
			}
			int count = elementIdentifierMappings.Count;
			for (int i = 0; i < count; i++)
			{
				ElementIdentifierMap elementIdentifierMap = elementIdentifierMappings[i];
				if (elementIdentifierMap != null && elementIdentifierMap.templateId == elementIdentifierId)
				{
					return new HxKJTjnYRTlXmgqyhXzejFNHdCai(ControllerTemplateElementType.Button, false, new cyWFKpvAJpSlVUbktJsNPZKHVsyU(controller, elementIdentifierMap.joystickId, AxisRange.Full), new cyWFKpvAJpSlVUbktJsNPZKHVsyU(controller, -1, AxisRange.Positive), new cyWFKpvAJpSlVUbktJsNPZKHVsyU(controller, -1, AxisRange.Positive));
				}
			}
			return null;
		}
	}
}
