using System;
using System.Collections.Generic;
using System.Reflection;

namespace QFSW.QC
{
	public class LambdaCommandData : CommandData
	{
		private readonly object _lambdaTarget;

		public LambdaCommandData(Delegate lambda, string commandName, string commandDescription = "")
			: base(lambda.Method, new CommandAttribute(commandName, commandDescription, MonoTargetType.Registry, Platform.AllPlatforms))
		{
			_lambdaTarget = lambda.Target;
		}

		protected override IEnumerable<object> GetInvocationTargets(MethodInfo invokingMethod)
		{
			yield return _lambdaTarget;
		}
	}
}
