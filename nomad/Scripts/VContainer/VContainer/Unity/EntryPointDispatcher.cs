using System;
using System.Collections.Generic;
using UnityEngine;
using VContainer.Internal;

namespace VContainer.Unity
{
	public sealed class EntryPointDispatcher : IDisposable
	{
		private readonly IObjectResolver container;

		private readonly CompositeDisposable disposable = new CompositeDisposable();

		[Inject]
		public EntryPointDispatcher(IObjectResolver container)
		{
			this.container = container;
		}

		public void Dispatch()
		{
			PlayerLoopHelper.EnsureInitialized();
			EntryPointExceptionHandler entryPointExceptionHandler = container.ResolveOrDefault<EntryPointExceptionHandler>();
			IReadOnlyList<IInitializable> value = container.Resolve<ContainerLocal<IReadOnlyList<IInitializable>>>().Value;
			for (int i = 0; i < value.Count; i++)
			{
				try
				{
					value[i].Initialize();
				}
				catch (Exception ex)
				{
					if (entryPointExceptionHandler != null)
					{
						entryPointExceptionHandler.Publish(ex);
					}
					else
					{
						Debug.LogException(ex);
					}
				}
			}
			IReadOnlyList<IPostInitializable> value2 = container.Resolve<ContainerLocal<IReadOnlyList<IPostInitializable>>>().Value;
			for (int j = 0; j < value2.Count; j++)
			{
				try
				{
					value2[j].PostInitialize();
				}
				catch (Exception ex2)
				{
					if (entryPointExceptionHandler != null)
					{
						entryPointExceptionHandler.Publish(ex2);
					}
					else
					{
						Debug.LogException(ex2);
					}
				}
			}
			IReadOnlyList<IStartable> value3 = container.Resolve<ContainerLocal<IReadOnlyList<IStartable>>>().Value;
			if (value3.Count > 0)
			{
				StartableLoopItem item = new StartableLoopItem(value3, entryPointExceptionHandler);
				disposable.Add(item);
				PlayerLoopHelper.Dispatch(PlayerLoopTiming.Startup, item);
			}
			IReadOnlyList<IPostStartable> value4 = container.Resolve<ContainerLocal<IReadOnlyList<IPostStartable>>>().Value;
			if (value4.Count > 0)
			{
				PostStartableLoopItem item2 = new PostStartableLoopItem(value4, entryPointExceptionHandler);
				disposable.Add(item2);
				PlayerLoopHelper.Dispatch(PlayerLoopTiming.PostStartup, item2);
			}
			IReadOnlyList<IFixedTickable> value5 = container.Resolve<ContainerLocal<IReadOnlyList<IFixedTickable>>>().Value;
			if (value5.Count > 0)
			{
				FixedTickableLoopItem item3 = new FixedTickableLoopItem(value5, entryPointExceptionHandler);
				disposable.Add(item3);
				PlayerLoopHelper.Dispatch(PlayerLoopTiming.FixedUpdate, item3);
			}
			IReadOnlyList<IPostFixedTickable> value6 = container.Resolve<ContainerLocal<IReadOnlyList<IPostFixedTickable>>>().Value;
			if (value6.Count > 0)
			{
				PostFixedTickableLoopItem item4 = new PostFixedTickableLoopItem(value6, entryPointExceptionHandler);
				disposable.Add(item4);
				PlayerLoopHelper.Dispatch(PlayerLoopTiming.PostFixedUpdate, item4);
			}
			IReadOnlyList<ITickable> value7 = container.Resolve<ContainerLocal<IReadOnlyList<ITickable>>>().Value;
			if (value7.Count > 0)
			{
				TickableLoopItem item5 = new TickableLoopItem(value7, entryPointExceptionHandler);
				disposable.Add(item5);
				PlayerLoopHelper.Dispatch(PlayerLoopTiming.Update, item5);
			}
			IReadOnlyList<IPostTickable> value8 = container.Resolve<ContainerLocal<IReadOnlyList<IPostTickable>>>().Value;
			if (value8.Count > 0)
			{
				PostTickableLoopItem item6 = new PostTickableLoopItem(value8, entryPointExceptionHandler);
				disposable.Add(item6);
				PlayerLoopHelper.Dispatch(PlayerLoopTiming.PostUpdate, item6);
			}
			IReadOnlyList<ILateTickable> value9 = container.Resolve<ContainerLocal<IReadOnlyList<ILateTickable>>>().Value;
			if (value9.Count > 0)
			{
				LateTickableLoopItem item7 = new LateTickableLoopItem(value9, entryPointExceptionHandler);
				disposable.Add(item7);
				PlayerLoopHelper.Dispatch(PlayerLoopTiming.LateUpdate, item7);
			}
			IReadOnlyList<IPostLateTickable> value10 = container.Resolve<ContainerLocal<IReadOnlyList<IPostLateTickable>>>().Value;
			if (value10.Count > 0)
			{
				PostLateTickableLoopItem item8 = new PostLateTickableLoopItem(value10, entryPointExceptionHandler);
				disposable.Add(item8);
				PlayerLoopHelper.Dispatch(PlayerLoopTiming.PostLateUpdate, item8);
			}
			IReadOnlyList<IAsyncStartable> value11 = container.Resolve<ContainerLocal<IReadOnlyList<IAsyncStartable>>>().Value;
			if (value11.Count > 0)
			{
				AsyncStartableLoopItem item9 = new AsyncStartableLoopItem(value11, entryPointExceptionHandler);
				disposable.Add(item9);
				PlayerLoopHelper.Dispatch(PlayerLoopTiming.Startup, item9);
			}
		}

		public void Dispose()
		{
			disposable.Dispose();
		}
	}
}
