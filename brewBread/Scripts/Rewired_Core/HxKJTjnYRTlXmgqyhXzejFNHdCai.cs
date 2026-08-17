using System;
using Rewired;

internal sealed class HxKJTjnYRTlXmgqyhXzejFNHdCai : IControllerTemplateElementSource, IControllerTemplateAxisSource, IControllerTemplateButtonSource
{
	private ControllerTemplateElementType RUyyQSEmqRidLNphokXShBrzehNy;

	private bool cifaubFgEtUIjCdUvPvNAShDcUJvB;

	private IControllerElementTarget WdbNWWRsjxFdwKePDCTZQcFZsADA;

	private IControllerElementTarget JoKqYIKMlofOxRIcPhUqksJskSwh;

	private IControllerElementTarget MzqTRBXjswEIgkpsvjPDHxpkoIHCA;

	ControllerTemplateElementSourceType IControllerTemplateElementSource.type => OzpbBOHoTkskKeBaOXWaTXQCmjjBA.oDUYYDBtfTaUYefFeWLVhqWxwMSv(RUyyQSEmqRidLNphokXShBrzehNy, false);

	bool IControllerTemplateAxisSource.splitAxis => cifaubFgEtUIjCdUvPvNAShDcUJvB;

	IControllerElementTarget IControllerTemplateAxisSource.fullTarget => WdbNWWRsjxFdwKePDCTZQcFZsADA;

	IControllerElementTarget IControllerTemplateAxisSource.positiveTarget => JoKqYIKMlofOxRIcPhUqksJskSwh;

	IControllerElementTarget IControllerTemplateAxisSource.negativeTarget => MzqTRBXjswEIgkpsvjPDHxpkoIHCA;

	IControllerElementTarget IControllerTemplateButtonSource.target => WdbNWWRsjxFdwKePDCTZQcFZsADA;

	internal HxKJTjnYRTlXmgqyhXzejFNHdCai(ControllerTemplateElementType P_0, bool P_1, IControllerElementTarget P_2, IControllerElementTarget P_3, IControllerElementTarget P_4)
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
		RUyyQSEmqRidLNphokXShBrzehNy = P_0;
		cifaubFgEtUIjCdUvPvNAShDcUJvB = P_1;
		WdbNWWRsjxFdwKePDCTZQcFZsADA = P_2;
		JoKqYIKMlofOxRIcPhUqksJskSwh = P_3;
		MzqTRBXjswEIgkpsvjPDHxpkoIHCA = P_4;
	}

	internal static HxKJTjnYRTlXmgqyhXzejFNHdCai jqAWqFpdLUDQRsdZuZzXWwxrjFMg(ControllerTemplateElementType P_0)
	{
		return new HxKJTjnYRTlXmgqyhXzejFNHdCai(P_0, false, cyWFKpvAJpSlVUbktJsNPZKHVsyU.jqAWqFpdLUDQRsdZuZzXWwxrjFMg(), cyWFKpvAJpSlVUbktJsNPZKHVsyU.jqAWqFpdLUDQRsdZuZzXWwxrjFMg(), cyWFKpvAJpSlVUbktJsNPZKHVsyU.jqAWqFpdLUDQRsdZuZzXWwxrjFMg());
	}
}
