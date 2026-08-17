using System;
using Rewired;
using Rewired.Utils.Classes.Data;

internal sealed class JBObjUemFLdkzOFbxqzCefghedsSc<_0001> where _0001 : class
{
	private readonly IndexedDictionary<uint, WeakReference> BjxznWFwTJfttvAtpEkgvlRPkGhi;

	private Id SeZkiiCJlQDdsnoEkcpNbNhudwfZA;

	private double NzjACzeonIgEkOywNfvTSMSeUDjm;

	private float UpUxlBbWnWsncNGRmlTqokudfaaB;

	public JBObjUemFLdkzOFbxqzCefghedsSc()
	{
		BjxznWFwTJfttvAtpEkgvlRPkGhi = new IndexedDictionary<uint, WeakReference>();
		SeZkiiCJlQDdsnoEkcpNbNhudwfZA = 1u;
	}

	public JBObjUemFLdkzOFbxqzCefghedsSc(float P_0)
		: this()
	{
		UpUxlBbWnWsncNGRmlTqokudfaaB = P_0;
	}

	public bool DteszXvNHQfqVeNHoeHVMuDqjWvW(uint P_0, out _0001 P_1)
	{
		if (!BjxznWFwTJfttvAtpEkgvlRPkGhi.TryGetValue(P_0, out var value))
		{
			P_1 = null;
			return false;
		}
		if (!(value.Target is _0001 val))
		{
			BjxznWFwTJfttvAtpEkgvlRPkGhi.Remove(P_0);
			P_1 = null;
			return false;
		}
		P_1 = val;
		return true;
	}

	public uint sJQDtVtbrpkDDcVlnWqHJwMgozbN(_0001 P_0)
	{
		if (P_0 == null)
		{
			throw new ArgumentNullException();
		}
		YmoZzDzMjkkcATsfPDuwzWRNgsYk();
		BjxznWFwTJfttvAtpEkgvlRPkGhi.SetValue(SeZkiiCJlQDdsnoEkcpNbNhudwfZA.id, new WeakReference(P_0, trackResurrection: false));
		SeZkiiCJlQDdsnoEkcpNbNhudwfZA.Increment();
		return SeZkiiCJlQDdsnoEkcpNbNhudwfZA.id;
	}

	public bool hexsubMGcGqoTRCNpeAuAKJQpCdz(uint P_0)
	{
		YmoZzDzMjkkcATsfPDuwzWRNgsYk();
		return BjxznWFwTJfttvAtpEkgvlRPkGhi.Remove(P_0);
	}

	public void PecQYXbrNbodqHbeNaKQJKgkJILn()
	{
		for (int num = BjxznWFwTJfttvAtpEkgvlRPkGhi.Count - 1; num >= 0; num--)
		{
			if (!BjxznWFwTJfttvAtpEkgvlRPkGhi[num].IsAlive)
			{
				BjxznWFwTJfttvAtpEkgvlRPkGhi.RemoveAt(num);
			}
		}
		NzjACzeonIgEkOywNfvTSMSeUDjm = ReInput.unscaledTime + (double)UpUxlBbWnWsncNGRmlTqokudfaaB;
	}

	public void zitygvTOkbnKqqpSWWQDpLAXkwTP(Action<_0001> P_0)
	{
		for (int num = BjxznWFwTJfttvAtpEkgvlRPkGhi.Count - 1; num >= 0; num--)
		{
			if (!(BjxznWFwTJfttvAtpEkgvlRPkGhi[num].Target is _0001 obj))
			{
				BjxznWFwTJfttvAtpEkgvlRPkGhi.RemoveAt(num);
			}
			else
			{
				P_0(obj);
			}
		}
		NzjACzeonIgEkOywNfvTSMSeUDjm = ReInput.unscaledTime + (double)UpUxlBbWnWsncNGRmlTqokudfaaB;
	}

	private void YmoZzDzMjkkcATsfPDuwzWRNgsYk()
	{
		if (!(UpUxlBbWnWsncNGRmlTqokudfaaB <= 0f) && ReInput.unscaledTime > NzjACzeonIgEkOywNfvTSMSeUDjm)
		{
			PecQYXbrNbodqHbeNaKQJKgkJILn();
		}
	}
}
