using System;

[AttributeUsage(AttributeTargets.Interface)]
internal class attzlEyCPCThJZBaKBlUbGRHIeJbb : Attribute
{
	private Type bQxispwTyMaWqZOFhwYzNNoSkMgi;

	public Type lhIrpCzAlkfgVHpeLkMrWybTDiDQA => bQxispwTyMaWqZOFhwYzNNoSkMgi;

	public attzlEyCPCThJZBaKBlUbGRHIeJbb(Type P_0)
	{
		bQxispwTyMaWqZOFhwYzNNoSkMgi = P_0;
	}

	public static attzlEyCPCThJZBaKBlUbGRHIeJbb bzntzGEOucjfXHIGhmznjiQKaxfE(Type P_0)
	{
		object[] customAttributes = P_0.GetCustomAttributes(typeof(attzlEyCPCThJZBaKBlUbGRHIeJbb), inherit: false);
		if (customAttributes.Length == 0)
		{
			return null;
		}
		return (attzlEyCPCThJZBaKBlUbGRHIeJbb)customAttributes[0];
	}
}
