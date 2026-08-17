using System;
using Rewired;

internal sealed class xcslkxDzwrCojABLPbRuUvYdnRhl : IControllerTemplateElementSource, IControllerTemplateAxisSource, IControllerTemplateButtonSource
{
	private ControllerTemplateElementType MqsQRKsObPagAnqzOHpNqiCzICnU;

	private bool UgfDvrDFJlPDsgHYvPmMEMlHPdDpA;

	private IControllerElementTarget FQNeNgjgMBglfXyFZaJJDgEcPwORb;

	private IControllerElementTarget RBAzdUJWJKFRoACyTSPSBAzcsuTL;

	private IControllerElementTarget fnthzgRFBvxrLifFzUwEpmRzufsS;

	ControllerTemplateElementSourceType IControllerTemplateElementSource.type => cVDyIiOsEfJNYzVuZSmuEXqylgT.xgduMFNQeYnnCfHZPecJsjqUfFKZ(MqsQRKsObPagAnqzOHpNqiCzICnU, false);

	bool IControllerTemplateAxisSource.splitAxis => UgfDvrDFJlPDsgHYvPmMEMlHPdDpA;

	IControllerElementTarget IControllerTemplateAxisSource.fullTarget => FQNeNgjgMBglfXyFZaJJDgEcPwORb;

	IControllerElementTarget IControllerTemplateAxisSource.positiveTarget => RBAzdUJWJKFRoACyTSPSBAzcsuTL;

	IControllerElementTarget IControllerTemplateAxisSource.negativeTarget => fnthzgRFBvxrLifFzUwEpmRzufsS;

	IControllerElementTarget IControllerTemplateButtonSource.target => FQNeNgjgMBglfXyFZaJJDgEcPwORb;

	internal xcslkxDzwrCojABLPbRuUvYdnRhl(ControllerTemplateElementType P_0, bool P_1, IControllerElementTarget P_2, IControllerElementTarget P_3, IControllerElementTarget P_4)
	{
		if (P_2 == null)
		{
			throw new ArgumentNullException("target");
		}
		if (P_4 == null)
		{
			throw new ArgumentNullException("positiveTarget");
		}
		if (P_3 == null)
		{
			throw new ArgumentNullException("negativeTarget");
		}
		MqsQRKsObPagAnqzOHpNqiCzICnU = P_0;
		UgfDvrDFJlPDsgHYvPmMEMlHPdDpA = P_1;
		FQNeNgjgMBglfXyFZaJJDgEcPwORb = P_2;
		RBAzdUJWJKFRoACyTSPSBAzcsuTL = P_3;
		fnthzgRFBvxrLifFzUwEpmRzufsS = P_4;
	}

	internal static xcslkxDzwrCojABLPbRuUvYdnRhl szNaYxvmrjQKoEMODBrltQFmmKyi(ControllerTemplateElementType P_0)
	{
		return new xcslkxDzwrCojABLPbRuUvYdnRhl(P_0, false, SzcVmbDpoJahYmnXXukLaOXfCanz.MoUHBwpcMangYCquFpcvJDNGaBMD(), SzcVmbDpoJahYmnXXukLaOXfCanz.MoUHBwpcMangYCquFpcvJDNGaBMD(), SzcVmbDpoJahYmnXXukLaOXfCanz.MoUHBwpcMangYCquFpcvJDNGaBMD());
	}
}
