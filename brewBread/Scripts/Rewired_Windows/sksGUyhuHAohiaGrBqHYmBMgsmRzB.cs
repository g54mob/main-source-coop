using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using Rewired.Utils;

[DefaultMember("Item")]
internal class sksGUyhuHAohiaGrBqHYmBMgsmRzB : IEnumerable<byte>, IDisposable, IEnumerable
{
	private struct VrNBqhBEBGkqPFNqZoVpeyEeOeMgA : IEnumerator<byte>, IDisposable, IEnumerator
	{
		private sksGUyhuHAohiaGrBqHYmBMgsmRzB qWuFmxevgfxfOuvDTdFrvOcscbOR;

		private int lWYXkoDFoSoXRdNhXqVEuUspsLqx;

		public byte Current => qWuFmxevgfxfOuvDTdFrvOcscbOR.odusCDdstdDtOyBggtazUFaPCYwF(lWYXkoDFoSoXRdNhXqVEuUspsLqx);

		object IEnumerator.Current => qWuFmxevgfxfOuvDTdFrvOcscbOR.odusCDdstdDtOyBggtazUFaPCYwF(lWYXkoDFoSoXRdNhXqVEuUspsLqx);

		public VrNBqhBEBGkqPFNqZoVpeyEeOeMgA(sksGUyhuHAohiaGrBqHYmBMgsmRzB P_0)
		{
			qWuFmxevgfxfOuvDTdFrvOcscbOR = P_0;
			lWYXkoDFoSoXRdNhXqVEuUspsLqx = -1;
		}

		public void Dispose()
		{
		}

		public bool MoveNext()
		{
			if (lWYXkoDFoSoXRdNhXqVEuUspsLqx >= qWuFmxevgfxfOuvDTdFrvOcscbOR.CTVzpgRhTiDxbgXTcpWPhKXADeUxA - 1)
			{
				return false;
			}
			lWYXkoDFoSoXRdNhXqVEuUspsLqx++;
			return true;
		}

		public void Reset()
		{
			lWYXkoDFoSoXRdNhXqVEuUspsLqx = 0;
		}
	}

	private int CTVzpgRhTiDxbgXTcpWPhKXADeUxA;

	private unsafe byte* UwuHDIqixLebpujhcqnVmliUjFRjA;

	public int oRjNdVSumbQrnPpjzCOLcZisplBNA => CTVzpgRhTiDxbgXTcpWPhKXADeUxA;

	public unsafe bool JSayXFTNziEjxHctRbOJQBhdcOwp
	{
		get
		{
			if (CTVzpgRhTiDxbgXTcpWPhKXADeUxA <= 0)
			{
				return true;
			}
			return UwuHDIqixLebpujhcqnVmliUjFRjA != null;
		}
	}

	public unsafe byte muarktpyJSWZctSRxmEoxarAcZUv
	{
		get
		{
			if (P_0 < 0 || P_0 >= CTVzpgRhTiDxbgXTcpWPhKXADeUxA)
			{
				throw new IndexOutOfRangeException();
			}
			return UwuHDIqixLebpujhcqnVmliUjFRjA[P_0];
		}
		set
		{
			if (num < 0 || num >= CTVzpgRhTiDxbgXTcpWPhKXADeUxA)
			{
				throw new IndexOutOfRangeException();
			}
			UwuHDIqixLebpujhcqnVmliUjFRjA[num] = b;
		}
	}

	public sksGUyhuHAohiaGrBqHYmBMgsmRzB(int P_0)
	{
		VKWzxEEjqafsNSMawblfEqtHdvmX(P_0);
	}

	public unsafe sksGUyhuHAohiaGrBqHYmBMgsmRzB(params byte[] P_0)
		: this(P_0.Length)
	{
		Marshal.Copy(P_0, 0, (IntPtr)UwuHDIqixLebpujhcqnVmliUjFRjA, P_0.Length);
	}

	public sksGUyhuHAohiaGrBqHYmBMgsmRzB(sksGUyhuHAohiaGrBqHYmBMgsmRzB P_0)
		: this(P_0.CTVzpgRhTiDxbgXTcpWPhKXADeUxA)
	{
		P_0.BLqgPdOCPoGteBSAuNYyfLdTFwpJ(this, 0, P_0.CTVzpgRhTiDxbgXTcpWPhKXADeUxA);
	}

	public unsafe sksGUyhuHAohiaGrBqHYmBMgsmRzB(byte* P_0, int P_1)
		: this(P_1)
	{
		EigdSMgqkBzyFeSpQPsmKNepnjhE.RnAWqRWOrAiuOkNLYFPejoszYblU(P_0, UwuHDIqixLebpujhcqnVmliUjFRjA, 0, 0, P_1);
	}

	public unsafe bool BLqgPdOCPoGteBSAuNYyfLdTFwpJ(byte* P_0, int P_1, int P_2, int P_3, bool P_4 = true)
	{
		if (P_0 == null)
		{
			if (P_4)
			{
				throw new ArgumentNullException("destination");
			}
			return false;
		}
		if (P_2 < 0 || P_2 >= CTVzpgRhTiDxbgXTcpWPhKXADeUxA || P_2 >= P_1)
		{
			if (P_4)
			{
				throw new IndexOutOfRangeException("startIndex");
			}
			return false;
		}
		if (P_3 <= 0 || P_3 > CTVzpgRhTiDxbgXTcpWPhKXADeUxA || P_3 > P_1)
		{
			if (P_4)
			{
				throw new ArgumentOutOfRangeException("length");
			}
			return false;
		}
		int num = P_3 + P_2;
		if (num >= CTVzpgRhTiDxbgXTcpWPhKXADeUxA || num >= P_1)
		{
			if (P_4)
			{
				throw new ArgumentOutOfRangeException("startIndex + length must be < Length of either array");
			}
			return false;
		}
		return EigdSMgqkBzyFeSpQPsmKNepnjhE.RnAWqRWOrAiuOkNLYFPejoszYblU(UwuHDIqixLebpujhcqnVmliUjFRjA, P_0, P_2, P_2, P_3);
	}

	public unsafe bool BLqgPdOCPoGteBSAuNYyfLdTFwpJ(sksGUyhuHAohiaGrBqHYmBMgsmRzB P_0, int P_1, int P_2, bool P_3 = true)
	{
		if (P_0 == null)
		{
			if (P_3)
			{
				throw new ArgumentNullException("destination");
			}
			return false;
		}
		return BLqgPdOCPoGteBSAuNYyfLdTFwpJ(P_0.UwuHDIqixLebpujhcqnVmliUjFRjA, P_0.CTVzpgRhTiDxbgXTcpWPhKXADeUxA, P_1, P_2, P_3);
	}

	public unsafe bool BLqgPdOCPoGteBSAuNYyfLdTFwpJ(byte[] P_0, int P_1, int P_2, bool P_3 = true)
	{
		if (P_0 == null)
		{
			if (P_3)
			{
				throw new ArgumentNullException("destination");
			}
			return false;
		}
		if (P_1 < 0 || P_1 >= CTVzpgRhTiDxbgXTcpWPhKXADeUxA || P_1 >= P_0.Length)
		{
			if (P_3)
			{
				throw new IndexOutOfRangeException("startIndex");
			}
			return false;
		}
		if (P_2 <= 0 || P_2 > CTVzpgRhTiDxbgXTcpWPhKXADeUxA || P_2 > P_0.Length)
		{
			if (P_3)
			{
				throw new ArgumentOutOfRangeException("length");
			}
			return false;
		}
		int num = P_2 + P_1;
		if (num >= CTVzpgRhTiDxbgXTcpWPhKXADeUxA || num >= P_0.Length)
		{
			if (P_3)
			{
				throw new ArgumentOutOfRangeException("startIndex + length must be < Length of either array");
			}
			return false;
		}
		return NativeTools.CopyMemory((IntPtr)UwuHDIqixLebpujhcqnVmliUjFRjA, P_0, P_1, P_1, P_2, P_3);
	}

	public unsafe bool BLqgPdOCPoGteBSAuNYyfLdTFwpJ(byte* P_0, int P_1, int P_2, int P_3, int P_4, bool P_5 = true)
	{
		if (P_0 == null)
		{
			if (P_5)
			{
				throw new ArgumentNullException("destination");
			}
			return false;
		}
		if (P_2 < 0 || P_2 >= CTVzpgRhTiDxbgXTcpWPhKXADeUxA)
		{
			if (P_5)
			{
				throw new IndexOutOfRangeException("startIndex");
			}
			return false;
		}
		if (P_3 < 0 || P_3 >= P_1)
		{
			if (P_5)
			{
				throw new IndexOutOfRangeException("startIndex");
			}
			return false;
		}
		if (P_4 <= 0 || P_4 > CTVzpgRhTiDxbgXTcpWPhKXADeUxA || P_4 > P_1)
		{
			if (P_5)
			{
				throw new ArgumentOutOfRangeException("length");
			}
			return false;
		}
		if (P_4 + P_2 >= CTVzpgRhTiDxbgXTcpWPhKXADeUxA)
		{
			if (P_5)
			{
				throw new ArgumentOutOfRangeException("sourceStartIndex + length must be < source.Length");
			}
			return false;
		}
		if (P_4 + P_3 >= P_1)
		{
			if (P_5)
			{
				throw new ArgumentOutOfRangeException("destinationStartIndex + length must be < destination.Length");
			}
			return false;
		}
		return EigdSMgqkBzyFeSpQPsmKNepnjhE.RnAWqRWOrAiuOkNLYFPejoszYblU(UwuHDIqixLebpujhcqnVmliUjFRjA, P_0, P_2, P_3, P_4);
	}

	public unsafe bool BLqgPdOCPoGteBSAuNYyfLdTFwpJ(sksGUyhuHAohiaGrBqHYmBMgsmRzB P_0, int P_1, int P_2, int P_3, bool P_4 = true)
	{
		if (P_0 == null)
		{
			if (P_4)
			{
				throw new ArgumentNullException("destination");
			}
			return false;
		}
		return BLqgPdOCPoGteBSAuNYyfLdTFwpJ(P_0.UwuHDIqixLebpujhcqnVmliUjFRjA, P_0.CTVzpgRhTiDxbgXTcpWPhKXADeUxA, P_1, P_2, P_3, P_4);
	}

	public unsafe bool BLqgPdOCPoGteBSAuNYyfLdTFwpJ(byte[] P_0, int P_1, int P_2, int P_3, bool P_4 = true)
	{
		if (P_0 == null)
		{
			if (P_4)
			{
				throw new ArgumentNullException("destination");
			}
			return false;
		}
		if (P_1 < 0 || P_1 >= CTVzpgRhTiDxbgXTcpWPhKXADeUxA)
		{
			if (P_4)
			{
				throw new IndexOutOfRangeException("startIndex");
			}
			return false;
		}
		if (P_2 < 0 || P_2 >= P_0.Length)
		{
			if (P_4)
			{
				throw new IndexOutOfRangeException("startIndex");
			}
			return false;
		}
		if (P_3 <= 0 || P_3 > CTVzpgRhTiDxbgXTcpWPhKXADeUxA || P_3 > P_0.Length)
		{
			if (P_4)
			{
				throw new ArgumentOutOfRangeException("length");
			}
			return false;
		}
		if (P_3 + P_1 >= CTVzpgRhTiDxbgXTcpWPhKXADeUxA)
		{
			if (P_4)
			{
				throw new ArgumentOutOfRangeException("sourceStartIndex + length must be < source.Length");
			}
			return false;
		}
		if (P_3 + P_2 >= P_0.Length)
		{
			if (P_4)
			{
				throw new ArgumentOutOfRangeException("destinationStartIndex + length must be < destination.Length");
			}
			return false;
		}
		return NativeTools.CopyMemory((IntPtr)UwuHDIqixLebpujhcqnVmliUjFRjA, P_0, P_1, P_2, P_3, P_4);
	}

	public unsafe bool BUqLbIMmctxGPvYuZDoYpLlVESzX(byte* P_0, int P_1, int P_2, int P_3)
	{
		if (P_0 == null)
		{
			return false;
		}
		if (P_2 >= CTVzpgRhTiDxbgXTcpWPhKXADeUxA || P_2 >= P_1)
		{
			return false;
		}
		if (P_2 < 0)
		{
			P_2 = 0;
		}
		int num = P_3 + P_2;
		if (num >= CTVzpgRhTiDxbgXTcpWPhKXADeUxA)
		{
			P_3 = CTVzpgRhTiDxbgXTcpWPhKXADeUxA - P_2;
		}
		if (num >= P_1)
		{
			P_3 = P_1 - P_2;
		}
		if (P_3 <= 0)
		{
			return false;
		}
		return EigdSMgqkBzyFeSpQPsmKNepnjhE.RnAWqRWOrAiuOkNLYFPejoszYblU(UwuHDIqixLebpujhcqnVmliUjFRjA, P_0, P_2, P_2, P_3);
	}

	public unsafe bool BUqLbIMmctxGPvYuZDoYpLlVESzX(sksGUyhuHAohiaGrBqHYmBMgsmRzB P_0, int P_1, int P_2)
	{
		if (P_0 == null)
		{
			return false;
		}
		return BUqLbIMmctxGPvYuZDoYpLlVESzX(P_0.UwuHDIqixLebpujhcqnVmliUjFRjA, P_0.CTVzpgRhTiDxbgXTcpWPhKXADeUxA, P_1, P_2);
	}

	public unsafe bool BUqLbIMmctxGPvYuZDoYpLlVESzX(byte[] P_0, int P_1, int P_2)
	{
		if (P_0 == null)
		{
			return false;
		}
		if (P_1 >= CTVzpgRhTiDxbgXTcpWPhKXADeUxA || P_1 >= P_0.Length)
		{
			return false;
		}
		if (P_1 < 0)
		{
			P_1 = 0;
		}
		int num = P_2 + P_1;
		if (num >= CTVzpgRhTiDxbgXTcpWPhKXADeUxA)
		{
			P_2 = CTVzpgRhTiDxbgXTcpWPhKXADeUxA - P_1;
		}
		if (num >= P_0.Length)
		{
			P_2 = P_0.Length - P_1;
		}
		if (P_2 <= 0)
		{
			return false;
		}
		return NativeTools.CopyMemory((IntPtr)UwuHDIqixLebpujhcqnVmliUjFRjA, P_0, P_1, P_1, P_2, throwOnError: false);
	}

	public unsafe bool BUqLbIMmctxGPvYuZDoYpLlVESzX(byte* P_0, int P_1, int P_2, int P_3, int P_4)
	{
		if (P_0 == null)
		{
			return false;
		}
		if (P_2 >= CTVzpgRhTiDxbgXTcpWPhKXADeUxA)
		{
			return false;
		}
		if (P_3 >= P_1)
		{
			return false;
		}
		if (P_2 < 0)
		{
			P_2 = 0;
		}
		if (P_3 < 0)
		{
			P_3 = 0;
		}
		if (P_4 + P_2 >= CTVzpgRhTiDxbgXTcpWPhKXADeUxA)
		{
			P_4 = CTVzpgRhTiDxbgXTcpWPhKXADeUxA - P_2;
		}
		if (P_4 + P_3 >= P_1)
		{
			P_4 = P_1 - P_3;
		}
		if (P_4 <= 0)
		{
			return false;
		}
		return EigdSMgqkBzyFeSpQPsmKNepnjhE.RnAWqRWOrAiuOkNLYFPejoszYblU(UwuHDIqixLebpujhcqnVmliUjFRjA, P_0, P_2, P_3, P_4);
	}

	public unsafe bool BUqLbIMmctxGPvYuZDoYpLlVESzX(sksGUyhuHAohiaGrBqHYmBMgsmRzB P_0, int P_1, int P_2, int P_3)
	{
		if (P_0 == null)
		{
			return false;
		}
		return BUqLbIMmctxGPvYuZDoYpLlVESzX(P_0.UwuHDIqixLebpujhcqnVmliUjFRjA, P_0.CTVzpgRhTiDxbgXTcpWPhKXADeUxA, P_1, P_2, P_3);
	}

	public unsafe bool BUqLbIMmctxGPvYuZDoYpLlVESzX(byte[] P_0, int P_1, int P_2, int P_3)
	{
		if (P_0 == null)
		{
			return false;
		}
		if (P_1 >= CTVzpgRhTiDxbgXTcpWPhKXADeUxA)
		{
			return false;
		}
		if (P_2 >= P_0.Length)
		{
			return false;
		}
		if (P_1 < 0)
		{
			P_1 = 0;
		}
		if (P_2 < 0)
		{
			P_2 = 0;
		}
		if (P_3 + P_1 >= CTVzpgRhTiDxbgXTcpWPhKXADeUxA)
		{
			P_3 = CTVzpgRhTiDxbgXTcpWPhKXADeUxA - P_1;
		}
		if (P_3 + P_2 >= P_0.Length)
		{
			P_3 = P_0.Length - P_2;
		}
		if (P_3 <= 0)
		{
			return false;
		}
		return NativeTools.CopyMemory((IntPtr)UwuHDIqixLebpujhcqnVmliUjFRjA, P_0, P_1, P_2, P_3, throwOnError: false);
	}

	public void wZhBNjzUhcxMVUfUPRlDRkEipJDq(int P_0)
	{
		if (P_0 < 0)
		{
			throw new ArgumentOutOfRangeException("length must be >= 0");
		}
		if (CTVzpgRhTiDxbgXTcpWPhKXADeUxA != P_0)
		{
			VKWzxEEjqafsNSMawblfEqtHdvmX(P_0);
		}
	}

	public unsafe void ZrbFhGEbWRbTzVxxQimUntnkwisKA()
	{
		if (CTVzpgRhTiDxbgXTcpWPhKXADeUxA != 0 && UwuHDIqixLebpujhcqnVmliUjFRjA != null)
		{
			EigdSMgqkBzyFeSpQPsmKNepnjhE.PnrdEHoiFUoVjeoZDBZbwGDJGFrGA(UwuHDIqixLebpujhcqnVmliUjFRjA, CTVzpgRhTiDxbgXTcpWPhKXADeUxA);
		}
	}

	private unsafe void VKWzxEEjqafsNSMawblfEqtHdvmX(int P_0)
	{
		if (P_0 == CTVzpgRhTiDxbgXTcpWPhKXADeUxA)
		{
			ZrbFhGEbWRbTzVxxQimUntnkwisKA();
			return;
		}
		if (CTVzpgRhTiDxbgXTcpWPhKXADeUxA > 0)
		{
			KEqdnGbXoElmEwtwpmkOhyFfvtEAB();
		}
		UwuHDIqixLebpujhcqnVmliUjFRjA = (byte*)(void*)Marshal.AllocHGlobal(P_0);
		if (UwuHDIqixLebpujhcqnVmliUjFRjA == null)
		{
			throw new Exception("Could not allocate memory for array.");
		}
		CTVzpgRhTiDxbgXTcpWPhKXADeUxA = P_0;
		ZrbFhGEbWRbTzVxxQimUntnkwisKA();
	}

	private unsafe void KEqdnGbXoElmEwtwpmkOhyFfvtEAB()
	{
		if (UwuHDIqixLebpujhcqnVmliUjFRjA != null)
		{
			Marshal.FreeHGlobal((IntPtr)UwuHDIqixLebpujhcqnVmliUjFRjA);
		}
		UwuHDIqixLebpujhcqnVmliUjFRjA = null;
		CTVzpgRhTiDxbgXTcpWPhKXADeUxA = 0;
	}

	public void Dispose()
	{
		lDxnsjCDTQrmresvWgbliNUVruIc(true);
		GC.SuppressFinalize(this);
	}

	protected virtual void zNJVymYugIbeeZuNgMrKxyYWbziV()
	{
		try
		{
			lDxnsjCDTQrmresvWgbliNUVruIc(false);
		}
		finally
		{
			base.Finalize();
		}
	}

	protected void lDxnsjCDTQrmresvWgbliNUVruIc(bool P_0)
	{
		KEqdnGbXoElmEwtwpmkOhyFfvtEAB();
	}

	public IEnumerator<byte> GetEnumerator()
	{
		return new VrNBqhBEBGkqPFNqZoVpeyEeOeMgA(this);
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return new VrNBqhBEBGkqPFNqZoVpeyEeOeMgA(this);
	}
}
