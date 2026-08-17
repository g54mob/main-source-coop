using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Rewired.Data.Mapping;
using Rewired.Interfaces;
using Rewired.Utils;
using Rewired.Utils.Classes.Data;
using UnityEngine;

namespace Rewired
{
	public abstract class ControllerTemplate : IControllerTemplate
	{
		internal abstract class tWsRepKMnZBPynxcHEOrqJwJrtqi : IControllerTemplateElement, IControllerTemplateElement_Internal
		{
			private readonly IControllerTemplate afWhZEYTbMFjMUeulVkAPjICyxpP;

			private readonly int YVGDeWQAhUWKAOSPoxXuizkXaiTI;

			private readonly string hEJFspqWqeBOdedbqHNRpPNEbfBG;

			private readonly ControllerTemplateElementType TIpqQbLCpFnyVWkQInNfNRXErYkK;

			protected readonly int QajDFFaomlkLHzaostfWYpGUioys;

			public int id
			{
				get
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return -1;
					}
					return YVGDeWQAhUWKAOSPoxXuizkXaiTI;
				}
			}

			public string descriptiveName
			{
				get
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return null;
					}
					return hEJFspqWqeBOdedbqHNRpPNEbfBG;
				}
			}

			public ControllerTemplateElementType type
			{
				get
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return ControllerTemplateElementType.Axis;
					}
					return TIpqQbLCpFnyVWkQInNfNRXErYkK;
				}
			}

			public IControllerTemplate parent => afWhZEYTbMFjMUeulVkAPjICyxpP;

			public abstract int elementCount { get; }

			public abstract IControllerTemplateElementSource source { get; }

			public abstract bool exists { get; }

			protected tWsRepKMnZBPynxcHEOrqJwJrtqi(IControllerTemplate P_0, int P_1, string P_2, ControllerTemplateElementType P_3)
			{
				if (P_0 == null)
				{
					throw new ArgumentNullException("parent");
				}
				afWhZEYTbMFjMUeulVkAPjICyxpP = P_0;
				YVGDeWQAhUWKAOSPoxXuizkXaiTI = P_1;
				hEJFspqWqeBOdedbqHNRpPNEbfBG = P_2;
				TIpqQbLCpFnyVWkQInNfNRXErYkK = P_3;
				QajDFFaomlkLHzaostfWYpGUioys = ReInput.id;
			}

			public abstract IControllerTemplateElement GetElement(int index);

			public abstract int GetElementTargets(ControllerElementTarget find, ref IList<ControllerTemplateElementTarget> list);
		}

		internal abstract class bDpsuBEObmlAmAiaSwlxSBIiPRrE : tWsRepKMnZBPynxcHEOrqJwJrtqi
		{
			protected readonly int uflBDILlxYOcIlCXbGnzgzAUpqtj;

			protected readonly AckvZZpMMZJEeXiTnzpZGygPjFpk[] mLceIVIgGsVFwNSYjUnnnJkOqsgBA;

			bool tWsRepKMnZBPynxcHEOrqJwJrtqi.exists
			{
				get
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return false;
					}
					if (mLceIVIgGsVFwNSYjUnnnJkOqsgBA == null)
					{
						return false;
					}
					for (int i = 0; i < mLceIVIgGsVFwNSYjUnnnJkOqsgBA.Length; i++)
					{
						if (mLceIVIgGsVFwNSYjUnnnJkOqsgBA[i].IeBiSvPSlvevRBzqkYknNspTYxmIA != null)
						{
							return true;
						}
					}
					return false;
				}
			}

			protected bDpsuBEObmlAmAiaSwlxSBIiPRrE(IControllerTemplate P_0, int P_1, string P_2, ControllerTemplateElementType P_3, IList<AckvZZpMMZJEeXiTnzpZGygPjFpk> P_4)
				: base(P_0, P_1, P_2, P_3)
			{
				mLceIVIgGsVFwNSYjUnnnJkOqsgBA = ((P_4 != null) ? ListTools.ToArray(P_4) : null);
				uflBDILlxYOcIlCXbGnzgzAUpqtj = ((mLceIVIgGsVFwNSYjUnnnJkOqsgBA != null) ? mLceIVIgGsVFwNSYjUnnnJkOqsgBA.Length : 0);
			}
		}

		internal abstract class xplOfuRdmqGAkFpxdfKgUEIduiHT : bDpsuBEObmlAmAiaSwlxSBIiPRrE, IControllerTemplateElement, IControllerTemplateButton, IControllerTemplateAxis
		{
			private HxKJTjnYRTlXmgqyhXzejFNHdCai WdbNWWRsjxFdwKePDCTZQcFZsADA;

			private string sXTfsIlBXgXZPLOKJteRVBexfODV;

			private string qEHEGyASthJfEnPlUcWyGSdtXxUdA;

			public float brHdwUJAbVcsilgLZZabrTqmRcFR
			{
				get
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return 0f;
					}
					if (uflBDILlxYOcIlCXbGnzgzAUpqtj == 1)
					{
						return mLceIVIgGsVFwNSYjUnnnJkOqsgBA[0].brHdwUJAbVcsilgLZZabrTqmRcFR;
					}
					if (uflBDILlxYOcIlCXbGnzgzAUpqtj == 2)
					{
						float num = mLceIVIgGsVFwNSYjUnnnJkOqsgBA[0].brHdwUJAbVcsilgLZZabrTqmRcFR;
						float num2 = mLceIVIgGsVFwNSYjUnnnJkOqsgBA[1].brHdwUJAbVcsilgLZZabrTqmRcFR;
						return MathTools.Clamp(num + num2, -1f, 1f);
					}
					return 0f;
				}
			}

			public float rEhPDLHiMGJDkTLyTdjOJSGOONIl
			{
				get
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return 0f;
					}
					if (uflBDILlxYOcIlCXbGnzgzAUpqtj == 1)
					{
						return mLceIVIgGsVFwNSYjUnnnJkOqsgBA[0].rEhPDLHiMGJDkTLyTdjOJSGOONIl;
					}
					if (uflBDILlxYOcIlCXbGnzgzAUpqtj == 2)
					{
						float num = mLceIVIgGsVFwNSYjUnnnJkOqsgBA[0].rEhPDLHiMGJDkTLyTdjOJSGOONIl;
						float num2 = mLceIVIgGsVFwNSYjUnnnJkOqsgBA[1].rEhPDLHiMGJDkTLyTdjOJSGOONIl;
						return MathTools.Clamp(num + num2, -1f, 1f);
					}
					return 0f;
				}
			}

			public bool vTrlgouSJGtOvawjunaiRTKrzwsD
			{
				get
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return false;
					}
					if (uflBDILlxYOcIlCXbGnzgzAUpqtj == 1)
					{
						return mLceIVIgGsVFwNSYjUnnnJkOqsgBA[0].vTrlgouSJGtOvawjunaiRTKrzwsD;
					}
					if (uflBDILlxYOcIlCXbGnzgzAUpqtj == 2)
					{
						if (!mLceIVIgGsVFwNSYjUnnnJkOqsgBA[0].vTrlgouSJGtOvawjunaiRTKrzwsD)
						{
							return mLceIVIgGsVFwNSYjUnnnJkOqsgBA[1].vTrlgouSJGtOvawjunaiRTKrzwsD;
						}
						return true;
					}
					return false;
				}
			}

			public bool fDZzkobccRjlJoXBBUdrauDEaisJ
			{
				get
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return false;
					}
					if (uflBDILlxYOcIlCXbGnzgzAUpqtj == 1)
					{
						return mLceIVIgGsVFwNSYjUnnnJkOqsgBA[0].fDZzkobccRjlJoXBBUdrauDEaisJ;
					}
					if (uflBDILlxYOcIlCXbGnzgzAUpqtj == 2)
					{
						if (!mLceIVIgGsVFwNSYjUnnnJkOqsgBA[0].fDZzkobccRjlJoXBBUdrauDEaisJ)
						{
							return mLceIVIgGsVFwNSYjUnnnJkOqsgBA[1].fDZzkobccRjlJoXBBUdrauDEaisJ;
						}
						return true;
					}
					return false;
				}
			}

			string IControllerTemplateAxis.positiveDescriptiveName
			{
				get
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return null;
					}
					return sXTfsIlBXgXZPLOKJteRVBexfODV;
				}
			}

			string IControllerTemplateAxis.negativeDescriptiveName
			{
				get
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return null;
					}
					return qEHEGyASthJfEnPlUcWyGSdtXxUdA;
				}
			}

			float IControllerTemplateAxis.value
			{
				get
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return 0f;
					}
					return brHdwUJAbVcsilgLZZabrTqmRcFR;
				}
			}

			float IControllerTemplateAxis.valuePrev
			{
				get
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return 0f;
					}
					return rEhPDLHiMGJDkTLyTdjOJSGOONIl;
				}
			}

			IControllerTemplateAxisSource IControllerTemplateAxis.source
			{
				get
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return null;
					}
					return WdbNWWRsjxFdwKePDCTZQcFZsADA;
				}
			}

			bool IControllerTemplateButton.value
			{
				get
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return false;
					}
					return vTrlgouSJGtOvawjunaiRTKrzwsD;
				}
			}

			bool IControllerTemplateButton.valuePrev
			{
				get
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return false;
					}
					return fDZzkobccRjlJoXBBUdrauDEaisJ;
				}
			}

			bool IControllerTemplateButton.justPressed
			{
				get
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return false;
					}
					if (uflBDILlxYOcIlCXbGnzgzAUpqtj == 1)
					{
						return mLceIVIgGsVFwNSYjUnnnJkOqsgBA[0].zJkMPGtMddueMcJDyMsXdhcqorQl;
					}
					if (uflBDILlxYOcIlCXbGnzgzAUpqtj == 2)
					{
						if (!mLceIVIgGsVFwNSYjUnnnJkOqsgBA[0].zJkMPGtMddueMcJDyMsXdhcqorQl || mLceIVIgGsVFwNSYjUnnnJkOqsgBA[1].fDZzkobccRjlJoXBBUdrauDEaisJ)
						{
							if (mLceIVIgGsVFwNSYjUnnnJkOqsgBA[1].zJkMPGtMddueMcJDyMsXdhcqorQl)
							{
								return !mLceIVIgGsVFwNSYjUnnnJkOqsgBA[0].fDZzkobccRjlJoXBBUdrauDEaisJ;
							}
							return false;
						}
						return true;
					}
					return false;
				}
			}

			bool IControllerTemplateButton.justReleased
			{
				get
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return false;
					}
					if (uflBDILlxYOcIlCXbGnzgzAUpqtj == 1)
					{
						return mLceIVIgGsVFwNSYjUnnnJkOqsgBA[0].uoMLTbBPDCNDEQdFdJvyeBicfQGhA;
					}
					if (uflBDILlxYOcIlCXbGnzgzAUpqtj == 2)
					{
						if (!mLceIVIgGsVFwNSYjUnnnJkOqsgBA[0].uoMLTbBPDCNDEQdFdJvyeBicfQGhA || mLceIVIgGsVFwNSYjUnnnJkOqsgBA[1].vTrlgouSJGtOvawjunaiRTKrzwsD)
						{
							if (mLceIVIgGsVFwNSYjUnnnJkOqsgBA[1].uoMLTbBPDCNDEQdFdJvyeBicfQGhA)
							{
								return !mLceIVIgGsVFwNSYjUnnnJkOqsgBA[0].vTrlgouSJGtOvawjunaiRTKrzwsD;
							}
							return false;
						}
						return true;
					}
					return false;
				}
			}

			bool IControllerTemplateButton.justChangedState
			{
				get
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return false;
					}
					return vTrlgouSJGtOvawjunaiRTKrzwsD != fDZzkobccRjlJoXBBUdrauDEaisJ;
				}
			}

			float IControllerTemplateButton.pressure
			{
				get
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return 0f;
					}
					return brHdwUJAbVcsilgLZZabrTqmRcFR;
				}
			}

			float IControllerTemplateButton.pressurePrev
			{
				get
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return 0f;
					}
					return rEhPDLHiMGJDkTLyTdjOJSGOONIl;
				}
			}

			IControllerTemplateButtonSource IControllerTemplateButton.source
			{
				get
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return null;
					}
					return WdbNWWRsjxFdwKePDCTZQcFZsADA;
				}
			}

			public override IControllerTemplateElementSource source
			{
				get
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return null;
					}
					return WdbNWWRsjxFdwKePDCTZQcFZsADA;
				}
			}

			public override int elementCount => 0;

			public IControllerTemplateAxis AsAxis
			{
				get
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return null;
					}
					return this;
				}
			}

			public IControllerTemplateButton AsButton
			{
				get
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return null;
					}
					return this;
				}
			}

			protected xplOfuRdmqGAkFpxdfKgUEIduiHT(IControllerTemplate P_0, int P_1, string P_2, string P_3, string P_4, ControllerTemplateElementType P_5, HxKJTjnYRTlXmgqyhXzejFNHdCai P_6, IList<AckvZZpMMZJEeXiTnzpZGygPjFpk> P_7)
				: base(P_0, P_1, P_2, P_5, P_7)
			{
				if (P_7 != null && P_7.Count > 2)
				{
					throw new ArgumentOutOfRangeException("sourceElements.Count must be <= 2.");
				}
				if (P_6 == null)
				{
					throw new ArgumentNullException("target");
				}
				WdbNWWRsjxFdwKePDCTZQcFZsADA = P_6;
				sXTfsIlBXgXZPLOKJteRVBexfODV = P_3;
				qEHEGyASthJfEnPlUcWyGSdtXxUdA = P_4;
			}

			string IControllerTemplateAxis.GetDescriptiveName(AxisRange axisRange)
			{
				if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
				{
					ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
					return null;
				}
				return axisRange switch
				{
					AxisRange.Full => base.descriptiveName, 
					AxisRange.Positive => sXTfsIlBXgXZPLOKJteRVBexfODV, 
					AxisRange.Negative => qEHEGyASthJfEnPlUcWyGSdtXxUdA, 
					_ => throw new NotImplementedException(), 
				};
			}

			public override IControllerTemplateElement GetElement(int index)
			{
				return null;
			}

			public override int GetElementTargets(ControllerElementTarget find, ref IList<ControllerTemplateElementTarget> list)
			{
				if (find.elementIdentifierId < 0)
				{
					return 0;
				}
				int num = 0;
				switch (base.type)
				{
				case ControllerTemplateElementType.Axis:
				{
					IControllerTemplateAxisSource wdbNWWRsjxFdwKePDCTZQcFZsADA = WdbNWWRsjxFdwKePDCTZQcFZsADA;
					if (wdbNWWRsjxFdwKePDCTZQcFZsADA.splitAxis)
					{
						if (HeYIANgmGmbuIDCCgMyGfxvlhNCCB(find, wdbNWWRsjxFdwKePDCTZQcFZsADA.positiveTarget))
						{
							ListTools.AddAndCreateList(ref list, new ControllerTemplateElementTarget(this, AxisRange.Positive));
							num++;
						}
						if (HeYIANgmGmbuIDCCgMyGfxvlhNCCB(find, wdbNWWRsjxFdwKePDCTZQcFZsADA.negativeTarget))
						{
							ListTools.AddAndCreateList(ref list, new ControllerTemplateElementTarget(this, AxisRange.Negative));
							num++;
						}
					}
					else if (HeYIANgmGmbuIDCCgMyGfxvlhNCCB(find, wdbNWWRsjxFdwKePDCTZQcFZsADA.fullTarget))
					{
						ListTools.AddAndCreateList(ref list, new ControllerTemplateElementTarget(this, find.axisRange));
						num++;
					}
					break;
				}
				case ControllerTemplateElementType.Button:
					if (HeYIANgmGmbuIDCCgMyGfxvlhNCCB(find, ((IControllerTemplateButtonSource)WdbNWWRsjxFdwKePDCTZQcFZsADA).target))
					{
						ListTools.AddAndCreateList(ref list, new ControllerTemplateElementTarget(this, AxisRange.Full));
						num++;
					}
					break;
				default:
					throw new NotImplementedException();
				}
				return num;
			}

			private static bool HeYIANgmGmbuIDCCgMyGfxvlhNCCB(ControllerElementTarget P_0, IControllerElementTarget P_1)
			{
				if (P_1.elementIdentifierId != P_0.elementIdentifierId)
				{
					return false;
				}
				switch (P_1.elementType)
				{
				case ControllerElementType.Axis:
				{
					AxisRange axisRange = P_1.axisRange;
					if (axisRange == AxisRange.Full)
					{
						return true;
					}
					if (axisRange == P_0.axisRange)
					{
						return true;
					}
					return false;
				}
				case ControllerElementType.Button:
					return true;
				default:
					throw new NotImplementedException();
				}
			}
		}

		internal sealed class CUvgYPcuNWQUoLfxEuBtidskpMACb : xplOfuRdmqGAkFpxdfKgUEIduiHT
		{
			public CUvgYPcuNWQUoLfxEuBtidskpMACb(IControllerTemplate P_0, int P_1, string P_2, string P_3, string P_4, HxKJTjnYRTlXmgqyhXzejFNHdCai P_5, IList<AckvZZpMMZJEeXiTnzpZGygPjFpk> P_6)
				: base(P_0, P_1, P_2, P_3, P_4, ControllerTemplateElementType.Axis, P_5, P_6)
			{
				if (P_6 != null && P_6.Count > 2)
				{
					throw new ArgumentOutOfRangeException("sourceElements.Count must be <= 2.");
				}
			}

			internal static CUvgYPcuNWQUoLfxEuBtidskpMACb jqAWqFpdLUDQRsdZuZzXWwxrjFMg(IControllerTemplate P_0)
			{
				return new CUvgYPcuNWQUoLfxEuBtidskpMACb(P_0, -1, string.Empty, string.Empty, string.Empty, HxKJTjnYRTlXmgqyhXzejFNHdCai.jqAWqFpdLUDQRsdZuZzXWwxrjFMg(ControllerTemplateElementType.Axis), null);
			}
		}

		internal sealed class lxRJFUppkWtxxkUrZqBFElqfoxBT : xplOfuRdmqGAkFpxdfKgUEIduiHT
		{
			public lxRJFUppkWtxxkUrZqBFElqfoxBT(IControllerTemplate P_0, int P_1, string P_2, string P_3, string P_4, HxKJTjnYRTlXmgqyhXzejFNHdCai P_5, IList<AckvZZpMMZJEeXiTnzpZGygPjFpk> P_6)
				: base(P_0, P_1, P_2, P_3, P_4, ControllerTemplateElementType.Button, P_5, P_6)
			{
				if (P_6 != null && P_6.Count > 1)
				{
					throw new ArgumentOutOfRangeException("sourceElements.Count must be <= 1.");
				}
			}

			internal static lxRJFUppkWtxxkUrZqBFElqfoxBT jqAWqFpdLUDQRsdZuZzXWwxrjFMg(IControllerTemplate P_0)
			{
				return new lxRJFUppkWtxxkUrZqBFElqfoxBT(P_0, -1, string.Empty, string.Empty, string.Empty, HxKJTjnYRTlXmgqyhXzejFNHdCai.jqAWqFpdLUDQRsdZuZzXWwxrjFMg(ControllerTemplateElementType.Button), null);
			}
		}

		internal abstract class HdTcpZLoztFKwinvXwmQwXyhMbTN : tWsRepKMnZBPynxcHEOrqJwJrtqi
		{
			protected readonly int pQhFxcuPrcDhCdBqMHKrOfeGZOVtA;

			protected readonly tWsRepKMnZBPynxcHEOrqJwJrtqi[] hObmyuQkxgNXDiBtSEwFNgLuBsoZ;

			bool tWsRepKMnZBPynxcHEOrqJwJrtqi.exists
			{
				get
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return false;
					}
					for (int i = 0; i < pQhFxcuPrcDhCdBqMHKrOfeGZOVtA; i++)
					{
						if (hObmyuQkxgNXDiBtSEwFNgLuBsoZ[i].exists)
						{
							return true;
						}
					}
					return false;
				}
			}

			IControllerTemplateElementSource tWsRepKMnZBPynxcHEOrqJwJrtqi.source
			{
				get
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return null;
					}
					return null;
				}
			}

			int tWsRepKMnZBPynxcHEOrqJwJrtqi.elementCount => pQhFxcuPrcDhCdBqMHKrOfeGZOVtA;

			protected HdTcpZLoztFKwinvXwmQwXyhMbTN(IControllerTemplate P_0, int P_1, string P_2, ControllerTemplateElementType P_3, tWsRepKMnZBPynxcHEOrqJwJrtqi[] P_4)
				: base(P_0, P_1, P_2, P_3)
			{
				if (P_4 == null)
				{
					throw new ArgumentNullException("elements");
				}
				if (P_4.Length == 0)
				{
					throw new ArgumentException("elements.Length is zero.");
				}
				for (int i = 0; i < P_4.Length; i++)
				{
					if (P_4[i] == null)
					{
						throw new ArgumentNullException("elements contains a null entry.");
					}
				}
				hObmyuQkxgNXDiBtSEwFNgLuBsoZ = P_4;
				pQhFxcuPrcDhCdBqMHKrOfeGZOVtA = P_4.Length;
			}

			public virtual IControllerTemplateElement pAGcyUYRwJwYiiycWKSARieMOZIo(int P_0)
			{
				return hObmyuQkxgNXDiBtSEwFNgLuBsoZ[P_0];
			}

			public virtual int dcYAIFdORVfJDeuFEeeoCqlvIpJzB(ControllerElementTarget P_0, ref IList<ControllerTemplateElementTarget> P_1)
			{
				int num = 0;
				for (int i = 0; i < hObmyuQkxgNXDiBtSEwFNgLuBsoZ.Length; i++)
				{
					num += hObmyuQkxgNXDiBtSEwFNgLuBsoZ[i].GetElementTargets(P_0, ref P_1);
				}
				return num;
			}
		}

		internal abstract class nYkTjlGDFGRTHCcTrhwSGlyYngjs : HdTcpZLoztFKwinvXwmQwXyhMbTN, IControllerTemplateElement, IControllerTemplateAxis2D
		{
			protected const int NyAdTxfTPKOmetsXDPebKUTftqyY = 0;

			protected const int iYJTVWAKNldOdMdKSGrXqCPLNBMT = 1;

			protected const int RwspCOoHhBBqdDffSWOkpbgGCdbMA = 2;

			public Vector2 value
			{
				get
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return Vector2.zero;
					}
					return new Vector2((pQhFxcuPrcDhCdBqMHKrOfeGZOVtA > 0) ? ((xplOfuRdmqGAkFpxdfKgUEIduiHT)hObmyuQkxgNXDiBtSEwFNgLuBsoZ[0]).brHdwUJAbVcsilgLZZabrTqmRcFR : 0f, (pQhFxcuPrcDhCdBqMHKrOfeGZOVtA > 1) ? ((xplOfuRdmqGAkFpxdfKgUEIduiHT)hObmyuQkxgNXDiBtSEwFNgLuBsoZ[1]).brHdwUJAbVcsilgLZZabrTqmRcFR : 0f);
				}
			}

			public Vector2 valuePrev
			{
				get
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return Vector2.zero;
					}
					return new Vector2((pQhFxcuPrcDhCdBqMHKrOfeGZOVtA > 0) ? ((xplOfuRdmqGAkFpxdfKgUEIduiHT)hObmyuQkxgNXDiBtSEwFNgLuBsoZ[0]).rEhPDLHiMGJDkTLyTdjOJSGOONIl : 0f, (pQhFxcuPrcDhCdBqMHKrOfeGZOVtA > 1) ? ((xplOfuRdmqGAkFpxdfKgUEIduiHT)hObmyuQkxgNXDiBtSEwFNgLuBsoZ[1]).rEhPDLHiMGJDkTLyTdjOJSGOONIl : 0f);
				}
			}

			public IControllerTemplateAxis horizontal
			{
				get
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return null;
					}
					return (IControllerTemplateAxis)hObmyuQkxgNXDiBtSEwFNgLuBsoZ[0];
				}
			}

			public IControllerTemplateAxis vertical
			{
				get
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return null;
					}
					return (IControllerTemplateAxis)hObmyuQkxgNXDiBtSEwFNgLuBsoZ[1];
				}
			}

			protected nYkTjlGDFGRTHCcTrhwSGlyYngjs(IControllerTemplate P_0, int P_1, string P_2, ControllerTemplateElementType P_3, tWsRepKMnZBPynxcHEOrqJwJrtqi[] P_4)
				: base(P_0, P_1, P_2, P_3, P_4)
			{
			}
		}

		internal abstract class bjgAGIyWikBSsbhClQzhHYYDVXWHA : HdTcpZLoztFKwinvXwmQwXyhMbTN, IControllerTemplateElement, IControllerTemplateAxis3D
		{
			protected const int NyAdTxfTPKOmetsXDPebKUTftqyY = 0;

			protected const int iYJTVWAKNldOdMdKSGrXqCPLNBMT = 1;

			protected const int oJqmcwmqpQxrWVzNRcuCwKgKHBMM = 2;

			protected const int RwspCOoHhBBqdDffSWOkpbgGCdbMA = 3;

			public Vector3 value
			{
				get
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return Vector3.zero;
					}
					return new Vector3((pQhFxcuPrcDhCdBqMHKrOfeGZOVtA > 0) ? ((xplOfuRdmqGAkFpxdfKgUEIduiHT)hObmyuQkxgNXDiBtSEwFNgLuBsoZ[0]).brHdwUJAbVcsilgLZZabrTqmRcFR : 0f, (pQhFxcuPrcDhCdBqMHKrOfeGZOVtA > 1) ? ((xplOfuRdmqGAkFpxdfKgUEIduiHT)hObmyuQkxgNXDiBtSEwFNgLuBsoZ[1]).brHdwUJAbVcsilgLZZabrTqmRcFR : 0f, (pQhFxcuPrcDhCdBqMHKrOfeGZOVtA > 2) ? ((xplOfuRdmqGAkFpxdfKgUEIduiHT)hObmyuQkxgNXDiBtSEwFNgLuBsoZ[2]).brHdwUJAbVcsilgLZZabrTqmRcFR : 0f);
				}
			}

			public Vector3 valuePrev
			{
				get
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return Vector3.zero;
					}
					return new Vector3((pQhFxcuPrcDhCdBqMHKrOfeGZOVtA > 0) ? ((xplOfuRdmqGAkFpxdfKgUEIduiHT)hObmyuQkxgNXDiBtSEwFNgLuBsoZ[0]).rEhPDLHiMGJDkTLyTdjOJSGOONIl : 0f, (pQhFxcuPrcDhCdBqMHKrOfeGZOVtA > 1) ? ((xplOfuRdmqGAkFpxdfKgUEIduiHT)hObmyuQkxgNXDiBtSEwFNgLuBsoZ[1]).rEhPDLHiMGJDkTLyTdjOJSGOONIl : 0f, (pQhFxcuPrcDhCdBqMHKrOfeGZOVtA > 2) ? ((xplOfuRdmqGAkFpxdfKgUEIduiHT)hObmyuQkxgNXDiBtSEwFNgLuBsoZ[2]).rEhPDLHiMGJDkTLyTdjOJSGOONIl : 0f);
				}
			}

			public IControllerTemplateAxis horizontal
			{
				get
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return null;
					}
					return (IControllerTemplateAxis)hObmyuQkxgNXDiBtSEwFNgLuBsoZ[0];
				}
			}

			public IControllerTemplateAxis vertical
			{
				get
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return null;
					}
					return (IControllerTemplateAxis)hObmyuQkxgNXDiBtSEwFNgLuBsoZ[1];
				}
			}

			public IControllerTemplateAxis depth
			{
				get
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return null;
					}
					return (IControllerTemplateAxis)hObmyuQkxgNXDiBtSEwFNgLuBsoZ[2];
				}
			}

			protected bjgAGIyWikBSsbhClQzhHYYDVXWHA(IControllerTemplate P_0, int P_1, string P_2, ControllerTemplateElementType P_3, tWsRepKMnZBPynxcHEOrqJwJrtqi[] P_4)
				: base(P_0, P_1, P_2, P_3, P_4)
			{
			}
		}

		internal abstract class NiMBoKRLrOMewcYQIYFQMWTnOIKl : HdTcpZLoztFKwinvXwmQwXyhMbTN, IControllerTemplateElement, IControllerTemplateAxis6D
		{
			protected const int bwMeIZNVfqGavIkkkXlANNCNwAVA = 0;

			protected const int kgnszWoGmZlRJzwgWzYKOjipsmSf = 1;

			protected const int PZNgOXspHIrRIEhpWpawWZJaTszB = 2;

			protected const int uwThbdOQttTTfAvSXhMbGNkqPbQQ = 3;

			protected const int PwsaUbHVfCEXOmySZHuJkdRtjoJJ = 4;

			protected const int DwBCJMgygmXOEBitLvPwyHnyntWp = 5;

			protected const int RwspCOoHhBBqdDffSWOkpbgGCdbMA = 6;

			public Vector3 position
			{
				get
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return Vector3.zero;
					}
					return new Vector3((pQhFxcuPrcDhCdBqMHKrOfeGZOVtA > 0) ? ((xplOfuRdmqGAkFpxdfKgUEIduiHT)hObmyuQkxgNXDiBtSEwFNgLuBsoZ[0]).brHdwUJAbVcsilgLZZabrTqmRcFR : 0f, (pQhFxcuPrcDhCdBqMHKrOfeGZOVtA > 1) ? ((xplOfuRdmqGAkFpxdfKgUEIduiHT)hObmyuQkxgNXDiBtSEwFNgLuBsoZ[1]).brHdwUJAbVcsilgLZZabrTqmRcFR : 0f, (pQhFxcuPrcDhCdBqMHKrOfeGZOVtA > 2) ? ((xplOfuRdmqGAkFpxdfKgUEIduiHT)hObmyuQkxgNXDiBtSEwFNgLuBsoZ[2]).brHdwUJAbVcsilgLZZabrTqmRcFR : 0f);
				}
			}

			public Vector3 positionPrev
			{
				get
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return Vector3.zero;
					}
					return new Vector3((pQhFxcuPrcDhCdBqMHKrOfeGZOVtA > 0) ? ((xplOfuRdmqGAkFpxdfKgUEIduiHT)hObmyuQkxgNXDiBtSEwFNgLuBsoZ[0]).rEhPDLHiMGJDkTLyTdjOJSGOONIl : 0f, (pQhFxcuPrcDhCdBqMHKrOfeGZOVtA > 1) ? ((xplOfuRdmqGAkFpxdfKgUEIduiHT)hObmyuQkxgNXDiBtSEwFNgLuBsoZ[1]).rEhPDLHiMGJDkTLyTdjOJSGOONIl : 0f, (pQhFxcuPrcDhCdBqMHKrOfeGZOVtA > 2) ? ((xplOfuRdmqGAkFpxdfKgUEIduiHT)hObmyuQkxgNXDiBtSEwFNgLuBsoZ[2]).rEhPDLHiMGJDkTLyTdjOJSGOONIl : 0f);
				}
			}

			public Vector3 rotation
			{
				get
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return Vector3.zero;
					}
					return new Vector3((pQhFxcuPrcDhCdBqMHKrOfeGZOVtA > 3) ? ((xplOfuRdmqGAkFpxdfKgUEIduiHT)hObmyuQkxgNXDiBtSEwFNgLuBsoZ[3]).brHdwUJAbVcsilgLZZabrTqmRcFR : 0f, (pQhFxcuPrcDhCdBqMHKrOfeGZOVtA > 4) ? ((xplOfuRdmqGAkFpxdfKgUEIduiHT)hObmyuQkxgNXDiBtSEwFNgLuBsoZ[4]).brHdwUJAbVcsilgLZZabrTqmRcFR : 0f, (pQhFxcuPrcDhCdBqMHKrOfeGZOVtA > 5) ? ((xplOfuRdmqGAkFpxdfKgUEIduiHT)hObmyuQkxgNXDiBtSEwFNgLuBsoZ[5]).brHdwUJAbVcsilgLZZabrTqmRcFR : 0f);
				}
			}

			public Vector3 rotationPrev
			{
				get
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return Vector3.zero;
					}
					return new Vector3((pQhFxcuPrcDhCdBqMHKrOfeGZOVtA > 3) ? ((xplOfuRdmqGAkFpxdfKgUEIduiHT)hObmyuQkxgNXDiBtSEwFNgLuBsoZ[3]).rEhPDLHiMGJDkTLyTdjOJSGOONIl : 0f, (pQhFxcuPrcDhCdBqMHKrOfeGZOVtA > 4) ? ((xplOfuRdmqGAkFpxdfKgUEIduiHT)hObmyuQkxgNXDiBtSEwFNgLuBsoZ[4]).rEhPDLHiMGJDkTLyTdjOJSGOONIl : 0f, (pQhFxcuPrcDhCdBqMHKrOfeGZOVtA > 5) ? ((xplOfuRdmqGAkFpxdfKgUEIduiHT)hObmyuQkxgNXDiBtSEwFNgLuBsoZ[5]).rEhPDLHiMGJDkTLyTdjOJSGOONIl : 0f);
				}
			}

			public IControllerTemplateAxis positionX
			{
				get
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return null;
					}
					return (IControllerTemplateAxis)hObmyuQkxgNXDiBtSEwFNgLuBsoZ[0];
				}
			}

			public IControllerTemplateAxis positionY
			{
				get
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return null;
					}
					return (IControllerTemplateAxis)hObmyuQkxgNXDiBtSEwFNgLuBsoZ[1];
				}
			}

			public IControllerTemplateAxis positionZ
			{
				get
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return null;
					}
					return (IControllerTemplateAxis)hObmyuQkxgNXDiBtSEwFNgLuBsoZ[2];
				}
			}

			public IControllerTemplateAxis rotationX
			{
				get
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return null;
					}
					return (IControllerTemplateAxis)hObmyuQkxgNXDiBtSEwFNgLuBsoZ[3];
				}
			}

			public IControllerTemplateAxis rotationY
			{
				get
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return null;
					}
					return (IControllerTemplateAxis)hObmyuQkxgNXDiBtSEwFNgLuBsoZ[4];
				}
			}

			public IControllerTemplateAxis rotationZ
			{
				get
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return null;
					}
					return (IControllerTemplateAxis)hObmyuQkxgNXDiBtSEwFNgLuBsoZ[5];
				}
			}

			protected NiMBoKRLrOMewcYQIYFQMWTnOIKl(IControllerTemplate P_0, int P_1, string P_2, ControllerTemplateElementType P_3, tWsRepKMnZBPynxcHEOrqJwJrtqi[] P_4)
				: base(P_0, P_1, P_2, P_3, P_4)
			{
			}
		}

		internal sealed class RAKYiYDzCotPmRFpoYhKhoekJhXO : bjgAGIyWikBSsbhClQzhHYYDVXWHA, IControllerTemplateElement, IControllerTemplateStick
		{
			private new const int RwspCOoHhBBqdDffSWOkpbgGCdbMA = 3;

			public IControllerTemplateAxis rotation
			{
				get
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return null;
					}
					return (IControllerTemplateAxis)hObmyuQkxgNXDiBtSEwFNgLuBsoZ[2];
				}
			}

			private RAKYiYDzCotPmRFpoYhKhoekJhXO(IControllerTemplate P_0, int P_1, string P_2, tWsRepKMnZBPynxcHEOrqJwJrtqi[] P_3)
				: base(P_0, P_1, P_2, ControllerTemplateElementType.Stick, P_3)
			{
				if (P_3.Length != 3)
				{
					throw new ArgumentException("elements.Length must be " + 3);
				}
			}

			public RAKYiYDzCotPmRFpoYhKhoekJhXO(IControllerTemplate P_0, int P_1, string P_2, xplOfuRdmqGAkFpxdfKgUEIduiHT P_3, xplOfuRdmqGAkFpxdfKgUEIduiHT P_4, xplOfuRdmqGAkFpxdfKgUEIduiHT P_5)
				: this(P_0, P_1, P_2, new tWsRepKMnZBPynxcHEOrqJwJrtqi[3] { P_3, P_4, P_5 })
			{
			}
		}

		internal sealed class SKdXFctHzBFnUqLKBBYLuFHjtgwO : nYkTjlGDFGRTHCcTrhwSGlyYngjs, IControllerTemplateElement, IControllerTemplateThumbStick
		{
			private const int JUhlQhKiRkMQGGZjhnSBkApryjih = 2;

			private new const int RwspCOoHhBBqdDffSWOkpbgGCdbMA = 3;

			public IControllerTemplateButton press
			{
				get
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return null;
					}
					return (IControllerTemplateButton)hObmyuQkxgNXDiBtSEwFNgLuBsoZ[2];
				}
			}

			private SKdXFctHzBFnUqLKBBYLuFHjtgwO(IControllerTemplate P_0, int P_1, string P_2, tWsRepKMnZBPynxcHEOrqJwJrtqi[] P_3)
				: base(P_0, P_1, P_2, ControllerTemplateElementType.ThumbStick, P_3)
			{
				if (P_3.Length != 3)
				{
					throw new ArgumentException("elements.Length must be " + 3);
				}
			}

			internal SKdXFctHzBFnUqLKBBYLuFHjtgwO(IControllerTemplate P_0, int P_1, string P_2, xplOfuRdmqGAkFpxdfKgUEIduiHT P_3, xplOfuRdmqGAkFpxdfKgUEIduiHT P_4, xplOfuRdmqGAkFpxdfKgUEIduiHT P_5)
				: this(P_0, P_1, P_2, new tWsRepKMnZBPynxcHEOrqJwJrtqi[3] { P_3, P_4, P_5 })
			{
			}
		}

		internal sealed class mZYAZnUOIAemtuOjxZzStAVMBQqAA : HdTcpZLoztFKwinvXwmQwXyhMbTN, IControllerTemplateElement, IControllerTemplateDPad
		{
			private const int TlddnabrliKZIVjElyTxPQrhIKAC = 0;

			private const int GfnKQkVRBNqdQTPKXBslQDUJVRvv = 1;

			private const int sBjkfKjIKYHhJNbfVtipDWvirFEY = 2;

			private const int GRXsxpiBBSqEZRxmJKRNfgresZkC = 3;

			private const int wheKgMknrWDLZTOLYhmdAaXwqwOB = 4;

			private const int RwspCOoHhBBqdDffSWOkpbgGCdbMA = 5;

			public Vector2 value
			{
				get
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return Vector2.zero;
					}
					return new Vector2(MathTools.Clamp(((xplOfuRdmqGAkFpxdfKgUEIduiHT)hObmyuQkxgNXDiBtSEwFNgLuBsoZ[0]).brHdwUJAbVcsilgLZZabrTqmRcFR + ((xplOfuRdmqGAkFpxdfKgUEIduiHT)hObmyuQkxgNXDiBtSEwFNgLuBsoZ[2]).brHdwUJAbVcsilgLZZabrTqmRcFR * -1f, -1f, 1f), MathTools.Clamp(((xplOfuRdmqGAkFpxdfKgUEIduiHT)hObmyuQkxgNXDiBtSEwFNgLuBsoZ[3]).brHdwUJAbVcsilgLZZabrTqmRcFR * -1f + ((xplOfuRdmqGAkFpxdfKgUEIduiHT)hObmyuQkxgNXDiBtSEwFNgLuBsoZ[1]).brHdwUJAbVcsilgLZZabrTqmRcFR, -1f, 1f));
				}
			}

			public Vector2 valuePrev
			{
				get
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return Vector2.zero;
					}
					return new Vector2(MathTools.Clamp(((xplOfuRdmqGAkFpxdfKgUEIduiHT)hObmyuQkxgNXDiBtSEwFNgLuBsoZ[0]).rEhPDLHiMGJDkTLyTdjOJSGOONIl + ((xplOfuRdmqGAkFpxdfKgUEIduiHT)hObmyuQkxgNXDiBtSEwFNgLuBsoZ[2]).rEhPDLHiMGJDkTLyTdjOJSGOONIl * -1f, -1f, 1f), MathTools.Clamp(((xplOfuRdmqGAkFpxdfKgUEIduiHT)hObmyuQkxgNXDiBtSEwFNgLuBsoZ[3]).rEhPDLHiMGJDkTLyTdjOJSGOONIl * -1f + ((xplOfuRdmqGAkFpxdfKgUEIduiHT)hObmyuQkxgNXDiBtSEwFNgLuBsoZ[1]).rEhPDLHiMGJDkTLyTdjOJSGOONIl, -1f, 1f));
				}
			}

			public IControllerTemplateButton up
			{
				get
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return null;
					}
					return (IControllerTemplateButton)hObmyuQkxgNXDiBtSEwFNgLuBsoZ[0];
				}
			}

			public IControllerTemplateButton right
			{
				get
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return null;
					}
					return (IControllerTemplateButton)hObmyuQkxgNXDiBtSEwFNgLuBsoZ[1];
				}
			}

			public IControllerTemplateButton down
			{
				get
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return null;
					}
					return (IControllerTemplateButton)hObmyuQkxgNXDiBtSEwFNgLuBsoZ[2];
				}
			}

			public IControllerTemplateButton left
			{
				get
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return null;
					}
					return (IControllerTemplateButton)hObmyuQkxgNXDiBtSEwFNgLuBsoZ[3];
				}
			}

			public IControllerTemplateButton press
			{
				get
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return null;
					}
					return (IControllerTemplateButton)hObmyuQkxgNXDiBtSEwFNgLuBsoZ[4];
				}
			}

			private mZYAZnUOIAemtuOjxZzStAVMBQqAA(IControllerTemplate P_0, int P_1, string P_2, tWsRepKMnZBPynxcHEOrqJwJrtqi[] P_3)
				: base(P_0, P_1, P_2, ControllerTemplateElementType.DPad, P_3)
			{
				if (P_3.Length != 5)
				{
					throw new ArgumentException("elements.Length must be " + 5);
				}
			}

			internal mZYAZnUOIAemtuOjxZzStAVMBQqAA(IControllerTemplate P_0, int P_1, string P_2, xplOfuRdmqGAkFpxdfKgUEIduiHT P_3, xplOfuRdmqGAkFpxdfKgUEIduiHT P_4, xplOfuRdmqGAkFpxdfKgUEIduiHT P_5, xplOfuRdmqGAkFpxdfKgUEIduiHT P_6, xplOfuRdmqGAkFpxdfKgUEIduiHT P_7)
				: this(P_0, P_1, P_2, new tWsRepKMnZBPynxcHEOrqJwJrtqi[5] { P_3, P_4, P_5, P_6, P_7 })
			{
			}
		}

		internal sealed class jkoAMVJJeANHSBEVLSTTFmSjWbSNb : HdTcpZLoztFKwinvXwmQwXyhMbTN, IControllerTemplateElement, IControllerTemplateThrottle
		{
			private const int otNJAhLTPOEPBklezZdBDdnBVJsV = 0;

			private const int zSczXgPCwKaVfWcaRioMDLaCHiqaA = 1;

			private const int RwspCOoHhBBqdDffSWOkpbgGCdbMA = 2;

			public float value
			{
				get
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return 0f;
					}
					return ((xplOfuRdmqGAkFpxdfKgUEIduiHT)hObmyuQkxgNXDiBtSEwFNgLuBsoZ[0]).brHdwUJAbVcsilgLZZabrTqmRcFR;
				}
			}

			public float valuePrev
			{
				get
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return 0f;
					}
					return ((xplOfuRdmqGAkFpxdfKgUEIduiHT)hObmyuQkxgNXDiBtSEwFNgLuBsoZ[0]).rEhPDLHiMGJDkTLyTdjOJSGOONIl;
				}
			}

			public IControllerTemplateAxis throttle
			{
				get
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return null;
					}
					return (IControllerTemplateAxis)hObmyuQkxgNXDiBtSEwFNgLuBsoZ[0];
				}
			}

			public IControllerTemplateButton minDetent
			{
				get
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return null;
					}
					return (IControllerTemplateButton)hObmyuQkxgNXDiBtSEwFNgLuBsoZ[1];
				}
			}

			private jkoAMVJJeANHSBEVLSTTFmSjWbSNb(IControllerTemplate P_0, int P_1, string P_2, tWsRepKMnZBPynxcHEOrqJwJrtqi[] P_3)
				: base(P_0, P_1, P_2, ControllerTemplateElementType.Throttle, P_3)
			{
				if (P_3.Length != 2)
				{
					throw new ArgumentException("elements.Length must be " + 2);
				}
			}

			internal jkoAMVJJeANHSBEVLSTTFmSjWbSNb(IControllerTemplate P_0, int P_1, string P_2, xplOfuRdmqGAkFpxdfKgUEIduiHT P_3, xplOfuRdmqGAkFpxdfKgUEIduiHT P_4)
				: this(P_0, P_1, P_2, new tWsRepKMnZBPynxcHEOrqJwJrtqi[2] { P_3, P_4 })
			{
			}
		}

		internal sealed class ICIKEZuceqNSJzmhODcuDQQHKDajb : HdTcpZLoztFKwinvXwmQwXyhMbTN, IControllerTemplateElement, IControllerTemplateHat
		{
			private const int TlddnabrliKZIVjElyTxPQrhIKAC = 0;

			private const int nfLZJRdxyUaPdHbjnTRdlaojGINn = 1;

			private const int GfnKQkVRBNqdQTPKXBslQDUJVRvv = 2;

			private const int ATZhweoTSCekdqvLEaGkHFVwcGCb = 3;

			private const int sBjkfKjIKYHhJNbfVtipDWvirFEY = 4;

			private const int MWTsyqcDaigpiEHeHBnUfPaOBCAhb = 5;

			private const int GRXsxpiBBSqEZRxmJKRNfgresZkC = 6;

			private const int baiQFOorUvLipdINLgUuYQxsntII = 7;

			private const int RwspCOoHhBBqdDffSWOkpbgGCdbMA = 8;

			public Vector2 value
			{
				get
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return Vector2.zero;
					}
					Vector2 result = default(Vector2);
					result.y += ((xplOfuRdmqGAkFpxdfKgUEIduiHT)hObmyuQkxgNXDiBtSEwFNgLuBsoZ[0]).brHdwUJAbVcsilgLZZabrTqmRcFR;
					result.x += ((xplOfuRdmqGAkFpxdfKgUEIduiHT)hObmyuQkxgNXDiBtSEwFNgLuBsoZ[2]).brHdwUJAbVcsilgLZZabrTqmRcFR;
					result.y -= ((xplOfuRdmqGAkFpxdfKgUEIduiHT)hObmyuQkxgNXDiBtSEwFNgLuBsoZ[4]).brHdwUJAbVcsilgLZZabrTqmRcFR;
					result.x -= ((xplOfuRdmqGAkFpxdfKgUEIduiHT)hObmyuQkxgNXDiBtSEwFNgLuBsoZ[6]).brHdwUJAbVcsilgLZZabrTqmRcFR;
					float num = ((xplOfuRdmqGAkFpxdfKgUEIduiHT)hObmyuQkxgNXDiBtSEwFNgLuBsoZ[1]).brHdwUJAbVcsilgLZZabrTqmRcFR;
					float num2 = ((xplOfuRdmqGAkFpxdfKgUEIduiHT)hObmyuQkxgNXDiBtSEwFNgLuBsoZ[3]).brHdwUJAbVcsilgLZZabrTqmRcFR;
					float num3 = ((xplOfuRdmqGAkFpxdfKgUEIduiHT)hObmyuQkxgNXDiBtSEwFNgLuBsoZ[5]).brHdwUJAbVcsilgLZZabrTqmRcFR;
					float num4 = ((xplOfuRdmqGAkFpxdfKgUEIduiHT)hObmyuQkxgNXDiBtSEwFNgLuBsoZ[7]).brHdwUJAbVcsilgLZZabrTqmRcFR;
					result.x += num + num2 - num3 - num4;
					result.y += num + num4 - num2 - num3;
					result.x = MathTools.Clamp(result.x, -1f, 1f);
					result.y = MathTools.Clamp(result.y, -1f, 1f);
					return result;
				}
			}

			public Vector2 valuePrev
			{
				get
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return Vector2.zero;
					}
					Vector2 result = default(Vector2);
					result.y += ((xplOfuRdmqGAkFpxdfKgUEIduiHT)hObmyuQkxgNXDiBtSEwFNgLuBsoZ[0]).rEhPDLHiMGJDkTLyTdjOJSGOONIl;
					result.x += ((xplOfuRdmqGAkFpxdfKgUEIduiHT)hObmyuQkxgNXDiBtSEwFNgLuBsoZ[2]).rEhPDLHiMGJDkTLyTdjOJSGOONIl;
					result.y -= ((xplOfuRdmqGAkFpxdfKgUEIduiHT)hObmyuQkxgNXDiBtSEwFNgLuBsoZ[4]).rEhPDLHiMGJDkTLyTdjOJSGOONIl;
					result.x -= ((xplOfuRdmqGAkFpxdfKgUEIduiHT)hObmyuQkxgNXDiBtSEwFNgLuBsoZ[6]).rEhPDLHiMGJDkTLyTdjOJSGOONIl;
					float num = ((xplOfuRdmqGAkFpxdfKgUEIduiHT)hObmyuQkxgNXDiBtSEwFNgLuBsoZ[1]).rEhPDLHiMGJDkTLyTdjOJSGOONIl;
					float num2 = ((xplOfuRdmqGAkFpxdfKgUEIduiHT)hObmyuQkxgNXDiBtSEwFNgLuBsoZ[3]).rEhPDLHiMGJDkTLyTdjOJSGOONIl;
					float num3 = ((xplOfuRdmqGAkFpxdfKgUEIduiHT)hObmyuQkxgNXDiBtSEwFNgLuBsoZ[5]).rEhPDLHiMGJDkTLyTdjOJSGOONIl;
					float num4 = ((xplOfuRdmqGAkFpxdfKgUEIduiHT)hObmyuQkxgNXDiBtSEwFNgLuBsoZ[7]).rEhPDLHiMGJDkTLyTdjOJSGOONIl;
					result.x += num + num2 - num3 - num4;
					result.y += num + num4 - num2 - num3;
					result.x = MathTools.Clamp(result.x, -1f, 1f);
					result.y = MathTools.Clamp(result.y, -1f, 1f);
					return result;
				}
			}

			public IControllerTemplateButton up
			{
				get
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return null;
					}
					return (IControllerTemplateButton)hObmyuQkxgNXDiBtSEwFNgLuBsoZ[0];
				}
			}

			public IControllerTemplateButton upRight
			{
				get
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return null;
					}
					return (IControllerTemplateButton)hObmyuQkxgNXDiBtSEwFNgLuBsoZ[1];
				}
			}

			public IControllerTemplateButton right
			{
				get
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return null;
					}
					return (IControllerTemplateButton)hObmyuQkxgNXDiBtSEwFNgLuBsoZ[2];
				}
			}

			public IControllerTemplateButton downRight
			{
				get
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return null;
					}
					return (IControllerTemplateButton)hObmyuQkxgNXDiBtSEwFNgLuBsoZ[3];
				}
			}

			public IControllerTemplateButton down
			{
				get
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return null;
					}
					return (IControllerTemplateButton)hObmyuQkxgNXDiBtSEwFNgLuBsoZ[4];
				}
			}

			public IControllerTemplateButton downLeft
			{
				get
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return null;
					}
					return (IControllerTemplateButton)hObmyuQkxgNXDiBtSEwFNgLuBsoZ[5];
				}
			}

			public IControllerTemplateButton left
			{
				get
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return null;
					}
					return (IControllerTemplateButton)hObmyuQkxgNXDiBtSEwFNgLuBsoZ[6];
				}
			}

			public IControllerTemplateButton upLeft
			{
				get
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return null;
					}
					return (IControllerTemplateButton)hObmyuQkxgNXDiBtSEwFNgLuBsoZ[7];
				}
			}

			private ICIKEZuceqNSJzmhODcuDQQHKDajb(IControllerTemplate P_0, int P_1, string P_2, tWsRepKMnZBPynxcHEOrqJwJrtqi[] P_3)
				: base(P_0, P_1, P_2, ControllerTemplateElementType.Hat, P_3)
			{
				if (P_3.Length != 8)
				{
					throw new ArgumentException("elements.Length must be " + 8);
				}
			}

			internal ICIKEZuceqNSJzmhODcuDQQHKDajb(IControllerTemplate P_0, int P_1, string P_2, xplOfuRdmqGAkFpxdfKgUEIduiHT P_3, xplOfuRdmqGAkFpxdfKgUEIduiHT P_4, xplOfuRdmqGAkFpxdfKgUEIduiHT P_5, xplOfuRdmqGAkFpxdfKgUEIduiHT P_6, xplOfuRdmqGAkFpxdfKgUEIduiHT P_7, xplOfuRdmqGAkFpxdfKgUEIduiHT P_8, xplOfuRdmqGAkFpxdfKgUEIduiHT P_9, xplOfuRdmqGAkFpxdfKgUEIduiHT P_10)
				: this(P_0, P_1, P_2, new tWsRepKMnZBPynxcHEOrqJwJrtqi[8] { P_3, P_4, P_5, P_6, P_7, P_8, P_9, P_10 })
			{
			}
		}

		internal sealed class CLDVmlgHxUZAcZnHbFwnwNPRZHzA : nYkTjlGDFGRTHCcTrhwSGlyYngjs, IControllerTemplateElement, IControllerTemplateYoke
		{
			private new const int RwspCOoHhBBqdDffSWOkpbgGCdbMA = 2;

			public IControllerTemplateAxis rotation
			{
				get
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return null;
					}
					return (IControllerTemplateAxis)hObmyuQkxgNXDiBtSEwFNgLuBsoZ[0];
				}
			}

			public IControllerTemplateAxis pushPull
			{
				get
				{
					if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
					{
						ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
						return null;
					}
					return (IControllerTemplateAxis)hObmyuQkxgNXDiBtSEwFNgLuBsoZ[1];
				}
			}

			private CLDVmlgHxUZAcZnHbFwnwNPRZHzA(IControllerTemplate P_0, int P_1, string P_2, tWsRepKMnZBPynxcHEOrqJwJrtqi[] P_3)
				: base(P_0, P_1, P_2, ControllerTemplateElementType.Yoke, P_3)
			{
			}

			internal CLDVmlgHxUZAcZnHbFwnwNPRZHzA(IControllerTemplate P_0, int P_1, string P_2, xplOfuRdmqGAkFpxdfKgUEIduiHT P_3, xplOfuRdmqGAkFpxdfKgUEIduiHT P_4)
				: base(P_0, P_1, P_2, ControllerTemplateElementType.Yoke, new tWsRepKMnZBPynxcHEOrqJwJrtqi[2] { P_3, P_4 })
			{
			}
		}

		internal sealed class iiKqtnQKUCcgSHGKrWUpGepvbyMab : NiMBoKRLrOMewcYQIYFQMWTnOIKl, IControllerTemplateElement, IControllerTemplateStick6D
		{
			private new const int RwspCOoHhBBqdDffSWOkpbgGCdbMA = 6;

			private iiKqtnQKUCcgSHGKrWUpGepvbyMab(IControllerTemplate P_0, int P_1, string P_2, tWsRepKMnZBPynxcHEOrqJwJrtqi[] P_3)
				: base(P_0, P_1, P_2, ControllerTemplateElementType.Stick6D, P_3)
			{
			}

			internal iiKqtnQKUCcgSHGKrWUpGepvbyMab(IControllerTemplate P_0, int P_1, string P_2, xplOfuRdmqGAkFpxdfKgUEIduiHT P_3, xplOfuRdmqGAkFpxdfKgUEIduiHT P_4, xplOfuRdmqGAkFpxdfKgUEIduiHT P_5, xplOfuRdmqGAkFpxdfKgUEIduiHT P_6, xplOfuRdmqGAkFpxdfKgUEIduiHT P_7, xplOfuRdmqGAkFpxdfKgUEIduiHT P_8)
				: base(P_0, P_1, P_2, ControllerTemplateElementType.Stick6D, new tWsRepKMnZBPynxcHEOrqJwJrtqi[6] { P_3, P_4, P_5, P_6, P_7, P_8 })
			{
			}
		}

		internal class AckvZZpMMZJEeXiTnzpZGygPjFpk
		{
			public readonly Controller.Element IeBiSvPSlvevRBzqkYknNspTYxmIA;

			public readonly IControllerElementTarget QMmaBzbGhLRYBQhUiAbwUcRQwInF;

			public bool vTrlgouSJGtOvawjunaiRTKrzwsD
			{
				get
				{
					if (IeBiSvPSlvevRBzqkYknNspTYxmIA == null)
					{
						return false;
					}
					switch (IeBiSvPSlvevRBzqkYknNspTYxmIA.type)
					{
					case ControllerElementType.Button:
						return (IeBiSvPSlvevRBzqkYknNspTYxmIA as Controller.Button).value;
					case ControllerElementType.Axis:
					{
						float value = (IeBiSvPSlvevRBzqkYknNspTYxmIA as Controller.Axis).value;
						switch (QMmaBzbGhLRYBQhUiAbwUcRQwInF.axisRange)
						{
						case AxisRange.Full:
							if (value > 0.01f)
							{
								return true;
							}
							if (value < -0.01f)
							{
								return true;
							}
							break;
						case AxisRange.Positive:
							if (value > 0.01f)
							{
								return true;
							}
							break;
						case AxisRange.Negative:
							if (value < -0.01f)
							{
								return true;
							}
							break;
						}
						break;
					}
					}
					return false;
				}
			}

			public bool fDZzkobccRjlJoXBBUdrauDEaisJ
			{
				get
				{
					if (IeBiSvPSlvevRBzqkYknNspTYxmIA == null)
					{
						return false;
					}
					switch (IeBiSvPSlvevRBzqkYknNspTYxmIA.type)
					{
					case ControllerElementType.Button:
						return (IeBiSvPSlvevRBzqkYknNspTYxmIA as Controller.Button).valuePrev;
					case ControllerElementType.Axis:
					{
						float valuePrev = (IeBiSvPSlvevRBzqkYknNspTYxmIA as Controller.Axis).valuePrev;
						switch (QMmaBzbGhLRYBQhUiAbwUcRQwInF.axisRange)
						{
						case AxisRange.Full:
							if (valuePrev > 0.01f)
							{
								return true;
							}
							if (valuePrev < -0.01f)
							{
								return true;
							}
							break;
						case AxisRange.Positive:
							if (valuePrev > 0.01f)
							{
								return true;
							}
							break;
						case AxisRange.Negative:
							if (valuePrev < -0.01f)
							{
								return true;
							}
							break;
						}
						break;
					}
					}
					return false;
				}
			}

			public bool zJkMPGtMddueMcJDyMsXdhcqorQl
			{
				get
				{
					if (IeBiSvPSlvevRBzqkYknNspTYxmIA == null)
					{
						return false;
					}
					switch (IeBiSvPSlvevRBzqkYknNspTYxmIA.type)
					{
					case ControllerElementType.Button:
						return (IeBiSvPSlvevRBzqkYknNspTYxmIA as Controller.Button).justPressed;
					case ControllerElementType.Axis:
						if (MathTools.Abs(brHdwUJAbVcsilgLZZabrTqmRcFR) > 0.01f && MathTools.Abs(rEhPDLHiMGJDkTLyTdjOJSGOONIl) <= 0.01f)
						{
							return true;
						}
						break;
					}
					return false;
				}
			}

			public bool uoMLTbBPDCNDEQdFdJvyeBicfQGhA
			{
				get
				{
					if (IeBiSvPSlvevRBzqkYknNspTYxmIA == null)
					{
						return false;
					}
					switch (IeBiSvPSlvevRBzqkYknNspTYxmIA.type)
					{
					case ControllerElementType.Button:
						return (IeBiSvPSlvevRBzqkYknNspTYxmIA as Controller.Button).justReleased;
					case ControllerElementType.Axis:
						if (MathTools.Abs(brHdwUJAbVcsilgLZZabrTqmRcFR) <= 0.01f && MathTools.Abs(rEhPDLHiMGJDkTLyTdjOJSGOONIl) > 0.01f)
						{
							return true;
						}
						break;
					}
					return false;
				}
			}

			public float brHdwUJAbVcsilgLZZabrTqmRcFR
			{
				get
				{
					if (IeBiSvPSlvevRBzqkYknNspTYxmIA == null)
					{
						return 0f;
					}
					switch (IeBiSvPSlvevRBzqkYknNspTYxmIA.type)
					{
					case ControllerElementType.Button:
						if (!(IeBiSvPSlvevRBzqkYknNspTYxmIA as Controller.Button).value)
						{
							return 0f;
						}
						return 1f;
					case ControllerElementType.Axis:
					{
						float value = (IeBiSvPSlvevRBzqkYknNspTYxmIA as Controller.Axis).value;
						switch (QMmaBzbGhLRYBQhUiAbwUcRQwInF.axisRange)
						{
						case AxisRange.Full:
							return value;
						case AxisRange.Positive:
							if (value > 0f)
							{
								return value;
							}
							break;
						case AxisRange.Negative:
							if (value < 0f)
							{
								return value;
							}
							break;
						}
						break;
					}
					}
					return 0f;
				}
			}

			public float rEhPDLHiMGJDkTLyTdjOJSGOONIl
			{
				get
				{
					if (IeBiSvPSlvevRBzqkYknNspTYxmIA == null)
					{
						return 0f;
					}
					switch (IeBiSvPSlvevRBzqkYknNspTYxmIA.type)
					{
					case ControllerElementType.Button:
						if (!(IeBiSvPSlvevRBzqkYknNspTYxmIA as Controller.Button).valuePrev)
						{
							return 0f;
						}
						return 1f;
					case ControllerElementType.Axis:
					{
						float valuePrev = (IeBiSvPSlvevRBzqkYknNspTYxmIA as Controller.Axis).valuePrev;
						switch (QMmaBzbGhLRYBQhUiAbwUcRQwInF.axisRange)
						{
						case AxisRange.Full:
							return valuePrev;
						case AxisRange.Positive:
							if (valuePrev > 0f)
							{
								return valuePrev;
							}
							break;
						case AxisRange.Negative:
							if (valuePrev < 0f)
							{
								return valuePrev;
							}
							break;
						}
						break;
					}
					}
					return 0f;
				}
			}

			public AckvZZpMMZJEeXiTnzpZGygPjFpk(IControllerElementTarget P_0, Controller.Element P_1)
			{
				IeBiSvPSlvevRBzqkYknNspTYxmIA = P_1;
				QMmaBzbGhLRYBQhUiAbwUcRQwInF = P_0;
			}

			public static AckvZZpMMZJEeXiTnzpZGygPjFpk jqAWqFpdLUDQRsdZuZzXWwxrjFMg()
			{
				return new AckvZZpMMZJEeXiTnzpZGygPjFpk(cyWFKpvAJpSlVUbktJsNPZKHVsyU.jqAWqFpdLUDQRsdZuZzXWwxrjFMg(), null);
			}
		}

		internal class mMyCOJEBAEHmGFJutMHNAUKrIxjSA
		{
			public readonly Controller MHuXHKLCPsUIeLOovImpnHVJaYufA;

			public readonly IHardwareControllerTemplateMap_Internal DqWEEmjnsXsjFhDjdIFgWVfVygCQA;

			public mMyCOJEBAEHmGFJutMHNAUKrIxjSA(Controller P_0, IHardwareControllerTemplateMap_Internal P_1)
			{
				if (P_0 == null)
				{
					throw new ArgumentNullException("controller");
				}
				if (P_1 == null)
				{
					throw new ArgumentNullException("templateMap");
				}
				MHuXHKLCPsUIeLOovImpnHVJaYufA = P_0;
				DqWEEmjnsXsjFhDjdIFgWVfVygCQA = P_1;
			}
		}

		private readonly string hEJFspqWqeBOdedbqHNRpPNEbfBG;

		private readonly Guid OJEPZrVGmynCKCXPlorDNMQIZfnQ;

		private readonly Controller oBPMdftKtJLWDwKjOKgLuimxBfUKA;

		private readonly ADictionary<int, IControllerTemplateElement> DSJJnwwCJPjjjysohBSGxpSbEGtBA;

		private readonly ADictionary<string, IControllerTemplateElement> fnwsuLHukmUXSqAdAeHEuLmuQsWD;

		private IControllerTemplateElement[] hObmyuQkxgNXDiBtSEwFNgLuBsoZ;

		private ReadOnlyCollection<IControllerTemplateElement> VewvmtxDhBKjfOztXaGoFFLyxNDV;

		private readonly int QajDFFaomlkLHzaostfWYpGUioys;

		Controller IControllerTemplate.controller
		{
			get
			{
				if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
				{
					ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
					return null;
				}
				return oBPMdftKtJLWDwKjOKgLuimxBfUKA;
			}
		}

		string IControllerTemplate.name
		{
			get
			{
				if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
				{
					ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
					return null;
				}
				return hEJFspqWqeBOdedbqHNRpPNEbfBG;
			}
		}

		Guid IControllerTemplate.typeGuid
		{
			get
			{
				if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
				{
					ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
					return Guid.Empty;
				}
				return OJEPZrVGmynCKCXPlorDNMQIZfnQ;
			}
		}

		IList<IControllerTemplateElement> IControllerTemplate.elements
		{
			get
			{
				if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
				{
					ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
					return null;
				}
				return VewvmtxDhBKjfOztXaGoFFLyxNDV;
			}
		}

		int IControllerTemplate.elementCount
		{
			get
			{
				if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
				{
					ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
					return 0;
				}
				return hObmyuQkxgNXDiBtSEwFNgLuBsoZ.Length;
			}
		}

		protected ControllerTemplate(object P_0)
			: this((mMyCOJEBAEHmGFJutMHNAUKrIxjSA)P_0)
		{
		}

		private ControllerTemplate(mMyCOJEBAEHmGFJutMHNAUKrIxjSA P_0)
		{
			if (P_0 == null)
			{
				throw new ArgumentNullException("initializer");
			}
			if (P_0.MHuXHKLCPsUIeLOovImpnHVJaYufA == null)
			{
				throw new ArgumentNullException("initializer.controller");
			}
			if (P_0.DqWEEmjnsXsjFhDjdIFgWVfVygCQA == null)
			{
				throw new ArgumentNullException("initializer.templateMap");
			}
			QajDFFaomlkLHzaostfWYpGUioys = ReInput.id;
			oBPMdftKtJLWDwKjOKgLuimxBfUKA = P_0.MHuXHKLCPsUIeLOovImpnHVJaYufA;
			IHardwareControllerTemplateMap_Internal dqWEEmjnsXsjFhDjdIFgWVfVygCQA = P_0.DqWEEmjnsXsjFhDjdIFgWVfVygCQA;
			hEJFspqWqeBOdedbqHNRpPNEbfBG = dqWEEmjnsXsjFhDjdIFgWVfVygCQA.name;
			OJEPZrVGmynCKCXPlorDNMQIZfnQ = dqWEEmjnsXsjFhDjdIFgWVfVygCQA.typeGuid;
			int elementIdentifierCount = dqWEEmjnsXsjFhDjdIFgWVfVygCQA.GetElementIdentifierCount();
			ADictionary<int, IControllerTemplateElement> aDictionary = new ADictionary<int, IControllerTemplateElement>();
			List<IControllerTemplateElement> list = new List<IControllerTemplateElement>();
			List<IControllerTemplateAxis> list2 = new List<IControllerTemplateAxis>();
			List<IControllerTemplateButton> list3 = new List<IControllerTemplateButton>();
			List<IControllerTemplateElement> list4 = new List<IControllerTemplateElement>();
			for (int i = 0; i < elementIdentifierCount; i++)
			{
				IControllerTemplateElementIdentifier templateElementIdentifier = dqWEEmjnsXsjFhDjdIFgWVfVygCQA.GetTemplateElementIdentifier(i);
				if (templateElementIdentifier != null && InputTools.IsMappableType(templateElementIdentifier.elementType))
				{
					switch (templateElementIdentifier.elementType)
					{
					case ControllerTemplateElementType.Axis:
					{
						HxKJTjnYRTlXmgqyhXzejFNHdCai hxKJTjnYRTlXmgqyhXzejFNHdCai2 = dqWEEmjnsXsjFhDjdIFgWVfVygCQA.GetAxisTarget(oBPMdftKtJLWDwKjOKgLuimxBfUKA, templateElementIdentifier.id) ?? HxKJTjnYRTlXmgqyhXzejFNHdCai.jqAWqFpdLUDQRsdZuZzXWwxrjFMg(ControllerTemplateElementType.Axis);
						CUvgYPcuNWQUoLfxEuBtidskpMACb item2 = new CUvgYPcuNWQUoLfxEuBtidskpMACb(this, templateElementIdentifier.id, templateElementIdentifier.name, (!string.IsNullOrEmpty(templateElementIdentifier.positiveName)) ? templateElementIdentifier.positiveName : (templateElementIdentifier.name + " +"), (!string.IsNullOrEmpty(templateElementIdentifier.negativeName)) ? templateElementIdentifier.negativeName : (templateElementIdentifier.name + " -"), hxKJTjnYRTlXmgqyhXzejFNHdCai2, MgpsizkckMCngeoIfdZBxlQJxgpDA(oBPMdftKtJLWDwKjOKgLuimxBfUKA, (IControllerTemplateAxisSource)hxKJTjnYRTlXmgqyhXzejFNHdCai2));
						list2.Add(item2);
						break;
					}
					case ControllerTemplateElementType.Button:
					{
						HxKJTjnYRTlXmgqyhXzejFNHdCai hxKJTjnYRTlXmgqyhXzejFNHdCai = dqWEEmjnsXsjFhDjdIFgWVfVygCQA.GetButtonTarget(oBPMdftKtJLWDwKjOKgLuimxBfUKA, templateElementIdentifier.id) ?? HxKJTjnYRTlXmgqyhXzejFNHdCai.jqAWqFpdLUDQRsdZuZzXWwxrjFMg(ControllerTemplateElementType.Button);
						lxRJFUppkWtxxkUrZqBFElqfoxBT item = new lxRJFUppkWtxxkUrZqBFElqfoxBT(this, templateElementIdentifier.id, templateElementIdentifier.name, templateElementIdentifier.name, templateElementIdentifier.name + " -", hxKJTjnYRTlXmgqyhXzejFNHdCai, MgpsizkckMCngeoIfdZBxlQJxgpDA(oBPMdftKtJLWDwKjOKgLuimxBfUKA, (IControllerTemplateButtonSource)hxKJTjnYRTlXmgqyhXzejFNHdCai));
						list3.Add(item);
						break;
					}
					default:
						throw new NotImplementedException();
					}
				}
			}
			for (int j = 0; j < list2.Count; j++)
			{
				list.Add(list2[j]);
			}
			for (int k = 0; k < list3.Count; k++)
			{
				list.Add(list3[k]);
			}
			for (int l = 0; l < list.Count; l++)
			{
				aDictionary.Add(list[l].id, list[l]);
			}
			for (int m = 0; m < elementIdentifierCount; m++)
			{
				IControllerTemplateElementIdentifier templateElementIdentifier2 = dqWEEmjnsXsjFhDjdIFgWVfVygCQA.GetTemplateElementIdentifier(m);
				if (templateElementIdentifier2 == null || InputTools.IsMappableType(templateElementIdentifier2.elementType))
				{
					continue;
				}
				IControllerTemplateMapSpecialElement_Internal specialTemplateElementByElementIdentifierId = dqWEEmjnsXsjFhDjdIFgWVfVygCQA.GetSpecialTemplateElementByElementIdentifierId(templateElementIdentifier2.id);
				tWsRepKMnZBPynxcHEOrqJwJrtqi tWsRepKMnZBPynxcHEOrqJwJrtqi2;
				switch (templateElementIdentifier2.elementType)
				{
				case ControllerTemplateElementType.ThumbStick:
				{
					if (specialTemplateElementByElementIdentifierId == null)
					{
						Logger.LogError(templateElementIdentifier2.elementType.ToString() + " element missing for Element Identifier Id " + templateElementIdentifier2.id);
					}
					ControllerTemplateThumbStickMapping mapping5 = specialTemplateElementByElementIdentifierId.GetMapping<ControllerTemplateThumbStickMapping>();
					tWsRepKMnZBPynxcHEOrqJwJrtqi2 = new SKdXFctHzBFnUqLKBBYLuFHjtgwO(this, templateElementIdentifier2.id, templateElementIdentifier2.name, (mapping5 != null) ? JDJOMZTtkxuCBxrYAauGFfbVcvIbA(this, aDictionary, mapping5.eid_axisX) : CUvgYPcuNWQUoLfxEuBtidskpMACb.jqAWqFpdLUDQRsdZuZzXWwxrjFMg(this), (mapping5 != null) ? JDJOMZTtkxuCBxrYAauGFfbVcvIbA(this, aDictionary, mapping5.eid_axisY) : CUvgYPcuNWQUoLfxEuBtidskpMACb.jqAWqFpdLUDQRsdZuZzXWwxrjFMg(this), (mapping5 != null) ? ksUjSAMUrwIVaaryoEPEWmsYdJdS(this, aDictionary, mapping5.eid_button) : lxRJFUppkWtxxkUrZqBFElqfoxBT.jqAWqFpdLUDQRsdZuZzXWwxrjFMg(this));
					break;
				}
				case ControllerTemplateElementType.DPad:
				{
					if (specialTemplateElementByElementIdentifierId == null)
					{
						Logger.LogError(templateElementIdentifier2.elementType.ToString() + " element missing for Element Identifier Id " + templateElementIdentifier2.id);
					}
					ControllerTemplateDPadMapping mapping3 = specialTemplateElementByElementIdentifierId.GetMapping<ControllerTemplateDPadMapping>();
					tWsRepKMnZBPynxcHEOrqJwJrtqi2 = new mZYAZnUOIAemtuOjxZzStAVMBQqAA(this, templateElementIdentifier2.id, templateElementIdentifier2.name, (mapping3 != null) ? ksUjSAMUrwIVaaryoEPEWmsYdJdS(this, aDictionary, mapping3.eid_up) : lxRJFUppkWtxxkUrZqBFElqfoxBT.jqAWqFpdLUDQRsdZuZzXWwxrjFMg(this), (mapping3 != null) ? ksUjSAMUrwIVaaryoEPEWmsYdJdS(this, aDictionary, mapping3.eid_right) : lxRJFUppkWtxxkUrZqBFElqfoxBT.jqAWqFpdLUDQRsdZuZzXWwxrjFMg(this), (mapping3 != null) ? ksUjSAMUrwIVaaryoEPEWmsYdJdS(this, aDictionary, mapping3.eid_down) : lxRJFUppkWtxxkUrZqBFElqfoxBT.jqAWqFpdLUDQRsdZuZzXWwxrjFMg(this), (mapping3 != null) ? ksUjSAMUrwIVaaryoEPEWmsYdJdS(this, aDictionary, mapping3.eid_left) : lxRJFUppkWtxxkUrZqBFElqfoxBT.jqAWqFpdLUDQRsdZuZzXWwxrjFMg(this), (mapping3 != null) ? ksUjSAMUrwIVaaryoEPEWmsYdJdS(this, aDictionary, mapping3.eid_press) : lxRJFUppkWtxxkUrZqBFElqfoxBT.jqAWqFpdLUDQRsdZuZzXWwxrjFMg(this));
					break;
				}
				case ControllerTemplateElementType.Stick:
				{
					if (specialTemplateElementByElementIdentifierId == null)
					{
						Logger.LogError(templateElementIdentifier2.elementType.ToString() + " element missing for Element Identifier Id " + templateElementIdentifier2.id);
					}
					ControllerTemplateStickMapping mapping2 = specialTemplateElementByElementIdentifierId.GetMapping<ControllerTemplateStickMapping>();
					tWsRepKMnZBPynxcHEOrqJwJrtqi2 = new RAKYiYDzCotPmRFpoYhKhoekJhXO(this, templateElementIdentifier2.id, templateElementIdentifier2.name, (mapping2 != null) ? JDJOMZTtkxuCBxrYAauGFfbVcvIbA(this, aDictionary, mapping2.eid_axisX) : CUvgYPcuNWQUoLfxEuBtidskpMACb.jqAWqFpdLUDQRsdZuZzXWwxrjFMg(this), (mapping2 != null) ? JDJOMZTtkxuCBxrYAauGFfbVcvIbA(this, aDictionary, mapping2.eid_axisY) : CUvgYPcuNWQUoLfxEuBtidskpMACb.jqAWqFpdLUDQRsdZuZzXWwxrjFMg(this), (mapping2 != null) ? JDJOMZTtkxuCBxrYAauGFfbVcvIbA(this, aDictionary, mapping2.eid_axisZ) : CUvgYPcuNWQUoLfxEuBtidskpMACb.jqAWqFpdLUDQRsdZuZzXWwxrjFMg(this));
					break;
				}
				case ControllerTemplateElementType.Throttle:
				{
					if (specialTemplateElementByElementIdentifierId == null)
					{
						Logger.LogError(templateElementIdentifier2.elementType.ToString() + " element missing for Element Identifier Id " + templateElementIdentifier2.id);
					}
					ControllerTemplateThrottleMapping mapping6 = specialTemplateElementByElementIdentifierId.GetMapping<ControllerTemplateThrottleMapping>();
					tWsRepKMnZBPynxcHEOrqJwJrtqi2 = new jkoAMVJJeANHSBEVLSTTFmSjWbSNb(this, templateElementIdentifier2.id, templateElementIdentifier2.name, (mapping6 != null) ? JDJOMZTtkxuCBxrYAauGFfbVcvIbA(this, aDictionary, mapping6.eid_axis) : CUvgYPcuNWQUoLfxEuBtidskpMACb.jqAWqFpdLUDQRsdZuZzXWwxrjFMg(this), (mapping6 != null) ? ksUjSAMUrwIVaaryoEPEWmsYdJdS(this, aDictionary, mapping6.eid_minDetent) : lxRJFUppkWtxxkUrZqBFElqfoxBT.jqAWqFpdLUDQRsdZuZzXWwxrjFMg(this));
					break;
				}
				case ControllerTemplateElementType.Hat:
				{
					if (specialTemplateElementByElementIdentifierId == null)
					{
						Logger.LogError(templateElementIdentifier2.elementType.ToString() + " element missing for Element Identifier Id " + templateElementIdentifier2.id);
					}
					ControllerTemplateHatMapping mapping7 = specialTemplateElementByElementIdentifierId.GetMapping<ControllerTemplateHatMapping>();
					tWsRepKMnZBPynxcHEOrqJwJrtqi2 = new ICIKEZuceqNSJzmhODcuDQQHKDajb(this, templateElementIdentifier2.id, templateElementIdentifier2.name, (mapping7 != null) ? ksUjSAMUrwIVaaryoEPEWmsYdJdS(this, aDictionary, mapping7.eid_up) : lxRJFUppkWtxxkUrZqBFElqfoxBT.jqAWqFpdLUDQRsdZuZzXWwxrjFMg(this), (mapping7 != null) ? ksUjSAMUrwIVaaryoEPEWmsYdJdS(this, aDictionary, mapping7.eid_upRight) : lxRJFUppkWtxxkUrZqBFElqfoxBT.jqAWqFpdLUDQRsdZuZzXWwxrjFMg(this), (mapping7 != null) ? ksUjSAMUrwIVaaryoEPEWmsYdJdS(this, aDictionary, mapping7.eid_right) : lxRJFUppkWtxxkUrZqBFElqfoxBT.jqAWqFpdLUDQRsdZuZzXWwxrjFMg(this), (mapping7 != null) ? ksUjSAMUrwIVaaryoEPEWmsYdJdS(this, aDictionary, mapping7.eid_downRight) : lxRJFUppkWtxxkUrZqBFElqfoxBT.jqAWqFpdLUDQRsdZuZzXWwxrjFMg(this), (mapping7 != null) ? ksUjSAMUrwIVaaryoEPEWmsYdJdS(this, aDictionary, mapping7.eid_down) : lxRJFUppkWtxxkUrZqBFElqfoxBT.jqAWqFpdLUDQRsdZuZzXWwxrjFMg(this), (mapping7 != null) ? ksUjSAMUrwIVaaryoEPEWmsYdJdS(this, aDictionary, mapping7.eid_downLeft) : lxRJFUppkWtxxkUrZqBFElqfoxBT.jqAWqFpdLUDQRsdZuZzXWwxrjFMg(this), (mapping7 != null) ? ksUjSAMUrwIVaaryoEPEWmsYdJdS(this, aDictionary, mapping7.eid_left) : lxRJFUppkWtxxkUrZqBFElqfoxBT.jqAWqFpdLUDQRsdZuZzXWwxrjFMg(this), (mapping7 != null) ? ksUjSAMUrwIVaaryoEPEWmsYdJdS(this, aDictionary, mapping7.eid_upLeft) : lxRJFUppkWtxxkUrZqBFElqfoxBT.jqAWqFpdLUDQRsdZuZzXWwxrjFMg(this));
					break;
				}
				case ControllerTemplateElementType.Yoke:
				{
					if (specialTemplateElementByElementIdentifierId == null)
					{
						Logger.LogError(templateElementIdentifier2.elementType.ToString() + " element missing for Element Identifier Id " + templateElementIdentifier2.id);
					}
					ControllerTemplateYokeMapping mapping4 = specialTemplateElementByElementIdentifierId.GetMapping<ControllerTemplateYokeMapping>();
					tWsRepKMnZBPynxcHEOrqJwJrtqi2 = new CLDVmlgHxUZAcZnHbFwnwNPRZHzA(this, templateElementIdentifier2.id, templateElementIdentifier2.name, (mapping4 != null) ? JDJOMZTtkxuCBxrYAauGFfbVcvIbA(this, aDictionary, mapping4.eid_axisX) : CUvgYPcuNWQUoLfxEuBtidskpMACb.jqAWqFpdLUDQRsdZuZzXWwxrjFMg(this), (mapping4 != null) ? JDJOMZTtkxuCBxrYAauGFfbVcvIbA(this, aDictionary, mapping4.eid_axisZ) : CUvgYPcuNWQUoLfxEuBtidskpMACb.jqAWqFpdLUDQRsdZuZzXWwxrjFMg(this));
					break;
				}
				case ControllerTemplateElementType.Stick6D:
				{
					if (specialTemplateElementByElementIdentifierId == null)
					{
						Logger.LogError(templateElementIdentifier2.elementType.ToString() + " element missing for Element Identifier Id " + templateElementIdentifier2.id);
					}
					ControllerTemplateStick6DMapping mapping = specialTemplateElementByElementIdentifierId.GetMapping<ControllerTemplateStick6DMapping>();
					tWsRepKMnZBPynxcHEOrqJwJrtqi2 = new iiKqtnQKUCcgSHGKrWUpGepvbyMab(this, templateElementIdentifier2.id, templateElementIdentifier2.name, (mapping != null) ? JDJOMZTtkxuCBxrYAauGFfbVcvIbA(this, aDictionary, mapping.eid_positionX) : CUvgYPcuNWQUoLfxEuBtidskpMACb.jqAWqFpdLUDQRsdZuZzXWwxrjFMg(this), (mapping != null) ? JDJOMZTtkxuCBxrYAauGFfbVcvIbA(this, aDictionary, mapping.eid_positionY) : CUvgYPcuNWQUoLfxEuBtidskpMACb.jqAWqFpdLUDQRsdZuZzXWwxrjFMg(this), (mapping != null) ? JDJOMZTtkxuCBxrYAauGFfbVcvIbA(this, aDictionary, mapping.eid_positionZ) : CUvgYPcuNWQUoLfxEuBtidskpMACb.jqAWqFpdLUDQRsdZuZzXWwxrjFMg(this), (mapping != null) ? JDJOMZTtkxuCBxrYAauGFfbVcvIbA(this, aDictionary, mapping.eid_rotationX) : CUvgYPcuNWQUoLfxEuBtidskpMACb.jqAWqFpdLUDQRsdZuZzXWwxrjFMg(this), (mapping != null) ? JDJOMZTtkxuCBxrYAauGFfbVcvIbA(this, aDictionary, mapping.eid_rotationY) : CUvgYPcuNWQUoLfxEuBtidskpMACb.jqAWqFpdLUDQRsdZuZzXWwxrjFMg(this), (mapping != null) ? JDJOMZTtkxuCBxrYAauGFfbVcvIbA(this, aDictionary, mapping.eid_rotationZ) : CUvgYPcuNWQUoLfxEuBtidskpMACb.jqAWqFpdLUDQRsdZuZzXWwxrjFMg(this));
					break;
				}
				default:
					throw new NotImplementedException();
				}
				if (tWsRepKMnZBPynxcHEOrqJwJrtqi2 != null)
				{
					list4.Add(tWsRepKMnZBPynxcHEOrqJwJrtqi2);
				}
			}
			for (int n = 0; n < list4.Count; n++)
			{
				list.Add(list4[n]);
				aDictionary.Add(list4[n].id, list4[n]);
			}
			hObmyuQkxgNXDiBtSEwFNgLuBsoZ = list.ToArray();
			DSJJnwwCJPjjjysohBSGxpSbEGtBA = aDictionary;
			fnwsuLHukmUXSqAdAeHEuLmuQsWD = new ADictionary<string, IControllerTemplateElement>();
			for (int num = 0; num < hObmyuQkxgNXDiBtSEwFNgLuBsoZ.Length; num++)
			{
				if (!(dqWEEmjnsXsjFhDjdIFgWVfVygCQA.GetTemplateElementIdentifierById(hObmyuQkxgNXDiBtSEwFNgLuBsoZ[num].id) is IControllerTemplateElementIdentifier_Editor controllerTemplateElementIdentifier_Editor))
				{
					continue;
				}
				for (int num2 = 0; num2 < 2; num2++)
				{
					string text = ((num2 != 0) ? controllerTemplateElementIdentifier_Editor.alternateScriptingName : controllerTemplateElementIdentifier_Editor.scriptingName);
					if (!string.IsNullOrEmpty(text))
					{
						try
						{
							fnwsuLHukmUXSqAdAeHEuLmuQsWD.Add(text, hObmyuQkxgNXDiBtSEwFNgLuBsoZ[num]);
						}
						catch
						{
							Logger.LogError("A duplicate Controller Template element scripting name (" + text + ") was found in template " + hEJFspqWqeBOdedbqHNRpPNEbfBG + ". This element should be renamed to a unique name.");
						}
					}
				}
			}
			VewvmtxDhBKjfOztXaGoFFLyxNDV = new ReadOnlyCollection<IControllerTemplateElement>(hObmyuQkxgNXDiBtSEwFNgLuBsoZ);
		}

		protected IControllerTemplateElement GetElement(int id)
		{
			if (!DSJJnwwCJPjjjysohBSGxpSbEGtBA.TryGetValue(id, out var value))
			{
				Logger.LogWarning("There is no element with the id \"" + id + "\" in the " + GetType().ToString() + ".");
			}
			return value;
		}

		protected T GetElement<T>(int id) where T : class, IControllerTemplateElement
		{
			return GetElement(id) as T;
		}

		IControllerTemplateElement IControllerTemplate.GetElement(int id)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return null;
			}
			return GetElement(id);
		}

		T IControllerTemplate.GetElement<T>(int id)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return null;
			}
			return GetElement<T>(id);
		}

		int IControllerTemplate.GetElementTargets(ControllerElementTarget find, IList<ControllerTemplateElementTarget> results)
		{
			if (ReInput._id != QajDFFaomlkLHzaostfWYpGUioys)
			{
				ReInput.CheckInitialized(QajDFFaomlkLHzaostfWYpGUioys);
				return 0;
			}
			if (results == null)
			{
				throw new ArgumentNullException("results");
			}
			return GetElementTargets(find, ref results);
		}

		private int GetElementTargets(ControllerElementTarget find, ref IList<ControllerTemplateElementTarget> results)
		{
			if (results != null)
			{
				results.Clear();
			}
			int num = 0;
			for (int i = 0; i < hObmyuQkxgNXDiBtSEwFNgLuBsoZ.Length; i++)
			{
				if (InputTools.IsMappableType(hObmyuQkxgNXDiBtSEwFNgLuBsoZ[i].type))
				{
					num += (hObmyuQkxgNXDiBtSEwFNgLuBsoZ[i] as IControllerTemplateElement_Internal).GetElementTargets(find, ref results);
				}
			}
			return num;
		}

		[CustomObfuscation(rename = false)]
		internal static Type GetInterfaceType(ControllerTemplateElementType elementType)
		{
			return elementType switch
			{
				ControllerTemplateElementType.Axis => typeof(IControllerTemplateAxis), 
				ControllerTemplateElementType.Button => typeof(IControllerTemplateButton), 
				ControllerTemplateElementType.ThumbStick => typeof(IControllerTemplateThumbStick), 
				ControllerTemplateElementType.DPad => typeof(IControllerTemplateDPad), 
				ControllerTemplateElementType.Stick => typeof(IControllerTemplateStick), 
				ControllerTemplateElementType.Throttle => typeof(IControllerTemplateThrottle), 
				ControllerTemplateElementType.Hat => typeof(IControllerTemplateHat), 
				ControllerTemplateElementType.Yoke => typeof(IControllerTemplateYoke), 
				ControllerTemplateElementType.Stick6D => typeof(IControllerTemplateStick6D), 
				_ => throw new NotImplementedException(), 
			};
		}

		private static IList<AckvZZpMMZJEeXiTnzpZGygPjFpk> MgpsizkckMCngeoIfdZBxlQJxgpDA(Controller P_0, IControllerTemplateAxisSource P_1)
		{
			if (P_1 == null)
			{
				return null;
			}
			if (P_1.splitAxis)
			{
				IList<AckvZZpMMZJEeXiTnzpZGygPjFpk> list = null;
				bool flag = false;
				if (P_1.positiveTarget != null)
				{
					Controller.Element elementById = P_0.GetElementById(P_1.positiveTarget.elementIdentifierId);
					if (elementById != null)
					{
						ListTools.AddAndCreateList(ref list, new AckvZZpMMZJEeXiTnzpZGygPjFpk(P_1.positiveTarget, elementById));
						flag = true;
					}
				}
				if (!flag)
				{
					ListTools.AddAndCreateList(ref list, AckvZZpMMZJEeXiTnzpZGygPjFpk.jqAWqFpdLUDQRsdZuZzXWwxrjFMg());
				}
				flag = false;
				if (P_1.negativeTarget != null)
				{
					Controller.Element elementById2 = P_0.GetElementById(P_1.negativeTarget.elementIdentifierId);
					if (elementById2 != null)
					{
						ListTools.AddAndCreateList(ref list, new AckvZZpMMZJEeXiTnzpZGygPjFpk(P_1.negativeTarget, elementById2));
						flag = true;
					}
				}
				if (!flag)
				{
					ListTools.AddAndCreateList(ref list, AckvZZpMMZJEeXiTnzpZGygPjFpk.jqAWqFpdLUDQRsdZuZzXWwxrjFMg());
				}
				return list;
			}
			return MgpsizkckMCngeoIfdZBxlQJxgpDA(P_0, P_1.fullTarget);
		}

		private static IList<AckvZZpMMZJEeXiTnzpZGygPjFpk> MgpsizkckMCngeoIfdZBxlQJxgpDA(Controller P_0, IControllerTemplateButtonSource P_1)
		{
			if (P_1 == null)
			{
				return null;
			}
			return MgpsizkckMCngeoIfdZBxlQJxgpDA(P_0, P_1.target);
		}

		private static IList<AckvZZpMMZJEeXiTnzpZGygPjFpk> MgpsizkckMCngeoIfdZBxlQJxgpDA(Controller P_0, IControllerElementTarget P_1)
		{
			if (P_1 == null)
			{
				return null;
			}
			Controller.Element elementById = P_0.GetElementById(P_1.elementIdentifierId);
			if (elementById == null)
			{
				return null;
			}
			return new List<AckvZZpMMZJEeXiTnzpZGygPjFpk>
			{
				new AckvZZpMMZJEeXiTnzpZGygPjFpk(P_1, elementById)
			};
		}

		private static IControllerTemplateElement yauKhkbBGjIEDCJSIEtSjLnuhXfUA(List<IControllerTemplateElement> P_0, int P_1)
		{
			int count = P_0.Count;
			for (int i = 0; i < count; i++)
			{
				if (P_0[i].id == P_1)
				{
					return P_0[i];
				}
			}
			return null;
		}

		private static xplOfuRdmqGAkFpxdfKgUEIduiHT JDJOMZTtkxuCBxrYAauGFfbVcvIbA(IControllerTemplate P_0, ADictionary<int, IControllerTemplateElement> P_1, int P_2)
		{
			if (!(P_1.GetValueSafe(P_2) is xplOfuRdmqGAkFpxdfKgUEIduiHT result))
			{
				return CUvgYPcuNWQUoLfxEuBtidskpMACb.jqAWqFpdLUDQRsdZuZzXWwxrjFMg(P_0);
			}
			return result;
		}

		private static xplOfuRdmqGAkFpxdfKgUEIduiHT ksUjSAMUrwIVaaryoEPEWmsYdJdS(IControllerTemplate P_0, ADictionary<int, IControllerTemplateElement> P_1, int P_2)
		{
			if (!(P_1.GetValueSafe(P_2) is xplOfuRdmqGAkFpxdfKgUEIduiHT result))
			{
				return lxRJFUppkWtxxkUrZqBFElqfoxBT.jqAWqFpdLUDQRsdZuZzXWwxrjFMg(P_0);
			}
			return result;
		}
	}
}
