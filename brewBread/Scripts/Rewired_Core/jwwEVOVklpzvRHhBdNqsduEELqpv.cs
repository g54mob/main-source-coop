using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Rewired;
using Rewired.Utils;
using Rewired.Utils.Classes.Data;

internal class jwwEVOVklpzvRHhBdNqsduEELqpv
{
	private class hRriVntNPQpVRwjlSpXyLPLiSgUR
	{
		private readonly AList<IControllerTemplate> jaEouEThGebUjciilFRztgFLhYrT;

		private IList XxErZGdBlNpCgIgOlnKuZxFfAdly;

		private IList gCKuDkiVDhhHsKIPeOiUjWafLdmc;

		public readonly Type gwUdWoefqvdKVvJvsShVNYBtVNpSA;

		public hRriVntNPQpVRwjlSpXyLPLiSgUR(Type P_0)
		{
			gwUdWoefqvdKVvJvsShVNYBtVNpSA = P_0;
			jaEouEThGebUjciilFRztgFLhYrT = new AList<IControllerTemplate>();
		}

		public IList<_0001> eSISbadMeLSkFsmVxVmjgTxXfofi<_0001>() where _0001 : IControllerTemplate
		{
			if (XxErZGdBlNpCgIgOlnKuZxFfAdly == null)
			{
				bDyxWTOhNJUYpTJTkBACwZeSMEAe<_0001>();
			}
			return gCKuDkiVDhhHsKIPeOiUjWafLdmc as IList<_0001>;
		}

		public void BcNRXlUcIeCeQoTBnFpegDhuaAap(IControllerTemplate P_0)
		{
			if (P_0 != null)
			{
				jaEouEThGebUjciilFRztgFLhYrT.Add(P_0);
				if (XxErZGdBlNpCgIgOlnKuZxFfAdly != null)
				{
					XxErZGdBlNpCgIgOlnKuZxFfAdly.Add(P_0);
				}
			}
		}

		public void qZjUkljUkFefeYpopqopvDZOAIkDA(IControllerTemplate P_0)
		{
			if (P_0 != null)
			{
				jaEouEThGebUjciilFRztgFLhYrT.Remove(P_0);
				if (XxErZGdBlNpCgIgOlnKuZxFfAdly != null)
				{
					XxErZGdBlNpCgIgOlnKuZxFfAdly.Remove(P_0);
				}
			}
		}

		private void bDyxWTOhNJUYpTJTkBACwZeSMEAe<_0001>() where _0001 : IControllerTemplate
		{
			XxErZGdBlNpCgIgOlnKuZxFfAdly = new AList<_0001>();
			gCKuDkiVDhhHsKIPeOiUjWafLdmc = new ReadOnlyCollection<_0001>((AList<_0001>)XxErZGdBlNpCgIgOlnKuZxFfAdly);
			for (int i = 0; i < jaEouEThGebUjciilFRztgFLhYrT._count; i++)
			{
				XxErZGdBlNpCgIgOlnKuZxFfAdly.Add(jaEouEThGebUjciilFRztgFLhYrT._items[i]);
			}
		}
	}

	private readonly AList<hRriVntNPQpVRwjlSpXyLPLiSgUR> uLVFtSGVlWpKOAXAzHqzFEiYcoKMA;

	private readonly Type[] LvwAEWfqEvyBEskcOpqZhwMkDmLab;

	private readonly Type[] nVDiGEdrgsjzlIZZuMNxNLNlttDL;

	private readonly int VKYfdXZgBsfFFfgVnQwYHUlvyeVI;

	public jwwEVOVklpzvRHhBdNqsduEELqpv(Type[] P_0, Type[] P_1)
	{
		if (P_0.Length != P_1.Length)
		{
			throw new Exception("Controller template types and controller template interface types array lengths do not match.");
		}
		LvwAEWfqEvyBEskcOpqZhwMkDmLab = P_0;
		nVDiGEdrgsjzlIZZuMNxNLNlttDL = P_1;
		VKYfdXZgBsfFFfgVnQwYHUlvyeVI = LvwAEWfqEvyBEskcOpqZhwMkDmLab.Length;
		uLVFtSGVlWpKOAXAzHqzFEiYcoKMA = new AList<hRriVntNPQpVRwjlSpXyLPLiSgUR>();
		for (int i = 0; i < VKYfdXZgBsfFFfgVnQwYHUlvyeVI; i++)
		{
			uLVFtSGVlWpKOAXAzHqzFEiYcoKMA.Add(new hRriVntNPQpVRwjlSpXyLPLiSgUR(nVDiGEdrgsjzlIZZuMNxNLNlttDL[i]));
		}
	}

	public void ycndZKmKnXmWelVDbcsLODEZWhMV(Controller P_0)
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
			Type type = ISCqdCJzybcUPFIAIEFNJArpZmcX(controllerTemplate.GetType());
			if ((object)type == null)
			{
				Logger.LogError("Interface type " + controllerTemplate.GetType().Name + " was not found.");
			}
			else
			{
				bglCShHBYVzBwMJHPLvSIDkCZATeA(type)?.BcNRXlUcIeCeQoTBnFpegDhuaAap(controllerTemplate);
			}
		}
	}

	public void FcypqzUtVfbvdvFSJGJKEXpZVBLaA(Controller P_0)
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
			Type type = ISCqdCJzybcUPFIAIEFNJArpZmcX(controllerTemplate.GetType());
			if ((object)type == null)
			{
				Logger.LogError("Interface type " + controllerTemplate.GetType().Name + " was not found.");
			}
			else
			{
				bglCShHBYVzBwMJHPLvSIDkCZATeA(type)?.qZjUkljUkFefeYpopqopvDZOAIkDA(controllerTemplate);
			}
		}
	}

	public IList<_0001> eSISbadMeLSkFsmVxVmjgTxXfofi<_0001>() where _0001 : IControllerTemplate
	{
		Type typeFromHandle = typeof(_0001);
		for (int i = 0; i < uLVFtSGVlWpKOAXAzHqzFEiYcoKMA._count; i++)
		{
			hRriVntNPQpVRwjlSpXyLPLiSgUR hRriVntNPQpVRwjlSpXyLPLiSgUR2 = uLVFtSGVlWpKOAXAzHqzFEiYcoKMA._items[i];
			if ((object)hRriVntNPQpVRwjlSpXyLPLiSgUR2.gwUdWoefqvdKVvJvsShVNYBtVNpSA == typeFromHandle)
			{
				return hRriVntNPQpVRwjlSpXyLPLiSgUR2.eSISbadMeLSkFsmVxVmjgTxXfofi<_0001>();
			}
		}
		string text = "";
		for (int j = 0; j < nVDiGEdrgsjzlIZZuMNxNLNlttDL.Length; j++)
		{
			text += nVDiGEdrgsjzlIZZuMNxNLNlttDL[j].Name;
			if (j != nVDiGEdrgsjzlIZZuMNxNLNlttDL.Length - 1)
			{
				text += "\n";
			}
		}
		Logger.LogError("Invalid Controller Template type \"" + typeFromHandle.Name + "\". Only the following Controller Template interface types are allowed:\n" + text);
		return EmptyObjects<_0001>.EmptyReadOnlyIListT;
	}

	private hRriVntNPQpVRwjlSpXyLPLiSgUR bglCShHBYVzBwMJHPLvSIDkCZATeA(Type P_0)
	{
		for (int i = 0; i < uLVFtSGVlWpKOAXAzHqzFEiYcoKMA._count; i++)
		{
			if ((object)P_0 == uLVFtSGVlWpKOAXAzHqzFEiYcoKMA._items[i].gwUdWoefqvdKVvJvsShVNYBtVNpSA)
			{
				return uLVFtSGVlWpKOAXAzHqzFEiYcoKMA._items[i];
			}
		}
		return null;
	}

	private Type ISCqdCJzybcUPFIAIEFNJArpZmcX(Type P_0)
	{
		for (int i = 0; i < VKYfdXZgBsfFFfgVnQwYHUlvyeVI; i++)
		{
			if ((object)LvwAEWfqEvyBEskcOpqZhwMkDmLab[i] == P_0)
			{
				return nVDiGEdrgsjzlIZZuMNxNLNlttDL[i];
			}
		}
		return null;
	}
}
