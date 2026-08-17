using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Rewired.Config;
using Rewired.Data.Mapping;
using Rewired.Utils;
using Rewired.Utils.Libraries.TinyJson;
using UnityEngine;

namespace Rewired.Data
{
	[Serializable]
	public sealed class UserData
	{
		private static class VLVBKeKIsfcpczgBLQCLgmwKfWlb
		{
			[DefaultMember("Item")]
			private class gIBPcMQrDhIZFlpFWCJzmmnjiNkm
			{
				public enum CsysAjVUMGvYOWqTKclZPrJCrsOx
				{
					origId = 0,
					otherId = 1,
					finalId = 2
				}

				public int oEeyMMqMwhcCYcSilSIudqxFGpHM;

				public int prNJUrqsciTAXxyaCYjaYFJMFovV;

				public int qrhejhOprjbEagkMvfpSxTcPHfNtA;

				public int MFDajgeXtMasdPFhiOZXvTwpEsYn
				{
					get
					{
						return P_0 switch
						{
							CsysAjVUMGvYOWqTKclZPrJCrsOx.origId => oEeyMMqMwhcCYcSilSIudqxFGpHM, 
							CsysAjVUMGvYOWqTKclZPrJCrsOx.otherId => prNJUrqsciTAXxyaCYjaYFJMFovV, 
							CsysAjVUMGvYOWqTKclZPrJCrsOx.finalId => qrhejhOprjbEagkMvfpSxTcPHfNtA, 
							_ => throw new NotImplementedException(), 
						};
					}
					set
					{
						switch (csysAjVUMGvYOWqTKclZPrJCrsOx)
						{
						case CsysAjVUMGvYOWqTKclZPrJCrsOx.origId:
							oEeyMMqMwhcCYcSilSIudqxFGpHM = num;
							break;
						case CsysAjVUMGvYOWqTKclZPrJCrsOx.otherId:
							prNJUrqsciTAXxyaCYjaYFJMFovV = num;
							break;
						case CsysAjVUMGvYOWqTKclZPrJCrsOx.finalId:
							qrhejhOprjbEagkMvfpSxTcPHfNtA = num;
							break;
						default:
							throw new NotImplementedException();
						}
					}
				}

				public gIBPcMQrDhIZFlpFWCJzmmnjiNkm(int P_0, int P_1, int P_2)
				{
					oEeyMMqMwhcCYcSilSIudqxFGpHM = P_0;
					prNJUrqsciTAXxyaCYjaYFJMFovV = P_1;
					qrhejhOprjbEagkMvfpSxTcPHfNtA = P_2;
				}

				public virtual string OOjbZsEwsmFRhdWSnDLVresvUI()
				{
					return string.Concat(string.Concat("" + StringTools.WriteVar("origId", oEeyMMqMwhcCYcSilSIudqxFGpHM), StringTools.WriteVar("otherId", prNJUrqsciTAXxyaCYjaYFJMFovV)), StringTools.WriteVar("finalId", qrhejhOprjbEagkMvfpSxTcPHfNtA));
				}
			}

			private class okMYBPiHOMuwixSfVPWSIlunhHYl<_0001>
			{
				public _0001 FVLGSdhpzjAimcwGFLVvMxxwOUGq;

				public _0001 jFUdDvYgaHwuUQnjbfbCcgVkDAZFb;

				public gIBPcMQrDhIZFlpFWCJzmmnjiNkm.CsysAjVUMGvYOWqTKclZPrJCrsOx FVGMuRfCeZZjKXFaZLuJYUrOHJsf;

				public IList<_0001> zDJfCWfvzOnJQTofBEEdeXUQjwkF;

				public bool fnkkZlyPXUXkbxepljPcfxGtgcTu;

				public okMYBPiHOMuwixSfVPWSIlunhHYl(_0001 P_0, _0001 P_1, gIBPcMQrDhIZFlpFWCJzmmnjiNkm.CsysAjVUMGvYOWqTKclZPrJCrsOx P_2, IList<_0001> P_3, bool P_4)
				{
					FVLGSdhpzjAimcwGFLVvMxxwOUGq = P_0;
					jFUdDvYgaHwuUQnjbfbCcgVkDAZFb = P_1;
					FVGMuRfCeZZjKXFaZLuJYUrOHJsf = P_2;
					zDJfCWfvzOnJQTofBEEdeXUQjwkF = P_3;
					fnkkZlyPXUXkbxepljPcfxGtgcTu = P_4;
				}
			}

			[Serializable]
			private sealed class mseacdDYvvCxiPOCKMxdHSAWgPDjb
			{
				public static readonly mseacdDYvvCxiPOCKMxdHSAWgPDjb _003C_003E9 = new mseacdDYvvCxiPOCKMxdHSAWgPDjb();

				public static Func<InputActionCategory, int> _003C_003E9__0_0;

				public static Func<InputActionCategory, string> _003C_003E9__0_1;

				public static Func<InputActionCategory, IList<InputActionCategory>, int> _003C_003E9__0_2;

				public static Func<InputBehavior, int> _003C_003E9__0_4;

				public static Func<InputBehavior, string> _003C_003E9__0_5;

				public static Func<InputBehavior, IList<InputBehavior>, int> _003C_003E9__0_6;

				public static Func<InputAction, int> _003C_003E9__0_8;

				public static Func<InputAction, string> _003C_003E9__0_9;

				public static Func<InputAction, IList<InputAction>, int> _003C_003E9__0_10;

				public static Func<InputMapCategory, int> _003C_003E9__0_47;

				public static Func<InputMapCategory, string> _003C_003E9__0_48;

				public static Func<InputMapCategory, IList<InputMapCategory>, int> _003C_003E9__0_49;

				public static Func<InputLayout, int> _003C_003E9__0_12;

				public static Func<InputLayout, string> _003C_003E9__0_13;

				public static Func<InputLayout, IList<InputLayout>, int> _003C_003E9__0_14;

				public static Func<InputLayout, int> _003C_003E9__0_16;

				public static Func<InputLayout, string> _003C_003E9__0_17;

				public static Func<InputLayout, IList<InputLayout>, int> _003C_003E9__0_18;

				public static Func<InputLayout, int> _003C_003E9__0_20;

				public static Func<InputLayout, string> _003C_003E9__0_21;

				public static Func<InputLayout, IList<InputLayout>, int> _003C_003E9__0_22;

				public static Func<InputLayout, int> _003C_003E9__0_24;

				public static Func<InputLayout, string> _003C_003E9__0_25;

				public static Func<InputLayout, IList<InputLayout>, int> _003C_003E9__0_26;

				public static Func<CustomController_Editor, int> _003C_003E9__0_29;

				public static Func<CustomController_Editor, string> _003C_003E9__0_30;

				public static Func<CustomController_Editor, IList<CustomController_Editor>, int> _003C_003E9__0_31;

				public static Func<ControllerMapLayoutManager_RuleSet_Editor, int> _003C_003E9__0_33;

				public static Func<ControllerMapLayoutManager_RuleSet_Editor, string> _003C_003E9__0_34;

				public static Func<ControllerMapLayoutManager_RuleSet_Editor, IList<ControllerMapLayoutManager_RuleSet_Editor>, int> _003C_003E9__0_35;

				public static Func<ControllerMapEnabler_RuleSet_Editor, int> _003C_003E9__0_37;

				public static Func<ControllerMapEnabler_RuleSet_Editor, string> _003C_003E9__0_38;

				public static Func<ControllerMapEnabler_RuleSet_Editor, IList<ControllerMapEnabler_RuleSet_Editor>, int> _003C_003E9__0_39;

				public static Func<Player_Editor, int> _003C_003E9__0_41;

				public static Func<Player_Editor, string> _003C_003E9__0_42;

				public static Func<Player_Editor, IList<Player_Editor>, int> _003C_003E9__0_43;

				public static Func<Player_Editor.Mapping, IList<Player_Editor.Mapping>, int> _003C_003E9__0_64;

				public static Func<Player_Editor.CreateControllerInfo, IList<Player_Editor.CreateControllerInfo>, int> _003C_003E9__0_65;

				public static Func<ControllerMap_Editor, int> _003C_003E9__0_66;

				public static Func<ControllerMap_Editor, string> _003C_003E9__0_67;

				public static Func<ActionElementMap, IList<ActionElementMap>, int> _003C_003E9__0_75;

				public static Func<ControllerMap_Editor, int> _003C_003E9__0_76;

				public static Func<ControllerMap_Editor, string> _003C_003E9__0_77;

				public static Func<ActionElementMap, IList<ActionElementMap>, int> _003C_003E9__0_85;

				public static Func<ControllerMap_Editor, int> _003C_003E9__0_86;

				public static Func<ControllerMap_Editor, string> _003C_003E9__0_87;

				public static Func<ActionElementMap, IList<ActionElementMap>, int> _003C_003E9__0_95;

				public static Func<ControllerMap_Editor, int> _003C_003E9__0_96;

				public static Func<ControllerMap_Editor, string> _003C_003E9__0_97;

				public static Func<ActionElementMap, IList<ActionElementMap>, int> _003C_003E9__0_107;

				internal int pljvUMdUmPapsfSagwcZAvFhCKch(InputActionCategory P_0)
				{
					return P_0.id;
				}

				internal string IRuaKnINFChelfQORLDidLkGHneeB(InputActionCategory P_0)
				{
					return P_0.name;
				}

				internal int vuvyHAJbotopEJpAFJfuTTUmeYcr(InputActionCategory P_0, IList<InputActionCategory> P_1)
				{
					for (int i = 0; i < P_1.Count; i++)
					{
						if (string.Equals(P_0.name, P_1[i].name, StringComparison.OrdinalIgnoreCase))
						{
							return i;
						}
					}
					return -1;
				}

				internal int zUNbdEABPbANVrJoHLfByBhHigcgb(InputBehavior P_0)
				{
					return P_0.id;
				}

				internal string TmEflHfHsjhHfUnvmaLbAMvcLsxAb(InputBehavior P_0)
				{
					return P_0.name;
				}

				internal int QgeSVYFECRfVIGpKYuWxHIosqrEu(InputBehavior P_0, IList<InputBehavior> P_1)
				{
					for (int i = 0; i < P_1.Count; i++)
					{
						if (string.Equals(P_0.name, P_1[i].name, StringComparison.OrdinalIgnoreCase))
						{
							return i;
						}
					}
					return -1;
				}

				internal int aPTjTWzIbLpyjDXDfXIocYrSKtJb(InputAction P_0)
				{
					return P_0.id;
				}

				internal string TrAGOSMDdcAVjdnSUXysAnIBPzJU(InputAction P_0)
				{
					return P_0.name;
				}

				internal int MNPUsEZmXxGLbOayTUofJzPAXPfA(InputAction P_0, IList<InputAction> P_1)
				{
					for (int i = 0; i < P_1.Count; i++)
					{
						if (string.Equals(P_0.name, P_1[i].name, StringComparison.OrdinalIgnoreCase))
						{
							return i;
						}
					}
					return -1;
				}

				internal int bGtEHkepcZYHUjLQEEMThJOjDJdTb(InputMapCategory P_0)
				{
					return P_0.id;
				}

				internal string jUAcRPMEJMdEoaNztKGtIhJoWMpo(InputMapCategory P_0)
				{
					return P_0.name;
				}

				internal int clYsPVqGRhLzUfzCXbBsMymXlkaw(InputMapCategory P_0, IList<InputMapCategory> P_1)
				{
					for (int i = 0; i < P_1.Count; i++)
					{
						if (string.Equals(P_0.name, P_1[i].name, StringComparison.OrdinalIgnoreCase))
						{
							return i;
						}
					}
					return -1;
				}

				internal int MHnLrLIKQlNvkPGpodVmHgBBgkBu(InputLayout P_0)
				{
					return P_0.id;
				}

				internal string YczICcKSwkVhNuXjWYtzfOHFnxjEA(InputLayout P_0)
				{
					return P_0.name;
				}

				internal int FSpCRzAQxyxcZaPHxipUPStcJEHkA(InputLayout P_0, IList<InputLayout> P_1)
				{
					for (int i = 0; i < P_1.Count; i++)
					{
						if (string.Equals(P_0.name, P_1[i].name, StringComparison.OrdinalIgnoreCase))
						{
							return i;
						}
					}
					return -1;
				}

				internal int guAvSPRfphGCgPLwsGhjQjCxGvve(InputLayout P_0)
				{
					return P_0.id;
				}

				internal string sUvSMAHoSGYeeqDxBuBjvYkfcTHm(InputLayout P_0)
				{
					return P_0.name;
				}

				internal int EfSHMcfyctQsNSkrrMlqQCIbamaCb(InputLayout P_0, IList<InputLayout> P_1)
				{
					for (int i = 0; i < P_1.Count; i++)
					{
						if (string.Equals(P_0.name, P_1[i].name, StringComparison.OrdinalIgnoreCase))
						{
							return i;
						}
					}
					return -1;
				}

				internal int boMLvzramTvEUmwhNDHAogxQPIXe(InputLayout P_0)
				{
					return P_0.id;
				}

				internal string lAismnfqlasnUDKSICpRsNybEYCq(InputLayout P_0)
				{
					return P_0.name;
				}

				internal int MRJEiSKzwZmCOvnYNIyPirXgAVbAb(InputLayout P_0, IList<InputLayout> P_1)
				{
					for (int i = 0; i < P_1.Count; i++)
					{
						if (string.Equals(P_0.name, P_1[i].name, StringComparison.OrdinalIgnoreCase))
						{
							return i;
						}
					}
					return -1;
				}

				internal int ZzjwerfKGxNcfuMBKjeNuBsAcWvd(InputLayout P_0)
				{
					return P_0.id;
				}

				internal string hDqSwtYKuRURCLFmpAEXaWuTIMXM(InputLayout P_0)
				{
					return P_0.name;
				}

				internal int jGXMnfgLJzqGuvGxJlFDFdqzCuHGA(InputLayout P_0, IList<InputLayout> P_1)
				{
					for (int i = 0; i < P_1.Count; i++)
					{
						if (string.Equals(P_0.name, P_1[i].name, StringComparison.OrdinalIgnoreCase))
						{
							return i;
						}
					}
					return -1;
				}

				internal int jWsBKFEJVjqsqEStjrrNclkFiiYZB(CustomController_Editor P_0)
				{
					return P_0.id;
				}

				internal string cVwtJlXtEYulyeItkSLgPqkJfBpFA(CustomController_Editor P_0)
				{
					return P_0.name;
				}

				internal int jgCYvqcbDTODUNVewqZPxIZTumnJ(CustomController_Editor P_0, IList<CustomController_Editor> P_1)
				{
					for (int i = 0; i < P_1.Count; i++)
					{
						if (string.Equals(P_0.name, P_1[i].name, StringComparison.OrdinalIgnoreCase))
						{
							return i;
						}
					}
					return -1;
				}

				internal int GxpuZfULUGLBIzCisavYJMtBepFn(ControllerMapLayoutManager_RuleSet_Editor P_0)
				{
					return P_0.id;
				}

				internal string qQhjNNEDfurDjWleKhAVgKwaWUyhA(ControllerMapLayoutManager_RuleSet_Editor P_0)
				{
					return P_0.name;
				}

				internal int EumQjyOqYfbOhIDBXodDDkKaIkoFc(ControllerMapLayoutManager_RuleSet_Editor P_0, IList<ControllerMapLayoutManager_RuleSet_Editor> P_1)
				{
					for (int i = 0; i < P_1.Count; i++)
					{
						if (string.Equals(P_0.name, P_1[i].name, StringComparison.OrdinalIgnoreCase))
						{
							return i;
						}
					}
					return -1;
				}

				internal int LBgfmxkwMZWCkOOSfNiPISwMaTHJA(ControllerMapEnabler_RuleSet_Editor P_0)
				{
					return P_0.id;
				}

				internal string tUEHTohUNypeeUWDQFwwWPgqpAVw(ControllerMapEnabler_RuleSet_Editor P_0)
				{
					return P_0.name;
				}

				internal int aHbniMPeQNTnrFtnRAWxMyBNacGK(ControllerMapEnabler_RuleSet_Editor P_0, IList<ControllerMapEnabler_RuleSet_Editor> P_1)
				{
					for (int i = 0; i < P_1.Count; i++)
					{
						if (string.Equals(P_0.name, P_1[i].name, StringComparison.OrdinalIgnoreCase))
						{
							return i;
						}
					}
					return -1;
				}

				internal int skKaMTcGPfGSDuAxZGtJBneNwyhRA(Player_Editor P_0)
				{
					return P_0.id;
				}

				internal string IxVxQylvDADCjtkLZMSEjBbIAgRe(Player_Editor P_0)
				{
					return P_0.name;
				}

				internal int RtwtCAAQKSHAJRIhZpksGhmcbAZp(Player_Editor P_0, IList<Player_Editor> P_1)
				{
					for (int i = 0; i < P_1.Count; i++)
					{
						if (string.Equals(P_0.name, P_1[i].name, StringComparison.OrdinalIgnoreCase))
						{
							return i;
						}
					}
					return -1;
				}

				internal int NxQXsYQgfukkLSgjiasEZoVbsWDH(Player_Editor.Mapping P_0, IList<Player_Editor.Mapping> P_1)
				{
					for (int i = 0; i < P_1.Count; i++)
					{
						if (P_1[i].categoryId == P_0.categoryId && P_1[i].layoutId == P_0.layoutId)
						{
							return i;
						}
					}
					return -1;
				}

				internal int mffsLDvIqbfrxCZxIVtPaJWmZwxRA(Player_Editor.CreateControllerInfo P_0, IList<Player_Editor.CreateControllerInfo> P_1)
				{
					for (int i = 0; i < P_1.Count; i++)
					{
						if (P_1[i].sourceId == P_0.sourceId)
						{
							return i;
						}
					}
					return -1;
				}

				internal int YEQJBFquHblPQEcFwVDDhHzPgGmZ(ControllerMap_Editor P_0)
				{
					return P_0.id;
				}

				internal string wvNbpaeNdfVaeNnuhPCzOkOSbvaOA(ControllerMap_Editor P_0)
				{
					return P_0.name;
				}

				internal int sgGgsyaOhnZVNDIgVNQjRHWgKbdBA(ActionElementMap P_0, IList<ActionElementMap> P_1)
				{
					for (int i = 0; i < P_1.Count; i++)
					{
						if (P_1[i]._keyboardKeyCode == P_0._keyboardKeyCode && P_1[i]._modifierKey1 == P_0._modifierKey1 && P_1[i]._modifierKey2 == P_0._modifierKey2 && P_1[i]._modifierKey3 == P_0._modifierKey3 && P_1[i]._axisContribution == P_0._axisContribution && P_1[i]._actionId == P_0._actionId)
						{
							return i;
						}
					}
					return -1;
				}

				internal int onpLFvxKmwGshwHYpAinEOtxMneV(ControllerMap_Editor P_0)
				{
					return P_0.id;
				}

				internal string NKBhTWYvvMejnueEIqfWAYMhLUnf(ControllerMap_Editor P_0)
				{
					return P_0.name;
				}

				internal int bUVwysfzlZLDWIzMTSyluxqJnRzF(ActionElementMap P_0, IList<ActionElementMap> P_1)
				{
					for (int i = 0; i < P_1.Count; i++)
					{
						if (P_1[i]._elementIdentifierId == P_0._elementIdentifierId && P_1[i]._axisRange == P_0._axisRange && P_1[i]._axisContribution == P_0._axisContribution && P_1[i]._actionId == P_0._actionId)
						{
							return i;
						}
					}
					return -1;
				}

				internal int yEzjCAvwPjkKWfSSBOkMGEPqqFgU(ControllerMap_Editor P_0)
				{
					return P_0.id;
				}

				internal string dIDQYTNgaCosJrVExLeTJleGJPVt(ControllerMap_Editor P_0)
				{
					return P_0.name;
				}

				internal int MqchHpYahnmGfxfYPAWNHkMfaBvcA(ActionElementMap P_0, IList<ActionElementMap> P_1)
				{
					for (int i = 0; i < P_1.Count; i++)
					{
						if (P_1[i]._elementIdentifierId == P_0._elementIdentifierId && P_1[i]._axisRange == P_0._axisRange && P_1[i]._axisContribution == P_0._axisContribution && P_1[i]._actionId == P_0._actionId)
						{
							return i;
						}
					}
					return -1;
				}

				internal int NxRvsrWmSEtlEGHdIUvDhYhplcCc(ControllerMap_Editor P_0)
				{
					return P_0.id;
				}

				internal string xzTsdYAAPgeIniCqOktrinqXbfSx(ControllerMap_Editor P_0)
				{
					return P_0.name;
				}

				internal int SkFlARcQIgRQPZAMumQZrdaMORgu(ActionElementMap P_0, IList<ActionElementMap> P_1)
				{
					for (int i = 0; i < P_1.Count; i++)
					{
						if (P_1[i]._elementIdentifierId == P_0._elementIdentifierId && P_1[i]._axisRange == P_0._axisRange && P_1[i]._axisContribution == P_0._axisContribution && P_1[i]._actionId == P_0._actionId)
						{
							return i;
						}
					}
					return -1;
				}
			}

			private sealed class pNiBgndhirjKwSeKSnElGpWIhUIab
			{
				public UserData VGcgcOsNmgmQWYkAXDCxgeYloFfeA;

				public List<gIBPcMQrDhIZFlpFWCJzmmnjiNkm> tndXldzonxrGYSzWdGzTJlymunSv;

				public List<gIBPcMQrDhIZFlpFWCJzmmnjiNkm> XMjXmMjdlZnoWCAabkaNjtdKAnKK;

				public List<gIBPcMQrDhIZFlpFWCJzmmnjiNkm> WrBLrEQoisYZPIWzDfJYdiITgZtfA;

				public List<gIBPcMQrDhIZFlpFWCJzmmnjiNkm> zSRKGqTMdaxenOqwPohNjSGvKuZS;

				public List<gIBPcMQrDhIZFlpFWCJzmmnjiNkm> nYtbnZrMSQbsMDwMuxJhWhyPAGMl;

				public List<gIBPcMQrDhIZFlpFWCJzmmnjiNkm> LkDUmDwrUoTLTkfJQEsCqAtIFFUHA;

				public List<gIBPcMQrDhIZFlpFWCJzmmnjiNkm> azPMbUiryIgrSfKcUyTkLdttmScO;

				public Func<ControllerType, List<gIBPcMQrDhIZFlpFWCJzmmnjiNkm>> CbJHWLTvKsRFOifIrUgGvisjgImcA;

				public List<gIBPcMQrDhIZFlpFWCJzmmnjiNkm> BxvShEVaGesKdYRqoIRFhVDAsfCI;

				public List<gIBPcMQrDhIZFlpFWCJzmmnjiNkm> fNeYIWEKThcTUdEIrtoRcwIQYAwJ;

				public List<gIBPcMQrDhIZFlpFWCJzmmnjiNkm> NwxqNrKQGJvHyVDFwgMmaoSsZjNeA;

				public List<gIBPcMQrDhIZFlpFWCJzmmnjiNkm> OqpaNlVZdIOZBWSzcdKBDFxyiLEN;

				internal InputActionCategory wfHjMVgYIfypLGMeGPidbzPmPmUdA(okMYBPiHOMuwixSfVPWSIlunhHYl<InputActionCategory> P_0)
				{
					InputActionCategory inputActionCategory = JsonTools.Clone(P_0.FVLGSdhpzjAimcwGFLVvMxxwOUGq);
					InputActionCategory inputActionCategory2;
					if (P_0.fnkkZlyPXUXkbxepljPcfxGtgcTu)
					{
						inputActionCategory2 = P_0.jFUdDvYgaHwuUQnjbfbCcgVkDAZFb;
					}
					else
					{
						VGcgcOsNmgmQWYkAXDCxgeYloFfeA.AddActionCategory();
						inputActionCategory2 = P_0.zDJfCWfvzOnJQTofBEEdeXUQjwkF[P_0.zDJfCWfvzOnJQTofBEEdeXUQjwkF.Count - 1];
					}
					inputActionCategory.id = inputActionCategory2.id;
					int index = P_0.zDJfCWfvzOnJQTofBEEdeXUQjwkF.IndexOf(inputActionCategory2);
					P_0.zDJfCWfvzOnJQTofBEEdeXUQjwkF[index] = inputActionCategory;
					return inputActionCategory;
				}

				internal InputBehavior szAynGrVdWiHhlpAeUoicFFqiBlcA(okMYBPiHOMuwixSfVPWSIlunhHYl<InputBehavior> P_0)
				{
					InputBehavior inputBehavior = JsonTools.Clone(P_0.FVLGSdhpzjAimcwGFLVvMxxwOUGq);
					InputBehavior inputBehavior2;
					if (P_0.fnkkZlyPXUXkbxepljPcfxGtgcTu)
					{
						inputBehavior2 = P_0.jFUdDvYgaHwuUQnjbfbCcgVkDAZFb;
					}
					else
					{
						VGcgcOsNmgmQWYkAXDCxgeYloFfeA.AddInputBehavior();
						inputBehavior2 = P_0.zDJfCWfvzOnJQTofBEEdeXUQjwkF[P_0.zDJfCWfvzOnJQTofBEEdeXUQjwkF.Count - 1];
					}
					inputBehavior.id = inputBehavior2.id;
					int index = P_0.zDJfCWfvzOnJQTofBEEdeXUQjwkF.IndexOf(inputBehavior2);
					P_0.zDJfCWfvzOnJQTofBEEdeXUQjwkF[index] = inputBehavior;
					return inputBehavior;
				}

				internal InputAction PzsSKglgkffUaMPtctPJYXTXTFQA(okMYBPiHOMuwixSfVPWSIlunhHYl<InputAction> P_0)
				{
					ZjzqbCWADWxNHblhGxQgbOuopCnj zjzqbCWADWxNHblhGxQgbOuopCnj = new ZjzqbCWADWxNHblhGxQgbOuopCnj();
					zjzqbCWADWxNHblhGxQgbOuopCnj.GzMgzDrRoXiWTYOFZKsUDlXzsslk = P_0;
					InputAction inputAction = JsonTools.Clone(zjzqbCWADWxNHblhGxQgbOuopCnj.GzMgzDrRoXiWTYOFZKsUDlXzsslk.FVLGSdhpzjAimcwGFLVvMxxwOUGq);
					int num = tndXldzonxrGYSzWdGzTJlymunSv.Find(zjzqbCWADWxNHblhGxQgbOuopCnj.twUCwQilNQPcGpyfsgvLnKBHdPqm)?.qrhejhOprjbEagkMvfpSxTcPHfNtA ?? 0;
					InputAction inputAction2;
					if (zjzqbCWADWxNHblhGxQgbOuopCnj.GzMgzDrRoXiWTYOFZKsUDlXzsslk.fnkkZlyPXUXkbxepljPcfxGtgcTu)
					{
						inputAction2 = zjzqbCWADWxNHblhGxQgbOuopCnj.GzMgzDrRoXiWTYOFZKsUDlXzsslk.jFUdDvYgaHwuUQnjbfbCcgVkDAZFb;
					}
					else
					{
						VGcgcOsNmgmQWYkAXDCxgeYloFfeA.AddAction(num);
						inputAction2 = zjzqbCWADWxNHblhGxQgbOuopCnj.GzMgzDrRoXiWTYOFZKsUDlXzsslk.zDJfCWfvzOnJQTofBEEdeXUQjwkF[zjzqbCWADWxNHblhGxQgbOuopCnj.GzMgzDrRoXiWTYOFZKsUDlXzsslk.zDJfCWfvzOnJQTofBEEdeXUQjwkF.Count - 1];
					}
					int num2 = XMjXmMjdlZnoWCAabkaNjtdKAnKK.Find(zjzqbCWADWxNHblhGxQgbOuopCnj.CHyBHuIpTFoTZcSEZsXqVYrYEETh)?.qrhejhOprjbEagkMvfpSxTcPHfNtA ?? 0;
					inputAction.id = inputAction2.id;
					if (num != inputAction2.categoryId)
					{
						VGcgcOsNmgmQWYkAXDCxgeYloFfeA.ChangeActionCategory(inputAction2.id, num);
					}
					inputAction.categoryId = num;
					inputAction.behaviorId = num2;
					int index = zjzqbCWADWxNHblhGxQgbOuopCnj.GzMgzDrRoXiWTYOFZKsUDlXzsslk.zDJfCWfvzOnJQTofBEEdeXUQjwkF.IndexOf(inputAction2);
					zjzqbCWADWxNHblhGxQgbOuopCnj.GzMgzDrRoXiWTYOFZKsUDlXzsslk.zDJfCWfvzOnJQTofBEEdeXUQjwkF[index] = inputAction;
					return inputAction;
				}

				internal InputLayout gNjFLIwNtEbrjKtPSKfFJhCxyvOdA(okMYBPiHOMuwixSfVPWSIlunhHYl<InputLayout> P_0)
				{
					InputLayout inputLayout = JsonTools.Clone(P_0.FVLGSdhpzjAimcwGFLVvMxxwOUGq);
					InputLayout inputLayout2;
					if (P_0.fnkkZlyPXUXkbxepljPcfxGtgcTu)
					{
						inputLayout2 = P_0.jFUdDvYgaHwuUQnjbfbCcgVkDAZFb;
					}
					else
					{
						VGcgcOsNmgmQWYkAXDCxgeYloFfeA.AddKeyboardLayout();
						inputLayout2 = P_0.zDJfCWfvzOnJQTofBEEdeXUQjwkF[P_0.zDJfCWfvzOnJQTofBEEdeXUQjwkF.Count - 1];
					}
					inputLayout.id = inputLayout2.id;
					int index = P_0.zDJfCWfvzOnJQTofBEEdeXUQjwkF.IndexOf(inputLayout2);
					P_0.zDJfCWfvzOnJQTofBEEdeXUQjwkF[index] = inputLayout;
					return inputLayout;
				}

				internal InputLayout YrWkHdSpdVoBbGENfFibsuJnBfXp(okMYBPiHOMuwixSfVPWSIlunhHYl<InputLayout> P_0)
				{
					InputLayout inputLayout = JsonTools.Clone(P_0.FVLGSdhpzjAimcwGFLVvMxxwOUGq);
					InputLayout inputLayout2;
					if (P_0.fnkkZlyPXUXkbxepljPcfxGtgcTu)
					{
						inputLayout2 = P_0.jFUdDvYgaHwuUQnjbfbCcgVkDAZFb;
					}
					else
					{
						VGcgcOsNmgmQWYkAXDCxgeYloFfeA.AddMouseLayout();
						inputLayout2 = P_0.zDJfCWfvzOnJQTofBEEdeXUQjwkF[P_0.zDJfCWfvzOnJQTofBEEdeXUQjwkF.Count - 1];
					}
					inputLayout.id = inputLayout2.id;
					int index = P_0.zDJfCWfvzOnJQTofBEEdeXUQjwkF.IndexOf(inputLayout2);
					P_0.zDJfCWfvzOnJQTofBEEdeXUQjwkF[index] = inputLayout;
					return inputLayout;
				}

				internal InputLayout WscaCCDhlxzyojdGftnMLhDmIzii(okMYBPiHOMuwixSfVPWSIlunhHYl<InputLayout> P_0)
				{
					InputLayout inputLayout = JsonTools.Clone(P_0.FVLGSdhpzjAimcwGFLVvMxxwOUGq);
					InputLayout inputLayout2;
					if (P_0.fnkkZlyPXUXkbxepljPcfxGtgcTu)
					{
						inputLayout2 = P_0.jFUdDvYgaHwuUQnjbfbCcgVkDAZFb;
					}
					else
					{
						VGcgcOsNmgmQWYkAXDCxgeYloFfeA.AddJoystickLayout();
						inputLayout2 = P_0.zDJfCWfvzOnJQTofBEEdeXUQjwkF[P_0.zDJfCWfvzOnJQTofBEEdeXUQjwkF.Count - 1];
					}
					inputLayout.id = inputLayout2.id;
					int index = P_0.zDJfCWfvzOnJQTofBEEdeXUQjwkF.IndexOf(inputLayout2);
					P_0.zDJfCWfvzOnJQTofBEEdeXUQjwkF[index] = inputLayout;
					return inputLayout;
				}

				internal InputLayout qUfxPEbYfCnckWBnNGWJNkjecxOx(okMYBPiHOMuwixSfVPWSIlunhHYl<InputLayout> P_0)
				{
					InputLayout inputLayout = JsonTools.Clone(P_0.FVLGSdhpzjAimcwGFLVvMxxwOUGq);
					InputLayout inputLayout2;
					if (P_0.fnkkZlyPXUXkbxepljPcfxGtgcTu)
					{
						inputLayout2 = P_0.jFUdDvYgaHwuUQnjbfbCcgVkDAZFb;
					}
					else
					{
						VGcgcOsNmgmQWYkAXDCxgeYloFfeA.AddCustomControllerLayout();
						inputLayout2 = P_0.zDJfCWfvzOnJQTofBEEdeXUQjwkF[P_0.zDJfCWfvzOnJQTofBEEdeXUQjwkF.Count - 1];
					}
					inputLayout.id = inputLayout2.id;
					int index = P_0.zDJfCWfvzOnJQTofBEEdeXUQjwkF.IndexOf(inputLayout2);
					P_0.zDJfCWfvzOnJQTofBEEdeXUQjwkF[index] = inputLayout;
					return inputLayout;
				}

				internal List<gIBPcMQrDhIZFlpFWCJzmmnjiNkm> wyuEzRDcbLdKAYQpvBysOATpxSpf(ControllerType P_0)
				{
					return P_0 switch
					{
						ControllerType.Keyboard => WrBLrEQoisYZPIWzDfJYdiITgZtfA, 
						ControllerType.Mouse => zSRKGqTMdaxenOqwPohNjSGvKuZS, 
						ControllerType.Joystick => nYtbnZrMSQbsMDwMuxJhWhyPAGMl, 
						ControllerType.Custom => LkDUmDwrUoTLTkfJQEsCqAtIFFUHA, 
						_ => throw new NotImplementedException(), 
					};
				}

				internal CustomController_Editor uDZaYfuBcZjZKVWSLwFRmbERUMzT(okMYBPiHOMuwixSfVPWSIlunhHYl<CustomController_Editor> P_0)
				{
					CustomController_Editor customController_Editor = JsonTools.Clone(P_0.FVLGSdhpzjAimcwGFLVvMxxwOUGq);
					CustomController_Editor customController_Editor2;
					if (P_0.fnkkZlyPXUXkbxepljPcfxGtgcTu)
					{
						customController_Editor2 = P_0.jFUdDvYgaHwuUQnjbfbCcgVkDAZFb;
					}
					else
					{
						VGcgcOsNmgmQWYkAXDCxgeYloFfeA.AddCustomController(Guid.Empty);
						customController_Editor2 = P_0.zDJfCWfvzOnJQTofBEEdeXUQjwkF[P_0.zDJfCWfvzOnJQTofBEEdeXUQjwkF.Count - 1];
					}
					customController_Editor.id = customController_Editor2.id;
					int index = P_0.zDJfCWfvzOnJQTofBEEdeXUQjwkF.IndexOf(customController_Editor2);
					P_0.zDJfCWfvzOnJQTofBEEdeXUQjwkF[index] = customController_Editor;
					return customController_Editor;
				}

				internal ControllerMapLayoutManager_RuleSet_Editor oXjcAMDcoOBbNxrxXyYESyFEFRVqA(okMYBPiHOMuwixSfVPWSIlunhHYl<ControllerMapLayoutManager_RuleSet_Editor> P_0)
				{
					qQpSvXhNBgGxofhpEHBDHBhVNpDeA qQpSvXhNBgGxofhpEHBDHBhVNpDeA2 = new qQpSvXhNBgGxofhpEHBDHBhVNpDeA();
					qQpSvXhNBgGxofhpEHBDHBhVNpDeA2.iTbDfsACSWETYaMsHzcweptwtqHl = P_0;
					ControllerMapLayoutManager_RuleSet_Editor controllerMapLayoutManager_RuleSet_Editor = JsonTools.Clone(qQpSvXhNBgGxofhpEHBDHBhVNpDeA2.iTbDfsACSWETYaMsHzcweptwtqHl.FVLGSdhpzjAimcwGFLVvMxxwOUGq);
					int num = ((controllerMapLayoutManager_RuleSet_Editor.rules != null) ? controllerMapLayoutManager_RuleSet_Editor.rules.Count : 0);
					for (int i = 0; i < num; i++)
					{
						ControllerMapLayoutManager_Rule_Editor controllerMapLayoutManager_Rule_Editor = controllerMapLayoutManager_RuleSet_Editor.rules[i];
						if (controllerMapLayoutManager_Rule_Editor == null || controllerMapLayoutManager_Rule_Editor.categoryIds == null)
						{
							continue;
						}
						List<int> list = new List<int>();
						int num2 = ((controllerMapLayoutManager_Rule_Editor.categoryIds != null) ? controllerMapLayoutManager_Rule_Editor.categoryIds.Count : 0);
						for (int j = 0; j < num2; j++)
						{
							TBKafMpJfADXDfXfTzXvQxYKvsmdA tBKafMpJfADXDfXfTzXvQxYKvsmdA = new TBKafMpJfADXDfXfTzXvQxYKvsmdA();
							tBKafMpJfADXDfXfTzXvQxYKvsmdA.RPUKOejNyKqSkEttguUvJyNuPHkQ = qQpSvXhNBgGxofhpEHBDHBhVNpDeA2;
							tBKafMpJfADXDfXfTzXvQxYKvsmdA.pHZcVhiKGTWzuZXluzKXjatFzHTN = controllerMapLayoutManager_Rule_Editor.categoryIds[j];
							gIBPcMQrDhIZFlpFWCJzmmnjiNkm gIBPcMQrDhIZFlpFWCJzmmnjiNkm2 = azPMbUiryIgrSfKcUyTkLdttmScO.Find(tBKafMpJfADXDfXfTzXvQxYKvsmdA.jIzeQHrbepXlNawdbmhLsIbzVAVf);
							if (gIBPcMQrDhIZFlpFWCJzmmnjiNkm2 == null)
							{
								Logger.LogError("No new Map Category Id found for old id: " + tBKafMpJfADXDfXfTzXvQxYKvsmdA.pHZcVhiKGTWzuZXluzKXjatFzHTN);
							}
							else
							{
								list.Add(gIBPcMQrDhIZFlpFWCJzmmnjiNkm2.qrhejhOprjbEagkMvfpSxTcPHfNtA);
							}
						}
						controllerMapLayoutManager_Rule_Editor.categoryIds = list;
					}
					int num3 = ((controllerMapLayoutManager_RuleSet_Editor.rules != null) ? controllerMapLayoutManager_RuleSet_Editor.rules.Count : 0);
					for (int k = 0; k < num3; k++)
					{
						rIAiUiGFnhBrJchvEInIczDFhWrub rIAiUiGFnhBrJchvEInIczDFhWrub2 = new rIAiUiGFnhBrJchvEInIczDFhWrub();
						rIAiUiGFnhBrJchvEInIczDFhWrub2.uoeKFjJjjeDDVGBrcMDVuOcTLgnrA = qQpSvXhNBgGxofhpEHBDHBhVNpDeA2;
						ControllerMapLayoutManager_Rule_Editor controllerMapLayoutManager_Rule_Editor2 = controllerMapLayoutManager_RuleSet_Editor.rules[k];
						if (controllerMapLayoutManager_Rule_Editor2 != null && controllerMapLayoutManager_Rule_Editor2.layoutId > 0)
						{
							ControllerType controllerType = controllerMapLayoutManager_Rule_Editor2.controllerSetSelector.controllerType;
							List<gIBPcMQrDhIZFlpFWCJzmmnjiNkm> list2 = CbJHWLTvKsRFOifIrUgGvisjgImcA(controllerType);
							rIAiUiGFnhBrJchvEInIczDFhWrub2.jsdpSSHZRljSeokqEGytoPlSNoao = controllerMapLayoutManager_Rule_Editor2.layoutId;
							gIBPcMQrDhIZFlpFWCJzmmnjiNkm gIBPcMQrDhIZFlpFWCJzmmnjiNkm3 = list2.Find(rIAiUiGFnhBrJchvEInIczDFhWrub2.SWRzkCHKKDshiNTraeCWymGVJcfE);
							if (gIBPcMQrDhIZFlpFWCJzmmnjiNkm3 == null)
							{
								controllerMapLayoutManager_Rule_Editor2.layoutId = -1;
								Logger.LogError("No new " + controllerType.ToString() + " Layout Id found for old id: " + rIAiUiGFnhBrJchvEInIczDFhWrub2.jsdpSSHZRljSeokqEGytoPlSNoao);
							}
							else
							{
								controllerMapLayoutManager_Rule_Editor2.layoutId = gIBPcMQrDhIZFlpFWCJzmmnjiNkm3.qrhejhOprjbEagkMvfpSxTcPHfNtA;
							}
						}
					}
					int num4 = ((controllerMapLayoutManager_RuleSet_Editor.rules != null) ? controllerMapLayoutManager_RuleSet_Editor.rules.Count : 0);
					for (int l = 0; l < num4; l++)
					{
						ControllerMapLayoutManager_Rule_Editor controllerMapLayoutManager_Rule_Editor3 = controllerMapLayoutManager_RuleSet_Editor.rules[l];
						if (controllerMapLayoutManager_Rule_Editor3 != null && controllerMapLayoutManager_Rule_Editor3.controllerSetSelector != null && controllerMapLayoutManager_Rule_Editor3.controllerSetSelector.controllerType == ControllerType.Custom)
						{
							sLBumGEeVkgJyvPbSVypppqVnEF sLBumGEeVkgJyvPbSVypppqVnEF2 = new sLBumGEeVkgJyvPbSVypppqVnEF();
							sLBumGEeVkgJyvPbSVypppqVnEF2.ipyQPxYKrQoJFADpcZeafcBQLTvC = qQpSvXhNBgGxofhpEHBDHBhVNpDeA2;
							List<gIBPcMQrDhIZFlpFWCJzmmnjiNkm> bxvShEVaGesKdYRqoIRFhVDAsfCI = BxvShEVaGesKdYRqoIRFhVDAsfCI;
							sLBumGEeVkgJyvPbSVypppqVnEF2.QSZBaxcymNsUzzrMcoZZQwUsCAqK = controllerMapLayoutManager_Rule_Editor3.controllerSetSelector.customControllerSourceId;
							gIBPcMQrDhIZFlpFWCJzmmnjiNkm gIBPcMQrDhIZFlpFWCJzmmnjiNkm4 = bxvShEVaGesKdYRqoIRFhVDAsfCI.Find(sLBumGEeVkgJyvPbSVypppqVnEF2.zwFzDXtYWhtcJrlSsWKvgLSGtLCC);
							if (gIBPcMQrDhIZFlpFWCJzmmnjiNkm4 == null)
							{
								controllerMapLayoutManager_Rule_Editor3.controllerSetSelector.customControllerSourceId = -1;
								Logger.LogError("No new Custom Controller found for old id: " + sLBumGEeVkgJyvPbSVypppqVnEF2.QSZBaxcymNsUzzrMcoZZQwUsCAqK);
							}
							else
							{
								controllerMapLayoutManager_Rule_Editor3.controllerSetSelector.customControllerSourceId = gIBPcMQrDhIZFlpFWCJzmmnjiNkm4.qrhejhOprjbEagkMvfpSxTcPHfNtA;
							}
						}
					}
					ControllerMapLayoutManager_RuleSet_Editor controllerMapLayoutManager_RuleSet_Editor2;
					if (qQpSvXhNBgGxofhpEHBDHBhVNpDeA2.iTbDfsACSWETYaMsHzcweptwtqHl.fnkkZlyPXUXkbxepljPcfxGtgcTu)
					{
						controllerMapLayoutManager_RuleSet_Editor2 = qQpSvXhNBgGxofhpEHBDHBhVNpDeA2.iTbDfsACSWETYaMsHzcweptwtqHl.jFUdDvYgaHwuUQnjbfbCcgVkDAZFb;
					}
					else
					{
						VGcgcOsNmgmQWYkAXDCxgeYloFfeA.AddControllerMapLayoutManagerRuleSet();
						controllerMapLayoutManager_RuleSet_Editor2 = qQpSvXhNBgGxofhpEHBDHBhVNpDeA2.iTbDfsACSWETYaMsHzcweptwtqHl.zDJfCWfvzOnJQTofBEEdeXUQjwkF[qQpSvXhNBgGxofhpEHBDHBhVNpDeA2.iTbDfsACSWETYaMsHzcweptwtqHl.zDJfCWfvzOnJQTofBEEdeXUQjwkF.Count - 1];
					}
					controllerMapLayoutManager_RuleSet_Editor.id = controllerMapLayoutManager_RuleSet_Editor2.id;
					int index = qQpSvXhNBgGxofhpEHBDHBhVNpDeA2.iTbDfsACSWETYaMsHzcweptwtqHl.zDJfCWfvzOnJQTofBEEdeXUQjwkF.IndexOf(controllerMapLayoutManager_RuleSet_Editor2);
					qQpSvXhNBgGxofhpEHBDHBhVNpDeA2.iTbDfsACSWETYaMsHzcweptwtqHl.zDJfCWfvzOnJQTofBEEdeXUQjwkF[index] = controllerMapLayoutManager_RuleSet_Editor;
					return controllerMapLayoutManager_RuleSet_Editor;
				}

				internal ControllerMapEnabler_RuleSet_Editor UbuGHgmGdSilTgRoUChhxXirpguuA(okMYBPiHOMuwixSfVPWSIlunhHYl<ControllerMapEnabler_RuleSet_Editor> P_0)
				{
					zkVtzBreatSOLZVthBSiPMQyNCed zkVtzBreatSOLZVthBSiPMQyNCed2 = new zkVtzBreatSOLZVthBSiPMQyNCed();
					zkVtzBreatSOLZVthBSiPMQyNCed2.SHjefFUwuWFlXlQfxXVBfKKACWpcA = P_0;
					ControllerMapEnabler_RuleSet_Editor controllerMapEnabler_RuleSet_Editor = JsonTools.Clone(zkVtzBreatSOLZVthBSiPMQyNCed2.SHjefFUwuWFlXlQfxXVBfKKACWpcA.FVLGSdhpzjAimcwGFLVvMxxwOUGq);
					int num = ((controllerMapEnabler_RuleSet_Editor.rules != null) ? controllerMapEnabler_RuleSet_Editor.rules.Count : 0);
					for (int i = 0; i < num; i++)
					{
						ControllerMapEnabler_Rule_Editor controllerMapEnabler_Rule_Editor = controllerMapEnabler_RuleSet_Editor.rules[i];
						if (controllerMapEnabler_Rule_Editor == null || controllerMapEnabler_Rule_Editor.categoryIds == null)
						{
							continue;
						}
						List<int> list = new List<int>();
						for (int j = 0; j < controllerMapEnabler_Rule_Editor.categoryIds.Count; j++)
						{
							XrSaileFHJMnoSvTZHUuFoFtgStgA xrSaileFHJMnoSvTZHUuFoFtgStgA = new XrSaileFHJMnoSvTZHUuFoFtgStgA();
							xrSaileFHJMnoSvTZHUuFoFtgStgA.oFcOyoPVrhegIPCpcFEiFbnqyodIA = zkVtzBreatSOLZVthBSiPMQyNCed2;
							xrSaileFHJMnoSvTZHUuFoFtgStgA.HEPasztPgfQDRNvIzGCwgnBVoRCI = controllerMapEnabler_Rule_Editor.categoryIds[j];
							gIBPcMQrDhIZFlpFWCJzmmnjiNkm gIBPcMQrDhIZFlpFWCJzmmnjiNkm2 = azPMbUiryIgrSfKcUyTkLdttmScO.Find(xrSaileFHJMnoSvTZHUuFoFtgStgA.pLQYgmZCFGTkJHBqIyZQGnyQJfnn);
							if (gIBPcMQrDhIZFlpFWCJzmmnjiNkm2 == null)
							{
								Logger.LogError("No new Map Category Id found for old id: " + xrSaileFHJMnoSvTZHUuFoFtgStgA.HEPasztPgfQDRNvIzGCwgnBVoRCI);
							}
							else
							{
								list.Add(gIBPcMQrDhIZFlpFWCJzmmnjiNkm2.qrhejhOprjbEagkMvfpSxTcPHfNtA);
							}
						}
						controllerMapEnabler_Rule_Editor.categoryIds = list;
					}
					int num2 = ((controllerMapEnabler_RuleSet_Editor.rules != null) ? controllerMapEnabler_RuleSet_Editor.rules.Count : 0);
					for (int k = 0; k < num2; k++)
					{
						ControllerMapEnabler_Rule_Editor controllerMapEnabler_Rule_Editor2 = controllerMapEnabler_RuleSet_Editor.rules[k];
						if (controllerMapEnabler_Rule_Editor2 == null || controllerMapEnabler_Rule_Editor2.layoutIds == null)
						{
							continue;
						}
						ControllerType controllerType = controllerMapEnabler_Rule_Editor2.controllerSetSelector.controllerType;
						List<gIBPcMQrDhIZFlpFWCJzmmnjiNkm> list2 = CbJHWLTvKsRFOifIrUgGvisjgImcA(controllerType);
						List<int> list3 = new List<int>();
						int num3 = ((controllerMapEnabler_Rule_Editor2.layoutIds != null) ? controllerMapEnabler_Rule_Editor2.layoutIds.Count : 0);
						for (int l = 0; l < num3; l++)
						{
							hbpcYEpVAFoWsDjWhOWybSkPLTfh hbpcYEpVAFoWsDjWhOWybSkPLTfh2 = new hbpcYEpVAFoWsDjWhOWybSkPLTfh();
							hbpcYEpVAFoWsDjWhOWybSkPLTfh2.nTeUnOtnJYIXigzvTbViwQLQPzoO = zkVtzBreatSOLZVthBSiPMQyNCed2;
							hbpcYEpVAFoWsDjWhOWybSkPLTfh2.mtFBsIVKCPQJGpbvDawToTrOqfFS = controllerMapEnabler_Rule_Editor2.layoutIds[l];
							gIBPcMQrDhIZFlpFWCJzmmnjiNkm gIBPcMQrDhIZFlpFWCJzmmnjiNkm3 = list2.Find(hbpcYEpVAFoWsDjWhOWybSkPLTfh2.JgiFWhGQOzNKtsuNoKHVyaCfogps);
							if (gIBPcMQrDhIZFlpFWCJzmmnjiNkm3 == null)
							{
								Logger.LogError("No new " + controllerType.ToString() + " Layout Id found for old id: " + hbpcYEpVAFoWsDjWhOWybSkPLTfh2.mtFBsIVKCPQJGpbvDawToTrOqfFS);
							}
							else
							{
								list3.Add(gIBPcMQrDhIZFlpFWCJzmmnjiNkm3.qrhejhOprjbEagkMvfpSxTcPHfNtA);
							}
						}
						controllerMapEnabler_Rule_Editor2.layoutIds = list3;
					}
					int num4 = ((controllerMapEnabler_RuleSet_Editor.rules != null) ? controllerMapEnabler_RuleSet_Editor.rules.Count : 0);
					for (int m = 0; m < num4; m++)
					{
						ControllerMapEnabler_Rule_Editor controllerMapEnabler_Rule_Editor3 = controllerMapEnabler_RuleSet_Editor.rules[m];
						if (controllerMapEnabler_Rule_Editor3 != null && controllerMapEnabler_Rule_Editor3.controllerSetSelector != null && controllerMapEnabler_Rule_Editor3.controllerSetSelector.controllerType == ControllerType.Custom)
						{
							cjEPtppCNbaiyfjTduIcsYUCHlPTA cjEPtppCNbaiyfjTduIcsYUCHlPTA2 = new cjEPtppCNbaiyfjTduIcsYUCHlPTA();
							cjEPtppCNbaiyfjTduIcsYUCHlPTA2.VbTCXInOPtgMfARryMhfqZLztdCtA = zkVtzBreatSOLZVthBSiPMQyNCed2;
							List<gIBPcMQrDhIZFlpFWCJzmmnjiNkm> bxvShEVaGesKdYRqoIRFhVDAsfCI = BxvShEVaGesKdYRqoIRFhVDAsfCI;
							cjEPtppCNbaiyfjTduIcsYUCHlPTA2.GDXwIqPpQyBYiZbSmEHIZKyifBRP = controllerMapEnabler_Rule_Editor3.controllerSetSelector.customControllerSourceId;
							gIBPcMQrDhIZFlpFWCJzmmnjiNkm gIBPcMQrDhIZFlpFWCJzmmnjiNkm4 = bxvShEVaGesKdYRqoIRFhVDAsfCI.Find(cjEPtppCNbaiyfjTduIcsYUCHlPTA2.adwaJNbpNcdXLUYiqeDCuheLleWFb);
							if (gIBPcMQrDhIZFlpFWCJzmmnjiNkm4 == null)
							{
								controllerMapEnabler_Rule_Editor3.controllerSetSelector.customControllerSourceId = -1;
								Logger.LogError("No new Custom Controller found for old id: " + cjEPtppCNbaiyfjTduIcsYUCHlPTA2.GDXwIqPpQyBYiZbSmEHIZKyifBRP);
							}
							else
							{
								controllerMapEnabler_Rule_Editor3.controllerSetSelector.customControllerSourceId = gIBPcMQrDhIZFlpFWCJzmmnjiNkm4.qrhejhOprjbEagkMvfpSxTcPHfNtA;
							}
						}
					}
					ControllerMapEnabler_RuleSet_Editor controllerMapEnabler_RuleSet_Editor2;
					if (zkVtzBreatSOLZVthBSiPMQyNCed2.SHjefFUwuWFlXlQfxXVBfKKACWpcA.fnkkZlyPXUXkbxepljPcfxGtgcTu)
					{
						controllerMapEnabler_RuleSet_Editor2 = zkVtzBreatSOLZVthBSiPMQyNCed2.SHjefFUwuWFlXlQfxXVBfKKACWpcA.jFUdDvYgaHwuUQnjbfbCcgVkDAZFb;
					}
					else
					{
						VGcgcOsNmgmQWYkAXDCxgeYloFfeA.AddControllerMapEnablerRuleSet();
						controllerMapEnabler_RuleSet_Editor2 = zkVtzBreatSOLZVthBSiPMQyNCed2.SHjefFUwuWFlXlQfxXVBfKKACWpcA.zDJfCWfvzOnJQTofBEEdeXUQjwkF[zkVtzBreatSOLZVthBSiPMQyNCed2.SHjefFUwuWFlXlQfxXVBfKKACWpcA.zDJfCWfvzOnJQTofBEEdeXUQjwkF.Count - 1];
					}
					controllerMapEnabler_RuleSet_Editor.id = controllerMapEnabler_RuleSet_Editor2.id;
					int index = zkVtzBreatSOLZVthBSiPMQyNCed2.SHjefFUwuWFlXlQfxXVBfKKACWpcA.zDJfCWfvzOnJQTofBEEdeXUQjwkF.IndexOf(controllerMapEnabler_RuleSet_Editor2);
					zkVtzBreatSOLZVthBSiPMQyNCed2.SHjefFUwuWFlXlQfxXVBfKKACWpcA.zDJfCWfvzOnJQTofBEEdeXUQjwkF[index] = controllerMapEnabler_RuleSet_Editor;
					return controllerMapEnabler_RuleSet_Editor;
				}

				internal Player_Editor vNzsOWDlUIepQcVnJpnwpXnLGOSd(okMYBPiHOMuwixSfVPWSIlunhHYl<Player_Editor> P_0)
				{
					SBlLpmkijhRKaSxArPrxmMGRiQHn sBlLpmkijhRKaSxArPrxmMGRiQHn = new SBlLpmkijhRKaSxArPrxmMGRiQHn();
					sBlLpmkijhRKaSxArPrxmMGRiQHn.WvzJMEHHSXDmaKVLNdsdnLCOXDGkA = this;
					sBlLpmkijhRKaSxArPrxmMGRiQHn.wyIAZcpjdxJNNwkYWvGEuzgETQLX = P_0;
					Player_Editor player_Editor = JsonTools.Clone(sBlLpmkijhRKaSxArPrxmMGRiQHn.wyIAZcpjdxJNNwkYWvGEuzgETQLX.FVLGSdhpzjAimcwGFLVvMxxwOUGq);
					Action<List<Player_Editor.Mapping>, List<gIBPcMQrDhIZFlpFWCJzmmnjiNkm>> action = sBlLpmkijhRKaSxArPrxmMGRiQHn.eShheKDALQCErigczENRVCqtqgMDA;
					action(player_Editor.defaultKeyboardMaps, WrBLrEQoisYZPIWzDfJYdiITgZtfA);
					action(player_Editor.defaultMouseMaps, zSRKGqTMdaxenOqwPohNjSGvKuZS);
					action(player_Editor.defaultJoystickMaps, nYtbnZrMSQbsMDwMuxJhWhyPAGMl);
					action(player_Editor.defaultCustomControllerMaps, LkDUmDwrUoTLTkfJQEsCqAtIFFUHA);
					for (int i = 0; i < player_Editor.startingCustomControllers.Count; i++)
					{
						nLExDZWhtvvyntIylAabknmLNvwd nLExDZWhtvvyntIylAabknmLNvwd2 = new nLExDZWhtvvyntIylAabknmLNvwd();
						nLExDZWhtvvyntIylAabknmLNvwd2.YefgKpHMeqjQQrndwravEkWjThXeB = sBlLpmkijhRKaSxArPrxmMGRiQHn;
						nLExDZWhtvvyntIylAabknmLNvwd2.FyLEtUIlZUkBFFAAVWyfINfbOcxV = player_Editor.startingCustomControllers[i];
						gIBPcMQrDhIZFlpFWCJzmmnjiNkm gIBPcMQrDhIZFlpFWCJzmmnjiNkm2 = BxvShEVaGesKdYRqoIRFhVDAsfCI.Find(nLExDZWhtvvyntIylAabknmLNvwd2.WQYibXZfZkUsNKGsHfGCaqMtDYpEA);
						nLExDZWhtvvyntIylAabknmLNvwd2.FyLEtUIlZUkBFFAAVWyfINfbOcxV.sourceId = gIBPcMQrDhIZFlpFWCJzmmnjiNkm2?.qrhejhOprjbEagkMvfpSxTcPHfNtA ?? (-1);
					}
					List<Player_Editor.RuleSetMapping> list = new List<Player_Editor.RuleSetMapping>();
					List<Player_Editor.RuleSetMapping> ruleSets = player_Editor.controllerMapLayoutManagerSettings.ruleSets;
					for (int j = 0; j < ruleSets.Count; j++)
					{
						jLNfaNjUrxPajIoRfdZecklZslSj jLNfaNjUrxPajIoRfdZecklZslSj2 = new jLNfaNjUrxPajIoRfdZecklZslSj();
						jLNfaNjUrxPajIoRfdZecklZslSj2.yPRObqeVjrskIarpTprFqhsnkVbY = sBlLpmkijhRKaSxArPrxmMGRiQHn;
						Player_Editor.RuleSetMapping ruleSetMapping = ruleSets[j];
						if (ruleSetMapping != null)
						{
							jLNfaNjUrxPajIoRfdZecklZslSj2.ZEPHBRkGrVuvXGgQxcuPcElQqashA = ruleSetMapping.id;
							gIBPcMQrDhIZFlpFWCJzmmnjiNkm gIBPcMQrDhIZFlpFWCJzmmnjiNkm3 = fNeYIWEKThcTUdEIrtoRcwIQYAwJ.Find(jLNfaNjUrxPajIoRfdZecklZslSj2.dANWAvRuhQlVYtYxQqbvlqbjivqY);
							if (gIBPcMQrDhIZFlpFWCJzmmnjiNkm3 == null)
							{
								Logger.LogError("No new Controller Map Layout Manager Set found for old id: " + jLNfaNjUrxPajIoRfdZecklZslSj2.ZEPHBRkGrVuvXGgQxcuPcElQqashA);
								continue;
							}
							ruleSetMapping = ruleSetMapping.Clone();
							ruleSetMapping.id = gIBPcMQrDhIZFlpFWCJzmmnjiNkm3.qrhejhOprjbEagkMvfpSxTcPHfNtA;
							list.Add(ruleSetMapping);
						}
					}
					player_Editor.controllerMapLayoutManagerSettings.ruleSets = list;
					List<Player_Editor.RuleSetMapping> list2 = new List<Player_Editor.RuleSetMapping>();
					List<Player_Editor.RuleSetMapping> ruleSets2 = player_Editor.controllerMapEnablerSettings.ruleSets;
					for (int k = 0; k < ruleSets2.Count; k++)
					{
						caIaxOMYabNHgiAkiYgUSlmiECw caIaxOMYabNHgiAkiYgUSlmiECw2 = new caIaxOMYabNHgiAkiYgUSlmiECw();
						caIaxOMYabNHgiAkiYgUSlmiECw2.FfUFnnEBkAkpSGpSlIhEbezgwMSk = sBlLpmkijhRKaSxArPrxmMGRiQHn;
						Player_Editor.RuleSetMapping ruleSetMapping2 = ruleSets2[k];
						if (ruleSetMapping2 != null)
						{
							caIaxOMYabNHgiAkiYgUSlmiECw2.WGxhvOCVchqVqnBBsLOkpVXcBUnu = ruleSetMapping2.id;
							gIBPcMQrDhIZFlpFWCJzmmnjiNkm gIBPcMQrDhIZFlpFWCJzmmnjiNkm4 = NwxqNrKQGJvHyVDFwgMmaoSsZjNeA.Find(caIaxOMYabNHgiAkiYgUSlmiECw2.fzcEslQDnpbVOnJwIezmAzemxYKm);
							if (gIBPcMQrDhIZFlpFWCJzmmnjiNkm4 == null)
							{
								Logger.LogError("No new Controller Map Enabler Set found for old id: " + caIaxOMYabNHgiAkiYgUSlmiECw2.WGxhvOCVchqVqnBBsLOkpVXcBUnu);
								continue;
							}
							ruleSetMapping2 = ruleSetMapping2.Clone();
							ruleSetMapping2.id = gIBPcMQrDhIZFlpFWCJzmmnjiNkm4.qrhejhOprjbEagkMvfpSxTcPHfNtA;
							list2.Add(ruleSetMapping2);
						}
					}
					player_Editor.controllerMapEnablerSettings.ruleSets = list2;
					Player_Editor player_Editor2;
					if (sBlLpmkijhRKaSxArPrxmMGRiQHn.wyIAZcpjdxJNNwkYWvGEuzgETQLX.fnkkZlyPXUXkbxepljPcfxGtgcTu)
					{
						player_Editor2 = sBlLpmkijhRKaSxArPrxmMGRiQHn.wyIAZcpjdxJNNwkYWvGEuzgETQLX.jFUdDvYgaHwuUQnjbfbCcgVkDAZFb;
						Player_Editor player_Editor3 = JsonTools.Clone(player_Editor);
						player_Editor3.defaultKeyboardMaps.Clear();
						player_Editor3.defaultMouseMaps.Clear();
						player_Editor3.defaultJoystickMaps.Clear();
						player_Editor3.defaultCustomControllerMaps.Clear();
						player_Editor3.startingCustomControllers.Clear();
						Func<Player_Editor.Mapping, IList<Player_Editor.Mapping>, int> func = mseacdDYvvCxiPOCKMxdHSAWgPDjb._003C_003E9.NxQXsYQgfukkLSgjiasEZoVbsWDH;
						ocQEtlXNkJtMEVpcDsjDtyoJEAQQ(player_Editor2.defaultKeyboardMaps, player_Editor.defaultKeyboardMaps, player_Editor3.defaultKeyboardMaps, func);
						ocQEtlXNkJtMEVpcDsjDtyoJEAQQ(player_Editor2.defaultMouseMaps, player_Editor.defaultMouseMaps, player_Editor3.defaultMouseMaps, func);
						ocQEtlXNkJtMEVpcDsjDtyoJEAQQ(player_Editor2.defaultJoystickMaps, player_Editor.defaultJoystickMaps, player_Editor3.defaultJoystickMaps, func);
						ocQEtlXNkJtMEVpcDsjDtyoJEAQQ(player_Editor2.defaultCustomControllerMaps, player_Editor.defaultCustomControllerMaps, player_Editor3.defaultCustomControllerMaps, func);
						ocQEtlXNkJtMEVpcDsjDtyoJEAQQ(player_Editor2.startingCustomControllers, player_Editor.startingCustomControllers, player_Editor3.startingCustomControllers, mseacdDYvvCxiPOCKMxdHSAWgPDjb._003C_003E9.mffsLDvIqbfrxCZxIVtPaJWmZwxRA);
						player_Editor = player_Editor3;
					}
					else
					{
						VGcgcOsNmgmQWYkAXDCxgeYloFfeA.AddPlayer();
						player_Editor2 = sBlLpmkijhRKaSxArPrxmMGRiQHn.wyIAZcpjdxJNNwkYWvGEuzgETQLX.zDJfCWfvzOnJQTofBEEdeXUQjwkF[sBlLpmkijhRKaSxArPrxmMGRiQHn.wyIAZcpjdxJNNwkYWvGEuzgETQLX.zDJfCWfvzOnJQTofBEEdeXUQjwkF.Count - 1];
					}
					player_Editor.id = player_Editor2.id;
					int index = sBlLpmkijhRKaSxArPrxmMGRiQHn.wyIAZcpjdxJNNwkYWvGEuzgETQLX.zDJfCWfvzOnJQTofBEEdeXUQjwkF.IndexOf(player_Editor2);
					sBlLpmkijhRKaSxArPrxmMGRiQHn.wyIAZcpjdxJNNwkYWvGEuzgETQLX.zDJfCWfvzOnJQTofBEEdeXUQjwkF[index] = player_Editor;
					return player_Editor;
				}
			}

			private sealed class ZjzqbCWADWxNHblhGxQgbOuopCnj
			{
				public okMYBPiHOMuwixSfVPWSIlunhHYl<InputAction> GzMgzDrRoXiWTYOFZKsUDlXzsslk;

				internal bool twUCwQilNQPcGpyfsgvLnKBHdPqm(gIBPcMQrDhIZFlpFWCJzmmnjiNkm P_0)
				{
					return P_0.fDoSYUFwxFeTVLhMrbgEBYlYhFJu(GzMgzDrRoXiWTYOFZKsUDlXzsslk.FVGMuRfCeZZjKXFaZLuJYUrOHJsf) == GzMgzDrRoXiWTYOFZKsUDlXzsslk.FVLGSdhpzjAimcwGFLVvMxxwOUGq.categoryId;
				}

				internal bool CHyBHuIpTFoTZcSEZsXqVYrYEETh(gIBPcMQrDhIZFlpFWCJzmmnjiNkm P_0)
				{
					return P_0.fDoSYUFwxFeTVLhMrbgEBYlYhFJu(GzMgzDrRoXiWTYOFZKsUDlXzsslk.FVGMuRfCeZZjKXFaZLuJYUrOHJsf) == GzMgzDrRoXiWTYOFZKsUDlXzsslk.FVLGSdhpzjAimcwGFLVvMxxwOUGq.behaviorId;
				}
			}

			private sealed class hbpcYEpVAFoWsDjWhOWybSkPLTfh
			{
				public int mtFBsIVKCPQJGpbvDawToTrOqfFS;

				public zkVtzBreatSOLZVthBSiPMQyNCed nTeUnOtnJYIXigzvTbViwQLQPzoO;

				internal bool JgiFWhGQOzNKtsuNoKHVyaCfogps(gIBPcMQrDhIZFlpFWCJzmmnjiNkm P_0)
				{
					return P_0.fDoSYUFwxFeTVLhMrbgEBYlYhFJu(nTeUnOtnJYIXigzvTbViwQLQPzoO.SHjefFUwuWFlXlQfxXVBfKKACWpcA.FVGMuRfCeZZjKXFaZLuJYUrOHJsf) == mtFBsIVKCPQJGpbvDawToTrOqfFS;
				}
			}

			private sealed class cjEPtppCNbaiyfjTduIcsYUCHlPTA
			{
				public int GDXwIqPpQyBYiZbSmEHIZKyifBRP;

				public zkVtzBreatSOLZVthBSiPMQyNCed VbTCXInOPtgMfARryMhfqZLztdCtA;

				internal bool adwaJNbpNcdXLUYiqeDCuheLleWFb(gIBPcMQrDhIZFlpFWCJzmmnjiNkm P_0)
				{
					return P_0.fDoSYUFwxFeTVLhMrbgEBYlYhFJu(VbTCXInOPtgMfARryMhfqZLztdCtA.SHjefFUwuWFlXlQfxXVBfKKACWpcA.FVGMuRfCeZZjKXFaZLuJYUrOHJsf) == GDXwIqPpQyBYiZbSmEHIZKyifBRP;
				}
			}

			private sealed class SBlLpmkijhRKaSxArPrxmMGRiQHn
			{
				public okMYBPiHOMuwixSfVPWSIlunhHYl<Player_Editor> wyIAZcpjdxJNNwkYWvGEuzgETQLX;

				public pNiBgndhirjKwSeKSnElGpWIhUIab WvzJMEHHSXDmaKVLNdsdnLCOXDGkA;

				internal void eShheKDALQCErigczENRVCqtqgMDA(List<Player_Editor.Mapping> P_0, List<gIBPcMQrDhIZFlpFWCJzmmnjiNkm> P_1)
				{
					for (int i = 0; i < P_0.Count; i++)
					{
						NttAQRLRMogLOKqNBxgFAewIdjDP nttAQRLRMogLOKqNBxgFAewIdjDP = new NttAQRLRMogLOKqNBxgFAewIdjDP();
						nttAQRLRMogLOKqNBxgFAewIdjDP.JbdbNnjdIrPnthxXfWOnrgSJNlUQb = this;
						nttAQRLRMogLOKqNBxgFAewIdjDP.DPankPPTGzhPXJCcDlhRqtvHccIE = P_0[i];
						gIBPcMQrDhIZFlpFWCJzmmnjiNkm gIBPcMQrDhIZFlpFWCJzmmnjiNkm2 = WvzJMEHHSXDmaKVLNdsdnLCOXDGkA.azPMbUiryIgrSfKcUyTkLdttmScO.Find(nttAQRLRMogLOKqNBxgFAewIdjDP.WUAcpxQMPKDFLlXCABLvDNcaGtJz);
						nttAQRLRMogLOKqNBxgFAewIdjDP.DPankPPTGzhPXJCcDlhRqtvHccIE.categoryId = gIBPcMQrDhIZFlpFWCJzmmnjiNkm2?.qrhejhOprjbEagkMvfpSxTcPHfNtA ?? (-1);
						gIBPcMQrDhIZFlpFWCJzmmnjiNkm2 = P_1.Find(nttAQRLRMogLOKqNBxgFAewIdjDP.MXBEdzDfdYNNRxGSxmkUEVzxRtHeA);
						nttAQRLRMogLOKqNBxgFAewIdjDP.DPankPPTGzhPXJCcDlhRqtvHccIE.layoutId = gIBPcMQrDhIZFlpFWCJzmmnjiNkm2?.qrhejhOprjbEagkMvfpSxTcPHfNtA ?? (-1);
					}
				}
			}

			private sealed class NttAQRLRMogLOKqNBxgFAewIdjDP
			{
				public Player_Editor.Mapping DPankPPTGzhPXJCcDlhRqtvHccIE;

				public SBlLpmkijhRKaSxArPrxmMGRiQHn JbdbNnjdIrPnthxXfWOnrgSJNlUQb;

				internal bool WUAcpxQMPKDFLlXCABLvDNcaGtJz(gIBPcMQrDhIZFlpFWCJzmmnjiNkm P_0)
				{
					return P_0.fDoSYUFwxFeTVLhMrbgEBYlYhFJu(JbdbNnjdIrPnthxXfWOnrgSJNlUQb.wyIAZcpjdxJNNwkYWvGEuzgETQLX.FVGMuRfCeZZjKXFaZLuJYUrOHJsf) == DPankPPTGzhPXJCcDlhRqtvHccIE.categoryId;
				}

				internal bool MXBEdzDfdYNNRxGSxmkUEVzxRtHeA(gIBPcMQrDhIZFlpFWCJzmmnjiNkm P_0)
				{
					return P_0.fDoSYUFwxFeTVLhMrbgEBYlYhFJu(JbdbNnjdIrPnthxXfWOnrgSJNlUQb.wyIAZcpjdxJNNwkYWvGEuzgETQLX.FVGMuRfCeZZjKXFaZLuJYUrOHJsf) == DPankPPTGzhPXJCcDlhRqtvHccIE.layoutId;
				}
			}

			private sealed class nLExDZWhtvvyntIylAabknmLNvwd
			{
				public Player_Editor.CreateControllerInfo FyLEtUIlZUkBFFAAVWyfINfbOcxV;

				public SBlLpmkijhRKaSxArPrxmMGRiQHn YefgKpHMeqjQQrndwravEkWjThXeB;

				internal bool WQYibXZfZkUsNKGsHfGCaqMtDYpEA(gIBPcMQrDhIZFlpFWCJzmmnjiNkm P_0)
				{
					return P_0.fDoSYUFwxFeTVLhMrbgEBYlYhFJu(YefgKpHMeqjQQrndwravEkWjThXeB.wyIAZcpjdxJNNwkYWvGEuzgETQLX.FVGMuRfCeZZjKXFaZLuJYUrOHJsf) == FyLEtUIlZUkBFFAAVWyfINfbOcxV.sourceId;
				}
			}

			private sealed class jLNfaNjUrxPajIoRfdZecklZslSj
			{
				public int ZEPHBRkGrVuvXGgQxcuPcElQqashA;

				public SBlLpmkijhRKaSxArPrxmMGRiQHn yPRObqeVjrskIarpTprFqhsnkVbY;

				internal bool dANWAvRuhQlVYtYxQqbvlqbjivqY(gIBPcMQrDhIZFlpFWCJzmmnjiNkm P_0)
				{
					return P_0.fDoSYUFwxFeTVLhMrbgEBYlYhFJu(yPRObqeVjrskIarpTprFqhsnkVbY.wyIAZcpjdxJNNwkYWvGEuzgETQLX.FVGMuRfCeZZjKXFaZLuJYUrOHJsf) == ZEPHBRkGrVuvXGgQxcuPcElQqashA;
				}
			}

			private sealed class caIaxOMYabNHgiAkiYgUSlmiECw
			{
				public int WGxhvOCVchqVqnBBsLOkpVXcBUnu;

				public SBlLpmkijhRKaSxArPrxmMGRiQHn FfUFnnEBkAkpSGpSlIhEbezgwMSk;

				internal bool fzcEslQDnpbVOnJwIezmAzemxYKm(gIBPcMQrDhIZFlpFWCJzmmnjiNkm P_0)
				{
					return P_0.fDoSYUFwxFeTVLhMrbgEBYlYhFJu(FfUFnnEBkAkpSGpSlIhEbezgwMSk.wyIAZcpjdxJNNwkYWvGEuzgETQLX.FVGMuRfCeZZjKXFaZLuJYUrOHJsf) == WGxhvOCVchqVqnBBsLOkpVXcBUnu;
				}
			}

			private sealed class RJqUxBmpHyZkjWyIPWZQNVPkaIhi
			{
				public List<gIBPcMQrDhIZFlpFWCJzmmnjiNkm> EoSFXfCXLtZOdQWLAAtrDdhMjJvpA;

				public pNiBgndhirjKwSeKSnElGpWIhUIab TYkYTCiKvRnYFFzxKmwMbzumHSED;

				internal int eXOEOiliMFsQVbJeSawSdbtsvJqQ(ControllerMap_Editor P_0, IList<ControllerMap_Editor> P_1)
				{
					mHpniZNDXEJqhxhSKODUklJWIKwE mHpniZNDXEJqhxhSKODUklJWIKwE2 = new mHpniZNDXEJqhxhSKODUklJWIKwE();
					mHpniZNDXEJqhxhSKODUklJWIKwE2.eURcQPftwejaEZCKaNdgYVgbJLTZ = P_0;
					for (int i = 0; i < P_1.Count; i++)
					{
						gIBPcMQrDhIZFlpFWCJzmmnjiNkm gIBPcMQrDhIZFlpFWCJzmmnjiNkm2 = TYkYTCiKvRnYFFzxKmwMbzumHSED.azPMbUiryIgrSfKcUyTkLdttmScO.Find(mHpniZNDXEJqhxhSKODUklJWIKwE2.TzUmfHzeyHANQivqolujbbvchXhc);
						gIBPcMQrDhIZFlpFWCJzmmnjiNkm gIBPcMQrDhIZFlpFWCJzmmnjiNkm3 = EoSFXfCXLtZOdQWLAAtrDdhMjJvpA.Find(mHpniZNDXEJqhxhSKODUklJWIKwE2.BeAAGKZDdZgmopNhKLnxfquCnSuB);
						if (gIBPcMQrDhIZFlpFWCJzmmnjiNkm2 != null && gIBPcMQrDhIZFlpFWCJzmmnjiNkm2.qrhejhOprjbEagkMvfpSxTcPHfNtA == P_1[i].categoryId && gIBPcMQrDhIZFlpFWCJzmmnjiNkm3 != null && gIBPcMQrDhIZFlpFWCJzmmnjiNkm3.qrhejhOprjbEagkMvfpSxTcPHfNtA == P_1[i].layoutId)
						{
							return i;
						}
					}
					return -1;
				}

				internal ControllerMap_Editor qauxrNSTcduxzDxLBAXXkgQrhnBV(okMYBPiHOMuwixSfVPWSIlunhHYl<ControllerMap_Editor> P_0)
				{
					TqdQscyFSFKqjZbJIcEbNJCrZzVi tqdQscyFSFKqjZbJIcEbNJCrZzVi = new TqdQscyFSFKqjZbJIcEbNJCrZzVi();
					tqdQscyFSFKqjZbJIcEbNJCrZzVi.uQtbwUTxZcYfEiBzeIiRojdCQeHP = P_0;
					tqdQscyFSFKqjZbJIcEbNJCrZzVi.dZkfwNzkrjxbhmPpMQjTwcBheDGA = JsonTools.Clone(tqdQscyFSFKqjZbJIcEbNJCrZzVi.uQtbwUTxZcYfEiBzeIiRojdCQeHP.FVLGSdhpzjAimcwGFLVvMxxwOUGq);
					gIBPcMQrDhIZFlpFWCJzmmnjiNkm gIBPcMQrDhIZFlpFWCJzmmnjiNkm2 = TYkYTCiKvRnYFFzxKmwMbzumHSED.azPMbUiryIgrSfKcUyTkLdttmScO.Find(tqdQscyFSFKqjZbJIcEbNJCrZzVi.ooFdPJYzKcnSgjcCXlRbOTJTDMlb);
					gIBPcMQrDhIZFlpFWCJzmmnjiNkm gIBPcMQrDhIZFlpFWCJzmmnjiNkm3 = EoSFXfCXLtZOdQWLAAtrDdhMjJvpA.Find(tqdQscyFSFKqjZbJIcEbNJCrZzVi.liHdEKqvNWYFVgjebwTQouCLvJqU);
					tqdQscyFSFKqjZbJIcEbNJCrZzVi.dZkfwNzkrjxbhmPpMQjTwcBheDGA.categoryId = gIBPcMQrDhIZFlpFWCJzmmnjiNkm2?.qrhejhOprjbEagkMvfpSxTcPHfNtA ?? (-1);
					tqdQscyFSFKqjZbJIcEbNJCrZzVi.dZkfwNzkrjxbhmPpMQjTwcBheDGA.layoutId = gIBPcMQrDhIZFlpFWCJzmmnjiNkm3?.qrhejhOprjbEagkMvfpSxTcPHfNtA ?? (-1);
					for (int i = 0; i < tqdQscyFSFKqjZbJIcEbNJCrZzVi.dZkfwNzkrjxbhmPpMQjTwcBheDGA.actionElementMaps.Count; i++)
					{
						iFqDCxbkhiQXcQfyQDLfhkCJoSQGA iFqDCxbkhiQXcQfyQDLfhkCJoSQGA2 = new iFqDCxbkhiQXcQfyQDLfhkCJoSQGA();
						iFqDCxbkhiQXcQfyQDLfhkCJoSQGA2.JJXgGIwvwaFgbOhTujlfBxgRMTMn = tqdQscyFSFKqjZbJIcEbNJCrZzVi;
						iFqDCxbkhiQXcQfyQDLfhkCJoSQGA2.xfediqeVkGuRhquuqrQKopjIqqoC = iFqDCxbkhiQXcQfyQDLfhkCJoSQGA2.JJXgGIwvwaFgbOhTujlfBxgRMTMn.dZkfwNzkrjxbhmPpMQjTwcBheDGA.actionElementMaps[i];
						gIBPcMQrDhIZFlpFWCJzmmnjiNkm gIBPcMQrDhIZFlpFWCJzmmnjiNkm4 = TYkYTCiKvRnYFFzxKmwMbzumHSED.OqpaNlVZdIOZBWSzcdKBDFxyiLEN.Find(iFqDCxbkhiQXcQfyQDLfhkCJoSQGA2.bHWgXVaBAQRYawrpSYLxqTvidgjaA);
						iFqDCxbkhiQXcQfyQDLfhkCJoSQGA2.xfediqeVkGuRhquuqrQKopjIqqoC._actionId = gIBPcMQrDhIZFlpFWCJzmmnjiNkm4?.qrhejhOprjbEagkMvfpSxTcPHfNtA ?? (-1);
						iFqDCxbkhiQXcQfyQDLfhkCJoSQGA2.xfediqeVkGuRhquuqrQKopjIqqoC._actionCategoryId = ((TYkYTCiKvRnYFFzxKmwMbzumHSED.VGcgcOsNmgmQWYkAXDCxgeYloFfeA.GetActionById(iFqDCxbkhiQXcQfyQDLfhkCJoSQGA2.xfediqeVkGuRhquuqrQKopjIqqoC._actionId) != null) ? TYkYTCiKvRnYFFzxKmwMbzumHSED.VGcgcOsNmgmQWYkAXDCxgeYloFfeA.GetActionById(iFqDCxbkhiQXcQfyQDLfhkCJoSQGA2.xfediqeVkGuRhquuqrQKopjIqqoC._actionId).categoryId : 0);
					}
					ControllerMap_Editor controllerMap_Editor;
					if (tqdQscyFSFKqjZbJIcEbNJCrZzVi.uQtbwUTxZcYfEiBzeIiRojdCQeHP.fnkkZlyPXUXkbxepljPcfxGtgcTu)
					{
						controllerMap_Editor = tqdQscyFSFKqjZbJIcEbNJCrZzVi.uQtbwUTxZcYfEiBzeIiRojdCQeHP.jFUdDvYgaHwuUQnjbfbCcgVkDAZFb;
						ControllerMap_Editor controllerMap_Editor2 = JsonTools.Clone(tqdQscyFSFKqjZbJIcEbNJCrZzVi.dZkfwNzkrjxbhmPpMQjTwcBheDGA);
						controllerMap_Editor2.actionElementMaps.Clear();
						Func<ActionElementMap, IList<ActionElementMap>, int> func = mseacdDYvvCxiPOCKMxdHSAWgPDjb._003C_003E9.sgGgsyaOhnZVNDIgVNQjRHWgKbdBA;
						ocQEtlXNkJtMEVpcDsjDtyoJEAQQ(controllerMap_Editor.actionElementMaps, tqdQscyFSFKqjZbJIcEbNJCrZzVi.dZkfwNzkrjxbhmPpMQjTwcBheDGA.actionElementMaps, controllerMap_Editor2.actionElementMaps, func);
						tqdQscyFSFKqjZbJIcEbNJCrZzVi.dZkfwNzkrjxbhmPpMQjTwcBheDGA = controllerMap_Editor2;
					}
					else
					{
						TYkYTCiKvRnYFFzxKmwMbzumHSED.VGcgcOsNmgmQWYkAXDCxgeYloFfeA.CreateKeyboardMap(tqdQscyFSFKqjZbJIcEbNJCrZzVi.dZkfwNzkrjxbhmPpMQjTwcBheDGA.categoryId, tqdQscyFSFKqjZbJIcEbNJCrZzVi.dZkfwNzkrjxbhmPpMQjTwcBheDGA.layoutId);
						controllerMap_Editor = tqdQscyFSFKqjZbJIcEbNJCrZzVi.uQtbwUTxZcYfEiBzeIiRojdCQeHP.zDJfCWfvzOnJQTofBEEdeXUQjwkF[tqdQscyFSFKqjZbJIcEbNJCrZzVi.uQtbwUTxZcYfEiBzeIiRojdCQeHP.zDJfCWfvzOnJQTofBEEdeXUQjwkF.Count - 1];
					}
					tqdQscyFSFKqjZbJIcEbNJCrZzVi.dZkfwNzkrjxbhmPpMQjTwcBheDGA.id = controllerMap_Editor.id;
					int index = tqdQscyFSFKqjZbJIcEbNJCrZzVi.uQtbwUTxZcYfEiBzeIiRojdCQeHP.zDJfCWfvzOnJQTofBEEdeXUQjwkF.IndexOf(controllerMap_Editor);
					tqdQscyFSFKqjZbJIcEbNJCrZzVi.uQtbwUTxZcYfEiBzeIiRojdCQeHP.zDJfCWfvzOnJQTofBEEdeXUQjwkF[index] = tqdQscyFSFKqjZbJIcEbNJCrZzVi.dZkfwNzkrjxbhmPpMQjTwcBheDGA;
					return tqdQscyFSFKqjZbJIcEbNJCrZzVi.dZkfwNzkrjxbhmPpMQjTwcBheDGA;
				}
			}

			private sealed class mHpniZNDXEJqhxhSKODUklJWIKwE
			{
				public ControllerMap_Editor eURcQPftwejaEZCKaNdgYVgbJLTZ;

				public Predicate<gIBPcMQrDhIZFlpFWCJzmmnjiNkm> wIaXFrwWvtdRkJKHIIkQvojadbBp;

				public Predicate<gIBPcMQrDhIZFlpFWCJzmmnjiNkm> NyiZSSObFtAMFeNYukeiaonGbnhLA;

				internal bool TzUmfHzeyHANQivqolujbbvchXhc(gIBPcMQrDhIZFlpFWCJzmmnjiNkm P_0)
				{
					return P_0.prNJUrqsciTAXxyaCYjaYFJMFovV == eURcQPftwejaEZCKaNdgYVgbJLTZ.categoryId;
				}

				internal bool BeAAGKZDdZgmopNhKLnxfquCnSuB(gIBPcMQrDhIZFlpFWCJzmmnjiNkm P_0)
				{
					return P_0.prNJUrqsciTAXxyaCYjaYFJMFovV == eURcQPftwejaEZCKaNdgYVgbJLTZ.layoutId;
				}
			}

			private sealed class TqdQscyFSFKqjZbJIcEbNJCrZzVi
			{
				public okMYBPiHOMuwixSfVPWSIlunhHYl<ControllerMap_Editor> uQtbwUTxZcYfEiBzeIiRojdCQeHP;

				public ControllerMap_Editor dZkfwNzkrjxbhmPpMQjTwcBheDGA;

				internal bool ooFdPJYzKcnSgjcCXlRbOTJTDMlb(gIBPcMQrDhIZFlpFWCJzmmnjiNkm P_0)
				{
					return P_0.fDoSYUFwxFeTVLhMrbgEBYlYhFJu(uQtbwUTxZcYfEiBzeIiRojdCQeHP.FVGMuRfCeZZjKXFaZLuJYUrOHJsf) == dZkfwNzkrjxbhmPpMQjTwcBheDGA.categoryId;
				}

				internal bool liHdEKqvNWYFVgjebwTQouCLvJqU(gIBPcMQrDhIZFlpFWCJzmmnjiNkm P_0)
				{
					return P_0.fDoSYUFwxFeTVLhMrbgEBYlYhFJu(uQtbwUTxZcYfEiBzeIiRojdCQeHP.FVGMuRfCeZZjKXFaZLuJYUrOHJsf) == dZkfwNzkrjxbhmPpMQjTwcBheDGA.layoutId;
				}
			}

			private sealed class FNZQtWhPTEXepFRXjiFvckODlNxg
			{
				public List<int> YsbuNOyZhjlUoBWcKePWdghpoXlo;

				public pNiBgndhirjKwSeKSnElGpWIhUIab NyOxYkUooNNAOACadHszkFFsiouFA;

				internal InputMapCategory rJmGzOinXwDyIadpDlqyhrMMRVakb(okMYBPiHOMuwixSfVPWSIlunhHYl<InputMapCategory> P_0)
				{
					InputMapCategory inputMapCategory = JsonTools.Clone(P_0.FVLGSdhpzjAimcwGFLVvMxxwOUGq);
					InputMapCategory inputMapCategory2;
					if (P_0.fnkkZlyPXUXkbxepljPcfxGtgcTu)
					{
						inputMapCategory2 = P_0.jFUdDvYgaHwuUQnjbfbCcgVkDAZFb;
					}
					else
					{
						NyOxYkUooNNAOACadHszkFFsiouFA.VGcgcOsNmgmQWYkAXDCxgeYloFfeA.AddMapCategory();
						inputMapCategory2 = P_0.zDJfCWfvzOnJQTofBEEdeXUQjwkF[P_0.zDJfCWfvzOnJQTofBEEdeXUQjwkF.Count - 1];
					}
					int num = P_0.zDJfCWfvzOnJQTofBEEdeXUQjwkF.IndexOf(inputMapCategory2);
					if (P_0.FVGMuRfCeZZjKXFaZLuJYUrOHJsf == gIBPcMQrDhIZFlpFWCJzmmnjiNkm.CsysAjVUMGvYOWqTKclZPrJCrsOx.otherId)
					{
						YsbuNOyZhjlUoBWcKePWdghpoXlo.Add(num);
					}
					inputMapCategory.id = inputMapCategory2.id;
					P_0.zDJfCWfvzOnJQTofBEEdeXUQjwkF[num] = inputMapCategory;
					return inputMapCategory;
				}
			}

			private sealed class iFqDCxbkhiQXcQfyQDLfhkCJoSQGA
			{
				public ActionElementMap xfediqeVkGuRhquuqrQKopjIqqoC;

				public TqdQscyFSFKqjZbJIcEbNJCrZzVi JJXgGIwvwaFgbOhTujlfBxgRMTMn;

				internal bool bHWgXVaBAQRYawrpSYLxqTvidgjaA(gIBPcMQrDhIZFlpFWCJzmmnjiNkm P_0)
				{
					return P_0.fDoSYUFwxFeTVLhMrbgEBYlYhFJu(JJXgGIwvwaFgbOhTujlfBxgRMTMn.uQtbwUTxZcYfEiBzeIiRojdCQeHP.FVGMuRfCeZZjKXFaZLuJYUrOHJsf) == xfediqeVkGuRhquuqrQKopjIqqoC._actionId;
				}
			}

			private sealed class iAJcwVUHPLCCEwKPioYugsEvDsaO
			{
				public List<gIBPcMQrDhIZFlpFWCJzmmnjiNkm> QdHiCvjazdIoZuhyqTfpPwOBJfdkA;

				public pNiBgndhirjKwSeKSnElGpWIhUIab XMUzFFrImhxwVWDkYlneFXwUGrfA;

				internal int opTRgjuasYTDaDbAJzJEtbGybLrbA(ControllerMap_Editor P_0, IList<ControllerMap_Editor> P_1)
				{
					tEFFKprtchbPYcPjOITwthSnugmO tEFFKprtchbPYcPjOITwthSnugmO2 = new tEFFKprtchbPYcPjOITwthSnugmO();
					tEFFKprtchbPYcPjOITwthSnugmO2.NYEWYjzZeafHjeszDLDydacgeZkB = P_0;
					for (int i = 0; i < P_1.Count; i++)
					{
						gIBPcMQrDhIZFlpFWCJzmmnjiNkm gIBPcMQrDhIZFlpFWCJzmmnjiNkm2 = XMUzFFrImhxwVWDkYlneFXwUGrfA.azPMbUiryIgrSfKcUyTkLdttmScO.Find(tEFFKprtchbPYcPjOITwthSnugmO2.jwMfwKcOnRfqWqgJuChIDnSVcTzw);
						gIBPcMQrDhIZFlpFWCJzmmnjiNkm gIBPcMQrDhIZFlpFWCJzmmnjiNkm3 = QdHiCvjazdIoZuhyqTfpPwOBJfdkA.Find(tEFFKprtchbPYcPjOITwthSnugmO2.TGFxLMQGdABGGFBXzNmNCMsEIBLjA);
						if (gIBPcMQrDhIZFlpFWCJzmmnjiNkm2 != null && gIBPcMQrDhIZFlpFWCJzmmnjiNkm2.qrhejhOprjbEagkMvfpSxTcPHfNtA == P_1[i].categoryId && gIBPcMQrDhIZFlpFWCJzmmnjiNkm3 != null && gIBPcMQrDhIZFlpFWCJzmmnjiNkm3.qrhejhOprjbEagkMvfpSxTcPHfNtA == P_1[i].layoutId)
						{
							return i;
						}
					}
					return -1;
				}

				internal ControllerMap_Editor xnhvtSjGQqiDYxCxRQXzqmliMJpW(okMYBPiHOMuwixSfVPWSIlunhHYl<ControllerMap_Editor> P_0)
				{
					DxqTCiFMjcVVooFcLbMoGdXkugErA dxqTCiFMjcVVooFcLbMoGdXkugErA = new DxqTCiFMjcVVooFcLbMoGdXkugErA();
					dxqTCiFMjcVVooFcLbMoGdXkugErA.XqhbhosMGUZltXLOpTdJlFksqYlR = P_0;
					dxqTCiFMjcVVooFcLbMoGdXkugErA.KMNzFpVehzaJLQUPemVNTBGCMDI = JsonTools.Clone(dxqTCiFMjcVVooFcLbMoGdXkugErA.XqhbhosMGUZltXLOpTdJlFksqYlR.FVLGSdhpzjAimcwGFLVvMxxwOUGq);
					gIBPcMQrDhIZFlpFWCJzmmnjiNkm gIBPcMQrDhIZFlpFWCJzmmnjiNkm2 = XMUzFFrImhxwVWDkYlneFXwUGrfA.azPMbUiryIgrSfKcUyTkLdttmScO.Find(dxqTCiFMjcVVooFcLbMoGdXkugErA.zCyNvowytxTqnGuSjqfJclVfSmSe);
					gIBPcMQrDhIZFlpFWCJzmmnjiNkm gIBPcMQrDhIZFlpFWCJzmmnjiNkm3 = QdHiCvjazdIoZuhyqTfpPwOBJfdkA.Find(dxqTCiFMjcVVooFcLbMoGdXkugErA.IImtVPOOYLbfdgkwpxwZzUjcIIAp);
					dxqTCiFMjcVVooFcLbMoGdXkugErA.KMNzFpVehzaJLQUPemVNTBGCMDI.categoryId = gIBPcMQrDhIZFlpFWCJzmmnjiNkm2?.qrhejhOprjbEagkMvfpSxTcPHfNtA ?? (-1);
					dxqTCiFMjcVVooFcLbMoGdXkugErA.KMNzFpVehzaJLQUPemVNTBGCMDI.layoutId = gIBPcMQrDhIZFlpFWCJzmmnjiNkm3?.qrhejhOprjbEagkMvfpSxTcPHfNtA ?? (-1);
					for (int i = 0; i < dxqTCiFMjcVVooFcLbMoGdXkugErA.KMNzFpVehzaJLQUPemVNTBGCMDI.actionElementMaps.Count; i++)
					{
						DaESorNpmnrJRsrlDZEMQSADRSAb daESorNpmnrJRsrlDZEMQSADRSAb = new DaESorNpmnrJRsrlDZEMQSADRSAb();
						daESorNpmnrJRsrlDZEMQSADRSAb.aJAcGFtWtEAYUlmREVqIRmaAtSDD = dxqTCiFMjcVVooFcLbMoGdXkugErA;
						daESorNpmnrJRsrlDZEMQSADRSAb.aYcCuDiodXlYPfhSkcrxZSGKGXXjB = daESorNpmnrJRsrlDZEMQSADRSAb.aJAcGFtWtEAYUlmREVqIRmaAtSDD.KMNzFpVehzaJLQUPemVNTBGCMDI.actionElementMaps[i];
						gIBPcMQrDhIZFlpFWCJzmmnjiNkm gIBPcMQrDhIZFlpFWCJzmmnjiNkm4 = XMUzFFrImhxwVWDkYlneFXwUGrfA.OqpaNlVZdIOZBWSzcdKBDFxyiLEN.Find(daESorNpmnrJRsrlDZEMQSADRSAb.rfpkSgUccJzsNORxYWgQnIZwTBnB);
						daESorNpmnrJRsrlDZEMQSADRSAb.aYcCuDiodXlYPfhSkcrxZSGKGXXjB._actionId = gIBPcMQrDhIZFlpFWCJzmmnjiNkm4?.qrhejhOprjbEagkMvfpSxTcPHfNtA ?? (-1);
						daESorNpmnrJRsrlDZEMQSADRSAb.aYcCuDiodXlYPfhSkcrxZSGKGXXjB._actionCategoryId = ((XMUzFFrImhxwVWDkYlneFXwUGrfA.VGcgcOsNmgmQWYkAXDCxgeYloFfeA.GetActionById(daESorNpmnrJRsrlDZEMQSADRSAb.aYcCuDiodXlYPfhSkcrxZSGKGXXjB._actionId) != null) ? XMUzFFrImhxwVWDkYlneFXwUGrfA.VGcgcOsNmgmQWYkAXDCxgeYloFfeA.GetActionById(daESorNpmnrJRsrlDZEMQSADRSAb.aYcCuDiodXlYPfhSkcrxZSGKGXXjB._actionId).categoryId : 0);
					}
					ControllerMap_Editor controllerMap_Editor;
					if (dxqTCiFMjcVVooFcLbMoGdXkugErA.XqhbhosMGUZltXLOpTdJlFksqYlR.fnkkZlyPXUXkbxepljPcfxGtgcTu)
					{
						controllerMap_Editor = dxqTCiFMjcVVooFcLbMoGdXkugErA.XqhbhosMGUZltXLOpTdJlFksqYlR.jFUdDvYgaHwuUQnjbfbCcgVkDAZFb;
						ControllerMap_Editor controllerMap_Editor2 = JsonTools.Clone(dxqTCiFMjcVVooFcLbMoGdXkugErA.KMNzFpVehzaJLQUPemVNTBGCMDI);
						controllerMap_Editor2.actionElementMaps.Clear();
						Func<ActionElementMap, IList<ActionElementMap>, int> func = mseacdDYvvCxiPOCKMxdHSAWgPDjb._003C_003E9.bUVwysfzlZLDWIzMTSyluxqJnRzF;
						ocQEtlXNkJtMEVpcDsjDtyoJEAQQ(controllerMap_Editor.actionElementMaps, dxqTCiFMjcVVooFcLbMoGdXkugErA.KMNzFpVehzaJLQUPemVNTBGCMDI.actionElementMaps, controllerMap_Editor2.actionElementMaps, func);
						dxqTCiFMjcVVooFcLbMoGdXkugErA.KMNzFpVehzaJLQUPemVNTBGCMDI = controllerMap_Editor2;
					}
					else
					{
						XMUzFFrImhxwVWDkYlneFXwUGrfA.VGcgcOsNmgmQWYkAXDCxgeYloFfeA.CreateMouseMap(dxqTCiFMjcVVooFcLbMoGdXkugErA.KMNzFpVehzaJLQUPemVNTBGCMDI.categoryId, dxqTCiFMjcVVooFcLbMoGdXkugErA.KMNzFpVehzaJLQUPemVNTBGCMDI.layoutId);
						controllerMap_Editor = dxqTCiFMjcVVooFcLbMoGdXkugErA.XqhbhosMGUZltXLOpTdJlFksqYlR.zDJfCWfvzOnJQTofBEEdeXUQjwkF[dxqTCiFMjcVVooFcLbMoGdXkugErA.XqhbhosMGUZltXLOpTdJlFksqYlR.zDJfCWfvzOnJQTofBEEdeXUQjwkF.Count - 1];
					}
					dxqTCiFMjcVVooFcLbMoGdXkugErA.KMNzFpVehzaJLQUPemVNTBGCMDI.id = controllerMap_Editor.id;
					int index = dxqTCiFMjcVVooFcLbMoGdXkugErA.XqhbhosMGUZltXLOpTdJlFksqYlR.zDJfCWfvzOnJQTofBEEdeXUQjwkF.IndexOf(controllerMap_Editor);
					dxqTCiFMjcVVooFcLbMoGdXkugErA.XqhbhosMGUZltXLOpTdJlFksqYlR.zDJfCWfvzOnJQTofBEEdeXUQjwkF[index] = dxqTCiFMjcVVooFcLbMoGdXkugErA.KMNzFpVehzaJLQUPemVNTBGCMDI;
					return dxqTCiFMjcVVooFcLbMoGdXkugErA.KMNzFpVehzaJLQUPemVNTBGCMDI;
				}
			}

			private sealed class tEFFKprtchbPYcPjOITwthSnugmO
			{
				public ControllerMap_Editor NYEWYjzZeafHjeszDLDydacgeZkB;

				public Predicate<gIBPcMQrDhIZFlpFWCJzmmnjiNkm> nbnDqBmiynDZHMyfkMcmXPkosbUN;

				public Predicate<gIBPcMQrDhIZFlpFWCJzmmnjiNkm> FKbrxDWwugCOakJHuHVByXZADitIA;

				internal bool jwMfwKcOnRfqWqgJuChIDnSVcTzw(gIBPcMQrDhIZFlpFWCJzmmnjiNkm P_0)
				{
					return P_0.prNJUrqsciTAXxyaCYjaYFJMFovV == NYEWYjzZeafHjeszDLDydacgeZkB.categoryId;
				}

				internal bool TGFxLMQGdABGGFBXzNmNCMsEIBLjA(gIBPcMQrDhIZFlpFWCJzmmnjiNkm P_0)
				{
					return P_0.prNJUrqsciTAXxyaCYjaYFJMFovV == NYEWYjzZeafHjeszDLDydacgeZkB.layoutId;
				}
			}

			private sealed class DxqTCiFMjcVVooFcLbMoGdXkugErA
			{
				public okMYBPiHOMuwixSfVPWSIlunhHYl<ControllerMap_Editor> XqhbhosMGUZltXLOpTdJlFksqYlR;

				public ControllerMap_Editor KMNzFpVehzaJLQUPemVNTBGCMDI;

				internal bool zCyNvowytxTqnGuSjqfJclVfSmSe(gIBPcMQrDhIZFlpFWCJzmmnjiNkm P_0)
				{
					return P_0.fDoSYUFwxFeTVLhMrbgEBYlYhFJu(XqhbhosMGUZltXLOpTdJlFksqYlR.FVGMuRfCeZZjKXFaZLuJYUrOHJsf) == KMNzFpVehzaJLQUPemVNTBGCMDI.categoryId;
				}

				internal bool IImtVPOOYLbfdgkwpxwZzUjcIIAp(gIBPcMQrDhIZFlpFWCJzmmnjiNkm P_0)
				{
					return P_0.fDoSYUFwxFeTVLhMrbgEBYlYhFJu(XqhbhosMGUZltXLOpTdJlFksqYlR.FVGMuRfCeZZjKXFaZLuJYUrOHJsf) == KMNzFpVehzaJLQUPemVNTBGCMDI.layoutId;
				}
			}

			private sealed class DaESorNpmnrJRsrlDZEMQSADRSAb
			{
				public ActionElementMap aYcCuDiodXlYPfhSkcrxZSGKGXXjB;

				public DxqTCiFMjcVVooFcLbMoGdXkugErA aJAcGFtWtEAYUlmREVqIRmaAtSDD;

				internal bool rfpkSgUccJzsNORxYWgQnIZwTBnB(gIBPcMQrDhIZFlpFWCJzmmnjiNkm P_0)
				{
					return P_0.fDoSYUFwxFeTVLhMrbgEBYlYhFJu(aJAcGFtWtEAYUlmREVqIRmaAtSDD.XqhbhosMGUZltXLOpTdJlFksqYlR.FVGMuRfCeZZjKXFaZLuJYUrOHJsf) == aYcCuDiodXlYPfhSkcrxZSGKGXXjB._actionId;
				}
			}

			private sealed class ZsgxPgtkWjRxEebkYffttNfTrinM
			{
				public List<gIBPcMQrDhIZFlpFWCJzmmnjiNkm> kIYRWVlBRNAYxafRZnVnXQuIsMxj;

				public pNiBgndhirjKwSeKSnElGpWIhUIab fhmoMIKOeSpntIGSAReXdPjUIGbx;

				internal int BNBeTPfPvGcVWFkCjzbsZqSqKQjbc(ControllerMap_Editor P_0, IList<ControllerMap_Editor> P_1)
				{
					IJvwFsjgMRgZhORlQTqzzZOCHUhN jvwFsjgMRgZhORlQTqzzZOCHUhN = new IJvwFsjgMRgZhORlQTqzzZOCHUhN();
					jvwFsjgMRgZhORlQTqzzZOCHUhN.ICSPbLjFNjIgsJobNoASradcJGvb = P_0;
					for (int i = 0; i < P_1.Count; i++)
					{
						gIBPcMQrDhIZFlpFWCJzmmnjiNkm gIBPcMQrDhIZFlpFWCJzmmnjiNkm2 = fhmoMIKOeSpntIGSAReXdPjUIGbx.azPMbUiryIgrSfKcUyTkLdttmScO.Find(jvwFsjgMRgZhORlQTqzzZOCHUhN.oIkKNqDGJpNkBDigNmzFCdjZBTnJA);
						gIBPcMQrDhIZFlpFWCJzmmnjiNkm gIBPcMQrDhIZFlpFWCJzmmnjiNkm3 = kIYRWVlBRNAYxafRZnVnXQuIsMxj.Find(jvwFsjgMRgZhORlQTqzzZOCHUhN.IhrhGVUcGoxdwWZhLCOjJCokqJnU);
						if (jvwFsjgMRgZhORlQTqzzZOCHUhN.ICSPbLjFNjIgsJobNoASradcJGvb.hardwareGuid == P_1[i].hardwareGuid && gIBPcMQrDhIZFlpFWCJzmmnjiNkm2 != null && gIBPcMQrDhIZFlpFWCJzmmnjiNkm2.qrhejhOprjbEagkMvfpSxTcPHfNtA == P_1[i].categoryId && gIBPcMQrDhIZFlpFWCJzmmnjiNkm3 != null && gIBPcMQrDhIZFlpFWCJzmmnjiNkm3.qrhejhOprjbEagkMvfpSxTcPHfNtA == P_1[i].layoutId)
						{
							return i;
						}
					}
					return -1;
				}

				internal ControllerMap_Editor xPnFHLjvXnUglXzCAvirewNZKDWN(okMYBPiHOMuwixSfVPWSIlunhHYl<ControllerMap_Editor> P_0)
				{
					OKaJEfYFwBOnNtDDIbnACJdWfVlV oKaJEfYFwBOnNtDDIbnACJdWfVlV = new OKaJEfYFwBOnNtDDIbnACJdWfVlV();
					oKaJEfYFwBOnNtDDIbnACJdWfVlV.jkhXHIZzbihnNOlncwKVVnasSZpk = P_0;
					oKaJEfYFwBOnNtDDIbnACJdWfVlV.QkqIyIEdzytyUaLjVsrUlAwRwQHQ = JsonTools.Clone(oKaJEfYFwBOnNtDDIbnACJdWfVlV.jkhXHIZzbihnNOlncwKVVnasSZpk.FVLGSdhpzjAimcwGFLVvMxxwOUGq);
					gIBPcMQrDhIZFlpFWCJzmmnjiNkm gIBPcMQrDhIZFlpFWCJzmmnjiNkm2 = fhmoMIKOeSpntIGSAReXdPjUIGbx.azPMbUiryIgrSfKcUyTkLdttmScO.Find(oKaJEfYFwBOnNtDDIbnACJdWfVlV.bAewklOiqPPicTHcyMKKqGtqZHrI);
					gIBPcMQrDhIZFlpFWCJzmmnjiNkm gIBPcMQrDhIZFlpFWCJzmmnjiNkm3 = kIYRWVlBRNAYxafRZnVnXQuIsMxj.Find(oKaJEfYFwBOnNtDDIbnACJdWfVlV.bCZMiIGdUbPBmnHZTDDYwvJJUdcv);
					oKaJEfYFwBOnNtDDIbnACJdWfVlV.QkqIyIEdzytyUaLjVsrUlAwRwQHQ.categoryId = gIBPcMQrDhIZFlpFWCJzmmnjiNkm2?.qrhejhOprjbEagkMvfpSxTcPHfNtA ?? (-1);
					oKaJEfYFwBOnNtDDIbnACJdWfVlV.QkqIyIEdzytyUaLjVsrUlAwRwQHQ.layoutId = gIBPcMQrDhIZFlpFWCJzmmnjiNkm3?.qrhejhOprjbEagkMvfpSxTcPHfNtA ?? (-1);
					for (int i = 0; i < oKaJEfYFwBOnNtDDIbnACJdWfVlV.QkqIyIEdzytyUaLjVsrUlAwRwQHQ.actionElementMaps.Count; i++)
					{
						OBhtvXdlyZqFgTzcubUmhFthNcHm oBhtvXdlyZqFgTzcubUmhFthNcHm = new OBhtvXdlyZqFgTzcubUmhFthNcHm();
						oBhtvXdlyZqFgTzcubUmhFthNcHm.HkyRPpoXeAyoEjdyGuVHtSxBZRpH = oKaJEfYFwBOnNtDDIbnACJdWfVlV;
						oBhtvXdlyZqFgTzcubUmhFthNcHm.QxdrXdPdhAWkewnjoeuoThlqYcNn = oBhtvXdlyZqFgTzcubUmhFthNcHm.HkyRPpoXeAyoEjdyGuVHtSxBZRpH.QkqIyIEdzytyUaLjVsrUlAwRwQHQ.actionElementMaps[i];
						gIBPcMQrDhIZFlpFWCJzmmnjiNkm gIBPcMQrDhIZFlpFWCJzmmnjiNkm4 = fhmoMIKOeSpntIGSAReXdPjUIGbx.OqpaNlVZdIOZBWSzcdKBDFxyiLEN.Find(oBhtvXdlyZqFgTzcubUmhFthNcHm.RLtHgomLyKqgZukKYqmiLyMPtXDw);
						oBhtvXdlyZqFgTzcubUmhFthNcHm.QxdrXdPdhAWkewnjoeuoThlqYcNn._actionId = gIBPcMQrDhIZFlpFWCJzmmnjiNkm4?.qrhejhOprjbEagkMvfpSxTcPHfNtA ?? (-1);
						oBhtvXdlyZqFgTzcubUmhFthNcHm.QxdrXdPdhAWkewnjoeuoThlqYcNn._actionCategoryId = ((fhmoMIKOeSpntIGSAReXdPjUIGbx.VGcgcOsNmgmQWYkAXDCxgeYloFfeA.GetActionById(oBhtvXdlyZqFgTzcubUmhFthNcHm.QxdrXdPdhAWkewnjoeuoThlqYcNn._actionId) != null) ? fhmoMIKOeSpntIGSAReXdPjUIGbx.VGcgcOsNmgmQWYkAXDCxgeYloFfeA.GetActionById(oBhtvXdlyZqFgTzcubUmhFthNcHm.QxdrXdPdhAWkewnjoeuoThlqYcNn._actionId).categoryId : 0);
					}
					ControllerMap_Editor controllerMap_Editor;
					if (oKaJEfYFwBOnNtDDIbnACJdWfVlV.jkhXHIZzbihnNOlncwKVVnasSZpk.fnkkZlyPXUXkbxepljPcfxGtgcTu)
					{
						controllerMap_Editor = oKaJEfYFwBOnNtDDIbnACJdWfVlV.jkhXHIZzbihnNOlncwKVVnasSZpk.jFUdDvYgaHwuUQnjbfbCcgVkDAZFb;
						ControllerMap_Editor controllerMap_Editor2 = JsonTools.Clone(oKaJEfYFwBOnNtDDIbnACJdWfVlV.QkqIyIEdzytyUaLjVsrUlAwRwQHQ);
						controllerMap_Editor2.actionElementMaps.Clear();
						Func<ActionElementMap, IList<ActionElementMap>, int> func = mseacdDYvvCxiPOCKMxdHSAWgPDjb._003C_003E9.MqchHpYahnmGfxfYPAWNHkMfaBvcA;
						ocQEtlXNkJtMEVpcDsjDtyoJEAQQ(controllerMap_Editor.actionElementMaps, oKaJEfYFwBOnNtDDIbnACJdWfVlV.QkqIyIEdzytyUaLjVsrUlAwRwQHQ.actionElementMaps, controllerMap_Editor2.actionElementMaps, func);
						oKaJEfYFwBOnNtDDIbnACJdWfVlV.QkqIyIEdzytyUaLjVsrUlAwRwQHQ = controllerMap_Editor2;
					}
					else
					{
						fhmoMIKOeSpntIGSAReXdPjUIGbx.VGcgcOsNmgmQWYkAXDCxgeYloFfeA.CreateJoystickMap(oKaJEfYFwBOnNtDDIbnACJdWfVlV.QkqIyIEdzytyUaLjVsrUlAwRwQHQ.categoryId, oKaJEfYFwBOnNtDDIbnACJdWfVlV.QkqIyIEdzytyUaLjVsrUlAwRwQHQ.hardwareGuid, oKaJEfYFwBOnNtDDIbnACJdWfVlV.QkqIyIEdzytyUaLjVsrUlAwRwQHQ.layoutId);
						controllerMap_Editor = oKaJEfYFwBOnNtDDIbnACJdWfVlV.jkhXHIZzbihnNOlncwKVVnasSZpk.zDJfCWfvzOnJQTofBEEdeXUQjwkF[oKaJEfYFwBOnNtDDIbnACJdWfVlV.jkhXHIZzbihnNOlncwKVVnasSZpk.zDJfCWfvzOnJQTofBEEdeXUQjwkF.Count - 1];
					}
					oKaJEfYFwBOnNtDDIbnACJdWfVlV.QkqIyIEdzytyUaLjVsrUlAwRwQHQ.id = controllerMap_Editor.id;
					int index = oKaJEfYFwBOnNtDDIbnACJdWfVlV.jkhXHIZzbihnNOlncwKVVnasSZpk.zDJfCWfvzOnJQTofBEEdeXUQjwkF.IndexOf(controllerMap_Editor);
					oKaJEfYFwBOnNtDDIbnACJdWfVlV.jkhXHIZzbihnNOlncwKVVnasSZpk.zDJfCWfvzOnJQTofBEEdeXUQjwkF[index] = oKaJEfYFwBOnNtDDIbnACJdWfVlV.QkqIyIEdzytyUaLjVsrUlAwRwQHQ;
					return oKaJEfYFwBOnNtDDIbnACJdWfVlV.QkqIyIEdzytyUaLjVsrUlAwRwQHQ;
				}
			}

			private sealed class IJvwFsjgMRgZhORlQTqzzZOCHUhN
			{
				public ControllerMap_Editor ICSPbLjFNjIgsJobNoASradcJGvb;

				public Predicate<gIBPcMQrDhIZFlpFWCJzmmnjiNkm> UCypWoXLRrcqiXxzIFEkLWWimnpy;

				public Predicate<gIBPcMQrDhIZFlpFWCJzmmnjiNkm> MJEHrWygkjTvbIbhctfdGshjvvHu;

				internal bool oIkKNqDGJpNkBDigNmzFCdjZBTnJA(gIBPcMQrDhIZFlpFWCJzmmnjiNkm P_0)
				{
					return P_0.prNJUrqsciTAXxyaCYjaYFJMFovV == ICSPbLjFNjIgsJobNoASradcJGvb.categoryId;
				}

				internal bool IhrhGVUcGoxdwWZhLCOjJCokqJnU(gIBPcMQrDhIZFlpFWCJzmmnjiNkm P_0)
				{
					return P_0.prNJUrqsciTAXxyaCYjaYFJMFovV == ICSPbLjFNjIgsJobNoASradcJGvb.layoutId;
				}
			}

			private sealed class OKaJEfYFwBOnNtDDIbnACJdWfVlV
			{
				public okMYBPiHOMuwixSfVPWSIlunhHYl<ControllerMap_Editor> jkhXHIZzbihnNOlncwKVVnasSZpk;

				public ControllerMap_Editor QkqIyIEdzytyUaLjVsrUlAwRwQHQ;

				internal bool bAewklOiqPPicTHcyMKKqGtqZHrI(gIBPcMQrDhIZFlpFWCJzmmnjiNkm P_0)
				{
					return P_0.fDoSYUFwxFeTVLhMrbgEBYlYhFJu(jkhXHIZzbihnNOlncwKVVnasSZpk.FVGMuRfCeZZjKXFaZLuJYUrOHJsf) == QkqIyIEdzytyUaLjVsrUlAwRwQHQ.categoryId;
				}

				internal bool bCZMiIGdUbPBmnHZTDDYwvJJUdcv(gIBPcMQrDhIZFlpFWCJzmmnjiNkm P_0)
				{
					return P_0.fDoSYUFwxFeTVLhMrbgEBYlYhFJu(jkhXHIZzbihnNOlncwKVVnasSZpk.FVGMuRfCeZZjKXFaZLuJYUrOHJsf) == QkqIyIEdzytyUaLjVsrUlAwRwQHQ.layoutId;
				}
			}

			private sealed class OBhtvXdlyZqFgTzcubUmhFthNcHm
			{
				public ActionElementMap QxdrXdPdhAWkewnjoeuoThlqYcNn;

				public OKaJEfYFwBOnNtDDIbnACJdWfVlV HkyRPpoXeAyoEjdyGuVHtSxBZRpH;

				internal bool RLtHgomLyKqgZukKYqmiLyMPtXDw(gIBPcMQrDhIZFlpFWCJzmmnjiNkm P_0)
				{
					return P_0.fDoSYUFwxFeTVLhMrbgEBYlYhFJu(HkyRPpoXeAyoEjdyGuVHtSxBZRpH.jkhXHIZzbihnNOlncwKVVnasSZpk.FVGMuRfCeZZjKXFaZLuJYUrOHJsf) == QxdrXdPdhAWkewnjoeuoThlqYcNn._actionId;
				}
			}

			private sealed class YtOfKVamCAAaHPAKThYrexnnzyHuA
			{
				public List<gIBPcMQrDhIZFlpFWCJzmmnjiNkm> OBThBKcvdhrWizrcaXIFvBGvOuss;

				public pNiBgndhirjKwSeKSnElGpWIhUIab arHSQdTQnQFqjkNBMWlFuEBAvrYvA;

				internal int vLMFTfkUtsrfWhlQgBzpwvgNwBBf(ControllerMap_Editor P_0, IList<ControllerMap_Editor> P_1)
				{
					cPAdVohmBSZGkcFpfeddBIzfDBNwb cPAdVohmBSZGkcFpfeddBIzfDBNwb2 = new cPAdVohmBSZGkcFpfeddBIzfDBNwb();
					cPAdVohmBSZGkcFpfeddBIzfDBNwb2.QNhXBYlNSuLGBKPKSBRifeVIIdhs = P_0;
					for (int i = 0; i < P_1.Count; i++)
					{
						gIBPcMQrDhIZFlpFWCJzmmnjiNkm gIBPcMQrDhIZFlpFWCJzmmnjiNkm2 = arHSQdTQnQFqjkNBMWlFuEBAvrYvA.BxvShEVaGesKdYRqoIRFhVDAsfCI.Find(cPAdVohmBSZGkcFpfeddBIzfDBNwb2.SBseQmheQiGLXKdxGWNWaoQJgCWk);
						gIBPcMQrDhIZFlpFWCJzmmnjiNkm gIBPcMQrDhIZFlpFWCJzmmnjiNkm3 = arHSQdTQnQFqjkNBMWlFuEBAvrYvA.azPMbUiryIgrSfKcUyTkLdttmScO.Find(cPAdVohmBSZGkcFpfeddBIzfDBNwb2.iHJOZPFlslGWYTfgkxKNwJZmxFmx);
						gIBPcMQrDhIZFlpFWCJzmmnjiNkm gIBPcMQrDhIZFlpFWCJzmmnjiNkm4 = OBThBKcvdhrWizrcaXIFvBGvOuss.Find(cPAdVohmBSZGkcFpfeddBIzfDBNwb2.dEZsjkLkFLjVIAmQbGgzbSAEUuVNb);
						if (gIBPcMQrDhIZFlpFWCJzmmnjiNkm2 != null && gIBPcMQrDhIZFlpFWCJzmmnjiNkm2.qrhejhOprjbEagkMvfpSxTcPHfNtA == P_1[i].customControllerUid && gIBPcMQrDhIZFlpFWCJzmmnjiNkm3 != null && gIBPcMQrDhIZFlpFWCJzmmnjiNkm3.qrhejhOprjbEagkMvfpSxTcPHfNtA == P_1[i].categoryId && gIBPcMQrDhIZFlpFWCJzmmnjiNkm4 != null && gIBPcMQrDhIZFlpFWCJzmmnjiNkm4.qrhejhOprjbEagkMvfpSxTcPHfNtA == P_1[i].layoutId)
						{
							return i;
						}
					}
					return -1;
				}

				internal ControllerMap_Editor TOeboXpAWrlNTbGnNmemSBOsegaP(okMYBPiHOMuwixSfVPWSIlunhHYl<ControllerMap_Editor> P_0)
				{
					RECFEsgsTkPfwDVXaVcVSEjlKRDVB rECFEsgsTkPfwDVXaVcVSEjlKRDVB = new RECFEsgsTkPfwDVXaVcVSEjlKRDVB();
					rECFEsgsTkPfwDVXaVcVSEjlKRDVB.nipOnoddgtUfrPUTpgKtudHqdSZeA = P_0;
					rECFEsgsTkPfwDVXaVcVSEjlKRDVB.gGqNRZAxyCiuWyLleQVBDtQXfSdx = JsonTools.Clone(rECFEsgsTkPfwDVXaVcVSEjlKRDVB.nipOnoddgtUfrPUTpgKtudHqdSZeA.FVLGSdhpzjAimcwGFLVvMxxwOUGq);
					gIBPcMQrDhIZFlpFWCJzmmnjiNkm gIBPcMQrDhIZFlpFWCJzmmnjiNkm2 = arHSQdTQnQFqjkNBMWlFuEBAvrYvA.BxvShEVaGesKdYRqoIRFhVDAsfCI.Find(rECFEsgsTkPfwDVXaVcVSEjlKRDVB.rKiERGtCrQBpLJrrLdsVYHfoRRsRA);
					gIBPcMQrDhIZFlpFWCJzmmnjiNkm gIBPcMQrDhIZFlpFWCJzmmnjiNkm3 = arHSQdTQnQFqjkNBMWlFuEBAvrYvA.azPMbUiryIgrSfKcUyTkLdttmScO.Find(rECFEsgsTkPfwDVXaVcVSEjlKRDVB.beGmQJORUEGAGbrmUWNUxEFrstwc);
					gIBPcMQrDhIZFlpFWCJzmmnjiNkm gIBPcMQrDhIZFlpFWCJzmmnjiNkm4 = OBThBKcvdhrWizrcaXIFvBGvOuss.Find(rECFEsgsTkPfwDVXaVcVSEjlKRDVB.tRRqIxYwEAhGIAgMomQSMcpvNgfw);
					rECFEsgsTkPfwDVXaVcVSEjlKRDVB.gGqNRZAxyCiuWyLleQVBDtQXfSdx.customControllerUid = gIBPcMQrDhIZFlpFWCJzmmnjiNkm2?.qrhejhOprjbEagkMvfpSxTcPHfNtA ?? (-1);
					rECFEsgsTkPfwDVXaVcVSEjlKRDVB.gGqNRZAxyCiuWyLleQVBDtQXfSdx.categoryId = gIBPcMQrDhIZFlpFWCJzmmnjiNkm3?.qrhejhOprjbEagkMvfpSxTcPHfNtA ?? (-1);
					rECFEsgsTkPfwDVXaVcVSEjlKRDVB.gGqNRZAxyCiuWyLleQVBDtQXfSdx.layoutId = gIBPcMQrDhIZFlpFWCJzmmnjiNkm4?.qrhejhOprjbEagkMvfpSxTcPHfNtA ?? (-1);
					for (int i = 0; i < rECFEsgsTkPfwDVXaVcVSEjlKRDVB.gGqNRZAxyCiuWyLleQVBDtQXfSdx.actionElementMaps.Count; i++)
					{
						CZlnySAYdijHnlTKrpeUDQOpMcsk cZlnySAYdijHnlTKrpeUDQOpMcsk = new CZlnySAYdijHnlTKrpeUDQOpMcsk();
						cZlnySAYdijHnlTKrpeUDQOpMcsk.FJcLiQNnnYnBXCwIxPCgudObLxRT = rECFEsgsTkPfwDVXaVcVSEjlKRDVB;
						cZlnySAYdijHnlTKrpeUDQOpMcsk.wEjBScuHBwBXrCSMHzJfRTEpiKwhb = cZlnySAYdijHnlTKrpeUDQOpMcsk.FJcLiQNnnYnBXCwIxPCgudObLxRT.gGqNRZAxyCiuWyLleQVBDtQXfSdx.actionElementMaps[i];
						gIBPcMQrDhIZFlpFWCJzmmnjiNkm gIBPcMQrDhIZFlpFWCJzmmnjiNkm5 = arHSQdTQnQFqjkNBMWlFuEBAvrYvA.OqpaNlVZdIOZBWSzcdKBDFxyiLEN.Find(cZlnySAYdijHnlTKrpeUDQOpMcsk.WdjcIgKezXnWwpJFDdFwnzvIQpEW);
						cZlnySAYdijHnlTKrpeUDQOpMcsk.wEjBScuHBwBXrCSMHzJfRTEpiKwhb._actionId = gIBPcMQrDhIZFlpFWCJzmmnjiNkm5?.qrhejhOprjbEagkMvfpSxTcPHfNtA ?? (-1);
						cZlnySAYdijHnlTKrpeUDQOpMcsk.wEjBScuHBwBXrCSMHzJfRTEpiKwhb._actionCategoryId = ((arHSQdTQnQFqjkNBMWlFuEBAvrYvA.VGcgcOsNmgmQWYkAXDCxgeYloFfeA.GetActionById(cZlnySAYdijHnlTKrpeUDQOpMcsk.wEjBScuHBwBXrCSMHzJfRTEpiKwhb._actionId) != null) ? arHSQdTQnQFqjkNBMWlFuEBAvrYvA.VGcgcOsNmgmQWYkAXDCxgeYloFfeA.GetActionById(cZlnySAYdijHnlTKrpeUDQOpMcsk.wEjBScuHBwBXrCSMHzJfRTEpiKwhb._actionId).categoryId : 0);
					}
					ControllerMap_Editor controllerMap_Editor;
					if (rECFEsgsTkPfwDVXaVcVSEjlKRDVB.nipOnoddgtUfrPUTpgKtudHqdSZeA.fnkkZlyPXUXkbxepljPcfxGtgcTu)
					{
						controllerMap_Editor = rECFEsgsTkPfwDVXaVcVSEjlKRDVB.nipOnoddgtUfrPUTpgKtudHqdSZeA.jFUdDvYgaHwuUQnjbfbCcgVkDAZFb;
						ControllerMap_Editor controllerMap_Editor2 = JsonTools.Clone(rECFEsgsTkPfwDVXaVcVSEjlKRDVB.gGqNRZAxyCiuWyLleQVBDtQXfSdx);
						controllerMap_Editor2.actionElementMaps.Clear();
						Func<ActionElementMap, IList<ActionElementMap>, int> func = mseacdDYvvCxiPOCKMxdHSAWgPDjb._003C_003E9.SkFlARcQIgRQPZAMumQZrdaMORgu;
						ocQEtlXNkJtMEVpcDsjDtyoJEAQQ(controllerMap_Editor.actionElementMaps, rECFEsgsTkPfwDVXaVcVSEjlKRDVB.gGqNRZAxyCiuWyLleQVBDtQXfSdx.actionElementMaps, controllerMap_Editor2.actionElementMaps, func);
						rECFEsgsTkPfwDVXaVcVSEjlKRDVB.gGqNRZAxyCiuWyLleQVBDtQXfSdx = controllerMap_Editor2;
					}
					else
					{
						arHSQdTQnQFqjkNBMWlFuEBAvrYvA.VGcgcOsNmgmQWYkAXDCxgeYloFfeA.CreateCustomControllerMap(rECFEsgsTkPfwDVXaVcVSEjlKRDVB.gGqNRZAxyCiuWyLleQVBDtQXfSdx.categoryId, rECFEsgsTkPfwDVXaVcVSEjlKRDVB.gGqNRZAxyCiuWyLleQVBDtQXfSdx.customControllerUid, rECFEsgsTkPfwDVXaVcVSEjlKRDVB.gGqNRZAxyCiuWyLleQVBDtQXfSdx.layoutId);
						controllerMap_Editor = rECFEsgsTkPfwDVXaVcVSEjlKRDVB.nipOnoddgtUfrPUTpgKtudHqdSZeA.zDJfCWfvzOnJQTofBEEdeXUQjwkF[rECFEsgsTkPfwDVXaVcVSEjlKRDVB.nipOnoddgtUfrPUTpgKtudHqdSZeA.zDJfCWfvzOnJQTofBEEdeXUQjwkF.Count - 1];
					}
					rECFEsgsTkPfwDVXaVcVSEjlKRDVB.gGqNRZAxyCiuWyLleQVBDtQXfSdx.id = controllerMap_Editor.id;
					int index = rECFEsgsTkPfwDVXaVcVSEjlKRDVB.nipOnoddgtUfrPUTpgKtudHqdSZeA.zDJfCWfvzOnJQTofBEEdeXUQjwkF.IndexOf(controllerMap_Editor);
					rECFEsgsTkPfwDVXaVcVSEjlKRDVB.nipOnoddgtUfrPUTpgKtudHqdSZeA.zDJfCWfvzOnJQTofBEEdeXUQjwkF[index] = rECFEsgsTkPfwDVXaVcVSEjlKRDVB.gGqNRZAxyCiuWyLleQVBDtQXfSdx;
					return rECFEsgsTkPfwDVXaVcVSEjlKRDVB.gGqNRZAxyCiuWyLleQVBDtQXfSdx;
				}
			}

			private sealed class cvzXGSeNFTlHYpdcRsQZqWSbyroi
			{
				public int MmdXpvTIikEFzijNzsyAmZxNxCwV;

				internal bool rGeYrnWmPYUvwPVmQSUyFqksIyxv(gIBPcMQrDhIZFlpFWCJzmmnjiNkm P_0)
				{
					return P_0.prNJUrqsciTAXxyaCYjaYFJMFovV == MmdXpvTIikEFzijNzsyAmZxNxCwV;
				}
			}

			private sealed class cPAdVohmBSZGkcFpfeddBIzfDBNwb
			{
				public ControllerMap_Editor QNhXBYlNSuLGBKPKSBRifeVIIdhs;

				public Predicate<gIBPcMQrDhIZFlpFWCJzmmnjiNkm> yxaMmGxBzRDEKBVjaRbSPXNqYWtqA;

				public Predicate<gIBPcMQrDhIZFlpFWCJzmmnjiNkm> yDXwcXLGjSphqigChDRROwblElWN;

				public Predicate<gIBPcMQrDhIZFlpFWCJzmmnjiNkm> cbFoHVGSnbojuYICXdQYoMbzDHyN;

				internal bool SBseQmheQiGLXKdxGWNWaoQJgCWk(gIBPcMQrDhIZFlpFWCJzmmnjiNkm P_0)
				{
					return P_0.prNJUrqsciTAXxyaCYjaYFJMFovV == QNhXBYlNSuLGBKPKSBRifeVIIdhs.customControllerUid;
				}

				internal bool iHJOZPFlslGWYTfgkxKNwJZmxFmx(gIBPcMQrDhIZFlpFWCJzmmnjiNkm P_0)
				{
					return P_0.prNJUrqsciTAXxyaCYjaYFJMFovV == QNhXBYlNSuLGBKPKSBRifeVIIdhs.categoryId;
				}

				internal bool dEZsjkLkFLjVIAmQbGgzbSAEUuVNb(gIBPcMQrDhIZFlpFWCJzmmnjiNkm P_0)
				{
					return P_0.prNJUrqsciTAXxyaCYjaYFJMFovV == QNhXBYlNSuLGBKPKSBRifeVIIdhs.layoutId;
				}
			}

			private sealed class RECFEsgsTkPfwDVXaVcVSEjlKRDVB
			{
				public okMYBPiHOMuwixSfVPWSIlunhHYl<ControllerMap_Editor> nipOnoddgtUfrPUTpgKtudHqdSZeA;

				public ControllerMap_Editor gGqNRZAxyCiuWyLleQVBDtQXfSdx;

				internal bool rKiERGtCrQBpLJrrLdsVYHfoRRsRA(gIBPcMQrDhIZFlpFWCJzmmnjiNkm P_0)
				{
					return P_0.fDoSYUFwxFeTVLhMrbgEBYlYhFJu(nipOnoddgtUfrPUTpgKtudHqdSZeA.FVGMuRfCeZZjKXFaZLuJYUrOHJsf) == gGqNRZAxyCiuWyLleQVBDtQXfSdx.customControllerUid;
				}

				internal bool beGmQJORUEGAGbrmUWNUxEFrstwc(gIBPcMQrDhIZFlpFWCJzmmnjiNkm P_0)
				{
					return P_0.fDoSYUFwxFeTVLhMrbgEBYlYhFJu(nipOnoddgtUfrPUTpgKtudHqdSZeA.FVGMuRfCeZZjKXFaZLuJYUrOHJsf) == gGqNRZAxyCiuWyLleQVBDtQXfSdx.categoryId;
				}

				internal bool tRRqIxYwEAhGIAgMomQSMcpvNgfw(gIBPcMQrDhIZFlpFWCJzmmnjiNkm P_0)
				{
					return P_0.fDoSYUFwxFeTVLhMrbgEBYlYhFJu(nipOnoddgtUfrPUTpgKtudHqdSZeA.FVGMuRfCeZZjKXFaZLuJYUrOHJsf) == gGqNRZAxyCiuWyLleQVBDtQXfSdx.layoutId;
				}
			}

			private sealed class CZlnySAYdijHnlTKrpeUDQOpMcsk
			{
				public ActionElementMap wEjBScuHBwBXrCSMHzJfRTEpiKwhb;

				public RECFEsgsTkPfwDVXaVcVSEjlKRDVB FJcLiQNnnYnBXCwIxPCgudObLxRT;

				internal bool WdjcIgKezXnWwpJFDdFwnzvIQpEW(gIBPcMQrDhIZFlpFWCJzmmnjiNkm P_0)
				{
					return P_0.fDoSYUFwxFeTVLhMrbgEBYlYhFJu(FJcLiQNnnYnBXCwIxPCgudObLxRT.nipOnoddgtUfrPUTpgKtudHqdSZeA.FVGMuRfCeZZjKXFaZLuJYUrOHJsf) == wEjBScuHBwBXrCSMHzJfRTEpiKwhb._actionId;
				}
			}

			private sealed class qQpSvXhNBgGxofhpEHBDHBhVNpDeA
			{
				public okMYBPiHOMuwixSfVPWSIlunhHYl<ControllerMapLayoutManager_RuleSet_Editor> iTbDfsACSWETYaMsHzcweptwtqHl;
			}

			private sealed class TBKafMpJfADXDfXfTzXvQxYKvsmdA
			{
				public int pHZcVhiKGTWzuZXluzKXjatFzHTN;

				public qQpSvXhNBgGxofhpEHBDHBhVNpDeA RPUKOejNyKqSkEttguUvJyNuPHkQ;

				internal bool jIzeQHrbepXlNawdbmhLsIbzVAVf(gIBPcMQrDhIZFlpFWCJzmmnjiNkm P_0)
				{
					return P_0.fDoSYUFwxFeTVLhMrbgEBYlYhFJu(RPUKOejNyKqSkEttguUvJyNuPHkQ.iTbDfsACSWETYaMsHzcweptwtqHl.FVGMuRfCeZZjKXFaZLuJYUrOHJsf) == pHZcVhiKGTWzuZXluzKXjatFzHTN;
				}
			}

			private sealed class rIAiUiGFnhBrJchvEInIczDFhWrub
			{
				public int jsdpSSHZRljSeokqEGytoPlSNoao;

				public qQpSvXhNBgGxofhpEHBDHBhVNpDeA uoeKFjJjjeDDVGBrcMDVuOcTLgnrA;

				internal bool SWRzkCHKKDshiNTraeCWymGVJcfE(gIBPcMQrDhIZFlpFWCJzmmnjiNkm P_0)
				{
					return P_0.fDoSYUFwxFeTVLhMrbgEBYlYhFJu(uoeKFjJjjeDDVGBrcMDVuOcTLgnrA.iTbDfsACSWETYaMsHzcweptwtqHl.FVGMuRfCeZZjKXFaZLuJYUrOHJsf) == jsdpSSHZRljSeokqEGytoPlSNoao;
				}
			}

			private sealed class sLBumGEeVkgJyvPbSVypppqVnEF
			{
				public int QSZBaxcymNsUzzrMcoZZQwUsCAqK;

				public qQpSvXhNBgGxofhpEHBDHBhVNpDeA ipyQPxYKrQoJFADpcZeafcBQLTvC;

				internal bool zwFzDXtYWhtcJrlSsWKvgLSGtLCC(gIBPcMQrDhIZFlpFWCJzmmnjiNkm P_0)
				{
					return P_0.fDoSYUFwxFeTVLhMrbgEBYlYhFJu(ipyQPxYKrQoJFADpcZeafcBQLTvC.iTbDfsACSWETYaMsHzcweptwtqHl.FVGMuRfCeZZjKXFaZLuJYUrOHJsf) == QSZBaxcymNsUzzrMcoZZQwUsCAqK;
				}
			}

			private sealed class zkVtzBreatSOLZVthBSiPMQyNCed
			{
				public okMYBPiHOMuwixSfVPWSIlunhHYl<ControllerMapEnabler_RuleSet_Editor> SHjefFUwuWFlXlQfxXVBfKKACWpcA;
			}

			private sealed class XrSaileFHJMnoSvTZHUuFoFtgStgA
			{
				public int HEPasztPgfQDRNvIzGCwgnBVoRCI;

				public zkVtzBreatSOLZVthBSiPMQyNCed oFcOyoPVrhegIPCpcFEiFbnqyodIA;

				internal bool pLQYgmZCFGTkJHBqIyZQGnyQJfnn(gIBPcMQrDhIZFlpFWCJzmmnjiNkm P_0)
				{
					return P_0.fDoSYUFwxFeTVLhMrbgEBYlYhFJu(oFcOyoPVrhegIPCpcFEiFbnqyodIA.SHjefFUwuWFlXlQfxXVBfKKACWpcA.FVGMuRfCeZZjKXFaZLuJYUrOHJsf) == HEPasztPgfQDRNvIzGCwgnBVoRCI;
				}
			}

			private sealed class WkNCorgsikNbZadwjwtpMBMeuvdYb<_0001> where _0001 : class
			{
				public Func<_0001, int> YIbWYydlQVbHzkrumbaUWDpggktp;
			}

			private sealed class cudFAuMjaallCOdELXEJEqApMBxX<_0001> where _0001 : class
			{
				public _0001 mWbZUWszAZQIjLGRbrLeHOXjzbOR;

				public WkNCorgsikNbZadwjwtpMBMeuvdYb<_0001> XJXFtQxgcchtxBLQvUvZEaCsJKWLA;

				internal bool PFIhgYeIYNHcjadOQgbsIdwGnkPIA(gIBPcMQrDhIZFlpFWCJzmmnjiNkm P_0)
				{
					return P_0.qrhejhOprjbEagkMvfpSxTcPHfNtA == XJXFtQxgcchtxBLQvUvZEaCsJKWLA.YIbWYydlQVbHzkrumbaUWDpggktp(mWbZUWszAZQIjLGRbrLeHOXjzbOR);
				}
			}

			public static UserData LpThxjFUnKCEcAfYJbELHJdqpLnQA(UserData P_0, UserData P_1, bool P_2)
			{
				pNiBgndhirjKwSeKSnElGpWIhUIab pNiBgndhirjKwSeKSnElGpWIhUIab2 = new pNiBgndhirjKwSeKSnElGpWIhUIab();
				if (P_0 == null)
				{
					throw new ArgumentNullException("orig");
				}
				P_0 = JsonTools.Clone(P_0);
				P_1 = ((P_1 != null) ? JsonTools.Clone(P_1) : null);
				pNiBgndhirjKwSeKSnElGpWIhUIab2.VGcgcOsNmgmQWYkAXDCxgeYloFfeA = (P_2 ? P_0 : new UserData(false));
				if (P_1 != null)
				{
					pNiBgndhirjKwSeKSnElGpWIhUIab2.VGcgcOsNmgmQWYkAXDCxgeYloFfeA.configVars = JsonTools.Clone(P_1.configVars);
				}
				else
				{
					pNiBgndhirjKwSeKSnElGpWIhUIab2.VGcgcOsNmgmQWYkAXDCxgeYloFfeA.configVars = JsonTools.Clone(P_0.configVars);
				}
				pNiBgndhirjKwSeKSnElGpWIhUIab2.tndXldzonxrGYSzWdGzTJlymunSv = new List<gIBPcMQrDhIZFlpFWCJzmmnjiNkm>();
				sqLpXADcKZsSREiPXrmaqMgDeGWh("Action Category", P_0.actionCategories, P_1?.actionCategories, pNiBgndhirjKwSeKSnElGpWIhUIab2.VGcgcOsNmgmQWYkAXDCxgeYloFfeA.actionCategories, P_2, pNiBgndhirjKwSeKSnElGpWIhUIab2.tndXldzonxrGYSzWdGzTJlymunSv, mseacdDYvvCxiPOCKMxdHSAWgPDjb._003C_003E9.pljvUMdUmPapsfSagwcZAvFhCKch, mseacdDYvvCxiPOCKMxdHSAWgPDjb._003C_003E9.IRuaKnINFChelfQORLDidLkGHneeB, mseacdDYvvCxiPOCKMxdHSAWgPDjb._003C_003E9.vuvyHAJbotopEJpAFJfuTTUmeYcr, pNiBgndhirjKwSeKSnElGpWIhUIab2.wfHjMVgYIfypLGMeGPidbzPmPmUdA);
				pNiBgndhirjKwSeKSnElGpWIhUIab2.XMjXmMjdlZnoWCAabkaNjtdKAnKK = new List<gIBPcMQrDhIZFlpFWCJzmmnjiNkm>();
				sqLpXADcKZsSREiPXrmaqMgDeGWh("Input Behavior", P_0.inputBehaviors, P_1?.inputBehaviors, pNiBgndhirjKwSeKSnElGpWIhUIab2.VGcgcOsNmgmQWYkAXDCxgeYloFfeA.inputBehaviors, P_2, pNiBgndhirjKwSeKSnElGpWIhUIab2.XMjXmMjdlZnoWCAabkaNjtdKAnKK, mseacdDYvvCxiPOCKMxdHSAWgPDjb._003C_003E9.zUNbdEABPbANVrJoHLfByBhHigcgb, mseacdDYvvCxiPOCKMxdHSAWgPDjb._003C_003E9.TmEflHfHsjhHfUnvmaLbAMvcLsxAb, mseacdDYvvCxiPOCKMxdHSAWgPDjb._003C_003E9.QgeSVYFECRfVIGpKYuWxHIosqrEu, pNiBgndhirjKwSeKSnElGpWIhUIab2.szAynGrVdWiHhlpAeUoicFFqiBlcA);
				pNiBgndhirjKwSeKSnElGpWIhUIab2.OqpaNlVZdIOZBWSzcdKBDFxyiLEN = new List<gIBPcMQrDhIZFlpFWCJzmmnjiNkm>();
				sqLpXADcKZsSREiPXrmaqMgDeGWh("Action", P_0.kKnhPVRioLmZoBOgUQuQYtoEHyTc, P_1?.kKnhPVRioLmZoBOgUQuQYtoEHyTc, pNiBgndhirjKwSeKSnElGpWIhUIab2.VGcgcOsNmgmQWYkAXDCxgeYloFfeA.kKnhPVRioLmZoBOgUQuQYtoEHyTc, P_2, pNiBgndhirjKwSeKSnElGpWIhUIab2.OqpaNlVZdIOZBWSzcdKBDFxyiLEN, mseacdDYvvCxiPOCKMxdHSAWgPDjb._003C_003E9.aPTjTWzIbLpyjDXDfXIocYrSKtJb, mseacdDYvvCxiPOCKMxdHSAWgPDjb._003C_003E9.TrAGOSMDdcAVjdnSUXysAnIBPzJU, mseacdDYvvCxiPOCKMxdHSAWgPDjb._003C_003E9.MNPUsEZmXxGLbOayTUofJzPAXPfA, pNiBgndhirjKwSeKSnElGpWIhUIab2.PzsSKglgkffUaMPtctPJYXTXTFQA);
				pNiBgndhirjKwSeKSnElGpWIhUIab2.azPMbUiryIgrSfKcUyTkLdttmScO = new List<gIBPcMQrDhIZFlpFWCJzmmnjiNkm>();
				FNZQtWhPTEXepFRXjiFvckODlNxg fNZQtWhPTEXepFRXjiFvckODlNxg = new FNZQtWhPTEXepFRXjiFvckODlNxg();
				fNZQtWhPTEXepFRXjiFvckODlNxg.NyOxYkUooNNAOACadHszkFFsiouFA = pNiBgndhirjKwSeKSnElGpWIhUIab2;
				fNZQtWhPTEXepFRXjiFvckODlNxg.YsbuNOyZhjlUoBWcKePWdghpoXlo = new List<int>();
				sqLpXADcKZsSREiPXrmaqMgDeGWh("Map Category", P_0.mapCategories, P_1?.mapCategories, fNZQtWhPTEXepFRXjiFvckODlNxg.NyOxYkUooNNAOACadHszkFFsiouFA.VGcgcOsNmgmQWYkAXDCxgeYloFfeA.mapCategories, P_2, fNZQtWhPTEXepFRXjiFvckODlNxg.NyOxYkUooNNAOACadHszkFFsiouFA.azPMbUiryIgrSfKcUyTkLdttmScO, mseacdDYvvCxiPOCKMxdHSAWgPDjb._003C_003E9.bGtEHkepcZYHUjLQEEMThJOjDJdTb, mseacdDYvvCxiPOCKMxdHSAWgPDjb._003C_003E9.jUAcRPMEJMdEoaNztKGtIhJoWMpo, mseacdDYvvCxiPOCKMxdHSAWgPDjb._003C_003E9.clYsPVqGRhLzUfzCXbBsMymXlkaw, fNZQtWhPTEXepFRXjiFvckODlNxg.rJmGzOinXwDyIadpDlqyhrMMRVakb);
				for (int i = 0; i < fNZQtWhPTEXepFRXjiFvckODlNxg.YsbuNOyZhjlUoBWcKePWdghpoXlo.Count; i++)
				{
					int index = fNZQtWhPTEXepFRXjiFvckODlNxg.YsbuNOyZhjlUoBWcKePWdghpoXlo[i];
					InputMapCategory inputMapCategory = fNZQtWhPTEXepFRXjiFvckODlNxg.NyOxYkUooNNAOACadHszkFFsiouFA.VGcgcOsNmgmQWYkAXDCxgeYloFfeA.mapCategories[index];
					for (int j = 0; j < inputMapCategory.GyuVOtOtNvaOqGMXwzdKrYardgfo.Count; j++)
					{
						cvzXGSeNFTlHYpdcRsQZqWSbyroi cvzXGSeNFTlHYpdcRsQZqWSbyroi2 = new cvzXGSeNFTlHYpdcRsQZqWSbyroi();
						cvzXGSeNFTlHYpdcRsQZqWSbyroi2.MmdXpvTIikEFzijNzsyAmZxNxCwV = inputMapCategory.GyuVOtOtNvaOqGMXwzdKrYardgfo[j];
						gIBPcMQrDhIZFlpFWCJzmmnjiNkm gIBPcMQrDhIZFlpFWCJzmmnjiNkm2 = fNZQtWhPTEXepFRXjiFvckODlNxg.NyOxYkUooNNAOACadHszkFFsiouFA.azPMbUiryIgrSfKcUyTkLdttmScO.Find(cvzXGSeNFTlHYpdcRsQZqWSbyroi2.rGeYrnWmPYUvwPVmQSUyFqksIyxv);
						inputMapCategory.GyuVOtOtNvaOqGMXwzdKrYardgfo[j] = gIBPcMQrDhIZFlpFWCJzmmnjiNkm2?.qrhejhOprjbEagkMvfpSxTcPHfNtA ?? (-1);
					}
				}
				pNiBgndhirjKwSeKSnElGpWIhUIab2.WrBLrEQoisYZPIWzDfJYdiITgZtfA = new List<gIBPcMQrDhIZFlpFWCJzmmnjiNkm>();
				sqLpXADcKZsSREiPXrmaqMgDeGWh("Keyboard Layout", P_0.keyboardLayouts, P_1?.keyboardLayouts, pNiBgndhirjKwSeKSnElGpWIhUIab2.VGcgcOsNmgmQWYkAXDCxgeYloFfeA.keyboardLayouts, P_2, pNiBgndhirjKwSeKSnElGpWIhUIab2.WrBLrEQoisYZPIWzDfJYdiITgZtfA, mseacdDYvvCxiPOCKMxdHSAWgPDjb._003C_003E9.MHnLrLIKQlNvkPGpodVmHgBBgkBu, mseacdDYvvCxiPOCKMxdHSAWgPDjb._003C_003E9.YczICcKSwkVhNuXjWYtzfOHFnxjEA, mseacdDYvvCxiPOCKMxdHSAWgPDjb._003C_003E9.FSpCRzAQxyxcZaPHxipUPStcJEHkA, pNiBgndhirjKwSeKSnElGpWIhUIab2.gNjFLIwNtEbrjKtPSKfFJhCxyvOdA);
				pNiBgndhirjKwSeKSnElGpWIhUIab2.zSRKGqTMdaxenOqwPohNjSGvKuZS = new List<gIBPcMQrDhIZFlpFWCJzmmnjiNkm>();
				sqLpXADcKZsSREiPXrmaqMgDeGWh("Mouse Layout", P_0.mouseLayouts, P_1?.mouseLayouts, pNiBgndhirjKwSeKSnElGpWIhUIab2.VGcgcOsNmgmQWYkAXDCxgeYloFfeA.mouseLayouts, P_2, pNiBgndhirjKwSeKSnElGpWIhUIab2.zSRKGqTMdaxenOqwPohNjSGvKuZS, mseacdDYvvCxiPOCKMxdHSAWgPDjb._003C_003E9.guAvSPRfphGCgPLwsGhjQjCxGvve, mseacdDYvvCxiPOCKMxdHSAWgPDjb._003C_003E9.sUvSMAHoSGYeeqDxBuBjvYkfcTHm, mseacdDYvvCxiPOCKMxdHSAWgPDjb._003C_003E9.EfSHMcfyctQsNSkrrMlqQCIbamaCb, pNiBgndhirjKwSeKSnElGpWIhUIab2.YrWkHdSpdVoBbGENfFibsuJnBfXp);
				pNiBgndhirjKwSeKSnElGpWIhUIab2.nYtbnZrMSQbsMDwMuxJhWhyPAGMl = new List<gIBPcMQrDhIZFlpFWCJzmmnjiNkm>();
				sqLpXADcKZsSREiPXrmaqMgDeGWh("Joystick Layout", P_0.joystickLayouts, P_1?.joystickLayouts, pNiBgndhirjKwSeKSnElGpWIhUIab2.VGcgcOsNmgmQWYkAXDCxgeYloFfeA.joystickLayouts, P_2, pNiBgndhirjKwSeKSnElGpWIhUIab2.nYtbnZrMSQbsMDwMuxJhWhyPAGMl, mseacdDYvvCxiPOCKMxdHSAWgPDjb._003C_003E9.boMLvzramTvEUmwhNDHAogxQPIXe, mseacdDYvvCxiPOCKMxdHSAWgPDjb._003C_003E9.lAismnfqlasnUDKSICpRsNybEYCq, mseacdDYvvCxiPOCKMxdHSAWgPDjb._003C_003E9.MRJEiSKzwZmCOvnYNIyPirXgAVbAb, pNiBgndhirjKwSeKSnElGpWIhUIab2.WscaCCDhlxzyojdGftnMLhDmIzii);
				pNiBgndhirjKwSeKSnElGpWIhUIab2.LkDUmDwrUoTLTkfJQEsCqAtIFFUHA = new List<gIBPcMQrDhIZFlpFWCJzmmnjiNkm>();
				sqLpXADcKZsSREiPXrmaqMgDeGWh("Custom Controller Layout", P_0.customControllerLayouts, P_1?.customControllerLayouts, pNiBgndhirjKwSeKSnElGpWIhUIab2.VGcgcOsNmgmQWYkAXDCxgeYloFfeA.customControllerLayouts, P_2, pNiBgndhirjKwSeKSnElGpWIhUIab2.LkDUmDwrUoTLTkfJQEsCqAtIFFUHA, mseacdDYvvCxiPOCKMxdHSAWgPDjb._003C_003E9.ZzjwerfKGxNcfuMBKjeNuBsAcWvd, mseacdDYvvCxiPOCKMxdHSAWgPDjb._003C_003E9.hDqSwtYKuRURCLFmpAEXaWuTIMXM, mseacdDYvvCxiPOCKMxdHSAWgPDjb._003C_003E9.jGXMnfgLJzqGuvGxJlFDFdqzCuHGA, pNiBgndhirjKwSeKSnElGpWIhUIab2.qUfxPEbYfCnckWBnNGWJNkjecxOx);
				pNiBgndhirjKwSeKSnElGpWIhUIab2.CbJHWLTvKsRFOifIrUgGvisjgImcA = pNiBgndhirjKwSeKSnElGpWIhUIab2.wyuEzRDcbLdKAYQpvBysOATpxSpf;
				pNiBgndhirjKwSeKSnElGpWIhUIab2.BxvShEVaGesKdYRqoIRFhVDAsfCI = new List<gIBPcMQrDhIZFlpFWCJzmmnjiNkm>();
				sqLpXADcKZsSREiPXrmaqMgDeGWh("Custom Controller", P_0.customControllers, P_1?.customControllers, pNiBgndhirjKwSeKSnElGpWIhUIab2.VGcgcOsNmgmQWYkAXDCxgeYloFfeA.customControllers, P_2, pNiBgndhirjKwSeKSnElGpWIhUIab2.BxvShEVaGesKdYRqoIRFhVDAsfCI, mseacdDYvvCxiPOCKMxdHSAWgPDjb._003C_003E9.jWsBKFEJVjqsqEStjrrNclkFiiYZB, mseacdDYvvCxiPOCKMxdHSAWgPDjb._003C_003E9.cVwtJlXtEYulyeItkSLgPqkJfBpFA, mseacdDYvvCxiPOCKMxdHSAWgPDjb._003C_003E9.jgCYvqcbDTODUNVewqZPxIZTumnJ, pNiBgndhirjKwSeKSnElGpWIhUIab2.uDZaYfuBcZjZKVWSLwFRmbERUMzT);
				pNiBgndhirjKwSeKSnElGpWIhUIab2.fNeYIWEKThcTUdEIrtoRcwIQYAwJ = new List<gIBPcMQrDhIZFlpFWCJzmmnjiNkm>();
				sqLpXADcKZsSREiPXrmaqMgDeGWh("Layout Manager Set", P_0.controllerMapLayoutManagerRuleSets, P_1?.controllerMapLayoutManagerRuleSets, pNiBgndhirjKwSeKSnElGpWIhUIab2.VGcgcOsNmgmQWYkAXDCxgeYloFfeA.controllerMapLayoutManagerRuleSets, P_2, pNiBgndhirjKwSeKSnElGpWIhUIab2.fNeYIWEKThcTUdEIrtoRcwIQYAwJ, mseacdDYvvCxiPOCKMxdHSAWgPDjb._003C_003E9.GxpuZfULUGLBIzCisavYJMtBepFn, mseacdDYvvCxiPOCKMxdHSAWgPDjb._003C_003E9.qQhjNNEDfurDjWleKhAVgKwaWUyhA, mseacdDYvvCxiPOCKMxdHSAWgPDjb._003C_003E9.EumQjyOqYfbOhIDBXodDDkKaIkoFc, pNiBgndhirjKwSeKSnElGpWIhUIab2.oXjcAMDcoOBbNxrxXyYESyFEFRVqA);
				pNiBgndhirjKwSeKSnElGpWIhUIab2.NwxqNrKQGJvHyVDFwgMmaoSsZjNeA = new List<gIBPcMQrDhIZFlpFWCJzmmnjiNkm>();
				sqLpXADcKZsSREiPXrmaqMgDeGWh("Controller Map Enabler Set", P_0.controllerMapEnablerRuleSets, P_1?.controllerMapEnablerRuleSets, pNiBgndhirjKwSeKSnElGpWIhUIab2.VGcgcOsNmgmQWYkAXDCxgeYloFfeA.controllerMapEnablerRuleSets, P_2, pNiBgndhirjKwSeKSnElGpWIhUIab2.NwxqNrKQGJvHyVDFwgMmaoSsZjNeA, mseacdDYvvCxiPOCKMxdHSAWgPDjb._003C_003E9.LBgfmxkwMZWCkOOSfNiPISwMaTHJA, mseacdDYvvCxiPOCKMxdHSAWgPDjb._003C_003E9.tUEHTohUNypeeUWDQFwwWPgqpAVw, mseacdDYvvCxiPOCKMxdHSAWgPDjb._003C_003E9.aHbniMPeQNTnrFtnRAWxMyBNacGK, pNiBgndhirjKwSeKSnElGpWIhUIab2.UbuGHgmGdSilTgRoUChhxXirpguuA);
				List<gIBPcMQrDhIZFlpFWCJzmmnjiNkm> list = new List<gIBPcMQrDhIZFlpFWCJzmmnjiNkm>();
				sqLpXADcKZsSREiPXrmaqMgDeGWh("Player", P_0.players, P_1?.players, pNiBgndhirjKwSeKSnElGpWIhUIab2.VGcgcOsNmgmQWYkAXDCxgeYloFfeA.players, P_2, list, mseacdDYvvCxiPOCKMxdHSAWgPDjb._003C_003E9.skKaMTcGPfGSDuAxZGtJBneNwyhRA, mseacdDYvvCxiPOCKMxdHSAWgPDjb._003C_003E9.IxVxQylvDADCjtkLZMSEjBbIAgRe, mseacdDYvvCxiPOCKMxdHSAWgPDjb._003C_003E9.RtwtCAAQKSHAJRIhZpksGhmcbAZp, pNiBgndhirjKwSeKSnElGpWIhUIab2.vNzsOWDlUIepQcVnJpnwpXnLGOSd);
				List<gIBPcMQrDhIZFlpFWCJzmmnjiNkm> list2 = new List<gIBPcMQrDhIZFlpFWCJzmmnjiNkm>();
				RJqUxBmpHyZkjWyIPWZQNVPkaIhi rJqUxBmpHyZkjWyIPWZQNVPkaIhi = new RJqUxBmpHyZkjWyIPWZQNVPkaIhi();
				rJqUxBmpHyZkjWyIPWZQNVPkaIhi.TYkYTCiKvRnYFFzxKmwMbzumHSED = pNiBgndhirjKwSeKSnElGpWIhUIab2;
				rJqUxBmpHyZkjWyIPWZQNVPkaIhi.EoSFXfCXLtZOdQWLAAtrDdhMjJvpA = rJqUxBmpHyZkjWyIPWZQNVPkaIhi.TYkYTCiKvRnYFFzxKmwMbzumHSED.WrBLrEQoisYZPIWzDfJYdiITgZtfA;
				sqLpXADcKZsSREiPXrmaqMgDeGWh("Keyboard Map", P_0.keyboardMaps, P_1?.keyboardMaps, rJqUxBmpHyZkjWyIPWZQNVPkaIhi.TYkYTCiKvRnYFFzxKmwMbzumHSED.VGcgcOsNmgmQWYkAXDCxgeYloFfeA.keyboardMaps, P_2, list2, mseacdDYvvCxiPOCKMxdHSAWgPDjb._003C_003E9.YEQJBFquHblPQEcFwVDDhHzPgGmZ, mseacdDYvvCxiPOCKMxdHSAWgPDjb._003C_003E9.wvNbpaeNdfVaeNnuhPCzOkOSbvaOA, rJqUxBmpHyZkjWyIPWZQNVPkaIhi.eXOEOiliMFsQVbJeSawSdbtsvJqQ, rJqUxBmpHyZkjWyIPWZQNVPkaIhi.qauxrNSTcduxzDxLBAXXkgQrhnBV);
				List<gIBPcMQrDhIZFlpFWCJzmmnjiNkm> list3 = new List<gIBPcMQrDhIZFlpFWCJzmmnjiNkm>();
				iAJcwVUHPLCCEwKPioYugsEvDsaO iAJcwVUHPLCCEwKPioYugsEvDsaO2 = new iAJcwVUHPLCCEwKPioYugsEvDsaO();
				iAJcwVUHPLCCEwKPioYugsEvDsaO2.XMUzFFrImhxwVWDkYlneFXwUGrfA = pNiBgndhirjKwSeKSnElGpWIhUIab2;
				iAJcwVUHPLCCEwKPioYugsEvDsaO2.QdHiCvjazdIoZuhyqTfpPwOBJfdkA = iAJcwVUHPLCCEwKPioYugsEvDsaO2.XMUzFFrImhxwVWDkYlneFXwUGrfA.zSRKGqTMdaxenOqwPohNjSGvKuZS;
				sqLpXADcKZsSREiPXrmaqMgDeGWh("Mouse Map", P_0.mouseMaps, P_1?.mouseMaps, iAJcwVUHPLCCEwKPioYugsEvDsaO2.XMUzFFrImhxwVWDkYlneFXwUGrfA.VGcgcOsNmgmQWYkAXDCxgeYloFfeA.mouseMaps, P_2, list3, mseacdDYvvCxiPOCKMxdHSAWgPDjb._003C_003E9.onpLFvxKmwGshwHYpAinEOtxMneV, mseacdDYvvCxiPOCKMxdHSAWgPDjb._003C_003E9.NKBhTWYvvMejnueEIqfWAYMhLUnf, iAJcwVUHPLCCEwKPioYugsEvDsaO2.opTRgjuasYTDaDbAJzJEtbGybLrbA, iAJcwVUHPLCCEwKPioYugsEvDsaO2.xnhvtSjGQqiDYxCxRQXzqmliMJpW);
				List<gIBPcMQrDhIZFlpFWCJzmmnjiNkm> list4 = new List<gIBPcMQrDhIZFlpFWCJzmmnjiNkm>();
				ZsgxPgtkWjRxEebkYffttNfTrinM zsgxPgtkWjRxEebkYffttNfTrinM = new ZsgxPgtkWjRxEebkYffttNfTrinM();
				zsgxPgtkWjRxEebkYffttNfTrinM.fhmoMIKOeSpntIGSAReXdPjUIGbx = pNiBgndhirjKwSeKSnElGpWIhUIab2;
				zsgxPgtkWjRxEebkYffttNfTrinM.kIYRWVlBRNAYxafRZnVnXQuIsMxj = zsgxPgtkWjRxEebkYffttNfTrinM.fhmoMIKOeSpntIGSAReXdPjUIGbx.nYtbnZrMSQbsMDwMuxJhWhyPAGMl;
				sqLpXADcKZsSREiPXrmaqMgDeGWh("Joystick Map", P_0.joystickMaps, P_1?.joystickMaps, zsgxPgtkWjRxEebkYffttNfTrinM.fhmoMIKOeSpntIGSAReXdPjUIGbx.VGcgcOsNmgmQWYkAXDCxgeYloFfeA.joystickMaps, P_2, list4, mseacdDYvvCxiPOCKMxdHSAWgPDjb._003C_003E9.yEzjCAvwPjkKWfSSBOkMGEPqqFgU, mseacdDYvvCxiPOCKMxdHSAWgPDjb._003C_003E9.dIDQYTNgaCosJrVExLeTJleGJPVt, zsgxPgtkWjRxEebkYffttNfTrinM.BNBeTPfPvGcVWFkCjzbsZqSqKQjbc, zsgxPgtkWjRxEebkYffttNfTrinM.xPnFHLjvXnUglXzCAvirewNZKDWN);
				List<gIBPcMQrDhIZFlpFWCJzmmnjiNkm> list5 = new List<gIBPcMQrDhIZFlpFWCJzmmnjiNkm>();
				YtOfKVamCAAaHPAKThYrexnnzyHuA ytOfKVamCAAaHPAKThYrexnnzyHuA = new YtOfKVamCAAaHPAKThYrexnnzyHuA();
				ytOfKVamCAAaHPAKThYrexnnzyHuA.arHSQdTQnQFqjkNBMWlFuEBAvrYvA = pNiBgndhirjKwSeKSnElGpWIhUIab2;
				ytOfKVamCAAaHPAKThYrexnnzyHuA.OBThBKcvdhrWizrcaXIFvBGvOuss = ytOfKVamCAAaHPAKThYrexnnzyHuA.arHSQdTQnQFqjkNBMWlFuEBAvrYvA.LkDUmDwrUoTLTkfJQEsCqAtIFFUHA;
				sqLpXADcKZsSREiPXrmaqMgDeGWh("Custom Controller Map", P_0.customControllerMaps, P_1?.customControllerMaps, ytOfKVamCAAaHPAKThYrexnnzyHuA.arHSQdTQnQFqjkNBMWlFuEBAvrYvA.VGcgcOsNmgmQWYkAXDCxgeYloFfeA.customControllerMaps, P_2, list5, mseacdDYvvCxiPOCKMxdHSAWgPDjb._003C_003E9.NxRvsrWmSEtlEGHdIUvDhYhplcCc, mseacdDYvvCxiPOCKMxdHSAWgPDjb._003C_003E9.xzTsdYAAPgeIniCqOktrinqXbfSx, ytOfKVamCAAaHPAKThYrexnnzyHuA.vLMFTfkUtsrfWhlQgBzpwvgNwBBf, ytOfKVamCAAaHPAKThYrexnnzyHuA.TOeboXpAWrlNTbGnNmemSBOsegaP);
				return pNiBgndhirjKwSeKSnElGpWIhUIab2.VGcgcOsNmgmQWYkAXDCxgeYloFfeA;
			}

			[Conditional("DEBUG_IMPORT")]
			private static void WjGvuRFxmZbdhrDRxBFhshYqPbaQ(object P_0)
			{
				Logger.Log("[DEBUG_IMPORT] " + P_0);
			}

			private static void ocQEtlXNkJtMEVpcDsjDtyoJEAQQ<_0001>(IList<_0001> P_0, IList<_0001> P_1, IList<_0001> P_2, Func<_0001, IList<_0001>, int> P_3)
			{
				for (int i = 0; i < P_0.Count; i++)
				{
					P_2.Add(P_0[i]);
				}
				if (P_1 == null)
				{
					return;
				}
				for (int j = 0; j < P_1.Count; j++)
				{
					_0001 val = P_1[j];
					int num = P_3(val, P_2);
					if (num >= 0)
					{
						P_2[num] = val;
					}
					else
					{
						P_2.Add(val);
					}
				}
			}

			private static void sqLpXADcKZsSREiPXrmaqMgDeGWh<_0001>(string P_0, IList<_0001> P_1, IList<_0001> P_2, IList<_0001> P_3, bool P_4, List<gIBPcMQrDhIZFlpFWCJzmmnjiNkm> P_5, Func<_0001, int> P_6, Func<_0001, string> P_7, Func<_0001, IList<_0001>, int> P_8, Func<okMYBPiHOMuwixSfVPWSIlunhHYl<_0001>, _0001> P_9) where _0001 : class
			{
				WkNCorgsikNbZadwjwtpMBMeuvdYb<_0001> wkNCorgsikNbZadwjwtpMBMeuvdYb = new WkNCorgsikNbZadwjwtpMBMeuvdYb<_0001>();
				wkNCorgsikNbZadwjwtpMBMeuvdYb.YIbWYydlQVbHzkrumbaUWDpggktp = P_6;
				for (int i = 0; i < P_1.Count; i++)
				{
					_0001 val = P_1[i];
					if (P_4)
					{
						P_5.Add(new gIBPcMQrDhIZFlpFWCJzmmnjiNkm(wkNCorgsikNbZadwjwtpMBMeuvdYb.YIbWYydlQVbHzkrumbaUWDpggktp(val), -1, wkNCorgsikNbZadwjwtpMBMeuvdYb.YIbWYydlQVbHzkrumbaUWDpggktp(val)));
						continue;
					}
					_0001 arg = P_9(new okMYBPiHOMuwixSfVPWSIlunhHYl<_0001>(val, null, gIBPcMQrDhIZFlpFWCJzmmnjiNkm.CsysAjVUMGvYOWqTKclZPrJCrsOx.origId, P_3, false));
					P_5.Add(new gIBPcMQrDhIZFlpFWCJzmmnjiNkm(wkNCorgsikNbZadwjwtpMBMeuvdYb.YIbWYydlQVbHzkrumbaUWDpggktp(val), -1, wkNCorgsikNbZadwjwtpMBMeuvdYb.YIbWYydlQVbHzkrumbaUWDpggktp(arg)));
				}
				if (P_2 == null)
				{
					return;
				}
				for (int j = 0; j < P_2.Count; j++)
				{
					_0001 val2 = P_2[j];
					int num = P_8(val2, P_3);
					if (num >= 0)
					{
						cudFAuMjaallCOdELXEJEqApMBxX<_0001> cudFAuMjaallCOdELXEJEqApMBxX2 = new cudFAuMjaallCOdELXEJEqApMBxX<_0001>();
						cudFAuMjaallCOdELXEJEqApMBxX2.XJXFtQxgcchtxBLQvUvZEaCsJKWLA = wkNCorgsikNbZadwjwtpMBMeuvdYb;
						_0001 val3 = P_3[num];
						cudFAuMjaallCOdELXEJEqApMBxX2.mWbZUWszAZQIjLGRbrLeHOXjzbOR = P_9(new okMYBPiHOMuwixSfVPWSIlunhHYl<_0001>(val2, val3, gIBPcMQrDhIZFlpFWCJzmmnjiNkm.CsysAjVUMGvYOWqTKclZPrJCrsOx.otherId, P_3, true));
						P_5.Find(cudFAuMjaallCOdELXEJEqApMBxX2.PFIhgYeIYNHcjadOQgbsIdwGnkPIA).prNJUrqsciTAXxyaCYjaYFJMFovV = cudFAuMjaallCOdELXEJEqApMBxX2.XJXFtQxgcchtxBLQvUvZEaCsJKWLA.YIbWYydlQVbHzkrumbaUWDpggktp(val2);
						string text = ((!string.IsNullOrEmpty(P_7(val2))) ? ("\"" + P_7(val2) + "\"") : "");
						Logger.Log(P_0 + ((!string.IsNullOrEmpty(text)) ? (" " + text) : "") + " already exists. Imported data will replace original.");
					}
					else
					{
						_0001 arg2 = P_9(new okMYBPiHOMuwixSfVPWSIlunhHYl<_0001>(val2, null, gIBPcMQrDhIZFlpFWCJzmmnjiNkm.CsysAjVUMGvYOWqTKclZPrJCrsOx.otherId, P_3, false));
						P_5.Add(new gIBPcMQrDhIZFlpFWCJzmmnjiNkm(-1, wkNCorgsikNbZadwjwtpMBMeuvdYb.YIbWYydlQVbHzkrumbaUWDpggktp(val2), wkNCorgsikNbZadwjwtpMBMeuvdYb.YIbWYydlQVbHzkrumbaUWDpggktp(arg2)));
						string text2 = ((!string.IsNullOrEmpty(P_7(val2))) ? ("\"" + P_7(val2) + "\"") : "");
						Logger.Log("Imported new " + P_0 + ((!string.IsNullOrEmpty(text2)) ? (" " + text2) : "") + ".");
					}
				}
			}
		}

		[Serializable]
		private sealed class BqxrkBNzJTKgNcopOxEBoFgXGtpFA
		{
			public static readonly BqxrkBNzJTKgNcopOxEBoFgXGtpFA _003C_003E9 = new BqxrkBNzJTKgNcopOxEBoFgXGtpFA();

			public static Action<List<Player_Editor.Mapping>, int> _003C_003E9__199_0;

			public static Action<List<Player_Editor.Mapping>, int> _003C_003E9__217_0;

			public static Action<List<Player_Editor.Mapping>, int> _003C_003E9__233_0;

			public static Action<List<Player_Editor.Mapping>, int> _003C_003E9__249_0;

			public static Action<List<Player_Editor.Mapping>, int> _003C_003E9__265_0;

			internal void LpNvWAkQhcwNqKZVbjABPxWMxCCC(List<Player_Editor.Mapping> P_0, int P_1)
			{
				if (P_0 == null)
				{
					return;
				}
				for (int num = P_0.Count - 1; num >= 0; num--)
				{
					if (P_0[num] == null || P_0[num].categoryId == P_1)
					{
						P_0.RemoveAt(num);
					}
				}
			}

			internal void bXocRkjHdiUEuVPjWTBFWdlvNHML(List<Player_Editor.Mapping> P_0, int P_1)
			{
				if (P_0 == null)
				{
					return;
				}
				for (int num = P_0.Count - 1; num >= 0; num--)
				{
					if (P_0[num] == null || P_0[num].layoutId == P_1)
					{
						P_0.RemoveAt(num);
					}
				}
			}

			internal void bDphMvksNrTLseAeEtRbwdoSNQdAb(List<Player_Editor.Mapping> P_0, int P_1)
			{
				if (P_0 == null)
				{
					return;
				}
				for (int num = P_0.Count - 1; num >= 0; num--)
				{
					if (P_0[num] == null || P_0[num].layoutId == P_1)
					{
						P_0.RemoveAt(num);
					}
				}
			}

			internal void QbXeASbrswitYDXztjAbeVyfvtajB(List<Player_Editor.Mapping> P_0, int P_1)
			{
				if (P_0 == null)
				{
					return;
				}
				for (int num = P_0.Count - 1; num >= 0; num--)
				{
					if (P_0[num] == null || P_0[num].layoutId == P_1)
					{
						P_0.RemoveAt(num);
					}
				}
			}

			internal void waPAeydhwoJBtdNkTOMzPhTaMxXm(List<Player_Editor.Mapping> P_0, int P_1)
			{
				if (P_0 == null)
				{
					return;
				}
				for (int num = P_0.Count - 1; num >= 0; num--)
				{
					if (P_0[num] == null || P_0[num].layoutId == P_1)
					{
						P_0.RemoveAt(num);
					}
				}
			}
		}

		private sealed class aIeBdCkZGlYpejfWiKBvqyXdyjGw
		{
			public List<InputLayout> rZnomdfbWuvhWJNQQVhCgtRYtzub;

			internal int RERiQDiULkPbzZXWSdyGbjLiKxfkB(ControllerMap_Editor P_0, ControllerMap_Editor P_1)
			{
				gHQjnrcYcypVHzwRRexBVZBaiOTS gHQjnrcYcypVHzwRRexBVZBaiOTS2 = new gHQjnrcYcypVHzwRRexBVZBaiOTS();
				gHQjnrcYcypVHzwRRexBVZBaiOTS2.hvqCpuTNvZABSGOMlbInOFunOfTi = P_0;
				gHQjnrcYcypVHzwRRexBVZBaiOTS2.bhlHpgZtjccAkoEPLdkmgryAbkKjb = P_1;
				int num = rZnomdfbWuvhWJNQQVhCgtRYtzub.FindIndex(gHQjnrcYcypVHzwRRexBVZBaiOTS2.ZBFFCTRvFHKRLtSoWRmOJbrVEAqT);
				int num2 = rZnomdfbWuvhWJNQQVhCgtRYtzub.FindIndex(gHQjnrcYcypVHzwRRexBVZBaiOTS2.NkbJxhaptdpbYAeRSdcQtpRKjIvB);
				if (num > num2)
				{
					return 1;
				}
				if (num < num2)
				{
					return -1;
				}
				return 0;
			}
		}

		private sealed class gHQjnrcYcypVHzwRRexBVZBaiOTS
		{
			public ControllerMap_Editor hvqCpuTNvZABSGOMlbInOFunOfTi;

			public ControllerMap_Editor bhlHpgZtjccAkoEPLdkmgryAbkKjb;

			internal bool ZBFFCTRvFHKRLtSoWRmOJbrVEAqT(InputLayout P_0)
			{
				return P_0.id == hvqCpuTNvZABSGOMlbInOFunOfTi.id;
			}

			internal bool NkbJxhaptdpbYAeRSdcQtpRKjIvB(InputLayout P_0)
			{
				return P_0.id == bhlHpgZtjccAkoEPLdkmgryAbkKjb.id;
			}
		}

		private sealed class akvZAFMlvyRqpyFGWBfzhqTAqNBW : IEnumerable<InputCategory>, IEnumerable, IEnumerator<InputCategory>, IEnumerator, IDisposable
		{
			private int KsUBOHPffTYvZtQmvSdOWlAgPMcS;

			private InputCategory KJVcWJKbeADkqkAtlvGZVrEATqkB;

			private int TCSbywEfdFuVHWUqITxueZCzoEyN;

			private string LECaeBlHahjoORbBPkKnIbyWyEDH;

			public string UbgaNrIHaXshgcASkBGlNRXEfvASd;

			public UserData LIRpTWeTbeljrPkXBVZYFKdcqDsg;

			private int dyfoxpVgDLhJcjLElCDtUqVkCgjK;

			InputCategory IEnumerator<InputCategory>.Current
			{
				[DebuggerHidden]
				get
				{
					return KJVcWJKbeADkqkAtlvGZVrEATqkB;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return KJVcWJKbeADkqkAtlvGZVrEATqkB;
				}
			}

			[DebuggerHidden]
			public akvZAFMlvyRqpyFGWBfzhqTAqNBW(int P_0)
			{
				KsUBOHPffTYvZtQmvSdOWlAgPMcS = P_0;
				TCSbywEfdFuVHWUqITxueZCzoEyN = Environment.CurrentManagedThreadId;
			}

			[DebuggerHidden]
			void IDisposable.Dispose()
			{
				KsUBOHPffTYvZtQmvSdOWlAgPMcS = -2;
			}

			private bool MoveNext()
			{
				int ksUBOHPffTYvZtQmvSdOWlAgPMcS = KsUBOHPffTYvZtQmvSdOWlAgPMcS;
				UserData lIRpTWeTbeljrPkXBVZYFKdcqDsg = LIRpTWeTbeljrPkXBVZYFKdcqDsg;
				if (ksUBOHPffTYvZtQmvSdOWlAgPMcS != 0)
				{
					if (ksUBOHPffTYvZtQmvSdOWlAgPMcS != 1)
					{
						return false;
					}
					KsUBOHPffTYvZtQmvSdOWlAgPMcS = -1;
					goto IL_0098;
				}
				KsUBOHPffTYvZtQmvSdOWlAgPMcS = -1;
				if (LECaeBlHahjoORbBPkKnIbyWyEDH == null || LECaeBlHahjoORbBPkKnIbyWyEDH == string.Empty)
				{
					return false;
				}
				if (lIRpTWeTbeljrPkXBVZYFKdcqDsg.actionCategories == null)
				{
					return false;
				}
				dyfoxpVgDLhJcjLElCDtUqVkCgjK = 0;
				goto IL_00a8;
				IL_00a8:
				if (dyfoxpVgDLhJcjLElCDtUqVkCgjK < lIRpTWeTbeljrPkXBVZYFKdcqDsg.actionCategories.Count)
				{
					if (lIRpTWeTbeljrPkXBVZYFKdcqDsg.actionCategories[dyfoxpVgDLhJcjLElCDtUqVkCgjK].tag.Equals(LECaeBlHahjoORbBPkKnIbyWyEDH, StringComparison.OrdinalIgnoreCase))
					{
						KJVcWJKbeADkqkAtlvGZVrEATqkB = lIRpTWeTbeljrPkXBVZYFKdcqDsg.actionCategories[dyfoxpVgDLhJcjLElCDtUqVkCgjK];
						KsUBOHPffTYvZtQmvSdOWlAgPMcS = 1;
						return true;
					}
					goto IL_0098;
				}
				return false;
				IL_0098:
				dyfoxpVgDLhJcjLElCDtUqVkCgjK++;
				goto IL_00a8;
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
			IEnumerator<InputCategory> IEnumerable<InputCategory>.GetEnumerator()
			{
				akvZAFMlvyRqpyFGWBfzhqTAqNBW akvZAFMlvyRqpyFGWBfzhqTAqNBW2;
				if (KsUBOHPffTYvZtQmvSdOWlAgPMcS == -2 && TCSbywEfdFuVHWUqITxueZCzoEyN == Environment.CurrentManagedThreadId)
				{
					KsUBOHPffTYvZtQmvSdOWlAgPMcS = 0;
					akvZAFMlvyRqpyFGWBfzhqTAqNBW2 = this;
				}
				else
				{
					akvZAFMlvyRqpyFGWBfzhqTAqNBW2 = new akvZAFMlvyRqpyFGWBfzhqTAqNBW(0);
					akvZAFMlvyRqpyFGWBfzhqTAqNBW2.LIRpTWeTbeljrPkXBVZYFKdcqDsg = LIRpTWeTbeljrPkXBVZYFKdcqDsg;
				}
				akvZAFMlvyRqpyFGWBfzhqTAqNBW2.LECaeBlHahjoORbBPkKnIbyWyEDH = UbgaNrIHaXshgcASkBGlNRXEfvASd;
				return akvZAFMlvyRqpyFGWBfzhqTAqNBW2;
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return ((IEnumerable<InputCategory>)this).GetEnumerator();
			}
		}

		private sealed class JRJlbejvcpfzSxesysQmbhHdgNWgA : IEnumerable<InputAction>, IEnumerable, IEnumerator<InputAction>, IEnumerator, IDisposable
		{
			private int vDsBtNIuueSbpUEJfNvSAqVPljVt;

			private InputAction HCpIbRlJquQGizbXRsmKHuhHoNoj;

			private int EfAJerNhmFfadjbkjgyBbJpdNGDvb;

			public UserData bexIVQKTHyptIyixOLQNhtcScAxw;

			private string oGubAkGaSBFkbPSVDPulvGYbDyQS;

			public string wyGBiLVuEkCrSxBbgeIpdzvCBANeA;

			private int mHltJDkXHFdevmqmHPvNducePxXJ;

			private int iotnYTvbvTuoZyLimGijLnqkUmnT;

			private InputCategory UYRBTkzFibzbYGEGVYeGzvNLDtwE;

			private int vmqgaHhRuGXIRpVPolFVmBhjRDbLA;

			InputAction IEnumerator<InputAction>.Current
			{
				[DebuggerHidden]
				get
				{
					return HCpIbRlJquQGizbXRsmKHuhHoNoj;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return HCpIbRlJquQGizbXRsmKHuhHoNoj;
				}
			}

			[DebuggerHidden]
			public JRJlbejvcpfzSxesysQmbhHdgNWgA(int P_0)
			{
				vDsBtNIuueSbpUEJfNvSAqVPljVt = P_0;
				EfAJerNhmFfadjbkjgyBbJpdNGDvb = Environment.CurrentManagedThreadId;
			}

			[DebuggerHidden]
			void IDisposable.Dispose()
			{
				UYRBTkzFibzbYGEGVYeGzvNLDtwE = null;
				vDsBtNIuueSbpUEJfNvSAqVPljVt = -2;
			}

			private bool MoveNext()
			{
				int num = vDsBtNIuueSbpUEJfNvSAqVPljVt;
				UserData userData = bexIVQKTHyptIyixOLQNhtcScAxw;
				if (num != 0)
				{
					if (num != 1)
					{
						return false;
					}
					vDsBtNIuueSbpUEJfNvSAqVPljVt = -1;
					goto IL_00fd;
				}
				vDsBtNIuueSbpUEJfNvSAqVPljVt = -1;
				if (userData.kKnhPVRioLmZoBOgUQuQYtoEHyTc == null || userData.actionCategories == null)
				{
					return false;
				}
				if (oGubAkGaSBFkbPSVDPulvGYbDyQS == null || oGubAkGaSBFkbPSVDPulvGYbDyQS == string.Empty)
				{
					return false;
				}
				mHltJDkXHFdevmqmHPvNducePxXJ = userData.kKnhPVRioLmZoBOgUQuQYtoEHyTc.Count;
				iotnYTvbvTuoZyLimGijLnqkUmnT = 0;
				goto IL_0132;
				IL_0122:
				iotnYTvbvTuoZyLimGijLnqkUmnT++;
				goto IL_0132;
				IL_00fd:
				vmqgaHhRuGXIRpVPolFVmBhjRDbLA++;
				goto IL_010d;
				IL_010d:
				if (vmqgaHhRuGXIRpVPolFVmBhjRDbLA < mHltJDkXHFdevmqmHPvNducePxXJ)
				{
					if (UYRBTkzFibzbYGEGVYeGzvNLDtwE.id == userData.kKnhPVRioLmZoBOgUQuQYtoEHyTc[vmqgaHhRuGXIRpVPolFVmBhjRDbLA].categoryId)
					{
						HCpIbRlJquQGizbXRsmKHuhHoNoj = userData.kKnhPVRioLmZoBOgUQuQYtoEHyTc[vmqgaHhRuGXIRpVPolFVmBhjRDbLA];
						vDsBtNIuueSbpUEJfNvSAqVPljVt = 1;
						return true;
					}
					goto IL_00fd;
				}
				UYRBTkzFibzbYGEGVYeGzvNLDtwE = null;
				goto IL_0122;
				IL_0132:
				if (iotnYTvbvTuoZyLimGijLnqkUmnT < userData.actionCategories.Count)
				{
					if (userData.actionCategories[iotnYTvbvTuoZyLimGijLnqkUmnT].tag.Equals(oGubAkGaSBFkbPSVDPulvGYbDyQS, StringComparison.OrdinalIgnoreCase))
					{
						UYRBTkzFibzbYGEGVYeGzvNLDtwE = userData.actionCategories[iotnYTvbvTuoZyLimGijLnqkUmnT];
						vmqgaHhRuGXIRpVPolFVmBhjRDbLA = 0;
						goto IL_010d;
					}
					goto IL_0122;
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
			IEnumerator<InputAction> IEnumerable<InputAction>.GetEnumerator()
			{
				JRJlbejvcpfzSxesysQmbhHdgNWgA jRJlbejvcpfzSxesysQmbhHdgNWgA;
				if (vDsBtNIuueSbpUEJfNvSAqVPljVt == -2 && EfAJerNhmFfadjbkjgyBbJpdNGDvb == Environment.CurrentManagedThreadId)
				{
					vDsBtNIuueSbpUEJfNvSAqVPljVt = 0;
					jRJlbejvcpfzSxesysQmbhHdgNWgA = this;
				}
				else
				{
					jRJlbejvcpfzSxesysQmbhHdgNWgA = new JRJlbejvcpfzSxesysQmbhHdgNWgA(0);
					jRJlbejvcpfzSxesysQmbhHdgNWgA.bexIVQKTHyptIyixOLQNhtcScAxw = bexIVQKTHyptIyixOLQNhtcScAxw;
				}
				jRJlbejvcpfzSxesysQmbhHdgNWgA.oGubAkGaSBFkbPSVDPulvGYbDyQS = wyGBiLVuEkCrSxBbgeIpdzvCBANeA;
				return jRJlbejvcpfzSxesysQmbhHdgNWgA;
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return ((IEnumerable<InputAction>)this).GetEnumerator();
			}
		}

		private sealed class jeRplBHJbrkwrSwymxfoACSRlXW : IEnumerable<InputAction>, IEnumerable, IEnumerator<InputAction>, IEnumerator, IDisposable
		{
			private int FRgYFrrzBVRJyYQkxDwClYFBGqZR;

			private InputAction IuyuLrUifdTViXmbeRzkgOUrQPBV;

			private int ltkZjnWcLWwXYgTyMXvaTSBEhxlj;

			public UserData FtZMZfXcvfdAaBYOPUSOrhGojGso;

			private bool mspRTXLtRtIlVomouRONAcCANtYl;

			public bool YuoiGuqpZOEcSftXDZdIPeIihzVEb;

			private int eojBBiAIShXHSifkpoAkTefsCTTJA;

			public int mFLELYQJrDtFbKlOflLmrmYHxUAS;

			private IEnumerator<int> KQGLiTKmkpyapHAkTgOfczXOlaXtA;

			private int tIAfHdBDrgfGCJHMdHBJQbKBeOsfA;

			InputAction IEnumerator<InputAction>.Current
			{
				[DebuggerHidden]
				get
				{
					return IuyuLrUifdTViXmbeRzkgOUrQPBV;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return IuyuLrUifdTViXmbeRzkgOUrQPBV;
				}
			}

			[DebuggerHidden]
			public jeRplBHJbrkwrSwymxfoACSRlXW(int P_0)
			{
				FRgYFrrzBVRJyYQkxDwClYFBGqZR = P_0;
				ltkZjnWcLWwXYgTyMXvaTSBEhxlj = Environment.CurrentManagedThreadId;
			}

			[DebuggerHidden]
			void IDisposable.Dispose()
			{
				int fRgYFrrzBVRJyYQkxDwClYFBGqZR = FRgYFrrzBVRJyYQkxDwClYFBGqZR;
				if (fRgYFrrzBVRJyYQkxDwClYFBGqZR == -3 || fRgYFrrzBVRJyYQkxDwClYFBGqZR == 1)
				{
					try
					{
					}
					finally
					{
						OwpscuPAasjyiQFklHuejHTgPLyW();
					}
				}
				KQGLiTKmkpyapHAkTgOfczXOlaXtA = null;
				FRgYFrrzBVRJyYQkxDwClYFBGqZR = -2;
			}

			private bool MoveNext()
			{
				try
				{
					int fRgYFrrzBVRJyYQkxDwClYFBGqZR = FRgYFrrzBVRJyYQkxDwClYFBGqZR;
					UserData ftZMZfXcvfdAaBYOPUSOrhGojGso = FtZMZfXcvfdAaBYOPUSOrhGojGso;
					switch (fRgYFrrzBVRJyYQkxDwClYFBGqZR)
					{
					default:
						return false;
					case 0:
						FRgYFrrzBVRJyYQkxDwClYFBGqZR = -1;
						if (ftZMZfXcvfdAaBYOPUSOrhGojGso.kKnhPVRioLmZoBOgUQuQYtoEHyTc == null || ftZMZfXcvfdAaBYOPUSOrhGojGso.actionCategories == null)
						{
							return false;
						}
						if (mspRTXLtRtIlVomouRONAcCANtYl)
						{
							KQGLiTKmkpyapHAkTgOfczXOlaXtA = ftZMZfXcvfdAaBYOPUSOrhGojGso.SortedActionIdsInCategory(eojBBiAIShXHSifkpoAkTefsCTTJA).GetEnumerator();
							FRgYFrrzBVRJyYQkxDwClYFBGqZR = -3;
							goto IL_00a5;
						}
						tIAfHdBDrgfGCJHMdHBJQbKBeOsfA = 0;
						goto IL_0123;
					case 1:
						FRgYFrrzBVRJyYQkxDwClYFBGqZR = -3;
						goto IL_00a5;
					case 2:
						{
							FRgYFrrzBVRJyYQkxDwClYFBGqZR = -1;
							goto IL_0111;
						}
						IL_0123:
						if (tIAfHdBDrgfGCJHMdHBJQbKBeOsfA >= ftZMZfXcvfdAaBYOPUSOrhGojGso.kKnhPVRioLmZoBOgUQuQYtoEHyTc.Count)
						{
							break;
						}
						if (ftZMZfXcvfdAaBYOPUSOrhGojGso.kKnhPVRioLmZoBOgUQuQYtoEHyTc[tIAfHdBDrgfGCJHMdHBJQbKBeOsfA].categoryId == eojBBiAIShXHSifkpoAkTefsCTTJA)
						{
							IuyuLrUifdTViXmbeRzkgOUrQPBV = ftZMZfXcvfdAaBYOPUSOrhGojGso.kKnhPVRioLmZoBOgUQuQYtoEHyTc[tIAfHdBDrgfGCJHMdHBJQbKBeOsfA];
							FRgYFrrzBVRJyYQkxDwClYFBGqZR = 2;
							return true;
						}
						goto IL_0111;
						IL_0111:
						tIAfHdBDrgfGCJHMdHBJQbKBeOsfA++;
						goto IL_0123;
						IL_00a5:
						while (KQGLiTKmkpyapHAkTgOfczXOlaXtA.MoveNext())
						{
							int current = KQGLiTKmkpyapHAkTgOfczXOlaXtA.Current;
							InputAction actionById = ftZMZfXcvfdAaBYOPUSOrhGojGso.GetActionById(current);
							if (actionById != null)
							{
								IuyuLrUifdTViXmbeRzkgOUrQPBV = actionById;
								FRgYFrrzBVRJyYQkxDwClYFBGqZR = 1;
								return true;
							}
						}
						OwpscuPAasjyiQFklHuejHTgPLyW();
						KQGLiTKmkpyapHAkTgOfczXOlaXtA = null;
						break;
					}
					return false;
				}
				catch
				{
					//try-fault
					((IDisposable)this).Dispose();
					throw;
				}
			}

			bool IEnumerator.MoveNext()
			{
				//ILSpy generated this explicit interface implementation from .override directive in MoveNext
				return this.MoveNext();
			}

			private void OwpscuPAasjyiQFklHuejHTgPLyW()
			{
				FRgYFrrzBVRJyYQkxDwClYFBGqZR = -1;
				if (KQGLiTKmkpyapHAkTgOfczXOlaXtA != null)
				{
					KQGLiTKmkpyapHAkTgOfczXOlaXtA.Dispose();
				}
			}

			[DebuggerHidden]
			void IEnumerator.Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator<InputAction> IEnumerable<InputAction>.GetEnumerator()
			{
				jeRplBHJbrkwrSwymxfoACSRlXW jeRplBHJbrkwrSwymxfoACSRlXW2;
				if (FRgYFrrzBVRJyYQkxDwClYFBGqZR == -2 && ltkZjnWcLWwXYgTyMXvaTSBEhxlj == Environment.CurrentManagedThreadId)
				{
					FRgYFrrzBVRJyYQkxDwClYFBGqZR = 0;
					jeRplBHJbrkwrSwymxfoACSRlXW2 = this;
				}
				else
				{
					jeRplBHJbrkwrSwymxfoACSRlXW2 = new jeRplBHJbrkwrSwymxfoACSRlXW(0);
					jeRplBHJbrkwrSwymxfoACSRlXW2.FtZMZfXcvfdAaBYOPUSOrhGojGso = FtZMZfXcvfdAaBYOPUSOrhGojGso;
				}
				jeRplBHJbrkwrSwymxfoACSRlXW2.eojBBiAIShXHSifkpoAkTefsCTTJA = mFLELYQJrDtFbKlOflLmrmYHxUAS;
				jeRplBHJbrkwrSwymxfoACSRlXW2.mspRTXLtRtIlVomouRONAcCANtYl = YuoiGuqpZOEcSftXDZdIPeIihzVEb;
				return jeRplBHJbrkwrSwymxfoACSRlXW2;
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return ((IEnumerable<InputAction>)this).GetEnumerator();
			}
		}

		private sealed class WFCIjqjOXnvEClZabpnEqtsweOFFA : IEnumerable<InputAction>, IEnumerable, IEnumerator<InputAction>, IEnumerator, IDisposable
		{
			private int LovyIlqEtXvIlKNGrDqwdQRfnBFZA;

			private InputAction ELnluOsHeaQHQjPKyfKpVkhSBbNw;

			private int DwegTccMPojLutBiERpcsibpoUwTA;

			public UserData WcsyAArQAbBQPAUIaFXMKsJUhUoMA;

			private string TziPjjUwZrrpzNKhPBQWARrhHLSgA;

			public string bjoPWLQITyozDBcytDERwljUKvDf;

			private bool ywaITQeDNRvRxhAoPbigmKKtEMdV;

			public bool JfnrqKHHFCtabfdDvEBCVReqjpHz;

			private InputCategory TyWgwdGdqopHuhFxFrlsloWdMHAHb;

			private IEnumerator<int> mSXTjjEulrWuzFiykBsMijeXkVTm;

			private int jXbzpTkSaJoUcNoIhjCWFSgKiMRcA;

			InputAction IEnumerator<InputAction>.Current
			{
				[DebuggerHidden]
				get
				{
					return ELnluOsHeaQHQjPKyfKpVkhSBbNw;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return ELnluOsHeaQHQjPKyfKpVkhSBbNw;
				}
			}

			[DebuggerHidden]
			public WFCIjqjOXnvEClZabpnEqtsweOFFA(int P_0)
			{
				LovyIlqEtXvIlKNGrDqwdQRfnBFZA = P_0;
				DwegTccMPojLutBiERpcsibpoUwTA = Environment.CurrentManagedThreadId;
			}

			[DebuggerHidden]
			void IDisposable.Dispose()
			{
				int lovyIlqEtXvIlKNGrDqwdQRfnBFZA = LovyIlqEtXvIlKNGrDqwdQRfnBFZA;
				if (lovyIlqEtXvIlKNGrDqwdQRfnBFZA == -3 || lovyIlqEtXvIlKNGrDqwdQRfnBFZA == 1)
				{
					try
					{
					}
					finally
					{
						yQIIgOmCyQMAYyrLJJlUhxikVBdh();
					}
				}
				TyWgwdGdqopHuhFxFrlsloWdMHAHb = null;
				mSXTjjEulrWuzFiykBsMijeXkVTm = null;
				LovyIlqEtXvIlKNGrDqwdQRfnBFZA = -2;
			}

			private bool MoveNext()
			{
				try
				{
					int lovyIlqEtXvIlKNGrDqwdQRfnBFZA = LovyIlqEtXvIlKNGrDqwdQRfnBFZA;
					UserData wcsyAArQAbBQPAUIaFXMKsJUhUoMA = WcsyAArQAbBQPAUIaFXMKsJUhUoMA;
					switch (lovyIlqEtXvIlKNGrDqwdQRfnBFZA)
					{
					default:
						return false;
					case 0:
					{
						LovyIlqEtXvIlKNGrDqwdQRfnBFZA = -1;
						if (wcsyAArQAbBQPAUIaFXMKsJUhUoMA.kKnhPVRioLmZoBOgUQuQYtoEHyTc == null || wcsyAArQAbBQPAUIaFXMKsJUhUoMA.actionCategories == null)
						{
							return false;
						}
						if (TziPjjUwZrrpzNKhPBQWARrhHLSgA == null || TziPjjUwZrrpzNKhPBQWARrhHLSgA == string.Empty)
						{
							return false;
						}
						int num = wcsyAArQAbBQPAUIaFXMKsJUhUoMA.IndexOfActionCategory(TziPjjUwZrrpzNKhPBQWARrhHLSgA);
						if (num < 0)
						{
							return false;
						}
						TyWgwdGdqopHuhFxFrlsloWdMHAHb = wcsyAArQAbBQPAUIaFXMKsJUhUoMA.GetActionCategory(num);
						if (ywaITQeDNRvRxhAoPbigmKKtEMdV)
						{
							mSXTjjEulrWuzFiykBsMijeXkVTm = wcsyAArQAbBQPAUIaFXMKsJUhUoMA.SortedActionIdsInCategory(TyWgwdGdqopHuhFxFrlsloWdMHAHb.id).GetEnumerator();
							LovyIlqEtXvIlKNGrDqwdQRfnBFZA = -3;
							goto IL_00f2;
						}
						jXbzpTkSaJoUcNoIhjCWFSgKiMRcA = 0;
						goto IL_0175;
					}
					case 1:
						LovyIlqEtXvIlKNGrDqwdQRfnBFZA = -3;
						goto IL_00f2;
					case 2:
						{
							LovyIlqEtXvIlKNGrDqwdQRfnBFZA = -1;
							goto IL_0163;
						}
						IL_0175:
						if (jXbzpTkSaJoUcNoIhjCWFSgKiMRcA >= wcsyAArQAbBQPAUIaFXMKsJUhUoMA.kKnhPVRioLmZoBOgUQuQYtoEHyTc.Count)
						{
							break;
						}
						if (wcsyAArQAbBQPAUIaFXMKsJUhUoMA.kKnhPVRioLmZoBOgUQuQYtoEHyTc[jXbzpTkSaJoUcNoIhjCWFSgKiMRcA].categoryId == TyWgwdGdqopHuhFxFrlsloWdMHAHb.id)
						{
							ELnluOsHeaQHQjPKyfKpVkhSBbNw = wcsyAArQAbBQPAUIaFXMKsJUhUoMA.kKnhPVRioLmZoBOgUQuQYtoEHyTc[jXbzpTkSaJoUcNoIhjCWFSgKiMRcA];
							LovyIlqEtXvIlKNGrDqwdQRfnBFZA = 2;
							return true;
						}
						goto IL_0163;
						IL_00f2:
						while (mSXTjjEulrWuzFiykBsMijeXkVTm.MoveNext())
						{
							int current = mSXTjjEulrWuzFiykBsMijeXkVTm.Current;
							InputAction actionById = wcsyAArQAbBQPAUIaFXMKsJUhUoMA.GetActionById(current);
							if (actionById != null)
							{
								ELnluOsHeaQHQjPKyfKpVkhSBbNw = actionById;
								LovyIlqEtXvIlKNGrDqwdQRfnBFZA = 1;
								return true;
							}
						}
						yQIIgOmCyQMAYyrLJJlUhxikVBdh();
						mSXTjjEulrWuzFiykBsMijeXkVTm = null;
						break;
						IL_0163:
						jXbzpTkSaJoUcNoIhjCWFSgKiMRcA++;
						goto IL_0175;
					}
					return false;
				}
				catch
				{
					//try-fault
					((IDisposable)this).Dispose();
					throw;
				}
			}

			bool IEnumerator.MoveNext()
			{
				//ILSpy generated this explicit interface implementation from .override directive in MoveNext
				return this.MoveNext();
			}

			private void yQIIgOmCyQMAYyrLJJlUhxikVBdh()
			{
				LovyIlqEtXvIlKNGrDqwdQRfnBFZA = -1;
				if (mSXTjjEulrWuzFiykBsMijeXkVTm != null)
				{
					mSXTjjEulrWuzFiykBsMijeXkVTm.Dispose();
				}
			}

			[DebuggerHidden]
			void IEnumerator.Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator<InputAction> IEnumerable<InputAction>.GetEnumerator()
			{
				WFCIjqjOXnvEClZabpnEqtsweOFFA wFCIjqjOXnvEClZabpnEqtsweOFFA;
				if (LovyIlqEtXvIlKNGrDqwdQRfnBFZA == -2 && DwegTccMPojLutBiERpcsibpoUwTA == Environment.CurrentManagedThreadId)
				{
					LovyIlqEtXvIlKNGrDqwdQRfnBFZA = 0;
					wFCIjqjOXnvEClZabpnEqtsweOFFA = this;
				}
				else
				{
					wFCIjqjOXnvEClZabpnEqtsweOFFA = new WFCIjqjOXnvEClZabpnEqtsweOFFA(0);
					wFCIjqjOXnvEClZabpnEqtsweOFFA.WcsyAArQAbBQPAUIaFXMKsJUhUoMA = WcsyAArQAbBQPAUIaFXMKsJUhUoMA;
				}
				wFCIjqjOXnvEClZabpnEqtsweOFFA.TziPjjUwZrrpzNKhPBQWARrhHLSgA = bjoPWLQITyozDBcytDERwljUKvDf;
				wFCIjqjOXnvEClZabpnEqtsweOFFA.ywaITQeDNRvRxhAoPbigmKKtEMdV = JfnrqKHHFCtabfdDvEBCVReqjpHz;
				return wFCIjqjOXnvEClZabpnEqtsweOFFA;
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return ((IEnumerable<InputAction>)this).GetEnumerator();
			}
		}

		private sealed class pOXeQFNrEvVHhtOijUteGFoWlTLf : IEnumerable<InputMapCategory>, IEnumerable, IEnumerator<InputMapCategory>, IEnumerator, IDisposable
		{
			private int SwJKewzipJQMFRPSrTgTmcEHcNjj;

			private InputMapCategory gNPemIOSnBrZtiBGLFmauhezcbHE;

			private int PdKMqCDoPnknlPhdogxCBQeMkSZn;

			private string gJUygxBLtSaJFaFPKwkYophgpgnZA;

			public string BEPZmHRHVZnANLNaqoKOZqjAnTot;

			public UserData NSoIMErurYsXosjonekKwRissrXg;

			private int PDiEZfzwbzzgeyoWSikolnnNTdVF;

			InputMapCategory IEnumerator<InputMapCategory>.Current
			{
				[DebuggerHidden]
				get
				{
					return gNPemIOSnBrZtiBGLFmauhezcbHE;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return gNPemIOSnBrZtiBGLFmauhezcbHE;
				}
			}

			[DebuggerHidden]
			public pOXeQFNrEvVHhtOijUteGFoWlTLf(int P_0)
			{
				SwJKewzipJQMFRPSrTgTmcEHcNjj = P_0;
				PdKMqCDoPnknlPhdogxCBQeMkSZn = Environment.CurrentManagedThreadId;
			}

			[DebuggerHidden]
			void IDisposable.Dispose()
			{
				SwJKewzipJQMFRPSrTgTmcEHcNjj = -2;
			}

			private bool MoveNext()
			{
				int swJKewzipJQMFRPSrTgTmcEHcNjj = SwJKewzipJQMFRPSrTgTmcEHcNjj;
				UserData nSoIMErurYsXosjonekKwRissrXg = NSoIMErurYsXosjonekKwRissrXg;
				if (swJKewzipJQMFRPSrTgTmcEHcNjj != 0)
				{
					if (swJKewzipJQMFRPSrTgTmcEHcNjj != 1)
					{
						return false;
					}
					SwJKewzipJQMFRPSrTgTmcEHcNjj = -1;
					goto IL_0098;
				}
				SwJKewzipJQMFRPSrTgTmcEHcNjj = -1;
				if (gJUygxBLtSaJFaFPKwkYophgpgnZA == null || gJUygxBLtSaJFaFPKwkYophgpgnZA == string.Empty)
				{
					return false;
				}
				if (nSoIMErurYsXosjonekKwRissrXg.mapCategories == null)
				{
					return false;
				}
				PDiEZfzwbzzgeyoWSikolnnNTdVF = 0;
				goto IL_00a8;
				IL_00a8:
				if (PDiEZfzwbzzgeyoWSikolnnNTdVF < nSoIMErurYsXosjonekKwRissrXg.mapCategories.Count)
				{
					if (nSoIMErurYsXosjonekKwRissrXg.mapCategories[PDiEZfzwbzzgeyoWSikolnnNTdVF].tag.Equals(gJUygxBLtSaJFaFPKwkYophgpgnZA, StringComparison.OrdinalIgnoreCase))
					{
						gNPemIOSnBrZtiBGLFmauhezcbHE = nSoIMErurYsXosjonekKwRissrXg.mapCategories[PDiEZfzwbzzgeyoWSikolnnNTdVF];
						SwJKewzipJQMFRPSrTgTmcEHcNjj = 1;
						return true;
					}
					goto IL_0098;
				}
				return false;
				IL_0098:
				PDiEZfzwbzzgeyoWSikolnnNTdVF++;
				goto IL_00a8;
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
			IEnumerator<InputMapCategory> IEnumerable<InputMapCategory>.GetEnumerator()
			{
				pOXeQFNrEvVHhtOijUteGFoWlTLf pOXeQFNrEvVHhtOijUteGFoWlTLf2;
				if (SwJKewzipJQMFRPSrTgTmcEHcNjj == -2 && PdKMqCDoPnknlPhdogxCBQeMkSZn == Environment.CurrentManagedThreadId)
				{
					SwJKewzipJQMFRPSrTgTmcEHcNjj = 0;
					pOXeQFNrEvVHhtOijUteGFoWlTLf2 = this;
				}
				else
				{
					pOXeQFNrEvVHhtOijUteGFoWlTLf2 = new pOXeQFNrEvVHhtOijUteGFoWlTLf(0);
					pOXeQFNrEvVHhtOijUteGFoWlTLf2.NSoIMErurYsXosjonekKwRissrXg = NSoIMErurYsXosjonekKwRissrXg;
				}
				pOXeQFNrEvVHhtOijUteGFoWlTLf2.gJUygxBLtSaJFaFPKwkYophgpgnZA = BEPZmHRHVZnANLNaqoKOZqjAnTot;
				return pOXeQFNrEvVHhtOijUteGFoWlTLf2;
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return ((IEnumerable<InputMapCategory>)this).GetEnumerator();
			}
		}

		private sealed class ScIKNKgSZWiXAgoUWOSJbCQjNspK : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
		{
			private int jMGYYUmZnGHEDjNwUbeJNZBYpXAH;

			private string vGtMWhrqMaKGtSErRrrghIxUFZdbA;

			private int TtsPGwjyHJsLDjRCUcbymDbFHQbJA;

			public UserData pPFffriaQTyXVpKuSiEJQKTEDuYr;

			private int NZiIJpiyjzeskmqgbitqSuHKKyGJ;

			public int lePHncbgCfOCVOoncsSKSGwRgfLH;

			private IEnumerator<int> xnQEtMiYYcFzBTCGgEfnjgLSGMXH;

			string IEnumerator<string>.Current
			{
				[DebuggerHidden]
				get
				{
					return vGtMWhrqMaKGtSErRrrghIxUFZdbA;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return vGtMWhrqMaKGtSErRrrghIxUFZdbA;
				}
			}

			[DebuggerHidden]
			public ScIKNKgSZWiXAgoUWOSJbCQjNspK(int P_0)
			{
				jMGYYUmZnGHEDjNwUbeJNZBYpXAH = P_0;
				TtsPGwjyHJsLDjRCUcbymDbFHQbJA = Environment.CurrentManagedThreadId;
			}

			[DebuggerHidden]
			void IDisposable.Dispose()
			{
				int num = jMGYYUmZnGHEDjNwUbeJNZBYpXAH;
				if (num == -3 || num == 1)
				{
					try
					{
					}
					finally
					{
						hDuomvPfyLqOnMvieCKCdeSUccHDA();
					}
				}
				xnQEtMiYYcFzBTCGgEfnjgLSGMXH = null;
				jMGYYUmZnGHEDjNwUbeJNZBYpXAH = -2;
			}

			private bool MoveNext()
			{
				try
				{
					int num = jMGYYUmZnGHEDjNwUbeJNZBYpXAH;
					UserData userData = pPFffriaQTyXVpKuSiEJQKTEDuYr;
					switch (num)
					{
					default:
						return false;
					case 0:
						jMGYYUmZnGHEDjNwUbeJNZBYpXAH = -1;
						if (userData.actionCategories == null || userData.kKnhPVRioLmZoBOgUQuQYtoEHyTc == null)
						{
							return false;
						}
						xnQEtMiYYcFzBTCGgEfnjgLSGMXH = userData.actionCategoryMap.ActionIdsInCategory(NZiIJpiyjzeskmqgbitqSuHKKyGJ).GetEnumerator();
						jMGYYUmZnGHEDjNwUbeJNZBYpXAH = -3;
						break;
					case 1:
						jMGYYUmZnGHEDjNwUbeJNZBYpXAH = -3;
						break;
					}
					while (xnQEtMiYYcFzBTCGgEfnjgLSGMXH.MoveNext())
					{
						int current = xnQEtMiYYcFzBTCGgEfnjgLSGMXH.Current;
						InputAction actionById = userData.GetActionById(current);
						if (actionById != null)
						{
							vGtMWhrqMaKGtSErRrrghIxUFZdbA = actionById.descriptiveName;
							jMGYYUmZnGHEDjNwUbeJNZBYpXAH = 1;
							return true;
						}
					}
					hDuomvPfyLqOnMvieCKCdeSUccHDA();
					xnQEtMiYYcFzBTCGgEfnjgLSGMXH = null;
					return false;
				}
				catch
				{
					//try-fault
					((IDisposable)this).Dispose();
					throw;
				}
			}

			bool IEnumerator.MoveNext()
			{
				//ILSpy generated this explicit interface implementation from .override directive in MoveNext
				return this.MoveNext();
			}

			private void hDuomvPfyLqOnMvieCKCdeSUccHDA()
			{
				jMGYYUmZnGHEDjNwUbeJNZBYpXAH = -1;
				if (xnQEtMiYYcFzBTCGgEfnjgLSGMXH != null)
				{
					xnQEtMiYYcFzBTCGgEfnjgLSGMXH.Dispose();
				}
			}

			[DebuggerHidden]
			void IEnumerator.Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator<string> IEnumerable<string>.GetEnumerator()
			{
				ScIKNKgSZWiXAgoUWOSJbCQjNspK scIKNKgSZWiXAgoUWOSJbCQjNspK;
				if (jMGYYUmZnGHEDjNwUbeJNZBYpXAH == -2 && TtsPGwjyHJsLDjRCUcbymDbFHQbJA == Environment.CurrentManagedThreadId)
				{
					jMGYYUmZnGHEDjNwUbeJNZBYpXAH = 0;
					scIKNKgSZWiXAgoUWOSJbCQjNspK = this;
				}
				else
				{
					scIKNKgSZWiXAgoUWOSJbCQjNspK = new ScIKNKgSZWiXAgoUWOSJbCQjNspK(0);
					scIKNKgSZWiXAgoUWOSJbCQjNspK.pPFffriaQTyXVpKuSiEJQKTEDuYr = pPFffriaQTyXVpKuSiEJQKTEDuYr;
				}
				scIKNKgSZWiXAgoUWOSJbCQjNspK.NZiIJpiyjzeskmqgbitqSuHKKyGJ = lePHncbgCfOCVOoncsSKSGwRgfLH;
				return scIKNKgSZWiXAgoUWOSJbCQjNspK;
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return ((IEnumerable<string>)this).GetEnumerator();
			}
		}

		private sealed class IlUBngBbMrbDQHKPhbvqmVnsOTzdc : IEnumerable<int>, IEnumerable, IEnumerator<int>, IEnumerator, IDisposable
		{
			private int RjndoZidFisPPpQhUXDBGRicQVwbb;

			private int GxonZSuLuiTyDwDGBZFARIYxJTxH;

			private int FmysjfjhLbaMHLFkOwLXQmeBeDjhA;

			public UserData lIfgfVVsLaBYaFICnCUFQaIYngxab;

			private int YtnrbRsgZdAihEdakVmrCHuCKWqS;

			public int qTIJTJMjmyaYmAXFIyApnQsgfLfm;

			private IEnumerator<int> ZrOHanmyTwJgpuxqANmIUuErcmbv;

			int IEnumerator<int>.Current
			{
				[DebuggerHidden]
				get
				{
					return GxonZSuLuiTyDwDGBZFARIYxJTxH;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return GxonZSuLuiTyDwDGBZFARIYxJTxH;
				}
			}

			[DebuggerHidden]
			public IlUBngBbMrbDQHKPhbvqmVnsOTzdc(int P_0)
			{
				RjndoZidFisPPpQhUXDBGRicQVwbb = P_0;
				FmysjfjhLbaMHLFkOwLXQmeBeDjhA = Environment.CurrentManagedThreadId;
			}

			[DebuggerHidden]
			void IDisposable.Dispose()
			{
				int rjndoZidFisPPpQhUXDBGRicQVwbb = RjndoZidFisPPpQhUXDBGRicQVwbb;
				if (rjndoZidFisPPpQhUXDBGRicQVwbb == -3 || rjndoZidFisPPpQhUXDBGRicQVwbb == 1)
				{
					try
					{
					}
					finally
					{
						mVarUvcfchjGnwiyWfoTbryYEDMR();
					}
				}
				ZrOHanmyTwJgpuxqANmIUuErcmbv = null;
				RjndoZidFisPPpQhUXDBGRicQVwbb = -2;
			}

			private bool MoveNext()
			{
				try
				{
					int rjndoZidFisPPpQhUXDBGRicQVwbb = RjndoZidFisPPpQhUXDBGRicQVwbb;
					UserData userData = lIfgfVVsLaBYaFICnCUFQaIYngxab;
					switch (rjndoZidFisPPpQhUXDBGRicQVwbb)
					{
					default:
						return false;
					case 0:
						RjndoZidFisPPpQhUXDBGRicQVwbb = -1;
						if (userData.actionCategories == null || userData.kKnhPVRioLmZoBOgUQuQYtoEHyTc == null)
						{
							return false;
						}
						ZrOHanmyTwJgpuxqANmIUuErcmbv = userData.actionCategoryMap.ActionIdsInCategory(YtnrbRsgZdAihEdakVmrCHuCKWqS).GetEnumerator();
						RjndoZidFisPPpQhUXDBGRicQVwbb = -3;
						break;
					case 1:
						RjndoZidFisPPpQhUXDBGRicQVwbb = -3;
						break;
					}
					if (ZrOHanmyTwJgpuxqANmIUuErcmbv.MoveNext())
					{
						int current = ZrOHanmyTwJgpuxqANmIUuErcmbv.Current;
						GxonZSuLuiTyDwDGBZFARIYxJTxH = current;
						RjndoZidFisPPpQhUXDBGRicQVwbb = 1;
						return true;
					}
					mVarUvcfchjGnwiyWfoTbryYEDMR();
					ZrOHanmyTwJgpuxqANmIUuErcmbv = null;
					return false;
				}
				catch
				{
					//try-fault
					((IDisposable)this).Dispose();
					throw;
				}
			}

			bool IEnumerator.MoveNext()
			{
				//ILSpy generated this explicit interface implementation from .override directive in MoveNext
				return this.MoveNext();
			}

			private void mVarUvcfchjGnwiyWfoTbryYEDMR()
			{
				RjndoZidFisPPpQhUXDBGRicQVwbb = -1;
				if (ZrOHanmyTwJgpuxqANmIUuErcmbv != null)
				{
					ZrOHanmyTwJgpuxqANmIUuErcmbv.Dispose();
				}
			}

			[DebuggerHidden]
			void IEnumerator.Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator<int> IEnumerable<int>.GetEnumerator()
			{
				IlUBngBbMrbDQHKPhbvqmVnsOTzdc ilUBngBbMrbDQHKPhbvqmVnsOTzdc;
				if (RjndoZidFisPPpQhUXDBGRicQVwbb == -2 && FmysjfjhLbaMHLFkOwLXQmeBeDjhA == Environment.CurrentManagedThreadId)
				{
					RjndoZidFisPPpQhUXDBGRicQVwbb = 0;
					ilUBngBbMrbDQHKPhbvqmVnsOTzdc = this;
				}
				else
				{
					ilUBngBbMrbDQHKPhbvqmVnsOTzdc = new IlUBngBbMrbDQHKPhbvqmVnsOTzdc(0);
					ilUBngBbMrbDQHKPhbvqmVnsOTzdc.lIfgfVVsLaBYaFICnCUFQaIYngxab = lIfgfVVsLaBYaFICnCUFQaIYngxab;
				}
				ilUBngBbMrbDQHKPhbvqmVnsOTzdc.YtnrbRsgZdAihEdakVmrCHuCKWqS = qTIJTJMjmyaYmAXFIyApnQsgfLfm;
				return ilUBngBbMrbDQHKPhbvqmVnsOTzdc;
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return ((IEnumerable<int>)this).GetEnumerator();
			}
		}

		private sealed class UsVcNshWAYkknnPjZOjTZMsLaqUo : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
		{
			private int qlDRObqaMATEOWOsTBrYQSTsceGL;

			private string fvBcFEevChsLmiFWaaxiuqGbLlWHb;

			private int PTrkxzrnmyafQTXLUDfKDTbmJyCt;

			public UserData QlkgeGvTazXozkIwxYuVLLYnoQas;

			private int FJTCubASeaDtxpPNJGEWDCfuoTPV;

			public int qAqJaDsxhXtvjsXHZmCgtCFbeIsQ;

			private IEnumerator<int> nDFjHhZglCuVcZnljcUiNSHNcqCq;

			string IEnumerator<string>.Current
			{
				[DebuggerHidden]
				get
				{
					return fvBcFEevChsLmiFWaaxiuqGbLlWHb;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return fvBcFEevChsLmiFWaaxiuqGbLlWHb;
				}
			}

			[DebuggerHidden]
			public UsVcNshWAYkknnPjZOjTZMsLaqUo(int P_0)
			{
				qlDRObqaMATEOWOsTBrYQSTsceGL = P_0;
				PTrkxzrnmyafQTXLUDfKDTbmJyCt = Environment.CurrentManagedThreadId;
			}

			[DebuggerHidden]
			void IDisposable.Dispose()
			{
				int num = qlDRObqaMATEOWOsTBrYQSTsceGL;
				if (num == -3 || num == 1)
				{
					try
					{
					}
					finally
					{
						ABuxToubqgICAVcMifsYHCHhvtzB();
					}
				}
				nDFjHhZglCuVcZnljcUiNSHNcqCq = null;
				qlDRObqaMATEOWOsTBrYQSTsceGL = -2;
			}

			private bool MoveNext()
			{
				try
				{
					int num = qlDRObqaMATEOWOsTBrYQSTsceGL;
					UserData qlkgeGvTazXozkIwxYuVLLYnoQas = QlkgeGvTazXozkIwxYuVLLYnoQas;
					switch (num)
					{
					default:
						return false;
					case 0:
						qlDRObqaMATEOWOsTBrYQSTsceGL = -1;
						if (qlkgeGvTazXozkIwxYuVLLYnoQas.actionCategories == null || qlkgeGvTazXozkIwxYuVLLYnoQas.kKnhPVRioLmZoBOgUQuQYtoEHyTc == null)
						{
							return false;
						}
						nDFjHhZglCuVcZnljcUiNSHNcqCq = qlkgeGvTazXozkIwxYuVLLYnoQas.actionCategoryMap.ActionIdsInCategory(FJTCubASeaDtxpPNJGEWDCfuoTPV).GetEnumerator();
						qlDRObqaMATEOWOsTBrYQSTsceGL = -3;
						break;
					case 1:
						qlDRObqaMATEOWOsTBrYQSTsceGL = -3;
						break;
					}
					while (nDFjHhZglCuVcZnljcUiNSHNcqCq.MoveNext())
					{
						int current = nDFjHhZglCuVcZnljcUiNSHNcqCq.Current;
						InputAction actionById = qlkgeGvTazXozkIwxYuVLLYnoQas.GetActionById(current);
						if (actionById != null)
						{
							fvBcFEevChsLmiFWaaxiuqGbLlWHb = actionById.name;
							qlDRObqaMATEOWOsTBrYQSTsceGL = 1;
							return true;
						}
					}
					ABuxToubqgICAVcMifsYHCHhvtzB();
					nDFjHhZglCuVcZnljcUiNSHNcqCq = null;
					return false;
				}
				catch
				{
					//try-fault
					((IDisposable)this).Dispose();
					throw;
				}
			}

			bool IEnumerator.MoveNext()
			{
				//ILSpy generated this explicit interface implementation from .override directive in MoveNext
				return this.MoveNext();
			}

			private void ABuxToubqgICAVcMifsYHCHhvtzB()
			{
				qlDRObqaMATEOWOsTBrYQSTsceGL = -1;
				if (nDFjHhZglCuVcZnljcUiNSHNcqCq != null)
				{
					nDFjHhZglCuVcZnljcUiNSHNcqCq.Dispose();
				}
			}

			[DebuggerHidden]
			void IEnumerator.Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator<string> IEnumerable<string>.GetEnumerator()
			{
				UsVcNshWAYkknnPjZOjTZMsLaqUo usVcNshWAYkknnPjZOjTZMsLaqUo;
				if (qlDRObqaMATEOWOsTBrYQSTsceGL == -2 && PTrkxzrnmyafQTXLUDfKDTbmJyCt == Environment.CurrentManagedThreadId)
				{
					qlDRObqaMATEOWOsTBrYQSTsceGL = 0;
					usVcNshWAYkknnPjZOjTZMsLaqUo = this;
				}
				else
				{
					usVcNshWAYkknnPjZOjTZMsLaqUo = new UsVcNshWAYkknnPjZOjTZMsLaqUo(0);
					usVcNshWAYkknnPjZOjTZMsLaqUo.QlkgeGvTazXozkIwxYuVLLYnoQas = QlkgeGvTazXozkIwxYuVLLYnoQas;
				}
				usVcNshWAYkknnPjZOjTZMsLaqUo.FJTCubASeaDtxpPNJGEWDCfuoTPV = qAqJaDsxhXtvjsXHZmCgtCFbeIsQ;
				return usVcNshWAYkknnPjZOjTZMsLaqUo;
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return ((IEnumerable<string>)this).GetEnumerator();
			}
		}

		private sealed class cXgAzmbhCvUoaISnKebadELfsZTn : IEnumerable<InputCategory>, IEnumerable, IEnumerator<InputCategory>, IEnumerator, IDisposable
		{
			private int HJmUTPOwCuGjxMQjdfKPjqhKagGCA;

			private InputCategory cgibemNHpKFHsJPscayGAoMRnQXZ;

			private int OMvNZqHNqnAFwMWAVcivcFAvuYWm;

			private string tToJufDSbgtdgMBsYyWESqoCvJOo;

			public string BrGjdsJqIJskSEfmJMcLoFfgbWjX;

			public UserData VLDANBVcmbWJgcFVGPdynfiXsHTP;

			private int LOHFwVaQdsIrjwqsqBMuWHicmHxN;

			InputCategory IEnumerator<InputCategory>.Current
			{
				[DebuggerHidden]
				get
				{
					return cgibemNHpKFHsJPscayGAoMRnQXZ;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return cgibemNHpKFHsJPscayGAoMRnQXZ;
				}
			}

			[DebuggerHidden]
			public cXgAzmbhCvUoaISnKebadELfsZTn(int P_0)
			{
				HJmUTPOwCuGjxMQjdfKPjqhKagGCA = P_0;
				OMvNZqHNqnAFwMWAVcivcFAvuYWm = Environment.CurrentManagedThreadId;
			}

			[DebuggerHidden]
			void IDisposable.Dispose()
			{
				HJmUTPOwCuGjxMQjdfKPjqhKagGCA = -2;
			}

			private bool MoveNext()
			{
				int hJmUTPOwCuGjxMQjdfKPjqhKagGCA = HJmUTPOwCuGjxMQjdfKPjqhKagGCA;
				UserData vLDANBVcmbWJgcFVGPdynfiXsHTP = VLDANBVcmbWJgcFVGPdynfiXsHTP;
				if (hJmUTPOwCuGjxMQjdfKPjqhKagGCA != 0)
				{
					if (hJmUTPOwCuGjxMQjdfKPjqhKagGCA != 1)
					{
						return false;
					}
					HJmUTPOwCuGjxMQjdfKPjqhKagGCA = -1;
					goto IL_00b3;
				}
				HJmUTPOwCuGjxMQjdfKPjqhKagGCA = -1;
				if (tToJufDSbgtdgMBsYyWESqoCvJOo == null || tToJufDSbgtdgMBsYyWESqoCvJOo == string.Empty)
				{
					return false;
				}
				if (vLDANBVcmbWJgcFVGPdynfiXsHTP.actionCategories == null)
				{
					return false;
				}
				LOHFwVaQdsIrjwqsqBMuWHicmHxN = 0;
				goto IL_00c3;
				IL_00c3:
				if (LOHFwVaQdsIrjwqsqBMuWHicmHxN < vLDANBVcmbWJgcFVGPdynfiXsHTP.actionCategories.Count)
				{
					if (vLDANBVcmbWJgcFVGPdynfiXsHTP.actionCategories[LOHFwVaQdsIrjwqsqBMuWHicmHxN].userAssignable && vLDANBVcmbWJgcFVGPdynfiXsHTP.actionCategories[LOHFwVaQdsIrjwqsqBMuWHicmHxN].tag.Equals(tToJufDSbgtdgMBsYyWESqoCvJOo, StringComparison.OrdinalIgnoreCase))
					{
						cgibemNHpKFHsJPscayGAoMRnQXZ = vLDANBVcmbWJgcFVGPdynfiXsHTP.actionCategories[LOHFwVaQdsIrjwqsqBMuWHicmHxN];
						HJmUTPOwCuGjxMQjdfKPjqhKagGCA = 1;
						return true;
					}
					goto IL_00b3;
				}
				return false;
				IL_00b3:
				LOHFwVaQdsIrjwqsqBMuWHicmHxN++;
				goto IL_00c3;
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
			IEnumerator<InputCategory> IEnumerable<InputCategory>.GetEnumerator()
			{
				cXgAzmbhCvUoaISnKebadELfsZTn cXgAzmbhCvUoaISnKebadELfsZTn2;
				if (HJmUTPOwCuGjxMQjdfKPjqhKagGCA == -2 && OMvNZqHNqnAFwMWAVcivcFAvuYWm == Environment.CurrentManagedThreadId)
				{
					HJmUTPOwCuGjxMQjdfKPjqhKagGCA = 0;
					cXgAzmbhCvUoaISnKebadELfsZTn2 = this;
				}
				else
				{
					cXgAzmbhCvUoaISnKebadELfsZTn2 = new cXgAzmbhCvUoaISnKebadELfsZTn(0);
					cXgAzmbhCvUoaISnKebadELfsZTn2.VLDANBVcmbWJgcFVGPdynfiXsHTP = VLDANBVcmbWJgcFVGPdynfiXsHTP;
				}
				cXgAzmbhCvUoaISnKebadELfsZTn2.tToJufDSbgtdgMBsYyWESqoCvJOo = BrGjdsJqIJskSEfmJMcLoFfgbWjX;
				return cXgAzmbhCvUoaISnKebadELfsZTn2;
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return ((IEnumerable<InputCategory>)this).GetEnumerator();
			}
		}

		private sealed class HZZNgIGcRRXwvNaPnoipqlNnoEUo : IEnumerable<InputAction>, IEnumerable, IEnumerator<InputAction>, IEnumerator, IDisposable
		{
			private int szUGfzJvqpwebmDalhFWkppEJkcHb;

			private InputAction HIZfnEezsJhXfYIhQCizKEMYTpym;

			private int WizxwOKxRILWVhvyRQyHQEkbLcKf;

			public UserData mioHKMTVrYgbeQtpKHZlRGysRNjT;

			private int JWryWgjRvaAydWtnRITBIJFemrRW;

			public int HulMNgeGtzYDuZjrKrEgEnRlKKpA;

			private bool LgcRliptTWnSyghbGWskfLNGhXYAA;

			public bool nqNDditaByiEeNiCDAztfqrofcAiA;

			private InputCategory fKZoysetNqeuDOJqcgRsKuKPULuAA;

			private IEnumerator<int> JoZtJLxJnqgiGYBuhMzsWIJGJjET;

			private int lygPkeWgepAqEDhAYucpAdiupgnv;

			InputAction IEnumerator<InputAction>.Current
			{
				[DebuggerHidden]
				get
				{
					return HIZfnEezsJhXfYIhQCizKEMYTpym;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return HIZfnEezsJhXfYIhQCizKEMYTpym;
				}
			}

			[DebuggerHidden]
			public HZZNgIGcRRXwvNaPnoipqlNnoEUo(int P_0)
			{
				szUGfzJvqpwebmDalhFWkppEJkcHb = P_0;
				WizxwOKxRILWVhvyRQyHQEkbLcKf = Environment.CurrentManagedThreadId;
			}

			[DebuggerHidden]
			void IDisposable.Dispose()
			{
				int num = szUGfzJvqpwebmDalhFWkppEJkcHb;
				if (num == -3 || num == 1)
				{
					try
					{
					}
					finally
					{
						NmCtNbwSQnXgwSQCDgsSLLsfOgzK();
					}
				}
				fKZoysetNqeuDOJqcgRsKuKPULuAA = null;
				JoZtJLxJnqgiGYBuhMzsWIJGJjET = null;
				szUGfzJvqpwebmDalhFWkppEJkcHb = -2;
			}

			private bool MoveNext()
			{
				try
				{
					int num = szUGfzJvqpwebmDalhFWkppEJkcHb;
					UserData userData = mioHKMTVrYgbeQtpKHZlRGysRNjT;
					InputAction inputAction;
					switch (num)
					{
					default:
						return false;
					case 0:
						szUGfzJvqpwebmDalhFWkppEJkcHb = -1;
						if (userData.kKnhPVRioLmZoBOgUQuQYtoEHyTc == null || userData.actionCategories == null)
						{
							return false;
						}
						fKZoysetNqeuDOJqcgRsKuKPULuAA = userData.GetActionCategoryById(JWryWgjRvaAydWtnRITBIJFemrRW);
						if (fKZoysetNqeuDOJqcgRsKuKPULuAA == null || !fKZoysetNqeuDOJqcgRsKuKPULuAA.userAssignable)
						{
							return false;
						}
						if (LgcRliptTWnSyghbGWskfLNGhXYAA)
						{
							JoZtJLxJnqgiGYBuhMzsWIJGJjET = userData.SortedActionIdsInCategory(fKZoysetNqeuDOJqcgRsKuKPULuAA.id).GetEnumerator();
							szUGfzJvqpwebmDalhFWkppEJkcHb = -3;
							goto IL_00e4;
						}
						lygPkeWgepAqEDhAYucpAdiupgnv = 0;
						goto IL_0165;
					case 1:
						szUGfzJvqpwebmDalhFWkppEJkcHb = -3;
						goto IL_00e4;
					case 2:
						{
							szUGfzJvqpwebmDalhFWkppEJkcHb = -1;
							goto IL_0153;
						}
						IL_00e4:
						while (JoZtJLxJnqgiGYBuhMzsWIJGJjET.MoveNext())
						{
							int current = JoZtJLxJnqgiGYBuhMzsWIJGJjET.Current;
							InputAction actionById = userData.GetActionById(current);
							if (actionById != null && actionById.userAssignable)
							{
								HIZfnEezsJhXfYIhQCizKEMYTpym = actionById;
								szUGfzJvqpwebmDalhFWkppEJkcHb = 1;
								return true;
							}
						}
						NmCtNbwSQnXgwSQCDgsSLLsfOgzK();
						JoZtJLxJnqgiGYBuhMzsWIJGJjET = null;
						break;
						IL_0153:
						lygPkeWgepAqEDhAYucpAdiupgnv++;
						goto IL_0165;
						IL_0165:
						if (lygPkeWgepAqEDhAYucpAdiupgnv >= userData.kKnhPVRioLmZoBOgUQuQYtoEHyTc.Count)
						{
							break;
						}
						inputAction = userData.kKnhPVRioLmZoBOgUQuQYtoEHyTc[lygPkeWgepAqEDhAYucpAdiupgnv];
						if (inputAction.categoryId == fKZoysetNqeuDOJqcgRsKuKPULuAA.id && inputAction.userAssignable)
						{
							HIZfnEezsJhXfYIhQCizKEMYTpym = inputAction;
							szUGfzJvqpwebmDalhFWkppEJkcHb = 2;
							return true;
						}
						goto IL_0153;
					}
					return false;
				}
				catch
				{
					//try-fault
					((IDisposable)this).Dispose();
					throw;
				}
			}

			bool IEnumerator.MoveNext()
			{
				//ILSpy generated this explicit interface implementation from .override directive in MoveNext
				return this.MoveNext();
			}

			private void NmCtNbwSQnXgwSQCDgsSLLsfOgzK()
			{
				szUGfzJvqpwebmDalhFWkppEJkcHb = -1;
				if (JoZtJLxJnqgiGYBuhMzsWIJGJjET != null)
				{
					JoZtJLxJnqgiGYBuhMzsWIJGJjET.Dispose();
				}
			}

			[DebuggerHidden]
			void IEnumerator.Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator<InputAction> IEnumerable<InputAction>.GetEnumerator()
			{
				HZZNgIGcRRXwvNaPnoipqlNnoEUo hZZNgIGcRRXwvNaPnoipqlNnoEUo;
				if (szUGfzJvqpwebmDalhFWkppEJkcHb == -2 && WizxwOKxRILWVhvyRQyHQEkbLcKf == Environment.CurrentManagedThreadId)
				{
					szUGfzJvqpwebmDalhFWkppEJkcHb = 0;
					hZZNgIGcRRXwvNaPnoipqlNnoEUo = this;
				}
				else
				{
					hZZNgIGcRRXwvNaPnoipqlNnoEUo = new HZZNgIGcRRXwvNaPnoipqlNnoEUo(0);
					hZZNgIGcRRXwvNaPnoipqlNnoEUo.mioHKMTVrYgbeQtpKHZlRGysRNjT = mioHKMTVrYgbeQtpKHZlRGysRNjT;
				}
				hZZNgIGcRRXwvNaPnoipqlNnoEUo.JWryWgjRvaAydWtnRITBIJFemrRW = HulMNgeGtzYDuZjrKrEgEnRlKKpA;
				hZZNgIGcRRXwvNaPnoipqlNnoEUo.LgcRliptTWnSyghbGWskfLNGhXYAA = nqNDditaByiEeNiCDAztfqrofcAiA;
				return hZZNgIGcRRXwvNaPnoipqlNnoEUo;
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return ((IEnumerable<InputAction>)this).GetEnumerator();
			}
		}

		private sealed class UFJjEpfRQRajlIAkCyEjiApMAVhic : IEnumerable<InputAction>, IEnumerable, IEnumerator<InputAction>, IEnumerator, IDisposable
		{
			private int htbersTCIkGovGgnGnkDpmkluTQr;

			private InputAction pwsyPzrBQzPAUlCoNbildkmidpAGA;

			private int ANynxZtpnppDVqTEYjOMxHUiEpAm;

			public UserData fYYASVgjcCMCPokAqbwmwtsibAjdb;

			private string LFxkFIHTHirxwSZxOMpAYdzCcDkI;

			public string MKilOVFNRMGXIszHiwQCBQwwSZLP;

			private bool frzjmrXmRrbezdHVpOvpDmsytcCeb;

			public bool mKLCOIVBfwDdxAhUNSPmfZpfAqzpA;

			private InputCategory ZdiFGkPdAMJkbQxVfDcpRNYsDOHg;

			private IEnumerator<int> KhhuYSsZKoVpYiRtmyreqUjoItre;

			private int wXytujDquvBltsTDIQixzReGlGwN;

			InputAction IEnumerator<InputAction>.Current
			{
				[DebuggerHidden]
				get
				{
					return pwsyPzrBQzPAUlCoNbildkmidpAGA;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return pwsyPzrBQzPAUlCoNbildkmidpAGA;
				}
			}

			[DebuggerHidden]
			public UFJjEpfRQRajlIAkCyEjiApMAVhic(int P_0)
			{
				htbersTCIkGovGgnGnkDpmkluTQr = P_0;
				ANynxZtpnppDVqTEYjOMxHUiEpAm = Environment.CurrentManagedThreadId;
			}

			[DebuggerHidden]
			void IDisposable.Dispose()
			{
				int num = htbersTCIkGovGgnGnkDpmkluTQr;
				if (num == -3 || num == 1)
				{
					try
					{
					}
					finally
					{
						oRolUBIcLwtzJvqAZWFBybeVCTAZ();
					}
				}
				ZdiFGkPdAMJkbQxVfDcpRNYsDOHg = null;
				KhhuYSsZKoVpYiRtmyreqUjoItre = null;
				htbersTCIkGovGgnGnkDpmkluTQr = -2;
			}

			private bool MoveNext()
			{
				try
				{
					int num = htbersTCIkGovGgnGnkDpmkluTQr;
					UserData userData = fYYASVgjcCMCPokAqbwmwtsibAjdb;
					InputAction inputAction;
					switch (num)
					{
					default:
						return false;
					case 0:
						htbersTCIkGovGgnGnkDpmkluTQr = -1;
						if (userData.kKnhPVRioLmZoBOgUQuQYtoEHyTc == null || userData.actionCategories == null)
						{
							return false;
						}
						ZdiFGkPdAMJkbQxVfDcpRNYsDOHg = userData.GetActionCategory(LFxkFIHTHirxwSZxOMpAYdzCcDkI);
						if (ZdiFGkPdAMJkbQxVfDcpRNYsDOHg == null || !ZdiFGkPdAMJkbQxVfDcpRNYsDOHg.userAssignable)
						{
							return false;
						}
						if (frzjmrXmRrbezdHVpOvpDmsytcCeb)
						{
							KhhuYSsZKoVpYiRtmyreqUjoItre = userData.SortedActionIdsInCategory(ZdiFGkPdAMJkbQxVfDcpRNYsDOHg.id).GetEnumerator();
							htbersTCIkGovGgnGnkDpmkluTQr = -3;
							goto IL_00e4;
						}
						wXytujDquvBltsTDIQixzReGlGwN = 0;
						goto IL_0165;
					case 1:
						htbersTCIkGovGgnGnkDpmkluTQr = -3;
						goto IL_00e4;
					case 2:
						{
							htbersTCIkGovGgnGnkDpmkluTQr = -1;
							goto IL_0153;
						}
						IL_00e4:
						while (KhhuYSsZKoVpYiRtmyreqUjoItre.MoveNext())
						{
							int current = KhhuYSsZKoVpYiRtmyreqUjoItre.Current;
							InputAction actionById = userData.GetActionById(current);
							if (actionById != null && actionById.userAssignable)
							{
								pwsyPzrBQzPAUlCoNbildkmidpAGA = actionById;
								htbersTCIkGovGgnGnkDpmkluTQr = 1;
								return true;
							}
						}
						oRolUBIcLwtzJvqAZWFBybeVCTAZ();
						KhhuYSsZKoVpYiRtmyreqUjoItre = null;
						break;
						IL_0153:
						wXytujDquvBltsTDIQixzReGlGwN++;
						goto IL_0165;
						IL_0165:
						if (wXytujDquvBltsTDIQixzReGlGwN >= userData.kKnhPVRioLmZoBOgUQuQYtoEHyTc.Count)
						{
							break;
						}
						inputAction = userData.kKnhPVRioLmZoBOgUQuQYtoEHyTc[wXytujDquvBltsTDIQixzReGlGwN];
						if (inputAction.categoryId == ZdiFGkPdAMJkbQxVfDcpRNYsDOHg.id && inputAction.userAssignable)
						{
							pwsyPzrBQzPAUlCoNbildkmidpAGA = inputAction;
							htbersTCIkGovGgnGnkDpmkluTQr = 2;
							return true;
						}
						goto IL_0153;
					}
					return false;
				}
				catch
				{
					//try-fault
					((IDisposable)this).Dispose();
					throw;
				}
			}

			bool IEnumerator.MoveNext()
			{
				//ILSpy generated this explicit interface implementation from .override directive in MoveNext
				return this.MoveNext();
			}

			private void oRolUBIcLwtzJvqAZWFBybeVCTAZ()
			{
				htbersTCIkGovGgnGnkDpmkluTQr = -1;
				if (KhhuYSsZKoVpYiRtmyreqUjoItre != null)
				{
					KhhuYSsZKoVpYiRtmyreqUjoItre.Dispose();
				}
			}

			[DebuggerHidden]
			void IEnumerator.Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator<InputAction> IEnumerable<InputAction>.GetEnumerator()
			{
				UFJjEpfRQRajlIAkCyEjiApMAVhic uFJjEpfRQRajlIAkCyEjiApMAVhic;
				if (htbersTCIkGovGgnGnkDpmkluTQr == -2 && ANynxZtpnppDVqTEYjOMxHUiEpAm == Environment.CurrentManagedThreadId)
				{
					htbersTCIkGovGgnGnkDpmkluTQr = 0;
					uFJjEpfRQRajlIAkCyEjiApMAVhic = this;
				}
				else
				{
					uFJjEpfRQRajlIAkCyEjiApMAVhic = new UFJjEpfRQRajlIAkCyEjiApMAVhic(0);
					uFJjEpfRQRajlIAkCyEjiApMAVhic.fYYASVgjcCMCPokAqbwmwtsibAjdb = fYYASVgjcCMCPokAqbwmwtsibAjdb;
				}
				uFJjEpfRQRajlIAkCyEjiApMAVhic.LFxkFIHTHirxwSZxOMpAYdzCcDkI = MKilOVFNRMGXIszHiwQCBQwwSZLP;
				uFJjEpfRQRajlIAkCyEjiApMAVhic.frzjmrXmRrbezdHVpOvpDmsytcCeb = mKLCOIVBfwDdxAhUNSPmfZpfAqzpA;
				return uFJjEpfRQRajlIAkCyEjiApMAVhic;
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return ((IEnumerable<InputAction>)this).GetEnumerator();
			}
		}

		private sealed class cIinLrYQvIcRxvUNjkZNGpchLqLN : IEnumerable<InputMapCategory>, IEnumerable, IEnumerator<InputMapCategory>, IEnumerator, IDisposable
		{
			private int dktzftNZxdupHHiJsYeSvTGPbcdk;

			private InputMapCategory dRiEYeGPKemvJPJUDBbulugzwYsu;

			private int OICfEwnvAUEfDKsntyubnpDCfLFd;

			private string mUNciMegzZJJRTMHoTuSwhqMnUGC;

			public string waibivfrefPnXISbzXXaPxinIzSIA;

			public UserData calGZgMyyfubjNBsabMkyBkDGeyq;

			private int gvHKgXLctcaozxGamsBBGfnoIbQA;

			InputMapCategory IEnumerator<InputMapCategory>.Current
			{
				[DebuggerHidden]
				get
				{
					return dRiEYeGPKemvJPJUDBbulugzwYsu;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return dRiEYeGPKemvJPJUDBbulugzwYsu;
				}
			}

			[DebuggerHidden]
			public cIinLrYQvIcRxvUNjkZNGpchLqLN(int P_0)
			{
				dktzftNZxdupHHiJsYeSvTGPbcdk = P_0;
				OICfEwnvAUEfDKsntyubnpDCfLFd = Environment.CurrentManagedThreadId;
			}

			[DebuggerHidden]
			void IDisposable.Dispose()
			{
				dktzftNZxdupHHiJsYeSvTGPbcdk = -2;
			}

			private bool MoveNext()
			{
				int num = dktzftNZxdupHHiJsYeSvTGPbcdk;
				UserData userData = calGZgMyyfubjNBsabMkyBkDGeyq;
				if (num != 0)
				{
					if (num != 1)
					{
						return false;
					}
					dktzftNZxdupHHiJsYeSvTGPbcdk = -1;
					goto IL_00b3;
				}
				dktzftNZxdupHHiJsYeSvTGPbcdk = -1;
				if (mUNciMegzZJJRTMHoTuSwhqMnUGC == null || mUNciMegzZJJRTMHoTuSwhqMnUGC == string.Empty)
				{
					return false;
				}
				if (userData.mapCategories == null)
				{
					return false;
				}
				gvHKgXLctcaozxGamsBBGfnoIbQA = 0;
				goto IL_00c3;
				IL_00c3:
				if (gvHKgXLctcaozxGamsBBGfnoIbQA < userData.mapCategories.Count)
				{
					if (userData.mapCategories[gvHKgXLctcaozxGamsBBGfnoIbQA].userAssignable && userData.mapCategories[gvHKgXLctcaozxGamsBBGfnoIbQA].tag.Equals(mUNciMegzZJJRTMHoTuSwhqMnUGC, StringComparison.OrdinalIgnoreCase))
					{
						dRiEYeGPKemvJPJUDBbulugzwYsu = userData.mapCategories[gvHKgXLctcaozxGamsBBGfnoIbQA];
						dktzftNZxdupHHiJsYeSvTGPbcdk = 1;
						return true;
					}
					goto IL_00b3;
				}
				return false;
				IL_00b3:
				gvHKgXLctcaozxGamsBBGfnoIbQA++;
				goto IL_00c3;
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
			IEnumerator<InputMapCategory> IEnumerable<InputMapCategory>.GetEnumerator()
			{
				cIinLrYQvIcRxvUNjkZNGpchLqLN cIinLrYQvIcRxvUNjkZNGpchLqLN2;
				if (dktzftNZxdupHHiJsYeSvTGPbcdk == -2 && OICfEwnvAUEfDKsntyubnpDCfLFd == Environment.CurrentManagedThreadId)
				{
					dktzftNZxdupHHiJsYeSvTGPbcdk = 0;
					cIinLrYQvIcRxvUNjkZNGpchLqLN2 = this;
				}
				else
				{
					cIinLrYQvIcRxvUNjkZNGpchLqLN2 = new cIinLrYQvIcRxvUNjkZNGpchLqLN(0);
					cIinLrYQvIcRxvUNjkZNGpchLqLN2.calGZgMyyfubjNBsabMkyBkDGeyq = calGZgMyyfubjNBsabMkyBkDGeyq;
				}
				cIinLrYQvIcRxvUNjkZNGpchLqLN2.mUNciMegzZJJRTMHoTuSwhqMnUGC = waibivfrefPnXISbzXXaPxinIzSIA;
				return cIinLrYQvIcRxvUNjkZNGpchLqLN2;
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return ((IEnumerable<InputMapCategory>)this).GetEnumerator();
			}
		}

		private sealed class LDXeXNfHFuNKGnEoNqoneizeSsGYb : IEnumerable<InputCategory>, IEnumerable, IEnumerator<InputCategory>, IEnumerator, IDisposable
		{
			private int walCjInJmSjpBBjNAIyZfZwTThNjb;

			private InputCategory mGPVLixgawZvDvbbDOlTHmuXQGoP;

			private int mjidrdtSUUZlLAeNUVGMaDLQILYFA;

			public UserData TldUFNPksjFWYqQXpJtsHEyYPdvK;

			private int YNlZpqKajKwnRtoccEwAVdjkaVks;

			InputCategory IEnumerator<InputCategory>.Current
			{
				[DebuggerHidden]
				get
				{
					return mGPVLixgawZvDvbbDOlTHmuXQGoP;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return mGPVLixgawZvDvbbDOlTHmuXQGoP;
				}
			}

			[DebuggerHidden]
			public LDXeXNfHFuNKGnEoNqoneizeSsGYb(int P_0)
			{
				walCjInJmSjpBBjNAIyZfZwTThNjb = P_0;
				mjidrdtSUUZlLAeNUVGMaDLQILYFA = Environment.CurrentManagedThreadId;
			}

			[DebuggerHidden]
			void IDisposable.Dispose()
			{
				walCjInJmSjpBBjNAIyZfZwTThNjb = -2;
			}

			private bool MoveNext()
			{
				int num = walCjInJmSjpBBjNAIyZfZwTThNjb;
				UserData tldUFNPksjFWYqQXpJtsHEyYPdvK = TldUFNPksjFWYqQXpJtsHEyYPdvK;
				if (num != 0)
				{
					if (num != 1)
					{
						return false;
					}
					walCjInJmSjpBBjNAIyZfZwTThNjb = -1;
					goto IL_0070;
				}
				walCjInJmSjpBBjNAIyZfZwTThNjb = -1;
				if (tldUFNPksjFWYqQXpJtsHEyYPdvK.actionCategories == null)
				{
					return false;
				}
				YNlZpqKajKwnRtoccEwAVdjkaVks = 0;
				goto IL_0080;
				IL_0080:
				if (YNlZpqKajKwnRtoccEwAVdjkaVks < tldUFNPksjFWYqQXpJtsHEyYPdvK.actionCategories.Count)
				{
					if (tldUFNPksjFWYqQXpJtsHEyYPdvK.actionCategories[YNlZpqKajKwnRtoccEwAVdjkaVks].userAssignable)
					{
						mGPVLixgawZvDvbbDOlTHmuXQGoP = tldUFNPksjFWYqQXpJtsHEyYPdvK.actionCategories[YNlZpqKajKwnRtoccEwAVdjkaVks];
						walCjInJmSjpBBjNAIyZfZwTThNjb = 1;
						return true;
					}
					goto IL_0070;
				}
				return false;
				IL_0070:
				YNlZpqKajKwnRtoccEwAVdjkaVks++;
				goto IL_0080;
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
			IEnumerator<InputCategory> IEnumerable<InputCategory>.GetEnumerator()
			{
				LDXeXNfHFuNKGnEoNqoneizeSsGYb lDXeXNfHFuNKGnEoNqoneizeSsGYb;
				if (walCjInJmSjpBBjNAIyZfZwTThNjb == -2 && mjidrdtSUUZlLAeNUVGMaDLQILYFA == Environment.CurrentManagedThreadId)
				{
					walCjInJmSjpBBjNAIyZfZwTThNjb = 0;
					lDXeXNfHFuNKGnEoNqoneizeSsGYb = this;
				}
				else
				{
					lDXeXNfHFuNKGnEoNqoneizeSsGYb = new LDXeXNfHFuNKGnEoNqoneizeSsGYb(0);
					lDXeXNfHFuNKGnEoNqoneizeSsGYb.TldUFNPksjFWYqQXpJtsHEyYPdvK = TldUFNPksjFWYqQXpJtsHEyYPdvK;
				}
				return lDXeXNfHFuNKGnEoNqoneizeSsGYb;
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return ((IEnumerable<InputCategory>)this).GetEnumerator();
			}
		}

		private sealed class mGTbkAlmxdViUtFvLXrRjEdGmauB : IEnumerable<InputAction>, IEnumerable, IEnumerator<InputAction>, IEnumerator, IDisposable
		{
			private int ohleHOgeXnhmfvQlNvgxKrLUKECD;

			private InputAction ZCZhEzfnzqtlvVbQzlWIfmOKjgjAA;

			private int xnNgVMPHvDfzuPZawJdDaiIZRHHP;

			public UserData dALaaHMvfdgDSVgHzmAUAhVMuCST;

			private int huJnLfbUSsdYcrEuAlNEdDQXtDZC;

			InputAction IEnumerator<InputAction>.Current
			{
				[DebuggerHidden]
				get
				{
					return ZCZhEzfnzqtlvVbQzlWIfmOKjgjAA;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return ZCZhEzfnzqtlvVbQzlWIfmOKjgjAA;
				}
			}

			[DebuggerHidden]
			public mGTbkAlmxdViUtFvLXrRjEdGmauB(int P_0)
			{
				ohleHOgeXnhmfvQlNvgxKrLUKECD = P_0;
				xnNgVMPHvDfzuPZawJdDaiIZRHHP = Environment.CurrentManagedThreadId;
			}

			[DebuggerHidden]
			void IDisposable.Dispose()
			{
				ohleHOgeXnhmfvQlNvgxKrLUKECD = -2;
			}

			private bool MoveNext()
			{
				int num = ohleHOgeXnhmfvQlNvgxKrLUKECD;
				UserData userData = dALaaHMvfdgDSVgHzmAUAhVMuCST;
				if (num != 0)
				{
					if (num != 1)
					{
						return false;
					}
					ohleHOgeXnhmfvQlNvgxKrLUKECD = -1;
					goto IL_007a;
				}
				ohleHOgeXnhmfvQlNvgxKrLUKECD = -1;
				if (userData.kKnhPVRioLmZoBOgUQuQYtoEHyTc == null)
				{
					return false;
				}
				huJnLfbUSsdYcrEuAlNEdDQXtDZC = 0;
				goto IL_008c;
				IL_008c:
				if (huJnLfbUSsdYcrEuAlNEdDQXtDZC < userData.kKnhPVRioLmZoBOgUQuQYtoEHyTc.Count)
				{
					InputAction inputAction = userData.kKnhPVRioLmZoBOgUQuQYtoEHyTc[huJnLfbUSsdYcrEuAlNEdDQXtDZC];
					InputCategory actionCategoryById = userData.GetActionCategoryById(inputAction.categoryId);
					if (actionCategoryById != null && actionCategoryById.userAssignable && inputAction.userAssignable)
					{
						ZCZhEzfnzqtlvVbQzlWIfmOKjgjAA = inputAction;
						ohleHOgeXnhmfvQlNvgxKrLUKECD = 1;
						return true;
					}
					goto IL_007a;
				}
				return false;
				IL_007a:
				huJnLfbUSsdYcrEuAlNEdDQXtDZC++;
				goto IL_008c;
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
			IEnumerator<InputAction> IEnumerable<InputAction>.GetEnumerator()
			{
				mGTbkAlmxdViUtFvLXrRjEdGmauB mGTbkAlmxdViUtFvLXrRjEdGmauB2;
				if (ohleHOgeXnhmfvQlNvgxKrLUKECD == -2 && xnNgVMPHvDfzuPZawJdDaiIZRHHP == Environment.CurrentManagedThreadId)
				{
					ohleHOgeXnhmfvQlNvgxKrLUKECD = 0;
					mGTbkAlmxdViUtFvLXrRjEdGmauB2 = this;
				}
				else
				{
					mGTbkAlmxdViUtFvLXrRjEdGmauB2 = new mGTbkAlmxdViUtFvLXrRjEdGmauB(0);
					mGTbkAlmxdViUtFvLXrRjEdGmauB2.dALaaHMvfdgDSVgHzmAUAhVMuCST = dALaaHMvfdgDSVgHzmAUAhVMuCST;
				}
				return mGTbkAlmxdViUtFvLXrRjEdGmauB2;
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return ((IEnumerable<InputAction>)this).GetEnumerator();
			}
		}

		private sealed class mSESvsTakPNhpWXDxHJFLnSxDDdp : IEnumerable<InputMapCategory>, IEnumerable, IEnumerator<InputMapCategory>, IEnumerator, IDisposable
		{
			private int BZjdUPbuFTCYjLzDDqDzkbZDabSMA;

			private InputMapCategory mlpAqAqOjghZJQzzhdRABxQatGQ;

			private int kQJMUntpOfFEuasMgkGFHamGoOySb;

			public UserData tZpVXOdSNXQCVbRxymBQPojHsrqQ;

			private int KLwPzKCGoDKbthSTnadHMAXhCQK;

			InputMapCategory IEnumerator<InputMapCategory>.Current
			{
				[DebuggerHidden]
				get
				{
					return mlpAqAqOjghZJQzzhdRABxQatGQ;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return mlpAqAqOjghZJQzzhdRABxQatGQ;
				}
			}

			[DebuggerHidden]
			public mSESvsTakPNhpWXDxHJFLnSxDDdp(int P_0)
			{
				BZjdUPbuFTCYjLzDDqDzkbZDabSMA = P_0;
				kQJMUntpOfFEuasMgkGFHamGoOySb = Environment.CurrentManagedThreadId;
			}

			[DebuggerHidden]
			void IDisposable.Dispose()
			{
				BZjdUPbuFTCYjLzDDqDzkbZDabSMA = -2;
			}

			private bool MoveNext()
			{
				int bZjdUPbuFTCYjLzDDqDzkbZDabSMA = BZjdUPbuFTCYjLzDDqDzkbZDabSMA;
				UserData userData = tZpVXOdSNXQCVbRxymBQPojHsrqQ;
				if (bZjdUPbuFTCYjLzDDqDzkbZDabSMA != 0)
				{
					if (bZjdUPbuFTCYjLzDDqDzkbZDabSMA != 1)
					{
						return false;
					}
					BZjdUPbuFTCYjLzDDqDzkbZDabSMA = -1;
					goto IL_0070;
				}
				BZjdUPbuFTCYjLzDDqDzkbZDabSMA = -1;
				if (userData.mapCategories == null)
				{
					return false;
				}
				KLwPzKCGoDKbthSTnadHMAXhCQK = 0;
				goto IL_0080;
				IL_0080:
				if (KLwPzKCGoDKbthSTnadHMAXhCQK < userData.mapCategories.Count)
				{
					if (userData.mapCategories[KLwPzKCGoDKbthSTnadHMAXhCQK].userAssignable)
					{
						mlpAqAqOjghZJQzzhdRABxQatGQ = userData.mapCategories[KLwPzKCGoDKbthSTnadHMAXhCQK];
						BZjdUPbuFTCYjLzDDqDzkbZDabSMA = 1;
						return true;
					}
					goto IL_0070;
				}
				return false;
				IL_0070:
				KLwPzKCGoDKbthSTnadHMAXhCQK++;
				goto IL_0080;
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
			IEnumerator<InputMapCategory> IEnumerable<InputMapCategory>.GetEnumerator()
			{
				mSESvsTakPNhpWXDxHJFLnSxDDdp mSESvsTakPNhpWXDxHJFLnSxDDdp2;
				if (BZjdUPbuFTCYjLzDDqDzkbZDabSMA == -2 && kQJMUntpOfFEuasMgkGFHamGoOySb == Environment.CurrentManagedThreadId)
				{
					BZjdUPbuFTCYjLzDDqDzkbZDabSMA = 0;
					mSESvsTakPNhpWXDxHJFLnSxDDdp2 = this;
				}
				else
				{
					mSESvsTakPNhpWXDxHJFLnSxDDdp2 = new mSESvsTakPNhpWXDxHJFLnSxDDdp(0);
					mSESvsTakPNhpWXDxHJFLnSxDDdp2.tZpVXOdSNXQCVbRxymBQPojHsrqQ = tZpVXOdSNXQCVbRxymBQPojHsrqQ;
				}
				return mSESvsTakPNhpWXDxHJFLnSxDDdp2;
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return ((IEnumerable<InputMapCategory>)this).GetEnumerator();
			}
		}

		[SerializeField]
		[CustomObfuscation(rename = false)]
		private ConfigVars configVars = new ConfigVars();

		[SerializeField]
		[CustomObfuscation(rename = false)]
		private List<Player_Editor> players = new List<Player_Editor>();

		[SerializeField]
		[CustomObfuscation(rename = false)]
		private List<InputAction> actions = new List<InputAction>();

		[SerializeField]
		[CustomObfuscation(rename = false)]
		private List<InputActionCategory> actionCategories = new List<InputActionCategory>();

		[SerializeField]
		[CustomObfuscation(rename = false)]
		private ActionCategoryMap actionCategoryMap = new ActionCategoryMap();

		[SerializeField]
		[CustomObfuscation(rename = false)]
		private List<InputBehavior> inputBehaviors = new List<InputBehavior>();

		[SerializeField]
		[CustomObfuscation(rename = false)]
		private List<InputMapCategory> mapCategories = new List<InputMapCategory>();

		[SerializeField]
		[CustomObfuscation(rename = false)]
		private List<InputLayout> joystickLayouts = new List<InputLayout>();

		[SerializeField]
		[CustomObfuscation(rename = false)]
		private List<InputLayout> keyboardLayouts = new List<InputLayout>();

		[SerializeField]
		[CustomObfuscation(rename = false)]
		private List<InputLayout> mouseLayouts = new List<InputLayout>();

		[SerializeField]
		[CustomObfuscation(rename = false)]
		private List<InputLayout> customControllerLayouts = new List<InputLayout>();

		[SerializeField]
		[CustomObfuscation(rename = false)]
		private List<ControllerMap_Editor> joystickMaps = new List<ControllerMap_Editor>();

		[SerializeField]
		[CustomObfuscation(rename = false)]
		private List<ControllerMap_Editor> keyboardMaps = new List<ControllerMap_Editor>();

		[SerializeField]
		[CustomObfuscation(rename = false)]
		private List<ControllerMap_Editor> mouseMaps = new List<ControllerMap_Editor>();

		[SerializeField]
		[CustomObfuscation(rename = false)]
		private List<ControllerMap_Editor> customControllerMaps = new List<ControllerMap_Editor>();

		[SerializeField]
		[CustomObfuscation(rename = false)]
		private List<CustomController_Editor> customControllers = new List<CustomController_Editor>();

		[SerializeField]
		[CustomObfuscation(rename = false)]
		private List<ControllerMapLayoutManager_RuleSet_Editor> controllerMapLayoutManagerRuleSets = new List<ControllerMapLayoutManager_RuleSet_Editor>();

		[SerializeField]
		[CustomObfuscation(rename = false)]
		private List<ControllerMapEnabler_RuleSet_Editor> controllerMapEnablerRuleSets = new List<ControllerMapEnabler_RuleSet_Editor>();

		[NonSerialized]
		private List<InputAction> gcsncNCaRRPuhDcaaLiikvmhVRaG;

		[NonSerialized]
		private bool wFOAzFxiXEXdSCuxaPrTUvxCQQzw;

		[CompilerGenerated]
		private IList<Player_Editor> _003CPlayers_readOnly_003Ek__BackingField;

		[CompilerGenerated]
		private IList<InputAction> _003CActions_readOnly_003Ek__BackingField;

		[CompilerGenerated]
		private IList<InputCategory> _003CActionCategories_readOnly_003Ek__BackingField;

		[CompilerGenerated]
		private IList<InputBehavior> _003CInputBehaviors_readOnly_003Ek__BackingField;

		[CompilerGenerated]
		private IList<InputMapCategory> _003CMapCategories_readOnly_003Ek__BackingField;

		[CompilerGenerated]
		private IList<InputLayout> _003CJoystickLayouts_readOnly_003Ek__BackingField;

		[CompilerGenerated]
		private IList<InputLayout> _003CKeyboardLayouts_readOnly_003Ek__BackingField;

		[CompilerGenerated]
		private IList<InputLayout> _003CMouseLayouts_readOnly_003Ek__BackingField;

		[CompilerGenerated]
		private IList<InputLayout> _003CCustomControllerLayouts_readOnly_003Ek__BackingField;

		[CompilerGenerated]
		private IList<ControllerMap_Editor> _003CJoystickMaps_readOnly_003Ek__BackingField;

		[CompilerGenerated]
		private IList<ControllerMap_Editor> _003CKeyboardMaps_readOnly_003Ek__BackingField;

		[CompilerGenerated]
		private IList<ControllerMap_Editor> _003CMouseMaps_readOnly_003Ek__BackingField;

		[CompilerGenerated]
		private IList<ControllerMap_Editor> _003CCustomControllerMaps_readOnly_003Ek__BackingField;

		[CompilerGenerated]
		private IList<ControllerMapLayoutManager_RuleSet_Editor> _003CControllerMapLayoutManagerRuleSets_readOnly_003Ek__BackingField;

		[CompilerGenerated]
		private IList<ControllerMapEnabler_RuleSet_Editor> _003CControllerMapEnablerRuleSets_readOnly_003Ek__BackingField;

		[SerializeField]
		[CustomObfuscation(rename = false)]
		private int playerIdCounter;

		[SerializeField]
		[CustomObfuscation(rename = false)]
		private int actionIdCounter;

		[SerializeField]
		[CustomObfuscation(rename = false)]
		private int actionCategoryIdCounter;

		[SerializeField]
		[CustomObfuscation(rename = false)]
		private int inputBehaviorIdCounter;

		[SerializeField]
		[CustomObfuscation(rename = false)]
		private int mapCategoryIdCounter;

		[SerializeField]
		[CustomObfuscation(rename = false)]
		private int joystickLayoutIdCounter;

		[SerializeField]
		[CustomObfuscation(rename = false)]
		private int keyboardLayoutIdCounter;

		[SerializeField]
		[CustomObfuscation(rename = false)]
		private int mouseLayoutIdCounter;

		[SerializeField]
		[CustomObfuscation(rename = false)]
		private int customControllerLayoutIdCounter;

		[SerializeField]
		[CustomObfuscation(rename = false)]
		private int joystickMapIdCounter;

		[SerializeField]
		[CustomObfuscation(rename = false)]
		private int keyboardMapIdCounter;

		[SerializeField]
		[CustomObfuscation(rename = false)]
		private int mouseMapIdCounter;

		[SerializeField]
		[CustomObfuscation(rename = false)]
		private int customControllerMapIdCounter;

		[SerializeField]
		[CustomObfuscation(rename = false)]
		private int customControllerIdCounter;

		[SerializeField]
		[CustomObfuscation(rename = false)]
		private int controllerMapLayoutManagerSetIdCounter;

		[SerializeField]
		[CustomObfuscation(rename = false)]
		private int controllerMapEnablerSetIdCounter;

		private Func<int, bool> containsActionDelegate;

		internal IList<Player_Editor> JIZWSkNLBmbxmgqTylFTFDyIHEkLB
		{
			[CompilerGenerated]
			get
			{
				return _003CPlayers_readOnly_003Ek__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				_003CPlayers_readOnly_003Ek__BackingField = list;
			}
		}

		internal IList<InputAction> UaeAwHiobbzFazhOCwHXZiSScIWDA
		{
			[CompilerGenerated]
			get
			{
				return _003CActions_readOnly_003Ek__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				_003CActions_readOnly_003Ek__BackingField = list;
			}
		}

		internal IList<InputCategory> IMddRaBENAmGfZWTNpvTlDLkNkHU
		{
			[CompilerGenerated]
			get
			{
				return _003CActionCategories_readOnly_003Ek__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				_003CActionCategories_readOnly_003Ek__BackingField = list;
			}
		}

		internal IList<InputBehavior> JMDCCNNbMZQKDbeMVYRPAhfYnALf
		{
			[CompilerGenerated]
			get
			{
				return _003CInputBehaviors_readOnly_003Ek__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				_003CInputBehaviors_readOnly_003Ek__BackingField = list;
			}
		}

		internal IList<InputMapCategory> psnMotVTaWJqzAFWxfjoaAVNVSFMA
		{
			[CompilerGenerated]
			get
			{
				return _003CMapCategories_readOnly_003Ek__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				_003CMapCategories_readOnly_003Ek__BackingField = list;
			}
		}

		internal IList<InputLayout> jRJbARDRHMLcZQovULcpACLAoSlYA
		{
			[CompilerGenerated]
			get
			{
				return _003CJoystickLayouts_readOnly_003Ek__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				_003CJoystickLayouts_readOnly_003Ek__BackingField = list;
			}
		}

		internal IList<InputLayout> DTuaINDJtEOtldDSZdZpfyTkmIIcA
		{
			[CompilerGenerated]
			get
			{
				return _003CKeyboardLayouts_readOnly_003Ek__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				_003CKeyboardLayouts_readOnly_003Ek__BackingField = list;
			}
		}

		internal IList<InputLayout> CYgKlfpQSrdTwHyRItikkHzdxbipA
		{
			[CompilerGenerated]
			get
			{
				return _003CMouseLayouts_readOnly_003Ek__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				_003CMouseLayouts_readOnly_003Ek__BackingField = list;
			}
		}

		internal IList<InputLayout> XzpJaGfpJYXkiIKQPTSLavfAQgiO
		{
			[CompilerGenerated]
			get
			{
				return _003CCustomControllerLayouts_readOnly_003Ek__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				_003CCustomControllerLayouts_readOnly_003Ek__BackingField = list;
			}
		}

		internal IList<ControllerMap_Editor> CSGjkgmFTTujzxKbxdyMdsSJqKezA
		{
			[CompilerGenerated]
			get
			{
				return _003CJoystickMaps_readOnly_003Ek__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				_003CJoystickMaps_readOnly_003Ek__BackingField = list;
			}
		}

		internal IList<ControllerMap_Editor> iLLWPQayDOVNpTuxTQEPyqnacxkX
		{
			[CompilerGenerated]
			get
			{
				return _003CKeyboardMaps_readOnly_003Ek__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				_003CKeyboardMaps_readOnly_003Ek__BackingField = list;
			}
		}

		internal IList<ControllerMap_Editor> ZGjsDKrfpxjZadgHDlmkGNtfcqerA
		{
			[CompilerGenerated]
			get
			{
				return _003CMouseMaps_readOnly_003Ek__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				_003CMouseMaps_readOnly_003Ek__BackingField = list;
			}
		}

		internal IList<ControllerMap_Editor> ZqhatfesYQmKBXVxJOmdkNywSXdj
		{
			[CompilerGenerated]
			get
			{
				return _003CCustomControllerMaps_readOnly_003Ek__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				_003CCustomControllerMaps_readOnly_003Ek__BackingField = list;
			}
		}

		internal IList<ControllerMapLayoutManager_RuleSet_Editor> hORnDYsESUNPDVMXKkCRoOvcfCbKA
		{
			[CompilerGenerated]
			get
			{
				return _003CControllerMapLayoutManagerRuleSets_readOnly_003Ek__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				_003CControllerMapLayoutManagerRuleSets_readOnly_003Ek__BackingField = list;
			}
		}

		internal IList<ControllerMapEnabler_RuleSet_Editor> UoIdkhzSDqqeiTjSUanOgCAIGtol
		{
			[CompilerGenerated]
			get
			{
				return _003CControllerMapEnablerRuleSets_readOnly_003Ek__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				_003CControllerMapEnablerRuleSets_readOnly_003Ek__BackingField = list;
			}
		}

		public ConfigVars ConfigVars => configVars;

		internal IEnumerable<InputMapCategory> rSknYcNUeqOIaSjQkNSDuOVKthIp
		{
			[IteratorStateMachine(typeof(mSESvsTakPNhpWXDxHJFLnSxDDdp))]
			get
			{
				return new mSESvsTakPNhpWXDxHJFLnSxDDdp(-2)
				{
					tZpVXOdSNXQCVbRxymBQPojHsrqQ = this
				};
			}
		}

		internal IEnumerable<InputCategory> TYBFYKfZaqmdJxUpoyQWiXdebyxY
		{
			[IteratorStateMachine(typeof(LDXeXNfHFuNKGnEoNqoneizeSsGYb))]
			get
			{
				return new LDXeXNfHFuNKGnEoNqoneizeSsGYb(-2)
				{
					TldUFNPksjFWYqQXpJtsHEyYPdvK = this
				};
			}
		}

		internal IEnumerable<InputAction> FRquySFZeZeRlsVwIqFntUDlCqSQ
		{
			[IteratorStateMachine(typeof(mGTbkAlmxdViUtFvLXrRjEdGmauB))]
			get
			{
				return new mGTbkAlmxdViUtFvLXrRjEdGmauB(-2)
				{
					dALaaHMvfdgDSVgHzmAUAhVMuCST = this
				};
			}
		}

		public int playerCount
		{
			get
			{
				if (players == null)
				{
					return 0;
				}
				return players.Count;
			}
		}

		private List<InputAction> kKnhPVRioLmZoBOgUQuQYtoEHyTc
		{
			get
			{
				if (!ReInput.isReady)
				{
					return actions;
				}
				return gcsncNCaRRPuhDcaaLiikvmhVRaG;
			}
		}

		[IteratorStateMachine(typeof(pOXeQFNrEvVHhtOijUteGFoWlTLf))]
		internal IEnumerable<InputMapCategory> zTfTxSSvYAQwXhDNxuTubihtRbQV(string P_0)
		{
			return new pOXeQFNrEvVHhtOijUteGFoWlTLf(-2)
			{
				NSoIMErurYsXosjonekKwRissrXg = this,
				BEPZmHRHVZnANLNaqoKOZqjAnTot = P_0
			};
		}

		[IteratorStateMachine(typeof(cIinLrYQvIcRxvUNjkZNGpchLqLN))]
		internal IEnumerable<InputMapCategory> qetRckIAzdanwcZBHJQZsfJLaiEr(string P_0)
		{
			return new cIinLrYQvIcRxvUNjkZNGpchLqLN(-2)
			{
				calGZgMyyfubjNBsabMkyBkDGeyq = this,
				waibivfrefPnXISbzXXaPxinIzSIA = P_0
			};
		}

		[IteratorStateMachine(typeof(akvZAFMlvyRqpyFGWBfzhqTAqNBW))]
		internal IEnumerable<InputCategory> AmZSnNbIgDdsTBtJwVYxtOhCqjqWA(string P_0)
		{
			return new akvZAFMlvyRqpyFGWBfzhqTAqNBW(-2)
			{
				LIRpTWeTbeljrPkXBVZYFKdcqDsg = this,
				UbgaNrIHaXshgcASkBGlNRXEfvASd = P_0
			};
		}

		[IteratorStateMachine(typeof(cXgAzmbhCvUoaISnKebadELfsZTn))]
		internal IEnumerable<InputCategory> mFiumTHjIeYSiWEaMUJPnTMDmsnW(string P_0)
		{
			return new cXgAzmbhCvUoaISnKebadELfsZTn(-2)
			{
				VLDANBVcmbWJgcFVGPdynfiXsHTP = this,
				BrGjdsJqIJskSEfmJMcLoFfgbWjX = P_0
			};
		}

		[IteratorStateMachine(typeof(jeRplBHJbrkwrSwymxfoACSRlXW))]
		internal IEnumerable<InputAction> sQgquvUbVxkrQbRmtQrCUAPgmcBS(int P_0, bool P_1)
		{
			return new jeRplBHJbrkwrSwymxfoACSRlXW(-2)
			{
				FtZMZfXcvfdAaBYOPUSOrhGojGso = this,
				mFLELYQJrDtFbKlOflLmrmYHxUAS = P_0,
				YuoiGuqpZOEcSftXDZdIPeIihzVEb = P_1
			};
		}

		[IteratorStateMachine(typeof(WFCIjqjOXnvEClZabpnEqtsweOFFA))]
		internal IEnumerable<InputAction> CMcipwivHVqDObFtqItXsOoadtMc(string P_0, bool P_1)
		{
			return new WFCIjqjOXnvEClZabpnEqtsweOFFA(-2)
			{
				WcsyAArQAbBQPAUIaFXMKsJUhUoMA = this,
				bjoPWLQITyozDBcytDERwljUKvDf = P_0,
				JfnrqKHHFCtabfdDvEBCVReqjpHz = P_1
			};
		}

		[IteratorStateMachine(typeof(JRJlbejvcpfzSxesysQmbhHdgNWgA))]
		internal IEnumerable<InputAction> zGCzlHzTlymQZPvFVEcaevqYHEjW(string P_0)
		{
			return new JRJlbejvcpfzSxesysQmbhHdgNWgA(-2)
			{
				bexIVQKTHyptIyixOLQNhtcScAxw = this,
				wyGBiLVuEkCrSxBbgeIpdzvCBANeA = P_0
			};
		}

		[IteratorStateMachine(typeof(HZZNgIGcRRXwvNaPnoipqlNnoEUo))]
		internal IEnumerable<InputAction> XZATmGlfJcOggZqyTEyTzCpkGCDbA(int P_0, bool P_1)
		{
			return new HZZNgIGcRRXwvNaPnoipqlNnoEUo(-2)
			{
				mioHKMTVrYgbeQtpKHZlRGysRNjT = this,
				HulMNgeGtzYDuZjrKrEgEnRlKKpA = P_0,
				nqNDditaByiEeNiCDAztfqrofcAiA = P_1
			};
		}

		[IteratorStateMachine(typeof(UFJjEpfRQRajlIAkCyEjiApMAVhic))]
		internal IEnumerable<InputAction> aWEziYZsMPytfEZDKeLsozQMNMUA(string P_0, bool P_1)
		{
			return new UFJjEpfRQRajlIAkCyEjiApMAVhic(-2)
			{
				fYYASVgjcCMCPokAqbwmwtsibAjdb = this,
				MKilOVFNRMGXIszHiwQCBQwwSZLP = P_0,
				mKLCOIVBfwDdxAhUNSPmfZpfAqzpA = P_1
			};
		}

		public UserData()
			: this(true)
		{
		}

		private UserData(bool P_0)
		{
			if (P_0)
			{
				configVars.updateLoop = UpdateLoopSetting.Update;
				configVars.defaultJoystickAxis2DDeadZoneType = DeadZone2DType.Radial;
				configVars.defaultJoystickAxis2DSensitivityType = AxisSensitivity2DType.Radial;
				Player_Editor player_Editor = oTvzqdfQFNEeFreRthKtbncVjSOD();
				player_Editor.name = "System";
				player_Editor.descriptiveName = player_Editor.name;
				player_Editor.key = "system_player";
				player_Editor.id = 9999999;
				player_Editor.startPlaying = true;
				player_Editor.assignMouseOnStart = true;
				player_Editor.assignKeyboardOnStart = true;
				player_Editor.excludeFromControllerAutoAssignment = true;
				players.Add(player_Editor);
				InputActionCategory inputActionCategory = MukaBDbnShpYGcdoHCwmholBbkTxb();
				inputActionCategory.name = "Default";
				inputActionCategory.descriptiveName = inputActionCategory.name;
				actionCategories.Add(inputActionCategory);
				actionCategoryMap.AddCategory(inputActionCategory.id);
				InputBehavior inputBehavior = xizFbYUAUudvlSimSrOkErzkqsrL();
				inputBehavior.name = "Default";
				inputBehaviors.Add(inputBehavior);
				InputMapCategory inputMapCategory = yxpIsNdrTAPqutLkpxjnUcKylTqX();
				inputMapCategory.name = "Default";
				inputMapCategory.descriptiveName = inputMapCategory.name;
				mapCategories.Add(inputMapCategory);
				InputLayout inputLayout = OrCWHNIwZwPcPxObVyVRanWJTfPC();
				inputLayout.name = "Default";
				inputLayout.descriptiveName = inputLayout.name;
				joystickLayouts.Add(inputLayout);
				InputLayout inputLayout2 = TDYbIwUqxjjOdugcAbRLcEuOgzafb();
				inputLayout2.name = "Default";
				inputLayout2.descriptiveName = inputLayout2.name;
				keyboardLayouts.Add(inputLayout2);
				InputLayout inputLayout3 = rpSNaZZDZzcZlrEJdUSlCUOVkdnW();
				inputLayout3.name = "Default";
				inputLayout3.descriptiveName = inputLayout3.name;
				mouseLayouts.Add(inputLayout3);
				InputLayout inputLayout4 = lLRFyHcCwYgxGHVxxbgwMDdmOUfA();
				inputLayout4.name = "Default";
				inputLayout4.descriptiveName = inputLayout4.name;
				customControllerLayouts.Add(inputLayout3);
			}
		}

		[CustomObfuscation(rename = false)]
		internal void SetDefaultValuesOnCreation()
		{
			configVars.platformVars_osxStandalone = new ConfigVars.PlatformVars_OSXStandalone();
			configVars.platformVars_osxStandalone.useAppleGameController = true;
			configVars.platformVars_windowsStandalone = new ConfigVars.PlatformVars_WindowsStandalone();
			configVars.platformVars_windowsStandalone.useWindowsGamingInput = true;
			configVars.keyCombinationOverrideMode = KeyCombinationOverrideMode.Cancel;
			configVars.generateKeyEventsOnKeyCombinationOverride = true;
		}

		public List<InputAction> GetActions_Copy()
		{
			List<InputAction> list = new List<InputAction>();
			for (int i = 0; i < kKnhPVRioLmZoBOgUQuQYtoEHyTc.Count; i++)
			{
				list.Add(kKnhPVRioLmZoBOgUQuQYtoEHyTc[i]);
			}
			return list;
		}

		public List<InputBehavior> GetInputBehaviors_Copy()
		{
			List<InputBehavior> list = new List<InputBehavior>();
			for (int i = 0; i < inputBehaviors.Count; i++)
			{
				list.Add(inputBehaviors[i].Clone());
			}
			return list;
		}

		public List<KeyboardMap> GetKeyboardMaps_Copy()
		{
			List<KeyboardMap> list = new List<KeyboardMap>();
			for (int i = 0; i < keyboardMaps.Count; i++)
			{
				KeyboardMap item = keyboardMaps[i].MiKHWjwNQMtsJoGokzWUTjqgUeQN(containsActionDelegate);
				list.Add(item);
			}
			return list;
		}

		public List<MouseMap> GetMouseMaps_Copy()
		{
			List<MouseMap> list = new List<MouseMap>();
			for (int i = 0; i < mouseMaps.Count; i++)
			{
				MouseMap item = mouseMaps[i].xiVcMoyOaQYFCzHpRSCcraHKrfb(containsActionDelegate);
				list.Add(item);
			}
			return list;
		}

		public void AddPlayer()
		{
			players.Add(oTvzqdfQFNEeFreRthKtbncVjSOD());
		}

		public void InsertPlayer(int index)
		{
			if (index < 0 || index >= players.Count)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			players.Insert(index, oTvzqdfQFNEeFreRthKtbncVjSOD());
		}

		public void DeletePlayer(int index)
		{
			if (players == null || index < 0 || index >= players.Count)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			players.RemoveAt(index);
		}

		public bool ReorderPlayer(int index, bool offsetDown, bool offsetNow)
		{
			return ListTools.OffsetAtIndex(players, index, offsetDown, offsetNow);
		}

		public void DuplicatePlayer(int index)
		{
			if (players == null || index < 0 || index >= players.Count)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			Player_Editor player_Editor = players[index].Clone();
			player_Editor.id = GetNewPlayerId();
			player_Editor.name = StringTools.IterateName(player_Editor.name, -1, GetPlayerNames());
			player_Editor.assignMouseOnStart = false;
			if (index == players.Count - 1)
			{
				players.Add(player_Editor);
			}
			else
			{
				players.Insert(index + 1, player_Editor);
			}
		}

		public string[] GetPlayerNames()
		{
			if (players == null)
			{
				return null;
			}
			string[] array = new string[players.Count];
			for (int i = 0; i < players.Count; i++)
			{
				array[i] = players[i].name;
			}
			return array;
		}

		public int GetPlayerNames(IList<string> results)
		{
			if (results == null)
			{
				throw new ArgumentNullException("results");
			}
			results.Clear();
			if (players == null)
			{
				return 0;
			}
			for (int i = 0; i < players.Count; i++)
			{
				results.Add(players[i].name);
			}
			return results.Count;
		}

		public int[] GetPlayerIds()
		{
			if (players == null)
			{
				return null;
			}
			int[] array = new int[players.Count];
			for (int i = 0; i < players.Count; i++)
			{
				array[i] = players[i].id;
			}
			return array;
		}

		public int[] GetPlayerRuntimeIds()
		{
			if (players == null)
			{
				return null;
			}
			int[] array = new int[players.Count];
			for (int i = 0; i < players.Count; i++)
			{
				if (i == 0)
				{
					array[i] = 9999999;
				}
				else
				{
					array[i] = i - 1;
				}
			}
			return array;
		}

		public int GetPlayerRuntimeIds(IList<int> results)
		{
			if (results == null)
			{
				throw new ArgumentNullException("results");
			}
			results.Clear();
			if (players == null)
			{
				return 0;
			}
			for (int i = 0; i < players.Count; i++)
			{
				if (i == 0)
				{
					results.Add(9999999);
				}
				else
				{
					results.Add(i - 1);
				}
			}
			return results.Count;
		}

		public string GetPlayerNameById(int id)
		{
			if (players == null)
			{
				return string.Empty;
			}
			for (int i = 0; i < players.Count; i++)
			{
				if (players[i].id == id)
				{
					return players[i].name;
				}
			}
			return string.Empty;
		}

		public Player_Editor GetPlayer(int index)
		{
			if (players == null || index < 0 || index >= players.Count)
			{
				return null;
			}
			return players[index];
		}

		public int GetPlayerId(string name)
		{
			if (players == null)
			{
				return -1;
			}
			for (int i = 0; i < players.Count; i++)
			{
				if (players[i].name.Equals(name, StringComparison.OrdinalIgnoreCase))
				{
					return players[i].id;
				}
			}
			return -1;
		}

		public bool IsMouseAssigned()
		{
			if (players == null)
			{
				return false;
			}
			int count = players.Count;
			for (int i = 0; i < count; i++)
			{
				if (players[i].assignMouseOnStart)
				{
					return true;
				}
			}
			return false;
		}

		public void ClearMouseAssignments()
		{
			if (players != null)
			{
				int count = players.Count;
				for (int i = 0; i < count; i++)
				{
					players[i].assignMouseOnStart = false;
				}
			}
		}

		public bool IsKeyboardAssigned()
		{
			if (players == null)
			{
				return false;
			}
			int count = players.Count;
			for (int i = 0; i < count; i++)
			{
				if (players[i].assignKeyboardOnStart)
				{
					return true;
				}
			}
			return false;
		}

		public void ClearKeyboardAssignments()
		{
			if (players != null)
			{
				int count = players.Count;
				for (int i = 0; i < count; i++)
				{
					players[i].assignKeyboardOnStart = false;
				}
			}
		}

		public void AddAction(int categoryId)
		{
			InputAction inputAction = XRCiMKzJhzzUaQkaqNIJRNJHIqew();
			inputAction.categoryId = categoryId;
			kKnhPVRioLmZoBOgUQuQYtoEHyTc.Add(inputAction);
			actionCategoryMap.AddAction(categoryId, inputAction.id);
		}

		public void InsertAction(int categoryId, int actionId)
		{
			if (kKnhPVRioLmZoBOgUQuQYtoEHyTc != null)
			{
				InputAction inputAction = XRCiMKzJhzzUaQkaqNIJRNJHIqew();
				inputAction.categoryId = categoryId;
				kKnhPVRioLmZoBOgUQuQYtoEHyTc.Add(inputAction);
				int index = actionCategoryMap.IndexOfAction(categoryId, actionId);
				actionCategoryMap.InsertAction(categoryId, inputAction.id, index);
			}
		}

		public void DeleteAction(int categoryId, int actionId)
		{
			if (IndexOfActionCategory(categoryId) >= 0)
			{
				int num = IndexOfAction(actionId);
				if (num >= 0)
				{
					kKnhPVRioLmZoBOgUQuQYtoEHyTc.RemoveAt(num);
					actionCategoryMap.RemoveAction(categoryId, actionId);
				}
			}
		}

		public bool ReorderAction(int categoryId, int actionId, bool offsetDown, bool offsetNow)
		{
			return actionCategoryMap.ReorderAction(categoryId, actionId, offsetDown, offsetNow);
		}

		public int DuplicateAction_FromButton(int categoryId, int actionId)
		{
			if (IndexOfActionCategory(categoryId) < 0)
			{
				return -1;
			}
			int num = IndexOfAction(actionId);
			if (num < 0)
			{
				return -1;
			}
			InputAction actionById = GetActionById(actionId);
			if (actionById == null)
			{
				return -1;
			}
			InputAction inputAction = actionById.Clone();
			inputAction.id = GetNewActionId();
			inputAction.name = StringTools.IterateName(inputAction.name, -1, GetActionNames());
			if (num == kKnhPVRioLmZoBOgUQuQYtoEHyTc.Count - 1)
			{
				kKnhPVRioLmZoBOgUQuQYtoEHyTc.Add(inputAction);
				actionCategoryMap.AddAction(categoryId, inputAction.id);
				return kKnhPVRioLmZoBOgUQuQYtoEHyTc.Count - 1;
			}
			kKnhPVRioLmZoBOgUQuQYtoEHyTc.Insert(num + 1, inputAction);
			int num2 = actionCategoryMap.IndexOfAction(categoryId, actionId);
			actionCategoryMap.InsertAction(categoryId, inputAction.id, num2 + 1);
			return num + 1;
		}

		private int qoIAwJnmqZWADCqTPBIEiJpSniyjb(int P_0, InputAction P_1)
		{
			if (IndexOfActionCategory(P_0) < 0)
			{
				return -1;
			}
			InputAction inputAction = P_1.Clone();
			inputAction.id = GetNewActionId();
			inputAction.name = StringTools.IterateName(inputAction.name, -1, GetActionNames());
			kKnhPVRioLmZoBOgUQuQYtoEHyTc.Add(inputAction);
			return kKnhPVRioLmZoBOgUQuQYtoEHyTc.Count - 1;
		}

		public string[] GetActionNames()
		{
			if (kKnhPVRioLmZoBOgUQuQYtoEHyTc == null)
			{
				return null;
			}
			string[] array = new string[kKnhPVRioLmZoBOgUQuQYtoEHyTc.Count];
			for (int i = 0; i < kKnhPVRioLmZoBOgUQuQYtoEHyTc.Count; i++)
			{
				array[i] = kKnhPVRioLmZoBOgUQuQYtoEHyTc[i].name;
			}
			return array;
		}

		public int GetActionNames(IList<string> results)
		{
			if (results == null)
			{
				throw new ArgumentNullException("results");
			}
			results.Clear();
			if (kKnhPVRioLmZoBOgUQuQYtoEHyTc == null)
			{
				return 0;
			}
			for (int i = 0; i < kKnhPVRioLmZoBOgUQuQYtoEHyTc.Count; i++)
			{
				results.Add(kKnhPVRioLmZoBOgUQuQYtoEHyTc[i].name);
			}
			return results.Count;
		}

		public int[] GetActionIds()
		{
			if (kKnhPVRioLmZoBOgUQuQYtoEHyTc == null)
			{
				return null;
			}
			int[] array = new int[kKnhPVRioLmZoBOgUQuQYtoEHyTc.Count];
			for (int i = 0; i < kKnhPVRioLmZoBOgUQuQYtoEHyTc.Count; i++)
			{
				array[i] = kKnhPVRioLmZoBOgUQuQYtoEHyTc[i].id;
			}
			return array;
		}

		public int GetActionIds(IList<int> results)
		{
			if (results == null)
			{
				throw new ArgumentNullException("results");
			}
			results.Clear();
			if (kKnhPVRioLmZoBOgUQuQYtoEHyTc == null)
			{
				return 0;
			}
			for (int i = 0; i < kKnhPVRioLmZoBOgUQuQYtoEHyTc.Count; i++)
			{
				results.Add(kKnhPVRioLmZoBOgUQuQYtoEHyTc[i].id);
			}
			return results.Count;
		}

		public string GetActionNameById(int id)
		{
			if (kKnhPVRioLmZoBOgUQuQYtoEHyTc == null)
			{
				return string.Empty;
			}
			for (int i = 0; i < kKnhPVRioLmZoBOgUQuQYtoEHyTc.Count; i++)
			{
				if (kKnhPVRioLmZoBOgUQuQYtoEHyTc[i].id == id)
				{
					return kKnhPVRioLmZoBOgUQuQYtoEHyTc[i].name;
				}
			}
			return string.Empty;
		}

		public InputAction GetAction(int index)
		{
			if (kKnhPVRioLmZoBOgUQuQYtoEHyTc == null || index < 0 || index >= kKnhPVRioLmZoBOgUQuQYtoEHyTc.Count)
			{
				return null;
			}
			return kKnhPVRioLmZoBOgUQuQYtoEHyTc[index];
		}

		public InputAction GetAction(string name)
		{
			if (kKnhPVRioLmZoBOgUQuQYtoEHyTc == null)
			{
				return null;
			}
			int num = IndexOfAction(name);
			if (num < 0)
			{
				return null;
			}
			return kKnhPVRioLmZoBOgUQuQYtoEHyTc[num];
		}

		public InputAction GetActionById(int id)
		{
			if (kKnhPVRioLmZoBOgUQuQYtoEHyTc == null)
			{
				return null;
			}
			for (int i = 0; i < kKnhPVRioLmZoBOgUQuQYtoEHyTc.Count; i++)
			{
				if (kKnhPVRioLmZoBOgUQuQYtoEHyTc[i].id == id)
				{
					return kKnhPVRioLmZoBOgUQuQYtoEHyTc[i];
				}
			}
			return null;
		}

		public int GetActionId(string name)
		{
			if (kKnhPVRioLmZoBOgUQuQYtoEHyTc == null)
			{
				return -1;
			}
			int num = IndexOfAction(name);
			if (num < 0)
			{
				return -1;
			}
			return kKnhPVRioLmZoBOgUQuQYtoEHyTc[num].id;
		}

		public string[] GetSortedActionNamesInCategory(int id)
		{
			if (actionCategories == null || kKnhPVRioLmZoBOgUQuQYtoEHyTc == null)
			{
				return null;
			}
			List<string> list = new List<string>();
			foreach (int item in actionCategoryMap.ActionIdsInCategory(id))
			{
				InputAction actionById = GetActionById(item);
				if (actionById != null)
				{
					list.Add(actionById.name);
				}
			}
			return list.ToArray();
		}

		[IteratorStateMachine(typeof(UsVcNshWAYkknnPjZOjTZMsLaqUo))]
		public IEnumerable<string> SortedActionNamesInCategory(int id)
		{
			return new UsVcNshWAYkknnPjZOjTZMsLaqUo(-2)
			{
				QlkgeGvTazXozkIwxYuVLLYnoQas = this,
				qAqJaDsxhXtvjsXHZmCgtCFbeIsQ = id
			};
		}

		public string[] GetSortedActionDescriptiveNamesInCategory(int id)
		{
			if (actionCategories == null || kKnhPVRioLmZoBOgUQuQYtoEHyTc == null)
			{
				return null;
			}
			List<string> list = new List<string>();
			foreach (int item in actionCategoryMap.ActionIdsInCategory(id))
			{
				InputAction actionById = GetActionById(item);
				if (actionById != null)
				{
					list.Add(actionById.descriptiveName);
				}
			}
			return list.ToArray();
		}

		[IteratorStateMachine(typeof(ScIKNKgSZWiXAgoUWOSJbCQjNspK))]
		public IEnumerable<string> SortedActionDescriptiveNamesInCategory(int id)
		{
			return new ScIKNKgSZWiXAgoUWOSJbCQjNspK(-2)
			{
				pPFffriaQTyXVpKuSiEJQKTEDuYr = this,
				lePHncbgCfOCVOoncsSKSGwRgfLH = id
			};
		}

		public int[] GetSortedActionIdsInCategory(int id)
		{
			if (actionCategories == null || kKnhPVRioLmZoBOgUQuQYtoEHyTc == null)
			{
				return null;
			}
			List<int> list = new List<int>();
			foreach (int item in actionCategoryMap.ActionIdsInCategory(id))
			{
				list.Add(item);
			}
			return list.ToArray();
		}

		[IteratorStateMachine(typeof(IlUBngBbMrbDQHKPhbvqmVnsOTzdc))]
		public IEnumerable<int> SortedActionIdsInCategory(int id)
		{
			return new IlUBngBbMrbDQHKPhbvqmVnsOTzdc(-2)
			{
				lIfgfVVsLaBYaFICnCUFQaIYngxab = this,
				qTIJTJMjmyaYmAXFIyApnQsgfLfm = id
			};
		}

		public bool ContainsAction(int id)
		{
			return IndexOfAction(id) >= 0;
		}

		public int IndexOfAction(int id)
		{
			if (kKnhPVRioLmZoBOgUQuQYtoEHyTc == null)
			{
				return -1;
			}
			for (int i = 0; i < kKnhPVRioLmZoBOgUQuQYtoEHyTc.Count; i++)
			{
				if (kKnhPVRioLmZoBOgUQuQYtoEHyTc[i].id == id)
				{
					return i;
				}
			}
			return -1;
		}

		public int IndexOfAction(string name)
		{
			if (kKnhPVRioLmZoBOgUQuQYtoEHyTc == null)
			{
				return -1;
			}
			if (name == null || name == string.Empty)
			{
				return -1;
			}
			for (int i = 0; i < kKnhPVRioLmZoBOgUQuQYtoEHyTc.Count; i++)
			{
				if (kKnhPVRioLmZoBOgUQuQYtoEHyTc[i].name.Equals(name, StringComparison.OrdinalIgnoreCase))
				{
					return i;
				}
			}
			return -1;
		}

		public void AddActionCategory()
		{
			InputActionCategory inputActionCategory = MukaBDbnShpYGcdoHCwmholBbkTxb();
			actionCategories.Add(inputActionCategory);
			actionCategoryMap.AddCategory(inputActionCategory.id);
		}

		public void InsertActionCategory(int index)
		{
			if (index < 0 || index >= actionCategories.Count)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			InputActionCategory inputActionCategory = MukaBDbnShpYGcdoHCwmholBbkTxb();
			actionCategories.Insert(index, inputActionCategory);
			actionCategoryMap.AddCategory(inputActionCategory.id);
		}

		public void DeleteActionCategory(int index)
		{
			if (actionCategories == null || index < 0 || index >= actionCategories.Count)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			int id = actionCategories[index].id;
			actionCategoryMap.RemoveCategory(id);
			if (kKnhPVRioLmZoBOgUQuQYtoEHyTc != null)
			{
				for (int num = kKnhPVRioLmZoBOgUQuQYtoEHyTc.Count - 1; num >= 0; num--)
				{
					if (kKnhPVRioLmZoBOgUQuQYtoEHyTc[num].categoryId == id)
					{
						kKnhPVRioLmZoBOgUQuQYtoEHyTc.RemoveAt(num);
					}
				}
			}
			actionCategories.RemoveAt(index);
		}

		public bool ReorderActionCategory(int index, bool offsetDown, bool offsetNow)
		{
			if (index < 0 || index >= actionCategories.Count)
			{
				return false;
			}
			return ListTools.OffsetAtIndex(actionCategories, index, offsetDown, offsetNow);
		}

		public void DuplicateActionCategory(int index, bool duplicateActions)
		{
			if (actionCategories == null || index < 0 || index >= actionCategories.Count)
			{
				return;
			}
			InputActionCategory inputActionCategory = new InputActionCategory(actionCategories[index]);
			inputActionCategory.id = GetNewActionCategoryId();
			inputActionCategory.name = StringTools.IterateName(inputActionCategory.name, -1, GetActionCategoryNames());
			if (index == actionCategories.Count - 1)
			{
				actionCategories.Add(inputActionCategory);
			}
			else
			{
				actionCategories.Insert(index + 1, inputActionCategory);
			}
			actionCategoryMap.AddCategory(inputActionCategory.id);
			if (!duplicateActions || kKnhPVRioLmZoBOgUQuQYtoEHyTc == null)
			{
				return;
			}
			int id = inputActionCategory.id;
			int id2 = actionCategories[index].id;
			List<int> list = new List<int>();
			for (int i = 0; i < kKnhPVRioLmZoBOgUQuQYtoEHyTc.Count; i++)
			{
				if (kKnhPVRioLmZoBOgUQuQYtoEHyTc[i].categoryId == id2)
				{
					list.Add(i);
				}
			}
			Dictionary<int, int> dictionary = new Dictionary<int, int>(list.Count);
			for (int j = 0; j < list.Count; j++)
			{
				InputAction inputAction = kKnhPVRioLmZoBOgUQuQYtoEHyTc[list[j]];
				int num = qoIAwJnmqZWADCqTPBIEiJpSniyjb(id2, inputAction);
				if (num >= 0)
				{
					InputAction inputAction2 = kKnhPVRioLmZoBOgUQuQYtoEHyTc[num];
					inputAction2.categoryId = id;
					dictionary.Add(inputAction.id, inputAction2.id);
				}
			}
			foreach (int item in actionCategoryMap.ActionIdsInCategory(id2))
			{
				if (dictionary.TryGetValue(item, out var value))
				{
					actionCategoryMap.AddAction(id, value);
				}
			}
		}

		public void ChangeActionCategory(int actionId, int newCategoryId)
		{
			int num = IndexOfAction(actionId);
			if (num >= 0 && kKnhPVRioLmZoBOgUQuQYtoEHyTc[num].categoryId != newCategoryId)
			{
				actionCategoryMap.ChangeCategory(actionId, newCategoryId);
				kKnhPVRioLmZoBOgUQuQYtoEHyTc[num].categoryId = newCategoryId;
			}
		}

		public int GetActionCategoryCount(int id)
		{
			if (actionCategories == null)
			{
				return 0;
			}
			int num = 0;
			if (kKnhPVRioLmZoBOgUQuQYtoEHyTc != null)
			{
				for (int i = 0; i < kKnhPVRioLmZoBOgUQuQYtoEHyTc.Count; i++)
				{
					if (kKnhPVRioLmZoBOgUQuQYtoEHyTc[i].categoryId == id)
					{
						num++;
					}
				}
			}
			return num;
		}

		public int GetActionCategoryIndex(int id)
		{
			if (actionCategories == null)
			{
				return 0;
			}
			for (int i = 0; i < actionCategories.Count; i++)
			{
				if (actionCategories[i].id == id)
				{
					return i;
				}
			}
			return -1;
		}

		public string[] GetActionCategoryNames()
		{
			if (actionCategories == null)
			{
				return null;
			}
			string[] array = new string[actionCategories.Count];
			for (int i = 0; i < actionCategories.Count; i++)
			{
				array[i] = actionCategories[i].name;
			}
			return array;
		}

		public int[] GetActionCategoryIds()
		{
			if (actionCategories == null)
			{
				return null;
			}
			int[] array = new int[actionCategories.Count];
			for (int i = 0; i < actionCategories.Count; i++)
			{
				array[i] = actionCategories[i].id;
			}
			return array;
		}

		public InputCategory GetActionCategory(int index)
		{
			if (actionCategories == null || index < 0 || index >= actionCategories.Count)
			{
				return null;
			}
			return actionCategories[index];
		}

		public InputCategory GetActionCategory(string name)
		{
			if (actionCategories == null)
			{
				return null;
			}
			int num = IndexOfActionCategory(name);
			if (num < 0)
			{
				return null;
			}
			return actionCategories[num];
		}

		public InputCategory GetActionCategoryById(int id)
		{
			int num = IndexOfActionCategory(id);
			if (num < 0)
			{
				return null;
			}
			return actionCategories[num];
		}

		public int GetActionCategoryId(string name)
		{
			if (actionCategories == null)
			{
				return -1;
			}
			int num = IndexOfActionCategory(name);
			if (num < 0)
			{
				return -1;
			}
			return actionCategories[num].id;
		}

		public string GetActionCategoryNameById(int id)
		{
			if (actionCategories == null)
			{
				return string.Empty;
			}
			for (int i = 0; i < actionCategories.Count; i++)
			{
				if (actionCategories[i].id == id)
				{
					return actionCategories[i].name;
				}
			}
			return string.Empty;
		}

		public int IndexOfActionCategory(int id)
		{
			if (actionCategories == null)
			{
				return -1;
			}
			for (int i = 0; i < actionCategories.Count; i++)
			{
				if (actionCategories[i].id == id)
				{
					return i;
				}
			}
			return -1;
		}

		public int IndexOfActionCategory(string name)
		{
			if (name == null || name == string.Empty)
			{
				return -1;
			}
			if (actionCategories == null)
			{
				return -1;
			}
			for (int i = 0; i < actionCategories.Count; i++)
			{
				if (actionCategories[i].name.Equals(name, StringComparison.OrdinalIgnoreCase))
				{
					return i;
				}
			}
			return -1;
		}

		public int GetActionCategoryCount()
		{
			if (actionCategories == null)
			{
				return 0;
			}
			return actionCategories.Count;
		}

		public void AddInputBehavior()
		{
			inputBehaviors.Add(xizFbYUAUudvlSimSrOkErzkqsrL());
		}

		public void InsertInputBehavior(int index)
		{
			if (index < 0 || index >= inputBehaviors.Count)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			inputBehaviors.Insert(index, xizFbYUAUudvlSimSrOkErzkqsrL());
		}

		public void DeleteInputBehavior(int index)
		{
			if (inputBehaviors == null || index < 0 || index >= inputBehaviors.Count)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			int id = inputBehaviors[index].id;
			if (kKnhPVRioLmZoBOgUQuQYtoEHyTc != null)
			{
				for (int i = 0; i < kKnhPVRioLmZoBOgUQuQYtoEHyTc.Count; i++)
				{
					if (kKnhPVRioLmZoBOgUQuQYtoEHyTc[i].behaviorId == id)
					{
						kKnhPVRioLmZoBOgUQuQYtoEHyTc[i].behaviorId = 0;
					}
				}
			}
			inputBehaviors.RemoveAt(index);
		}

		public bool ReorderInputBehavior(int index, bool offsetDown, bool offsetNow)
		{
			return ListTools.OffsetAtIndex(inputBehaviors, index, offsetDown, offsetNow);
		}

		public void DuplicateInputBehavior(int index)
		{
			if (inputBehaviors == null || index < 0 || index >= inputBehaviors.Count)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			InputBehavior inputBehavior = inputBehaviors[index].Clone();
			inputBehavior.id = GetNewInputBehaviorId();
			inputBehavior.name = StringTools.IterateName(inputBehavior.name, -1, GetInputBehaviorNames());
			if (index == inputBehaviors.Count - 1)
			{
				inputBehaviors.Add(inputBehavior);
			}
			else
			{
				inputBehaviors.Insert(index + 1, inputBehavior);
			}
		}

		public string[] GetInputBehaviorNames()
		{
			if (inputBehaviors == null)
			{
				return null;
			}
			string[] array = new string[inputBehaviors.Count];
			for (int i = 0; i < inputBehaviors.Count; i++)
			{
				array[i] = inputBehaviors[i].name;
			}
			return array;
		}

		public int[] GetInputBehaviorIds()
		{
			if (inputBehaviors == null)
			{
				return null;
			}
			int[] array = new int[inputBehaviors.Count];
			for (int i = 0; i < inputBehaviors.Count; i++)
			{
				array[i] = inputBehaviors[i].id;
			}
			return array;
		}

		public InputBehavior GetInputBehavior(int index)
		{
			if (inputBehaviors == null || index < 0 || index >= inputBehaviors.Count)
			{
				return null;
			}
			return inputBehaviors[index];
		}

		public InputBehavior GetInputBehavior(string name)
		{
			if (inputBehaviors == null)
			{
				return null;
			}
			int num = IndexOfInputBehavior(name);
			if (num < 0)
			{
				return null;
			}
			return inputBehaviors[num];
		}

		public InputBehavior GetInputBehaviorById(int id)
		{
			if (inputBehaviors == null)
			{
				return null;
			}
			for (int i = 0; i < inputBehaviors.Count; i++)
			{
				if (inputBehaviors[i].id == id)
				{
					return inputBehaviors[i];
				}
			}
			return null;
		}

		public int GetInputBehaviorId(string name)
		{
			if (inputBehaviors == null)
			{
				return -1;
			}
			int num = IndexOfInputBehavior(name);
			if (num < 0)
			{
				return -1;
			}
			return inputBehaviors[num].id;
		}

		public int IndexOfInputBehavior(int id)
		{
			if (inputBehaviors == null)
			{
				return -1;
			}
			for (int i = 0; i < inputBehaviors.Count; i++)
			{
				if (inputBehaviors[i].id == id)
				{
					return i;
				}
			}
			return -1;
		}

		public int IndexOfInputBehavior(string name)
		{
			if (inputBehaviors == null)
			{
				return -1;
			}
			if (name == null || name == string.Empty)
			{
				return -1;
			}
			for (int i = 0; i < inputBehaviors.Count; i++)
			{
				if (inputBehaviors[i].name.Equals(name, StringComparison.OrdinalIgnoreCase))
				{
					return i;
				}
			}
			return -1;
		}

		public void AddMapCategory()
		{
			mapCategories.Add(yxpIsNdrTAPqutLkpxjnUcKylTqX());
		}

		public void InsertMapCategory(int index)
		{
			if (index < 0 || index >= mapCategories.Count)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			mapCategories.Insert(index, yxpIsNdrTAPqutLkpxjnUcKylTqX());
		}

		public void DeleteMapCategory(int index)
		{
			if (mapCategories == null || index < 0 || index >= mapCategories.Count)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			int id = mapCategories[index].id;
			if (joystickMaps != null)
			{
				for (int num = joystickMaps.Count - 1; num >= 0; num--)
				{
					if (joystickMaps[num].categoryId == id)
					{
						joystickMaps.RemoveAt(num);
					}
				}
			}
			if (keyboardMaps != null)
			{
				for (int num2 = keyboardMaps.Count - 1; num2 >= 0; num2--)
				{
					if (keyboardMaps[num2].categoryId == id)
					{
						keyboardMaps.RemoveAt(num2);
					}
				}
			}
			if (mouseMaps != null)
			{
				for (int num3 = mouseMaps.Count - 1; num3 >= 0; num3--)
				{
					if (mouseMaps[num3].categoryId == id)
					{
						mouseMaps.RemoveAt(num3);
					}
				}
			}
			if (customControllerMaps != null)
			{
				for (int num4 = customControllerMaps.Count - 1; num4 >= 0; num4--)
				{
					if (customControllerMaps[num4].categoryId == id)
					{
						customControllerMaps.RemoveAt(num4);
					}
				}
			}
			if (mapCategories != null)
			{
				for (int i = 0; i < mapCategories.Count; i++)
				{
					InputMapCategory inputMapCategory = mapCategories[i];
					if (inputMapCategory.checkConflictsCategoryIds == null)
					{
						continue;
					}
					for (int j = 0; j < inputMapCategory.checkConflictsCategoryIds.Count; j++)
					{
						if (inputMapCategory.checkConflictsCategoryIds[j] == id)
						{
							inputMapCategory.checkConflictsCategoryIds.RemoveAt(j);
						}
					}
				}
			}
			if (players != null)
			{
				Action<List<Player_Editor.Mapping>, int> action = BqxrkBNzJTKgNcopOxEBoFgXGtpFA._003C_003E9.LpNvWAkQhcwNqKZVbjABPxWMxCCC;
				for (int k = 0; k < players.Count; k++)
				{
					Player_Editor player_Editor = players[k];
					if (player_Editor != null)
					{
						action(player_Editor.defaultKeyboardMaps, id);
						action(player_Editor.defaultMouseMaps, id);
						action(player_Editor.defaultJoystickMaps, id);
						action(player_Editor.defaultCustomControllerMaps, id);
					}
				}
			}
			mapCategories.RemoveAt(index);
		}

		public bool ReorderMapCategory(int index, bool offsetDown, bool offsetNow)
		{
			return ListTools.OffsetAtIndex(mapCategories, index, offsetDown, offsetNow);
		}

		public void DuplicateMapCategory(int index, bool duplicateMaps)
		{
			if (mapCategories == null || index < 0 || index >= mapCategories.Count)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			InputMapCategory inputMapCategory = new InputMapCategory(mapCategories[index]);
			inputMapCategory.id = GetNewMapCategoryId();
			inputMapCategory.name = StringTools.IterateName(inputMapCategory.name, -1, GetMapCategoryNames());
			if (index == mapCategories.Count - 1)
			{
				mapCategories.Add(inputMapCategory);
			}
			else
			{
				mapCategories.Insert(index + 1, inputMapCategory);
			}
			if (!duplicateMaps)
			{
				return;
			}
			int id = inputMapCategory.id;
			int id2 = mapCategories[index].id;
			if (joystickMaps != null)
			{
				for (int num = joystickMaps.Count - 1; num >= 0; num--)
				{
					if (joystickMaps[num].categoryId == id2)
					{
						int num2 = DuplicateJoystickMap(num);
						if (num2 >= 0)
						{
							joystickMaps[num2].categoryId = id;
						}
					}
				}
			}
			if (keyboardMaps != null)
			{
				for (int num3 = keyboardMaps.Count - 1; num3 >= 0; num3--)
				{
					if (keyboardMaps[num3].categoryId == id2)
					{
						int num4 = DuplicateKeyboardMap(num3);
						if (num4 >= 0)
						{
							keyboardMaps[num4].categoryId = id;
						}
					}
				}
			}
			if (mouseMaps != null)
			{
				for (int num5 = mouseMaps.Count - 1; num5 >= 0; num5--)
				{
					if (mouseMaps[num5].categoryId == id2)
					{
						int num6 = DuplicateMouseMap(num5);
						if (num6 >= 0)
						{
							mouseMaps[num6].categoryId = id;
						}
					}
				}
			}
			if (customControllerMaps == null)
			{
				return;
			}
			for (int num7 = customControllerMaps.Count - 1; num7 >= 0; num7--)
			{
				if (customControllerMaps[num7].categoryId == id2)
				{
					int num8 = DuplicateCustomControllerMap(num7);
					if (num8 >= 0)
					{
						customControllerMaps[num8].categoryId = id;
					}
				}
			}
		}

		public int GetMapCategoryMapCount(int id)
		{
			if (mapCategories == null)
			{
				return 0;
			}
			int num = 0;
			if (joystickMaps != null)
			{
				for (int i = 0; i < joystickMaps.Count; i++)
				{
					if (joystickMaps[i].categoryId == id)
					{
						num++;
					}
				}
			}
			if (keyboardMaps != null)
			{
				for (int j = 0; j < keyboardMaps.Count; j++)
				{
					if (keyboardMaps[j].categoryId == id)
					{
						num++;
					}
				}
			}
			if (mouseMaps != null)
			{
				for (int k = 0; k < mouseMaps.Count; k++)
				{
					if (mouseMaps[k].categoryId == id)
					{
						num++;
					}
				}
			}
			if (customControllerMaps != null)
			{
				for (int l = 0; l < customControllerMaps.Count; l++)
				{
					if (customControllerMaps[l].categoryId == id)
					{
						num++;
					}
				}
			}
			return num;
		}

		public int GetMapCategoryIndex(int id)
		{
			if (mapCategories == null)
			{
				return 0;
			}
			for (int i = 0; i < mapCategories.Count; i++)
			{
				if (mapCategories[i].id == id)
				{
					return i;
				}
			}
			return -1;
		}

		public string[] GetMapCategoryNames()
		{
			if (mapCategories == null)
			{
				return null;
			}
			string[] array = new string[mapCategories.Count];
			for (int i = 0; i < mapCategories.Count; i++)
			{
				array[i] = mapCategories[i].name;
			}
			return array;
		}

		public int[] GetMapCategoryIds()
		{
			if (mapCategories == null)
			{
				return null;
			}
			int[] array = new int[mapCategories.Count];
			for (int i = 0; i < mapCategories.Count; i++)
			{
				array[i] = mapCategories[i].id;
			}
			return array;
		}

		public InputMapCategory GetMapCategory(int index)
		{
			if (mapCategories == null || index < 0 || index >= mapCategories.Count)
			{
				return null;
			}
			return mapCategories[index];
		}

		public InputMapCategory GetMapCategory(string name)
		{
			if (mapCategories == null)
			{
				return null;
			}
			int num = IndexOfMapCategory(name);
			if (num < 0)
			{
				return null;
			}
			return mapCategories[num];
		}

		public InputMapCategory GetMapCategoryById(int id)
		{
			if (mapCategories == null)
			{
				return null;
			}
			for (int i = 0; i < mapCategories.Count; i++)
			{
				if (mapCategories[i].id == id)
				{
					return mapCategories[i];
				}
			}
			return null;
		}

		public int GetMapCategoryId(string name)
		{
			if (mapCategories == null)
			{
				return -1;
			}
			int num = IndexOfMapCategory(name);
			if (num < 0)
			{
				return -1;
			}
			return mapCategories[num].id;
		}

		public string GetMapCategoryNameById(int id)
		{
			if (mapCategories == null)
			{
				return string.Empty;
			}
			for (int i = 0; i < mapCategories.Count; i++)
			{
				if (mapCategories[i].id == id)
				{
					return mapCategories[i].name;
				}
			}
			return string.Empty;
		}

		public int IndexOfMapCategory(int id)
		{
			if (mapCategories == null)
			{
				return -1;
			}
			for (int i = 0; i < mapCategories.Count; i++)
			{
				if (mapCategories[i].id == id)
				{
					return i;
				}
			}
			return -1;
		}

		public int IndexOfMapCategory(string name)
		{
			if (name == null || name == string.Empty)
			{
				return -1;
			}
			if (mapCategories == null)
			{
				return -1;
			}
			for (int i = 0; i < mapCategories.Count; i++)
			{
				if (mapCategories[i].name.Equals(name, StringComparison.OrdinalIgnoreCase))
				{
					return i;
				}
			}
			return -1;
		}

		public string[] GetLayoutNames(ControllerType controllerType)
		{
			return controllerType switch
			{
				ControllerType.Keyboard => GetKeyboardLayoutNames(), 
				ControllerType.Mouse => GetMouseLayoutNames(), 
				ControllerType.Joystick => GetJoystickLayoutNames(), 
				ControllerType.Custom => GetCustomControllerLayoutNames(), 
				_ => throw new NotImplementedException(), 
			};
		}

		public int[] GetLayoutIds(ControllerType controllerType)
		{
			return controllerType switch
			{
				ControllerType.Keyboard => GetKeyboardLayoutIds(), 
				ControllerType.Mouse => GetMouseLayoutIds(), 
				ControllerType.Joystick => GetJoystickLayoutIds(), 
				ControllerType.Custom => GetCustomControllerLayoutIds(), 
				_ => throw new NotImplementedException(), 
			};
		}

		public void AddJoystickLayout()
		{
			joystickLayouts.Add(OrCWHNIwZwPcPxObVyVRanWJTfPC());
		}

		public void InsertJoystickLayout(int index)
		{
			if (index < 0 || index >= joystickLayouts.Count)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			joystickLayouts.Insert(index, OrCWHNIwZwPcPxObVyVRanWJTfPC());
		}

		public void DeleteJoystickLayout(int index)
		{
			if (joystickLayouts == null || index < 0 || index >= joystickLayouts.Count)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			int id = joystickLayouts[index].id;
			if (joystickMaps != null)
			{
				for (int num = joystickMaps.Count - 1; num >= 0; num--)
				{
					if (joystickMaps[num].layoutId == id)
					{
						joystickMaps.RemoveAt(num);
					}
				}
			}
			if (players != null)
			{
				Action<List<Player_Editor.Mapping>, int> action = BqxrkBNzJTKgNcopOxEBoFgXGtpFA._003C_003E9.bXocRkjHdiUEuVPjWTBFWdlvNHML;
				for (int i = 0; i < players.Count; i++)
				{
					Player_Editor player_Editor = players[i];
					if (player_Editor != null)
					{
						action(player_Editor.defaultJoystickMaps, id);
					}
				}
			}
			joystickLayouts.RemoveAt(index);
		}

		public bool ReorderJoystickLayout(int index, bool offsetDown, bool offsetNow)
		{
			return ListTools.OffsetAtIndex(joystickLayouts, index, offsetDown, offsetNow);
		}

		public void DuplicateJoystickLayout(int index, bool duplicateMaps)
		{
			if (joystickLayouts == null || index < 0 || index >= joystickLayouts.Count)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			InputLayout inputLayout = joystickLayouts[index].Clone();
			inputLayout.id = GetNewJoystickLayoutId();
			inputLayout.name = StringTools.IterateName(inputLayout.name, -1, GetJoystickLayoutNames());
			if (index == joystickLayouts.Count - 1)
			{
				joystickLayouts.Add(inputLayout);
			}
			else
			{
				joystickLayouts.Insert(index + 1, inputLayout);
			}
			if (!duplicateMaps)
			{
				return;
			}
			int id = inputLayout.id;
			int id2 = joystickLayouts[index].id;
			if (joystickMaps == null)
			{
				return;
			}
			for (int num = joystickMaps.Count - 1; num >= 0; num--)
			{
				if (joystickMaps[num].layoutId == id2)
				{
					int num2 = DuplicateJoystickMap(num);
					if (num2 >= 0)
					{
						joystickMaps[num2].layoutId = id;
					}
				}
			}
		}

		public int GetJoystickLayoutMapCount(int id)
		{
			if (joystickLayouts == null)
			{
				return 0;
			}
			int num = 0;
			if (joystickMaps != null)
			{
				for (int i = 0; i < joystickMaps.Count; i++)
				{
					if (joystickMaps[i].layoutId == id)
					{
						num++;
					}
				}
			}
			return num;
		}

		public int GetJoystickLayoutIndex(int id)
		{
			if (joystickLayouts == null)
			{
				return 0;
			}
			for (int i = 0; i < joystickLayouts.Count; i++)
			{
				if (joystickLayouts[i].id == id)
				{
					return i;
				}
			}
			return -1;
		}

		public string[] GetJoystickLayoutNames()
		{
			if (joystickLayouts == null)
			{
				return null;
			}
			string[] array = new string[joystickLayouts.Count];
			for (int i = 0; i < joystickLayouts.Count; i++)
			{
				array[i] = joystickLayouts[i].name;
			}
			return array;
		}

		public int[] GetJoystickLayoutIds()
		{
			if (joystickLayouts == null)
			{
				return null;
			}
			int[] array = new int[joystickLayouts.Count];
			for (int i = 0; i < joystickLayouts.Count; i++)
			{
				array[i] = joystickLayouts[i].id;
			}
			return array;
		}

		public InputLayout GetJoystickLayout(int index)
		{
			if (joystickLayouts == null || index < 0 || index >= joystickLayouts.Count)
			{
				return null;
			}
			return joystickLayouts[index];
		}

		public InputLayout GetJoystickLayout(string name)
		{
			if (joystickLayouts == null)
			{
				return null;
			}
			int num = IndexOfJoystickLayout(name);
			if (num < 0)
			{
				return null;
			}
			return joystickLayouts[num];
		}

		public InputLayout GetJoystickLayoutById(int id)
		{
			if (joystickLayouts == null)
			{
				return null;
			}
			int num = IndexOfJoystickLayout(id);
			if (num < 0)
			{
				return null;
			}
			return joystickLayouts[num];
		}

		public int GetJoystickLayoutId(string name)
		{
			if (joystickLayouts == null)
			{
				return -1;
			}
			int num = IndexOfJoystickLayout(name);
			if (num < 0)
			{
				return -1;
			}
			return joystickLayouts[num].id;
		}

		public int IndexOfJoystickLayout(int id)
		{
			if (joystickLayouts == null)
			{
				return -1;
			}
			for (int i = 0; i < joystickLayouts.Count; i++)
			{
				if (joystickLayouts[i].id == id)
				{
					return i;
				}
			}
			return -1;
		}

		public int IndexOfJoystickLayout(string name)
		{
			if (name == null || name == string.Empty)
			{
				return -1;
			}
			if (joystickLayouts == null)
			{
				return -1;
			}
			for (int i = 0; i < joystickLayouts.Count; i++)
			{
				if (joystickLayouts[i].name.Equals(name, StringComparison.OrdinalIgnoreCase))
				{
					return i;
				}
			}
			return -1;
		}

		public string GetJoystickLayoutNameById(int id)
		{
			if (joystickLayouts != null)
			{
				for (int i = 0; i < joystickLayouts.Count; i++)
				{
					if (joystickLayouts[i].id == id)
					{
						return joystickLayouts[i].name;
					}
				}
			}
			return "Unknown";
		}

		public void AddKeyboardLayout()
		{
			keyboardLayouts.Add(TDYbIwUqxjjOdugcAbRLcEuOgzafb());
		}

		public void InsertKeyboardLayout(int index)
		{
			if (index < 0 || index >= keyboardLayouts.Count)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			keyboardLayouts.Insert(index, TDYbIwUqxjjOdugcAbRLcEuOgzafb());
		}

		public void DeleteKeyboardLayout(int index)
		{
			if (keyboardLayouts == null || index < 0 || index >= keyboardLayouts.Count)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			int id = keyboardLayouts[index].id;
			if (keyboardMaps != null)
			{
				for (int num = keyboardMaps.Count - 1; num >= 0; num--)
				{
					if (keyboardMaps[num].layoutId == id)
					{
						keyboardMaps.RemoveAt(num);
					}
				}
			}
			if (players != null)
			{
				Action<List<Player_Editor.Mapping>, int> action = BqxrkBNzJTKgNcopOxEBoFgXGtpFA._003C_003E9.bDphMvksNrTLseAeEtRbwdoSNQdAb;
				for (int i = 0; i < players.Count; i++)
				{
					Player_Editor player_Editor = players[i];
					if (player_Editor != null)
					{
						action(player_Editor.defaultKeyboardMaps, id);
					}
				}
			}
			keyboardLayouts.RemoveAt(index);
		}

		public bool ReorderKeyboardLayout(int index, bool offsetDown, bool offsetNow)
		{
			return ListTools.OffsetAtIndex(keyboardLayouts, index, offsetDown, offsetNow);
		}

		public void DuplicateKeyboardLayout(int index, bool duplicateMaps)
		{
			if (keyboardLayouts == null || index < 0 || index >= keyboardLayouts.Count)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			InputLayout inputLayout = keyboardLayouts[index].Clone();
			inputLayout.id = GetNewKeyboardLayoutId();
			inputLayout.name = StringTools.IterateName(inputLayout.name, -1, GetKeyboardLayoutNames());
			if (index == keyboardLayouts.Count - 1)
			{
				keyboardLayouts.Add(inputLayout);
			}
			else
			{
				keyboardLayouts.Insert(index + 1, inputLayout);
			}
			if (!duplicateMaps)
			{
				return;
			}
			int id = inputLayout.id;
			int id2 = keyboardLayouts[index].id;
			if (keyboardMaps == null)
			{
				return;
			}
			for (int num = keyboardMaps.Count - 1; num >= 0; num--)
			{
				if (keyboardMaps[num].layoutId == id2)
				{
					int num2 = DuplicateKeyboardMap(num);
					if (num2 >= 0)
					{
						keyboardMaps[num2].layoutId = id;
					}
				}
			}
		}

		public int GetKeyboardLayoutMapCount(int id)
		{
			if (keyboardLayouts == null)
			{
				return 0;
			}
			int num = 0;
			if (keyboardMaps != null)
			{
				for (int i = 0; i < keyboardMaps.Count; i++)
				{
					if (keyboardMaps[i].layoutId == id)
					{
						num++;
					}
				}
			}
			return num;
		}

		public int GetKeyboardLayoutIndex(int id)
		{
			if (keyboardLayouts == null)
			{
				return 0;
			}
			for (int i = 0; i < keyboardLayouts.Count; i++)
			{
				if (keyboardLayouts[i].id == id)
				{
					return i;
				}
			}
			return -1;
		}

		public string[] GetKeyboardLayoutNames()
		{
			if (keyboardLayouts == null)
			{
				return null;
			}
			string[] array = new string[keyboardLayouts.Count];
			for (int i = 0; i < keyboardLayouts.Count; i++)
			{
				array[i] = keyboardLayouts[i].name;
			}
			return array;
		}

		public int[] GetKeyboardLayoutIds()
		{
			if (keyboardLayouts == null)
			{
				return null;
			}
			int[] array = new int[keyboardLayouts.Count];
			for (int i = 0; i < keyboardLayouts.Count; i++)
			{
				array[i] = keyboardLayouts[i].id;
			}
			return array;
		}

		public InputLayout GetKeyboardLayout(int index)
		{
			if (keyboardLayouts == null || index < 0 || index >= keyboardLayouts.Count)
			{
				return null;
			}
			return keyboardLayouts[index];
		}

		public InputLayout GetKeyboardLayout(string name)
		{
			if (keyboardLayouts == null)
			{
				return null;
			}
			int num = IndexOfKeyboardLayout(name);
			if (num < 0)
			{
				return null;
			}
			return keyboardLayouts[num];
		}

		public InputLayout GetKeyboardLayoutById(int id)
		{
			if (keyboardLayouts == null)
			{
				return null;
			}
			int num = IndexOfKeyboardLayout(id);
			if (num < 0)
			{
				return null;
			}
			return keyboardLayouts[num];
		}

		public int GetKeyboardLayoutId(string name)
		{
			if (keyboardLayouts == null)
			{
				return -1;
			}
			int num = IndexOfKeyboardLayout(name);
			if (num < 0)
			{
				return -1;
			}
			return keyboardLayouts[num].id;
		}

		public int IndexOfKeyboardLayout(int id)
		{
			if (keyboardLayouts == null)
			{
				return -1;
			}
			for (int i = 0; i < keyboardLayouts.Count; i++)
			{
				if (keyboardLayouts[i].id == id)
				{
					return i;
				}
			}
			return -1;
		}

		public int IndexOfKeyboardLayout(string name)
		{
			if (name == null || name == string.Empty)
			{
				return -1;
			}
			if (keyboardLayouts == null)
			{
				return -1;
			}
			for (int i = 0; i < keyboardLayouts.Count; i++)
			{
				if (keyboardLayouts[i].name.Equals(name, StringComparison.OrdinalIgnoreCase))
				{
					return i;
				}
			}
			return -1;
		}

		public string GetKeyboardLayoutNameById(int id)
		{
			if (keyboardLayouts != null)
			{
				for (int i = 0; i < keyboardLayouts.Count; i++)
				{
					if (keyboardLayouts[i].id == id)
					{
						return keyboardLayouts[i].name;
					}
				}
			}
			return "Unknown";
		}

		public void AddMouseLayout()
		{
			mouseLayouts.Add(rpSNaZZDZzcZlrEJdUSlCUOVkdnW());
		}

		public void InsertMouseLayout(int index)
		{
			if (index < 0 || index >= mouseLayouts.Count)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			mouseLayouts.Insert(index, rpSNaZZDZzcZlrEJdUSlCUOVkdnW());
		}

		public void DeleteMouseLayout(int index)
		{
			if (mouseLayouts == null || index < 0 || index >= mouseLayouts.Count)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			int id = mouseLayouts[index].id;
			if (mouseMaps != null)
			{
				for (int num = mouseMaps.Count - 1; num >= 0; num--)
				{
					if (mouseMaps[num].layoutId == id)
					{
						mouseMaps.RemoveAt(num);
					}
				}
			}
			if (players != null)
			{
				Action<List<Player_Editor.Mapping>, int> action = BqxrkBNzJTKgNcopOxEBoFgXGtpFA._003C_003E9.QbXeASbrswitYDXztjAbeVyfvtajB;
				for (int i = 0; i < players.Count; i++)
				{
					Player_Editor player_Editor = players[i];
					if (player_Editor != null)
					{
						action(player_Editor.defaultMouseMaps, id);
					}
				}
			}
			mouseLayouts.RemoveAt(index);
		}

		public bool ReorderMouseLayout(int index, bool offsetDown, bool offsetNow)
		{
			return ListTools.OffsetAtIndex(mouseLayouts, index, offsetDown, offsetNow);
		}

		public void DuplicateMouseLayout(int index, bool duplicateMaps)
		{
			if (mouseLayouts == null || index < 0 || index >= mouseLayouts.Count)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			InputLayout inputLayout = mouseLayouts[index].Clone();
			inputLayout.id = GetNewMouseLayoutId();
			inputLayout.name = StringTools.IterateName(inputLayout.name, -1, GetMouseLayoutNames());
			if (index == mouseLayouts.Count - 1)
			{
				mouseLayouts.Add(inputLayout);
			}
			else
			{
				mouseLayouts.Insert(index + 1, inputLayout);
			}
			if (!duplicateMaps)
			{
				return;
			}
			int id = inputLayout.id;
			int id2 = mouseLayouts[index].id;
			if (mouseMaps == null)
			{
				return;
			}
			for (int num = mouseMaps.Count - 1; num >= 0; num--)
			{
				if (mouseMaps[num].layoutId == id2)
				{
					int num2 = DuplicateMouseMap(num);
					if (num2 >= 0)
					{
						mouseMaps[num2].layoutId = id;
					}
				}
			}
		}

		public int GetMouseLayoutMapCount(int id)
		{
			if (mouseLayouts == null)
			{
				return 0;
			}
			int num = 0;
			if (mouseMaps != null)
			{
				for (int i = 0; i < mouseMaps.Count; i++)
				{
					if (mouseMaps[i].layoutId == id)
					{
						num++;
					}
				}
			}
			return num;
		}

		public int GetMouseLayoutIndex(int id)
		{
			if (mouseLayouts == null)
			{
				return 0;
			}
			for (int i = 0; i < mouseLayouts.Count; i++)
			{
				if (mouseLayouts[i].id == id)
				{
					return i;
				}
			}
			return -1;
		}

		public string[] GetMouseLayoutNames()
		{
			if (mouseLayouts == null)
			{
				return null;
			}
			string[] array = new string[mouseLayouts.Count];
			for (int i = 0; i < mouseLayouts.Count; i++)
			{
				array[i] = mouseLayouts[i].name;
			}
			return array;
		}

		public int[] GetMouseLayoutIds()
		{
			if (mouseLayouts == null)
			{
				return null;
			}
			int[] array = new int[mouseLayouts.Count];
			for (int i = 0; i < mouseLayouts.Count; i++)
			{
				array[i] = mouseLayouts[i].id;
			}
			return array;
		}

		public InputLayout GetMouseLayout(int index)
		{
			if (mouseLayouts == null || index < 0 || index >= mouseLayouts.Count)
			{
				return null;
			}
			return mouseLayouts[index];
		}

		public InputLayout GetMouseLayout(string name)
		{
			if (mouseLayouts == null)
			{
				return null;
			}
			int num = IndexOfMouseLayout(name);
			if (num < 0)
			{
				return null;
			}
			return mouseLayouts[num];
		}

		public InputLayout GetMouseLayoutById(int id)
		{
			if (mouseLayouts == null)
			{
				return null;
			}
			int num = IndexOfMouseLayout(id);
			if (num < 0)
			{
				return null;
			}
			return mouseLayouts[num];
		}

		public int GetMouseLayoutId(string name)
		{
			if (mouseLayouts == null)
			{
				return -1;
			}
			int num = IndexOfMouseLayout(name);
			if (num < 0)
			{
				return -1;
			}
			return mouseLayouts[num].id;
		}

		public int IndexOfMouseLayout(int id)
		{
			if (mouseLayouts == null)
			{
				return -1;
			}
			for (int i = 0; i < mouseLayouts.Count; i++)
			{
				if (mouseLayouts[i].id == id)
				{
					return i;
				}
			}
			return -1;
		}

		public int IndexOfMouseLayout(string name)
		{
			if (name == null || name == string.Empty)
			{
				return -1;
			}
			if (mouseLayouts == null)
			{
				return -1;
			}
			for (int i = 0; i < mouseLayouts.Count; i++)
			{
				if (mouseLayouts[i].name.Equals(name, StringComparison.OrdinalIgnoreCase))
				{
					return i;
				}
			}
			return -1;
		}

		public string GetMouseLayoutNameById(int id)
		{
			if (mouseLayouts != null)
			{
				for (int i = 0; i < mouseLayouts.Count; i++)
				{
					if (mouseLayouts[i].id == id)
					{
						return mouseLayouts[i].name;
					}
				}
			}
			return "Unknown";
		}

		public void AddCustomControllerLayout()
		{
			customControllerLayouts.Add(lLRFyHcCwYgxGHVxxbgwMDdmOUfA());
		}

		public void InsertCustomControllerLayout(int index)
		{
			if (index < 0 || index >= customControllerLayouts.Count)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			customControllerLayouts.Insert(index, lLRFyHcCwYgxGHVxxbgwMDdmOUfA());
		}

		public void DeleteCustomControllerLayout(int index)
		{
			if (customControllerLayouts == null || index < 0 || index >= customControllerLayouts.Count)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			int id = customControllerLayouts[index].id;
			if (customControllerMaps != null)
			{
				for (int num = customControllerMaps.Count - 1; num >= 0; num--)
				{
					if (customControllerMaps[num].layoutId == id)
					{
						customControllerMaps.RemoveAt(num);
					}
				}
			}
			if (players != null)
			{
				Action<List<Player_Editor.Mapping>, int> action = BqxrkBNzJTKgNcopOxEBoFgXGtpFA._003C_003E9.waPAeydhwoJBtdNkTOMzPhTaMxXm;
				for (int i = 0; i < players.Count; i++)
				{
					Player_Editor player_Editor = players[i];
					if (player_Editor != null)
					{
						action(player_Editor.defaultCustomControllerMaps, id);
					}
				}
			}
			customControllerLayouts.RemoveAt(index);
		}

		public bool ReorderCustomControllerLayout(int index, bool offsetDown, bool offsetNow)
		{
			return ListTools.OffsetAtIndex(customControllerLayouts, index, offsetDown, offsetNow);
		}

		public void DuplicateCustomControllerLayout(int index, bool duplicateMaps)
		{
			if (customControllerLayouts == null || index < 0 || index >= customControllerLayouts.Count)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			InputLayout inputLayout = customControllerLayouts[index].Clone();
			inputLayout.id = GetNewCustomControllerLayoutId();
			inputLayout.name = StringTools.IterateName(inputLayout.name, -1, GetCustomControllerLayoutNames());
			if (index == customControllerLayouts.Count - 1)
			{
				customControllerLayouts.Add(inputLayout);
			}
			else
			{
				customControllerLayouts.Insert(index + 1, inputLayout);
			}
			if (!duplicateMaps)
			{
				return;
			}
			int id = inputLayout.id;
			int id2 = customControllerLayouts[index].id;
			if (customControllerMaps == null)
			{
				return;
			}
			for (int num = customControllerMaps.Count - 1; num >= 0; num--)
			{
				if (customControllerMaps[num].layoutId == id2)
				{
					int num2 = DuplicateCustomControllerMap(num);
					if (num2 >= 0)
					{
						customControllerMaps[num2].layoutId = id;
					}
				}
			}
		}

		public int GetCustomControllerLayoutMapCount(int id)
		{
			if (customControllerLayouts == null)
			{
				return 0;
			}
			int num = 0;
			if (customControllerMaps != null)
			{
				for (int i = 0; i < customControllerMaps.Count; i++)
				{
					if (customControllerMaps[i].layoutId == id)
					{
						num++;
					}
				}
			}
			return num;
		}

		public int GetCustomControllerLayoutIndex(int id)
		{
			if (customControllerLayouts == null)
			{
				return 0;
			}
			for (int i = 0; i < customControllerLayouts.Count; i++)
			{
				if (customControllerLayouts[i].id == id)
				{
					return i;
				}
			}
			return -1;
		}

		public string[] GetCustomControllerLayoutNames()
		{
			if (customControllerLayouts == null)
			{
				return null;
			}
			string[] array = new string[customControllerLayouts.Count];
			for (int i = 0; i < customControllerLayouts.Count; i++)
			{
				array[i] = customControllerLayouts[i].name;
			}
			return array;
		}

		public int[] GetCustomControllerLayoutIds()
		{
			if (customControllerLayouts == null)
			{
				return null;
			}
			int[] array = new int[customControllerLayouts.Count];
			for (int i = 0; i < customControllerLayouts.Count; i++)
			{
				array[i] = customControllerLayouts[i].id;
			}
			return array;
		}

		public InputLayout GetCustomControllerLayout(int index)
		{
			if (customControllerLayouts == null || index < 0 || index >= customControllerLayouts.Count)
			{
				return null;
			}
			return customControllerLayouts[index];
		}

		public InputLayout GetCustomControllerLayout(string name)
		{
			if (customControllerLayouts == null)
			{
				return null;
			}
			int num = IndexOfCustomControllerLayout(name);
			if (num < 0)
			{
				return null;
			}
			return customControllerLayouts[num];
		}

		public InputLayout GetCustomControllerLayoutById(int id)
		{
			if (customControllerLayouts == null)
			{
				return null;
			}
			int num = IndexOfCustomControllerLayout(id);
			if (num < 0)
			{
				return null;
			}
			return customControllerLayouts[num];
		}

		public int GetCustomControllerLayoutId(string name)
		{
			if (customControllerLayouts == null)
			{
				return -1;
			}
			int num = IndexOfCustomControllerLayout(name);
			if (num < 0)
			{
				return -1;
			}
			return customControllerLayouts[num].id;
		}

		public int IndexOfCustomControllerLayout(int id)
		{
			if (customControllerLayouts == null)
			{
				return -1;
			}
			for (int i = 0; i < customControllerLayouts.Count; i++)
			{
				if (customControllerLayouts[i].id == id)
				{
					return i;
				}
			}
			return -1;
		}

		public int IndexOfCustomControllerLayout(string name)
		{
			if (name == null || name == string.Empty)
			{
				return -1;
			}
			if (customControllerLayouts == null)
			{
				return -1;
			}
			for (int i = 0; i < customControllerLayouts.Count; i++)
			{
				if (customControllerLayouts[i].name.Equals(name, StringComparison.OrdinalIgnoreCase))
				{
					return i;
				}
			}
			return -1;
		}

		public string GetCustomControllerLayoutNameById(int id)
		{
			if (customControllerLayouts != null)
			{
				for (int i = 0; i < customControllerLayouts.Count; i++)
				{
					if (customControllerLayouts[i].id == id)
					{
						return customControllerLayouts[i].name;
					}
				}
			}
			return "Unknown";
		}

		public string GetLayoutNameById(ControllerType controllerType, int id)
		{
			return controllerType switch
			{
				ControllerType.Joystick => GetJoystickLayoutNameById(id), 
				ControllerType.Keyboard => GetKeyboardLayoutNameById(id), 
				ControllerType.Mouse => GetMouseLayoutNameById(id), 
				ControllerType.Custom => GetCustomControllerLayoutNameById(id), 
				_ => throw new NotImplementedException(), 
			};
		}

		internal ControllerMap mlHpGZlwlGuuSPQRGehERBHqzRhy(Controller P_0, int P_1, int P_2)
		{
			if (P_0 == null)
			{
				return null;
			}
			return P_0.type switch
			{
				ControllerType.Joystick => vAHiNYrtViGADjHOxDfcaDShypcZb((Joystick)P_0, P_1, P_2), 
				ControllerType.Keyboard => FindKeyboardMap_Game((Keyboard)P_0, P_1, P_2), 
				ControllerType.Mouse => FindMouseMap_Game((Mouse)P_0, P_1, P_2), 
				ControllerType.Custom => MNKkjYicKUjWIvlytoqoHPNWWAfD(P_1, ((CustomController)P_0).sourceControllerId, P_2), 
				_ => throw new NotImplementedException(), 
			};
		}

		public ControllerMap_Editor GetJoystickMap(int categoryId, Guid hardwareGuid, int layoutId)
		{
			if (joystickMaps == null)
			{
				return null;
			}
			for (int i = 0; i < joystickMaps.Count; i++)
			{
				if (joystickMaps[i].categoryId == categoryId && joystickMaps[i].layoutId == layoutId && StringTools.ToGuid(joystickMaps[i].hardwareGuidString) == hardwareGuid)
				{
					return joystickMaps[i];
				}
			}
			return null;
		}

		public ControllerMap_Editor GetJoystickMapById(int id, out int joystickMapIndex)
		{
			joystickMapIndex = -1;
			if (joystickMaps == null)
			{
				return null;
			}
			for (int i = 0; i < joystickMaps.Count; i++)
			{
				if (joystickMaps[i].id == id)
				{
					joystickMapIndex = i;
					return joystickMaps[i];
				}
			}
			return null;
		}

		public List<ControllerMap_Editor> GetJoystickMaps(Guid hardwareGuid)
		{
			if (joystickMaps == null)
			{
				return null;
			}
			List<ControllerMap_Editor> list = new List<ControllerMap_Editor>();
			for (int i = 0; i < joystickMaps.Count; i++)
			{
				if (StringTools.ToGuid(joystickMaps[i].hardwareGuidString) == hardwareGuid)
				{
					list.Add(joystickMaps[i]);
				}
			}
			return list;
		}

		public int GetJoystickMapId(int categoryId, Guid hardwareGuid, int layoutId)
		{
			if (joystickMaps == null)
			{
				return -1;
			}
			for (int i = 0; i < joystickMaps.Count; i++)
			{
				if (joystickMaps[i].categoryId == categoryId && joystickMaps[i].layoutId == layoutId && StringTools.ToGuid(joystickMaps[i].hardwareGuidString) == hardwareGuid)
				{
					return joystickMaps[i].id;
				}
			}
			return -1;
		}

		public bool HasJoystickMap(int categoryId, Guid hardwareGuid, int layoutId)
		{
			if (joystickMaps == null)
			{
				return false;
			}
			for (int i = 0; i < joystickMaps.Count; i++)
			{
				if (joystickMaps[i].categoryId == categoryId && joystickMaps[i].layoutId == layoutId && StringTools.ToGuid(joystickMaps[i].hardwareGuidString) == hardwareGuid)
				{
					return true;
				}
			}
			return false;
		}

		public bool HasJoystickMap(Guid hardwareGuid)
		{
			if (joystickMaps == null)
			{
				return false;
			}
			for (int i = 0; i < joystickMaps.Count; i++)
			{
				if (StringTools.ToGuid(joystickMaps[i].hardwareGuidString) == hardwareGuid)
				{
					return true;
				}
			}
			return false;
		}

		public bool HasJoystickMapInCategory(Guid hardwareGuid, int categoryId)
		{
			if (joystickMaps == null)
			{
				return false;
			}
			for (int i = 0; i < joystickMaps.Count; i++)
			{
				if (StringTools.ToGuid(joystickMaps[i].hardwareGuidString) == hardwareGuid && joystickMaps[i].categoryId == categoryId)
				{
					return true;
				}
			}
			return false;
		}

		public bool CreateJoystickMap(int categoryId, Guid joystickOrTemplateGuid, int layoutId)
		{
			if (joystickMaps == null)
			{
				joystickMaps = new List<ControllerMap_Editor>();
			}
			ControllerMap_Editor controllerMap_Editor = new ControllerMap_Editor();
			controllerMap_Editor.id = GetNewJoystickMapId();
			controllerMap_Editor.categoryId = categoryId;
			controllerMap_Editor.layoutId = layoutId;
			controllerMap_Editor.hardwareGuidString = joystickOrTemplateGuid.ToString();
			joystickMaps.Add(controllerMap_Editor);
			return false;
		}

		public void DeleteJoystickMap(int id)
		{
			if (joystickMaps == null)
			{
				return;
			}
			for (int num = joystickMaps.Count - 1; num >= 0; num--)
			{
				if (joystickMaps[num].id == id)
				{
					joystickMaps.RemoveAt(num);
				}
			}
		}

		public int DuplicateJoystickMap(int index)
		{
			if (joystickMaps == null || index < 0 || index >= joystickMaps.Count)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			ControllerMap_Editor controllerMap_Editor = joystickMaps[index].Clone();
			controllerMap_Editor.id = GetNewJoystickMapId();
			joystickMaps.Add(controllerMap_Editor);
			return joystickMaps.Count - 1;
		}

		internal JoystickMap XmgsLQKXkLUGIsMVxjQlJrvzzSFL(HardwareControllerMapIdentifier P_0, int P_1, int P_2)
		{
			return KyXFkaJcFyldrJcJinxBAZrxZISHA(new HardwareControllerMapIdentifier(P_0.guid, P_0.inputSource, P_0.actualInputPlatform, P_0.variantIndex), P_1, P_2);
		}

		internal JoystickMap vAHiNYrtViGADjHOxDfcaDShypcZb(Joystick P_0, int P_1, int P_2)
		{
			if (P_0 == null)
			{
				return null;
			}
			return KyXFkaJcFyldrJcJinxBAZrxZISHA(P_0.BbaRKBqKWlkxZvWWKhByvwbeMuIC, P_1, P_2);
		}

		private JoystickMap KyXFkaJcFyldrJcJinxBAZrxZISHA(HardwareControllerMapIdentifier P_0, int P_1, int P_2)
		{
			Guid guid = P_0.guid;
			HardwareJoystickMap hardwareJoystickMap = ReInput.mPDhHJaciMKPHWhrdsNxKsGzQEcJ(guid);
			ControllerMap_Editor controllerMap_Editor = izojCUgPkgFINNALbMVduvZeGFgu(P_1, guid, P_2, false);
			if (controllerMap_Editor != null)
			{
				JoystickMap joystickMap = controllerMap_Editor.HmtEXGiPhJFBDjZFgEHxiqSkAnWl(containsActionDelegate, P_0, hardwareJoystickMap, controllerMap_Editor.hardwareGuid == ReInput.defaultHardwareJoystickMapGuid);
				joystickMap.LpfidFzDLEdNHIRSSPKMLjUqaoaLA(guid, P_1, P_2);
				return joystickMap;
			}
			if (hardwareJoystickMap != null)
			{
				foreach (Guid templateGuid in hardwareJoystickMap.TemplateGuids)
				{
					if (templateGuid == Guid.Empty)
					{
						continue;
					}
					HardwareJoystickTemplateMap hardwareJoystickTemplateMap = ReInput.aJxRUejDypCmjlkaIdpomVPvZxgi(templateGuid);
					if (!(hardwareJoystickTemplateMap != null))
					{
						continue;
					}
					controllerMap_Editor = izojCUgPkgFINNALbMVduvZeGFgu(P_1, templateGuid, P_2, false);
					if (controllerMap_Editor != null)
					{
						JoystickMap joystickMap = neCgzUsddktcqMaGtJMoVpoKfChDA(P_0, controllerMap_Editor, hardwareJoystickTemplateMap, hardwareJoystickMap, P_1, P_2);
						if (joystickMap != null)
						{
							joystickMap.LpfidFzDLEdNHIRSSPKMLjUqaoaLA(guid, P_1, P_2);
							return joystickMap;
						}
					}
				}
			}
			if (guid == Guid.Empty)
			{
				controllerMap_Editor = izojCUgPkgFINNALbMVduvZeGFgu(P_1, Guid.Empty, P_2, false);
				if (controllerMap_Editor != null)
				{
					JoystickMap joystickMap = controllerMap_Editor.HmtEXGiPhJFBDjZFgEHxiqSkAnWl(containsActionDelegate, P_0, null, controllerMap_Editor.hardwareGuid == ReInput.defaultHardwareJoystickMapGuid);
					joystickMap.LpfidFzDLEdNHIRSSPKMLjUqaoaLA(guid, P_1, P_2);
					if (joystickMap != null)
					{
						return joystickMap;
					}
				}
			}
			return JoystickMap.PfrqCKEIgMPkwkEXazIGyUcXeKcP(guid, P_1, P_2);
		}

		private ControllerMap_Editor izojCUgPkgFINNALbMVduvZeGFgu(int P_0, Guid P_1, int P_2, bool P_3)
		{
			ControllerMap_Editor joystickMap = GetJoystickMap(P_0, P_1, P_2);
			if (joystickMap != null)
			{
				return joystickMap;
			}
			if (P_3)
			{
				joystickMap = BcyivlzvPHfzyjkELzAfZYWsjHZP(P_0, P_1, P_2);
				if (joystickMap != null)
				{
					return joystickMap;
				}
			}
			return null;
		}

		private ControllerMap_Editor BcyivlzvPHfzyjkELzAfZYWsjHZP(int P_0, Guid P_1, int P_2)
		{
			List<ControllerMap_Editor> list = GetJoystickMaps(P_1);
			if (list != null && list.Count > 0)
			{
				yYwGeverSjtLCoPnzCUgHFreNWLyA(list, joystickLayouts);
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].categoryId == P_0)
					{
						return list[i];
					}
				}
				for (int j = 0; j < list.Count; j++)
				{
					if (list[j].categoryId == 0)
					{
						return list[j];
					}
				}
			}
			return null;
		}

		private JoystickMap neCgzUsddktcqMaGtJMoVpoKfChDA(HardwareControllerMapIdentifier P_0, ControllerMap_Editor P_1, HardwareJoystickTemplateMap P_2, HardwareJoystickMap P_3, int P_4, int P_5)
		{
			if (P_2 == null)
			{
				return null;
			}
			ControllerMap_Editor controllerMap_Editor = P_1.Clone();
			if (!P_2.ozIsgQebUHIVNlcdKAhLHhyLxQSdA(controllerMap_Editor, P_3, P_0.guid, out var text))
			{
				Logger.LogError("Error remapping joystick template " + P_2.Guid.ToString() + " to joystick " + P_0.guid.ToString() + "\nReason: " + text);
				return null;
			}
			return controllerMap_Editor.HmtEXGiPhJFBDjZFgEHxiqSkAnWl(containsActionDelegate, P_0, P_3, controllerMap_Editor.hardwareGuid == ReInput.defaultHardwareJoystickMapGuid);
		}

		private JoystickMap VFUNUBLlmsgYUeqWWyFvwqJHdPEsA(JoystickMap P_0, HardwareControllerMapIdentifier P_1)
		{
			if (P_0 == null)
			{
				return null;
			}
			HardwareJoystickMap hardwareJoystickMap = ReInput.mPDhHJaciMKPHWhrdsNxKsGzQEcJ(P_0.hardwareGuid);
			if (hardwareJoystickMap == null)
			{
				return null;
			}
			HardwareJoystickMap hardwareJoystickMap2 = ReInput.mPDhHJaciMKPHWhrdsNxKsGzQEcJ(Guid.Empty);
			if (hardwareJoystickMap2 == null)
			{
				return null;
			}
			hardwareJoystickMap.GetElementIdentifiersForControllerElements(P_1, isDefaultMap: false, out var buttons, out var axes);
			if (buttons == null && axes == null)
			{
				return null;
			}
			bool flag = false;
			List<int> list = new List<int>();
			foreach (ActionElementMap allMap in P_0.AllMaps)
			{
				ControllerElementIdentifier elementIdentifier = hardwareJoystickMap2.GetElementIdentifier(allMap._elementIdentifierId);
				if (elementIdentifier != null)
				{
					string text = elementIdentifier.Rewired_002EInterfaces_002EIControllerElementIdentifierCommon_Internal_002Ename;
					if (!string.IsNullOrEmpty(text))
					{
						int num = 0;
						int num2 = text.IndexOf("button", 0, StringComparison.OrdinalIgnoreCase);
						if (num2 < 0)
						{
							num2 = text.IndexOf("axis", 0, StringComparison.OrdinalIgnoreCase);
							num = 1;
						}
						if (num2 >= 0 && (num != 0 || buttons != null) && (num != 1 || axes != null))
						{
							string text2 = Regex.Replace(text, "[^0-9]+", "");
							Logger.Log(text2);
							if (int.TryParse(text2, out var result))
							{
								if (num == 0)
								{
									if (result < buttons.Length)
									{
										allMap._elementIdentifierId = buttons[result];
										goto IL_011f;
									}
								}
								else if (result < axes.Length)
								{
									allMap._elementIdentifierId = axes[result];
									goto IL_011f;
								}
							}
						}
					}
				}
				list.Add(allMap.nJilCjIhFvMUTsTBcUWuYpormNsu);
				continue;
				IL_011f:
				flag = true;
			}
			for (int i = 0; i < list.Count; i++)
			{
				P_0.DeleteElementMap(list[i]);
			}
			if (!flag)
			{
				return null;
			}
			return P_0;
		}

		public ControllerMap_Editor GetKeyboardMap(int categoryId, int layoutId)
		{
			if (keyboardMaps == null)
			{
				return null;
			}
			for (int i = 0; i < keyboardMaps.Count; i++)
			{
				if (keyboardMaps[i].categoryId == categoryId && keyboardMaps[i].layoutId == layoutId)
				{
					return keyboardMaps[i];
				}
			}
			return null;
		}

		public int GetKeyboardMapId(int categoryId, int layoutId)
		{
			if (keyboardMaps == null)
			{
				return -1;
			}
			for (int i = 0; i < keyboardMaps.Count; i++)
			{
				if (keyboardMaps[i].categoryId == categoryId && keyboardMaps[i].layoutId == layoutId)
				{
					return keyboardMaps[i].id;
				}
			}
			return -1;
		}

		public bool HasKeyboardMap(int categoryId, Guid hardwareGuid, int layoutId)
		{
			if (keyboardMaps == null)
			{
				return false;
			}
			for (int i = 0; i < keyboardMaps.Count; i++)
			{
				if (keyboardMaps[i].categoryId == categoryId && keyboardMaps[i].layoutId == layoutId && StringTools.ToGuid(keyboardMaps[i].hardwareGuidString) == hardwareGuid)
				{
					return true;
				}
			}
			return false;
		}

		public bool CreateKeyboardMap(int categoryId, int layoutId)
		{
			if (keyboardMaps == null)
			{
				keyboardMaps = new List<ControllerMap_Editor>();
			}
			ControllerMap_Editor controllerMap_Editor = new ControllerMap_Editor();
			controllerMap_Editor.id = GetNewKeyboardMapId();
			controllerMap_Editor.categoryId = categoryId;
			controllerMap_Editor.layoutId = layoutId;
			keyboardMaps.Add(controllerMap_Editor);
			return false;
		}

		public void DeleteKeyboardMap(int id)
		{
			if (keyboardMaps == null)
			{
				return;
			}
			for (int num = keyboardMaps.Count - 1; num >= 0; num--)
			{
				if (keyboardMaps[num].id == id)
				{
					keyboardMaps.RemoveAt(num);
				}
			}
		}

		public int DuplicateKeyboardMap(int index)
		{
			if (keyboardMaps == null || index < 0 || index >= keyboardMaps.Count)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			ControllerMap_Editor controllerMap_Editor = keyboardMaps[index].Clone();
			controllerMap_Editor.id = GetNewKeyboardMapId();
			keyboardMaps.Add(controllerMap_Editor);
			return keyboardMaps.Count - 1;
		}

		public ControllerMap_Editor GetKeyboardMapById(int id, out int keyboardMapIndex)
		{
			keyboardMapIndex = -1;
			if (keyboardMaps == null)
			{
				return null;
			}
			for (int i = 0; i < keyboardMaps.Count; i++)
			{
				if (keyboardMaps[i].id == id)
				{
					keyboardMapIndex = i;
					return keyboardMaps[i];
				}
			}
			return null;
		}

		public KeyboardMap FindKeyboardMap_Game(Keyboard keyboard, int categoryId, int layoutId)
		{
			ControllerMap_Editor controllerMap_Editor = llWoMUcwuQDldmqlmoFJkVqRCYAd(keyboardMaps, keyboardLayouts, categoryId, layoutId, false);
			KeyboardMap keyboardMap;
			if (controllerMap_Editor != null)
			{
				keyboardMap = controllerMap_Editor.MiKHWjwNQMtsJoGokzWUTjqgUeQN(containsActionDelegate);
				keyboardMap.gAUNPMUaaUKLvNZNbIwQEoXbsUzu(keyboard.zyYehdPaDXciYCtKVPxEsznJTyqP, categoryId, layoutId);
			}
			else
			{
				keyboardMap = KeyboardMap.ZjKfUjGyzyNIUfydHfYlzgEqlodt(keyboard.zyYehdPaDXciYCtKVPxEsznJTyqP, categoryId, layoutId);
			}
			return keyboardMap;
		}

		public bool HasKeyboardMapInCategory(int categoryId)
		{
			if (keyboardMaps == null)
			{
				return false;
			}
			for (int i = 0; i < keyboardMaps.Count; i++)
			{
				if (keyboardMaps[i].categoryId == categoryId)
				{
					return true;
				}
			}
			return false;
		}

		public bool HasKeyboardMapInLayout(int categoryId, int layoutId)
		{
			if (keyboardMaps == null)
			{
				return false;
			}
			for (int i = 0; i < keyboardMaps.Count; i++)
			{
				if (keyboardMaps[i].categoryId == categoryId && keyboardMaps[i].layoutId == layoutId)
				{
					return true;
				}
			}
			return false;
		}

		public ControllerMap_Editor GetMouseMap(int categoryId, int layoutId)
		{
			if (mouseMaps == null)
			{
				return null;
			}
			for (int i = 0; i < mouseMaps.Count; i++)
			{
				if (mouseMaps[i].categoryId == categoryId && mouseMaps[i].layoutId == layoutId)
				{
					return mouseMaps[i];
				}
			}
			return null;
		}

		public int GetMouseMapId(int categoryId, int layoutId)
		{
			if (mouseMaps == null)
			{
				return -1;
			}
			for (int i = 0; i < mouseMaps.Count; i++)
			{
				if (mouseMaps[i].categoryId == categoryId && mouseMaps[i].layoutId == layoutId)
				{
					return mouseMaps[i].id;
				}
			}
			return -1;
		}

		public bool HasMouseMap(int categoryId, Guid hardwareGuid, int layoutId)
		{
			if (mouseMaps == null)
			{
				return false;
			}
			for (int i = 0; i < mouseMaps.Count; i++)
			{
				if (mouseMaps[i].categoryId == categoryId && mouseMaps[i].layoutId == layoutId && StringTools.ToGuid(mouseMaps[i].hardwareGuidString) == hardwareGuid)
				{
					return true;
				}
			}
			return false;
		}

		public bool CreateMouseMap(int categoryId, int layoutId)
		{
			if (mouseMaps == null)
			{
				mouseMaps = new List<ControllerMap_Editor>();
			}
			ControllerMap_Editor controllerMap_Editor = new ControllerMap_Editor();
			controllerMap_Editor.id = GetNewMouseMapId();
			controllerMap_Editor.categoryId = categoryId;
			controllerMap_Editor.layoutId = layoutId;
			mouseMaps.Add(controllerMap_Editor);
			return false;
		}

		public void DeleteMouseMap(int id)
		{
			if (mouseMaps == null)
			{
				return;
			}
			for (int num = mouseMaps.Count - 1; num >= 0; num--)
			{
				if (mouseMaps[num].id == id)
				{
					mouseMaps.RemoveAt(num);
				}
			}
		}

		public int DuplicateMouseMap(int index)
		{
			if (mouseMaps == null || index < 0 || index >= mouseMaps.Count)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			ControllerMap_Editor controllerMap_Editor = mouseMaps[index].Clone();
			controllerMap_Editor.id = GetNewMouseMapId();
			mouseMaps.Add(controllerMap_Editor);
			return mouseMaps.Count - 1;
		}

		public ControllerMap_Editor GetMouseMapById(int id, out int mouseMapIndex)
		{
			mouseMapIndex = -1;
			if (mouseMaps == null)
			{
				return null;
			}
			for (int i = 0; i < mouseMaps.Count; i++)
			{
				if (mouseMaps[i].id == id)
				{
					mouseMapIndex = i;
					return mouseMaps[i];
				}
			}
			return null;
		}

		public MouseMap FindMouseMap_Game(Mouse mouse, int categoryId, int layoutId)
		{
			ControllerMap_Editor controllerMap_Editor = llWoMUcwuQDldmqlmoFJkVqRCYAd(mouseMaps, mouseLayouts, categoryId, layoutId, false);
			MouseMap mouseMap;
			if (controllerMap_Editor != null)
			{
				mouseMap = controllerMap_Editor.xiVcMoyOaQYFCzHpRSCcraHKrfb(containsActionDelegate);
				mouseMap.JimgaTjZkclOEUoSnpkWMcOaDhYr(mouse.zyYehdPaDXciYCtKVPxEsznJTyqP, categoryId, layoutId);
			}
			else
			{
				mouseMap = MouseMap.JhOhEpSscAngETaHuKgQZGczibSC(mouse.zyYehdPaDXciYCtKVPxEsznJTyqP, categoryId, layoutId);
			}
			return mouseMap;
		}

		public bool HasMouseMapInCategory(int categoryId)
		{
			if (mouseMaps == null)
			{
				return false;
			}
			for (int i = 0; i < mouseMaps.Count; i++)
			{
				if (mouseMaps[i].categoryId == categoryId)
				{
					return true;
				}
			}
			return false;
		}

		public bool HasMouseMapInLayout(int categoryId, int layoutId)
		{
			if (mouseMaps == null)
			{
				return false;
			}
			for (int i = 0; i < mouseMaps.Count; i++)
			{
				if (mouseMaps[i].categoryId == categoryId && mouseMaps[i].layoutId == layoutId)
				{
					return true;
				}
			}
			return false;
		}

		public ControllerMap_Editor GetCustomControllerMap(int categoryId, int controllerUid, int layoutId)
		{
			if (customControllerMaps == null)
			{
				return null;
			}
			for (int i = 0; i < customControllerMaps.Count; i++)
			{
				if (customControllerMaps[i].categoryId == categoryId && customControllerMaps[i].layoutId == layoutId && customControllerMaps[i].customControllerUid == controllerUid)
				{
					return customControllerMaps[i];
				}
			}
			return null;
		}

		public ControllerMap_Editor GetCustomControllerMapById(int mapId, out int customControllerMapIndex)
		{
			customControllerMapIndex = -1;
			if (customControllerMaps == null)
			{
				return null;
			}
			for (int i = 0; i < customControllerMaps.Count; i++)
			{
				if (customControllerMaps[i].id == mapId)
				{
					customControllerMapIndex = i;
					return customControllerMaps[i];
				}
			}
			return null;
		}

		public List<ControllerMap_Editor> GetCustomControllerMaps(int controllerUid)
		{
			if (customControllerMaps == null)
			{
				return null;
			}
			List<ControllerMap_Editor> list = new List<ControllerMap_Editor>();
			for (int i = 0; i < customControllerMaps.Count; i++)
			{
				if (customControllerMaps[i].customControllerUid == controllerUid)
				{
					list.Add(customControllerMaps[i]);
				}
			}
			return list;
		}

		public int GetCustomControllerMapId(int categoryId, int controllerUid, int layoutId)
		{
			if (customControllerMaps == null)
			{
				return -1;
			}
			for (int i = 0; i < customControllerMaps.Count; i++)
			{
				if (customControllerMaps[i].categoryId == categoryId && customControllerMaps[i].layoutId == layoutId && customControllerMaps[i].customControllerUid == controllerUid)
				{
					return customControllerMaps[i].id;
				}
			}
			return -1;
		}

		public bool HasCustomControllerMap(int mapId, int categoryId, int layoutId)
		{
			if (customControllerMaps == null)
			{
				return false;
			}
			for (int i = 0; i < customControllerMaps.Count; i++)
			{
				if (customControllerMaps[i].categoryId == categoryId && customControllerMaps[i].layoutId == layoutId && customControllerMaps[i].id == mapId)
				{
					return true;
				}
			}
			return false;
		}

		public bool HasCustomControllerMap(int mapId)
		{
			if (customControllerMaps == null)
			{
				return false;
			}
			for (int i = 0; i < customControllerMaps.Count; i++)
			{
				if (customControllerMaps[i].id == mapId)
				{
					return true;
				}
			}
			return false;
		}

		public bool HasCustomControllerMapInCategory(int controllerUid, int categoryId)
		{
			if (customControllerMaps == null)
			{
				return false;
			}
			for (int i = 0; i < customControllerMaps.Count; i++)
			{
				if (customControllerMaps[i].customControllerUid == controllerUid && customControllerMaps[i].categoryId == categoryId)
				{
					return true;
				}
			}
			return false;
		}

		public bool CreateCustomControllerMap(int categoryId, int controllerUid, int layoutId)
		{
			if (customControllerMaps == null)
			{
				customControllerMaps = new List<ControllerMap_Editor>();
			}
			ControllerMap_Editor controllerMap_Editor = new ControllerMap_Editor();
			controllerMap_Editor.id = GetNewCustomControllerMapId();
			controllerMap_Editor.categoryId = categoryId;
			controllerMap_Editor.layoutId = layoutId;
			controllerMap_Editor.hardwareGuidString = string.Empty;
			controllerMap_Editor.customControllerUid = controllerUid;
			customControllerMaps.Add(controllerMap_Editor);
			return false;
		}

		public void DeleteCustomControllerMap(int mapId)
		{
			if (customControllerMaps == null)
			{
				return;
			}
			for (int num = customControllerMaps.Count - 1; num >= 0; num--)
			{
				if (customControllerMaps[num].id == mapId)
				{
					customControllerMaps.RemoveAt(num);
				}
			}
		}

		public int DuplicateCustomControllerMap(int index)
		{
			if (customControllerMaps == null || index < 0 || index >= customControllerMaps.Count)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			ControllerMap_Editor controllerMap_Editor = customControllerMaps[index].Clone();
			controllerMap_Editor.id = GetNewCustomControllerMapId();
			customControllerMaps.Add(controllerMap_Editor);
			return customControllerMaps.Count - 1;
		}

		internal CustomControllerMap BrARhyONeKwqXgnVlAvKnHFNFZrI(Guid P_0, int P_1, int P_2)
		{
			return IXhJxWiHFTTrSMipzjoxCgBdxJHG(GetCustomControllerByHardwareTypeGuid(P_0), P_1, P_2);
		}

		internal CustomControllerMap MNKkjYicKUjWIvlytoqoHPNWWAfD(int P_0, int P_1, int P_2)
		{
			return IXhJxWiHFTTrSMipzjoxCgBdxJHG(GetCustomControllerById(P_1), P_0, P_2);
		}

		private CustomControllerMap IXhJxWiHFTTrSMipzjoxCgBdxJHG(CustomController_Editor P_0, int P_1, int P_2)
		{
			if (P_0 == null)
			{
				return null;
			}
			int id = P_0.id;
			ControllerMap_Editor controllerMap_Editor = dDxAivvEaHRwgnLmdHOVZofzHjJE(P_1, id, P_2, false);
			if (controllerMap_Editor != null)
			{
				CustomControllerMap customControllerMap = controllerMap_Editor.OaSueoFCwLpVBcZFfKDtRpgnzJQB(ContainsAction, P_0);
				customControllerMap.JtaxAKXFSTtJArgUOUsgzjVCftEiA(P_0.typeGuid, id, P_1, P_2);
				return customControllerMap;
			}
			CustomControllerMap customControllerMap2 = CustomControllerMap.WMPviajLMUiWvPCoyygwQMdkXrtE(P_0.typeGuid, id, P_1, P_2);
			customControllerMap2.JtaxAKXFSTtJArgUOUsgzjVCftEiA(P_0.typeGuid, id, P_1, P_2);
			return customControllerMap2;
		}

		private ControllerMap_Editor dDxAivvEaHRwgnLmdHOVZofzHjJE(int P_0, int P_1, int P_2, bool P_3)
		{
			ControllerMap_Editor customControllerMap = GetCustomControllerMap(P_0, P_1, P_2);
			if (customControllerMap != null)
			{
				return customControllerMap;
			}
			if (P_3)
			{
				customControllerMap = DrGbyhInHBGidAampWhYmCjspWHFb(P_0, P_1, P_2);
				if (customControllerMap != null)
				{
					return customControllerMap;
				}
			}
			return null;
		}

		private ControllerMap_Editor DrGbyhInHBGidAampWhYmCjspWHFb(int P_0, int P_1, int P_2)
		{
			List<ControllerMap_Editor> list = GetCustomControllerMaps(P_1);
			if (list != null && list.Count > 0)
			{
				yYwGeverSjtLCoPnzCUgHFreNWLyA(list, customControllerLayouts);
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].categoryId == P_0)
					{
						return list[i];
					}
				}
				for (int j = 0; j < list.Count; j++)
				{
					if (list[j].categoryId == 0)
					{
						return list[j];
					}
				}
			}
			return null;
		}

		public void DeleteControllerMap(ControllerType controllerType, int id)
		{
			switch (controllerType)
			{
			case ControllerType.Joystick:
				DeleteJoystickMap(id);
				break;
			case ControllerType.Keyboard:
				DeleteKeyboardMap(id);
				break;
			case ControllerType.Mouse:
				DeleteMouseMap(id);
				break;
			case ControllerType.Custom:
				DeleteCustomControllerMap(id);
				break;
			default:
				throw new NotImplementedException();
			}
		}

		public ControllerMap_Editor GetControllerMapByIndex(ControllerType controllerType, int index)
		{
			switch (controllerType)
			{
			case ControllerType.Joystick:
				if (joystickMaps == null)
				{
					return null;
				}
				return joystickMaps[index];
			case ControllerType.Keyboard:
				if (keyboardMaps == null)
				{
					return null;
				}
				return keyboardMaps[index];
			case ControllerType.Mouse:
				if (mouseMaps == null)
				{
					return null;
				}
				return mouseMaps[index];
			case ControllerType.Custom:
				if (customControllerMaps == null)
				{
					return null;
				}
				return customControllerMaps[index];
			default:
				throw new NotImplementedException();
			}
		}

		public ControllerMap_Editor GetControllerMapById(ControllerType controllerType, int id, out int controllerMapIndex)
		{
			return controllerType switch
			{
				ControllerType.Joystick => GetJoystickMapById(id, out controllerMapIndex), 
				ControllerType.Keyboard => GetKeyboardMapById(id, out controllerMapIndex), 
				ControllerType.Mouse => GetMouseMapById(id, out controllerMapIndex), 
				ControllerType.Custom => GetCustomControllerMapById(id, out controllerMapIndex), 
				_ => throw new NotImplementedException(), 
			};
		}

		public int DuplicateControllerMap(ControllerType controllerType, int index)
		{
			return controllerType switch
			{
				ControllerType.Joystick => DuplicateJoystickMap(index), 
				ControllerType.Keyboard => DuplicateKeyboardMap(index), 
				ControllerType.Mouse => DuplicateMouseMap(index), 
				ControllerType.Custom => DuplicateCustomControllerMap(index), 
				_ => throw new NotImplementedException(), 
			};
		}

		internal ControllerTemplateMap FDriMYNjLvRFqOeANQkVBNghaPBCA(Guid P_0, int P_1, int P_2)
		{
			return GetJoystickMap(P_1, P_0, P_2)?.BaKPvlVQGeyXHOiGlFHQLKDIOOzH();
		}

		[Obsolete("Does not validate type guid on creation to avoid clashes with other controllers. Use overload with typeGuid argument.", true)]
		public void AddCustomController()
		{
			AddCustomController(Guid.NewGuid());
		}

		public void AddCustomController(Guid typeGuid)
		{
			if (customControllers == null)
			{
				customControllers = new List<CustomController_Editor>();
			}
			customControllers.Add(mvUdomfWzUkpgbfJoLTmQhSjazzLA(typeGuid));
		}

		[Obsolete("Does not validate type guid on creation to avoid clashes with other controllers. Use overload with typeGuid argument.", true)]
		public void InsertCustomController(int index)
		{
			InsertCustomController(index, Guid.NewGuid());
		}

		public void InsertCustomController(int index, Guid typeGuid)
		{
			if (customControllers == null)
			{
				customControllers = new List<CustomController_Editor>();
			}
			if (index < 0 || index >= customControllers.Count)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			customControllers.Insert(index, mvUdomfWzUkpgbfJoLTmQhSjazzLA(typeGuid));
		}

		public void DeleteCustomController(int index)
		{
			if (customControllers == null || index < 0 || index >= customControllers.Count)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			int id = customControllers[index].id;
			if (customControllerMaps != null)
			{
				for (int num = customControllerMaps.Count - 1; num >= 0; num--)
				{
					if (customControllerMaps[num].customControllerUid == id)
					{
						customControllerMaps.RemoveAt(num);
					}
				}
			}
			customControllers.RemoveAt(index);
		}

		public bool ReorderCustomController(int index, bool offsetDown, bool offsetNow)
		{
			return ListTools.OffsetAtIndex(customControllers, index, offsetDown, offsetNow);
		}

		[Obsolete("Does not validate type guid on creation to avoid clashes with other controllers. Use overload with typeGuid argument.", true)]
		public void DuplicateCustomController(int index, bool duplicateMaps)
		{
			DuplicateCustomController(index, duplicateMaps, Guid.NewGuid());
		}

		public void DuplicateCustomController(int index, bool duplicateMaps, Guid typeGuid)
		{
			if (customControllers == null || index < 0 || index >= customControllers.Count)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			CustomController_Editor customController_Editor = customControllers[index].Clone();
			customController_Editor.id = GetNewCustomControllerId();
			customController_Editor.typeGuid = typeGuid;
			customController_Editor.name = StringTools.IterateName(customController_Editor.name, -1, GetCustomControllerNames());
			if (index == customControllers.Count - 1)
			{
				customControllers.Add(customController_Editor);
			}
			else
			{
				customControllers.Insert(index + 1, customController_Editor);
			}
			if (!duplicateMaps)
			{
				return;
			}
			int id = customController_Editor.id;
			int id2 = customControllers[index].id;
			if (customControllerMaps == null)
			{
				return;
			}
			for (int num = customControllerMaps.Count - 1; num >= 0; num--)
			{
				if (customControllerMaps[num].customControllerUid == id2)
				{
					int num2 = DuplicateCustomControllerMap(num);
					if (num2 >= 0)
					{
						customControllerMaps[num2].customControllerUid = id;
					}
				}
			}
		}

		public int GetCustomControllerMapCount(int controllerUid)
		{
			if (customControllers == null)
			{
				return 0;
			}
			int num = 0;
			if (customControllerMaps != null)
			{
				for (int i = 0; i < customControllerMaps.Count; i++)
				{
					if (customControllerMaps[i].customControllerUid == controllerUid)
					{
						num++;
					}
				}
			}
			return num;
		}

		public int GetCustomControllerIndex(int id)
		{
			if (customControllers == null)
			{
				return 0;
			}
			for (int i = 0; i < customControllers.Count; i++)
			{
				if (customControllers[i].id == id)
				{
					return i;
				}
			}
			return -1;
		}

		public string[] GetCustomControllerNames()
		{
			if (customControllers == null)
			{
				return null;
			}
			string[] array = new string[customControllers.Count];
			for (int i = 0; i < customControllers.Count; i++)
			{
				array[i] = customControllers[i].name;
			}
			return array;
		}

		public int[] GetCustomControllerIds()
		{
			if (customControllers == null)
			{
				return null;
			}
			int[] array = new int[customControllers.Count];
			for (int i = 0; i < customControllers.Count; i++)
			{
				array[i] = customControllers[i].id;
			}
			return array;
		}

		public Guid[] GetCustomControllerGuids()
		{
			if (customControllers == null)
			{
				return null;
			}
			Guid[] array = new Guid[customControllers.Count];
			for (int i = 0; i < customControllers.Count; i++)
			{
				array[i] = customControllers[i].typeGuid;
			}
			return array;
		}

		public CustomController_Editor GetCustomController(int index)
		{
			if (customControllers == null || index < 0 || index >= customControllers.Count)
			{
				return null;
			}
			return customControllers[index];
		}

		public CustomController_Editor GetCustomController(string name)
		{
			if (customControllers == null)
			{
				return null;
			}
			int num = IndexOfCustomController(name);
			if (num < 0)
			{
				return null;
			}
			return customControllers[num];
		}

		public CustomController_Editor GetCustomControllerById(int id)
		{
			if (customControllers == null)
			{
				return null;
			}
			int num = IndexOfCustomController(id);
			if (num < 0)
			{
				return null;
			}
			return customControllers[num];
		}

		public CustomController_Editor GetCustomControllerByHardwareTypeGuid(Guid hardwareTypeGuid)
		{
			if (customControllers == null)
			{
				return null;
			}
			int num = IndexOfCustomController(hardwareTypeGuid);
			if (num < 0)
			{
				return null;
			}
			return customControllers[num];
		}

		public int GetCustomControllerId(string name)
		{
			if (customControllers == null)
			{
				return -1;
			}
			int num = IndexOfCustomController(name);
			if (num < 0)
			{
				return -1;
			}
			return customControllers[num].id;
		}

		public int IndexOfCustomController(int id)
		{
			if (customControllers == null)
			{
				return -1;
			}
			for (int i = 0; i < customControllers.Count; i++)
			{
				if (customControllers[i].id == id)
				{
					return i;
				}
			}
			return -1;
		}

		public int IndexOfCustomController(string name)
		{
			if (name == null || name == string.Empty)
			{
				return -1;
			}
			if (customControllers == null)
			{
				return -1;
			}
			for (int i = 0; i < customControllers.Count; i++)
			{
				if (customControllers[i].name.Equals(name, StringComparison.OrdinalIgnoreCase))
				{
					return i;
				}
			}
			return -1;
		}

		public int IndexOfCustomController(Guid hardwareTypeGuid)
		{
			if (customControllers == null)
			{
				return -1;
			}
			for (int i = 0; i < customControllers.Count; i++)
			{
				if (customControllers[i].typeGuid == hardwareTypeGuid)
				{
					return i;
				}
			}
			return -1;
		}

		public string GetCustomControllerNameById(int id)
		{
			if (customControllers != null)
			{
				for (int i = 0; i < customControllers.Count; i++)
				{
					if (customControllers[i].id == id)
					{
						return customControllers[i].name;
					}
				}
			}
			return "Unknown";
		}

		public void AddControllerMapLayoutManagerRuleSet()
		{
			controllerMapLayoutManagerRuleSets.Add(ZiaadZTLfIoDGpQeVwUvAQkCbYUn());
		}

		public void InsertControllerMapLayoutManagerRuleSet(int index)
		{
			if (index < 0 || index >= controllerMapLayoutManagerRuleSets.Count)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			controllerMapLayoutManagerRuleSets.Insert(index, ZiaadZTLfIoDGpQeVwUvAQkCbYUn());
		}

		public void DeleteControllerMapLayoutManagerRuleSet(int index)
		{
			if (controllerMapLayoutManagerRuleSets == null || index < 0 || index >= controllerMapLayoutManagerRuleSets.Count)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			int id = controllerMapLayoutManagerRuleSets[index].id;
			if (players != null)
			{
				for (int i = 0; i < players.Count; i++)
				{
					Player_Editor player_Editor = players[i];
					if (player_Editor == null)
					{
						continue;
					}
					List<Player_Editor.RuleSetMapping> ruleSets = player_Editor.controllerMapLayoutManagerSettings.ruleSets;
					if (ruleSets == null)
					{
						continue;
					}
					for (int num = ruleSets.Count - 1; num >= 0; num--)
					{
						if (ruleSets[num] != null && ruleSets[num].id == id)
						{
							ruleSets.RemoveAt(num);
						}
					}
				}
			}
			controllerMapLayoutManagerRuleSets.RemoveAt(index);
		}

		public bool ReorderControllerMapLayoutManagerRuleSet(int index, bool offsetDown, bool offsetNow)
		{
			return ListTools.OffsetAtIndex(controllerMapLayoutManagerRuleSets, index, offsetDown, offsetNow);
		}

		public void DuplicateControllerMapLayoutManagerRuleSet(int index)
		{
			if (controllerMapLayoutManagerRuleSets == null || index < 0 || index >= controllerMapLayoutManagerRuleSets.Count)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			ControllerMapLayoutManager_RuleSet_Editor controllerMapLayoutManager_RuleSet_Editor = controllerMapLayoutManagerRuleSets[index].Clone();
			controllerMapLayoutManager_RuleSet_Editor.id = GetNewControllerMapLayoutManagerRuleSetId();
			controllerMapLayoutManager_RuleSet_Editor.name = StringTools.IterateName(controllerMapLayoutManager_RuleSet_Editor.name, -1, GetControllerMapLayoutManagerRuleSetNames());
			if (index == controllerMapLayoutManagerRuleSets.Count - 1)
			{
				controllerMapLayoutManagerRuleSets.Add(controllerMapLayoutManager_RuleSet_Editor);
			}
			else
			{
				controllerMapLayoutManagerRuleSets.Insert(index + 1, controllerMapLayoutManager_RuleSet_Editor);
			}
		}

		public int GetControllerMapLayoutManagerRuleSetUsedCount(int id)
		{
			if (controllerMapLayoutManagerRuleSets == null)
			{
				return 0;
			}
			int num = 0;
			if (players != null)
			{
				for (int i = 0; i < players.Count; i++)
				{
					Player_Editor player_Editor = players[i];
					if (player_Editor == null)
					{
						continue;
					}
					List<Player_Editor.RuleSetMapping> ruleSets = player_Editor.controllerMapLayoutManagerSettings.ruleSets;
					if (ruleSets == null)
					{
						continue;
					}
					for (int num2 = ruleSets.Count - 1; num2 >= 0; num2--)
					{
						if (ruleSets[num2] != null && ruleSets[num2].id == id)
						{
							num++;
						}
					}
				}
			}
			return num;
		}

		public int GetControllerMapLayoutManagerRuleSetIndex(int id)
		{
			if (controllerMapLayoutManagerRuleSets == null)
			{
				return 0;
			}
			for (int i = 0; i < controllerMapLayoutManagerRuleSets.Count; i++)
			{
				if (controllerMapLayoutManagerRuleSets[i].id == id)
				{
					return i;
				}
			}
			return -1;
		}

		public string[] GetControllerMapLayoutManagerRuleSetNames()
		{
			if (controllerMapLayoutManagerRuleSets == null)
			{
				return null;
			}
			string[] array = new string[controllerMapLayoutManagerRuleSets.Count];
			for (int i = 0; i < controllerMapLayoutManagerRuleSets.Count; i++)
			{
				array[i] = controllerMapLayoutManagerRuleSets[i].name;
			}
			return array;
		}

		public int[] GetControllerMapLayoutManagerRuleSetIds()
		{
			if (controllerMapLayoutManagerRuleSets == null)
			{
				return null;
			}
			int[] array = new int[controllerMapLayoutManagerRuleSets.Count];
			for (int i = 0; i < controllerMapLayoutManagerRuleSets.Count; i++)
			{
				array[i] = controllerMapLayoutManagerRuleSets[i].id;
			}
			return array;
		}

		public ControllerMapLayoutManager_RuleSet_Editor GetControllerMapLayoutManagerRuleSet(int index)
		{
			if (controllerMapLayoutManagerRuleSets == null || index < 0 || index >= controllerMapLayoutManagerRuleSets.Count)
			{
				return null;
			}
			return controllerMapLayoutManagerRuleSets[index];
		}

		public ControllerMapLayoutManager_RuleSet_Editor GetControllerMapLayoutManagerRuleSet(string name)
		{
			if (controllerMapLayoutManagerRuleSets == null)
			{
				return null;
			}
			int num = IndexOfControllerMapLayoutManagerRuleSet(name);
			if (num < 0)
			{
				return null;
			}
			return controllerMapLayoutManagerRuleSets[num];
		}

		public ControllerMapLayoutManager_RuleSet_Editor GetControllerMapLayoutManagerRuleSetById(int id)
		{
			if (controllerMapLayoutManagerRuleSets == null)
			{
				return null;
			}
			int num = IndexOfControllerMapLayoutManagerRuleSet(id);
			if (num < 0)
			{
				return null;
			}
			return controllerMapLayoutManagerRuleSets[num];
		}

		public int GetControllerMapLayoutManagerRuleSetId(string name)
		{
			if (controllerMapLayoutManagerRuleSets == null)
			{
				return -1;
			}
			int num = IndexOfControllerMapLayoutManagerRuleSet(name);
			if (num < 0)
			{
				return -1;
			}
			return controllerMapLayoutManagerRuleSets[num].id;
		}

		public int IndexOfControllerMapLayoutManagerRuleSet(int id)
		{
			if (controllerMapLayoutManagerRuleSets == null)
			{
				return -1;
			}
			for (int i = 0; i < controllerMapLayoutManagerRuleSets.Count; i++)
			{
				if (controllerMapLayoutManagerRuleSets[i].id == id)
				{
					return i;
				}
			}
			return -1;
		}

		public int IndexOfControllerMapLayoutManagerRuleSet(string name)
		{
			if (name == null || name == string.Empty)
			{
				return -1;
			}
			if (controllerMapLayoutManagerRuleSets == null)
			{
				return -1;
			}
			for (int i = 0; i < controllerMapLayoutManagerRuleSets.Count; i++)
			{
				if (controllerMapLayoutManagerRuleSets[i].name.Equals(name, StringComparison.OrdinalIgnoreCase))
				{
					return i;
				}
			}
			return -1;
		}

		public string GetControllerMapLayoutManagerRuleSetNameById(int id)
		{
			if (controllerMapLayoutManagerRuleSets != null)
			{
				for (int i = 0; i < controllerMapLayoutManagerRuleSets.Count; i++)
				{
					if (controllerMapLayoutManagerRuleSets[i].id == id)
					{
						return controllerMapLayoutManagerRuleSets[i].name;
					}
				}
			}
			return "Unknown";
		}

		public int GetControllerMapLayoutManagerRuleSetCount()
		{
			if (controllerMapLayoutManagerRuleSets == null)
			{
				return 0;
			}
			return controllerMapLayoutManagerRuleSets.Count;
		}

		public void AddControllerMapEnablerRuleSet()
		{
			controllerMapEnablerRuleSets.Add(ShSKNSWlgqZerAGGhzPhQLwSMeQC());
		}

		public void InsertControllerMapEnablerRuleSet(int index)
		{
			if (index < 0 || index >= controllerMapEnablerRuleSets.Count)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			controllerMapEnablerRuleSets.Insert(index, ShSKNSWlgqZerAGGhzPhQLwSMeQC());
		}

		public void DeleteControllerMapEnablerRuleSet(int index)
		{
			if (controllerMapEnablerRuleSets == null || index < 0 || index >= controllerMapEnablerRuleSets.Count)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			int id = controllerMapEnablerRuleSets[index].id;
			if (players != null)
			{
				for (int i = 0; i < players.Count; i++)
				{
					Player_Editor player_Editor = players[i];
					if (player_Editor == null)
					{
						continue;
					}
					List<Player_Editor.RuleSetMapping> ruleSets = player_Editor.controllerMapEnablerSettings.ruleSets;
					if (ruleSets == null)
					{
						continue;
					}
					for (int num = ruleSets.Count - 1; num >= 0; num--)
					{
						if (ruleSets[num] != null && ruleSets[num].id == id)
						{
							ruleSets.RemoveAt(num);
						}
					}
				}
			}
			controllerMapEnablerRuleSets.RemoveAt(index);
		}

		public bool ReorderControllerMapEnablerRuleSet(int index, bool offsetDown, bool offsetNow)
		{
			return ListTools.OffsetAtIndex(controllerMapEnablerRuleSets, index, offsetDown, offsetNow);
		}

		public void DuplicateControllerMapEnablerRuleSet(int index)
		{
			if (controllerMapEnablerRuleSets == null || index < 0 || index >= controllerMapEnablerRuleSets.Count)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			ControllerMapEnabler_RuleSet_Editor controllerMapEnabler_RuleSet_Editor = controllerMapEnablerRuleSets[index].Clone();
			controllerMapEnabler_RuleSet_Editor.id = GetNewControllerMapEnablerRuleSetId();
			controllerMapEnabler_RuleSet_Editor.name = StringTools.IterateName(controllerMapEnabler_RuleSet_Editor.name, -1, GetControllerMapEnablerRuleSetNames());
			if (index == controllerMapEnablerRuleSets.Count - 1)
			{
				controllerMapEnablerRuleSets.Add(controllerMapEnabler_RuleSet_Editor);
			}
			else
			{
				controllerMapEnablerRuleSets.Insert(index + 1, controllerMapEnabler_RuleSet_Editor);
			}
		}

		public int GetControllerMapEnablerRuleSetUsedCount(int id)
		{
			if (controllerMapEnablerRuleSets == null)
			{
				return 0;
			}
			int num = 0;
			if (players != null)
			{
				for (int i = 0; i < players.Count; i++)
				{
					Player_Editor player_Editor = players[i];
					if (player_Editor == null)
					{
						continue;
					}
					List<Player_Editor.RuleSetMapping> ruleSets = player_Editor.controllerMapEnablerSettings.ruleSets;
					if (ruleSets == null)
					{
						continue;
					}
					for (int num2 = ruleSets.Count - 1; num2 >= 0; num2--)
					{
						if (ruleSets[num2] != null && ruleSets[num2].id == id)
						{
							num++;
						}
					}
				}
			}
			return num;
		}

		public int GetControllerMapEnablerRuleSetIndex(int id)
		{
			if (controllerMapEnablerRuleSets == null)
			{
				return 0;
			}
			for (int i = 0; i < controllerMapEnablerRuleSets.Count; i++)
			{
				if (controllerMapEnablerRuleSets[i].id == id)
				{
					return i;
				}
			}
			return -1;
		}

		public string[] GetControllerMapEnablerRuleSetNames()
		{
			if (controllerMapEnablerRuleSets == null)
			{
				return null;
			}
			string[] array = new string[controllerMapEnablerRuleSets.Count];
			for (int i = 0; i < controllerMapEnablerRuleSets.Count; i++)
			{
				array[i] = controllerMapEnablerRuleSets[i].name;
			}
			return array;
		}

		public int[] GetControllerMapEnablerRuleSetIds()
		{
			if (controllerMapEnablerRuleSets == null)
			{
				return null;
			}
			int[] array = new int[controllerMapEnablerRuleSets.Count];
			for (int i = 0; i < controllerMapEnablerRuleSets.Count; i++)
			{
				array[i] = controllerMapEnablerRuleSets[i].id;
			}
			return array;
		}

		public ControllerMapEnabler_RuleSet_Editor GetControllerMapEnablerRuleSet(int index)
		{
			if (controllerMapEnablerRuleSets == null || index < 0 || index >= controllerMapEnablerRuleSets.Count)
			{
				return null;
			}
			return controllerMapEnablerRuleSets[index];
		}

		public ControllerMapEnabler_RuleSet_Editor GetControllerMapEnablerRuleSet(string name)
		{
			if (controllerMapEnablerRuleSets == null)
			{
				return null;
			}
			int num = IndexOfControllerMapEnablerRuleSet(name);
			if (num < 0)
			{
				return null;
			}
			return controllerMapEnablerRuleSets[num];
		}

		public ControllerMapEnabler_RuleSet_Editor GetControllerMapEnablerRuleSetById(int id)
		{
			if (controllerMapEnablerRuleSets == null)
			{
				return null;
			}
			int num = IndexOfControllerMapEnablerRuleSet(id);
			if (num < 0)
			{
				return null;
			}
			return controllerMapEnablerRuleSets[num];
		}

		public int GetControllerMapEnablerRuleSetId(string name)
		{
			if (controllerMapEnablerRuleSets == null)
			{
				return -1;
			}
			int num = IndexOfControllerMapEnablerRuleSet(name);
			if (num < 0)
			{
				return -1;
			}
			return controllerMapEnablerRuleSets[num].id;
		}

		public int IndexOfControllerMapEnablerRuleSet(int id)
		{
			if (controllerMapEnablerRuleSets == null)
			{
				return -1;
			}
			for (int i = 0; i < controllerMapEnablerRuleSets.Count; i++)
			{
				if (controllerMapEnablerRuleSets[i].id == id)
				{
					return i;
				}
			}
			return -1;
		}

		public int IndexOfControllerMapEnablerRuleSet(string name)
		{
			if (name == null || name == string.Empty)
			{
				return -1;
			}
			if (controllerMapEnablerRuleSets == null)
			{
				return -1;
			}
			for (int i = 0; i < controllerMapEnablerRuleSets.Count; i++)
			{
				if (controllerMapEnablerRuleSets[i].name.Equals(name, StringComparison.OrdinalIgnoreCase))
				{
					return i;
				}
			}
			return -1;
		}

		public string GetControllerMapEnablerRuleSetNameById(int id)
		{
			if (controllerMapEnablerRuleSets != null)
			{
				for (int i = 0; i < controllerMapEnablerRuleSets.Count; i++)
				{
					if (controllerMapEnablerRuleSets[i].id == id)
					{
						return controllerMapEnablerRuleSets[i].name;
					}
				}
			}
			return "Unknown";
		}

		public int GetControllerMapEnablerRuleSetCount()
		{
			if (controllerMapEnablerRuleSets == null)
			{
				return 0;
			}
			return controllerMapEnablerRuleSets.Count;
		}

		public int GetNewPlayerId()
		{
			int result = playerIdCounter;
			playerIdCounter++;
			return result;
		}

		public int GetNewActionId()
		{
			int result = actionIdCounter;
			actionIdCounter++;
			return result;
		}

		public int GetNewActionCategoryId()
		{
			int result = actionCategoryIdCounter;
			actionCategoryIdCounter++;
			return result;
		}

		public int GetNewInputBehaviorId()
		{
			int result = inputBehaviorIdCounter;
			inputBehaviorIdCounter++;
			return result;
		}

		public int GetNewMapCategoryId()
		{
			int result = mapCategoryIdCounter;
			mapCategoryIdCounter++;
			return result;
		}

		public int GetNewJoystickLayoutId()
		{
			int result = joystickLayoutIdCounter;
			joystickLayoutIdCounter++;
			return result;
		}

		public int GetNewKeyboardLayoutId()
		{
			int result = keyboardLayoutIdCounter;
			keyboardLayoutIdCounter++;
			return result;
		}

		public int GetNewMouseLayoutId()
		{
			int result = mouseLayoutIdCounter;
			mouseLayoutIdCounter++;
			return result;
		}

		public int GetNewCustomControllerLayoutId()
		{
			int result = customControllerLayoutIdCounter;
			customControllerLayoutIdCounter++;
			return result;
		}

		public int GetNewJoystickMapId()
		{
			int result = joystickMapIdCounter;
			joystickMapIdCounter++;
			return result;
		}

		public int GetNewKeyboardMapId()
		{
			int result = keyboardMapIdCounter;
			keyboardMapIdCounter++;
			return result;
		}

		public int GetNewMouseMapId()
		{
			int result = mouseMapIdCounter;
			mouseMapIdCounter++;
			return result;
		}

		public int GetNewCustomControllerMapId()
		{
			int result = customControllerMapIdCounter;
			customControllerMapIdCounter++;
			return result;
		}

		public int GetNewCustomControllerId()
		{
			int result = customControllerIdCounter;
			customControllerIdCounter++;
			return result;
		}

		public int GetNewControllerMapLayoutManagerRuleSetId()
		{
			int result = controllerMapLayoutManagerSetIdCounter;
			controllerMapLayoutManagerSetIdCounter++;
			return result;
		}

		public int GetNewControllerMapEnablerRuleSetId()
		{
			int result = controllerMapEnablerSetIdCounter;
			controllerMapEnablerSetIdCounter++;
			return result;
		}

		private Player_Editor oTvzqdfQFNEeFreRthKtbncVjSOD()
		{
			Player_Editor player_Editor = new Player_Editor();
			player_Editor.id = GetNewPlayerId();
			player_Editor.name = StringTools.IterateName("Player", -1, GetPlayerNames());
			player_Editor.descriptiveName = player_Editor.name;
			player_Editor.startPlaying = true;
			if (players.Count == 1)
			{
				player_Editor.assignMouseOnStart = true;
			}
			player_Editor.assignKeyboardOnStart = true;
			player_Editor.controllerMapEnablerSettings = new Player_Editor.ControllerMapEnablerSettings();
			player_Editor.controllerMapLayoutManagerSettings = new Player_Editor.ControllerMapLayoutManagerSettings();
			return player_Editor;
		}

		private InputAction XRCiMKzJhzzUaQkaqNIJRNJHIqew()
		{
			InputAction obj = new InputAction
			{
				id = GetNewActionId(),
				name = StringTools.IterateName("Action", -1, GetActionNames())
			};
			obj.descriptiveName = obj.name;
			obj.type = InputActionType.Button;
			obj.userAssignable = true;
			obj.behaviorId = 0;
			return obj;
		}

		private InputActionCategory MukaBDbnShpYGcdoHCwmholBbkTxb()
		{
			InputActionCategory obj = new InputActionCategory
			{
				id = GetNewActionCategoryId(),
				name = StringTools.IterateName("Category", -1, GetActionCategoryNames())
			};
			obj.descriptiveName = obj.name;
			obj.userAssignable = true;
			return obj;
		}

		private InputBehavior xizFbYUAUudvlSimSrOkErzkqsrL()
		{
			return new InputBehavior
			{
				id = GetNewInputBehaviorId(),
				name = StringTools.IterateName("Behavior", -1, GetInputBehaviorNames()),
				digitalAxisSimulation = false,
				digitalAxisSnap = true,
				digitalAxisInstantReverse = false,
				digitalAxisGravity = 3f,
				digitalAxisSensitivity = 3f,
				mouseXYAxisMode = MouseXYAxisMode.MouseAxis,
				mouseXYAxisSensitivity = 1f,
				mouseOtherAxisMode = MouseOtherAxisMode.MouseAxis,
				mouseOtherAxisSensitivity = 1f,
				buttonDoublePressSpeed = 0.3f,
				buttonShortPressTime = 0.25f,
				buttonShortPressExpiresIn = 0f,
				buttonLongPressTime = 1f,
				buttonLongPressExpiresIn = 0f,
				buttonDeadZone = 0.5f,
				buttonDownBuffer = 0f
			};
		}

		private InputMapCategory yxpIsNdrTAPqutLkpxjnUcKylTqX()
		{
			InputMapCategory obj = new InputMapCategory
			{
				id = GetNewMapCategoryId(),
				name = StringTools.IterateName("Category", -1, GetMapCategoryNames())
			};
			obj.descriptiveName = obj.name;
			obj.userAssignable = true;
			obj.checkConflictsWithAllCategories = true;
			return obj;
		}

		private InputLayout OrCWHNIwZwPcPxObVyVRanWJTfPC()
		{
			InputLayout obj = new InputLayout
			{
				id = GetNewJoystickLayoutId(),
				name = StringTools.IterateName("Layout", -1, GetJoystickLayoutNames())
			};
			obj.descriptiveName = obj.name;
			return obj;
		}

		private InputLayout TDYbIwUqxjjOdugcAbRLcEuOgzafb()
		{
			InputLayout obj = new InputLayout
			{
				id = GetNewKeyboardLayoutId(),
				name = StringTools.IterateName("Layout", -1, GetKeyboardLayoutNames())
			};
			obj.descriptiveName = obj.name;
			return obj;
		}

		private InputLayout rpSNaZZDZzcZlrEJdUSlCUOVkdnW()
		{
			InputLayout obj = new InputLayout
			{
				id = GetNewMouseLayoutId(),
				name = StringTools.IterateName("Layout", -1, GetMouseLayoutNames())
			};
			obj.descriptiveName = obj.name;
			return obj;
		}

		private InputLayout lLRFyHcCwYgxGHVxxbgwMDdmOUfA()
		{
			InputLayout obj = new InputLayout
			{
				id = GetNewCustomControllerLayoutId(),
				name = StringTools.IterateName("Layout", -1, GetCustomControllerLayoutNames())
			};
			obj.descriptiveName = obj.name;
			return obj;
		}

		private CustomController_Editor mvUdomfWzUkpgbfJoLTmQhSjazzLA(Guid P_0)
		{
			CustomController_Editor obj = new CustomController_Editor
			{
				id = GetNewCustomControllerId(),
				typeGuid = P_0,
				name = StringTools.IterateName("CustomController", -1, GetCustomControllerNames())
			};
			obj.descriptiveName = obj.name;
			return obj;
		}

		private ControllerMapLayoutManager_RuleSet_Editor ZiaadZTLfIoDGpQeVwUvAQkCbYUn()
		{
			return new ControllerMapLayoutManager_RuleSet_Editor
			{
				id = GetNewControllerMapLayoutManagerRuleSetId(),
				name = StringTools.IterateName("RuleSet", -1, GetControllerMapLayoutManagerRuleSetNames())
			};
		}

		private ControllerMapEnabler_RuleSet_Editor ShSKNSWlgqZerAGGhzPhQLwSMeQC()
		{
			return new ControllerMapEnabler_RuleSet_Editor
			{
				id = GetNewControllerMapEnablerRuleSetId(),
				name = StringTools.IterateName("RuleSet", -1, GetControllerMapEnablerRuleSetNames())
			};
		}

		private ControllerMap_Editor azeriRDlvBOgyLnDFIQanytVWxjc(List<ControllerMap_Editor> P_0, int P_1, int P_2)
		{
			if (P_0 == null)
			{
				return null;
			}
			for (int i = 0; i < P_0.Count; i++)
			{
				if (P_0[i].categoryId == P_1 && P_0[i].layoutId == P_2)
				{
					return P_0[i];
				}
			}
			return null;
		}

		private ControllerMap_Editor llWoMUcwuQDldmqlmoFJkVqRCYAd(List<ControllerMap_Editor> P_0, List<InputLayout> P_1, int P_2, int P_3, bool P_4)
		{
			ControllerMap_Editor controllerMap_Editor = azeriRDlvBOgyLnDFIQanytVWxjc(P_0, P_2, P_3);
			if (controllerMap_Editor != null)
			{
				return controllerMap_Editor;
			}
			if (P_4)
			{
				controllerMap_Editor = WYwBBVDdAMmaGDSdkkZjFSUWCugt(P_0, P_1, P_2, P_3);
				if (controllerMap_Editor != null)
				{
					return controllerMap_Editor;
				}
			}
			return null;
		}

		private ControllerMap_Editor WYwBBVDdAMmaGDSdkkZjFSUWCugt(List<ControllerMap_Editor> P_0, List<InputLayout> P_1, int P_2, int P_3)
		{
			List<ControllerMap_Editor> list = ListTools.ShallowCopy(P_0);
			if (list != null && list.Count > 0)
			{
				yYwGeverSjtLCoPnzCUgHFreNWLyA(list, P_1);
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].categoryId == P_2)
					{
						return list[i];
					}
				}
				for (int j = 0; j < list.Count; j++)
				{
					if (list[j].categoryId == 0)
					{
						return list[j];
					}
				}
			}
			return null;
		}

		private void yYwGeverSjtLCoPnzCUgHFreNWLyA(List<ControllerMap_Editor> P_0, List<InputLayout> P_1)
		{
			aIeBdCkZGlYpejfWiKBvqyXdyjGw aIeBdCkZGlYpejfWiKBvqyXdyjGw2 = new aIeBdCkZGlYpejfWiKBvqyXdyjGw();
			aIeBdCkZGlYpejfWiKBvqyXdyjGw2.rZnomdfbWuvhWJNQQVhCgtRYtzub = P_1;
			if (P_0 != null && aIeBdCkZGlYpejfWiKBvqyXdyjGw2.rZnomdfbWuvhWJNQQVhCgtRYtzub != null)
			{
				P_0.Sort(aIeBdCkZGlYpejfWiKBvqyXdyjGw2.RERiQDiULkPbzZXWSdyGbjLiKxfkB);
			}
		}

		internal void sYenrtjyzFlZbHGsMCbWQdyazWCe()
		{
			if (wFOAzFxiXEXdSCuxaPrTUvxCQQzw)
			{
				return;
			}
			gcsncNCaRRPuhDcaaLiikvmhVRaG = new List<InputAction>(actions.Count);
			for (int i = 0; i < actions.Count; i++)
			{
				if (actions[i] == null)
				{
					gcsncNCaRRPuhDcaaLiikvmhVRaG.Add(null);
				}
				gcsncNCaRRPuhDcaaLiikvmhVRaG.Add(new InputAction(actions[i]));
			}
			JIZWSkNLBmbxmgqTylFTFDyIHEkLB = new ReadOnlyCollection<Player_Editor>(players);
			UaeAwHiobbzFazhOCwHXZiSScIWDA = new ReadOnlyCollection<InputAction>(gcsncNCaRRPuhDcaaLiikvmhVRaG);
			List<InputCategory> list = new List<InputCategory>((actionCategories != null) ? actionCategories.Count : 0);
			for (int j = 0; j < actionCategories.Count; j++)
			{
				list.Add(actionCategories[j]);
			}
			IMddRaBENAmGfZWTNpvTlDLkNkHU = new ReadOnlyCollection<InputCategory>(list);
			JMDCCNNbMZQKDbeMVYRPAhfYnALf = new ReadOnlyCollection<InputBehavior>(inputBehaviors);
			psnMotVTaWJqzAFWxfjoaAVNVSFMA = new ReadOnlyCollection<InputMapCategory>(mapCategories);
			jRJbARDRHMLcZQovULcpACLAoSlYA = new ReadOnlyCollection<InputLayout>(joystickLayouts);
			DTuaINDJtEOtldDSZdZpfyTkmIIcA = new ReadOnlyCollection<InputLayout>(keyboardLayouts);
			CYgKlfpQSrdTwHyRItikkHzdxbipA = new ReadOnlyCollection<InputLayout>(mouseLayouts);
			XzpJaGfpJYXkiIKQPTSLavfAQgiO = new ReadOnlyCollection<InputLayout>(customControllerLayouts);
			CSGjkgmFTTujzxKbxdyMdsSJqKezA = new ReadOnlyCollection<ControllerMap_Editor>(joystickMaps);
			iLLWPQayDOVNpTuxTQEPyqnacxkX = new ReadOnlyCollection<ControllerMap_Editor>(keyboardMaps);
			ZGjsDKrfpxjZadgHDlmkGNtfcqerA = new ReadOnlyCollection<ControllerMap_Editor>(mouseMaps);
			ZqhatfesYQmKBXVxJOmdkNywSXdj = new ReadOnlyCollection<ControllerMap_Editor>(customControllerMaps);
			hORnDYsESUNPDVMXKkCRoOvcfCbKA = new ReadOnlyCollection<ControllerMapLayoutManager_RuleSet_Editor>(controllerMapLayoutManagerRuleSets);
			UoIdkhzSDqqeiTjSUanOgCAIGtol = new ReadOnlyCollection<ControllerMapEnabler_RuleSet_Editor>(controllerMapEnablerRuleSets);
			if (mapCategories != null)
			{
				for (int k = 0; k < mapCategories.Count; k++)
				{
					if (mapCategories[k] != null)
					{
						mapCategories[k].OyVwADHPtCrAvUkjrpdvrzNAWspf();
					}
				}
			}
			if (actionCategories != null)
			{
				for (int l = 0; l < actionCategories.Count; l++)
				{
					if (actionCategories[l] != null)
					{
						actionCategories[l].OyVwADHPtCrAvUkjrpdvrzNAWspf();
					}
				}
			}
			if (joystickLayouts != null)
			{
				for (int m = 0; m < joystickLayouts.Count; m++)
				{
					if (joystickLayouts[m] != null)
					{
						joystickLayouts[m].oqfPrkRCqhrtUheegalROxJRyzgr();
					}
				}
			}
			if (keyboardLayouts != null)
			{
				for (int n = 0; n < keyboardLayouts.Count; n++)
				{
					if (keyboardLayouts[n] != null)
					{
						keyboardLayouts[n].oqfPrkRCqhrtUheegalROxJRyzgr();
					}
				}
			}
			if (mouseLayouts != null)
			{
				for (int num = 0; num < mouseLayouts.Count; num++)
				{
					if (mouseLayouts[num] != null)
					{
						mouseLayouts[num].oqfPrkRCqhrtUheegalROxJRyzgr();
					}
				}
			}
			if (customControllerLayouts != null)
			{
				for (int num2 = 0; num2 < customControllerLayouts.Count; num2++)
				{
					if (customControllerLayouts[num2] != null)
					{
						customControllerLayouts[num2].oqfPrkRCqhrtUheegalROxJRyzgr();
					}
				}
			}
			if (gcsncNCaRRPuhDcaaLiikvmhVRaG != null)
			{
				for (int num3 = 0; num3 < gcsncNCaRRPuhDcaaLiikvmhVRaG.Count; num3++)
				{
					if (gcsncNCaRRPuhDcaaLiikvmhVRaG[num3] != null)
					{
						gcsncNCaRRPuhDcaaLiikvmhVRaG[num3].UyNKzGYQQbcOHOZGyQybTlKHSqIA();
					}
				}
			}
			containsActionDelegate = ContainsAction;
			wFOAzFxiXEXdSCuxaPrTUvxCQQzw = true;
		}

		internal void DGtuTHACBgsoyBTMvnQjtQIYnBlt()
		{
			if (!wFOAzFxiXEXdSCuxaPrTUvxCQQzw)
			{
				return;
			}
			if (mapCategories != null)
			{
				for (int i = 0; i < mapCategories.Count; i++)
				{
					if (mapCategories[i] != null)
					{
						mapCategories[i].mkcTIuyVhyjgcOyTVIRjtEYxhOAJ();
					}
				}
			}
			if (gcsncNCaRRPuhDcaaLiikvmhVRaG != null)
			{
				for (int j = 0; j < gcsncNCaRRPuhDcaaLiikvmhVRaG.Count; j++)
				{
					if (gcsncNCaRRPuhDcaaLiikvmhVRaG[j] != null)
					{
						gcsncNCaRRPuhDcaaLiikvmhVRaG[j].xSQboEuJLudqDiLbJTKEFhUJvsxmA();
					}
				}
			}
			wFOAzFxiXEXdSCuxaPrTUvxCQQzw = false;
		}

		[CustomObfuscation(rename = false)]
		internal static UserData Merge(UserData orig, UserData other, bool preserveOrigIds)
		{
			return VLVBKeKIsfcpczgBLQCLgmwKfWlb.LpThxjFUnKCEcAfYJbELHJdqpLnQA(orig, other, preserveOrigIds);
		}

		[CustomObfuscation(rename = false)]
		internal static UserData Compact(UserData orig)
		{
			return VLVBKeKIsfcpczgBLQCLgmwKfWlb.LpThxjFUnKCEcAfYJbELHJdqpLnQA(orig, null, false);
		}
	}
}
