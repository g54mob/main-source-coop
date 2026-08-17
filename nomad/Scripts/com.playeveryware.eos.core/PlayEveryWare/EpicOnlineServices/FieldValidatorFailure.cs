using System.Reflection;

namespace PlayEveryWare.EpicOnlineServices
{
	public struct FieldValidatorFailure
	{
		public FieldInfo FieldInfo;

		public FieldValidatorAttribute FailingAttribute;

		public string FailingMessage;

		public FieldValidatorFailure(FieldInfo failingField, FieldValidatorAttribute failingAttribute, string failingMessage)
		{
			FieldInfo = failingField;
			FailingAttribute = failingAttribute;
			FailingMessage = failingMessage;
		}
	}
}
