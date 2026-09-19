using System;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Scripting;

namespace Fusion
{
	[Preserve]
	public class FusionGlobalScriptableObjectAddressAttribute : FusionGlobalScriptableObjectSourceAttribute
	{
		public string Address { get; }

		public FusionGlobalScriptableObjectAddressAttribute(Type objectType, string address)
			: base(objectType)
		{
			Address = address;
		}

		public override FusionGlobalScriptableObjectLoadResult Load(Type type)
		{
			AsyncOperationHandle<FusionGlobalScriptableObject> op = Addressables.LoadAssetAsync<FusionGlobalScriptableObject>(Address);
			FusionGlobalScriptableObject obj = op.WaitForCompletion();
			if (op.Status == AsyncOperationStatus.Succeeded)
			{
				return new FusionGlobalScriptableObjectLoadResult(obj, delegate
				{
					Addressables.Release(op);
				});
			}
			return default(FusionGlobalScriptableObjectLoadResult);
		}

		public override Task<FusionGlobalScriptableObjectLoadResult> LoadAsync(Type type)
		{
			TaskCompletionSource<FusionGlobalScriptableObjectLoadResult> tcs = new TaskCompletionSource<FusionGlobalScriptableObjectLoadResult>();
			AsyncOperationHandle<FusionGlobalScriptableObject> asyncOperationHandle = Addressables.LoadAssetAsync<FusionGlobalScriptableObject>(Address);
			asyncOperationHandle.Completed += delegate(AsyncOperationHandle<FusionGlobalScriptableObject> _op)
			{
				if (_op.Status == AsyncOperationStatus.Succeeded)
				{
					tcs.SetResult(new FusionGlobalScriptableObjectLoadResult(_op.Result, delegate
					{
						Addressables.Release(_op);
					}));
				}
				else
				{
					tcs.SetException(_op.OperationException);
				}
			};
			return tcs.Task;
		}
	}
}
