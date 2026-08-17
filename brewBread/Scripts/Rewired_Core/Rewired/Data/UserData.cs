using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading;
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
		private static class zErewWsElUgBaKXLzTOENJhkptHEb
		{
			[DefaultMember("Item")]
			private class nujuvJulgYzNRYueyruWPgnaqlHP
			{
				public enum KfuHwiomPteqWnzNNKhAzBQQnmQh
				{
					origId = 0,
					otherId = 1,
					finalId = 2
				}

				public int iUXfqaKfPKLkHsUDkpzSgqaBxarZB;

				public int okeWwwmvgkmEMOtIulLmImzRWNpc;

				public int NmvBXsDtIAXqBCeQpumPWRwoEYyoA;

				public int xKHWVyyMFvFkZzVdcahSgmCfFoFGb
				{
					get
					{
						return P_0 switch
						{
							KfuHwiomPteqWnzNNKhAzBQQnmQh.origId => iUXfqaKfPKLkHsUDkpzSgqaBxarZB, 
							KfuHwiomPteqWnzNNKhAzBQQnmQh.otherId => okeWwwmvgkmEMOtIulLmImzRWNpc, 
							KfuHwiomPteqWnzNNKhAzBQQnmQh.finalId => NmvBXsDtIAXqBCeQpumPWRwoEYyoA, 
							_ => throw new NotImplementedException(), 
						};
					}
					set
					{
						switch (kfuHwiomPteqWnzNNKhAzBQQnmQh)
						{
						case KfuHwiomPteqWnzNNKhAzBQQnmQh.origId:
							iUXfqaKfPKLkHsUDkpzSgqaBxarZB = nmvBXsDtIAXqBCeQpumPWRwoEYyoA;
							break;
						case KfuHwiomPteqWnzNNKhAzBQQnmQh.otherId:
							okeWwwmvgkmEMOtIulLmImzRWNpc = nmvBXsDtIAXqBCeQpumPWRwoEYyoA;
							break;
						case KfuHwiomPteqWnzNNKhAzBQQnmQh.finalId:
							NmvBXsDtIAXqBCeQpumPWRwoEYyoA = nmvBXsDtIAXqBCeQpumPWRwoEYyoA;
							break;
						default:
							throw new NotImplementedException();
						}
					}
				}

				public nujuvJulgYzNRYueyruWPgnaqlHP(int P_0, int P_1, int P_2)
				{
					iUXfqaKfPKLkHsUDkpzSgqaBxarZB = P_0;
					okeWwwmvgkmEMOtIulLmImzRWNpc = P_1;
					NmvBXsDtIAXqBCeQpumPWRwoEYyoA = P_2;
				}

				public virtual string RsAkLkYigfQxKWvdxVBUwNqsOGbJ()
				{
					return string.Concat(string.Concat("" + StringTools.WriteVar("origId", iUXfqaKfPKLkHsUDkpzSgqaBxarZB), StringTools.WriteVar("otherId", okeWwwmvgkmEMOtIulLmImzRWNpc)), StringTools.WriteVar("finalId", NmvBXsDtIAXqBCeQpumPWRwoEYyoA));
				}
			}

			private class koiccHreeSzhhtmqpAfbekazGvOAb<_0001>
			{
				public _0001 UNvoUrngiTAgvWKCtPziXkqUPtZk;

				public _0001 ZUyOTflBkjftSKlGjdzKIQDMvsbu;

				public nujuvJulgYzNRYueyruWPgnaqlHP.KfuHwiomPteqWnzNNKhAzBQQnmQh GpmLUZnatheysQmKSGyhdsjsLXusA;

				public IList<_0001> zRnCKWcMztlPqYjfLLdxSVnTlRAdA;

				public bool idYOzFFtwFXzmBEeMDPCXTFwffBf;

				public koiccHreeSzhhtmqpAfbekazGvOAb(_0001 P_0, _0001 P_1, nujuvJulgYzNRYueyruWPgnaqlHP.KfuHwiomPteqWnzNNKhAzBQQnmQh P_2, IList<_0001> P_3, bool P_4)
				{
					UNvoUrngiTAgvWKCtPziXkqUPtZk = P_0;
					ZUyOTflBkjftSKlGjdzKIQDMvsbu = P_1;
					GpmLUZnatheysQmKSGyhdsjsLXusA = P_2;
					zRnCKWcMztlPqYjfLLdxSVnTlRAdA = P_3;
					idYOzFFtwFXzmBEeMDPCXTFwffBf = P_4;
				}
			}

			[Serializable]
			private sealed class rEvABygUkOfYEkBliaAlBIpFisUVB
			{
				public static readonly rEvABygUkOfYEkBliaAlBIpFisUVB _003C_003E9 = new rEvABygUkOfYEkBliaAlBIpFisUVB();

				public static Func<InputCategory, int> _003C_003E9__0_0;

				public static Func<InputCategory, string> _003C_003E9__0_1;

				public static Func<InputCategory, IList<InputCategory>, int> _003C_003E9__0_2;

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

				internal int sZmCxsfoLRUUNmmuynpMwZzTEMkeA(InputCategory P_0)
				{
					return P_0.id;
				}

				internal string byBgfkCOaQjxOtcewYpDcHyVgTCf(InputCategory P_0)
				{
					return P_0.name;
				}

				internal int CBczItUspJdkLcIrfivyrIEonIiy(InputCategory P_0, IList<InputCategory> P_1)
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

				internal int mGSrushEdOESLGvmmSStQZEXETAt(InputBehavior P_0)
				{
					return P_0.id;
				}

				internal string QowlghBXddeWtabdmdUpcdZXDtOOA(InputBehavior P_0)
				{
					return P_0.name;
				}

				internal int WTkhXHPBthKULenHXhoUkFGLHyiQA(InputBehavior P_0, IList<InputBehavior> P_1)
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

				internal int IpcuPsEKkYiyENqyBSMeczKNUjPt(InputAction P_0)
				{
					return P_0.id;
				}

				internal string dywmMPpcdtbbhshgDLohDTHfgHnI(InputAction P_0)
				{
					return P_0.name;
				}

				internal int WwjOCjHHcNvVqIgoUfuTEBGpeDIlA(InputAction P_0, IList<InputAction> P_1)
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

				internal int XXxGJFjsOVTJWnYyttxDiCmIHWLKA(InputMapCategory P_0)
				{
					return P_0.id;
				}

				internal string vIndSvjTDbjRibsSEQxMEUswmmZvb(InputMapCategory P_0)
				{
					return P_0.name;
				}

				internal int PwbAKpAStDlnUBqtepidlUDgzIeDc(InputMapCategory P_0, IList<InputMapCategory> P_1)
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

				internal int hsDXQgCYjPgMpysShAYFabahHQodA(InputLayout P_0)
				{
					return P_0.id;
				}

				internal string ZVSyQnjGclSSONFMpKmQuoALgeNN(InputLayout P_0)
				{
					return P_0.name;
				}

				internal int rKmCayrwCjMwflBmmRQIPJeVaSgu(InputLayout P_0, IList<InputLayout> P_1)
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

				internal int YhTaJwcKNTNcMKBumSVnBSSiMTzeb(InputLayout P_0)
				{
					return P_0.id;
				}

				internal string LbDLmPOnlvwajPhrmKgYMuQGsQax(InputLayout P_0)
				{
					return P_0.name;
				}

				internal int rffynhqizVSUpLVpUvSJUPqVuTZw(InputLayout P_0, IList<InputLayout> P_1)
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

				internal int TEftybiGKVFUVivVbHsxSvRzHVNn(InputLayout P_0)
				{
					return P_0.id;
				}

				internal string voQjXuPXhbJiRbnGmTgDrlhNuxXu(InputLayout P_0)
				{
					return P_0.name;
				}

				internal int lSnARcgYwpAtmlhfMszLsaOWWeWmA(InputLayout P_0, IList<InputLayout> P_1)
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

				internal int wZPYRybJbxGXkCykUdaPccPEpuVVA(InputLayout P_0)
				{
					return P_0.id;
				}

				internal string TetQdBTMVdmULyPltIGfZdqIASie(InputLayout P_0)
				{
					return P_0.name;
				}

				internal int AlGcxnqrdjDIUNTQuArcmQovPFDs(InputLayout P_0, IList<InputLayout> P_1)
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

				internal int AEuuRnbWxrmnKjXVDaPXeRzrOvLy(CustomController_Editor P_0)
				{
					return P_0.id;
				}

				internal string TDdUXlTHoXoGhYQWtloSTZPNgxXU(CustomController_Editor P_0)
				{
					return P_0.name;
				}

				internal int wjFwmKMByIqWuFzUMfWrIBMNAmfjA(CustomController_Editor P_0, IList<CustomController_Editor> P_1)
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

				internal int lKUzkqFVlonREFPNpCyPvyeZCtzV(ControllerMapLayoutManager_RuleSet_Editor P_0)
				{
					return P_0.id;
				}

				internal string rnWCEHhDVNstHQFGwmDTeKbgohQv(ControllerMapLayoutManager_RuleSet_Editor P_0)
				{
					return P_0.name;
				}

				internal int mpuWwRwaTVoKRwIDnucMyMZQqBtq(ControllerMapLayoutManager_RuleSet_Editor P_0, IList<ControllerMapLayoutManager_RuleSet_Editor> P_1)
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

				internal int KTCyFRTLNmNbYaehDSqHShkVIkvjA(ControllerMapEnabler_RuleSet_Editor P_0)
				{
					return P_0.id;
				}

				internal string nWhIKZCYeGjjbQcXwLlenlgljtiDA(ControllerMapEnabler_RuleSet_Editor P_0)
				{
					return P_0.name;
				}

				internal int UrdluDvpPMNumnjmJGRWaYjQNsBfA(ControllerMapEnabler_RuleSet_Editor P_0, IList<ControllerMapEnabler_RuleSet_Editor> P_1)
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

				internal int FvMGJZMGeQEDnposlgsnbWMuukKIA(Player_Editor P_0)
				{
					return P_0.id;
				}

				internal string WwSxtsFnHvVREFdtDsQfZOoPeqCCA(Player_Editor P_0)
				{
					return P_0.name;
				}

				internal int WVeKNwgFDxBcMENluBxJHYAhxpTHA(Player_Editor P_0, IList<Player_Editor> P_1)
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

				internal int OkmtevslrcOlWkCYKIDXjqOhnVfC(Player_Editor.Mapping P_0, IList<Player_Editor.Mapping> P_1)
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

				internal int uOeWQqIBQRpKudUwTbbLhSMziPaL(Player_Editor.CreateControllerInfo P_0, IList<Player_Editor.CreateControllerInfo> P_1)
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

				internal int xAfpMXPzsbRwpgTKnWNfDxeXtVDi(ControllerMap_Editor P_0)
				{
					return P_0.id;
				}

				internal string WjwlGjfLIDQvbpGGTrehxhDOVHRw(ControllerMap_Editor P_0)
				{
					return P_0.name;
				}

				internal int WKNpLOtfkHlvMFaHuIvRkCgnGBBx(ActionElementMap P_0, IList<ActionElementMap> P_1)
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

				internal int wNkthCnaqKXNYSKuKsJLjKaPmjFG(ControllerMap_Editor P_0)
				{
					return P_0.id;
				}

				internal string hadDyITekfZvjAkgQUspAHMsrXsv(ControllerMap_Editor P_0)
				{
					return P_0.name;
				}

				internal int pRzCzRDBBLLCMXJAnfRjAXDpItsbb(ActionElementMap P_0, IList<ActionElementMap> P_1)
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

				internal int shaAuobKyFujtuXwmawhvpEOiNzaA(ControllerMap_Editor P_0)
				{
					return P_0.id;
				}

				internal string khORnAgmfIjzSfauCGzMcChplAzlA(ControllerMap_Editor P_0)
				{
					return P_0.name;
				}

				internal int QDLvRBRtFKukZbgExEgEiKGIBraFA(ActionElementMap P_0, IList<ActionElementMap> P_1)
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

				internal int rLTGpKtAFwbQzjsdCpPSFNvcLZIBA(ControllerMap_Editor P_0)
				{
					return P_0.id;
				}

				internal string ITcelZhXFwpYEitzEbKFOfXUzuuH(ControllerMap_Editor P_0)
				{
					return P_0.name;
				}

				internal int pkuKURmfsKYFRWFHrjFcxDVnbGChA(ActionElementMap P_0, IList<ActionElementMap> P_1)
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

			private sealed class IXmcEKmCGRPEAZqGNAGkHqLXgNgA
			{
				public UserData HDkgCuGOBNmSZoCUejZdOdigNGDNA;

				public List<nujuvJulgYzNRYueyruWPgnaqlHP> KBLwofECeiBpLcJHNBOtMFGqCohXA;

				public List<nujuvJulgYzNRYueyruWPgnaqlHP> QRoDQvTXtDUJBScICbROkUpBwmHqA;

				public List<nujuvJulgYzNRYueyruWPgnaqlHP> LxnWPQAbzstdNmqElICPYQIjQnmw;

				public List<nujuvJulgYzNRYueyruWPgnaqlHP> BGClHotKwyDHRcCAZxUtRuojYxAeA;

				public List<nujuvJulgYzNRYueyruWPgnaqlHP> lhhvePGLXyqhBobYgHQMxKdAbBkP;

				public List<nujuvJulgYzNRYueyruWPgnaqlHP> gymYSGRESagpXdwDeEOEIjbJKGyAA;

				public List<nujuvJulgYzNRYueyruWPgnaqlHP> nKZygYNGQLtpMTjCCgCOFpgVORoI;

				public Func<ControllerType, List<nujuvJulgYzNRYueyruWPgnaqlHP>> kIDFsCBDuKsjohFGHGJKOkzFMmDb;

				public List<nujuvJulgYzNRYueyruWPgnaqlHP> UzvGjfkjNdAcVEwwAgeRhisBAbmgA;

				public List<nujuvJulgYzNRYueyruWPgnaqlHP> FJFXLnErPbqqkKvYfbEKQDpejRyH;

				public List<nujuvJulgYzNRYueyruWPgnaqlHP> AYlBweOATLFqPIszncJPPLFytPuK;

				public List<nujuvJulgYzNRYueyruWPgnaqlHP> ATTEKddHkreaqveCPUFoCfIJHcBEb;

				internal InputCategory RoagBLmVYnLGflCWtGsqKaAqFgKp(koiccHreeSzhhtmqpAfbekazGvOAb<InputCategory> P_0)
				{
					InputCategory inputCategory = JsonTools.Clone(P_0.UNvoUrngiTAgvWKCtPziXkqUPtZk);
					InputCategory inputCategory2;
					if (P_0.idYOzFFtwFXzmBEeMDPCXTFwffBf)
					{
						inputCategory2 = P_0.ZUyOTflBkjftSKlGjdzKIQDMvsbu;
					}
					else
					{
						HDkgCuGOBNmSZoCUejZdOdigNGDNA.AddActionCategory();
						inputCategory2 = P_0.zRnCKWcMztlPqYjfLLdxSVnTlRAdA[P_0.zRnCKWcMztlPqYjfLLdxSVnTlRAdA.Count - 1];
					}
					inputCategory.id = inputCategory2.id;
					int index = P_0.zRnCKWcMztlPqYjfLLdxSVnTlRAdA.IndexOf(inputCategory2);
					P_0.zRnCKWcMztlPqYjfLLdxSVnTlRAdA[index] = inputCategory;
					return inputCategory;
				}

				internal InputBehavior CSQikAhJBJoauvRxetozTfyGiCDP(koiccHreeSzhhtmqpAfbekazGvOAb<InputBehavior> P_0)
				{
					InputBehavior inputBehavior = JsonTools.Clone(P_0.UNvoUrngiTAgvWKCtPziXkqUPtZk);
					InputBehavior inputBehavior2;
					if (P_0.idYOzFFtwFXzmBEeMDPCXTFwffBf)
					{
						inputBehavior2 = P_0.ZUyOTflBkjftSKlGjdzKIQDMvsbu;
					}
					else
					{
						HDkgCuGOBNmSZoCUejZdOdigNGDNA.AddInputBehavior();
						inputBehavior2 = P_0.zRnCKWcMztlPqYjfLLdxSVnTlRAdA[P_0.zRnCKWcMztlPqYjfLLdxSVnTlRAdA.Count - 1];
					}
					inputBehavior.id = inputBehavior2.id;
					int index = P_0.zRnCKWcMztlPqYjfLLdxSVnTlRAdA.IndexOf(inputBehavior2);
					P_0.zRnCKWcMztlPqYjfLLdxSVnTlRAdA[index] = inputBehavior;
					return inputBehavior;
				}

				internal InputAction DROJRVXuYvykHVLACTJHqhnTNklb(koiccHreeSzhhtmqpAfbekazGvOAb<InputAction> P_0)
				{
					yicetfgQWuctWWPYHzEcTHePwpqP yicetfgQWuctWWPYHzEcTHePwpqP2 = new yicetfgQWuctWWPYHzEcTHePwpqP();
					yicetfgQWuctWWPYHzEcTHePwpqP2.RRKYNJnhsfaXVVTNYbIaGqeAMQLb = P_0;
					InputAction inputAction = JsonTools.Clone(yicetfgQWuctWWPYHzEcTHePwpqP2.RRKYNJnhsfaXVVTNYbIaGqeAMQLb.UNvoUrngiTAgvWKCtPziXkqUPtZk);
					int num = KBLwofECeiBpLcJHNBOtMFGqCohXA.Find(yicetfgQWuctWWPYHzEcTHePwpqP2.YmioNGoqOoaJMLbRumASckKbBbDbA)?.NmvBXsDtIAXqBCeQpumPWRwoEYyoA ?? 0;
					InputAction inputAction2;
					if (yicetfgQWuctWWPYHzEcTHePwpqP2.RRKYNJnhsfaXVVTNYbIaGqeAMQLb.idYOzFFtwFXzmBEeMDPCXTFwffBf)
					{
						inputAction2 = yicetfgQWuctWWPYHzEcTHePwpqP2.RRKYNJnhsfaXVVTNYbIaGqeAMQLb.ZUyOTflBkjftSKlGjdzKIQDMvsbu;
					}
					else
					{
						HDkgCuGOBNmSZoCUejZdOdigNGDNA.AddAction(num);
						inputAction2 = yicetfgQWuctWWPYHzEcTHePwpqP2.RRKYNJnhsfaXVVTNYbIaGqeAMQLb.zRnCKWcMztlPqYjfLLdxSVnTlRAdA[yicetfgQWuctWWPYHzEcTHePwpqP2.RRKYNJnhsfaXVVTNYbIaGqeAMQLb.zRnCKWcMztlPqYjfLLdxSVnTlRAdA.Count - 1];
					}
					int num2 = QRoDQvTXtDUJBScICbROkUpBwmHqA.Find(yicetfgQWuctWWPYHzEcTHePwpqP2.ByHAtBGzgKdXyMcPvZFnvUVSauNiA)?.NmvBXsDtIAXqBCeQpumPWRwoEYyoA ?? 0;
					inputAction.id = inputAction2.id;
					if (num != inputAction2.categoryId)
					{
						HDkgCuGOBNmSZoCUejZdOdigNGDNA.ChangeActionCategory(inputAction2.id, num);
					}
					inputAction.categoryId = num;
					inputAction.behaviorId = num2;
					int index = yicetfgQWuctWWPYHzEcTHePwpqP2.RRKYNJnhsfaXVVTNYbIaGqeAMQLb.zRnCKWcMztlPqYjfLLdxSVnTlRAdA.IndexOf(inputAction2);
					yicetfgQWuctWWPYHzEcTHePwpqP2.RRKYNJnhsfaXVVTNYbIaGqeAMQLb.zRnCKWcMztlPqYjfLLdxSVnTlRAdA[index] = inputAction;
					return inputAction;
				}

				internal InputLayout FcIlovTflCfNFfsDcYzEcUBPixHv(koiccHreeSzhhtmqpAfbekazGvOAb<InputLayout> P_0)
				{
					InputLayout inputLayout = JsonTools.Clone(P_0.UNvoUrngiTAgvWKCtPziXkqUPtZk);
					InputLayout inputLayout2;
					if (P_0.idYOzFFtwFXzmBEeMDPCXTFwffBf)
					{
						inputLayout2 = P_0.ZUyOTflBkjftSKlGjdzKIQDMvsbu;
					}
					else
					{
						HDkgCuGOBNmSZoCUejZdOdigNGDNA.AddKeyboardLayout();
						inputLayout2 = P_0.zRnCKWcMztlPqYjfLLdxSVnTlRAdA[P_0.zRnCKWcMztlPqYjfLLdxSVnTlRAdA.Count - 1];
					}
					inputLayout.id = inputLayout2.id;
					int index = P_0.zRnCKWcMztlPqYjfLLdxSVnTlRAdA.IndexOf(inputLayout2);
					P_0.zRnCKWcMztlPqYjfLLdxSVnTlRAdA[index] = inputLayout;
					return inputLayout;
				}

				internal InputLayout qSYNuusNqJHVdXdjTBFycHAgLUpg(koiccHreeSzhhtmqpAfbekazGvOAb<InputLayout> P_0)
				{
					InputLayout inputLayout = JsonTools.Clone(P_0.UNvoUrngiTAgvWKCtPziXkqUPtZk);
					InputLayout inputLayout2;
					if (P_0.idYOzFFtwFXzmBEeMDPCXTFwffBf)
					{
						inputLayout2 = P_0.ZUyOTflBkjftSKlGjdzKIQDMvsbu;
					}
					else
					{
						HDkgCuGOBNmSZoCUejZdOdigNGDNA.AddMouseLayout();
						inputLayout2 = P_0.zRnCKWcMztlPqYjfLLdxSVnTlRAdA[P_0.zRnCKWcMztlPqYjfLLdxSVnTlRAdA.Count - 1];
					}
					inputLayout.id = inputLayout2.id;
					int index = P_0.zRnCKWcMztlPqYjfLLdxSVnTlRAdA.IndexOf(inputLayout2);
					P_0.zRnCKWcMztlPqYjfLLdxSVnTlRAdA[index] = inputLayout;
					return inputLayout;
				}

				internal InputLayout WbKXFFyrbclzxNyWdqEkRbtkRmht(koiccHreeSzhhtmqpAfbekazGvOAb<InputLayout> P_0)
				{
					InputLayout inputLayout = JsonTools.Clone(P_0.UNvoUrngiTAgvWKCtPziXkqUPtZk);
					InputLayout inputLayout2;
					if (P_0.idYOzFFtwFXzmBEeMDPCXTFwffBf)
					{
						inputLayout2 = P_0.ZUyOTflBkjftSKlGjdzKIQDMvsbu;
					}
					else
					{
						HDkgCuGOBNmSZoCUejZdOdigNGDNA.AddJoystickLayout();
						inputLayout2 = P_0.zRnCKWcMztlPqYjfLLdxSVnTlRAdA[P_0.zRnCKWcMztlPqYjfLLdxSVnTlRAdA.Count - 1];
					}
					inputLayout.id = inputLayout2.id;
					int index = P_0.zRnCKWcMztlPqYjfLLdxSVnTlRAdA.IndexOf(inputLayout2);
					P_0.zRnCKWcMztlPqYjfLLdxSVnTlRAdA[index] = inputLayout;
					return inputLayout;
				}

				internal InputLayout RlasGeiTvXsNDDZDLtwGHrUeGOaP(koiccHreeSzhhtmqpAfbekazGvOAb<InputLayout> P_0)
				{
					InputLayout inputLayout = JsonTools.Clone(P_0.UNvoUrngiTAgvWKCtPziXkqUPtZk);
					InputLayout inputLayout2;
					if (P_0.idYOzFFtwFXzmBEeMDPCXTFwffBf)
					{
						inputLayout2 = P_0.ZUyOTflBkjftSKlGjdzKIQDMvsbu;
					}
					else
					{
						HDkgCuGOBNmSZoCUejZdOdigNGDNA.AddCustomControllerLayout();
						inputLayout2 = P_0.zRnCKWcMztlPqYjfLLdxSVnTlRAdA[P_0.zRnCKWcMztlPqYjfLLdxSVnTlRAdA.Count - 1];
					}
					inputLayout.id = inputLayout2.id;
					int index = P_0.zRnCKWcMztlPqYjfLLdxSVnTlRAdA.IndexOf(inputLayout2);
					P_0.zRnCKWcMztlPqYjfLLdxSVnTlRAdA[index] = inputLayout;
					return inputLayout;
				}

				internal List<nujuvJulgYzNRYueyruWPgnaqlHP> nTcYoreblegtunEHVXPeEGVPVzZg(ControllerType P_0)
				{
					return P_0 switch
					{
						ControllerType.Keyboard => LxnWPQAbzstdNmqElICPYQIjQnmw, 
						ControllerType.Mouse => BGClHotKwyDHRcCAZxUtRuojYxAeA, 
						ControllerType.Joystick => lhhvePGLXyqhBobYgHQMxKdAbBkP, 
						ControllerType.Custom => gymYSGRESagpXdwDeEOEIjbJKGyAA, 
						_ => throw new NotImplementedException(), 
					};
				}

				internal CustomController_Editor VqNTixjBXVIMUCziRHqrvOrxYEUF(koiccHreeSzhhtmqpAfbekazGvOAb<CustomController_Editor> P_0)
				{
					CustomController_Editor customController_Editor = JsonTools.Clone(P_0.UNvoUrngiTAgvWKCtPziXkqUPtZk);
					CustomController_Editor customController_Editor2;
					if (P_0.idYOzFFtwFXzmBEeMDPCXTFwffBf)
					{
						customController_Editor2 = P_0.ZUyOTflBkjftSKlGjdzKIQDMvsbu;
					}
					else
					{
						HDkgCuGOBNmSZoCUejZdOdigNGDNA.AddCustomController();
						customController_Editor2 = P_0.zRnCKWcMztlPqYjfLLdxSVnTlRAdA[P_0.zRnCKWcMztlPqYjfLLdxSVnTlRAdA.Count - 1];
					}
					customController_Editor.id = customController_Editor2.id;
					int index = P_0.zRnCKWcMztlPqYjfLLdxSVnTlRAdA.IndexOf(customController_Editor2);
					P_0.zRnCKWcMztlPqYjfLLdxSVnTlRAdA[index] = customController_Editor;
					return customController_Editor;
				}

				internal ControllerMapLayoutManager_RuleSet_Editor hLhsLJlMwsLsPxmCOPySytPWVEiL(koiccHreeSzhhtmqpAfbekazGvOAb<ControllerMapLayoutManager_RuleSet_Editor> P_0)
				{
					bMdGGcOBzYLlMEMrAsYyglzkIMoF bMdGGcOBzYLlMEMrAsYyglzkIMoF2 = new bMdGGcOBzYLlMEMrAsYyglzkIMoF();
					bMdGGcOBzYLlMEMrAsYyglzkIMoF2.RRKYNJnhsfaXVVTNYbIaGqeAMQLb = P_0;
					ControllerMapLayoutManager_RuleSet_Editor controllerMapLayoutManager_RuleSet_Editor = JsonTools.Clone(bMdGGcOBzYLlMEMrAsYyglzkIMoF2.RRKYNJnhsfaXVVTNYbIaGqeAMQLb.UNvoUrngiTAgvWKCtPziXkqUPtZk);
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
							XZNZoBICPHqcWoiOvRZiNSokcuWq xZNZoBICPHqcWoiOvRZiNSokcuWq = new XZNZoBICPHqcWoiOvRZiNSokcuWq();
							xZNZoBICPHqcWoiOvRZiNSokcuWq.voFPLgLgADgMZJodHztIjhUlouiu = bMdGGcOBzYLlMEMrAsYyglzkIMoF2;
							xZNZoBICPHqcWoiOvRZiNSokcuWq.xUdfZqFEnYagGOnOGpNjcAwEvfyBA = controllerMapLayoutManager_Rule_Editor.categoryIds[j];
							nujuvJulgYzNRYueyruWPgnaqlHP nujuvJulgYzNRYueyruWPgnaqlHP2 = nKZygYNGQLtpMTjCCgCOFpgVORoI.Find(xZNZoBICPHqcWoiOvRZiNSokcuWq.CSkFkGiACoCwhlnPEHOPhUJgEozEA);
							if (nujuvJulgYzNRYueyruWPgnaqlHP2 == null)
							{
								Logger.LogError("No new Map Category Id found for old id: " + xZNZoBICPHqcWoiOvRZiNSokcuWq.xUdfZqFEnYagGOnOGpNjcAwEvfyBA);
							}
							else
							{
								list.Add(nujuvJulgYzNRYueyruWPgnaqlHP2.NmvBXsDtIAXqBCeQpumPWRwoEYyoA);
							}
						}
						controllerMapLayoutManager_Rule_Editor.categoryIds = list;
					}
					int num3 = ((controllerMapLayoutManager_RuleSet_Editor.rules != null) ? controllerMapLayoutManager_RuleSet_Editor.rules.Count : 0);
					for (int k = 0; k < num3; k++)
					{
						sjWQylShnInRKAGhpVZdMyViQiKh sjWQylShnInRKAGhpVZdMyViQiKh2 = new sjWQylShnInRKAGhpVZdMyViQiKh();
						sjWQylShnInRKAGhpVZdMyViQiKh2.vAlvmBHCkpDaSiHXzsxuLeDzJcvDA = bMdGGcOBzYLlMEMrAsYyglzkIMoF2;
						ControllerMapLayoutManager_Rule_Editor controllerMapLayoutManager_Rule_Editor2 = controllerMapLayoutManager_RuleSet_Editor.rules[k];
						if (controllerMapLayoutManager_Rule_Editor2 != null && controllerMapLayoutManager_Rule_Editor2.layoutId > 0)
						{
							ControllerType controllerType = controllerMapLayoutManager_Rule_Editor2.controllerSetSelector.controllerType;
							List<nujuvJulgYzNRYueyruWPgnaqlHP> list2 = kIDFsCBDuKsjohFGHGJKOkzFMmDb(controllerType);
							sjWQylShnInRKAGhpVZdMyViQiKh2.xUdfZqFEnYagGOnOGpNjcAwEvfyBA = controllerMapLayoutManager_Rule_Editor2.layoutId;
							nujuvJulgYzNRYueyruWPgnaqlHP nujuvJulgYzNRYueyruWPgnaqlHP3 = list2.Find(sjWQylShnInRKAGhpVZdMyViQiKh2.JnRADBCuuQjYOPDrWipeqdWgrsxc);
							if (nujuvJulgYzNRYueyruWPgnaqlHP3 == null)
							{
								controllerMapLayoutManager_Rule_Editor2.layoutId = -1;
								Logger.LogError("No new " + controllerType.ToString() + " Layout Id found for old id: " + sjWQylShnInRKAGhpVZdMyViQiKh2.xUdfZqFEnYagGOnOGpNjcAwEvfyBA);
							}
							else
							{
								controllerMapLayoutManager_Rule_Editor2.layoutId = nujuvJulgYzNRYueyruWPgnaqlHP3.NmvBXsDtIAXqBCeQpumPWRwoEYyoA;
							}
						}
					}
					int num4 = ((controllerMapLayoutManager_RuleSet_Editor.rules != null) ? controllerMapLayoutManager_RuleSet_Editor.rules.Count : 0);
					for (int l = 0; l < num4; l++)
					{
						ControllerMapLayoutManager_Rule_Editor controllerMapLayoutManager_Rule_Editor3 = controllerMapLayoutManager_RuleSet_Editor.rules[l];
						if (controllerMapLayoutManager_Rule_Editor3 != null && controllerMapLayoutManager_Rule_Editor3.controllerSetSelector != null && controllerMapLayoutManager_Rule_Editor3.controllerSetSelector.controllerType == ControllerType.Custom)
						{
							MPfEylRDjoIOugAgndjpkZHjJUnDA mPfEylRDjoIOugAgndjpkZHjJUnDA = new MPfEylRDjoIOugAgndjpkZHjJUnDA();
							mPfEylRDjoIOugAgndjpkZHjJUnDA.GQITxFqCRYYMQLZJgPsCjYvoizzV = bMdGGcOBzYLlMEMrAsYyglzkIMoF2;
							List<nujuvJulgYzNRYueyruWPgnaqlHP> uzvGjfkjNdAcVEwwAgeRhisBAbmgA = UzvGjfkjNdAcVEwwAgeRhisBAbmgA;
							mPfEylRDjoIOugAgndjpkZHjJUnDA.xUdfZqFEnYagGOnOGpNjcAwEvfyBA = controllerMapLayoutManager_Rule_Editor3.controllerSetSelector.customControllerSourceId;
							nujuvJulgYzNRYueyruWPgnaqlHP nujuvJulgYzNRYueyruWPgnaqlHP4 = uzvGjfkjNdAcVEwwAgeRhisBAbmgA.Find(mPfEylRDjoIOugAgndjpkZHjJUnDA.oupSWVlEMClXdZCPvzegYBEotzDc);
							if (nujuvJulgYzNRYueyruWPgnaqlHP4 == null)
							{
								controllerMapLayoutManager_Rule_Editor3.controllerSetSelector.customControllerSourceId = -1;
								Logger.LogError("No new Custom Controller found for old id: " + mPfEylRDjoIOugAgndjpkZHjJUnDA.xUdfZqFEnYagGOnOGpNjcAwEvfyBA);
							}
							else
							{
								controllerMapLayoutManager_Rule_Editor3.controllerSetSelector.customControllerSourceId = nujuvJulgYzNRYueyruWPgnaqlHP4.NmvBXsDtIAXqBCeQpumPWRwoEYyoA;
							}
						}
					}
					ControllerMapLayoutManager_RuleSet_Editor controllerMapLayoutManager_RuleSet_Editor2;
					if (bMdGGcOBzYLlMEMrAsYyglzkIMoF2.RRKYNJnhsfaXVVTNYbIaGqeAMQLb.idYOzFFtwFXzmBEeMDPCXTFwffBf)
					{
						controllerMapLayoutManager_RuleSet_Editor2 = bMdGGcOBzYLlMEMrAsYyglzkIMoF2.RRKYNJnhsfaXVVTNYbIaGqeAMQLb.ZUyOTflBkjftSKlGjdzKIQDMvsbu;
					}
					else
					{
						HDkgCuGOBNmSZoCUejZdOdigNGDNA.AddControllerMapLayoutManagerRuleSet();
						controllerMapLayoutManager_RuleSet_Editor2 = bMdGGcOBzYLlMEMrAsYyglzkIMoF2.RRKYNJnhsfaXVVTNYbIaGqeAMQLb.zRnCKWcMztlPqYjfLLdxSVnTlRAdA[bMdGGcOBzYLlMEMrAsYyglzkIMoF2.RRKYNJnhsfaXVVTNYbIaGqeAMQLb.zRnCKWcMztlPqYjfLLdxSVnTlRAdA.Count - 1];
					}
					controllerMapLayoutManager_RuleSet_Editor.id = controllerMapLayoutManager_RuleSet_Editor2.id;
					int index = bMdGGcOBzYLlMEMrAsYyglzkIMoF2.RRKYNJnhsfaXVVTNYbIaGqeAMQLb.zRnCKWcMztlPqYjfLLdxSVnTlRAdA.IndexOf(controllerMapLayoutManager_RuleSet_Editor2);
					bMdGGcOBzYLlMEMrAsYyglzkIMoF2.RRKYNJnhsfaXVVTNYbIaGqeAMQLb.zRnCKWcMztlPqYjfLLdxSVnTlRAdA[index] = controllerMapLayoutManager_RuleSet_Editor;
					return controllerMapLayoutManager_RuleSet_Editor;
				}

				internal ControllerMapEnabler_RuleSet_Editor TIcLkuPnKTmeuGkHysUaOIkdpiri(koiccHreeSzhhtmqpAfbekazGvOAb<ControllerMapEnabler_RuleSet_Editor> P_0)
				{
					NjmTWflIvcoWwNjLSpbhnjchTaOE njmTWflIvcoWwNjLSpbhnjchTaOE = new NjmTWflIvcoWwNjLSpbhnjchTaOE();
					njmTWflIvcoWwNjLSpbhnjchTaOE.RRKYNJnhsfaXVVTNYbIaGqeAMQLb = P_0;
					ControllerMapEnabler_RuleSet_Editor controllerMapEnabler_RuleSet_Editor = JsonTools.Clone(njmTWflIvcoWwNjLSpbhnjchTaOE.RRKYNJnhsfaXVVTNYbIaGqeAMQLb.UNvoUrngiTAgvWKCtPziXkqUPtZk);
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
							paOYRDHVDgzjgxFkaEZtAGrnJxGd paOYRDHVDgzjgxFkaEZtAGrnJxGd2 = new paOYRDHVDgzjgxFkaEZtAGrnJxGd();
							paOYRDHVDgzjgxFkaEZtAGrnJxGd2.GTOUyYQaGDUneWDZKDKuaXXfqahI = njmTWflIvcoWwNjLSpbhnjchTaOE;
							paOYRDHVDgzjgxFkaEZtAGrnJxGd2.xUdfZqFEnYagGOnOGpNjcAwEvfyBA = controllerMapEnabler_Rule_Editor.categoryIds[j];
							nujuvJulgYzNRYueyruWPgnaqlHP nujuvJulgYzNRYueyruWPgnaqlHP2 = nKZygYNGQLtpMTjCCgCOFpgVORoI.Find(paOYRDHVDgzjgxFkaEZtAGrnJxGd2.AOetRJqaLYoOiMNDejUDseZsXzCS);
							if (nujuvJulgYzNRYueyruWPgnaqlHP2 == null)
							{
								Logger.LogError("No new Map Category Id found for old id: " + paOYRDHVDgzjgxFkaEZtAGrnJxGd2.xUdfZqFEnYagGOnOGpNjcAwEvfyBA);
							}
							else
							{
								list.Add(nujuvJulgYzNRYueyruWPgnaqlHP2.NmvBXsDtIAXqBCeQpumPWRwoEYyoA);
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
						List<nujuvJulgYzNRYueyruWPgnaqlHP> list2 = kIDFsCBDuKsjohFGHGJKOkzFMmDb(controllerType);
						List<int> list3 = new List<int>();
						int num3 = ((controllerMapEnabler_Rule_Editor2.layoutIds != null) ? controllerMapEnabler_Rule_Editor2.layoutIds.Count : 0);
						for (int l = 0; l < num3; l++)
						{
							RllyBKPHRelsYTlGukPIYHGFjGni rllyBKPHRelsYTlGukPIYHGFjGni = new RllyBKPHRelsYTlGukPIYHGFjGni();
							rllyBKPHRelsYTlGukPIYHGFjGni.iQgwLFfaRnrLkwiNfqSshwrvtIeF = njmTWflIvcoWwNjLSpbhnjchTaOE;
							rllyBKPHRelsYTlGukPIYHGFjGni.xUdfZqFEnYagGOnOGpNjcAwEvfyBA = controllerMapEnabler_Rule_Editor2.layoutIds[l];
							nujuvJulgYzNRYueyruWPgnaqlHP nujuvJulgYzNRYueyruWPgnaqlHP3 = list2.Find(rllyBKPHRelsYTlGukPIYHGFjGni.BMfcgaEzeRpeBgikiiYnsiJytvDgA);
							if (nujuvJulgYzNRYueyruWPgnaqlHP3 == null)
							{
								Logger.LogError("No new " + controllerType.ToString() + " Layout Id found for old id: " + rllyBKPHRelsYTlGukPIYHGFjGni.xUdfZqFEnYagGOnOGpNjcAwEvfyBA);
							}
							else
							{
								list3.Add(nujuvJulgYzNRYueyruWPgnaqlHP3.NmvBXsDtIAXqBCeQpumPWRwoEYyoA);
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
							WdgWhhDRyFTsdFlsPEqqxEHareOi wdgWhhDRyFTsdFlsPEqqxEHareOi = new WdgWhhDRyFTsdFlsPEqqxEHareOi();
							wdgWhhDRyFTsdFlsPEqqxEHareOi.zUnwIrsLEhOeAEaaKlWBCbxmaobb = njmTWflIvcoWwNjLSpbhnjchTaOE;
							List<nujuvJulgYzNRYueyruWPgnaqlHP> uzvGjfkjNdAcVEwwAgeRhisBAbmgA = UzvGjfkjNdAcVEwwAgeRhisBAbmgA;
							wdgWhhDRyFTsdFlsPEqqxEHareOi.xUdfZqFEnYagGOnOGpNjcAwEvfyBA = controllerMapEnabler_Rule_Editor3.controllerSetSelector.customControllerSourceId;
							nujuvJulgYzNRYueyruWPgnaqlHP nujuvJulgYzNRYueyruWPgnaqlHP4 = uzvGjfkjNdAcVEwwAgeRhisBAbmgA.Find(wdgWhhDRyFTsdFlsPEqqxEHareOi.rwQgjGzisUDHahszxiOpkbUXuzXk);
							if (nujuvJulgYzNRYueyruWPgnaqlHP4 == null)
							{
								controllerMapEnabler_Rule_Editor3.controllerSetSelector.customControllerSourceId = -1;
								Logger.LogError("No new Custom Controller found for old id: " + wdgWhhDRyFTsdFlsPEqqxEHareOi.xUdfZqFEnYagGOnOGpNjcAwEvfyBA);
							}
							else
							{
								controllerMapEnabler_Rule_Editor3.controllerSetSelector.customControllerSourceId = nujuvJulgYzNRYueyruWPgnaqlHP4.NmvBXsDtIAXqBCeQpumPWRwoEYyoA;
							}
						}
					}
					ControllerMapEnabler_RuleSet_Editor controllerMapEnabler_RuleSet_Editor2;
					if (njmTWflIvcoWwNjLSpbhnjchTaOE.RRKYNJnhsfaXVVTNYbIaGqeAMQLb.idYOzFFtwFXzmBEeMDPCXTFwffBf)
					{
						controllerMapEnabler_RuleSet_Editor2 = njmTWflIvcoWwNjLSpbhnjchTaOE.RRKYNJnhsfaXVVTNYbIaGqeAMQLb.ZUyOTflBkjftSKlGjdzKIQDMvsbu;
					}
					else
					{
						HDkgCuGOBNmSZoCUejZdOdigNGDNA.AddControllerMapEnablerRuleSet();
						controllerMapEnabler_RuleSet_Editor2 = njmTWflIvcoWwNjLSpbhnjchTaOE.RRKYNJnhsfaXVVTNYbIaGqeAMQLb.zRnCKWcMztlPqYjfLLdxSVnTlRAdA[njmTWflIvcoWwNjLSpbhnjchTaOE.RRKYNJnhsfaXVVTNYbIaGqeAMQLb.zRnCKWcMztlPqYjfLLdxSVnTlRAdA.Count - 1];
					}
					controllerMapEnabler_RuleSet_Editor.id = controllerMapEnabler_RuleSet_Editor2.id;
					int index = njmTWflIvcoWwNjLSpbhnjchTaOE.RRKYNJnhsfaXVVTNYbIaGqeAMQLb.zRnCKWcMztlPqYjfLLdxSVnTlRAdA.IndexOf(controllerMapEnabler_RuleSet_Editor2);
					njmTWflIvcoWwNjLSpbhnjchTaOE.RRKYNJnhsfaXVVTNYbIaGqeAMQLb.zRnCKWcMztlPqYjfLLdxSVnTlRAdA[index] = controllerMapEnabler_RuleSet_Editor;
					return controllerMapEnabler_RuleSet_Editor;
				}

				internal Player_Editor lDSftqDglEUWCuoxzbyvEcigktQxA(koiccHreeSzhhtmqpAfbekazGvOAb<Player_Editor> P_0)
				{
					pTVYBZxDmTkBICMIoyQMXZZKAflF pTVYBZxDmTkBICMIoyQMXZZKAflF2 = new pTVYBZxDmTkBICMIoyQMXZZKAflF();
					pTVYBZxDmTkBICMIoyQMXZZKAflF2.mJYLhmAovvBPBLdbTwVFFpoWWsxd = this;
					pTVYBZxDmTkBICMIoyQMXZZKAflF2.RRKYNJnhsfaXVVTNYbIaGqeAMQLb = P_0;
					Player_Editor player_Editor = JsonTools.Clone(pTVYBZxDmTkBICMIoyQMXZZKAflF2.RRKYNJnhsfaXVVTNYbIaGqeAMQLb.UNvoUrngiTAgvWKCtPziXkqUPtZk);
					Action<List<Player_Editor.Mapping>, List<nujuvJulgYzNRYueyruWPgnaqlHP>> action = pTVYBZxDmTkBICMIoyQMXZZKAflF2.DrKBMHuhEGBcCnkvBJlHYqtiKaoB;
					action(player_Editor.defaultKeyboardMaps, LxnWPQAbzstdNmqElICPYQIjQnmw);
					action(player_Editor.defaultMouseMaps, BGClHotKwyDHRcCAZxUtRuojYxAeA);
					action(player_Editor.defaultJoystickMaps, lhhvePGLXyqhBobYgHQMxKdAbBkP);
					action(player_Editor.defaultCustomControllerMaps, gymYSGRESagpXdwDeEOEIjbJKGyAA);
					for (int i = 0; i < player_Editor.startingCustomControllers.Count; i++)
					{
						NzgMcWYizTOlmdEyxYPtJtWLCvrl nzgMcWYizTOlmdEyxYPtJtWLCvrl = new NzgMcWYizTOlmdEyxYPtJtWLCvrl();
						nzgMcWYizTOlmdEyxYPtJtWLCvrl.SQtcnpesErssqFMijJyuuBnKJxtcB = pTVYBZxDmTkBICMIoyQMXZZKAflF2;
						nzgMcWYizTOlmdEyxYPtJtWLCvrl.lPMziJCFRnvinaKeGNwjXRpuzYwf = player_Editor.startingCustomControllers[i];
						nujuvJulgYzNRYueyruWPgnaqlHP nujuvJulgYzNRYueyruWPgnaqlHP2 = UzvGjfkjNdAcVEwwAgeRhisBAbmgA.Find(nzgMcWYizTOlmdEyxYPtJtWLCvrl.swfwmaLHDYwCeUntVkAmxRGAgRvT);
						nzgMcWYizTOlmdEyxYPtJtWLCvrl.lPMziJCFRnvinaKeGNwjXRpuzYwf.sourceId = nujuvJulgYzNRYueyruWPgnaqlHP2?.NmvBXsDtIAXqBCeQpumPWRwoEYyoA ?? (-1);
					}
					List<Player_Editor.RuleSetMapping> list = new List<Player_Editor.RuleSetMapping>();
					List<Player_Editor.RuleSetMapping> ruleSets = player_Editor.controllerMapLayoutManagerSettings.ruleSets;
					for (int j = 0; j < ruleSets.Count; j++)
					{
						NZsiuNxiSRDLrjpjfWVmQuoecEUqA nZsiuNxiSRDLrjpjfWVmQuoecEUqA = new NZsiuNxiSRDLrjpjfWVmQuoecEUqA();
						nZsiuNxiSRDLrjpjfWVmQuoecEUqA.pciAeQAYjtYoqLSRXgluwKjADgIyA = pTVYBZxDmTkBICMIoyQMXZZKAflF2;
						Player_Editor.RuleSetMapping ruleSetMapping = ruleSets[j];
						if (ruleSetMapping != null)
						{
							nZsiuNxiSRDLrjpjfWVmQuoecEUqA.QSZlWSsqUhpIVZjOleCIDuxHVUuQ = ruleSetMapping.id;
							nujuvJulgYzNRYueyruWPgnaqlHP nujuvJulgYzNRYueyruWPgnaqlHP3 = FJFXLnErPbqqkKvYfbEKQDpejRyH.Find(nZsiuNxiSRDLrjpjfWVmQuoecEUqA.elrgPdndREIuGpoJueKanITWDKJc);
							if (nujuvJulgYzNRYueyruWPgnaqlHP3 == null)
							{
								Logger.LogError("No new Controller Map Layout Manager Set found for old id: " + nZsiuNxiSRDLrjpjfWVmQuoecEUqA.QSZlWSsqUhpIVZjOleCIDuxHVUuQ);
								continue;
							}
							ruleSetMapping = ruleSetMapping.Clone();
							ruleSetMapping.id = nujuvJulgYzNRYueyruWPgnaqlHP3.NmvBXsDtIAXqBCeQpumPWRwoEYyoA;
							list.Add(ruleSetMapping);
						}
					}
					player_Editor.controllerMapLayoutManagerSettings.ruleSets = list;
					List<Player_Editor.RuleSetMapping> list2 = new List<Player_Editor.RuleSetMapping>();
					List<Player_Editor.RuleSetMapping> ruleSets2 = player_Editor.controllerMapEnablerSettings.ruleSets;
					for (int k = 0; k < ruleSets2.Count; k++)
					{
						eIfBdiiZAgubKxGorzvCmKsGbnOo eIfBdiiZAgubKxGorzvCmKsGbnOo2 = new eIfBdiiZAgubKxGorzvCmKsGbnOo();
						eIfBdiiZAgubKxGorzvCmKsGbnOo2.OIWzGuyhPMhFBWfZNlgdgVDKtDoU = pTVYBZxDmTkBICMIoyQMXZZKAflF2;
						Player_Editor.RuleSetMapping ruleSetMapping2 = ruleSets2[k];
						if (ruleSetMapping2 != null)
						{
							eIfBdiiZAgubKxGorzvCmKsGbnOo2.QSZlWSsqUhpIVZjOleCIDuxHVUuQ = ruleSetMapping2.id;
							nujuvJulgYzNRYueyruWPgnaqlHP nujuvJulgYzNRYueyruWPgnaqlHP4 = AYlBweOATLFqPIszncJPPLFytPuK.Find(eIfBdiiZAgubKxGorzvCmKsGbnOo2.CQbqbUHQZdgCCKpLeGsimxtnJcdo);
							if (nujuvJulgYzNRYueyruWPgnaqlHP4 == null)
							{
								Logger.LogError("No new Controller Map Enabler Set found for old id: " + eIfBdiiZAgubKxGorzvCmKsGbnOo2.QSZlWSsqUhpIVZjOleCIDuxHVUuQ);
								continue;
							}
							ruleSetMapping2 = ruleSetMapping2.Clone();
							ruleSetMapping2.id = nujuvJulgYzNRYueyruWPgnaqlHP4.NmvBXsDtIAXqBCeQpumPWRwoEYyoA;
							list2.Add(ruleSetMapping2);
						}
					}
					player_Editor.controllerMapEnablerSettings.ruleSets = list2;
					Player_Editor player_Editor2;
					if (pTVYBZxDmTkBICMIoyQMXZZKAflF2.RRKYNJnhsfaXVVTNYbIaGqeAMQLb.idYOzFFtwFXzmBEeMDPCXTFwffBf)
					{
						player_Editor2 = pTVYBZxDmTkBICMIoyQMXZZKAflF2.RRKYNJnhsfaXVVTNYbIaGqeAMQLb.ZUyOTflBkjftSKlGjdzKIQDMvsbu;
						Player_Editor player_Editor3 = JsonTools.Clone(player_Editor);
						player_Editor3.defaultKeyboardMaps.Clear();
						player_Editor3.defaultMouseMaps.Clear();
						player_Editor3.defaultJoystickMaps.Clear();
						player_Editor3.defaultCustomControllerMaps.Clear();
						player_Editor3.startingCustomControllers.Clear();
						Func<Player_Editor.Mapping, IList<Player_Editor.Mapping>, int> func = rEvABygUkOfYEkBliaAlBIpFisUVB._003C_003E9.OkmtevslrcOlWkCYKIDXjqOhnVfC;
						CiOTsWPWICCngyVEtwGSSYljjUcd(player_Editor2.defaultKeyboardMaps, player_Editor.defaultKeyboardMaps, player_Editor3.defaultKeyboardMaps, func);
						CiOTsWPWICCngyVEtwGSSYljjUcd(player_Editor2.defaultMouseMaps, player_Editor.defaultMouseMaps, player_Editor3.defaultMouseMaps, func);
						CiOTsWPWICCngyVEtwGSSYljjUcd(player_Editor2.defaultJoystickMaps, player_Editor.defaultJoystickMaps, player_Editor3.defaultJoystickMaps, func);
						CiOTsWPWICCngyVEtwGSSYljjUcd(player_Editor2.defaultCustomControllerMaps, player_Editor.defaultCustomControllerMaps, player_Editor3.defaultCustomControllerMaps, func);
						CiOTsWPWICCngyVEtwGSSYljjUcd(player_Editor2.startingCustomControllers, player_Editor.startingCustomControllers, player_Editor3.startingCustomControllers, rEvABygUkOfYEkBliaAlBIpFisUVB._003C_003E9.uOeWQqIBQRpKudUwTbbLhSMziPaL);
						player_Editor = player_Editor3;
					}
					else
					{
						HDkgCuGOBNmSZoCUejZdOdigNGDNA.AddPlayer();
						player_Editor2 = pTVYBZxDmTkBICMIoyQMXZZKAflF2.RRKYNJnhsfaXVVTNYbIaGqeAMQLb.zRnCKWcMztlPqYjfLLdxSVnTlRAdA[pTVYBZxDmTkBICMIoyQMXZZKAflF2.RRKYNJnhsfaXVVTNYbIaGqeAMQLb.zRnCKWcMztlPqYjfLLdxSVnTlRAdA.Count - 1];
					}
					player_Editor.id = player_Editor2.id;
					int index = pTVYBZxDmTkBICMIoyQMXZZKAflF2.RRKYNJnhsfaXVVTNYbIaGqeAMQLb.zRnCKWcMztlPqYjfLLdxSVnTlRAdA.IndexOf(player_Editor2);
					pTVYBZxDmTkBICMIoyQMXZZKAflF2.RRKYNJnhsfaXVVTNYbIaGqeAMQLb.zRnCKWcMztlPqYjfLLdxSVnTlRAdA[index] = player_Editor;
					return player_Editor;
				}
			}

			private sealed class yicetfgQWuctWWPYHzEcTHePwpqP
			{
				public koiccHreeSzhhtmqpAfbekazGvOAb<InputAction> RRKYNJnhsfaXVVTNYbIaGqeAMQLb;

				internal bool YmioNGoqOoaJMLbRumASckKbBbDbA(nujuvJulgYzNRYueyruWPgnaqlHP P_0)
				{
					return P_0.dvPsYKgTmQEzpmDbxLcTJoNaDxtw(RRKYNJnhsfaXVVTNYbIaGqeAMQLb.GpmLUZnatheysQmKSGyhdsjsLXusA) == RRKYNJnhsfaXVVTNYbIaGqeAMQLb.UNvoUrngiTAgvWKCtPziXkqUPtZk.categoryId;
				}

				internal bool ByHAtBGzgKdXyMcPvZFnvUVSauNiA(nujuvJulgYzNRYueyruWPgnaqlHP P_0)
				{
					return P_0.dvPsYKgTmQEzpmDbxLcTJoNaDxtw(RRKYNJnhsfaXVVTNYbIaGqeAMQLb.GpmLUZnatheysQmKSGyhdsjsLXusA) == RRKYNJnhsfaXVVTNYbIaGqeAMQLb.UNvoUrngiTAgvWKCtPziXkqUPtZk.behaviorId;
				}
			}

			private sealed class RllyBKPHRelsYTlGukPIYHGFjGni
			{
				public int xUdfZqFEnYagGOnOGpNjcAwEvfyBA;

				public NjmTWflIvcoWwNjLSpbhnjchTaOE iQgwLFfaRnrLkwiNfqSshwrvtIeF;

				internal bool BMfcgaEzeRpeBgikiiYnsiJytvDgA(nujuvJulgYzNRYueyruWPgnaqlHP P_0)
				{
					return P_0.dvPsYKgTmQEzpmDbxLcTJoNaDxtw(iQgwLFfaRnrLkwiNfqSshwrvtIeF.RRKYNJnhsfaXVVTNYbIaGqeAMQLb.GpmLUZnatheysQmKSGyhdsjsLXusA) == xUdfZqFEnYagGOnOGpNjcAwEvfyBA;
				}
			}

			private sealed class WdgWhhDRyFTsdFlsPEqqxEHareOi
			{
				public int xUdfZqFEnYagGOnOGpNjcAwEvfyBA;

				public NjmTWflIvcoWwNjLSpbhnjchTaOE zUnwIrsLEhOeAEaaKlWBCbxmaobb;

				internal bool rwQgjGzisUDHahszxiOpkbUXuzXk(nujuvJulgYzNRYueyruWPgnaqlHP P_0)
				{
					return P_0.dvPsYKgTmQEzpmDbxLcTJoNaDxtw(zUnwIrsLEhOeAEaaKlWBCbxmaobb.RRKYNJnhsfaXVVTNYbIaGqeAMQLb.GpmLUZnatheysQmKSGyhdsjsLXusA) == xUdfZqFEnYagGOnOGpNjcAwEvfyBA;
				}
			}

			private sealed class pTVYBZxDmTkBICMIoyQMXZZKAflF
			{
				public koiccHreeSzhhtmqpAfbekazGvOAb<Player_Editor> RRKYNJnhsfaXVVTNYbIaGqeAMQLb;

				public IXmcEKmCGRPEAZqGNAGkHqLXgNgA mJYLhmAovvBPBLdbTwVFFpoWWsxd;

				internal void DrKBMHuhEGBcCnkvBJlHYqtiKaoB(List<Player_Editor.Mapping> P_0, List<nujuvJulgYzNRYueyruWPgnaqlHP> P_1)
				{
					for (int i = 0; i < P_0.Count; i++)
					{
						dKSmlSvlyQjfDfkxbcXqIGKgZEuL dKSmlSvlyQjfDfkxbcXqIGKgZEuL2 = new dKSmlSvlyQjfDfkxbcXqIGKgZEuL();
						dKSmlSvlyQjfDfkxbcXqIGKgZEuL2.CZBBdckLGBzvoCqxAnDxaUgbRYoe = this;
						dKSmlSvlyQjfDfkxbcXqIGKgZEuL2.ZdyrardeHsUVqbcZHsfgfyoKKFfh = P_0[i];
						nujuvJulgYzNRYueyruWPgnaqlHP nujuvJulgYzNRYueyruWPgnaqlHP2 = mJYLhmAovvBPBLdbTwVFFpoWWsxd.nKZygYNGQLtpMTjCCgCOFpgVORoI.Find(dKSmlSvlyQjfDfkxbcXqIGKgZEuL2.cbYgbeaovJRpPjfqHaMxhDtWNfkv);
						dKSmlSvlyQjfDfkxbcXqIGKgZEuL2.ZdyrardeHsUVqbcZHsfgfyoKKFfh.categoryId = nujuvJulgYzNRYueyruWPgnaqlHP2?.NmvBXsDtIAXqBCeQpumPWRwoEYyoA ?? (-1);
						nujuvJulgYzNRYueyruWPgnaqlHP2 = P_1.Find(dKSmlSvlyQjfDfkxbcXqIGKgZEuL2.XZEtfeMhnnCBdTHskduFJWIpcquj);
						dKSmlSvlyQjfDfkxbcXqIGKgZEuL2.ZdyrardeHsUVqbcZHsfgfyoKKFfh.layoutId = nujuvJulgYzNRYueyruWPgnaqlHP2?.NmvBXsDtIAXqBCeQpumPWRwoEYyoA ?? (-1);
					}
				}
			}

			private sealed class dKSmlSvlyQjfDfkxbcXqIGKgZEuL
			{
				public Player_Editor.Mapping ZdyrardeHsUVqbcZHsfgfyoKKFfh;

				public pTVYBZxDmTkBICMIoyQMXZZKAflF CZBBdckLGBzvoCqxAnDxaUgbRYoe;

				internal bool cbYgbeaovJRpPjfqHaMxhDtWNfkv(nujuvJulgYzNRYueyruWPgnaqlHP P_0)
				{
					return P_0.dvPsYKgTmQEzpmDbxLcTJoNaDxtw(CZBBdckLGBzvoCqxAnDxaUgbRYoe.RRKYNJnhsfaXVVTNYbIaGqeAMQLb.GpmLUZnatheysQmKSGyhdsjsLXusA) == ZdyrardeHsUVqbcZHsfgfyoKKFfh.categoryId;
				}

				internal bool XZEtfeMhnnCBdTHskduFJWIpcquj(nujuvJulgYzNRYueyruWPgnaqlHP P_0)
				{
					return P_0.dvPsYKgTmQEzpmDbxLcTJoNaDxtw(CZBBdckLGBzvoCqxAnDxaUgbRYoe.RRKYNJnhsfaXVVTNYbIaGqeAMQLb.GpmLUZnatheysQmKSGyhdsjsLXusA) == ZdyrardeHsUVqbcZHsfgfyoKKFfh.layoutId;
				}
			}

			private sealed class NzgMcWYizTOlmdEyxYPtJtWLCvrl
			{
				public Player_Editor.CreateControllerInfo lPMziJCFRnvinaKeGNwjXRpuzYwf;

				public pTVYBZxDmTkBICMIoyQMXZZKAflF SQtcnpesErssqFMijJyuuBnKJxtcB;

				internal bool swfwmaLHDYwCeUntVkAmxRGAgRvT(nujuvJulgYzNRYueyruWPgnaqlHP P_0)
				{
					return P_0.dvPsYKgTmQEzpmDbxLcTJoNaDxtw(SQtcnpesErssqFMijJyuuBnKJxtcB.RRKYNJnhsfaXVVTNYbIaGqeAMQLb.GpmLUZnatheysQmKSGyhdsjsLXusA) == lPMziJCFRnvinaKeGNwjXRpuzYwf.sourceId;
				}
			}

			private sealed class NZsiuNxiSRDLrjpjfWVmQuoecEUqA
			{
				public int QSZlWSsqUhpIVZjOleCIDuxHVUuQ;

				public pTVYBZxDmTkBICMIoyQMXZZKAflF pciAeQAYjtYoqLSRXgluwKjADgIyA;

				internal bool elrgPdndREIuGpoJueKanITWDKJc(nujuvJulgYzNRYueyruWPgnaqlHP P_0)
				{
					return P_0.dvPsYKgTmQEzpmDbxLcTJoNaDxtw(pciAeQAYjtYoqLSRXgluwKjADgIyA.RRKYNJnhsfaXVVTNYbIaGqeAMQLb.GpmLUZnatheysQmKSGyhdsjsLXusA) == QSZlWSsqUhpIVZjOleCIDuxHVUuQ;
				}
			}

			private sealed class eIfBdiiZAgubKxGorzvCmKsGbnOo
			{
				public int QSZlWSsqUhpIVZjOleCIDuxHVUuQ;

				public pTVYBZxDmTkBICMIoyQMXZZKAflF OIWzGuyhPMhFBWfZNlgdgVDKtDoU;

				internal bool CQbqbUHQZdgCCKpLeGsimxtnJcdo(nujuvJulgYzNRYueyruWPgnaqlHP P_0)
				{
					return P_0.dvPsYKgTmQEzpmDbxLcTJoNaDxtw(OIWzGuyhPMhFBWfZNlgdgVDKtDoU.RRKYNJnhsfaXVVTNYbIaGqeAMQLb.GpmLUZnatheysQmKSGyhdsjsLXusA) == QSZlWSsqUhpIVZjOleCIDuxHVUuQ;
				}
			}

			private sealed class HwGAEEynbtCHfHxAwGLAdfbEnEowA
			{
				public List<nujuvJulgYzNRYueyruWPgnaqlHP> dNrwwVgnnfEEnwYQWpUyLcOkUKsS;

				public IXmcEKmCGRPEAZqGNAGkHqLXgNgA WpzTXfQiXHtduXsbRVkaPikyduFg;

				internal int tNunYImhAgnzIGBdzrYDpdOhOCsy(ControllerMap_Editor P_0, IList<ControllerMap_Editor> P_1)
				{
					ihNekzkXXJxyPrpexhYcufBroosbb ihNekzkXXJxyPrpexhYcufBroosbb2 = new ihNekzkXXJxyPrpexhYcufBroosbb();
					ihNekzkXXJxyPrpexhYcufBroosbb2.lPMziJCFRnvinaKeGNwjXRpuzYwf = P_0;
					for (int i = 0; i < P_1.Count; i++)
					{
						nujuvJulgYzNRYueyruWPgnaqlHP nujuvJulgYzNRYueyruWPgnaqlHP2 = WpzTXfQiXHtduXsbRVkaPikyduFg.nKZygYNGQLtpMTjCCgCOFpgVORoI.Find(ihNekzkXXJxyPrpexhYcufBroosbb2.hdATfXblKCBshspxTJCVFchjfXWh);
						nujuvJulgYzNRYueyruWPgnaqlHP nujuvJulgYzNRYueyruWPgnaqlHP3 = dNrwwVgnnfEEnwYQWpUyLcOkUKsS.Find(ihNekzkXXJxyPrpexhYcufBroosbb2.OfoSylPRureNFQTuQOxYRJaEharW);
						if (nujuvJulgYzNRYueyruWPgnaqlHP2 != null && nujuvJulgYzNRYueyruWPgnaqlHP2.NmvBXsDtIAXqBCeQpumPWRwoEYyoA == P_1[i].categoryId && nujuvJulgYzNRYueyruWPgnaqlHP3 != null && nujuvJulgYzNRYueyruWPgnaqlHP3.NmvBXsDtIAXqBCeQpumPWRwoEYyoA == P_1[i].layoutId)
						{
							return i;
						}
					}
					return -1;
				}

				internal ControllerMap_Editor xlsAcNkucHePEgcNHFMnHMlqGRxx(koiccHreeSzhhtmqpAfbekazGvOAb<ControllerMap_Editor> P_0)
				{
					ZrGFxvzXkEvzUiPNlvIZQnjvjUUd zrGFxvzXkEvzUiPNlvIZQnjvjUUd = new ZrGFxvzXkEvzUiPNlvIZQnjvjUUd();
					zrGFxvzXkEvzUiPNlvIZQnjvjUUd.RRKYNJnhsfaXVVTNYbIaGqeAMQLb = P_0;
					zrGFxvzXkEvzUiPNlvIZQnjvjUUd.KtLcYaFGtXgWVWKJtALgxYQNZlfKA = JsonTools.Clone(zrGFxvzXkEvzUiPNlvIZQnjvjUUd.RRKYNJnhsfaXVVTNYbIaGqeAMQLb.UNvoUrngiTAgvWKCtPziXkqUPtZk);
					nujuvJulgYzNRYueyruWPgnaqlHP nujuvJulgYzNRYueyruWPgnaqlHP2 = WpzTXfQiXHtduXsbRVkaPikyduFg.nKZygYNGQLtpMTjCCgCOFpgVORoI.Find(zrGFxvzXkEvzUiPNlvIZQnjvjUUd.KPktpLKWfhDaLhNteyoQWYxyfZlq);
					nujuvJulgYzNRYueyruWPgnaqlHP nujuvJulgYzNRYueyruWPgnaqlHP3 = dNrwwVgnnfEEnwYQWpUyLcOkUKsS.Find(zrGFxvzXkEvzUiPNlvIZQnjvjUUd.kgCfcpDaAAmnWVMbvEgpqfMardFqA);
					zrGFxvzXkEvzUiPNlvIZQnjvjUUd.KtLcYaFGtXgWVWKJtALgxYQNZlfKA.categoryId = nujuvJulgYzNRYueyruWPgnaqlHP2?.NmvBXsDtIAXqBCeQpumPWRwoEYyoA ?? (-1);
					zrGFxvzXkEvzUiPNlvIZQnjvjUUd.KtLcYaFGtXgWVWKJtALgxYQNZlfKA.layoutId = nujuvJulgYzNRYueyruWPgnaqlHP3?.NmvBXsDtIAXqBCeQpumPWRwoEYyoA ?? (-1);
					for (int i = 0; i < zrGFxvzXkEvzUiPNlvIZQnjvjUUd.KtLcYaFGtXgWVWKJtALgxYQNZlfKA.actionElementMaps.Count; i++)
					{
						tkhEfTtHngCKbeKkAgFVYfIyeFELA tkhEfTtHngCKbeKkAgFVYfIyeFELA2 = new tkhEfTtHngCKbeKkAgFVYfIyeFELA();
						tkhEfTtHngCKbeKkAgFVYfIyeFELA2.lvIOpgLQWCUaIQspGwMpnhsgApUm = zrGFxvzXkEvzUiPNlvIZQnjvjUUd;
						tkhEfTtHngCKbeKkAgFVYfIyeFELA2.ZdyrardeHsUVqbcZHsfgfyoKKFfh = tkhEfTtHngCKbeKkAgFVYfIyeFELA2.lvIOpgLQWCUaIQspGwMpnhsgApUm.KtLcYaFGtXgWVWKJtALgxYQNZlfKA.actionElementMaps[i];
						nujuvJulgYzNRYueyruWPgnaqlHP nujuvJulgYzNRYueyruWPgnaqlHP4 = WpzTXfQiXHtduXsbRVkaPikyduFg.ATTEKddHkreaqveCPUFoCfIJHcBEb.Find(tkhEfTtHngCKbeKkAgFVYfIyeFELA2.mNLQlUibyXKEOUvbvqcRPoeCNtfU);
						tkhEfTtHngCKbeKkAgFVYfIyeFELA2.ZdyrardeHsUVqbcZHsfgfyoKKFfh._actionId = nujuvJulgYzNRYueyruWPgnaqlHP4?.NmvBXsDtIAXqBCeQpumPWRwoEYyoA ?? (-1);
						tkhEfTtHngCKbeKkAgFVYfIyeFELA2.ZdyrardeHsUVqbcZHsfgfyoKKFfh._actionCategoryId = ((WpzTXfQiXHtduXsbRVkaPikyduFg.HDkgCuGOBNmSZoCUejZdOdigNGDNA.GetActionById(tkhEfTtHngCKbeKkAgFVYfIyeFELA2.ZdyrardeHsUVqbcZHsfgfyoKKFfh._actionId) != null) ? WpzTXfQiXHtduXsbRVkaPikyduFg.HDkgCuGOBNmSZoCUejZdOdigNGDNA.GetActionById(tkhEfTtHngCKbeKkAgFVYfIyeFELA2.ZdyrardeHsUVqbcZHsfgfyoKKFfh._actionId).categoryId : 0);
					}
					ControllerMap_Editor controllerMap_Editor;
					if (zrGFxvzXkEvzUiPNlvIZQnjvjUUd.RRKYNJnhsfaXVVTNYbIaGqeAMQLb.idYOzFFtwFXzmBEeMDPCXTFwffBf)
					{
						controllerMap_Editor = zrGFxvzXkEvzUiPNlvIZQnjvjUUd.RRKYNJnhsfaXVVTNYbIaGqeAMQLb.ZUyOTflBkjftSKlGjdzKIQDMvsbu;
						ControllerMap_Editor controllerMap_Editor2 = JsonTools.Clone(zrGFxvzXkEvzUiPNlvIZQnjvjUUd.KtLcYaFGtXgWVWKJtALgxYQNZlfKA);
						controllerMap_Editor2.actionElementMaps.Clear();
						Func<ActionElementMap, IList<ActionElementMap>, int> func = rEvABygUkOfYEkBliaAlBIpFisUVB._003C_003E9.WKNpLOtfkHlvMFaHuIvRkCgnGBBx;
						CiOTsWPWICCngyVEtwGSSYljjUcd(controllerMap_Editor.actionElementMaps, zrGFxvzXkEvzUiPNlvIZQnjvjUUd.KtLcYaFGtXgWVWKJtALgxYQNZlfKA.actionElementMaps, controllerMap_Editor2.actionElementMaps, func);
						zrGFxvzXkEvzUiPNlvIZQnjvjUUd.KtLcYaFGtXgWVWKJtALgxYQNZlfKA = controllerMap_Editor2;
					}
					else
					{
						WpzTXfQiXHtduXsbRVkaPikyduFg.HDkgCuGOBNmSZoCUejZdOdigNGDNA.CreateKeyboardMap(zrGFxvzXkEvzUiPNlvIZQnjvjUUd.KtLcYaFGtXgWVWKJtALgxYQNZlfKA.categoryId, zrGFxvzXkEvzUiPNlvIZQnjvjUUd.KtLcYaFGtXgWVWKJtALgxYQNZlfKA.layoutId);
						controllerMap_Editor = zrGFxvzXkEvzUiPNlvIZQnjvjUUd.RRKYNJnhsfaXVVTNYbIaGqeAMQLb.zRnCKWcMztlPqYjfLLdxSVnTlRAdA[zrGFxvzXkEvzUiPNlvIZQnjvjUUd.RRKYNJnhsfaXVVTNYbIaGqeAMQLb.zRnCKWcMztlPqYjfLLdxSVnTlRAdA.Count - 1];
					}
					zrGFxvzXkEvzUiPNlvIZQnjvjUUd.KtLcYaFGtXgWVWKJtALgxYQNZlfKA.id = controllerMap_Editor.id;
					int index = zrGFxvzXkEvzUiPNlvIZQnjvjUUd.RRKYNJnhsfaXVVTNYbIaGqeAMQLb.zRnCKWcMztlPqYjfLLdxSVnTlRAdA.IndexOf(controllerMap_Editor);
					zrGFxvzXkEvzUiPNlvIZQnjvjUUd.RRKYNJnhsfaXVVTNYbIaGqeAMQLb.zRnCKWcMztlPqYjfLLdxSVnTlRAdA[index] = zrGFxvzXkEvzUiPNlvIZQnjvjUUd.KtLcYaFGtXgWVWKJtALgxYQNZlfKA;
					return zrGFxvzXkEvzUiPNlvIZQnjvjUUd.KtLcYaFGtXgWVWKJtALgxYQNZlfKA;
				}
			}

			private sealed class ihNekzkXXJxyPrpexhYcufBroosbb
			{
				public ControllerMap_Editor lPMziJCFRnvinaKeGNwjXRpuzYwf;

				public Predicate<nujuvJulgYzNRYueyruWPgnaqlHP> MhowyUzXCYwDUAGyyyucNcbkeGHW;

				public Predicate<nujuvJulgYzNRYueyruWPgnaqlHP> MvqzEFLWggAAQpnUINuANvLhzVsb;

				internal bool hdATfXblKCBshspxTJCVFchjfXWh(nujuvJulgYzNRYueyruWPgnaqlHP P_0)
				{
					return P_0.okeWwwmvgkmEMOtIulLmImzRWNpc == lPMziJCFRnvinaKeGNwjXRpuzYwf.categoryId;
				}

				internal bool OfoSylPRureNFQTuQOxYRJaEharW(nujuvJulgYzNRYueyruWPgnaqlHP P_0)
				{
					return P_0.okeWwwmvgkmEMOtIulLmImzRWNpc == lPMziJCFRnvinaKeGNwjXRpuzYwf.layoutId;
				}
			}

			private sealed class ZrGFxvzXkEvzUiPNlvIZQnjvjUUd
			{
				public koiccHreeSzhhtmqpAfbekazGvOAb<ControllerMap_Editor> RRKYNJnhsfaXVVTNYbIaGqeAMQLb;

				public ControllerMap_Editor KtLcYaFGtXgWVWKJtALgxYQNZlfKA;

				internal bool KPktpLKWfhDaLhNteyoQWYxyfZlq(nujuvJulgYzNRYueyruWPgnaqlHP P_0)
				{
					return P_0.dvPsYKgTmQEzpmDbxLcTJoNaDxtw(RRKYNJnhsfaXVVTNYbIaGqeAMQLb.GpmLUZnatheysQmKSGyhdsjsLXusA) == KtLcYaFGtXgWVWKJtALgxYQNZlfKA.categoryId;
				}

				internal bool kgCfcpDaAAmnWVMbvEgpqfMardFqA(nujuvJulgYzNRYueyruWPgnaqlHP P_0)
				{
					return P_0.dvPsYKgTmQEzpmDbxLcTJoNaDxtw(RRKYNJnhsfaXVVTNYbIaGqeAMQLb.GpmLUZnatheysQmKSGyhdsjsLXusA) == KtLcYaFGtXgWVWKJtALgxYQNZlfKA.layoutId;
				}
			}

			private sealed class ebqPBQNZzNmxoEAcujnfePaDuuXK
			{
				public List<int> mRJkZfgrfrxTQWaYQkmZeqNVNofj;

				public IXmcEKmCGRPEAZqGNAGkHqLXgNgA cRXBQcKBiFKKgieBjgErDNqfQAQfc;

				internal InputMapCategory LGHqtvOofWNegEjpSgMAfHfqhlMeb(koiccHreeSzhhtmqpAfbekazGvOAb<InputMapCategory> P_0)
				{
					InputMapCategory inputMapCategory = JsonTools.Clone(P_0.UNvoUrngiTAgvWKCtPziXkqUPtZk);
					InputMapCategory inputMapCategory2;
					if (P_0.idYOzFFtwFXzmBEeMDPCXTFwffBf)
					{
						inputMapCategory2 = P_0.ZUyOTflBkjftSKlGjdzKIQDMvsbu;
					}
					else
					{
						cRXBQcKBiFKKgieBjgErDNqfQAQfc.HDkgCuGOBNmSZoCUejZdOdigNGDNA.AddMapCategory();
						inputMapCategory2 = P_0.zRnCKWcMztlPqYjfLLdxSVnTlRAdA[P_0.zRnCKWcMztlPqYjfLLdxSVnTlRAdA.Count - 1];
					}
					int num = P_0.zRnCKWcMztlPqYjfLLdxSVnTlRAdA.IndexOf(inputMapCategory2);
					if (P_0.GpmLUZnatheysQmKSGyhdsjsLXusA == nujuvJulgYzNRYueyruWPgnaqlHP.KfuHwiomPteqWnzNNKhAzBQQnmQh.otherId)
					{
						mRJkZfgrfrxTQWaYQkmZeqNVNofj.Add(num);
					}
					inputMapCategory.id = inputMapCategory2.id;
					P_0.zRnCKWcMztlPqYjfLLdxSVnTlRAdA[num] = inputMapCategory;
					return inputMapCategory;
				}
			}

			private sealed class tkhEfTtHngCKbeKkAgFVYfIyeFELA
			{
				public ActionElementMap ZdyrardeHsUVqbcZHsfgfyoKKFfh;

				public ZrGFxvzXkEvzUiPNlvIZQnjvjUUd lvIOpgLQWCUaIQspGwMpnhsgApUm;

				internal bool mNLQlUibyXKEOUvbvqcRPoeCNtfU(nujuvJulgYzNRYueyruWPgnaqlHP P_0)
				{
					return P_0.dvPsYKgTmQEzpmDbxLcTJoNaDxtw(lvIOpgLQWCUaIQspGwMpnhsgApUm.RRKYNJnhsfaXVVTNYbIaGqeAMQLb.GpmLUZnatheysQmKSGyhdsjsLXusA) == ZdyrardeHsUVqbcZHsfgfyoKKFfh._actionId;
				}
			}

			private sealed class gLCYxcpqdiaoCVDOvUkhQEaCoxxN
			{
				public List<nujuvJulgYzNRYueyruWPgnaqlHP> dNrwwVgnnfEEnwYQWpUyLcOkUKsS;

				public IXmcEKmCGRPEAZqGNAGkHqLXgNgA dnhUusjoiebHqBkeeJRcTylnlrQQ;

				internal int GmBcvQljYDzNazNYBZFlVHJFgdPH(ControllerMap_Editor P_0, IList<ControllerMap_Editor> P_1)
				{
					CHdLBGaFvZsApwPgwmOopjhtCjFu cHdLBGaFvZsApwPgwmOopjhtCjFu = new CHdLBGaFvZsApwPgwmOopjhtCjFu();
					cHdLBGaFvZsApwPgwmOopjhtCjFu.lPMziJCFRnvinaKeGNwjXRpuzYwf = P_0;
					for (int i = 0; i < P_1.Count; i++)
					{
						nujuvJulgYzNRYueyruWPgnaqlHP nujuvJulgYzNRYueyruWPgnaqlHP2 = dnhUusjoiebHqBkeeJRcTylnlrQQ.nKZygYNGQLtpMTjCCgCOFpgVORoI.Find(cHdLBGaFvZsApwPgwmOopjhtCjFu.uqGCaVhICxHschZufYDscvyFfFEqD);
						nujuvJulgYzNRYueyruWPgnaqlHP nujuvJulgYzNRYueyruWPgnaqlHP3 = dNrwwVgnnfEEnwYQWpUyLcOkUKsS.Find(cHdLBGaFvZsApwPgwmOopjhtCjFu.AwzojJkcAaSiquoWKhdCIjYuvxzf);
						if (nujuvJulgYzNRYueyruWPgnaqlHP2 != null && nujuvJulgYzNRYueyruWPgnaqlHP2.NmvBXsDtIAXqBCeQpumPWRwoEYyoA == P_1[i].categoryId && nujuvJulgYzNRYueyruWPgnaqlHP3 != null && nujuvJulgYzNRYueyruWPgnaqlHP3.NmvBXsDtIAXqBCeQpumPWRwoEYyoA == P_1[i].layoutId)
						{
							return i;
						}
					}
					return -1;
				}

				internal ControllerMap_Editor AWIDfhgPpXYdZFMCctNAZSzqwzWf(koiccHreeSzhhtmqpAfbekazGvOAb<ControllerMap_Editor> P_0)
				{
					cRtDMzACuSlLaBgKOcNBTSuelHJbb cRtDMzACuSlLaBgKOcNBTSuelHJbb2 = new cRtDMzACuSlLaBgKOcNBTSuelHJbb();
					cRtDMzACuSlLaBgKOcNBTSuelHJbb2.RRKYNJnhsfaXVVTNYbIaGqeAMQLb = P_0;
					cRtDMzACuSlLaBgKOcNBTSuelHJbb2.KtLcYaFGtXgWVWKJtALgxYQNZlfKA = JsonTools.Clone(cRtDMzACuSlLaBgKOcNBTSuelHJbb2.RRKYNJnhsfaXVVTNYbIaGqeAMQLb.UNvoUrngiTAgvWKCtPziXkqUPtZk);
					nujuvJulgYzNRYueyruWPgnaqlHP nujuvJulgYzNRYueyruWPgnaqlHP2 = dnhUusjoiebHqBkeeJRcTylnlrQQ.nKZygYNGQLtpMTjCCgCOFpgVORoI.Find(cRtDMzACuSlLaBgKOcNBTSuelHJbb2.wkdrHuqAZrqAVTupVOqLBLnxVxlv);
					nujuvJulgYzNRYueyruWPgnaqlHP nujuvJulgYzNRYueyruWPgnaqlHP3 = dNrwwVgnnfEEnwYQWpUyLcOkUKsS.Find(cRtDMzACuSlLaBgKOcNBTSuelHJbb2.mmCFKxdaEGoOKPTnEpOJiPOHguvm);
					cRtDMzACuSlLaBgKOcNBTSuelHJbb2.KtLcYaFGtXgWVWKJtALgxYQNZlfKA.categoryId = nujuvJulgYzNRYueyruWPgnaqlHP2?.NmvBXsDtIAXqBCeQpumPWRwoEYyoA ?? (-1);
					cRtDMzACuSlLaBgKOcNBTSuelHJbb2.KtLcYaFGtXgWVWKJtALgxYQNZlfKA.layoutId = nujuvJulgYzNRYueyruWPgnaqlHP3?.NmvBXsDtIAXqBCeQpumPWRwoEYyoA ?? (-1);
					for (int i = 0; i < cRtDMzACuSlLaBgKOcNBTSuelHJbb2.KtLcYaFGtXgWVWKJtALgxYQNZlfKA.actionElementMaps.Count; i++)
					{
						ZdrecckMbGKbgbisWsqinzwIsieo zdrecckMbGKbgbisWsqinzwIsieo = new ZdrecckMbGKbgbisWsqinzwIsieo();
						zdrecckMbGKbgbisWsqinzwIsieo.NLseWTgJRBhPPcEGicMZhpxVigKdA = cRtDMzACuSlLaBgKOcNBTSuelHJbb2;
						zdrecckMbGKbgbisWsqinzwIsieo.ZdyrardeHsUVqbcZHsfgfyoKKFfh = zdrecckMbGKbgbisWsqinzwIsieo.NLseWTgJRBhPPcEGicMZhpxVigKdA.KtLcYaFGtXgWVWKJtALgxYQNZlfKA.actionElementMaps[i];
						nujuvJulgYzNRYueyruWPgnaqlHP nujuvJulgYzNRYueyruWPgnaqlHP4 = dnhUusjoiebHqBkeeJRcTylnlrQQ.ATTEKddHkreaqveCPUFoCfIJHcBEb.Find(zdrecckMbGKbgbisWsqinzwIsieo.dxdGQJHIcZlXiBvKLFPUxvXeTyjNA);
						zdrecckMbGKbgbisWsqinzwIsieo.ZdyrardeHsUVqbcZHsfgfyoKKFfh._actionId = nujuvJulgYzNRYueyruWPgnaqlHP4?.NmvBXsDtIAXqBCeQpumPWRwoEYyoA ?? (-1);
						zdrecckMbGKbgbisWsqinzwIsieo.ZdyrardeHsUVqbcZHsfgfyoKKFfh._actionCategoryId = ((dnhUusjoiebHqBkeeJRcTylnlrQQ.HDkgCuGOBNmSZoCUejZdOdigNGDNA.GetActionById(zdrecckMbGKbgbisWsqinzwIsieo.ZdyrardeHsUVqbcZHsfgfyoKKFfh._actionId) != null) ? dnhUusjoiebHqBkeeJRcTylnlrQQ.HDkgCuGOBNmSZoCUejZdOdigNGDNA.GetActionById(zdrecckMbGKbgbisWsqinzwIsieo.ZdyrardeHsUVqbcZHsfgfyoKKFfh._actionId).categoryId : 0);
					}
					ControllerMap_Editor controllerMap_Editor;
					if (cRtDMzACuSlLaBgKOcNBTSuelHJbb2.RRKYNJnhsfaXVVTNYbIaGqeAMQLb.idYOzFFtwFXzmBEeMDPCXTFwffBf)
					{
						controllerMap_Editor = cRtDMzACuSlLaBgKOcNBTSuelHJbb2.RRKYNJnhsfaXVVTNYbIaGqeAMQLb.ZUyOTflBkjftSKlGjdzKIQDMvsbu;
						ControllerMap_Editor controllerMap_Editor2 = JsonTools.Clone(cRtDMzACuSlLaBgKOcNBTSuelHJbb2.KtLcYaFGtXgWVWKJtALgxYQNZlfKA);
						controllerMap_Editor2.actionElementMaps.Clear();
						Func<ActionElementMap, IList<ActionElementMap>, int> func = rEvABygUkOfYEkBliaAlBIpFisUVB._003C_003E9.pRzCzRDBBLLCMXJAnfRjAXDpItsbb;
						CiOTsWPWICCngyVEtwGSSYljjUcd(controllerMap_Editor.actionElementMaps, cRtDMzACuSlLaBgKOcNBTSuelHJbb2.KtLcYaFGtXgWVWKJtALgxYQNZlfKA.actionElementMaps, controllerMap_Editor2.actionElementMaps, func);
						cRtDMzACuSlLaBgKOcNBTSuelHJbb2.KtLcYaFGtXgWVWKJtALgxYQNZlfKA = controllerMap_Editor2;
					}
					else
					{
						dnhUusjoiebHqBkeeJRcTylnlrQQ.HDkgCuGOBNmSZoCUejZdOdigNGDNA.CreateMouseMap(cRtDMzACuSlLaBgKOcNBTSuelHJbb2.KtLcYaFGtXgWVWKJtALgxYQNZlfKA.categoryId, cRtDMzACuSlLaBgKOcNBTSuelHJbb2.KtLcYaFGtXgWVWKJtALgxYQNZlfKA.layoutId);
						controllerMap_Editor = cRtDMzACuSlLaBgKOcNBTSuelHJbb2.RRKYNJnhsfaXVVTNYbIaGqeAMQLb.zRnCKWcMztlPqYjfLLdxSVnTlRAdA[cRtDMzACuSlLaBgKOcNBTSuelHJbb2.RRKYNJnhsfaXVVTNYbIaGqeAMQLb.zRnCKWcMztlPqYjfLLdxSVnTlRAdA.Count - 1];
					}
					cRtDMzACuSlLaBgKOcNBTSuelHJbb2.KtLcYaFGtXgWVWKJtALgxYQNZlfKA.id = controllerMap_Editor.id;
					int index = cRtDMzACuSlLaBgKOcNBTSuelHJbb2.RRKYNJnhsfaXVVTNYbIaGqeAMQLb.zRnCKWcMztlPqYjfLLdxSVnTlRAdA.IndexOf(controllerMap_Editor);
					cRtDMzACuSlLaBgKOcNBTSuelHJbb2.RRKYNJnhsfaXVVTNYbIaGqeAMQLb.zRnCKWcMztlPqYjfLLdxSVnTlRAdA[index] = cRtDMzACuSlLaBgKOcNBTSuelHJbb2.KtLcYaFGtXgWVWKJtALgxYQNZlfKA;
					return cRtDMzACuSlLaBgKOcNBTSuelHJbb2.KtLcYaFGtXgWVWKJtALgxYQNZlfKA;
				}
			}

			private sealed class CHdLBGaFvZsApwPgwmOopjhtCjFu
			{
				public ControllerMap_Editor lPMziJCFRnvinaKeGNwjXRpuzYwf;

				public Predicate<nujuvJulgYzNRYueyruWPgnaqlHP> DUODApCmKnGbkoRSToHPWRMGutePA;

				public Predicate<nujuvJulgYzNRYueyruWPgnaqlHP> rMdeULixBNylBOVYkdHJmDOBfwQBb;

				internal bool uqGCaVhICxHschZufYDscvyFfFEqD(nujuvJulgYzNRYueyruWPgnaqlHP P_0)
				{
					return P_0.okeWwwmvgkmEMOtIulLmImzRWNpc == lPMziJCFRnvinaKeGNwjXRpuzYwf.categoryId;
				}

				internal bool AwzojJkcAaSiquoWKhdCIjYuvxzf(nujuvJulgYzNRYueyruWPgnaqlHP P_0)
				{
					return P_0.okeWwwmvgkmEMOtIulLmImzRWNpc == lPMziJCFRnvinaKeGNwjXRpuzYwf.layoutId;
				}
			}

			private sealed class cRtDMzACuSlLaBgKOcNBTSuelHJbb
			{
				public koiccHreeSzhhtmqpAfbekazGvOAb<ControllerMap_Editor> RRKYNJnhsfaXVVTNYbIaGqeAMQLb;

				public ControllerMap_Editor KtLcYaFGtXgWVWKJtALgxYQNZlfKA;

				internal bool wkdrHuqAZrqAVTupVOqLBLnxVxlv(nujuvJulgYzNRYueyruWPgnaqlHP P_0)
				{
					return P_0.dvPsYKgTmQEzpmDbxLcTJoNaDxtw(RRKYNJnhsfaXVVTNYbIaGqeAMQLb.GpmLUZnatheysQmKSGyhdsjsLXusA) == KtLcYaFGtXgWVWKJtALgxYQNZlfKA.categoryId;
				}

				internal bool mmCFKxdaEGoOKPTnEpOJiPOHguvm(nujuvJulgYzNRYueyruWPgnaqlHP P_0)
				{
					return P_0.dvPsYKgTmQEzpmDbxLcTJoNaDxtw(RRKYNJnhsfaXVVTNYbIaGqeAMQLb.GpmLUZnatheysQmKSGyhdsjsLXusA) == KtLcYaFGtXgWVWKJtALgxYQNZlfKA.layoutId;
				}
			}

			private sealed class ZdrecckMbGKbgbisWsqinzwIsieo
			{
				public ActionElementMap ZdyrardeHsUVqbcZHsfgfyoKKFfh;

				public cRtDMzACuSlLaBgKOcNBTSuelHJbb NLseWTgJRBhPPcEGicMZhpxVigKdA;

				internal bool dxdGQJHIcZlXiBvKLFPUxvXeTyjNA(nujuvJulgYzNRYueyruWPgnaqlHP P_0)
				{
					return P_0.dvPsYKgTmQEzpmDbxLcTJoNaDxtw(NLseWTgJRBhPPcEGicMZhpxVigKdA.RRKYNJnhsfaXVVTNYbIaGqeAMQLb.GpmLUZnatheysQmKSGyhdsjsLXusA) == ZdyrardeHsUVqbcZHsfgfyoKKFfh._actionId;
				}
			}

			private sealed class VNzISyiWRzGLyqtCYDrqLtfiVvfG
			{
				public List<nujuvJulgYzNRYueyruWPgnaqlHP> dNrwwVgnnfEEnwYQWpUyLcOkUKsS;

				public IXmcEKmCGRPEAZqGNAGkHqLXgNgA EacguhfpbEwuRJxWuDagiZnfZeFqA;

				internal int uwFRnXPQIgeUEsNEmClZTtssnJLl(ControllerMap_Editor P_0, IList<ControllerMap_Editor> P_1)
				{
					rtHERisNbGwyYbUcSbhZTBcbFuJDA rtHERisNbGwyYbUcSbhZTBcbFuJDA2 = new rtHERisNbGwyYbUcSbhZTBcbFuJDA();
					rtHERisNbGwyYbUcSbhZTBcbFuJDA2.lPMziJCFRnvinaKeGNwjXRpuzYwf = P_0;
					for (int i = 0; i < P_1.Count; i++)
					{
						nujuvJulgYzNRYueyruWPgnaqlHP nujuvJulgYzNRYueyruWPgnaqlHP2 = EacguhfpbEwuRJxWuDagiZnfZeFqA.nKZygYNGQLtpMTjCCgCOFpgVORoI.Find(rtHERisNbGwyYbUcSbhZTBcbFuJDA2.FmeRApAJZYSyfBpTxXdQyfElbnur);
						nujuvJulgYzNRYueyruWPgnaqlHP nujuvJulgYzNRYueyruWPgnaqlHP3 = dNrwwVgnnfEEnwYQWpUyLcOkUKsS.Find(rtHERisNbGwyYbUcSbhZTBcbFuJDA2.hAaJkNelmgYgZWTGDfnHnHpcEFMab);
						if (rtHERisNbGwyYbUcSbhZTBcbFuJDA2.lPMziJCFRnvinaKeGNwjXRpuzYwf.hardwareGuid == P_1[i].hardwareGuid && nujuvJulgYzNRYueyruWPgnaqlHP2 != null && nujuvJulgYzNRYueyruWPgnaqlHP2.NmvBXsDtIAXqBCeQpumPWRwoEYyoA == P_1[i].categoryId && nujuvJulgYzNRYueyruWPgnaqlHP3 != null && nujuvJulgYzNRYueyruWPgnaqlHP3.NmvBXsDtIAXqBCeQpumPWRwoEYyoA == P_1[i].layoutId)
						{
							return i;
						}
					}
					return -1;
				}

				internal ControllerMap_Editor QODdGzfhRxlQLaHIfIqPTBJFOtRd(koiccHreeSzhhtmqpAfbekazGvOAb<ControllerMap_Editor> P_0)
				{
					cNYUAdmpdgyRBxQAeKjqRDgLfQYA cNYUAdmpdgyRBxQAeKjqRDgLfQYA2 = new cNYUAdmpdgyRBxQAeKjqRDgLfQYA();
					cNYUAdmpdgyRBxQAeKjqRDgLfQYA2.RRKYNJnhsfaXVVTNYbIaGqeAMQLb = P_0;
					cNYUAdmpdgyRBxQAeKjqRDgLfQYA2.KtLcYaFGtXgWVWKJtALgxYQNZlfKA = JsonTools.Clone(cNYUAdmpdgyRBxQAeKjqRDgLfQYA2.RRKYNJnhsfaXVVTNYbIaGqeAMQLb.UNvoUrngiTAgvWKCtPziXkqUPtZk);
					nujuvJulgYzNRYueyruWPgnaqlHP nujuvJulgYzNRYueyruWPgnaqlHP2 = EacguhfpbEwuRJxWuDagiZnfZeFqA.nKZygYNGQLtpMTjCCgCOFpgVORoI.Find(cNYUAdmpdgyRBxQAeKjqRDgLfQYA2.txFcLiawfkCjPHdrdSQUiLeaKzyOA);
					nujuvJulgYzNRYueyruWPgnaqlHP nujuvJulgYzNRYueyruWPgnaqlHP3 = dNrwwVgnnfEEnwYQWpUyLcOkUKsS.Find(cNYUAdmpdgyRBxQAeKjqRDgLfQYA2.kMVdBQQujpREbUvdNJOgWpbTENvT);
					cNYUAdmpdgyRBxQAeKjqRDgLfQYA2.KtLcYaFGtXgWVWKJtALgxYQNZlfKA.categoryId = nujuvJulgYzNRYueyruWPgnaqlHP2?.NmvBXsDtIAXqBCeQpumPWRwoEYyoA ?? (-1);
					cNYUAdmpdgyRBxQAeKjqRDgLfQYA2.KtLcYaFGtXgWVWKJtALgxYQNZlfKA.layoutId = nujuvJulgYzNRYueyruWPgnaqlHP3?.NmvBXsDtIAXqBCeQpumPWRwoEYyoA ?? (-1);
					for (int i = 0; i < cNYUAdmpdgyRBxQAeKjqRDgLfQYA2.KtLcYaFGtXgWVWKJtALgxYQNZlfKA.actionElementMaps.Count; i++)
					{
						EIQGGZcRXFuaZpdFmWKlMKKjQCsQA eIQGGZcRXFuaZpdFmWKlMKKjQCsQA = new EIQGGZcRXFuaZpdFmWKlMKKjQCsQA();
						eIQGGZcRXFuaZpdFmWKlMKKjQCsQA.oVyWAxkWozsgalzWdabaYmfwPMzC = cNYUAdmpdgyRBxQAeKjqRDgLfQYA2;
						eIQGGZcRXFuaZpdFmWKlMKKjQCsQA.ZdyrardeHsUVqbcZHsfgfyoKKFfh = eIQGGZcRXFuaZpdFmWKlMKKjQCsQA.oVyWAxkWozsgalzWdabaYmfwPMzC.KtLcYaFGtXgWVWKJtALgxYQNZlfKA.actionElementMaps[i];
						nujuvJulgYzNRYueyruWPgnaqlHP nujuvJulgYzNRYueyruWPgnaqlHP4 = EacguhfpbEwuRJxWuDagiZnfZeFqA.ATTEKddHkreaqveCPUFoCfIJHcBEb.Find(eIQGGZcRXFuaZpdFmWKlMKKjQCsQA.ArkZHLOhoiemagYgMtwFOPryICdK);
						eIQGGZcRXFuaZpdFmWKlMKKjQCsQA.ZdyrardeHsUVqbcZHsfgfyoKKFfh._actionId = nujuvJulgYzNRYueyruWPgnaqlHP4?.NmvBXsDtIAXqBCeQpumPWRwoEYyoA ?? (-1);
						eIQGGZcRXFuaZpdFmWKlMKKjQCsQA.ZdyrardeHsUVqbcZHsfgfyoKKFfh._actionCategoryId = ((EacguhfpbEwuRJxWuDagiZnfZeFqA.HDkgCuGOBNmSZoCUejZdOdigNGDNA.GetActionById(eIQGGZcRXFuaZpdFmWKlMKKjQCsQA.ZdyrardeHsUVqbcZHsfgfyoKKFfh._actionId) != null) ? EacguhfpbEwuRJxWuDagiZnfZeFqA.HDkgCuGOBNmSZoCUejZdOdigNGDNA.GetActionById(eIQGGZcRXFuaZpdFmWKlMKKjQCsQA.ZdyrardeHsUVqbcZHsfgfyoKKFfh._actionId).categoryId : 0);
					}
					ControllerMap_Editor controllerMap_Editor;
					if (cNYUAdmpdgyRBxQAeKjqRDgLfQYA2.RRKYNJnhsfaXVVTNYbIaGqeAMQLb.idYOzFFtwFXzmBEeMDPCXTFwffBf)
					{
						controllerMap_Editor = cNYUAdmpdgyRBxQAeKjqRDgLfQYA2.RRKYNJnhsfaXVVTNYbIaGqeAMQLb.ZUyOTflBkjftSKlGjdzKIQDMvsbu;
						ControllerMap_Editor controllerMap_Editor2 = JsonTools.Clone(cNYUAdmpdgyRBxQAeKjqRDgLfQYA2.KtLcYaFGtXgWVWKJtALgxYQNZlfKA);
						controllerMap_Editor2.actionElementMaps.Clear();
						Func<ActionElementMap, IList<ActionElementMap>, int> func = rEvABygUkOfYEkBliaAlBIpFisUVB._003C_003E9.QDLvRBRtFKukZbgExEgEiKGIBraFA;
						CiOTsWPWICCngyVEtwGSSYljjUcd(controllerMap_Editor.actionElementMaps, cNYUAdmpdgyRBxQAeKjqRDgLfQYA2.KtLcYaFGtXgWVWKJtALgxYQNZlfKA.actionElementMaps, controllerMap_Editor2.actionElementMaps, func);
						cNYUAdmpdgyRBxQAeKjqRDgLfQYA2.KtLcYaFGtXgWVWKJtALgxYQNZlfKA = controllerMap_Editor2;
					}
					else
					{
						EacguhfpbEwuRJxWuDagiZnfZeFqA.HDkgCuGOBNmSZoCUejZdOdigNGDNA.CreateJoystickMap(cNYUAdmpdgyRBxQAeKjqRDgLfQYA2.KtLcYaFGtXgWVWKJtALgxYQNZlfKA.categoryId, cNYUAdmpdgyRBxQAeKjqRDgLfQYA2.KtLcYaFGtXgWVWKJtALgxYQNZlfKA.hardwareGuid, cNYUAdmpdgyRBxQAeKjqRDgLfQYA2.KtLcYaFGtXgWVWKJtALgxYQNZlfKA.layoutId);
						controllerMap_Editor = cNYUAdmpdgyRBxQAeKjqRDgLfQYA2.RRKYNJnhsfaXVVTNYbIaGqeAMQLb.zRnCKWcMztlPqYjfLLdxSVnTlRAdA[cNYUAdmpdgyRBxQAeKjqRDgLfQYA2.RRKYNJnhsfaXVVTNYbIaGqeAMQLb.zRnCKWcMztlPqYjfLLdxSVnTlRAdA.Count - 1];
					}
					cNYUAdmpdgyRBxQAeKjqRDgLfQYA2.KtLcYaFGtXgWVWKJtALgxYQNZlfKA.id = controllerMap_Editor.id;
					int index = cNYUAdmpdgyRBxQAeKjqRDgLfQYA2.RRKYNJnhsfaXVVTNYbIaGqeAMQLb.zRnCKWcMztlPqYjfLLdxSVnTlRAdA.IndexOf(controllerMap_Editor);
					cNYUAdmpdgyRBxQAeKjqRDgLfQYA2.RRKYNJnhsfaXVVTNYbIaGqeAMQLb.zRnCKWcMztlPqYjfLLdxSVnTlRAdA[index] = cNYUAdmpdgyRBxQAeKjqRDgLfQYA2.KtLcYaFGtXgWVWKJtALgxYQNZlfKA;
					return cNYUAdmpdgyRBxQAeKjqRDgLfQYA2.KtLcYaFGtXgWVWKJtALgxYQNZlfKA;
				}
			}

			private sealed class rtHERisNbGwyYbUcSbhZTBcbFuJDA
			{
				public ControllerMap_Editor lPMziJCFRnvinaKeGNwjXRpuzYwf;

				public Predicate<nujuvJulgYzNRYueyruWPgnaqlHP> NbOgotuKeboNrWLoIzhJaaZFCoNO;

				public Predicate<nujuvJulgYzNRYueyruWPgnaqlHP> uCYgPQwVuesAwbOtwEhrbJAaJghnA;

				internal bool FmeRApAJZYSyfBpTxXdQyfElbnur(nujuvJulgYzNRYueyruWPgnaqlHP P_0)
				{
					return P_0.okeWwwmvgkmEMOtIulLmImzRWNpc == lPMziJCFRnvinaKeGNwjXRpuzYwf.categoryId;
				}

				internal bool hAaJkNelmgYgZWTGDfnHnHpcEFMab(nujuvJulgYzNRYueyruWPgnaqlHP P_0)
				{
					return P_0.okeWwwmvgkmEMOtIulLmImzRWNpc == lPMziJCFRnvinaKeGNwjXRpuzYwf.layoutId;
				}
			}

			private sealed class cNYUAdmpdgyRBxQAeKjqRDgLfQYA
			{
				public koiccHreeSzhhtmqpAfbekazGvOAb<ControllerMap_Editor> RRKYNJnhsfaXVVTNYbIaGqeAMQLb;

				public ControllerMap_Editor KtLcYaFGtXgWVWKJtALgxYQNZlfKA;

				internal bool txFcLiawfkCjPHdrdSQUiLeaKzyOA(nujuvJulgYzNRYueyruWPgnaqlHP P_0)
				{
					return P_0.dvPsYKgTmQEzpmDbxLcTJoNaDxtw(RRKYNJnhsfaXVVTNYbIaGqeAMQLb.GpmLUZnatheysQmKSGyhdsjsLXusA) == KtLcYaFGtXgWVWKJtALgxYQNZlfKA.categoryId;
				}

				internal bool kMVdBQQujpREbUvdNJOgWpbTENvT(nujuvJulgYzNRYueyruWPgnaqlHP P_0)
				{
					return P_0.dvPsYKgTmQEzpmDbxLcTJoNaDxtw(RRKYNJnhsfaXVVTNYbIaGqeAMQLb.GpmLUZnatheysQmKSGyhdsjsLXusA) == KtLcYaFGtXgWVWKJtALgxYQNZlfKA.layoutId;
				}
			}

			private sealed class EIQGGZcRXFuaZpdFmWKlMKKjQCsQA
			{
				public ActionElementMap ZdyrardeHsUVqbcZHsfgfyoKKFfh;

				public cNYUAdmpdgyRBxQAeKjqRDgLfQYA oVyWAxkWozsgalzWdabaYmfwPMzC;

				internal bool ArkZHLOhoiemagYgMtwFOPryICdK(nujuvJulgYzNRYueyruWPgnaqlHP P_0)
				{
					return P_0.dvPsYKgTmQEzpmDbxLcTJoNaDxtw(oVyWAxkWozsgalzWdabaYmfwPMzC.RRKYNJnhsfaXVVTNYbIaGqeAMQLb.GpmLUZnatheysQmKSGyhdsjsLXusA) == ZdyrardeHsUVqbcZHsfgfyoKKFfh._actionId;
				}
			}

			private sealed class tUuipCEqOyHrMZiSdMxMuEyPdfKP
			{
				public List<nujuvJulgYzNRYueyruWPgnaqlHP> dNrwwVgnnfEEnwYQWpUyLcOkUKsS;

				public IXmcEKmCGRPEAZqGNAGkHqLXgNgA GTyExVTBYJavQJkfGWnrbPDkYeTC;

				internal int pFmvEwHAIjgLMAhBLbLDuoLBTotO(ControllerMap_Editor P_0, IList<ControllerMap_Editor> P_1)
				{
					DlQyrTZrnZUapIBfBhjvwnnhzNS dlQyrTZrnZUapIBfBhjvwnnhzNS = new DlQyrTZrnZUapIBfBhjvwnnhzNS();
					dlQyrTZrnZUapIBfBhjvwnnhzNS.lPMziJCFRnvinaKeGNwjXRpuzYwf = P_0;
					for (int i = 0; i < P_1.Count; i++)
					{
						nujuvJulgYzNRYueyruWPgnaqlHP nujuvJulgYzNRYueyruWPgnaqlHP2 = GTyExVTBYJavQJkfGWnrbPDkYeTC.UzvGjfkjNdAcVEwwAgeRhisBAbmgA.Find(dlQyrTZrnZUapIBfBhjvwnnhzNS.scfTWdktVDTnYotzvExymDrbooHE);
						nujuvJulgYzNRYueyruWPgnaqlHP nujuvJulgYzNRYueyruWPgnaqlHP3 = GTyExVTBYJavQJkfGWnrbPDkYeTC.nKZygYNGQLtpMTjCCgCOFpgVORoI.Find(dlQyrTZrnZUapIBfBhjvwnnhzNS.oZylfTXOeLcCzCVwcAsGRANbjUqo);
						nujuvJulgYzNRYueyruWPgnaqlHP nujuvJulgYzNRYueyruWPgnaqlHP4 = dNrwwVgnnfEEnwYQWpUyLcOkUKsS.Find(dlQyrTZrnZUapIBfBhjvwnnhzNS.XSyJZIZtZUaNOpTkSgeaOSwUHqOx);
						if (nujuvJulgYzNRYueyruWPgnaqlHP2 != null && nujuvJulgYzNRYueyruWPgnaqlHP2.NmvBXsDtIAXqBCeQpumPWRwoEYyoA == P_1[i].customControllerUid && nujuvJulgYzNRYueyruWPgnaqlHP3 != null && nujuvJulgYzNRYueyruWPgnaqlHP3.NmvBXsDtIAXqBCeQpumPWRwoEYyoA == P_1[i].categoryId && nujuvJulgYzNRYueyruWPgnaqlHP4 != null && nujuvJulgYzNRYueyruWPgnaqlHP4.NmvBXsDtIAXqBCeQpumPWRwoEYyoA == P_1[i].layoutId)
						{
							return i;
						}
					}
					return -1;
				}

				internal ControllerMap_Editor UMdTRdkWHNwhnmHAZVmfsbcXFziDA(koiccHreeSzhhtmqpAfbekazGvOAb<ControllerMap_Editor> P_0)
				{
					xiajFiGaIAKqvjcczwuTdokoThKe xiajFiGaIAKqvjcczwuTdokoThKe2 = new xiajFiGaIAKqvjcczwuTdokoThKe();
					xiajFiGaIAKqvjcczwuTdokoThKe2.RRKYNJnhsfaXVVTNYbIaGqeAMQLb = P_0;
					xiajFiGaIAKqvjcczwuTdokoThKe2.KtLcYaFGtXgWVWKJtALgxYQNZlfKA = JsonTools.Clone(xiajFiGaIAKqvjcczwuTdokoThKe2.RRKYNJnhsfaXVVTNYbIaGqeAMQLb.UNvoUrngiTAgvWKCtPziXkqUPtZk);
					nujuvJulgYzNRYueyruWPgnaqlHP nujuvJulgYzNRYueyruWPgnaqlHP2 = GTyExVTBYJavQJkfGWnrbPDkYeTC.UzvGjfkjNdAcVEwwAgeRhisBAbmgA.Find(xiajFiGaIAKqvjcczwuTdokoThKe2.UITlwbcxIpjynvafrfPqHFxDBIJY);
					nujuvJulgYzNRYueyruWPgnaqlHP nujuvJulgYzNRYueyruWPgnaqlHP3 = GTyExVTBYJavQJkfGWnrbPDkYeTC.nKZygYNGQLtpMTjCCgCOFpgVORoI.Find(xiajFiGaIAKqvjcczwuTdokoThKe2.woeKOrKqZiTIolciSOTzupFvkUwD);
					nujuvJulgYzNRYueyruWPgnaqlHP nujuvJulgYzNRYueyruWPgnaqlHP4 = dNrwwVgnnfEEnwYQWpUyLcOkUKsS.Find(xiajFiGaIAKqvjcczwuTdokoThKe2.YLXNLXkCAlkHwYqNFjjhijWVeOxqA);
					xiajFiGaIAKqvjcczwuTdokoThKe2.KtLcYaFGtXgWVWKJtALgxYQNZlfKA.customControllerUid = nujuvJulgYzNRYueyruWPgnaqlHP2?.NmvBXsDtIAXqBCeQpumPWRwoEYyoA ?? (-1);
					xiajFiGaIAKqvjcczwuTdokoThKe2.KtLcYaFGtXgWVWKJtALgxYQNZlfKA.categoryId = nujuvJulgYzNRYueyruWPgnaqlHP3?.NmvBXsDtIAXqBCeQpumPWRwoEYyoA ?? (-1);
					xiajFiGaIAKqvjcczwuTdokoThKe2.KtLcYaFGtXgWVWKJtALgxYQNZlfKA.layoutId = nujuvJulgYzNRYueyruWPgnaqlHP4?.NmvBXsDtIAXqBCeQpumPWRwoEYyoA ?? (-1);
					for (int i = 0; i < xiajFiGaIAKqvjcczwuTdokoThKe2.KtLcYaFGtXgWVWKJtALgxYQNZlfKA.actionElementMaps.Count; i++)
					{
						mAAJHnFFxUpkfoZtQLrTDoFZfBaqA mAAJHnFFxUpkfoZtQLrTDoFZfBaqA2 = new mAAJHnFFxUpkfoZtQLrTDoFZfBaqA();
						mAAJHnFFxUpkfoZtQLrTDoFZfBaqA2.yNJIkaQmtTmJCHzbmkvuAmBWzTzN = xiajFiGaIAKqvjcczwuTdokoThKe2;
						mAAJHnFFxUpkfoZtQLrTDoFZfBaqA2.ZdyrardeHsUVqbcZHsfgfyoKKFfh = mAAJHnFFxUpkfoZtQLrTDoFZfBaqA2.yNJIkaQmtTmJCHzbmkvuAmBWzTzN.KtLcYaFGtXgWVWKJtALgxYQNZlfKA.actionElementMaps[i];
						nujuvJulgYzNRYueyruWPgnaqlHP nujuvJulgYzNRYueyruWPgnaqlHP5 = GTyExVTBYJavQJkfGWnrbPDkYeTC.ATTEKddHkreaqveCPUFoCfIJHcBEb.Find(mAAJHnFFxUpkfoZtQLrTDoFZfBaqA2.icwBKyEkyFDYnInSOcGKiDJOhwNiA);
						mAAJHnFFxUpkfoZtQLrTDoFZfBaqA2.ZdyrardeHsUVqbcZHsfgfyoKKFfh._actionId = nujuvJulgYzNRYueyruWPgnaqlHP5?.NmvBXsDtIAXqBCeQpumPWRwoEYyoA ?? (-1);
						mAAJHnFFxUpkfoZtQLrTDoFZfBaqA2.ZdyrardeHsUVqbcZHsfgfyoKKFfh._actionCategoryId = ((GTyExVTBYJavQJkfGWnrbPDkYeTC.HDkgCuGOBNmSZoCUejZdOdigNGDNA.GetActionById(mAAJHnFFxUpkfoZtQLrTDoFZfBaqA2.ZdyrardeHsUVqbcZHsfgfyoKKFfh._actionId) != null) ? GTyExVTBYJavQJkfGWnrbPDkYeTC.HDkgCuGOBNmSZoCUejZdOdigNGDNA.GetActionById(mAAJHnFFxUpkfoZtQLrTDoFZfBaqA2.ZdyrardeHsUVqbcZHsfgfyoKKFfh._actionId).categoryId : 0);
					}
					ControllerMap_Editor controllerMap_Editor;
					if (xiajFiGaIAKqvjcczwuTdokoThKe2.RRKYNJnhsfaXVVTNYbIaGqeAMQLb.idYOzFFtwFXzmBEeMDPCXTFwffBf)
					{
						controllerMap_Editor = xiajFiGaIAKqvjcczwuTdokoThKe2.RRKYNJnhsfaXVVTNYbIaGqeAMQLb.ZUyOTflBkjftSKlGjdzKIQDMvsbu;
						ControllerMap_Editor controllerMap_Editor2 = JsonTools.Clone(xiajFiGaIAKqvjcczwuTdokoThKe2.KtLcYaFGtXgWVWKJtALgxYQNZlfKA);
						controllerMap_Editor2.actionElementMaps.Clear();
						Func<ActionElementMap, IList<ActionElementMap>, int> func = rEvABygUkOfYEkBliaAlBIpFisUVB._003C_003E9.pkuKURmfsKYFRWFHrjFcxDVnbGChA;
						CiOTsWPWICCngyVEtwGSSYljjUcd(controllerMap_Editor.actionElementMaps, xiajFiGaIAKqvjcczwuTdokoThKe2.KtLcYaFGtXgWVWKJtALgxYQNZlfKA.actionElementMaps, controllerMap_Editor2.actionElementMaps, func);
						xiajFiGaIAKqvjcczwuTdokoThKe2.KtLcYaFGtXgWVWKJtALgxYQNZlfKA = controllerMap_Editor2;
					}
					else
					{
						GTyExVTBYJavQJkfGWnrbPDkYeTC.HDkgCuGOBNmSZoCUejZdOdigNGDNA.CreateCustomControllerMap(xiajFiGaIAKqvjcczwuTdokoThKe2.KtLcYaFGtXgWVWKJtALgxYQNZlfKA.categoryId, xiajFiGaIAKqvjcczwuTdokoThKe2.KtLcYaFGtXgWVWKJtALgxYQNZlfKA.customControllerUid, xiajFiGaIAKqvjcczwuTdokoThKe2.KtLcYaFGtXgWVWKJtALgxYQNZlfKA.layoutId);
						controllerMap_Editor = xiajFiGaIAKqvjcczwuTdokoThKe2.RRKYNJnhsfaXVVTNYbIaGqeAMQLb.zRnCKWcMztlPqYjfLLdxSVnTlRAdA[xiajFiGaIAKqvjcczwuTdokoThKe2.RRKYNJnhsfaXVVTNYbIaGqeAMQLb.zRnCKWcMztlPqYjfLLdxSVnTlRAdA.Count - 1];
					}
					xiajFiGaIAKqvjcczwuTdokoThKe2.KtLcYaFGtXgWVWKJtALgxYQNZlfKA.id = controllerMap_Editor.id;
					int index = xiajFiGaIAKqvjcczwuTdokoThKe2.RRKYNJnhsfaXVVTNYbIaGqeAMQLb.zRnCKWcMztlPqYjfLLdxSVnTlRAdA.IndexOf(controllerMap_Editor);
					xiajFiGaIAKqvjcczwuTdokoThKe2.RRKYNJnhsfaXVVTNYbIaGqeAMQLb.zRnCKWcMztlPqYjfLLdxSVnTlRAdA[index] = xiajFiGaIAKqvjcczwuTdokoThKe2.KtLcYaFGtXgWVWKJtALgxYQNZlfKA;
					return xiajFiGaIAKqvjcczwuTdokoThKe2.KtLcYaFGtXgWVWKJtALgxYQNZlfKA;
				}
			}

			private sealed class YEGQvnVXBafOTtpqfOXDbBHMzxfA
			{
				public int okeWwwmvgkmEMOtIulLmImzRWNpc;

				internal bool yHdMyHYegIDsFLfOmBdSEhFyORQzA(nujuvJulgYzNRYueyruWPgnaqlHP P_0)
				{
					return P_0.okeWwwmvgkmEMOtIulLmImzRWNpc == okeWwwmvgkmEMOtIulLmImzRWNpc;
				}
			}

			private sealed class DlQyrTZrnZUapIBfBhjvwnnhzNS
			{
				public ControllerMap_Editor lPMziJCFRnvinaKeGNwjXRpuzYwf;

				public Predicate<nujuvJulgYzNRYueyruWPgnaqlHP> ZmfgzbHZfeoUpBYQstvsQBqMykMz;

				public Predicate<nujuvJulgYzNRYueyruWPgnaqlHP> jfVIaHSsMZtaKSMGyAewGTEhUfzfA;

				public Predicate<nujuvJulgYzNRYueyruWPgnaqlHP> jDcqpcofHVwyaHzrIKIaaaXCYMkt;

				internal bool scfTWdktVDTnYotzvExymDrbooHE(nujuvJulgYzNRYueyruWPgnaqlHP P_0)
				{
					return P_0.okeWwwmvgkmEMOtIulLmImzRWNpc == lPMziJCFRnvinaKeGNwjXRpuzYwf.customControllerUid;
				}

				internal bool oZylfTXOeLcCzCVwcAsGRANbjUqo(nujuvJulgYzNRYueyruWPgnaqlHP P_0)
				{
					return P_0.okeWwwmvgkmEMOtIulLmImzRWNpc == lPMziJCFRnvinaKeGNwjXRpuzYwf.categoryId;
				}

				internal bool XSyJZIZtZUaNOpTkSgeaOSwUHqOx(nujuvJulgYzNRYueyruWPgnaqlHP P_0)
				{
					return P_0.okeWwwmvgkmEMOtIulLmImzRWNpc == lPMziJCFRnvinaKeGNwjXRpuzYwf.layoutId;
				}
			}

			private sealed class xiajFiGaIAKqvjcczwuTdokoThKe
			{
				public koiccHreeSzhhtmqpAfbekazGvOAb<ControllerMap_Editor> RRKYNJnhsfaXVVTNYbIaGqeAMQLb;

				public ControllerMap_Editor KtLcYaFGtXgWVWKJtALgxYQNZlfKA;

				internal bool UITlwbcxIpjynvafrfPqHFxDBIJY(nujuvJulgYzNRYueyruWPgnaqlHP P_0)
				{
					return P_0.dvPsYKgTmQEzpmDbxLcTJoNaDxtw(RRKYNJnhsfaXVVTNYbIaGqeAMQLb.GpmLUZnatheysQmKSGyhdsjsLXusA) == KtLcYaFGtXgWVWKJtALgxYQNZlfKA.customControllerUid;
				}

				internal bool woeKOrKqZiTIolciSOTzupFvkUwD(nujuvJulgYzNRYueyruWPgnaqlHP P_0)
				{
					return P_0.dvPsYKgTmQEzpmDbxLcTJoNaDxtw(RRKYNJnhsfaXVVTNYbIaGqeAMQLb.GpmLUZnatheysQmKSGyhdsjsLXusA) == KtLcYaFGtXgWVWKJtALgxYQNZlfKA.categoryId;
				}

				internal bool YLXNLXkCAlkHwYqNFjjhijWVeOxqA(nujuvJulgYzNRYueyruWPgnaqlHP P_0)
				{
					return P_0.dvPsYKgTmQEzpmDbxLcTJoNaDxtw(RRKYNJnhsfaXVVTNYbIaGqeAMQLb.GpmLUZnatheysQmKSGyhdsjsLXusA) == KtLcYaFGtXgWVWKJtALgxYQNZlfKA.layoutId;
				}
			}

			private sealed class mAAJHnFFxUpkfoZtQLrTDoFZfBaqA
			{
				public ActionElementMap ZdyrardeHsUVqbcZHsfgfyoKKFfh;

				public xiajFiGaIAKqvjcczwuTdokoThKe yNJIkaQmtTmJCHzbmkvuAmBWzTzN;

				internal bool icwBKyEkyFDYnInSOcGKiDJOhwNiA(nujuvJulgYzNRYueyruWPgnaqlHP P_0)
				{
					return P_0.dvPsYKgTmQEzpmDbxLcTJoNaDxtw(yNJIkaQmtTmJCHzbmkvuAmBWzTzN.RRKYNJnhsfaXVVTNYbIaGqeAMQLb.GpmLUZnatheysQmKSGyhdsjsLXusA) == ZdyrardeHsUVqbcZHsfgfyoKKFfh._actionId;
				}
			}

			private sealed class bMdGGcOBzYLlMEMrAsYyglzkIMoF
			{
				public koiccHreeSzhhtmqpAfbekazGvOAb<ControllerMapLayoutManager_RuleSet_Editor> RRKYNJnhsfaXVVTNYbIaGqeAMQLb;
			}

			private sealed class XZNZoBICPHqcWoiOvRZiNSokcuWq
			{
				public int xUdfZqFEnYagGOnOGpNjcAwEvfyBA;

				public bMdGGcOBzYLlMEMrAsYyglzkIMoF voFPLgLgADgMZJodHztIjhUlouiu;

				internal bool CSkFkGiACoCwhlnPEHOPhUJgEozEA(nujuvJulgYzNRYueyruWPgnaqlHP P_0)
				{
					return P_0.dvPsYKgTmQEzpmDbxLcTJoNaDxtw(voFPLgLgADgMZJodHztIjhUlouiu.RRKYNJnhsfaXVVTNYbIaGqeAMQLb.GpmLUZnatheysQmKSGyhdsjsLXusA) == xUdfZqFEnYagGOnOGpNjcAwEvfyBA;
				}
			}

			private sealed class sjWQylShnInRKAGhpVZdMyViQiKh
			{
				public int xUdfZqFEnYagGOnOGpNjcAwEvfyBA;

				public bMdGGcOBzYLlMEMrAsYyglzkIMoF vAlvmBHCkpDaSiHXzsxuLeDzJcvDA;

				internal bool JnRADBCuuQjYOPDrWipeqdWgrsxc(nujuvJulgYzNRYueyruWPgnaqlHP P_0)
				{
					return P_0.dvPsYKgTmQEzpmDbxLcTJoNaDxtw(vAlvmBHCkpDaSiHXzsxuLeDzJcvDA.RRKYNJnhsfaXVVTNYbIaGqeAMQLb.GpmLUZnatheysQmKSGyhdsjsLXusA) == xUdfZqFEnYagGOnOGpNjcAwEvfyBA;
				}
			}

			private sealed class MPfEylRDjoIOugAgndjpkZHjJUnDA
			{
				public int xUdfZqFEnYagGOnOGpNjcAwEvfyBA;

				public bMdGGcOBzYLlMEMrAsYyglzkIMoF GQITxFqCRYYMQLZJgPsCjYvoizzV;

				internal bool oupSWVlEMClXdZCPvzegYBEotzDc(nujuvJulgYzNRYueyruWPgnaqlHP P_0)
				{
					return P_0.dvPsYKgTmQEzpmDbxLcTJoNaDxtw(GQITxFqCRYYMQLZJgPsCjYvoizzV.RRKYNJnhsfaXVVTNYbIaGqeAMQLb.GpmLUZnatheysQmKSGyhdsjsLXusA) == xUdfZqFEnYagGOnOGpNjcAwEvfyBA;
				}
			}

			private sealed class NjmTWflIvcoWwNjLSpbhnjchTaOE
			{
				public koiccHreeSzhhtmqpAfbekazGvOAb<ControllerMapEnabler_RuleSet_Editor> RRKYNJnhsfaXVVTNYbIaGqeAMQLb;
			}

			private sealed class paOYRDHVDgzjgxFkaEZtAGrnJxGd
			{
				public int xUdfZqFEnYagGOnOGpNjcAwEvfyBA;

				public NjmTWflIvcoWwNjLSpbhnjchTaOE GTOUyYQaGDUneWDZKDKuaXXfqahI;

				internal bool AOetRJqaLYoOiMNDejUDseZsXzCS(nujuvJulgYzNRYueyruWPgnaqlHP P_0)
				{
					return P_0.dvPsYKgTmQEzpmDbxLcTJoNaDxtw(GTOUyYQaGDUneWDZKDKuaXXfqahI.RRKYNJnhsfaXVVTNYbIaGqeAMQLb.GpmLUZnatheysQmKSGyhdsjsLXusA) == xUdfZqFEnYagGOnOGpNjcAwEvfyBA;
				}
			}

			private sealed class nfXRWZiPwVeOBzTukskSRmCkzkPO<_0001> where _0001 : class
			{
				public Func<_0001, int> vWPmVWuzoNKiwYZUxRLBYizWpHpj;
			}

			private sealed class RajAKAYAvJjmpOJPvFcgUtdyulNP<_0001> where _0001 : class
			{
				public _0001 KtLcYaFGtXgWVWKJtALgxYQNZlfKA;

				public nfXRWZiPwVeOBzTukskSRmCkzkPO<_0001> cRXBQcKBiFKKgieBjgErDNqfQAQfc;

				internal bool DrPdQBNBCgzNDAeCRIfyHcnSboJcb(nujuvJulgYzNRYueyruWPgnaqlHP P_0)
				{
					return P_0.NmvBXsDtIAXqBCeQpumPWRwoEYyoA == cRXBQcKBiFKKgieBjgErDNqfQAQfc.vWPmVWuzoNKiwYZUxRLBYizWpHpj(KtLcYaFGtXgWVWKJtALgxYQNZlfKA);
				}
			}

			public static UserData oGPVXKkaRuOYcNaLJIZFXXgWIxtx(UserData P_0, UserData P_1, bool P_2)
			{
				IXmcEKmCGRPEAZqGNAGkHqLXgNgA xmcEKmCGRPEAZqGNAGkHqLXgNgA = new IXmcEKmCGRPEAZqGNAGkHqLXgNgA();
				if (P_0 == null)
				{
					throw new ArgumentNullException("orig");
				}
				P_0 = JsonTools.Clone(P_0);
				P_1 = ((P_1 != null) ? JsonTools.Clone(P_1) : null);
				xmcEKmCGRPEAZqGNAGkHqLXgNgA.HDkgCuGOBNmSZoCUejZdOdigNGDNA = (P_2 ? P_0 : new UserData(false));
				if (P_1 != null)
				{
					xmcEKmCGRPEAZqGNAGkHqLXgNgA.HDkgCuGOBNmSZoCUejZdOdigNGDNA.configVars = JsonTools.Clone(P_1.configVars);
				}
				else
				{
					xmcEKmCGRPEAZqGNAGkHqLXgNgA.HDkgCuGOBNmSZoCUejZdOdigNGDNA.configVars = JsonTools.Clone(P_0.configVars);
				}
				xmcEKmCGRPEAZqGNAGkHqLXgNgA.KBLwofECeiBpLcJHNBOtMFGqCohXA = new List<nujuvJulgYzNRYueyruWPgnaqlHP>();
				juVnlHGtcxzTEfUNtrFIOyvluHNF("Action Category", P_0.actionCategories, P_1?.actionCategories, xmcEKmCGRPEAZqGNAGkHqLXgNgA.HDkgCuGOBNmSZoCUejZdOdigNGDNA.actionCategories, P_2, xmcEKmCGRPEAZqGNAGkHqLXgNgA.KBLwofECeiBpLcJHNBOtMFGqCohXA, rEvABygUkOfYEkBliaAlBIpFisUVB._003C_003E9.sZmCxsfoLRUUNmmuynpMwZzTEMkeA, rEvABygUkOfYEkBliaAlBIpFisUVB._003C_003E9.byBgfkCOaQjxOtcewYpDcHyVgTCf, rEvABygUkOfYEkBliaAlBIpFisUVB._003C_003E9.CBczItUspJdkLcIrfivyrIEonIiy, xmcEKmCGRPEAZqGNAGkHqLXgNgA.RoagBLmVYnLGflCWtGsqKaAqFgKp);
				xmcEKmCGRPEAZqGNAGkHqLXgNgA.QRoDQvTXtDUJBScICbROkUpBwmHqA = new List<nujuvJulgYzNRYueyruWPgnaqlHP>();
				juVnlHGtcxzTEfUNtrFIOyvluHNF("Input Behavior", P_0.inputBehaviors, P_1?.inputBehaviors, xmcEKmCGRPEAZqGNAGkHqLXgNgA.HDkgCuGOBNmSZoCUejZdOdigNGDNA.inputBehaviors, P_2, xmcEKmCGRPEAZqGNAGkHqLXgNgA.QRoDQvTXtDUJBScICbROkUpBwmHqA, rEvABygUkOfYEkBliaAlBIpFisUVB._003C_003E9.mGSrushEdOESLGvmmSStQZEXETAt, rEvABygUkOfYEkBliaAlBIpFisUVB._003C_003E9.QowlghBXddeWtabdmdUpcdZXDtOOA, rEvABygUkOfYEkBliaAlBIpFisUVB._003C_003E9.WTkhXHPBthKULenHXhoUkFGLHyiQA, xmcEKmCGRPEAZqGNAGkHqLXgNgA.CSQikAhJBJoauvRxetozTfyGiCDP);
				xmcEKmCGRPEAZqGNAGkHqLXgNgA.ATTEKddHkreaqveCPUFoCfIJHcBEb = new List<nujuvJulgYzNRYueyruWPgnaqlHP>();
				juVnlHGtcxzTEfUNtrFIOyvluHNF("Action", P_0.actions, P_1?.actions, xmcEKmCGRPEAZqGNAGkHqLXgNgA.HDkgCuGOBNmSZoCUejZdOdigNGDNA.actions, P_2, xmcEKmCGRPEAZqGNAGkHqLXgNgA.ATTEKddHkreaqveCPUFoCfIJHcBEb, rEvABygUkOfYEkBliaAlBIpFisUVB._003C_003E9.IpcuPsEKkYiyENqyBSMeczKNUjPt, rEvABygUkOfYEkBliaAlBIpFisUVB._003C_003E9.dywmMPpcdtbbhshgDLohDTHfgHnI, rEvABygUkOfYEkBliaAlBIpFisUVB._003C_003E9.WwjOCjHHcNvVqIgoUfuTEBGpeDIlA, xmcEKmCGRPEAZqGNAGkHqLXgNgA.DROJRVXuYvykHVLACTJHqhnTNklb);
				xmcEKmCGRPEAZqGNAGkHqLXgNgA.nKZygYNGQLtpMTjCCgCOFpgVORoI = new List<nujuvJulgYzNRYueyruWPgnaqlHP>();
				ebqPBQNZzNmxoEAcujnfePaDuuXK ebqPBQNZzNmxoEAcujnfePaDuuXK2 = new ebqPBQNZzNmxoEAcujnfePaDuuXK();
				ebqPBQNZzNmxoEAcujnfePaDuuXK2.cRXBQcKBiFKKgieBjgErDNqfQAQfc = xmcEKmCGRPEAZqGNAGkHqLXgNgA;
				ebqPBQNZzNmxoEAcujnfePaDuuXK2.mRJkZfgrfrxTQWaYQkmZeqNVNofj = new List<int>();
				juVnlHGtcxzTEfUNtrFIOyvluHNF("Map Category", P_0.mapCategories, P_1?.mapCategories, ebqPBQNZzNmxoEAcujnfePaDuuXK2.cRXBQcKBiFKKgieBjgErDNqfQAQfc.HDkgCuGOBNmSZoCUejZdOdigNGDNA.mapCategories, P_2, ebqPBQNZzNmxoEAcujnfePaDuuXK2.cRXBQcKBiFKKgieBjgErDNqfQAQfc.nKZygYNGQLtpMTjCCgCOFpgVORoI, rEvABygUkOfYEkBliaAlBIpFisUVB._003C_003E9.XXxGJFjsOVTJWnYyttxDiCmIHWLKA, rEvABygUkOfYEkBliaAlBIpFisUVB._003C_003E9.vIndSvjTDbjRibsSEQxMEUswmmZvb, rEvABygUkOfYEkBliaAlBIpFisUVB._003C_003E9.PwbAKpAStDlnUBqtepidlUDgzIeDc, ebqPBQNZzNmxoEAcujnfePaDuuXK2.LGHqtvOofWNegEjpSgMAfHfqhlMeb);
				for (int i = 0; i < ebqPBQNZzNmxoEAcujnfePaDuuXK2.mRJkZfgrfrxTQWaYQkmZeqNVNofj.Count; i++)
				{
					int index = ebqPBQNZzNmxoEAcujnfePaDuuXK2.mRJkZfgrfrxTQWaYQkmZeqNVNofj[i];
					InputMapCategory inputMapCategory = ebqPBQNZzNmxoEAcujnfePaDuuXK2.cRXBQcKBiFKKgieBjgErDNqfQAQfc.HDkgCuGOBNmSZoCUejZdOdigNGDNA.mapCategories[index];
					for (int j = 0; j < inputMapCategory.IjzdeMdTjOgEWlKUvZcNaMwzeZsDA.Count; j++)
					{
						YEGQvnVXBafOTtpqfOXDbBHMzxfA yEGQvnVXBafOTtpqfOXDbBHMzxfA = new YEGQvnVXBafOTtpqfOXDbBHMzxfA();
						yEGQvnVXBafOTtpqfOXDbBHMzxfA.okeWwwmvgkmEMOtIulLmImzRWNpc = inputMapCategory.IjzdeMdTjOgEWlKUvZcNaMwzeZsDA[j];
						nujuvJulgYzNRYueyruWPgnaqlHP nujuvJulgYzNRYueyruWPgnaqlHP2 = ebqPBQNZzNmxoEAcujnfePaDuuXK2.cRXBQcKBiFKKgieBjgErDNqfQAQfc.nKZygYNGQLtpMTjCCgCOFpgVORoI.Find(yEGQvnVXBafOTtpqfOXDbBHMzxfA.yHdMyHYegIDsFLfOmBdSEhFyORQzA);
						inputMapCategory.IjzdeMdTjOgEWlKUvZcNaMwzeZsDA[j] = nujuvJulgYzNRYueyruWPgnaqlHP2?.NmvBXsDtIAXqBCeQpumPWRwoEYyoA ?? (-1);
					}
				}
				xmcEKmCGRPEAZqGNAGkHqLXgNgA.LxnWPQAbzstdNmqElICPYQIjQnmw = new List<nujuvJulgYzNRYueyruWPgnaqlHP>();
				juVnlHGtcxzTEfUNtrFIOyvluHNF("Keyboard Layout", P_0.keyboardLayouts, P_1?.keyboardLayouts, xmcEKmCGRPEAZqGNAGkHqLXgNgA.HDkgCuGOBNmSZoCUejZdOdigNGDNA.keyboardLayouts, P_2, xmcEKmCGRPEAZqGNAGkHqLXgNgA.LxnWPQAbzstdNmqElICPYQIjQnmw, rEvABygUkOfYEkBliaAlBIpFisUVB._003C_003E9.hsDXQgCYjPgMpysShAYFabahHQodA, rEvABygUkOfYEkBliaAlBIpFisUVB._003C_003E9.ZVSyQnjGclSSONFMpKmQuoALgeNN, rEvABygUkOfYEkBliaAlBIpFisUVB._003C_003E9.rKmCayrwCjMwflBmmRQIPJeVaSgu, xmcEKmCGRPEAZqGNAGkHqLXgNgA.FcIlovTflCfNFfsDcYzEcUBPixHv);
				xmcEKmCGRPEAZqGNAGkHqLXgNgA.BGClHotKwyDHRcCAZxUtRuojYxAeA = new List<nujuvJulgYzNRYueyruWPgnaqlHP>();
				juVnlHGtcxzTEfUNtrFIOyvluHNF("Mouse Layout", P_0.mouseLayouts, P_1?.mouseLayouts, xmcEKmCGRPEAZqGNAGkHqLXgNgA.HDkgCuGOBNmSZoCUejZdOdigNGDNA.mouseLayouts, P_2, xmcEKmCGRPEAZqGNAGkHqLXgNgA.BGClHotKwyDHRcCAZxUtRuojYxAeA, rEvABygUkOfYEkBliaAlBIpFisUVB._003C_003E9.YhTaJwcKNTNcMKBumSVnBSSiMTzeb, rEvABygUkOfYEkBliaAlBIpFisUVB._003C_003E9.LbDLmPOnlvwajPhrmKgYMuQGsQax, rEvABygUkOfYEkBliaAlBIpFisUVB._003C_003E9.rffynhqizVSUpLVpUvSJUPqVuTZw, xmcEKmCGRPEAZqGNAGkHqLXgNgA.qSYNuusNqJHVdXdjTBFycHAgLUpg);
				xmcEKmCGRPEAZqGNAGkHqLXgNgA.lhhvePGLXyqhBobYgHQMxKdAbBkP = new List<nujuvJulgYzNRYueyruWPgnaqlHP>();
				juVnlHGtcxzTEfUNtrFIOyvluHNF("Joystick Layout", P_0.joystickLayouts, P_1?.joystickLayouts, xmcEKmCGRPEAZqGNAGkHqLXgNgA.HDkgCuGOBNmSZoCUejZdOdigNGDNA.joystickLayouts, P_2, xmcEKmCGRPEAZqGNAGkHqLXgNgA.lhhvePGLXyqhBobYgHQMxKdAbBkP, rEvABygUkOfYEkBliaAlBIpFisUVB._003C_003E9.TEftybiGKVFUVivVbHsxSvRzHVNn, rEvABygUkOfYEkBliaAlBIpFisUVB._003C_003E9.voQjXuPXhbJiRbnGmTgDrlhNuxXu, rEvABygUkOfYEkBliaAlBIpFisUVB._003C_003E9.lSnARcgYwpAtmlhfMszLsaOWWeWmA, xmcEKmCGRPEAZqGNAGkHqLXgNgA.WbKXFFyrbclzxNyWdqEkRbtkRmht);
				xmcEKmCGRPEAZqGNAGkHqLXgNgA.gymYSGRESagpXdwDeEOEIjbJKGyAA = new List<nujuvJulgYzNRYueyruWPgnaqlHP>();
				juVnlHGtcxzTEfUNtrFIOyvluHNF("Custom Controller Layout", P_0.customControllerLayouts, P_1?.customControllerLayouts, xmcEKmCGRPEAZqGNAGkHqLXgNgA.HDkgCuGOBNmSZoCUejZdOdigNGDNA.customControllerLayouts, P_2, xmcEKmCGRPEAZqGNAGkHqLXgNgA.gymYSGRESagpXdwDeEOEIjbJKGyAA, rEvABygUkOfYEkBliaAlBIpFisUVB._003C_003E9.wZPYRybJbxGXkCykUdaPccPEpuVVA, rEvABygUkOfYEkBliaAlBIpFisUVB._003C_003E9.TetQdBTMVdmULyPltIGfZdqIASie, rEvABygUkOfYEkBliaAlBIpFisUVB._003C_003E9.AlGcxnqrdjDIUNTQuArcmQovPFDs, xmcEKmCGRPEAZqGNAGkHqLXgNgA.RlasGeiTvXsNDDZDLtwGHrUeGOaP);
				xmcEKmCGRPEAZqGNAGkHqLXgNgA.kIDFsCBDuKsjohFGHGJKOkzFMmDb = xmcEKmCGRPEAZqGNAGkHqLXgNgA.nTcYoreblegtunEHVXPeEGVPVzZg;
				xmcEKmCGRPEAZqGNAGkHqLXgNgA.UzvGjfkjNdAcVEwwAgeRhisBAbmgA = new List<nujuvJulgYzNRYueyruWPgnaqlHP>();
				juVnlHGtcxzTEfUNtrFIOyvluHNF("Custom Controller", P_0.customControllers, P_1?.customControllers, xmcEKmCGRPEAZqGNAGkHqLXgNgA.HDkgCuGOBNmSZoCUejZdOdigNGDNA.customControllers, P_2, xmcEKmCGRPEAZqGNAGkHqLXgNgA.UzvGjfkjNdAcVEwwAgeRhisBAbmgA, rEvABygUkOfYEkBliaAlBIpFisUVB._003C_003E9.AEuuRnbWxrmnKjXVDaPXeRzrOvLy, rEvABygUkOfYEkBliaAlBIpFisUVB._003C_003E9.TDdUXlTHoXoGhYQWtloSTZPNgxXU, rEvABygUkOfYEkBliaAlBIpFisUVB._003C_003E9.wjFwmKMByIqWuFzUMfWrIBMNAmfjA, xmcEKmCGRPEAZqGNAGkHqLXgNgA.VqNTixjBXVIMUCziRHqrvOrxYEUF);
				xmcEKmCGRPEAZqGNAGkHqLXgNgA.FJFXLnErPbqqkKvYfbEKQDpejRyH = new List<nujuvJulgYzNRYueyruWPgnaqlHP>();
				juVnlHGtcxzTEfUNtrFIOyvluHNF("Layout Manager Set", P_0.controllerMapLayoutManagerRuleSets, P_1?.controllerMapLayoutManagerRuleSets, xmcEKmCGRPEAZqGNAGkHqLXgNgA.HDkgCuGOBNmSZoCUejZdOdigNGDNA.controllerMapLayoutManagerRuleSets, P_2, xmcEKmCGRPEAZqGNAGkHqLXgNgA.FJFXLnErPbqqkKvYfbEKQDpejRyH, rEvABygUkOfYEkBliaAlBIpFisUVB._003C_003E9.lKUzkqFVlonREFPNpCyPvyeZCtzV, rEvABygUkOfYEkBliaAlBIpFisUVB._003C_003E9.rnWCEHhDVNstHQFGwmDTeKbgohQv, rEvABygUkOfYEkBliaAlBIpFisUVB._003C_003E9.mpuWwRwaTVoKRwIDnucMyMZQqBtq, xmcEKmCGRPEAZqGNAGkHqLXgNgA.hLhsLJlMwsLsPxmCOPySytPWVEiL);
				xmcEKmCGRPEAZqGNAGkHqLXgNgA.AYlBweOATLFqPIszncJPPLFytPuK = new List<nujuvJulgYzNRYueyruWPgnaqlHP>();
				juVnlHGtcxzTEfUNtrFIOyvluHNF("Controller Map Enabler Set", P_0.controllerMapEnablerRuleSets, P_1?.controllerMapEnablerRuleSets, xmcEKmCGRPEAZqGNAGkHqLXgNgA.HDkgCuGOBNmSZoCUejZdOdigNGDNA.controllerMapEnablerRuleSets, P_2, xmcEKmCGRPEAZqGNAGkHqLXgNgA.AYlBweOATLFqPIszncJPPLFytPuK, rEvABygUkOfYEkBliaAlBIpFisUVB._003C_003E9.KTCyFRTLNmNbYaehDSqHShkVIkvjA, rEvABygUkOfYEkBliaAlBIpFisUVB._003C_003E9.nWhIKZCYeGjjbQcXwLlenlgljtiDA, rEvABygUkOfYEkBliaAlBIpFisUVB._003C_003E9.UrdluDvpPMNumnjmJGRWaYjQNsBfA, xmcEKmCGRPEAZqGNAGkHqLXgNgA.TIcLkuPnKTmeuGkHysUaOIkdpiri);
				List<nujuvJulgYzNRYueyruWPgnaqlHP> list = new List<nujuvJulgYzNRYueyruWPgnaqlHP>();
				juVnlHGtcxzTEfUNtrFIOyvluHNF("Player", P_0.players, P_1?.players, xmcEKmCGRPEAZqGNAGkHqLXgNgA.HDkgCuGOBNmSZoCUejZdOdigNGDNA.players, P_2, list, rEvABygUkOfYEkBliaAlBIpFisUVB._003C_003E9.FvMGJZMGeQEDnposlgsnbWMuukKIA, rEvABygUkOfYEkBliaAlBIpFisUVB._003C_003E9.WwSxtsFnHvVREFdtDsQfZOoPeqCCA, rEvABygUkOfYEkBliaAlBIpFisUVB._003C_003E9.WVeKNwgFDxBcMENluBxJHYAhxpTHA, xmcEKmCGRPEAZqGNAGkHqLXgNgA.lDSftqDglEUWCuoxzbyvEcigktQxA);
				List<nujuvJulgYzNRYueyruWPgnaqlHP> list2 = new List<nujuvJulgYzNRYueyruWPgnaqlHP>();
				HwGAEEynbtCHfHxAwGLAdfbEnEowA hwGAEEynbtCHfHxAwGLAdfbEnEowA = new HwGAEEynbtCHfHxAwGLAdfbEnEowA();
				hwGAEEynbtCHfHxAwGLAdfbEnEowA.WpzTXfQiXHtduXsbRVkaPikyduFg = xmcEKmCGRPEAZqGNAGkHqLXgNgA;
				hwGAEEynbtCHfHxAwGLAdfbEnEowA.dNrwwVgnnfEEnwYQWpUyLcOkUKsS = hwGAEEynbtCHfHxAwGLAdfbEnEowA.WpzTXfQiXHtduXsbRVkaPikyduFg.LxnWPQAbzstdNmqElICPYQIjQnmw;
				juVnlHGtcxzTEfUNtrFIOyvluHNF("Keyboard Map", P_0.keyboardMaps, P_1?.keyboardMaps, hwGAEEynbtCHfHxAwGLAdfbEnEowA.WpzTXfQiXHtduXsbRVkaPikyduFg.HDkgCuGOBNmSZoCUejZdOdigNGDNA.keyboardMaps, P_2, list2, rEvABygUkOfYEkBliaAlBIpFisUVB._003C_003E9.xAfpMXPzsbRwpgTKnWNfDxeXtVDi, rEvABygUkOfYEkBliaAlBIpFisUVB._003C_003E9.WjwlGjfLIDQvbpGGTrehxhDOVHRw, hwGAEEynbtCHfHxAwGLAdfbEnEowA.tNunYImhAgnzIGBdzrYDpdOhOCsy, hwGAEEynbtCHfHxAwGLAdfbEnEowA.xlsAcNkucHePEgcNHFMnHMlqGRxx);
				List<nujuvJulgYzNRYueyruWPgnaqlHP> list3 = new List<nujuvJulgYzNRYueyruWPgnaqlHP>();
				gLCYxcpqdiaoCVDOvUkhQEaCoxxN gLCYxcpqdiaoCVDOvUkhQEaCoxxN2 = new gLCYxcpqdiaoCVDOvUkhQEaCoxxN();
				gLCYxcpqdiaoCVDOvUkhQEaCoxxN2.dnhUusjoiebHqBkeeJRcTylnlrQQ = xmcEKmCGRPEAZqGNAGkHqLXgNgA;
				gLCYxcpqdiaoCVDOvUkhQEaCoxxN2.dNrwwVgnnfEEnwYQWpUyLcOkUKsS = gLCYxcpqdiaoCVDOvUkhQEaCoxxN2.dnhUusjoiebHqBkeeJRcTylnlrQQ.BGClHotKwyDHRcCAZxUtRuojYxAeA;
				juVnlHGtcxzTEfUNtrFIOyvluHNF("Mouse Map", P_0.mouseMaps, P_1?.mouseMaps, gLCYxcpqdiaoCVDOvUkhQEaCoxxN2.dnhUusjoiebHqBkeeJRcTylnlrQQ.HDkgCuGOBNmSZoCUejZdOdigNGDNA.mouseMaps, P_2, list3, rEvABygUkOfYEkBliaAlBIpFisUVB._003C_003E9.wNkthCnaqKXNYSKuKsJLjKaPmjFG, rEvABygUkOfYEkBliaAlBIpFisUVB._003C_003E9.hadDyITekfZvjAkgQUspAHMsrXsv, gLCYxcpqdiaoCVDOvUkhQEaCoxxN2.GmBcvQljYDzNazNYBZFlVHJFgdPH, gLCYxcpqdiaoCVDOvUkhQEaCoxxN2.AWIDfhgPpXYdZFMCctNAZSzqwzWf);
				List<nujuvJulgYzNRYueyruWPgnaqlHP> list4 = new List<nujuvJulgYzNRYueyruWPgnaqlHP>();
				VNzISyiWRzGLyqtCYDrqLtfiVvfG vNzISyiWRzGLyqtCYDrqLtfiVvfG = new VNzISyiWRzGLyqtCYDrqLtfiVvfG();
				vNzISyiWRzGLyqtCYDrqLtfiVvfG.EacguhfpbEwuRJxWuDagiZnfZeFqA = xmcEKmCGRPEAZqGNAGkHqLXgNgA;
				vNzISyiWRzGLyqtCYDrqLtfiVvfG.dNrwwVgnnfEEnwYQWpUyLcOkUKsS = vNzISyiWRzGLyqtCYDrqLtfiVvfG.EacguhfpbEwuRJxWuDagiZnfZeFqA.lhhvePGLXyqhBobYgHQMxKdAbBkP;
				juVnlHGtcxzTEfUNtrFIOyvluHNF("Joystick Map", P_0.joystickMaps, P_1?.joystickMaps, vNzISyiWRzGLyqtCYDrqLtfiVvfG.EacguhfpbEwuRJxWuDagiZnfZeFqA.HDkgCuGOBNmSZoCUejZdOdigNGDNA.joystickMaps, P_2, list4, rEvABygUkOfYEkBliaAlBIpFisUVB._003C_003E9.shaAuobKyFujtuXwmawhvpEOiNzaA, rEvABygUkOfYEkBliaAlBIpFisUVB._003C_003E9.khORnAgmfIjzSfauCGzMcChplAzlA, vNzISyiWRzGLyqtCYDrqLtfiVvfG.uwFRnXPQIgeUEsNEmClZTtssnJLl, vNzISyiWRzGLyqtCYDrqLtfiVvfG.QODdGzfhRxlQLaHIfIqPTBJFOtRd);
				List<nujuvJulgYzNRYueyruWPgnaqlHP> list5 = new List<nujuvJulgYzNRYueyruWPgnaqlHP>();
				tUuipCEqOyHrMZiSdMxMuEyPdfKP tUuipCEqOyHrMZiSdMxMuEyPdfKP2 = new tUuipCEqOyHrMZiSdMxMuEyPdfKP();
				tUuipCEqOyHrMZiSdMxMuEyPdfKP2.GTyExVTBYJavQJkfGWnrbPDkYeTC = xmcEKmCGRPEAZqGNAGkHqLXgNgA;
				tUuipCEqOyHrMZiSdMxMuEyPdfKP2.dNrwwVgnnfEEnwYQWpUyLcOkUKsS = tUuipCEqOyHrMZiSdMxMuEyPdfKP2.GTyExVTBYJavQJkfGWnrbPDkYeTC.gymYSGRESagpXdwDeEOEIjbJKGyAA;
				juVnlHGtcxzTEfUNtrFIOyvluHNF("Custom Controller Map", P_0.customControllerMaps, P_1?.customControllerMaps, tUuipCEqOyHrMZiSdMxMuEyPdfKP2.GTyExVTBYJavQJkfGWnrbPDkYeTC.HDkgCuGOBNmSZoCUejZdOdigNGDNA.customControllerMaps, P_2, list5, rEvABygUkOfYEkBliaAlBIpFisUVB._003C_003E9.rLTGpKtAFwbQzjsdCpPSFNvcLZIBA, rEvABygUkOfYEkBliaAlBIpFisUVB._003C_003E9.ITcelZhXFwpYEitzEbKFOfXUzuuH, tUuipCEqOyHrMZiSdMxMuEyPdfKP2.pFmvEwHAIjgLMAhBLbLDuoLBTotO, tUuipCEqOyHrMZiSdMxMuEyPdfKP2.UMdTRdkWHNwhnmHAZVmfsbcXFziDA);
				return xmcEKmCGRPEAZqGNAGkHqLXgNgA.HDkgCuGOBNmSZoCUejZdOdigNGDNA;
			}

			[Conditional("DEBUG_IMPORT")]
			private static void zIraDrjQUzfSUSObJgyxAwoDvSdZA(object P_0)
			{
				Logger.Log("[DEBUG_IMPORT] " + P_0);
			}

			private static void CiOTsWPWICCngyVEtwGSSYljjUcd<_0001>(IList<_0001> P_0, IList<_0001> P_1, IList<_0001> P_2, Func<_0001, IList<_0001>, int> P_3)
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

			private static void juVnlHGtcxzTEfUNtrFIOyvluHNF<_0001>(string P_0, IList<_0001> P_1, IList<_0001> P_2, IList<_0001> P_3, bool P_4, List<nujuvJulgYzNRYueyruWPgnaqlHP> P_5, Func<_0001, int> P_6, Func<_0001, string> P_7, Func<_0001, IList<_0001>, int> P_8, Func<koiccHreeSzhhtmqpAfbekazGvOAb<_0001>, _0001> P_9) where _0001 : class
			{
				nfXRWZiPwVeOBzTukskSRmCkzkPO<_0001> nfXRWZiPwVeOBzTukskSRmCkzkPO2 = new nfXRWZiPwVeOBzTukskSRmCkzkPO<_0001>();
				nfXRWZiPwVeOBzTukskSRmCkzkPO2.vWPmVWuzoNKiwYZUxRLBYizWpHpj = P_6;
				for (int i = 0; i < P_1.Count; i++)
				{
					_0001 val = P_1[i];
					if (P_4)
					{
						P_5.Add(new nujuvJulgYzNRYueyruWPgnaqlHP(nfXRWZiPwVeOBzTukskSRmCkzkPO2.vWPmVWuzoNKiwYZUxRLBYizWpHpj(val), -1, nfXRWZiPwVeOBzTukskSRmCkzkPO2.vWPmVWuzoNKiwYZUxRLBYizWpHpj(val)));
						continue;
					}
					_0001 arg = P_9(new koiccHreeSzhhtmqpAfbekazGvOAb<_0001>(val, null, nujuvJulgYzNRYueyruWPgnaqlHP.KfuHwiomPteqWnzNNKhAzBQQnmQh.origId, P_3, false));
					P_5.Add(new nujuvJulgYzNRYueyruWPgnaqlHP(nfXRWZiPwVeOBzTukskSRmCkzkPO2.vWPmVWuzoNKiwYZUxRLBYizWpHpj(val), -1, nfXRWZiPwVeOBzTukskSRmCkzkPO2.vWPmVWuzoNKiwYZUxRLBYizWpHpj(arg)));
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
						RajAKAYAvJjmpOJPvFcgUtdyulNP<_0001> rajAKAYAvJjmpOJPvFcgUtdyulNP = new RajAKAYAvJjmpOJPvFcgUtdyulNP<_0001>();
						rajAKAYAvJjmpOJPvFcgUtdyulNP.cRXBQcKBiFKKgieBjgErDNqfQAQfc = nfXRWZiPwVeOBzTukskSRmCkzkPO2;
						_0001 val3 = P_3[num];
						rajAKAYAvJjmpOJPvFcgUtdyulNP.KtLcYaFGtXgWVWKJtALgxYQNZlfKA = P_9(new koiccHreeSzhhtmqpAfbekazGvOAb<_0001>(val2, val3, nujuvJulgYzNRYueyruWPgnaqlHP.KfuHwiomPteqWnzNNKhAzBQQnmQh.otherId, P_3, true));
						P_5.Find(rajAKAYAvJjmpOJPvFcgUtdyulNP.DrPdQBNBCgzNDAeCRIfyHcnSboJcb).okeWwwmvgkmEMOtIulLmImzRWNpc = rajAKAYAvJjmpOJPvFcgUtdyulNP.cRXBQcKBiFKKgieBjgErDNqfQAQfc.vWPmVWuzoNKiwYZUxRLBYizWpHpj(val2);
						string text = ((!string.IsNullOrEmpty(P_7(val2))) ? ("\"" + P_7(val2) + "\"") : "");
						Logger.Log(P_0 + ((!string.IsNullOrEmpty(text)) ? (" " + text) : "") + " already exists. Imported data will replace original.");
					}
					else
					{
						_0001 arg2 = P_9(new koiccHreeSzhhtmqpAfbekazGvOAb<_0001>(val2, null, nujuvJulgYzNRYueyruWPgnaqlHP.KfuHwiomPteqWnzNNKhAzBQQnmQh.otherId, P_3, false));
						P_5.Add(new nujuvJulgYzNRYueyruWPgnaqlHP(-1, nfXRWZiPwVeOBzTukskSRmCkzkPO2.vWPmVWuzoNKiwYZUxRLBYizWpHpj(val2), nfXRWZiPwVeOBzTukskSRmCkzkPO2.vWPmVWuzoNKiwYZUxRLBYizWpHpj(arg2)));
						string text2 = ((!string.IsNullOrEmpty(P_7(val2))) ? ("\"" + P_7(val2) + "\"") : "");
						Logger.Log("Imported new " + P_0 + ((!string.IsNullOrEmpty(text2)) ? (" " + text2) : "") + ".");
					}
				}
			}
		}

		[Serializable]
		private sealed class tEXZZFhMsnLkGSeIaOnLZEnxfkok
		{
			public static readonly tEXZZFhMsnLkGSeIaOnLZEnxfkok _003C_003E9 = new tEXZZFhMsnLkGSeIaOnLZEnxfkok();

			public static Action<List<Player_Editor.Mapping>, int> _003C_003E9__195_0;

			public static Action<List<Player_Editor.Mapping>, int> _003C_003E9__213_0;

			public static Action<List<Player_Editor.Mapping>, int> _003C_003E9__229_0;

			public static Action<List<Player_Editor.Mapping>, int> _003C_003E9__245_0;

			public static Action<List<Player_Editor.Mapping>, int> _003C_003E9__261_0;

			internal void ChvEXcEOrAyMqdtCxcLSnlKpyhZH(List<Player_Editor.Mapping> P_0, int P_1)
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

			internal void jtSIrkhNmyHRPevtDPEevgLDfvRR(List<Player_Editor.Mapping> P_0, int P_1)
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

			internal void LbSnifsTtbVCOOLdOueiUThKVdah(List<Player_Editor.Mapping> P_0, int P_1)
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

			internal void NmDWOyxMfaFuPToNlxaFWdyiJJBu(List<Player_Editor.Mapping> P_0, int P_1)
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

			internal void JnKUCgfFGNBqXjkxRZDYsvhVpMVi(List<Player_Editor.Mapping> P_0, int P_1)
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

		private sealed class sxkDCdJWVEdObPfcoGRkqMMfezFXA
		{
			public List<InputLayout> SzSTUANKpBjMimPpLexrwwqIUDUj;

			internal int tmqarDMHdPacYIQiuQpsHaSyZvPpA(ControllerMap_Editor P_0, ControllerMap_Editor P_1)
			{
				aBRafFNNpPspwmQePalNNWLCqQHN aBRafFNNpPspwmQePalNNWLCqQHN2 = new aBRafFNNpPspwmQePalNNWLCqQHN();
				aBRafFNNpPspwmQePalNNWLCqQHN2.MoPTGRsFgXdMwTraXsxZonteyAEm = P_0;
				aBRafFNNpPspwmQePalNNWLCqQHN2.eBUQwXOmrrriDgdoiVUdsZqKsHQo = P_1;
				int num = SzSTUANKpBjMimPpLexrwwqIUDUj.FindIndex(aBRafFNNpPspwmQePalNNWLCqQHN2.BtRQTpizTSCVNkSpcQfmdOIEPWJD);
				int num2 = SzSTUANKpBjMimPpLexrwwqIUDUj.FindIndex(aBRafFNNpPspwmQePalNNWLCqQHN2.akuKljGxSugdhuRkpLqylvpjaaQH);
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

		private sealed class aBRafFNNpPspwmQePalNNWLCqQHN
		{
			public ControllerMap_Editor MoPTGRsFgXdMwTraXsxZonteyAEm;

			public ControllerMap_Editor eBUQwXOmrrriDgdoiVUdsZqKsHQo;

			internal bool BtRQTpizTSCVNkSpcQfmdOIEPWJD(InputLayout P_0)
			{
				return P_0.id == MoPTGRsFgXdMwTraXsxZonteyAEm.id;
			}

			internal bool akuKljGxSugdhuRkpLqylvpjaaQH(InputLayout P_0)
			{
				return P_0.id == eBUQwXOmrrriDgdoiVUdsZqKsHQo.id;
			}
		}

		private sealed class hCOcgraSglEiyycZfLLVTBKruhjA : IDisposable, IEnumerable, IEnumerator, IEnumerable<InputCategory>, IEnumerator<InputCategory>
		{
			private int RxAoyfYzYDsYonLGXsvUgwChukLk;

			private InputCategory VqEePGSMyrGKIqkWibsjeHcWPSIx;

			private int wLBvvzlDcbAEPyDfgnprolEZBsgN;

			private string eBfDTHTrxuRklzIaThUTCseJJiEeb;

			public string NgUGOCgMAPNuMsxXPWsrQJXFnwujA;

			public UserData TtytLoUfsgUyhsklaKccrnoMiiek;

			private int hWZNbLCFLBBWXNZxiKeGtzgrnTsg;

			InputCategory IEnumerator<InputCategory>.Current
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
			public hCOcgraSglEiyycZfLLVTBKruhjA(int P_0)
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
				UserData ttytLoUfsgUyhsklaKccrnoMiiek = TtytLoUfsgUyhsklaKccrnoMiiek;
				if (rxAoyfYzYDsYonLGXsvUgwChukLk != 0)
				{
					if (rxAoyfYzYDsYonLGXsvUgwChukLk != 1)
					{
						return false;
					}
					RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
					goto IL_0098;
				}
				RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
				if (eBfDTHTrxuRklzIaThUTCseJJiEeb == null || eBfDTHTrxuRklzIaThUTCseJJiEeb == string.Empty)
				{
					return false;
				}
				if (ttytLoUfsgUyhsklaKccrnoMiiek.actionCategories == null)
				{
					return false;
				}
				hWZNbLCFLBBWXNZxiKeGtzgrnTsg = 0;
				goto IL_00a8;
				IL_00a8:
				if (hWZNbLCFLBBWXNZxiKeGtzgrnTsg < ttytLoUfsgUyhsklaKccrnoMiiek.actionCategories.Count)
				{
					if (ttytLoUfsgUyhsklaKccrnoMiiek.actionCategories[hWZNbLCFLBBWXNZxiKeGtzgrnTsg].tag.Equals(eBfDTHTrxuRklzIaThUTCseJJiEeb, StringComparison.OrdinalIgnoreCase))
					{
						VqEePGSMyrGKIqkWibsjeHcWPSIx = ttytLoUfsgUyhsklaKccrnoMiiek.actionCategories[hWZNbLCFLBBWXNZxiKeGtzgrnTsg];
						RxAoyfYzYDsYonLGXsvUgwChukLk = 1;
						return true;
					}
					goto IL_0098;
				}
				return false;
				IL_0098:
				hWZNbLCFLBBWXNZxiKeGtzgrnTsg++;
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
				hCOcgraSglEiyycZfLLVTBKruhjA hCOcgraSglEiyycZfLLVTBKruhjA2;
				if (RxAoyfYzYDsYonLGXsvUgwChukLk == -2 && wLBvvzlDcbAEPyDfgnprolEZBsgN == Thread.CurrentThread.ManagedThreadId)
				{
					RxAoyfYzYDsYonLGXsvUgwChukLk = 0;
					hCOcgraSglEiyycZfLLVTBKruhjA2 = this;
				}
				else
				{
					hCOcgraSglEiyycZfLLVTBKruhjA2 = new hCOcgraSglEiyycZfLLVTBKruhjA(0);
					hCOcgraSglEiyycZfLLVTBKruhjA2.TtytLoUfsgUyhsklaKccrnoMiiek = TtytLoUfsgUyhsklaKccrnoMiiek;
				}
				hCOcgraSglEiyycZfLLVTBKruhjA2.eBfDTHTrxuRklzIaThUTCseJJiEeb = NgUGOCgMAPNuMsxXPWsrQJXFnwujA;
				return hCOcgraSglEiyycZfLLVTBKruhjA2;
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return ((IEnumerable<InputCategory>)this).GetEnumerator();
			}
		}

		private sealed class qFsuJfMnHXtpnYENKqUwoHFrQyRu : IDisposable, IEnumerable, IEnumerator, IEnumerable<InputAction>, IEnumerator<InputAction>
		{
			private int RxAoyfYzYDsYonLGXsvUgwChukLk;

			private InputAction VqEePGSMyrGKIqkWibsjeHcWPSIx;

			private int wLBvvzlDcbAEPyDfgnprolEZBsgN;

			public UserData TtytLoUfsgUyhsklaKccrnoMiiek;

			private string eBfDTHTrxuRklzIaThUTCseJJiEeb;

			public string NgUGOCgMAPNuMsxXPWsrQJXFnwujA;

			private int HuESZDqgOuhilhedORGWAZeeELeZA;

			private int fIMVaffCgsuIJcnrkMmGGKfPwwel;

			private InputCategory CiSqtyvJWIEswYbtlKqlNDzfSAAn;

			private int TgRRImNANcsJMttFJODvJoaYzMXW;

			InputAction IEnumerator<InputAction>.Current
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
			public qFsuJfMnHXtpnYENKqUwoHFrQyRu(int P_0)
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
				UserData ttytLoUfsgUyhsklaKccrnoMiiek = TtytLoUfsgUyhsklaKccrnoMiiek;
				if (rxAoyfYzYDsYonLGXsvUgwChukLk != 0)
				{
					if (rxAoyfYzYDsYonLGXsvUgwChukLk != 1)
					{
						return false;
					}
					RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
					goto IL_00fd;
				}
				RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
				if (ttytLoUfsgUyhsklaKccrnoMiiek.actions == null || ttytLoUfsgUyhsklaKccrnoMiiek.actionCategories == null)
				{
					return false;
				}
				if (eBfDTHTrxuRklzIaThUTCseJJiEeb == null || eBfDTHTrxuRklzIaThUTCseJJiEeb == string.Empty)
				{
					return false;
				}
				HuESZDqgOuhilhedORGWAZeeELeZA = ttytLoUfsgUyhsklaKccrnoMiiek.actions.Count;
				fIMVaffCgsuIJcnrkMmGGKfPwwel = 0;
				goto IL_0132;
				IL_0122:
				fIMVaffCgsuIJcnrkMmGGKfPwwel++;
				goto IL_0132;
				IL_00fd:
				TgRRImNANcsJMttFJODvJoaYzMXW++;
				goto IL_010d;
				IL_010d:
				if (TgRRImNANcsJMttFJODvJoaYzMXW < HuESZDqgOuhilhedORGWAZeeELeZA)
				{
					if (CiSqtyvJWIEswYbtlKqlNDzfSAAn.id == ttytLoUfsgUyhsklaKccrnoMiiek.actions[TgRRImNANcsJMttFJODvJoaYzMXW].categoryId)
					{
						VqEePGSMyrGKIqkWibsjeHcWPSIx = ttytLoUfsgUyhsklaKccrnoMiiek.actions[TgRRImNANcsJMttFJODvJoaYzMXW];
						RxAoyfYzYDsYonLGXsvUgwChukLk = 1;
						return true;
					}
					goto IL_00fd;
				}
				CiSqtyvJWIEswYbtlKqlNDzfSAAn = null;
				goto IL_0122;
				IL_0132:
				if (fIMVaffCgsuIJcnrkMmGGKfPwwel < ttytLoUfsgUyhsklaKccrnoMiiek.actionCategories.Count)
				{
					if (ttytLoUfsgUyhsklaKccrnoMiiek.actionCategories[fIMVaffCgsuIJcnrkMmGGKfPwwel].tag.Equals(eBfDTHTrxuRklzIaThUTCseJJiEeb, StringComparison.OrdinalIgnoreCase))
					{
						CiSqtyvJWIEswYbtlKqlNDzfSAAn = ttytLoUfsgUyhsklaKccrnoMiiek.actionCategories[fIMVaffCgsuIJcnrkMmGGKfPwwel];
						TgRRImNANcsJMttFJODvJoaYzMXW = 0;
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
				qFsuJfMnHXtpnYENKqUwoHFrQyRu qFsuJfMnHXtpnYENKqUwoHFrQyRu2;
				if (RxAoyfYzYDsYonLGXsvUgwChukLk == -2 && wLBvvzlDcbAEPyDfgnprolEZBsgN == Thread.CurrentThread.ManagedThreadId)
				{
					RxAoyfYzYDsYonLGXsvUgwChukLk = 0;
					qFsuJfMnHXtpnYENKqUwoHFrQyRu2 = this;
				}
				else
				{
					qFsuJfMnHXtpnYENKqUwoHFrQyRu2 = new qFsuJfMnHXtpnYENKqUwoHFrQyRu(0);
					qFsuJfMnHXtpnYENKqUwoHFrQyRu2.TtytLoUfsgUyhsklaKccrnoMiiek = TtytLoUfsgUyhsklaKccrnoMiiek;
				}
				qFsuJfMnHXtpnYENKqUwoHFrQyRu2.eBfDTHTrxuRklzIaThUTCseJJiEeb = NgUGOCgMAPNuMsxXPWsrQJXFnwujA;
				return qFsuJfMnHXtpnYENKqUwoHFrQyRu2;
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return ((IEnumerable<InputAction>)this).GetEnumerator();
			}
		}

		private sealed class GMLMKfxErfWoCSEfIbGUXoGNoQxs : IDisposable, IEnumerable, IEnumerator, IEnumerable<InputAction>, IEnumerator<InputAction>
		{
			private int RxAoyfYzYDsYonLGXsvUgwChukLk;

			private InputAction VqEePGSMyrGKIqkWibsjeHcWPSIx;

			private int wLBvvzlDcbAEPyDfgnprolEZBsgN;

			public UserData TtytLoUfsgUyhsklaKccrnoMiiek;

			private bool XNzRTyKnKPLTDsPJvWgNsGsOEcQdA;

			public bool BGTREeWkieZAGmzKkVdsDSoDuesc;

			private int ibNaRicErZhcmrUtserfrUoMsHIcA;

			public int CcDnwAqYkwKfkzcTgBMageoSxwgTA;

			private IEnumerator<int> hSeONskmQJhVsjVUHGUASYbDGJrU;

			private int fIMVaffCgsuIJcnrkMmGGKfPwwel;

			InputAction IEnumerator<InputAction>.Current
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
			public GMLMKfxErfWoCSEfIbGUXoGNoQxs(int P_0)
			{
				RxAoyfYzYDsYonLGXsvUgwChukLk = P_0;
				wLBvvzlDcbAEPyDfgnprolEZBsgN = Thread.CurrentThread.ManagedThreadId;
			}

			[DebuggerHidden]
			void IDisposable.Dispose()
			{
				int rxAoyfYzYDsYonLGXsvUgwChukLk = RxAoyfYzYDsYonLGXsvUgwChukLk;
				if (rxAoyfYzYDsYonLGXsvUgwChukLk == -3 || rxAoyfYzYDsYonLGXsvUgwChukLk == 1)
				{
					try
					{
					}
					finally
					{
						uvbwjgAeXsWySvtbvsLpuFVuAgJB();
					}
				}
			}

			private bool MoveNext()
			{
				try
				{
					int rxAoyfYzYDsYonLGXsvUgwChukLk = RxAoyfYzYDsYonLGXsvUgwChukLk;
					UserData ttytLoUfsgUyhsklaKccrnoMiiek = TtytLoUfsgUyhsklaKccrnoMiiek;
					switch (rxAoyfYzYDsYonLGXsvUgwChukLk)
					{
					default:
						return false;
					case 0:
						RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
						if (ttytLoUfsgUyhsklaKccrnoMiiek.actions == null || ttytLoUfsgUyhsklaKccrnoMiiek.actionCategories == null)
						{
							return false;
						}
						if (XNzRTyKnKPLTDsPJvWgNsGsOEcQdA)
						{
							hSeONskmQJhVsjVUHGUASYbDGJrU = ttytLoUfsgUyhsklaKccrnoMiiek.SortedActionIdsInCategory(ibNaRicErZhcmrUtserfrUoMsHIcA).GetEnumerator();
							RxAoyfYzYDsYonLGXsvUgwChukLk = -3;
							goto IL_00a5;
						}
						fIMVaffCgsuIJcnrkMmGGKfPwwel = 0;
						goto IL_0123;
					case 1:
						RxAoyfYzYDsYonLGXsvUgwChukLk = -3;
						goto IL_00a5;
					case 2:
						{
							RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
							goto IL_0111;
						}
						IL_0123:
						if (fIMVaffCgsuIJcnrkMmGGKfPwwel >= ttytLoUfsgUyhsklaKccrnoMiiek.actions.Count)
						{
							break;
						}
						if (ttytLoUfsgUyhsklaKccrnoMiiek.actions[fIMVaffCgsuIJcnrkMmGGKfPwwel].categoryId == ibNaRicErZhcmrUtserfrUoMsHIcA)
						{
							VqEePGSMyrGKIqkWibsjeHcWPSIx = ttytLoUfsgUyhsklaKccrnoMiiek.actions[fIMVaffCgsuIJcnrkMmGGKfPwwel];
							RxAoyfYzYDsYonLGXsvUgwChukLk = 2;
							return true;
						}
						goto IL_0111;
						IL_0111:
						fIMVaffCgsuIJcnrkMmGGKfPwwel++;
						goto IL_0123;
						IL_00a5:
						while (hSeONskmQJhVsjVUHGUASYbDGJrU.MoveNext())
						{
							int current = hSeONskmQJhVsjVUHGUASYbDGJrU.Current;
							InputAction actionById = ttytLoUfsgUyhsklaKccrnoMiiek.GetActionById(current);
							if (actionById != null)
							{
								VqEePGSMyrGKIqkWibsjeHcWPSIx = actionById;
								RxAoyfYzYDsYonLGXsvUgwChukLk = 1;
								return true;
							}
						}
						uvbwjgAeXsWySvtbvsLpuFVuAgJB();
						hSeONskmQJhVsjVUHGUASYbDGJrU = null;
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

			private void uvbwjgAeXsWySvtbvsLpuFVuAgJB()
			{
				RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
				if (hSeONskmQJhVsjVUHGUASYbDGJrU != null)
				{
					hSeONskmQJhVsjVUHGUASYbDGJrU.Dispose();
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
				GMLMKfxErfWoCSEfIbGUXoGNoQxs gMLMKfxErfWoCSEfIbGUXoGNoQxs;
				if (RxAoyfYzYDsYonLGXsvUgwChukLk == -2 && wLBvvzlDcbAEPyDfgnprolEZBsgN == Thread.CurrentThread.ManagedThreadId)
				{
					RxAoyfYzYDsYonLGXsvUgwChukLk = 0;
					gMLMKfxErfWoCSEfIbGUXoGNoQxs = this;
				}
				else
				{
					gMLMKfxErfWoCSEfIbGUXoGNoQxs = new GMLMKfxErfWoCSEfIbGUXoGNoQxs(0);
					gMLMKfxErfWoCSEfIbGUXoGNoQxs.TtytLoUfsgUyhsklaKccrnoMiiek = TtytLoUfsgUyhsklaKccrnoMiiek;
				}
				gMLMKfxErfWoCSEfIbGUXoGNoQxs.ibNaRicErZhcmrUtserfrUoMsHIcA = CcDnwAqYkwKfkzcTgBMageoSxwgTA;
				gMLMKfxErfWoCSEfIbGUXoGNoQxs.XNzRTyKnKPLTDsPJvWgNsGsOEcQdA = BGTREeWkieZAGmzKkVdsDSoDuesc;
				return gMLMKfxErfWoCSEfIbGUXoGNoQxs;
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return ((IEnumerable<InputAction>)this).GetEnumerator();
			}
		}

		private sealed class LHlQrpnsouKHQOGqkNgrJyVIHaVK : IDisposable, IEnumerable, IEnumerator, IEnumerable<InputAction>, IEnumerator<InputAction>
		{
			private int RxAoyfYzYDsYonLGXsvUgwChukLk;

			private InputAction VqEePGSMyrGKIqkWibsjeHcWPSIx;

			private int wLBvvzlDcbAEPyDfgnprolEZBsgN;

			public UserData TtytLoUfsgUyhsklaKccrnoMiiek;

			private string JfLWbdRgNfbsKBPdOxZRueGoaxsZ;

			public string IVdVQqSTIUohUJHKKUYEEfPwfuzz;

			private bool XNzRTyKnKPLTDsPJvWgNsGsOEcQdA;

			public bool BGTREeWkieZAGmzKkVdsDSoDuesc;

			private InputCategory VnqVBRHojIEbQahfALtKmhBesibXA;

			private IEnumerator<int> tIlurAGjswRzPeLpFSGHcEnzPBQq;

			private int FhIFjZbpbSEntgsVWVgqHGbPqlLqA;

			InputAction IEnumerator<InputAction>.Current
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
			public LHlQrpnsouKHQOGqkNgrJyVIHaVK(int P_0)
			{
				RxAoyfYzYDsYonLGXsvUgwChukLk = P_0;
				wLBvvzlDcbAEPyDfgnprolEZBsgN = Thread.CurrentThread.ManagedThreadId;
			}

			[DebuggerHidden]
			void IDisposable.Dispose()
			{
				int rxAoyfYzYDsYonLGXsvUgwChukLk = RxAoyfYzYDsYonLGXsvUgwChukLk;
				if (rxAoyfYzYDsYonLGXsvUgwChukLk == -3 || rxAoyfYzYDsYonLGXsvUgwChukLk == 1)
				{
					try
					{
					}
					finally
					{
						uvbwjgAeXsWySvtbvsLpuFVuAgJB();
					}
				}
			}

			private bool MoveNext()
			{
				try
				{
					int rxAoyfYzYDsYonLGXsvUgwChukLk = RxAoyfYzYDsYonLGXsvUgwChukLk;
					UserData ttytLoUfsgUyhsklaKccrnoMiiek = TtytLoUfsgUyhsklaKccrnoMiiek;
					switch (rxAoyfYzYDsYonLGXsvUgwChukLk)
					{
					default:
						return false;
					case 0:
					{
						RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
						if (ttytLoUfsgUyhsklaKccrnoMiiek.actions == null || ttytLoUfsgUyhsklaKccrnoMiiek.actionCategories == null)
						{
							return false;
						}
						if (JfLWbdRgNfbsKBPdOxZRueGoaxsZ == null || JfLWbdRgNfbsKBPdOxZRueGoaxsZ == string.Empty)
						{
							return false;
						}
						int num = ttytLoUfsgUyhsklaKccrnoMiiek.IndexOfActionCategory(JfLWbdRgNfbsKBPdOxZRueGoaxsZ);
						if (num < 0)
						{
							return false;
						}
						VnqVBRHojIEbQahfALtKmhBesibXA = ttytLoUfsgUyhsklaKccrnoMiiek.GetActionCategory(num);
						if (XNzRTyKnKPLTDsPJvWgNsGsOEcQdA)
						{
							tIlurAGjswRzPeLpFSGHcEnzPBQq = ttytLoUfsgUyhsklaKccrnoMiiek.SortedActionIdsInCategory(VnqVBRHojIEbQahfALtKmhBesibXA.id).GetEnumerator();
							RxAoyfYzYDsYonLGXsvUgwChukLk = -3;
							goto IL_00f2;
						}
						FhIFjZbpbSEntgsVWVgqHGbPqlLqA = 0;
						goto IL_0175;
					}
					case 1:
						RxAoyfYzYDsYonLGXsvUgwChukLk = -3;
						goto IL_00f2;
					case 2:
						{
							RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
							goto IL_0163;
						}
						IL_0175:
						if (FhIFjZbpbSEntgsVWVgqHGbPqlLqA >= ttytLoUfsgUyhsklaKccrnoMiiek.actions.Count)
						{
							break;
						}
						if (ttytLoUfsgUyhsklaKccrnoMiiek.actions[FhIFjZbpbSEntgsVWVgqHGbPqlLqA].categoryId == VnqVBRHojIEbQahfALtKmhBesibXA.id)
						{
							VqEePGSMyrGKIqkWibsjeHcWPSIx = ttytLoUfsgUyhsklaKccrnoMiiek.actions[FhIFjZbpbSEntgsVWVgqHGbPqlLqA];
							RxAoyfYzYDsYonLGXsvUgwChukLk = 2;
							return true;
						}
						goto IL_0163;
						IL_00f2:
						while (tIlurAGjswRzPeLpFSGHcEnzPBQq.MoveNext())
						{
							int current = tIlurAGjswRzPeLpFSGHcEnzPBQq.Current;
							InputAction actionById = ttytLoUfsgUyhsklaKccrnoMiiek.GetActionById(current);
							if (actionById != null)
							{
								VqEePGSMyrGKIqkWibsjeHcWPSIx = actionById;
								RxAoyfYzYDsYonLGXsvUgwChukLk = 1;
								return true;
							}
						}
						uvbwjgAeXsWySvtbvsLpuFVuAgJB();
						tIlurAGjswRzPeLpFSGHcEnzPBQq = null;
						break;
						IL_0163:
						FhIFjZbpbSEntgsVWVgqHGbPqlLqA++;
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

			private void uvbwjgAeXsWySvtbvsLpuFVuAgJB()
			{
				RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
				if (tIlurAGjswRzPeLpFSGHcEnzPBQq != null)
				{
					tIlurAGjswRzPeLpFSGHcEnzPBQq.Dispose();
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
				LHlQrpnsouKHQOGqkNgrJyVIHaVK lHlQrpnsouKHQOGqkNgrJyVIHaVK;
				if (RxAoyfYzYDsYonLGXsvUgwChukLk == -2 && wLBvvzlDcbAEPyDfgnprolEZBsgN == Thread.CurrentThread.ManagedThreadId)
				{
					RxAoyfYzYDsYonLGXsvUgwChukLk = 0;
					lHlQrpnsouKHQOGqkNgrJyVIHaVK = this;
				}
				else
				{
					lHlQrpnsouKHQOGqkNgrJyVIHaVK = new LHlQrpnsouKHQOGqkNgrJyVIHaVK(0);
					lHlQrpnsouKHQOGqkNgrJyVIHaVK.TtytLoUfsgUyhsklaKccrnoMiiek = TtytLoUfsgUyhsklaKccrnoMiiek;
				}
				lHlQrpnsouKHQOGqkNgrJyVIHaVK.JfLWbdRgNfbsKBPdOxZRueGoaxsZ = IVdVQqSTIUohUJHKKUYEEfPwfuzz;
				lHlQrpnsouKHQOGqkNgrJyVIHaVK.XNzRTyKnKPLTDsPJvWgNsGsOEcQdA = BGTREeWkieZAGmzKkVdsDSoDuesc;
				return lHlQrpnsouKHQOGqkNgrJyVIHaVK;
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return ((IEnumerable<InputAction>)this).GetEnumerator();
			}
		}

		private sealed class ibgHUDbSbKDfxabNuFrtdODeuNaIc : IDisposable, IEnumerable, IEnumerator, IEnumerable<InputMapCategory>, IEnumerator<InputMapCategory>
		{
			private int RxAoyfYzYDsYonLGXsvUgwChukLk;

			private InputMapCategory VqEePGSMyrGKIqkWibsjeHcWPSIx;

			private int wLBvvzlDcbAEPyDfgnprolEZBsgN;

			private string eBfDTHTrxuRklzIaThUTCseJJiEeb;

			public string NgUGOCgMAPNuMsxXPWsrQJXFnwujA;

			public UserData TtytLoUfsgUyhsklaKccrnoMiiek;

			private int hWZNbLCFLBBWXNZxiKeGtzgrnTsg;

			InputMapCategory IEnumerator<InputMapCategory>.Current
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
			public ibgHUDbSbKDfxabNuFrtdODeuNaIc(int P_0)
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
				UserData ttytLoUfsgUyhsklaKccrnoMiiek = TtytLoUfsgUyhsklaKccrnoMiiek;
				if (rxAoyfYzYDsYonLGXsvUgwChukLk != 0)
				{
					if (rxAoyfYzYDsYonLGXsvUgwChukLk != 1)
					{
						return false;
					}
					RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
					goto IL_0098;
				}
				RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
				if (eBfDTHTrxuRklzIaThUTCseJJiEeb == null || eBfDTHTrxuRklzIaThUTCseJJiEeb == string.Empty)
				{
					return false;
				}
				if (ttytLoUfsgUyhsklaKccrnoMiiek.mapCategories == null)
				{
					return false;
				}
				hWZNbLCFLBBWXNZxiKeGtzgrnTsg = 0;
				goto IL_00a8;
				IL_00a8:
				if (hWZNbLCFLBBWXNZxiKeGtzgrnTsg < ttytLoUfsgUyhsklaKccrnoMiiek.mapCategories.Count)
				{
					if (ttytLoUfsgUyhsklaKccrnoMiiek.mapCategories[hWZNbLCFLBBWXNZxiKeGtzgrnTsg].tag.Equals(eBfDTHTrxuRklzIaThUTCseJJiEeb, StringComparison.OrdinalIgnoreCase))
					{
						VqEePGSMyrGKIqkWibsjeHcWPSIx = ttytLoUfsgUyhsklaKccrnoMiiek.mapCategories[hWZNbLCFLBBWXNZxiKeGtzgrnTsg];
						RxAoyfYzYDsYonLGXsvUgwChukLk = 1;
						return true;
					}
					goto IL_0098;
				}
				return false;
				IL_0098:
				hWZNbLCFLBBWXNZxiKeGtzgrnTsg++;
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
				ibgHUDbSbKDfxabNuFrtdODeuNaIc ibgHUDbSbKDfxabNuFrtdODeuNaIc2;
				if (RxAoyfYzYDsYonLGXsvUgwChukLk == -2 && wLBvvzlDcbAEPyDfgnprolEZBsgN == Thread.CurrentThread.ManagedThreadId)
				{
					RxAoyfYzYDsYonLGXsvUgwChukLk = 0;
					ibgHUDbSbKDfxabNuFrtdODeuNaIc2 = this;
				}
				else
				{
					ibgHUDbSbKDfxabNuFrtdODeuNaIc2 = new ibgHUDbSbKDfxabNuFrtdODeuNaIc(0);
					ibgHUDbSbKDfxabNuFrtdODeuNaIc2.TtytLoUfsgUyhsklaKccrnoMiiek = TtytLoUfsgUyhsklaKccrnoMiiek;
				}
				ibgHUDbSbKDfxabNuFrtdODeuNaIc2.eBfDTHTrxuRklzIaThUTCseJJiEeb = NgUGOCgMAPNuMsxXPWsrQJXFnwujA;
				return ibgHUDbSbKDfxabNuFrtdODeuNaIc2;
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return ((IEnumerable<InputMapCategory>)this).GetEnumerator();
			}
		}

		private sealed class mLmESluXPpdjGmogNKrScYTCSJRJ : IDisposable, IEnumerable, IEnumerator, IEnumerable<string>, IEnumerator<string>
		{
			private int RxAoyfYzYDsYonLGXsvUgwChukLk;

			private string VqEePGSMyrGKIqkWibsjeHcWPSIx;

			private int wLBvvzlDcbAEPyDfgnprolEZBsgN;

			public UserData TtytLoUfsgUyhsklaKccrnoMiiek;

			private int QSZlWSsqUhpIVZjOleCIDuxHVUuQ;

			public int gPJVdtgfQFNtUzzgsvTbLQPASDBY;

			private IEnumerator<int> hSeONskmQJhVsjVUHGUASYbDGJrU;

			string IEnumerator<string>.Current
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
			public mLmESluXPpdjGmogNKrScYTCSJRJ(int P_0)
			{
				RxAoyfYzYDsYonLGXsvUgwChukLk = P_0;
				wLBvvzlDcbAEPyDfgnprolEZBsgN = Thread.CurrentThread.ManagedThreadId;
			}

			[DebuggerHidden]
			void IDisposable.Dispose()
			{
				int rxAoyfYzYDsYonLGXsvUgwChukLk = RxAoyfYzYDsYonLGXsvUgwChukLk;
				if (rxAoyfYzYDsYonLGXsvUgwChukLk == -3 || rxAoyfYzYDsYonLGXsvUgwChukLk == 1)
				{
					try
					{
					}
					finally
					{
						uvbwjgAeXsWySvtbvsLpuFVuAgJB();
					}
				}
			}

			private bool MoveNext()
			{
				try
				{
					int rxAoyfYzYDsYonLGXsvUgwChukLk = RxAoyfYzYDsYonLGXsvUgwChukLk;
					UserData ttytLoUfsgUyhsklaKccrnoMiiek = TtytLoUfsgUyhsklaKccrnoMiiek;
					switch (rxAoyfYzYDsYonLGXsvUgwChukLk)
					{
					default:
						return false;
					case 0:
						RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
						if (ttytLoUfsgUyhsklaKccrnoMiiek.actionCategories == null || ttytLoUfsgUyhsklaKccrnoMiiek.actions == null)
						{
							return false;
						}
						hSeONskmQJhVsjVUHGUASYbDGJrU = ttytLoUfsgUyhsklaKccrnoMiiek.actionCategoryMap.ActionIdsInCategory(QSZlWSsqUhpIVZjOleCIDuxHVUuQ).GetEnumerator();
						RxAoyfYzYDsYonLGXsvUgwChukLk = -3;
						break;
					case 1:
						RxAoyfYzYDsYonLGXsvUgwChukLk = -3;
						break;
					}
					while (hSeONskmQJhVsjVUHGUASYbDGJrU.MoveNext())
					{
						int current = hSeONskmQJhVsjVUHGUASYbDGJrU.Current;
						InputAction actionById = ttytLoUfsgUyhsklaKccrnoMiiek.GetActionById(current);
						if (actionById != null)
						{
							VqEePGSMyrGKIqkWibsjeHcWPSIx = actionById.descriptiveName;
							RxAoyfYzYDsYonLGXsvUgwChukLk = 1;
							return true;
						}
					}
					uvbwjgAeXsWySvtbvsLpuFVuAgJB();
					hSeONskmQJhVsjVUHGUASYbDGJrU = null;
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

			private void uvbwjgAeXsWySvtbvsLpuFVuAgJB()
			{
				RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
				if (hSeONskmQJhVsjVUHGUASYbDGJrU != null)
				{
					hSeONskmQJhVsjVUHGUASYbDGJrU.Dispose();
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
				mLmESluXPpdjGmogNKrScYTCSJRJ mLmESluXPpdjGmogNKrScYTCSJRJ2;
				if (RxAoyfYzYDsYonLGXsvUgwChukLk == -2 && wLBvvzlDcbAEPyDfgnprolEZBsgN == Thread.CurrentThread.ManagedThreadId)
				{
					RxAoyfYzYDsYonLGXsvUgwChukLk = 0;
					mLmESluXPpdjGmogNKrScYTCSJRJ2 = this;
				}
				else
				{
					mLmESluXPpdjGmogNKrScYTCSJRJ2 = new mLmESluXPpdjGmogNKrScYTCSJRJ(0);
					mLmESluXPpdjGmogNKrScYTCSJRJ2.TtytLoUfsgUyhsklaKccrnoMiiek = TtytLoUfsgUyhsklaKccrnoMiiek;
				}
				mLmESluXPpdjGmogNKrScYTCSJRJ2.QSZlWSsqUhpIVZjOleCIDuxHVUuQ = gPJVdtgfQFNtUzzgsvTbLQPASDBY;
				return mLmESluXPpdjGmogNKrScYTCSJRJ2;
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return ((IEnumerable<string>)this).GetEnumerator();
			}
		}

		private sealed class zPPcWxuVhWibAGKoZnGbEjWUkaZfb : IDisposable, IEnumerable, IEnumerator, IEnumerable<int>, IEnumerator<int>
		{
			private int RxAoyfYzYDsYonLGXsvUgwChukLk;

			private int VqEePGSMyrGKIqkWibsjeHcWPSIx;

			private int wLBvvzlDcbAEPyDfgnprolEZBsgN;

			public UserData TtytLoUfsgUyhsklaKccrnoMiiek;

			private int QSZlWSsqUhpIVZjOleCIDuxHVUuQ;

			public int gPJVdtgfQFNtUzzgsvTbLQPASDBY;

			private IEnumerator<int> hSeONskmQJhVsjVUHGUASYbDGJrU;

			int IEnumerator<int>.Current
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
			public zPPcWxuVhWibAGKoZnGbEjWUkaZfb(int P_0)
			{
				RxAoyfYzYDsYonLGXsvUgwChukLk = P_0;
				wLBvvzlDcbAEPyDfgnprolEZBsgN = Thread.CurrentThread.ManagedThreadId;
			}

			[DebuggerHidden]
			void IDisposable.Dispose()
			{
				int rxAoyfYzYDsYonLGXsvUgwChukLk = RxAoyfYzYDsYonLGXsvUgwChukLk;
				if (rxAoyfYzYDsYonLGXsvUgwChukLk == -3 || rxAoyfYzYDsYonLGXsvUgwChukLk == 1)
				{
					try
					{
					}
					finally
					{
						uvbwjgAeXsWySvtbvsLpuFVuAgJB();
					}
				}
			}

			private bool MoveNext()
			{
				try
				{
					int rxAoyfYzYDsYonLGXsvUgwChukLk = RxAoyfYzYDsYonLGXsvUgwChukLk;
					UserData ttytLoUfsgUyhsklaKccrnoMiiek = TtytLoUfsgUyhsklaKccrnoMiiek;
					switch (rxAoyfYzYDsYonLGXsvUgwChukLk)
					{
					default:
						return false;
					case 0:
						RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
						if (ttytLoUfsgUyhsklaKccrnoMiiek.actionCategories == null || ttytLoUfsgUyhsklaKccrnoMiiek.actions == null)
						{
							return false;
						}
						hSeONskmQJhVsjVUHGUASYbDGJrU = ttytLoUfsgUyhsklaKccrnoMiiek.actionCategoryMap.ActionIdsInCategory(QSZlWSsqUhpIVZjOleCIDuxHVUuQ).GetEnumerator();
						RxAoyfYzYDsYonLGXsvUgwChukLk = -3;
						break;
					case 1:
						RxAoyfYzYDsYonLGXsvUgwChukLk = -3;
						break;
					}
					if (hSeONskmQJhVsjVUHGUASYbDGJrU.MoveNext())
					{
						int current = hSeONskmQJhVsjVUHGUASYbDGJrU.Current;
						VqEePGSMyrGKIqkWibsjeHcWPSIx = current;
						RxAoyfYzYDsYonLGXsvUgwChukLk = 1;
						return true;
					}
					uvbwjgAeXsWySvtbvsLpuFVuAgJB();
					hSeONskmQJhVsjVUHGUASYbDGJrU = null;
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

			private void uvbwjgAeXsWySvtbvsLpuFVuAgJB()
			{
				RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
				if (hSeONskmQJhVsjVUHGUASYbDGJrU != null)
				{
					hSeONskmQJhVsjVUHGUASYbDGJrU.Dispose();
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
				zPPcWxuVhWibAGKoZnGbEjWUkaZfb zPPcWxuVhWibAGKoZnGbEjWUkaZfb2;
				if (RxAoyfYzYDsYonLGXsvUgwChukLk == -2 && wLBvvzlDcbAEPyDfgnprolEZBsgN == Thread.CurrentThread.ManagedThreadId)
				{
					RxAoyfYzYDsYonLGXsvUgwChukLk = 0;
					zPPcWxuVhWibAGKoZnGbEjWUkaZfb2 = this;
				}
				else
				{
					zPPcWxuVhWibAGKoZnGbEjWUkaZfb2 = new zPPcWxuVhWibAGKoZnGbEjWUkaZfb(0);
					zPPcWxuVhWibAGKoZnGbEjWUkaZfb2.TtytLoUfsgUyhsklaKccrnoMiiek = TtytLoUfsgUyhsklaKccrnoMiiek;
				}
				zPPcWxuVhWibAGKoZnGbEjWUkaZfb2.QSZlWSsqUhpIVZjOleCIDuxHVUuQ = gPJVdtgfQFNtUzzgsvTbLQPASDBY;
				return zPPcWxuVhWibAGKoZnGbEjWUkaZfb2;
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return ((IEnumerable<int>)this).GetEnumerator();
			}
		}

		private sealed class IhMyAtLvxPFXTQcouETnuqvgCPOHA : IDisposable, IEnumerable, IEnumerator, IEnumerable<string>, IEnumerator<string>
		{
			private int RxAoyfYzYDsYonLGXsvUgwChukLk;

			private string VqEePGSMyrGKIqkWibsjeHcWPSIx;

			private int wLBvvzlDcbAEPyDfgnprolEZBsgN;

			public UserData TtytLoUfsgUyhsklaKccrnoMiiek;

			private int QSZlWSsqUhpIVZjOleCIDuxHVUuQ;

			public int gPJVdtgfQFNtUzzgsvTbLQPASDBY;

			private IEnumerator<int> hSeONskmQJhVsjVUHGUASYbDGJrU;

			string IEnumerator<string>.Current
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
			public IhMyAtLvxPFXTQcouETnuqvgCPOHA(int P_0)
			{
				RxAoyfYzYDsYonLGXsvUgwChukLk = P_0;
				wLBvvzlDcbAEPyDfgnprolEZBsgN = Thread.CurrentThread.ManagedThreadId;
			}

			[DebuggerHidden]
			void IDisposable.Dispose()
			{
				int rxAoyfYzYDsYonLGXsvUgwChukLk = RxAoyfYzYDsYonLGXsvUgwChukLk;
				if (rxAoyfYzYDsYonLGXsvUgwChukLk == -3 || rxAoyfYzYDsYonLGXsvUgwChukLk == 1)
				{
					try
					{
					}
					finally
					{
						uvbwjgAeXsWySvtbvsLpuFVuAgJB();
					}
				}
			}

			private bool MoveNext()
			{
				try
				{
					int rxAoyfYzYDsYonLGXsvUgwChukLk = RxAoyfYzYDsYonLGXsvUgwChukLk;
					UserData ttytLoUfsgUyhsklaKccrnoMiiek = TtytLoUfsgUyhsklaKccrnoMiiek;
					switch (rxAoyfYzYDsYonLGXsvUgwChukLk)
					{
					default:
						return false;
					case 0:
						RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
						if (ttytLoUfsgUyhsklaKccrnoMiiek.actionCategories == null || ttytLoUfsgUyhsklaKccrnoMiiek.actions == null)
						{
							return false;
						}
						hSeONskmQJhVsjVUHGUASYbDGJrU = ttytLoUfsgUyhsklaKccrnoMiiek.actionCategoryMap.ActionIdsInCategory(QSZlWSsqUhpIVZjOleCIDuxHVUuQ).GetEnumerator();
						RxAoyfYzYDsYonLGXsvUgwChukLk = -3;
						break;
					case 1:
						RxAoyfYzYDsYonLGXsvUgwChukLk = -3;
						break;
					}
					while (hSeONskmQJhVsjVUHGUASYbDGJrU.MoveNext())
					{
						int current = hSeONskmQJhVsjVUHGUASYbDGJrU.Current;
						InputAction actionById = ttytLoUfsgUyhsklaKccrnoMiiek.GetActionById(current);
						if (actionById != null)
						{
							VqEePGSMyrGKIqkWibsjeHcWPSIx = actionById.name;
							RxAoyfYzYDsYonLGXsvUgwChukLk = 1;
							return true;
						}
					}
					uvbwjgAeXsWySvtbvsLpuFVuAgJB();
					hSeONskmQJhVsjVUHGUASYbDGJrU = null;
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

			private void uvbwjgAeXsWySvtbvsLpuFVuAgJB()
			{
				RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
				if (hSeONskmQJhVsjVUHGUASYbDGJrU != null)
				{
					hSeONskmQJhVsjVUHGUASYbDGJrU.Dispose();
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
				IhMyAtLvxPFXTQcouETnuqvgCPOHA ihMyAtLvxPFXTQcouETnuqvgCPOHA;
				if (RxAoyfYzYDsYonLGXsvUgwChukLk == -2 && wLBvvzlDcbAEPyDfgnprolEZBsgN == Thread.CurrentThread.ManagedThreadId)
				{
					RxAoyfYzYDsYonLGXsvUgwChukLk = 0;
					ihMyAtLvxPFXTQcouETnuqvgCPOHA = this;
				}
				else
				{
					ihMyAtLvxPFXTQcouETnuqvgCPOHA = new IhMyAtLvxPFXTQcouETnuqvgCPOHA(0);
					ihMyAtLvxPFXTQcouETnuqvgCPOHA.TtytLoUfsgUyhsklaKccrnoMiiek = TtytLoUfsgUyhsklaKccrnoMiiek;
				}
				ihMyAtLvxPFXTQcouETnuqvgCPOHA.QSZlWSsqUhpIVZjOleCIDuxHVUuQ = gPJVdtgfQFNtUzzgsvTbLQPASDBY;
				return ihMyAtLvxPFXTQcouETnuqvgCPOHA;
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return ((IEnumerable<string>)this).GetEnumerator();
			}
		}

		private sealed class SBhDdjhjzHZUoNLSBifgmfcDFLPfA : IDisposable, IEnumerable, IEnumerator, IEnumerable<InputCategory>, IEnumerator<InputCategory>
		{
			private int RxAoyfYzYDsYonLGXsvUgwChukLk;

			private InputCategory VqEePGSMyrGKIqkWibsjeHcWPSIx;

			private int wLBvvzlDcbAEPyDfgnprolEZBsgN;

			private string eBfDTHTrxuRklzIaThUTCseJJiEeb;

			public string NgUGOCgMAPNuMsxXPWsrQJXFnwujA;

			public UserData TtytLoUfsgUyhsklaKccrnoMiiek;

			private int hWZNbLCFLBBWXNZxiKeGtzgrnTsg;

			InputCategory IEnumerator<InputCategory>.Current
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
			public SBhDdjhjzHZUoNLSBifgmfcDFLPfA(int P_0)
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
				UserData ttytLoUfsgUyhsklaKccrnoMiiek = TtytLoUfsgUyhsklaKccrnoMiiek;
				if (rxAoyfYzYDsYonLGXsvUgwChukLk != 0)
				{
					if (rxAoyfYzYDsYonLGXsvUgwChukLk != 1)
					{
						return false;
					}
					RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
					goto IL_00b3;
				}
				RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
				if (eBfDTHTrxuRklzIaThUTCseJJiEeb == null || eBfDTHTrxuRklzIaThUTCseJJiEeb == string.Empty)
				{
					return false;
				}
				if (ttytLoUfsgUyhsklaKccrnoMiiek.actionCategories == null)
				{
					return false;
				}
				hWZNbLCFLBBWXNZxiKeGtzgrnTsg = 0;
				goto IL_00c3;
				IL_00c3:
				if (hWZNbLCFLBBWXNZxiKeGtzgrnTsg < ttytLoUfsgUyhsklaKccrnoMiiek.actionCategories.Count)
				{
					if (ttytLoUfsgUyhsklaKccrnoMiiek.actionCategories[hWZNbLCFLBBWXNZxiKeGtzgrnTsg].userAssignable && ttytLoUfsgUyhsklaKccrnoMiiek.actionCategories[hWZNbLCFLBBWXNZxiKeGtzgrnTsg].tag.Equals(eBfDTHTrxuRklzIaThUTCseJJiEeb, StringComparison.OrdinalIgnoreCase))
					{
						VqEePGSMyrGKIqkWibsjeHcWPSIx = ttytLoUfsgUyhsklaKccrnoMiiek.actionCategories[hWZNbLCFLBBWXNZxiKeGtzgrnTsg];
						RxAoyfYzYDsYonLGXsvUgwChukLk = 1;
						return true;
					}
					goto IL_00b3;
				}
				return false;
				IL_00b3:
				hWZNbLCFLBBWXNZxiKeGtzgrnTsg++;
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
				SBhDdjhjzHZUoNLSBifgmfcDFLPfA sBhDdjhjzHZUoNLSBifgmfcDFLPfA;
				if (RxAoyfYzYDsYonLGXsvUgwChukLk == -2 && wLBvvzlDcbAEPyDfgnprolEZBsgN == Thread.CurrentThread.ManagedThreadId)
				{
					RxAoyfYzYDsYonLGXsvUgwChukLk = 0;
					sBhDdjhjzHZUoNLSBifgmfcDFLPfA = this;
				}
				else
				{
					sBhDdjhjzHZUoNLSBifgmfcDFLPfA = new SBhDdjhjzHZUoNLSBifgmfcDFLPfA(0);
					sBhDdjhjzHZUoNLSBifgmfcDFLPfA.TtytLoUfsgUyhsklaKccrnoMiiek = TtytLoUfsgUyhsklaKccrnoMiiek;
				}
				sBhDdjhjzHZUoNLSBifgmfcDFLPfA.eBfDTHTrxuRklzIaThUTCseJJiEeb = NgUGOCgMAPNuMsxXPWsrQJXFnwujA;
				return sBhDdjhjzHZUoNLSBifgmfcDFLPfA;
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return ((IEnumerable<InputCategory>)this).GetEnumerator();
			}
		}

		private sealed class KbsTCursMguCYaadCFNodrPYURph : IDisposable, IEnumerable, IEnumerator, IEnumerable<InputAction>, IEnumerator<InputAction>
		{
			private int RxAoyfYzYDsYonLGXsvUgwChukLk;

			private InputAction VqEePGSMyrGKIqkWibsjeHcWPSIx;

			private int wLBvvzlDcbAEPyDfgnprolEZBsgN;

			public UserData TtytLoUfsgUyhsklaKccrnoMiiek;

			private int ibNaRicErZhcmrUtserfrUoMsHIcA;

			public int CcDnwAqYkwKfkzcTgBMageoSxwgTA;

			private bool XNzRTyKnKPLTDsPJvWgNsGsOEcQdA;

			public bool BGTREeWkieZAGmzKkVdsDSoDuesc;

			private InputCategory VnqVBRHojIEbQahfALtKmhBesibXA;

			private IEnumerator<int> tIlurAGjswRzPeLpFSGHcEnzPBQq;

			private int FhIFjZbpbSEntgsVWVgqHGbPqlLqA;

			InputAction IEnumerator<InputAction>.Current
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
			public KbsTCursMguCYaadCFNodrPYURph(int P_0)
			{
				RxAoyfYzYDsYonLGXsvUgwChukLk = P_0;
				wLBvvzlDcbAEPyDfgnprolEZBsgN = Thread.CurrentThread.ManagedThreadId;
			}

			[DebuggerHidden]
			void IDisposable.Dispose()
			{
				int rxAoyfYzYDsYonLGXsvUgwChukLk = RxAoyfYzYDsYonLGXsvUgwChukLk;
				if (rxAoyfYzYDsYonLGXsvUgwChukLk == -3 || rxAoyfYzYDsYonLGXsvUgwChukLk == 1)
				{
					try
					{
					}
					finally
					{
						uvbwjgAeXsWySvtbvsLpuFVuAgJB();
					}
				}
			}

			private bool MoveNext()
			{
				try
				{
					int rxAoyfYzYDsYonLGXsvUgwChukLk = RxAoyfYzYDsYonLGXsvUgwChukLk;
					UserData ttytLoUfsgUyhsklaKccrnoMiiek = TtytLoUfsgUyhsklaKccrnoMiiek;
					InputAction inputAction;
					switch (rxAoyfYzYDsYonLGXsvUgwChukLk)
					{
					default:
						return false;
					case 0:
						RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
						if (ttytLoUfsgUyhsklaKccrnoMiiek.actions == null || ttytLoUfsgUyhsklaKccrnoMiiek.actionCategories == null)
						{
							return false;
						}
						VnqVBRHojIEbQahfALtKmhBesibXA = ttytLoUfsgUyhsklaKccrnoMiiek.GetActionCategoryById(ibNaRicErZhcmrUtserfrUoMsHIcA);
						if (VnqVBRHojIEbQahfALtKmhBesibXA == null || !VnqVBRHojIEbQahfALtKmhBesibXA.userAssignable)
						{
							return false;
						}
						if (XNzRTyKnKPLTDsPJvWgNsGsOEcQdA)
						{
							tIlurAGjswRzPeLpFSGHcEnzPBQq = ttytLoUfsgUyhsklaKccrnoMiiek.SortedActionIdsInCategory(VnqVBRHojIEbQahfALtKmhBesibXA.id).GetEnumerator();
							RxAoyfYzYDsYonLGXsvUgwChukLk = -3;
							goto IL_00e4;
						}
						FhIFjZbpbSEntgsVWVgqHGbPqlLqA = 0;
						goto IL_0165;
					case 1:
						RxAoyfYzYDsYonLGXsvUgwChukLk = -3;
						goto IL_00e4;
					case 2:
						{
							RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
							goto IL_0153;
						}
						IL_00e4:
						while (tIlurAGjswRzPeLpFSGHcEnzPBQq.MoveNext())
						{
							int current = tIlurAGjswRzPeLpFSGHcEnzPBQq.Current;
							InputAction actionById = ttytLoUfsgUyhsklaKccrnoMiiek.GetActionById(current);
							if (actionById != null && actionById.userAssignable)
							{
								VqEePGSMyrGKIqkWibsjeHcWPSIx = actionById;
								RxAoyfYzYDsYonLGXsvUgwChukLk = 1;
								return true;
							}
						}
						uvbwjgAeXsWySvtbvsLpuFVuAgJB();
						tIlurAGjswRzPeLpFSGHcEnzPBQq = null;
						break;
						IL_0153:
						FhIFjZbpbSEntgsVWVgqHGbPqlLqA++;
						goto IL_0165;
						IL_0165:
						if (FhIFjZbpbSEntgsVWVgqHGbPqlLqA >= ttytLoUfsgUyhsklaKccrnoMiiek.actions.Count)
						{
							break;
						}
						inputAction = ttytLoUfsgUyhsklaKccrnoMiiek.actions[FhIFjZbpbSEntgsVWVgqHGbPqlLqA];
						if (inputAction.categoryId == VnqVBRHojIEbQahfALtKmhBesibXA.id && inputAction.userAssignable)
						{
							VqEePGSMyrGKIqkWibsjeHcWPSIx = inputAction;
							RxAoyfYzYDsYonLGXsvUgwChukLk = 2;
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

			private void uvbwjgAeXsWySvtbvsLpuFVuAgJB()
			{
				RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
				if (tIlurAGjswRzPeLpFSGHcEnzPBQq != null)
				{
					tIlurAGjswRzPeLpFSGHcEnzPBQq.Dispose();
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
				KbsTCursMguCYaadCFNodrPYURph kbsTCursMguCYaadCFNodrPYURph;
				if (RxAoyfYzYDsYonLGXsvUgwChukLk == -2 && wLBvvzlDcbAEPyDfgnprolEZBsgN == Thread.CurrentThread.ManagedThreadId)
				{
					RxAoyfYzYDsYonLGXsvUgwChukLk = 0;
					kbsTCursMguCYaadCFNodrPYURph = this;
				}
				else
				{
					kbsTCursMguCYaadCFNodrPYURph = new KbsTCursMguCYaadCFNodrPYURph(0);
					kbsTCursMguCYaadCFNodrPYURph.TtytLoUfsgUyhsklaKccrnoMiiek = TtytLoUfsgUyhsklaKccrnoMiiek;
				}
				kbsTCursMguCYaadCFNodrPYURph.ibNaRicErZhcmrUtserfrUoMsHIcA = CcDnwAqYkwKfkzcTgBMageoSxwgTA;
				kbsTCursMguCYaadCFNodrPYURph.XNzRTyKnKPLTDsPJvWgNsGsOEcQdA = BGTREeWkieZAGmzKkVdsDSoDuesc;
				return kbsTCursMguCYaadCFNodrPYURph;
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return ((IEnumerable<InputAction>)this).GetEnumerator();
			}
		}

		private sealed class LlVfWmrpICNsklPkqASwikGnbsjqA : IDisposable, IEnumerable, IEnumerator, IEnumerable<InputAction>, IEnumerator<InputAction>
		{
			private int RxAoyfYzYDsYonLGXsvUgwChukLk;

			private InputAction VqEePGSMyrGKIqkWibsjeHcWPSIx;

			private int wLBvvzlDcbAEPyDfgnprolEZBsgN;

			public UserData TtytLoUfsgUyhsklaKccrnoMiiek;

			private string xBSfHeTldwbKRMwEYavRXRPDvQcg;

			public string XOJnzrKRhiyiZuhYbGepPZQgtdkC;

			private bool XNzRTyKnKPLTDsPJvWgNsGsOEcQdA;

			public bool BGTREeWkieZAGmzKkVdsDSoDuesc;

			private InputCategory VnqVBRHojIEbQahfALtKmhBesibXA;

			private IEnumerator<int> tIlurAGjswRzPeLpFSGHcEnzPBQq;

			private int FhIFjZbpbSEntgsVWVgqHGbPqlLqA;

			InputAction IEnumerator<InputAction>.Current
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
			public LlVfWmrpICNsklPkqASwikGnbsjqA(int P_0)
			{
				RxAoyfYzYDsYonLGXsvUgwChukLk = P_0;
				wLBvvzlDcbAEPyDfgnprolEZBsgN = Thread.CurrentThread.ManagedThreadId;
			}

			[DebuggerHidden]
			void IDisposable.Dispose()
			{
				int rxAoyfYzYDsYonLGXsvUgwChukLk = RxAoyfYzYDsYonLGXsvUgwChukLk;
				if (rxAoyfYzYDsYonLGXsvUgwChukLk == -3 || rxAoyfYzYDsYonLGXsvUgwChukLk == 1)
				{
					try
					{
					}
					finally
					{
						uvbwjgAeXsWySvtbvsLpuFVuAgJB();
					}
				}
			}

			private bool MoveNext()
			{
				try
				{
					int rxAoyfYzYDsYonLGXsvUgwChukLk = RxAoyfYzYDsYonLGXsvUgwChukLk;
					UserData ttytLoUfsgUyhsklaKccrnoMiiek = TtytLoUfsgUyhsklaKccrnoMiiek;
					InputAction inputAction;
					switch (rxAoyfYzYDsYonLGXsvUgwChukLk)
					{
					default:
						return false;
					case 0:
						RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
						if (ttytLoUfsgUyhsklaKccrnoMiiek.actions == null || ttytLoUfsgUyhsklaKccrnoMiiek.actionCategories == null)
						{
							return false;
						}
						VnqVBRHojIEbQahfALtKmhBesibXA = ttytLoUfsgUyhsklaKccrnoMiiek.GetActionCategory(xBSfHeTldwbKRMwEYavRXRPDvQcg);
						if (VnqVBRHojIEbQahfALtKmhBesibXA == null || !VnqVBRHojIEbQahfALtKmhBesibXA.userAssignable)
						{
							return false;
						}
						if (XNzRTyKnKPLTDsPJvWgNsGsOEcQdA)
						{
							tIlurAGjswRzPeLpFSGHcEnzPBQq = ttytLoUfsgUyhsklaKccrnoMiiek.SortedActionIdsInCategory(VnqVBRHojIEbQahfALtKmhBesibXA.id).GetEnumerator();
							RxAoyfYzYDsYonLGXsvUgwChukLk = -3;
							goto IL_00e4;
						}
						FhIFjZbpbSEntgsVWVgqHGbPqlLqA = 0;
						goto IL_0165;
					case 1:
						RxAoyfYzYDsYonLGXsvUgwChukLk = -3;
						goto IL_00e4;
					case 2:
						{
							RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
							goto IL_0153;
						}
						IL_00e4:
						while (tIlurAGjswRzPeLpFSGHcEnzPBQq.MoveNext())
						{
							int current = tIlurAGjswRzPeLpFSGHcEnzPBQq.Current;
							InputAction actionById = ttytLoUfsgUyhsklaKccrnoMiiek.GetActionById(current);
							if (actionById != null && actionById.userAssignable)
							{
								VqEePGSMyrGKIqkWibsjeHcWPSIx = actionById;
								RxAoyfYzYDsYonLGXsvUgwChukLk = 1;
								return true;
							}
						}
						uvbwjgAeXsWySvtbvsLpuFVuAgJB();
						tIlurAGjswRzPeLpFSGHcEnzPBQq = null;
						break;
						IL_0153:
						FhIFjZbpbSEntgsVWVgqHGbPqlLqA++;
						goto IL_0165;
						IL_0165:
						if (FhIFjZbpbSEntgsVWVgqHGbPqlLqA >= ttytLoUfsgUyhsklaKccrnoMiiek.actions.Count)
						{
							break;
						}
						inputAction = ttytLoUfsgUyhsklaKccrnoMiiek.actions[FhIFjZbpbSEntgsVWVgqHGbPqlLqA];
						if (inputAction.categoryId == VnqVBRHojIEbQahfALtKmhBesibXA.id && inputAction.userAssignable)
						{
							VqEePGSMyrGKIqkWibsjeHcWPSIx = inputAction;
							RxAoyfYzYDsYonLGXsvUgwChukLk = 2;
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

			private void uvbwjgAeXsWySvtbvsLpuFVuAgJB()
			{
				RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
				if (tIlurAGjswRzPeLpFSGHcEnzPBQq != null)
				{
					tIlurAGjswRzPeLpFSGHcEnzPBQq.Dispose();
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
				LlVfWmrpICNsklPkqASwikGnbsjqA llVfWmrpICNsklPkqASwikGnbsjqA;
				if (RxAoyfYzYDsYonLGXsvUgwChukLk == -2 && wLBvvzlDcbAEPyDfgnprolEZBsgN == Thread.CurrentThread.ManagedThreadId)
				{
					RxAoyfYzYDsYonLGXsvUgwChukLk = 0;
					llVfWmrpICNsklPkqASwikGnbsjqA = this;
				}
				else
				{
					llVfWmrpICNsklPkqASwikGnbsjqA = new LlVfWmrpICNsklPkqASwikGnbsjqA(0);
					llVfWmrpICNsklPkqASwikGnbsjqA.TtytLoUfsgUyhsklaKccrnoMiiek = TtytLoUfsgUyhsklaKccrnoMiiek;
				}
				llVfWmrpICNsklPkqASwikGnbsjqA.xBSfHeTldwbKRMwEYavRXRPDvQcg = XOJnzrKRhiyiZuhYbGepPZQgtdkC;
				llVfWmrpICNsklPkqASwikGnbsjqA.XNzRTyKnKPLTDsPJvWgNsGsOEcQdA = BGTREeWkieZAGmzKkVdsDSoDuesc;
				return llVfWmrpICNsklPkqASwikGnbsjqA;
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return ((IEnumerable<InputAction>)this).GetEnumerator();
			}
		}

		private sealed class kFDlCSLINVNxekFKCkTCRrrOgxHW : IDisposable, IEnumerable, IEnumerator, IEnumerable<InputMapCategory>, IEnumerator<InputMapCategory>
		{
			private int RxAoyfYzYDsYonLGXsvUgwChukLk;

			private InputMapCategory VqEePGSMyrGKIqkWibsjeHcWPSIx;

			private int wLBvvzlDcbAEPyDfgnprolEZBsgN;

			private string eBfDTHTrxuRklzIaThUTCseJJiEeb;

			public string NgUGOCgMAPNuMsxXPWsrQJXFnwujA;

			public UserData TtytLoUfsgUyhsklaKccrnoMiiek;

			private int hWZNbLCFLBBWXNZxiKeGtzgrnTsg;

			InputMapCategory IEnumerator<InputMapCategory>.Current
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
			public kFDlCSLINVNxekFKCkTCRrrOgxHW(int P_0)
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
				UserData ttytLoUfsgUyhsklaKccrnoMiiek = TtytLoUfsgUyhsklaKccrnoMiiek;
				if (rxAoyfYzYDsYonLGXsvUgwChukLk != 0)
				{
					if (rxAoyfYzYDsYonLGXsvUgwChukLk != 1)
					{
						return false;
					}
					RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
					goto IL_00b3;
				}
				RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
				if (eBfDTHTrxuRklzIaThUTCseJJiEeb == null || eBfDTHTrxuRklzIaThUTCseJJiEeb == string.Empty)
				{
					return false;
				}
				if (ttytLoUfsgUyhsklaKccrnoMiiek.mapCategories == null)
				{
					return false;
				}
				hWZNbLCFLBBWXNZxiKeGtzgrnTsg = 0;
				goto IL_00c3;
				IL_00c3:
				if (hWZNbLCFLBBWXNZxiKeGtzgrnTsg < ttytLoUfsgUyhsklaKccrnoMiiek.mapCategories.Count)
				{
					if (ttytLoUfsgUyhsklaKccrnoMiiek.mapCategories[hWZNbLCFLBBWXNZxiKeGtzgrnTsg].userAssignable && ttytLoUfsgUyhsklaKccrnoMiiek.mapCategories[hWZNbLCFLBBWXNZxiKeGtzgrnTsg].tag.Equals(eBfDTHTrxuRklzIaThUTCseJJiEeb, StringComparison.OrdinalIgnoreCase))
					{
						VqEePGSMyrGKIqkWibsjeHcWPSIx = ttytLoUfsgUyhsklaKccrnoMiiek.mapCategories[hWZNbLCFLBBWXNZxiKeGtzgrnTsg];
						RxAoyfYzYDsYonLGXsvUgwChukLk = 1;
						return true;
					}
					goto IL_00b3;
				}
				return false;
				IL_00b3:
				hWZNbLCFLBBWXNZxiKeGtzgrnTsg++;
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
				kFDlCSLINVNxekFKCkTCRrrOgxHW kFDlCSLINVNxekFKCkTCRrrOgxHW2;
				if (RxAoyfYzYDsYonLGXsvUgwChukLk == -2 && wLBvvzlDcbAEPyDfgnprolEZBsgN == Thread.CurrentThread.ManagedThreadId)
				{
					RxAoyfYzYDsYonLGXsvUgwChukLk = 0;
					kFDlCSLINVNxekFKCkTCRrrOgxHW2 = this;
				}
				else
				{
					kFDlCSLINVNxekFKCkTCRrrOgxHW2 = new kFDlCSLINVNxekFKCkTCRrrOgxHW(0);
					kFDlCSLINVNxekFKCkTCRrrOgxHW2.TtytLoUfsgUyhsklaKccrnoMiiek = TtytLoUfsgUyhsklaKccrnoMiiek;
				}
				kFDlCSLINVNxekFKCkTCRrrOgxHW2.eBfDTHTrxuRklzIaThUTCseJJiEeb = NgUGOCgMAPNuMsxXPWsrQJXFnwujA;
				return kFDlCSLINVNxekFKCkTCRrrOgxHW2;
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return ((IEnumerable<InputMapCategory>)this).GetEnumerator();
			}
		}

		private sealed class axeIRcicyzTbsVEdDMzHYRcFWUro : IDisposable, IEnumerable, IEnumerator, IEnumerable<InputCategory>, IEnumerator<InputCategory>
		{
			private int RxAoyfYzYDsYonLGXsvUgwChukLk;

			private InputCategory VqEePGSMyrGKIqkWibsjeHcWPSIx;

			private int wLBvvzlDcbAEPyDfgnprolEZBsgN;

			public UserData TtytLoUfsgUyhsklaKccrnoMiiek;

			private int hWZNbLCFLBBWXNZxiKeGtzgrnTsg;

			InputCategory IEnumerator<InputCategory>.Current
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
			public axeIRcicyzTbsVEdDMzHYRcFWUro(int P_0)
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
				UserData ttytLoUfsgUyhsklaKccrnoMiiek = TtytLoUfsgUyhsklaKccrnoMiiek;
				if (rxAoyfYzYDsYonLGXsvUgwChukLk != 0)
				{
					if (rxAoyfYzYDsYonLGXsvUgwChukLk != 1)
					{
						return false;
					}
					RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
					goto IL_0070;
				}
				RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
				if (ttytLoUfsgUyhsklaKccrnoMiiek.actionCategories == null)
				{
					return false;
				}
				hWZNbLCFLBBWXNZxiKeGtzgrnTsg = 0;
				goto IL_0080;
				IL_0080:
				if (hWZNbLCFLBBWXNZxiKeGtzgrnTsg < ttytLoUfsgUyhsklaKccrnoMiiek.actionCategories.Count)
				{
					if (ttytLoUfsgUyhsklaKccrnoMiiek.actionCategories[hWZNbLCFLBBWXNZxiKeGtzgrnTsg].userAssignable)
					{
						VqEePGSMyrGKIqkWibsjeHcWPSIx = ttytLoUfsgUyhsklaKccrnoMiiek.actionCategories[hWZNbLCFLBBWXNZxiKeGtzgrnTsg];
						RxAoyfYzYDsYonLGXsvUgwChukLk = 1;
						return true;
					}
					goto IL_0070;
				}
				return false;
				IL_0070:
				hWZNbLCFLBBWXNZxiKeGtzgrnTsg++;
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
				axeIRcicyzTbsVEdDMzHYRcFWUro axeIRcicyzTbsVEdDMzHYRcFWUro2;
				if (RxAoyfYzYDsYonLGXsvUgwChukLk == -2 && wLBvvzlDcbAEPyDfgnprolEZBsgN == Thread.CurrentThread.ManagedThreadId)
				{
					RxAoyfYzYDsYonLGXsvUgwChukLk = 0;
					axeIRcicyzTbsVEdDMzHYRcFWUro2 = this;
				}
				else
				{
					axeIRcicyzTbsVEdDMzHYRcFWUro2 = new axeIRcicyzTbsVEdDMzHYRcFWUro(0);
					axeIRcicyzTbsVEdDMzHYRcFWUro2.TtytLoUfsgUyhsklaKccrnoMiiek = TtytLoUfsgUyhsklaKccrnoMiiek;
				}
				return axeIRcicyzTbsVEdDMzHYRcFWUro2;
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return ((IEnumerable<InputCategory>)this).GetEnumerator();
			}
		}

		private sealed class fAiLWgIBKGRdSfFiEotvMRRKuoXv : IDisposable, IEnumerable, IEnumerator, IEnumerable<InputAction>, IEnumerator<InputAction>
		{
			private int RxAoyfYzYDsYonLGXsvUgwChukLk;

			private InputAction VqEePGSMyrGKIqkWibsjeHcWPSIx;

			private int wLBvvzlDcbAEPyDfgnprolEZBsgN;

			public UserData TtytLoUfsgUyhsklaKccrnoMiiek;

			private int hWZNbLCFLBBWXNZxiKeGtzgrnTsg;

			InputAction IEnumerator<InputAction>.Current
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
			public fAiLWgIBKGRdSfFiEotvMRRKuoXv(int P_0)
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
				UserData ttytLoUfsgUyhsklaKccrnoMiiek = TtytLoUfsgUyhsklaKccrnoMiiek;
				if (rxAoyfYzYDsYonLGXsvUgwChukLk != 0)
				{
					if (rxAoyfYzYDsYonLGXsvUgwChukLk != 1)
					{
						return false;
					}
					RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
					goto IL_007a;
				}
				RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
				if (ttytLoUfsgUyhsklaKccrnoMiiek.actions == null)
				{
					return false;
				}
				hWZNbLCFLBBWXNZxiKeGtzgrnTsg = 0;
				goto IL_008c;
				IL_008c:
				if (hWZNbLCFLBBWXNZxiKeGtzgrnTsg < ttytLoUfsgUyhsklaKccrnoMiiek.actions.Count)
				{
					InputAction inputAction = ttytLoUfsgUyhsklaKccrnoMiiek.actions[hWZNbLCFLBBWXNZxiKeGtzgrnTsg];
					InputCategory actionCategoryById = ttytLoUfsgUyhsklaKccrnoMiiek.GetActionCategoryById(inputAction.categoryId);
					if (actionCategoryById != null && actionCategoryById.userAssignable && inputAction.userAssignable)
					{
						VqEePGSMyrGKIqkWibsjeHcWPSIx = inputAction;
						RxAoyfYzYDsYonLGXsvUgwChukLk = 1;
						return true;
					}
					goto IL_007a;
				}
				return false;
				IL_007a:
				hWZNbLCFLBBWXNZxiKeGtzgrnTsg++;
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
				fAiLWgIBKGRdSfFiEotvMRRKuoXv fAiLWgIBKGRdSfFiEotvMRRKuoXv2;
				if (RxAoyfYzYDsYonLGXsvUgwChukLk == -2 && wLBvvzlDcbAEPyDfgnprolEZBsgN == Thread.CurrentThread.ManagedThreadId)
				{
					RxAoyfYzYDsYonLGXsvUgwChukLk = 0;
					fAiLWgIBKGRdSfFiEotvMRRKuoXv2 = this;
				}
				else
				{
					fAiLWgIBKGRdSfFiEotvMRRKuoXv2 = new fAiLWgIBKGRdSfFiEotvMRRKuoXv(0);
					fAiLWgIBKGRdSfFiEotvMRRKuoXv2.TtytLoUfsgUyhsklaKccrnoMiiek = TtytLoUfsgUyhsklaKccrnoMiiek;
				}
				return fAiLWgIBKGRdSfFiEotvMRRKuoXv2;
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return ((IEnumerable<InputAction>)this).GetEnumerator();
			}
		}

		private sealed class MbxHDZDOZKwomSmJIkZwAgrIlHnQb : IDisposable, IEnumerable, IEnumerator, IEnumerable<InputMapCategory>, IEnumerator<InputMapCategory>
		{
			private int RxAoyfYzYDsYonLGXsvUgwChukLk;

			private InputMapCategory VqEePGSMyrGKIqkWibsjeHcWPSIx;

			private int wLBvvzlDcbAEPyDfgnprolEZBsgN;

			public UserData TtytLoUfsgUyhsklaKccrnoMiiek;

			private int hWZNbLCFLBBWXNZxiKeGtzgrnTsg;

			InputMapCategory IEnumerator<InputMapCategory>.Current
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
			public MbxHDZDOZKwomSmJIkZwAgrIlHnQb(int P_0)
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
				UserData ttytLoUfsgUyhsklaKccrnoMiiek = TtytLoUfsgUyhsklaKccrnoMiiek;
				if (rxAoyfYzYDsYonLGXsvUgwChukLk != 0)
				{
					if (rxAoyfYzYDsYonLGXsvUgwChukLk != 1)
					{
						return false;
					}
					RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
					goto IL_0070;
				}
				RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
				if (ttytLoUfsgUyhsklaKccrnoMiiek.mapCategories == null)
				{
					return false;
				}
				hWZNbLCFLBBWXNZxiKeGtzgrnTsg = 0;
				goto IL_0080;
				IL_0080:
				if (hWZNbLCFLBBWXNZxiKeGtzgrnTsg < ttytLoUfsgUyhsklaKccrnoMiiek.mapCategories.Count)
				{
					if (ttytLoUfsgUyhsklaKccrnoMiiek.mapCategories[hWZNbLCFLBBWXNZxiKeGtzgrnTsg].userAssignable)
					{
						VqEePGSMyrGKIqkWibsjeHcWPSIx = ttytLoUfsgUyhsklaKccrnoMiiek.mapCategories[hWZNbLCFLBBWXNZxiKeGtzgrnTsg];
						RxAoyfYzYDsYonLGXsvUgwChukLk = 1;
						return true;
					}
					goto IL_0070;
				}
				return false;
				IL_0070:
				hWZNbLCFLBBWXNZxiKeGtzgrnTsg++;
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
				MbxHDZDOZKwomSmJIkZwAgrIlHnQb mbxHDZDOZKwomSmJIkZwAgrIlHnQb;
				if (RxAoyfYzYDsYonLGXsvUgwChukLk == -2 && wLBvvzlDcbAEPyDfgnprolEZBsgN == Thread.CurrentThread.ManagedThreadId)
				{
					RxAoyfYzYDsYonLGXsvUgwChukLk = 0;
					mbxHDZDOZKwomSmJIkZwAgrIlHnQb = this;
				}
				else
				{
					mbxHDZDOZKwomSmJIkZwAgrIlHnQb = new MbxHDZDOZKwomSmJIkZwAgrIlHnQb(0);
					mbxHDZDOZKwomSmJIkZwAgrIlHnQb.TtytLoUfsgUyhsklaKccrnoMiiek = TtytLoUfsgUyhsklaKccrnoMiiek;
				}
				return mbxHDZDOZKwomSmJIkZwAgrIlHnQb;
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return ((IEnumerable<InputMapCategory>)this).GetEnumerator();
			}
		}

		[CustomObfuscation(rename = false)]
		[SerializeField]
		private ConfigVars configVars = new ConfigVars();

		[CustomObfuscation(rename = false)]
		[SerializeField]
		private List<Player_Editor> players = new List<Player_Editor>();

		[CustomObfuscation(rename = false)]
		[SerializeField]
		private List<InputAction> actions = new List<InputAction>();

		[CustomObfuscation(rename = false)]
		[SerializeField]
		private List<InputCategory> actionCategories = new List<InputCategory>();

		[CustomObfuscation(rename = false)]
		[SerializeField]
		private ActionCategoryMap actionCategoryMap = new ActionCategoryMap();

		[CustomObfuscation(rename = false)]
		[SerializeField]
		private List<InputBehavior> inputBehaviors = new List<InputBehavior>();

		[CustomObfuscation(rename = false)]
		[SerializeField]
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

		[CustomObfuscation(rename = false)]
		[SerializeField]
		private List<InputLayout> customControllerLayouts = new List<InputLayout>();

		[SerializeField]
		[CustomObfuscation(rename = false)]
		private List<ControllerMap_Editor> joystickMaps = new List<ControllerMap_Editor>();

		[SerializeField]
		[CustomObfuscation(rename = false)]
		private List<ControllerMap_Editor> keyboardMaps = new List<ControllerMap_Editor>();

		[CustomObfuscation(rename = false)]
		[SerializeField]
		private List<ControllerMap_Editor> mouseMaps = new List<ControllerMap_Editor>();

		[CustomObfuscation(rename = false)]
		[SerializeField]
		private List<ControllerMap_Editor> customControllerMaps = new List<ControllerMap_Editor>();

		[SerializeField]
		[CustomObfuscation(rename = false)]
		private List<CustomController_Editor> customControllers = new List<CustomController_Editor>();

		[CustomObfuscation(rename = false)]
		[SerializeField]
		private List<ControllerMapLayoutManager_RuleSet_Editor> controllerMapLayoutManagerRuleSets = new List<ControllerMapLayoutManager_RuleSet_Editor>();

		[CustomObfuscation(rename = false)]
		[SerializeField]
		private List<ControllerMapEnabler_RuleSet_Editor> controllerMapEnablerRuleSets = new List<ControllerMapEnabler_RuleSet_Editor>();

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

		[CustomObfuscation(rename = false)]
		[SerializeField]
		private int playerIdCounter;

		[SerializeField]
		[CustomObfuscation(rename = false)]
		private int actionIdCounter;

		[CustomObfuscation(rename = false)]
		[SerializeField]
		private int actionCategoryIdCounter;

		[SerializeField]
		[CustomObfuscation(rename = false)]
		private int inputBehaviorIdCounter;

		[SerializeField]
		[CustomObfuscation(rename = false)]
		private int mapCategoryIdCounter;

		[CustomObfuscation(rename = false)]
		[SerializeField]
		private int joystickLayoutIdCounter;

		[CustomObfuscation(rename = false)]
		[SerializeField]
		private int keyboardLayoutIdCounter;

		[CustomObfuscation(rename = false)]
		[SerializeField]
		private int mouseLayoutIdCounter;

		[SerializeField]
		[CustomObfuscation(rename = false)]
		private int customControllerLayoutIdCounter;

		[SerializeField]
		[CustomObfuscation(rename = false)]
		private int joystickMapIdCounter;

		[CustomObfuscation(rename = false)]
		[SerializeField]
		private int keyboardMapIdCounter;

		[SerializeField]
		[CustomObfuscation(rename = false)]
		private int mouseMapIdCounter;

		[CustomObfuscation(rename = false)]
		[SerializeField]
		private int customControllerMapIdCounter;

		[CustomObfuscation(rename = false)]
		[SerializeField]
		private int customControllerIdCounter;

		[SerializeField]
		[CustomObfuscation(rename = false)]
		private int controllerMapLayoutManagerSetIdCounter;

		[CustomObfuscation(rename = false)]
		[SerializeField]
		private int controllerMapEnablerSetIdCounter;

		private Func<int, bool> containsActionDelegate;

		internal IList<Player_Editor> WEZyqeTcxBEnozDVUGnNzVZBqwqJ
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

		internal IList<InputAction> DWsKafRrWuSEidWiEOtBUufWRMvE
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

		internal IList<InputCategory> OCPXiYawvnhuDUIpVLMqzhQlYhSf
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

		internal IList<InputBehavior> RVEoxRFIWCOfUOAtdCUZelrXyliLA
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

		internal IList<InputMapCategory> RSXfoCPlVhAqzWjiPGWRnMihRjmd
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

		internal IList<InputLayout> vyeJQWQbdYUeKBxeVafPMcqiYtME
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

		internal IList<InputLayout> KxMHtHeQmhHNKLltHJceXACHrZpV
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

		internal IList<InputLayout> ZyzyetomWfCQRSNHBxYMQdjdhWAs
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

		internal IList<InputLayout> lkOlJCNOJlpoOygUHGghIOtFqrdrA
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

		internal IList<ControllerMap_Editor> oVBpaVHekggsMPeeOvqTNITfkdmp
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

		internal IList<ControllerMap_Editor> KzWcIwNDUyfosHCIaTaKiSbSDfOeA
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

		internal IList<ControllerMap_Editor> GRwyNcOPlDqQRQrdDDPSVVJPnRf
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

		internal IList<ControllerMap_Editor> VkncxRQfYSaEaEbytuVbkKnmbmS
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

		internal IList<ControllerMapLayoutManager_RuleSet_Editor> RcBlWYzGshhboJbPPjNPpwWDPiyJ
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

		internal IList<ControllerMapEnabler_RuleSet_Editor> TdxyTDiiuadjoYWQlYaoJTOQWbXq
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

		internal IEnumerable<InputMapCategory> pLKbazjnyHBagGHAVFxumHJDFxgQ => new MbxHDZDOZKwomSmJIkZwAgrIlHnQb(-2)
		{
			TtytLoUfsgUyhsklaKccrnoMiiek = this
		};

		internal IEnumerable<InputCategory> eeieCjFPvCkMvhgikkHPegWBZQERe => new axeIRcicyzTbsVEdDMzHYRcFWUro(-2)
		{
			TtytLoUfsgUyhsklaKccrnoMiiek = this
		};

		internal IEnumerable<InputAction> uokisPQPtevGpzdvwIlJxaBPFzbB => new fAiLWgIBKGRdSfFiEotvMRRKuoXv(-2)
		{
			TtytLoUfsgUyhsklaKccrnoMiiek = this
		};

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

		internal IEnumerable<InputMapCategory> tNIcUmxLngoAzMHyIqmvCwvRNGJc(string P_0)
		{
			return new ibgHUDbSbKDfxabNuFrtdODeuNaIc(-2)
			{
				TtytLoUfsgUyhsklaKccrnoMiiek = this,
				NgUGOCgMAPNuMsxXPWsrQJXFnwujA = P_0
			};
		}

		internal IEnumerable<InputMapCategory> LLEhlxTnovXrgJamrJPyyoxmBYYx(string P_0)
		{
			return new kFDlCSLINVNxekFKCkTCRrrOgxHW(-2)
			{
				TtytLoUfsgUyhsklaKccrnoMiiek = this,
				NgUGOCgMAPNuMsxXPWsrQJXFnwujA = P_0
			};
		}

		internal IEnumerable<InputCategory> FtbyKBcUriukmsLuMgkIjnVAwSxfA(string P_0)
		{
			return new hCOcgraSglEiyycZfLLVTBKruhjA(-2)
			{
				TtytLoUfsgUyhsklaKccrnoMiiek = this,
				NgUGOCgMAPNuMsxXPWsrQJXFnwujA = P_0
			};
		}

		internal IEnumerable<InputCategory> nvCnollClWWQVpygolJAIGrAvCqc(string P_0)
		{
			return new SBhDdjhjzHZUoNLSBifgmfcDFLPfA(-2)
			{
				TtytLoUfsgUyhsklaKccrnoMiiek = this,
				NgUGOCgMAPNuMsxXPWsrQJXFnwujA = P_0
			};
		}

		internal IEnumerable<InputAction> YFWdcTWVATPjnOxbSRpOHEWQCkKEA(int P_0, bool P_1)
		{
			return new GMLMKfxErfWoCSEfIbGUXoGNoQxs(-2)
			{
				TtytLoUfsgUyhsklaKccrnoMiiek = this,
				CcDnwAqYkwKfkzcTgBMageoSxwgTA = P_0,
				BGTREeWkieZAGmzKkVdsDSoDuesc = P_1
			};
		}

		internal IEnumerable<InputAction> YFWdcTWVATPjnOxbSRpOHEWQCkKEA(string P_0, bool P_1)
		{
			return new LHlQrpnsouKHQOGqkNgrJyVIHaVK(-2)
			{
				TtytLoUfsgUyhsklaKccrnoMiiek = this,
				IVdVQqSTIUohUJHKKUYEEfPwfuzz = P_0,
				BGTREeWkieZAGmzKkVdsDSoDuesc = P_1
			};
		}

		internal IEnumerable<InputAction> HDlCkRPcvrfiGagWJebaOEdJgObk(string P_0)
		{
			return new qFsuJfMnHXtpnYENKqUwoHFrQyRu(-2)
			{
				TtytLoUfsgUyhsklaKccrnoMiiek = this,
				NgUGOCgMAPNuMsxXPWsrQJXFnwujA = P_0
			};
		}

		internal IEnumerable<InputAction> pYcfKZCKtilaPxDoEnkWooQHxgInA(int P_0, bool P_1)
		{
			return new KbsTCursMguCYaadCFNodrPYURph(-2)
			{
				TtytLoUfsgUyhsklaKccrnoMiiek = this,
				CcDnwAqYkwKfkzcTgBMageoSxwgTA = P_0,
				BGTREeWkieZAGmzKkVdsDSoDuesc = P_1
			};
		}

		internal IEnumerable<InputAction> pYcfKZCKtilaPxDoEnkWooQHxgInA(string P_0, bool P_1)
		{
			return new LlVfWmrpICNsklPkqASwikGnbsjqA(-2)
			{
				TtytLoUfsgUyhsklaKccrnoMiiek = this,
				XOJnzrKRhiyiZuhYbGepPZQgtdkC = P_0,
				BGTREeWkieZAGmzKkVdsDSoDuesc = P_1
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
				Player_Editor player_Editor = rfjBGFgnwBIyzMgqsDlOXRkxpHqv();
				player_Editor.name = "System";
				player_Editor.descriptiveName = player_Editor.name;
				player_Editor.id = 9999999;
				player_Editor.startPlaying = true;
				player_Editor.assignMouseOnStart = true;
				player_Editor.assignKeyboardOnStart = true;
				player_Editor.excludeFromControllerAutoAssignment = true;
				players.Add(player_Editor);
				InputCategory inputCategory = ssfYlGfYZqucyiHUQyfYbwyOSsfJ();
				inputCategory.name = "Default";
				inputCategory.descriptiveName = inputCategory.name;
				actionCategories.Add(inputCategory);
				actionCategoryMap.AddCategory(inputCategory.id);
				InputBehavior inputBehavior = JcZBhvMPQoAENynhxATxfMjqORvbb();
				inputBehavior.name = "Default";
				inputBehaviors.Add(inputBehavior);
				InputMapCategory inputMapCategory = LNsEzmacGXvBzSmHJrtLNSdWWbRk();
				inputMapCategory.name = "Default";
				inputMapCategory.descriptiveName = inputMapCategory.name;
				mapCategories.Add(inputMapCategory);
				InputLayout inputLayout = OEYDMDKIzvRluEGyUMPGLIatobNW();
				inputLayout.name = "Default";
				inputLayout.descriptiveName = inputLayout.name;
				joystickLayouts.Add(inputLayout);
				InputLayout inputLayout2 = teniefsctgkBYUcKVVWUmqRivJxU();
				inputLayout2.name = "Default";
				inputLayout2.descriptiveName = inputLayout2.name;
				keyboardLayouts.Add(inputLayout2);
				InputLayout inputLayout3 = LAqnTSpgvufrsjJNQoxBOPIXpBLI();
				inputLayout3.name = "Default";
				inputLayout3.descriptiveName = inputLayout3.name;
				mouseLayouts.Add(inputLayout3);
				InputLayout inputLayout4 = MjstfjdRqWWgdFVvKTCYiIFcSrUI();
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
		}

		public List<InputAction> GetActions_Copy()
		{
			List<InputAction> list = new List<InputAction>();
			for (int i = 0; i < actions.Count; i++)
			{
				list.Add(actions[i]);
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
				KeyboardMap item = keyboardMaps[i].fVMGDVEmClqGYHcOHaiLwzvRTLToA(containsActionDelegate);
				list.Add(item);
			}
			return list;
		}

		public List<MouseMap> GetMouseMaps_Copy()
		{
			List<MouseMap> list = new List<MouseMap>();
			for (int i = 0; i < mouseMaps.Count; i++)
			{
				MouseMap item = mouseMaps[i].ihWEtXcbQdlsHGWThRgSBwvYKbLwb(containsActionDelegate);
				list.Add(item);
			}
			return list;
		}

		public void AddPlayer()
		{
			players.Add(rfjBGFgnwBIyzMgqsDlOXRkxpHqv());
		}

		public void InsertPlayer(int index)
		{
			if (index < 0 || index >= players.Count)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			players.Insert(index, rfjBGFgnwBIyzMgqsDlOXRkxpHqv());
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
			InputAction inputAction = LAWxGABKJglXttNvxfAwFGRIBEpL();
			inputAction.categoryId = categoryId;
			actions.Add(inputAction);
			actionCategoryMap.AddAction(categoryId, inputAction.id);
		}

		public void InsertAction(int categoryId, int actionId)
		{
			if (actions != null)
			{
				InputAction inputAction = LAWxGABKJglXttNvxfAwFGRIBEpL();
				inputAction.categoryId = categoryId;
				actions.Add(inputAction);
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
					actions.RemoveAt(num);
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
			if (num == actions.Count - 1)
			{
				actions.Add(inputAction);
				actionCategoryMap.AddAction(categoryId, inputAction.id);
				return actions.Count - 1;
			}
			actions.Insert(num + 1, inputAction);
			int num2 = actionCategoryMap.IndexOfAction(categoryId, actionId);
			actionCategoryMap.InsertAction(categoryId, inputAction.id, num2 + 1);
			return num + 1;
		}

		private int MGLMBeBwwGBGTkLPVxhMXYmVZWmL(int P_0, InputAction P_1)
		{
			if (IndexOfActionCategory(P_0) < 0)
			{
				return -1;
			}
			InputAction inputAction = P_1.Clone();
			inputAction.id = GetNewActionId();
			inputAction.name = StringTools.IterateName(inputAction.name, -1, GetActionNames());
			actions.Add(inputAction);
			return actions.Count - 1;
		}

		public string[] GetActionNames()
		{
			if (actions == null)
			{
				return null;
			}
			string[] array = new string[actions.Count];
			for (int i = 0; i < actions.Count; i++)
			{
				array[i] = actions[i].name;
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
			if (actions == null)
			{
				return 0;
			}
			for (int i = 0; i < actions.Count; i++)
			{
				results.Add(actions[i].name);
			}
			return results.Count;
		}

		public int[] GetActionIds()
		{
			if (actions == null)
			{
				return null;
			}
			int[] array = new int[actions.Count];
			for (int i = 0; i < actions.Count; i++)
			{
				array[i] = actions[i].id;
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
			if (actions == null)
			{
				return 0;
			}
			for (int i = 0; i < actions.Count; i++)
			{
				results.Add(actions[i].id);
			}
			return results.Count;
		}

		public string GetActionNameById(int id)
		{
			if (actions == null)
			{
				return string.Empty;
			}
			for (int i = 0; i < actions.Count; i++)
			{
				if (actions[i].id == id)
				{
					return actions[i].name;
				}
			}
			return string.Empty;
		}

		public InputAction GetAction(int index)
		{
			if (actions == null || index < 0 || index >= actions.Count)
			{
				return null;
			}
			return actions[index];
		}

		public InputAction GetAction(string name)
		{
			if (actions == null)
			{
				return null;
			}
			int num = IndexOfAction(name);
			if (num < 0)
			{
				return null;
			}
			return actions[num];
		}

		public InputAction GetActionById(int id)
		{
			if (actions == null)
			{
				return null;
			}
			for (int i = 0; i < actions.Count; i++)
			{
				if (actions[i].id == id)
				{
					return actions[i];
				}
			}
			return null;
		}

		public int GetActionId(string name)
		{
			if (actions == null)
			{
				return -1;
			}
			int num = IndexOfAction(name);
			if (num < 0)
			{
				return -1;
			}
			return actions[num].id;
		}

		public string[] GetSortedActionNamesInCategory(int id)
		{
			if (actionCategories == null || actions == null)
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

		public IEnumerable<string> SortedActionNamesInCategory(int id)
		{
			return new IhMyAtLvxPFXTQcouETnuqvgCPOHA(-2)
			{
				TtytLoUfsgUyhsklaKccrnoMiiek = this,
				gPJVdtgfQFNtUzzgsvTbLQPASDBY = id
			};
		}

		public string[] GetSortedActionDescriptiveNamesInCategory(int id)
		{
			if (actionCategories == null || actions == null)
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

		public IEnumerable<string> SortedActionDescriptiveNamesInCategory(int id)
		{
			return new mLmESluXPpdjGmogNKrScYTCSJRJ(-2)
			{
				TtytLoUfsgUyhsklaKccrnoMiiek = this,
				gPJVdtgfQFNtUzzgsvTbLQPASDBY = id
			};
		}

		public int[] GetSortedActionIdsInCategory(int id)
		{
			if (actionCategories == null || actions == null)
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

		public IEnumerable<int> SortedActionIdsInCategory(int id)
		{
			return new zPPcWxuVhWibAGKoZnGbEjWUkaZfb(-2)
			{
				TtytLoUfsgUyhsklaKccrnoMiiek = this,
				gPJVdtgfQFNtUzzgsvTbLQPASDBY = id
			};
		}

		public bool ContainsAction(int id)
		{
			return IndexOfAction(id) >= 0;
		}

		public int IndexOfAction(int id)
		{
			if (actions == null)
			{
				return -1;
			}
			for (int i = 0; i < actions.Count; i++)
			{
				if (actions[i].id == id)
				{
					return i;
				}
			}
			return -1;
		}

		public int IndexOfAction(string name)
		{
			if (actions == null)
			{
				return -1;
			}
			if (name == null || name == string.Empty)
			{
				return -1;
			}
			for (int i = 0; i < actions.Count; i++)
			{
				if (actions[i].name.Equals(name, StringComparison.OrdinalIgnoreCase))
				{
					return i;
				}
			}
			return -1;
		}

		public void AddActionCategory()
		{
			InputCategory inputCategory = ssfYlGfYZqucyiHUQyfYbwyOSsfJ();
			actionCategories.Add(inputCategory);
			actionCategoryMap.AddCategory(inputCategory.id);
		}

		public void InsertActionCategory(int index)
		{
			if (index < 0 || index >= actionCategories.Count)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			InputCategory inputCategory = ssfYlGfYZqucyiHUQyfYbwyOSsfJ();
			actionCategories.Insert(index, inputCategory);
			actionCategoryMap.AddCategory(inputCategory.id);
		}

		public void DeleteActionCategory(int index)
		{
			if (actionCategories == null || index < 0 || index >= actionCategories.Count)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			int id = actionCategories[index].id;
			actionCategoryMap.RemoveCategory(id);
			if (actions != null)
			{
				for (int num = actions.Count - 1; num >= 0; num--)
				{
					if (actions[num].categoryId == id)
					{
						actions.RemoveAt(num);
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
			InputCategory inputCategory = new InputCategory(actionCategories[index]);
			inputCategory.id = GetNewActionCategoryId();
			inputCategory.name = StringTools.IterateName(inputCategory.name, -1, GetActionCategoryNames());
			if (index == actionCategories.Count - 1)
			{
				actionCategories.Add(inputCategory);
			}
			else
			{
				actionCategories.Insert(index + 1, inputCategory);
			}
			actionCategoryMap.AddCategory(inputCategory.id);
			if (!duplicateActions || actions == null)
			{
				return;
			}
			int id = inputCategory.id;
			int id2 = actionCategories[index].id;
			List<int> list = new List<int>();
			for (int i = 0; i < actions.Count; i++)
			{
				if (actions[i].categoryId == id2)
				{
					list.Add(i);
				}
			}
			Dictionary<int, int> dictionary = new Dictionary<int, int>(list.Count);
			for (int j = 0; j < list.Count; j++)
			{
				InputAction inputAction = actions[list[j]];
				int num = MGLMBeBwwGBGTkLPVxhMXYmVZWmL(id2, inputAction);
				if (num >= 0)
				{
					InputAction inputAction2 = actions[num];
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
			if (num >= 0 && actions[num].categoryId != newCategoryId)
			{
				actionCategoryMap.ChangeCategory(actionId, newCategoryId);
				actions[num].categoryId = newCategoryId;
			}
		}

		public int GetActionCategoryCount(int id)
		{
			if (actionCategories == null)
			{
				return 0;
			}
			int num = 0;
			if (actions != null)
			{
				for (int i = 0; i < actions.Count; i++)
				{
					if (actions[i].categoryId == id)
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
			inputBehaviors.Add(JcZBhvMPQoAENynhxATxfMjqORvbb());
		}

		public void InsertInputBehavior(int index)
		{
			if (index < 0 || index >= inputBehaviors.Count)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			inputBehaviors.Insert(index, JcZBhvMPQoAENynhxATxfMjqORvbb());
		}

		public void DeleteInputBehavior(int index)
		{
			if (inputBehaviors == null || index < 0 || index >= inputBehaviors.Count)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			int id = inputBehaviors[index].id;
			if (actions != null)
			{
				for (int i = 0; i < actions.Count; i++)
				{
					if (actions[i].behaviorId == id)
					{
						actions[i].behaviorId = 0;
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
			mapCategories.Add(LNsEzmacGXvBzSmHJrtLNSdWWbRk());
		}

		public void InsertMapCategory(int index)
		{
			if (index < 0 || index >= mapCategories.Count)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			mapCategories.Insert(index, LNsEzmacGXvBzSmHJrtLNSdWWbRk());
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
				Action<List<Player_Editor.Mapping>, int> action = tEXZZFhMsnLkGSeIaOnLZEnxfkok._003C_003E9.ChvEXcEOrAyMqdtCxcLSnlKpyhZH;
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
			joystickLayouts.Add(OEYDMDKIzvRluEGyUMPGLIatobNW());
		}

		public void InsertJoystickLayout(int index)
		{
			if (index < 0 || index >= joystickLayouts.Count)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			joystickLayouts.Insert(index, OEYDMDKIzvRluEGyUMPGLIatobNW());
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
				Action<List<Player_Editor.Mapping>, int> action = tEXZZFhMsnLkGSeIaOnLZEnxfkok._003C_003E9.jtSIrkhNmyHRPevtDPEevgLDfvRR;
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
			keyboardLayouts.Add(teniefsctgkBYUcKVVWUmqRivJxU());
		}

		public void InsertKeyboardLayout(int index)
		{
			if (index < 0 || index >= keyboardLayouts.Count)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			keyboardLayouts.Insert(index, teniefsctgkBYUcKVVWUmqRivJxU());
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
				Action<List<Player_Editor.Mapping>, int> action = tEXZZFhMsnLkGSeIaOnLZEnxfkok._003C_003E9.LbSnifsTtbVCOOLdOueiUThKVdah;
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
			mouseLayouts.Add(LAqnTSpgvufrsjJNQoxBOPIXpBLI());
		}

		public void InsertMouseLayout(int index)
		{
			if (index < 0 || index >= mouseLayouts.Count)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			mouseLayouts.Insert(index, LAqnTSpgvufrsjJNQoxBOPIXpBLI());
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
				Action<List<Player_Editor.Mapping>, int> action = tEXZZFhMsnLkGSeIaOnLZEnxfkok._003C_003E9.NmDWOyxMfaFuPToNlxaFWdyiJJBu;
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
			customControllerLayouts.Add(MjstfjdRqWWgdFVvKTCYiIFcSrUI());
		}

		public void InsertCustomControllerLayout(int index)
		{
			if (index < 0 || index >= customControllerLayouts.Count)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			customControllerLayouts.Insert(index, MjstfjdRqWWgdFVvKTCYiIFcSrUI());
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
				Action<List<Player_Editor.Mapping>, int> action = tEXZZFhMsnLkGSeIaOnLZEnxfkok._003C_003E9.JnKUCgfFGNBqXjkxRZDYsvhVpMVi;
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

		internal ControllerMap joPhuTnAZZbFFbZAKsHmYkOQMBQJ(Controller P_0, int P_1, int P_2)
		{
			if (P_0 == null)
			{
				return null;
			}
			return P_0.type switch
			{
				ControllerType.Joystick => WHOFmQewBIbguHOagLmODlVoDgCP((Joystick)P_0, P_1, P_2), 
				ControllerType.Keyboard => FindKeyboardMap_Game((Keyboard)P_0, P_1, P_2), 
				ControllerType.Mouse => FindMouseMap_Game((Mouse)P_0, P_1, P_2), 
				ControllerType.Custom => vrRCeAkEWTOCEPPnLUEetfzUZqDnA(P_1, ((CustomController)P_0).sourceControllerId, P_2), 
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

		internal JoystickMap rSuwQVxddIUsjiemBrsQiuZXYnhL(HardwareControllerMapIdentifier P_0, int P_1, int P_2)
		{
			return WHOFmQewBIbguHOagLmODlVoDgCP(new HardwareControllerMapIdentifier(P_0.guid, P_0.inputSource, P_0.actualInputPlatform, P_0.variantIndex), P_1, P_2);
		}

		internal JoystickMap WHOFmQewBIbguHOagLmODlVoDgCP(Joystick P_0, int P_1, int P_2)
		{
			if (P_0 == null)
			{
				return null;
			}
			return WHOFmQewBIbguHOagLmODlVoDgCP(P_0.BBOiFMbIkLuQbioSkwqyhykKdIjtA, P_1, P_2);
		}

		private JoystickMap WHOFmQewBIbguHOagLmODlVoDgCP(HardwareControllerMapIdentifier P_0, int P_1, int P_2)
		{
			Guid guid = P_0.guid;
			HardwareJoystickMap hardwareJoystickMap = ReInput.fdqXOouyCFlzjkPTPiIErxwlIHye(guid);
			ControllerMap_Editor controllerMap_Editor = GicvtXOzYfPiIQjekoXAOlJnhtFo(P_1, guid, P_2, false);
			if (controllerMap_Editor != null)
			{
				JoystickMap joystickMap = controllerMap_Editor.cnkdOWmHBfKszdwDwvUpVLxVNZSI(containsActionDelegate, P_0, hardwareJoystickMap, controllerMap_Editor.hardwareGuid == ReInput.defaultHardwareJoystickMapGuid);
				joystickMap.OPieIRwFQUNEFaYLpKUnHAyPmaUs(guid, P_1, P_2);
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
					HardwareJoystickTemplateMap hardwareJoystickTemplateMap = ReInput.rfsEypEpDSCojIvJWQqUMoXAhWHO(templateGuid);
					if (!(hardwareJoystickTemplateMap != null))
					{
						continue;
					}
					controllerMap_Editor = GicvtXOzYfPiIQjekoXAOlJnhtFo(P_1, templateGuid, P_2, false);
					if (controllerMap_Editor != null)
					{
						JoystickMap joystickMap = GkwpeKLvVzTvvpDzTmUexQiplAix(P_0, controllerMap_Editor, hardwareJoystickTemplateMap, hardwareJoystickMap, P_1, P_2);
						if (joystickMap != null)
						{
							joystickMap.OPieIRwFQUNEFaYLpKUnHAyPmaUs(guid, P_1, P_2);
							return joystickMap;
						}
					}
				}
			}
			if (guid == Guid.Empty || 1 == 0)
			{
				controllerMap_Editor = GicvtXOzYfPiIQjekoXAOlJnhtFo(P_1, Guid.Empty, P_2, false);
				if (controllerMap_Editor != null)
				{
					JoystickMap joystickMap = controllerMap_Editor.cnkdOWmHBfKszdwDwvUpVLxVNZSI(containsActionDelegate, P_0, null, controllerMap_Editor.hardwareGuid == ReInput.defaultHardwareJoystickMapGuid);
					joystickMap.OPieIRwFQUNEFaYLpKUnHAyPmaUs(guid, P_1, P_2);
					if (joystickMap != null)
					{
						return joystickMap;
					}
				}
			}
			return JoystickMap.JGdwEkTXDISoEvRBhOWRtTDetjzA(guid, P_1, P_2);
		}

		private ControllerMap_Editor GicvtXOzYfPiIQjekoXAOlJnhtFo(int P_0, Guid P_1, int P_2, bool P_3)
		{
			ControllerMap_Editor joystickMap = GetJoystickMap(P_0, P_1, P_2);
			if (joystickMap != null)
			{
				return joystickMap;
			}
			if (P_3)
			{
				joystickMap = VmCyAPqEkUDsVeBMULiEXLzmBFBI(P_0, P_1, P_2);
				if (joystickMap != null)
				{
					return joystickMap;
				}
			}
			return null;
		}

		private ControllerMap_Editor VmCyAPqEkUDsVeBMULiEXLzmBFBI(int P_0, Guid P_1, int P_2)
		{
			List<ControllerMap_Editor> list = GetJoystickMaps(P_1);
			if (list != null && list.Count > 0)
			{
				HXcIKBniuQkzBrQjQBAESnzYZyHj(list, joystickLayouts);
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

		private JoystickMap GkwpeKLvVzTvvpDzTmUexQiplAix(HardwareControllerMapIdentifier P_0, ControllerMap_Editor P_1, HardwareJoystickTemplateMap P_2, HardwareJoystickMap P_3, int P_4, int P_5)
		{
			if (P_2 == null)
			{
				return null;
			}
			ControllerMap_Editor controllerMap_Editor = P_1.Clone();
			if (!P_2.FnsmXdiZXlVuhjyKGKBoTnaakMhL(controllerMap_Editor, P_3, P_0.guid, out var text))
			{
				Logger.LogError("Error remapping joystick template " + P_2.Guid.ToString() + " to joystick " + P_0.guid.ToString() + "\nReason: " + text);
				return null;
			}
			return controllerMap_Editor.cnkdOWmHBfKszdwDwvUpVLxVNZSI(containsActionDelegate, P_0, P_3, controllerMap_Editor.hardwareGuid == ReInput.defaultHardwareJoystickMapGuid);
		}

		private JoystickMap rLDGdWyEYnIcRBMsEMnrWWXpwKUi(JoystickMap P_0, HardwareControllerMapIdentifier P_1)
		{
			if (P_0 == null)
			{
				return null;
			}
			HardwareJoystickMap hardwareJoystickMap = ReInput.fdqXOouyCFlzjkPTPiIErxwlIHye(P_0.hardwareGuid);
			if (hardwareJoystickMap == null)
			{
				return null;
			}
			HardwareJoystickMap hardwareJoystickMap2 = ReInput.fdqXOouyCFlzjkPTPiIErxwlIHye(Guid.Empty);
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
					string name = elementIdentifier.name;
					if (!string.IsNullOrEmpty(name))
					{
						int num = 0;
						int num2 = name.IndexOf("button", 0, StringComparison.OrdinalIgnoreCase);
						if (num2 < 0)
						{
							num2 = name.IndexOf("axis", 0, StringComparison.OrdinalIgnoreCase);
							num = 1;
						}
						if (num2 >= 0 && (num != 0 || buttons != null) && (num != 1 || axes != null))
						{
							string text = Regex.Replace(name, "[^0-9]+", "");
							Logger.Log(text);
							if (int.TryParse(text, out var result))
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
				list.Add(allMap.YVGDeWQAhUWKAOSPoxXuizkXaiTI);
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
			ControllerMap_Editor controllerMap_Editor = tELbqlnbcsWnxWqxmozcIVChOqWQ(keyboardMaps, keyboardLayouts, categoryId, layoutId, false);
			KeyboardMap keyboardMap;
			if (controllerMap_Editor != null)
			{
				keyboardMap = controllerMap_Editor.fVMGDVEmClqGYHcOHaiLwzvRTLToA(containsActionDelegate);
				keyboardMap.OPieIRwFQUNEFaYLpKUnHAyPmaUs(keyboard.fMxZVPLmyEupjctdQIaGgbDJdlvHA, categoryId, layoutId);
			}
			else
			{
				keyboardMap = KeyboardMap.JGdwEkTXDISoEvRBhOWRtTDetjzA(keyboard.fMxZVPLmyEupjctdQIaGgbDJdlvHA, categoryId, layoutId);
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
			ControllerMap_Editor controllerMap_Editor = tELbqlnbcsWnxWqxmozcIVChOqWQ(mouseMaps, mouseLayouts, categoryId, layoutId, false);
			MouseMap mouseMap;
			if (controllerMap_Editor != null)
			{
				mouseMap = controllerMap_Editor.ihWEtXcbQdlsHGWThRgSBwvYKbLwb(containsActionDelegate);
				mouseMap.OPieIRwFQUNEFaYLpKUnHAyPmaUs(mouse.fMxZVPLmyEupjctdQIaGgbDJdlvHA, categoryId, layoutId);
			}
			else
			{
				mouseMap = MouseMap.JGdwEkTXDISoEvRBhOWRtTDetjzA(mouse.fMxZVPLmyEupjctdQIaGgbDJdlvHA, categoryId, layoutId);
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

		internal CustomControllerMap vrRCeAkEWTOCEPPnLUEetfzUZqDnA(Guid P_0, int P_1, int P_2)
		{
			return vrRCeAkEWTOCEPPnLUEetfzUZqDnA(GetCustomControllerByHardwareTypeGuid(P_0), P_1, P_2);
		}

		internal CustomControllerMap vrRCeAkEWTOCEPPnLUEetfzUZqDnA(int P_0, int P_1, int P_2)
		{
			return vrRCeAkEWTOCEPPnLUEetfzUZqDnA(GetCustomControllerById(P_1), P_0, P_2);
		}

		private CustomControllerMap vrRCeAkEWTOCEPPnLUEetfzUZqDnA(CustomController_Editor P_0, int P_1, int P_2)
		{
			if (P_0 == null)
			{
				return null;
			}
			int id = P_0.id;
			ControllerMap_Editor controllerMap_Editor = DhQAgNyllLhYRUtAktYRBCUcxIAw(P_1, id, P_2, false);
			if (controllerMap_Editor != null)
			{
				CustomControllerMap customControllerMap = controllerMap_Editor.QgZjZLSarbBkWpacWDcovadPAkKi(ContainsAction, P_0);
				customControllerMap.OPieIRwFQUNEFaYLpKUnHAyPmaUs(P_0.typeGuid, id, P_1, P_2);
				return customControllerMap;
			}
			CustomControllerMap customControllerMap2 = CustomControllerMap.JGdwEkTXDISoEvRBhOWRtTDetjzA(P_0.typeGuid, id, P_1, P_2);
			customControllerMap2.OPieIRwFQUNEFaYLpKUnHAyPmaUs(P_0.typeGuid, id, P_1, P_2);
			return customControllerMap2;
		}

		private ControllerMap_Editor DhQAgNyllLhYRUtAktYRBCUcxIAw(int P_0, int P_1, int P_2, bool P_3)
		{
			ControllerMap_Editor customControllerMap = GetCustomControllerMap(P_0, P_1, P_2);
			if (customControllerMap != null)
			{
				return customControllerMap;
			}
			if (P_3)
			{
				customControllerMap = lEfZaVHnukGTvQJkeGTNLEHZlEXh(P_0, P_1, P_2);
				if (customControllerMap != null)
				{
					return customControllerMap;
				}
			}
			return null;
		}

		private ControllerMap_Editor lEfZaVHnukGTvQJkeGTNLEHZlEXh(int P_0, int P_1, int P_2)
		{
			List<ControllerMap_Editor> list = GetCustomControllerMaps(P_1);
			if (list != null && list.Count > 0)
			{
				HXcIKBniuQkzBrQjQBAESnzYZyHj(list, customControllerLayouts);
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

		internal ControllerTemplateMap NkbAmvqRrkSDcURXfqJuMaxmUEaE(Guid P_0, int P_1, int P_2)
		{
			return GetJoystickMap(P_1, P_0, P_2)?.nLmdNYRQIpjhrFVmXAfEyjhKKdgyA();
		}

		public void AddCustomController()
		{
			if (customControllers == null)
			{
				customControllers = new List<CustomController_Editor>();
			}
			customControllers.Add(GYbDbqIdsamcgbSKLwXmJdWcOYDHA());
		}

		public void InsertCustomController(int index)
		{
			if (customControllers == null)
			{
				customControllers = new List<CustomController_Editor>();
			}
			if (index < 0 || index >= customControllers.Count)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			customControllers.Insert(index, GYbDbqIdsamcgbSKLwXmJdWcOYDHA());
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

		public void DuplicateCustomController(int index, bool duplicateMaps)
		{
			if (customControllers == null || index < 0 || index >= customControllers.Count)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			CustomController_Editor customController_Editor = customControllers[index].Clone();
			customController_Editor.id = GetNewCustomControllerId();
			customController_Editor.typeGuid = Guid.NewGuid();
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
			controllerMapLayoutManagerRuleSets.Add(MMVlyZtsHHAtmTMJsbvqeUcnObFEb());
		}

		public void InsertControllerMapLayoutManagerRuleSet(int index)
		{
			if (index < 0 || index >= controllerMapLayoutManagerRuleSets.Count)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			controllerMapLayoutManagerRuleSets.Insert(index, MMVlyZtsHHAtmTMJsbvqeUcnObFEb());
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
			controllerMapEnablerRuleSets.Add(BfTgrSiSHuSMVoNAFcXEpkbJgSDe());
		}

		public void InsertControllerMapEnablerRuleSet(int index)
		{
			if (index < 0 || index >= controllerMapEnablerRuleSets.Count)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			controllerMapEnablerRuleSets.Insert(index, BfTgrSiSHuSMVoNAFcXEpkbJgSDe());
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

		private Player_Editor rfjBGFgnwBIyzMgqsDlOXRkxpHqv()
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

		private InputAction LAWxGABKJglXttNvxfAwFGRIBEpL()
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

		private InputCategory ssfYlGfYZqucyiHUQyfYbwyOSsfJ()
		{
			InputCategory obj = new InputCategory
			{
				id = GetNewActionCategoryId(),
				name = StringTools.IterateName("Category", -1, GetActionCategoryNames())
			};
			obj.descriptiveName = obj.name;
			obj.userAssignable = true;
			return obj;
		}

		private InputBehavior JcZBhvMPQoAENynhxATxfMjqORvbb()
		{
			return new InputBehavior
			{
				id = GetNewInputBehaviorId(),
				name = StringTools.IterateName("Behavior", -1, GetInputBehaviorNames()),
				digitalAxisSimulation = true,
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

		private InputMapCategory LNsEzmacGXvBzSmHJrtLNSdWWbRk()
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

		private InputLayout OEYDMDKIzvRluEGyUMPGLIatobNW()
		{
			InputLayout obj = new InputLayout
			{
				id = GetNewJoystickLayoutId(),
				name = StringTools.IterateName("Layout", -1, GetJoystickLayoutNames())
			};
			obj.descriptiveName = obj.name;
			return obj;
		}

		private InputLayout teniefsctgkBYUcKVVWUmqRivJxU()
		{
			InputLayout obj = new InputLayout
			{
				id = GetNewKeyboardLayoutId(),
				name = StringTools.IterateName("Layout", -1, GetKeyboardLayoutNames())
			};
			obj.descriptiveName = obj.name;
			return obj;
		}

		private InputLayout LAqnTSpgvufrsjJNQoxBOPIXpBLI()
		{
			InputLayout obj = new InputLayout
			{
				id = GetNewMouseLayoutId(),
				name = StringTools.IterateName("Layout", -1, GetMouseLayoutNames())
			};
			obj.descriptiveName = obj.name;
			return obj;
		}

		private InputLayout MjstfjdRqWWgdFVvKTCYiIFcSrUI()
		{
			InputLayout obj = new InputLayout
			{
				id = GetNewCustomControllerLayoutId(),
				name = StringTools.IterateName("Layout", -1, GetCustomControllerLayoutNames())
			};
			obj.descriptiveName = obj.name;
			return obj;
		}

		private CustomController_Editor GYbDbqIdsamcgbSKLwXmJdWcOYDHA()
		{
			CustomController_Editor obj = new CustomController_Editor
			{
				id = GetNewCustomControllerId(),
				typeGuid = Guid.NewGuid(),
				name = StringTools.IterateName("CustomController", -1, GetCustomControllerNames())
			};
			obj.descriptiveName = obj.name;
			return obj;
		}

		private ControllerMapLayoutManager_RuleSet_Editor MMVlyZtsHHAtmTMJsbvqeUcnObFEb()
		{
			return new ControllerMapLayoutManager_RuleSet_Editor
			{
				id = GetNewControllerMapLayoutManagerRuleSetId(),
				name = StringTools.IterateName("RuleSet", -1, GetControllerMapLayoutManagerRuleSetNames())
			};
		}

		private ControllerMapEnabler_RuleSet_Editor BfTgrSiSHuSMVoNAFcXEpkbJgSDe()
		{
			return new ControllerMapEnabler_RuleSet_Editor
			{
				id = GetNewControllerMapEnablerRuleSetId(),
				name = StringTools.IterateName("RuleSet", -1, GetControllerMapEnablerRuleSetNames())
			};
		}

		private ControllerMap_Editor BMTGOeJXPHnLQfiibQcLFfrAAGFZA(List<ControllerMap_Editor> P_0, int P_1, int P_2)
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

		private ControllerMap_Editor tELbqlnbcsWnxWqxmozcIVChOqWQ(List<ControllerMap_Editor> P_0, List<InputLayout> P_1, int P_2, int P_3, bool P_4)
		{
			ControllerMap_Editor controllerMap_Editor = BMTGOeJXPHnLQfiibQcLFfrAAGFZA(P_0, P_2, P_3);
			if (controllerMap_Editor != null)
			{
				return controllerMap_Editor;
			}
			if (P_4)
			{
				controllerMap_Editor = QqqRMVwMyWJEIzUbUqLUrZzpltQC(P_0, P_1, P_2, P_3);
				if (controllerMap_Editor != null)
				{
					return controllerMap_Editor;
				}
			}
			return null;
		}

		private ControllerMap_Editor QqqRMVwMyWJEIzUbUqLUrZzpltQC(List<ControllerMap_Editor> P_0, List<InputLayout> P_1, int P_2, int P_3)
		{
			List<ControllerMap_Editor> list = ListTools.ShallowCopy(P_0);
			if (list != null && list.Count > 0)
			{
				HXcIKBniuQkzBrQjQBAESnzYZyHj(list, P_1);
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

		private void HXcIKBniuQkzBrQjQBAESnzYZyHj(List<ControllerMap_Editor> P_0, List<InputLayout> P_1)
		{
			sxkDCdJWVEdObPfcoGRkqMMfezFXA sxkDCdJWVEdObPfcoGRkqMMfezFXA2 = new sxkDCdJWVEdObPfcoGRkqMMfezFXA();
			sxkDCdJWVEdObPfcoGRkqMMfezFXA2.SzSTUANKpBjMimPpLexrwwqIUDUj = P_1;
			if (P_0 != null && sxkDCdJWVEdObPfcoGRkqMMfezFXA2.SzSTUANKpBjMimPpLexrwwqIUDUj != null)
			{
				P_0.Sort(sxkDCdJWVEdObPfcoGRkqMMfezFXA2.tmqarDMHdPacYIQiuQpsHaSyZvPpA);
			}
		}

		internal void zQQfvDZMmpVqPPLYlLuSJXXpwJcI()
		{
			WEZyqeTcxBEnozDVUGnNzVZBqwqJ = new ReadOnlyCollection<Player_Editor>(players);
			DWsKafRrWuSEidWiEOtBUufWRMvE = new ReadOnlyCollection<InputAction>(actions);
			OCPXiYawvnhuDUIpVLMqzhQlYhSf = new ReadOnlyCollection<InputCategory>(actionCategories);
			RVEoxRFIWCOfUOAtdCUZelrXyliLA = new ReadOnlyCollection<InputBehavior>(inputBehaviors);
			RSXfoCPlVhAqzWjiPGWRnMihRjmd = new ReadOnlyCollection<InputMapCategory>(mapCategories);
			vyeJQWQbdYUeKBxeVafPMcqiYtME = new ReadOnlyCollection<InputLayout>(joystickLayouts);
			KxMHtHeQmhHNKLltHJceXACHrZpV = new ReadOnlyCollection<InputLayout>(keyboardLayouts);
			ZyzyetomWfCQRSNHBxYMQdjdhWAs = new ReadOnlyCollection<InputLayout>(mouseLayouts);
			lkOlJCNOJlpoOygUHGghIOtFqrdrA = new ReadOnlyCollection<InputLayout>(customControllerLayouts);
			oVBpaVHekggsMPeeOvqTNITfkdmp = new ReadOnlyCollection<ControllerMap_Editor>(joystickMaps);
			KzWcIwNDUyfosHCIaTaKiSbSDfOeA = new ReadOnlyCollection<ControllerMap_Editor>(keyboardMaps);
			GRwyNcOPlDqQRQrdDDPSVVJPnRf = new ReadOnlyCollection<ControllerMap_Editor>(mouseMaps);
			VkncxRQfYSaEaEbytuVbkKnmbmS = new ReadOnlyCollection<ControllerMap_Editor>(customControllerMaps);
			RcBlWYzGshhboJbPPjNPpwWDPiyJ = new ReadOnlyCollection<ControllerMapLayoutManager_RuleSet_Editor>(controllerMapLayoutManagerRuleSets);
			TdxyTDiiuadjoYWQlYaoJTOQWbXq = new ReadOnlyCollection<ControllerMapEnabler_RuleSet_Editor>(controllerMapEnablerRuleSets);
			if (mapCategories != null)
			{
				for (int i = 0; i < mapCategories.Count; i++)
				{
					mapCategories[i].zQQfvDZMmpVqPPLYlLuSJXXpwJcI();
				}
			}
			containsActionDelegate = ContainsAction;
		}

		[CustomObfuscation(rename = false)]
		internal static UserData Merge(UserData orig, UserData other, bool preserveOrigIds)
		{
			return zErewWsElUgBaKXLzTOENJhkptHEb.oGPVXKkaRuOYcNaLJIZFXXgWIxtx(orig, other, preserveOrigIds);
		}

		[CustomObfuscation(rename = false)]
		internal static UserData Compact(UserData orig)
		{
			return zErewWsElUgBaKXLzTOENJhkptHEb.oGPVXKkaRuOYcNaLJIZFXXgWIxtx(orig, null, false);
		}
	}
}
