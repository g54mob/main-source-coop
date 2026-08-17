using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Rewired;
using Rewired.Utils;
using Rewired.Utils.Classes.Data;

internal class BfSuWOtYIJOEShfeXemgQlZkXemn
{
	private class NDrPtvvJOMaYRaXuLfXVfsoPQUUm
	{
		private readonly AList<IControllerTemplate> qYIeCPXgCvdXPlIzrjlonxSUJXvo;

		private IList cCSipqRFPowvQjJRkOUdtOrzBTMKA;

		private IList cwGkuohtdNfdzqarbPbcAgwPbGZ;

		public readonly Type EFJXRExsNqyhnohPNrWxelQkepke;

		public NDrPtvvJOMaYRaXuLfXVfsoPQUUm(Type P_0)
		{
			EFJXRExsNqyhnohPNrWxelQkepke = P_0;
			qYIeCPXgCvdXPlIzrjlonxSUJXvo = new AList<IControllerTemplate>();
		}

		public IList<_0001> XCOFRrHFzCUzwucCIzxGnoBsFgvW<_0001>() where _0001 : IControllerTemplate
		{
			if (cCSipqRFPowvQjJRkOUdtOrzBTMKA == null)
			{
				ykQPDILtAvHZntMQcBCiBhSDbVOc<_0001>();
			}
			return cwGkuohtdNfdzqarbPbcAgwPbGZ as IList<_0001>;
		}

		public void fabOoOFclTovZfaycSDxySsHVFGh(IControllerTemplate P_0)
		{
			if (P_0 != null)
			{
				qYIeCPXgCvdXPlIzrjlonxSUJXvo.Add(P_0);
				if (cCSipqRFPowvQjJRkOUdtOrzBTMKA != null)
				{
					cCSipqRFPowvQjJRkOUdtOrzBTMKA.Add(P_0);
				}
			}
		}

		public void XixaYlBrNeugWSaHdcZJbjdmLjYg(IControllerTemplate P_0)
		{
			if (P_0 != null)
			{
				qYIeCPXgCvdXPlIzrjlonxSUJXvo.Remove(P_0);
				if (cCSipqRFPowvQjJRkOUdtOrzBTMKA != null)
				{
					cCSipqRFPowvQjJRkOUdtOrzBTMKA.Remove(P_0);
				}
			}
		}

		private void ykQPDILtAvHZntMQcBCiBhSDbVOc<_0001>() where _0001 : IControllerTemplate
		{
			cCSipqRFPowvQjJRkOUdtOrzBTMKA = new AList<_0001>();
			cwGkuohtdNfdzqarbPbcAgwPbGZ = new ReadOnlyCollection<_0001>((AList<_0001>)cCSipqRFPowvQjJRkOUdtOrzBTMKA);
			for (int i = 0; i < qYIeCPXgCvdXPlIzrjlonxSUJXvo._count; i++)
			{
				cCSipqRFPowvQjJRkOUdtOrzBTMKA.Add(qYIeCPXgCvdXPlIzrjlonxSUJXvo._items[i]);
			}
		}
	}

	private readonly AList<NDrPtvvJOMaYRaXuLfXVfsoPQUUm> AIqSvmGIlZAxvwwHWobqeBIcikgk;

	private readonly Type[] iGvGRVeQHzWyecFxlnbQVQvfZUOc;

	private readonly Type[] piTNdcdwwoXqPlsxYHNideQUtimc;

	private readonly int bfxhQHKUbQLJuDmrEiCvrlsAPdCsA;

	public BfSuWOtYIJOEShfeXemgQlZkXemn(Type[] P_0, Type[] P_1)
	{
		if (P_0.Length != P_1.Length)
		{
			throw new Exception("Controller template types and controller template interface types array lengths do not match.");
		}
		iGvGRVeQHzWyecFxlnbQVQvfZUOc = P_0;
		piTNdcdwwoXqPlsxYHNideQUtimc = P_1;
		bfxhQHKUbQLJuDmrEiCvrlsAPdCsA = iGvGRVeQHzWyecFxlnbQVQvfZUOc.Length;
		AIqSvmGIlZAxvwwHWobqeBIcikgk = new AList<NDrPtvvJOMaYRaXuLfXVfsoPQUUm>();
		for (int i = 0; i < bfxhQHKUbQLJuDmrEiCvrlsAPdCsA; i++)
		{
			AIqSvmGIlZAxvwwHWobqeBIcikgk.Add(new NDrPtvvJOMaYRaXuLfXVfsoPQUUm(piTNdcdwwoXqPlsxYHNideQUtimc[i]));
		}
	}

	public void twQjFuHMXyfPWoSHuycASCLyVoAM(Controller P_0)
	{
		if (P_0 == null)
		{
			return;
		}
		int templateCount = P_0.templateCount;
		for (int i = 0; i < templateCount; i++)
		{
			IControllerTemplate controllerTemplate = P_0.Templates[i];
			if (controllerTemplate == null)
			{
				Logger.LogError("Template was null.");
				continue;
			}
			Type type = sOQTyXEhBvKxnfXpkgFGRXYYPOBt(controllerTemplate.GetType());
			if (type == null)
			{
				Logger.LogError("Interface type " + controllerTemplate.GetType().Name + " was not found.");
			}
			else
			{
				TOXrVmYRPJwRIhIzAEehzXYTalpaA(type)?.fabOoOFclTovZfaycSDxySsHVFGh(controllerTemplate);
			}
		}
	}

	public void YPbLVCMIvIHWRQTVZLgUrWLBevvq(Controller P_0)
	{
		if (P_0 == null)
		{
			return;
		}
		int templateCount = P_0.templateCount;
		for (int i = 0; i < templateCount; i++)
		{
			IControllerTemplate controllerTemplate = P_0.Templates[i];
			if (controllerTemplate == null)
			{
				Logger.LogError("Template was null.");
				continue;
			}
			Type type = sOQTyXEhBvKxnfXpkgFGRXYYPOBt(controllerTemplate.GetType());
			if (type == null)
			{
				Logger.LogError("Interface type " + controllerTemplate.GetType().Name + " was not found.");
			}
			else
			{
				TOXrVmYRPJwRIhIzAEehzXYTalpaA(type)?.XixaYlBrNeugWSaHdcZJbjdmLjYg(controllerTemplate);
			}
		}
	}

	public IList<_0001> VJzsOuBgNaRhPOlQAJASDbsknCBn<_0001>() where _0001 : IControllerTemplate
	{
		Type typeFromHandle = typeof(_0001);
		for (int i = 0; i < AIqSvmGIlZAxvwwHWobqeBIcikgk._count; i++)
		{
			NDrPtvvJOMaYRaXuLfXVfsoPQUUm nDrPtvvJOMaYRaXuLfXVfsoPQUUm = AIqSvmGIlZAxvwwHWobqeBIcikgk._items[i];
			if ((object)nDrPtvvJOMaYRaXuLfXVfsoPQUUm.EFJXRExsNqyhnohPNrWxelQkepke == typeFromHandle)
			{
				return nDrPtvvJOMaYRaXuLfXVfsoPQUUm.XCOFRrHFzCUzwucCIzxGnoBsFgvW<_0001>();
			}
		}
		string text = "";
		for (int j = 0; j < piTNdcdwwoXqPlsxYHNideQUtimc.Length; j++)
		{
			text += piTNdcdwwoXqPlsxYHNideQUtimc[j].Name;
			if (j != piTNdcdwwoXqPlsxYHNideQUtimc.Length - 1)
			{
				text += "\n";
			}
		}
		Logger.LogError("Invalid Controller Template type \"" + typeFromHandle.Name + "\". Only the following Controller Template interface types are allowed:\n" + text);
		return EmptyObjects<_0001>.EmptyReadOnlyIListT;
	}

	private NDrPtvvJOMaYRaXuLfXVfsoPQUUm TOXrVmYRPJwRIhIzAEehzXYTalpaA(Type P_0)
	{
		for (int i = 0; i < AIqSvmGIlZAxvwwHWobqeBIcikgk._count; i++)
		{
			if ((object)P_0 == AIqSvmGIlZAxvwwHWobqeBIcikgk._items[i].EFJXRExsNqyhnohPNrWxelQkepke)
			{
				return AIqSvmGIlZAxvwwHWobqeBIcikgk._items[i];
			}
		}
		return null;
	}

	private Type sOQTyXEhBvKxnfXpkgFGRXYYPOBt(Type P_0)
	{
		for (int i = 0; i < bfxhQHKUbQLJuDmrEiCvrlsAPdCsA; i++)
		{
			if ((object)iGvGRVeQHzWyecFxlnbQVQvfZUOc[i] == P_0)
			{
				return piTNdcdwwoXqPlsxYHNideQUtimc[i];
			}
		}
		return null;
	}
}
