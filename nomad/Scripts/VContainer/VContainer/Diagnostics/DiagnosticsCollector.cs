using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using VContainer.Internal;

namespace VContainer.Diagnostics
{
	public sealed class DiagnosticsCollector
	{
		private readonly List<DiagnosticsInfo> diagnosticsInfos = new List<DiagnosticsInfo>();

		private readonly ThreadLocal<Stack<DiagnosticsInfo>> resolveCallStack = new ThreadLocal<Stack<DiagnosticsInfo>>(() => new Stack<DiagnosticsInfo>());

		public string ScopeName { get; }

		public DiagnosticsCollector(string scopeName)
		{
			ScopeName = scopeName;
		}

		public IReadOnlyList<DiagnosticsInfo> GetDiagnosticsInfos()
		{
			return diagnosticsInfos;
		}

		public void Clear()
		{
			lock (diagnosticsInfos)
			{
				diagnosticsInfos.Clear();
			}
		}

		public void TraceRegister(RegisterInfo registerInfo)
		{
			lock (diagnosticsInfos)
			{
				diagnosticsInfos.Add(new DiagnosticsInfo(ScopeName, registerInfo));
			}
		}

		public void TraceBuild(RegistrationBuilder registrationBuilder, Registration registration)
		{
			lock (diagnosticsInfos)
			{
				foreach (DiagnosticsInfo diagnosticsInfo in diagnosticsInfos)
				{
					if (diagnosticsInfo.RegisterInfo.RegistrationBuilder == registrationBuilder)
					{
						diagnosticsInfo.ResolveInfo = new ResolveInfo(registration);
						break;
					}
				}
			}
		}

		public object TraceResolve(Registration registration, Func<Registration, object> resolving)
		{
			DiagnosticsInfo diagnosticsInfo = DiagnositcsContext.FindByRegistration(registration);
			DiagnosticsInfo diagnosticsInfo2 = ((resolveCallStack.Value.Count > 0) ? resolveCallStack.Value.Peek() : null);
			if (!(registration.Provider is CollectionInstanceProvider) && diagnosticsInfo != null && diagnosticsInfo != diagnosticsInfo2)
			{
				diagnosticsInfo.ResolveInfo.RefCount++;
				diagnosticsInfo.ResolveInfo.MaxDepth = ((diagnosticsInfo.ResolveInfo.MaxDepth < 0) ? resolveCallStack.Value.Count : Math.Max(diagnosticsInfo.ResolveInfo.MaxDepth, resolveCallStack.Value.Count));
				diagnosticsInfo2?.Dependencies.Add(diagnosticsInfo);
				resolveCallStack.Value.Push(diagnosticsInfo);
				Stopwatch stopwatch = Stopwatch.StartNew();
				object obj = resolving(registration);
				stopwatch.Stop();
				resolveCallStack.Value.Pop();
				SetResolveTime(diagnosticsInfo, stopwatch.ElapsedMilliseconds);
				if (!diagnosticsInfo.ResolveInfo.Instances.Contains(obj))
				{
					diagnosticsInfo.ResolveInfo.Instances.Add(obj);
				}
				return obj;
			}
			return resolving(registration);
		}

		private static void SetResolveTime(DiagnosticsInfo current, long elapsedMilliseconds)
		{
			int refCount = current.ResolveInfo.RefCount;
			long num = current.ResolveInfo.ResolveTime;
			switch (current.ResolveInfo.Registration.Lifetime)
			{
			case Lifetime.Transient:
				num = (num * (refCount - 1) + elapsedMilliseconds) / refCount;
				break;
			case Lifetime.Singleton:
			case Lifetime.Scoped:
				if (elapsedMilliseconds > num)
				{
					num = elapsedMilliseconds;
				}
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
			current.ResolveInfo.ResolveTime = num;
		}

		public void NotifyContainerBuilt(IObjectResolver container)
		{
			DiagnositcsContext.NotifyContainerBuilt(container);
		}
	}
}
