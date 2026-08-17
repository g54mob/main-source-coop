using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;

internal class bGEdnRkfMmdsZAxxyHBUfUYaptKhB
{
	[CompilerGenerated]
	private DateTime LjXMtUoSrHzdlytBSLUXgGNIYTgJ;

	[CompilerGenerated]
	private WeakReference jNWXgBAciWShxVjjhhWPDorBgIiMA;

	[CompilerGenerated]
	private string MYkGlweJruNrgRdYxhCvNutAaseVA;

	public DateTime UpqIGvacMmXizaBmJbGtfRsBYORe
	{
		[CompilerGenerated]
		get
		{
			return LjXMtUoSrHzdlytBSLUXgGNIYTgJ;
		}
		[CompilerGenerated]
		private set
		{
			LjXMtUoSrHzdlytBSLUXgGNIYTgJ = ljXMtUoSrHzdlytBSLUXgGNIYTgJ;
		}
	}

	public WeakReference xlclGijLhuUnADiPEOnaeNLGlAzP
	{
		[CompilerGenerated]
		get
		{
			return jNWXgBAciWShxVjjhhWPDorBgIiMA;
		}
		[CompilerGenerated]
		private set
		{
			jNWXgBAciWShxVjjhhWPDorBgIiMA = weakReference;
		}
	}

	public string QFOtlTcrriYcHdBNEFpkkaXWXQzP
	{
		[CompilerGenerated]
		get
		{
			return MYkGlweJruNrgRdYxhCvNutAaseVA;
		}
		[CompilerGenerated]
		private set
		{
			MYkGlweJruNrgRdYxhCvNutAaseVA = mYkGlweJruNrgRdYxhCvNutAaseVA;
		}
	}

	public bool vrjEfMEhSWNyHgIccyqabVcQxbdOA => xlclGijLhuUnADiPEOnaeNLGlAzP.IsAlive;

	public bGEdnRkfMmdsZAxxyHBUfUYaptKhB(DateTime P_0, UMBfGVQXygOTeNKYveUhyPWibjPb P_1, string P_2)
	{
		UpqIGvacMmXizaBmJbGtfRsBYORe = P_0;
		xlclGijLhuUnADiPEOnaeNLGlAzP = new WeakReference(P_1, trackResurrection: true);
		QFOtlTcrriYcHdBNEFpkkaXWXQzP = P_2;
	}

	public virtual string GFrJAlTMaWterKRJyEKenILZzzqq()
	{
		if (!(xlclGijLhuUnADiPEOnaeNLGlAzP.Target is UMBfGVQXygOTeNKYveUhyPWibjPb uMBfGVQXygOTeNKYveUhyPWibjPb))
		{
			return "";
		}
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "Active COM Object: [0x{0:X}] Class: [{1}] Time [{2}] Stack:\r\n{3}", uMBfGVQXygOTeNKYveUhyPWibjPb.KUJrOBoduRcGKIkxgMoKicmgtCeb.ToInt64(), uMBfGVQXygOTeNKYveUhyPWibjPb.GetType().FullName, UpqIGvacMmXizaBmJbGtfRsBYORe, QFOtlTcrriYcHdBNEFpkkaXWXQzP).AppendLine();
		return stringBuilder.ToString();
	}
}
