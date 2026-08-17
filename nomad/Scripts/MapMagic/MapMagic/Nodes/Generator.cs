using System;
using System.Runtime.CompilerServices;
using Den.Tools;
using Den.Tools.Matrices;
using Den.Tools.Splines;
using MapMagic.Products;
using UnityEngine;

namespace MapMagic.Nodes
{
	[Serializable]
	public abstract class Generator : IUnit
	{
		public bool enabled = true;

		public ulong id;

		public ulong version;

		public Vector2 guiPosition;

		public Vector2 guiSize;

		public bool guiPreview;

		public bool guiAdvanced;

		public bool guiDebug;

		public static Action<Generator> OnGeneratorCreated;

		public ulong Id
		{
			get
			{
				return id;
			}
			set
			{
				id = value;
			}
		}

		public ulong LinkedOutletId { get; set; }

		public ulong LinkedGenId { get; set; }

		public Generator Gen => this;

		public Type AlternativeSerializationType => typeof(Placeholders.InletOutletPlaceholder);

		public void SetGen(Generator gen)
		{
		}

		public static Generator Create(Type type)
		{
			if (type.IsGenericTypeDefinition)
			{
				type = type.MakeGenericType(typeof(MatrixWorld));
			}
			Generator generator = (Generator)Activator.CreateInstance(type);
			generator.id = Den.Tools.Id.Generate();
			if (generator is IMultiLayer multiLayer)
			{
				foreach (IUnit layer in multiLayer.Layers)
				{
					layer.SetGen(generator);
					layer.Id = Den.Tools.Id.Generate();
				}
			}
			if (generator is IMultiInlet multiInlet)
			{
				foreach (IInlet<object> item in multiInlet.Inlets())
				{
					item.SetGen(generator);
					item.Id = Den.Tools.Id.Generate();
				}
			}
			if (generator is IMultiOutlet multiOutlet)
			{
				foreach (IOutlet<object> item2 in multiOutlet.Outlets())
				{
					item2.SetGen(generator);
					item2.Id = Den.Tools.Id.Generate();
				}
			}
			OnGeneratorCreated?.Invoke(generator);
			return generator;
		}

		public IUnit ShallowCopy()
		{
			return (Generator)MemberwiseClone();
		}

		public abstract void Generate(TileData data, StopToken stop);

		public static Type GetGenericType(Type type)
		{
			Type[] interfaces = type.GetInterfaces();
			foreach (Type type2 in interfaces)
			{
				if (type2.IsGenericType)
				{
					return type2.GenericTypeArguments[0];
				}
			}
			return null;
		}

		public static Type GetGenericType<T>(IOutlet<T> outlet) where T : class
		{
			return typeof(T);
		}

		public static Type GetGenericType<T>(IInlet<T> inlet) where T : class
		{
			return typeof(T);
		}

		public static Type GetGenericType(Generator gen)
		{
			if (gen is IOutlet<object> outlet)
			{
				return GetGenericType(outlet);
			}
			if (gen is IInlet<object> inlet)
			{
				return GetGenericType(inlet);
			}
			return null;
		}

		public static Type GetGenericType(IOutlet<object> outlet)
		{
			if (outlet is IOutlet<MatrixWorld>)
			{
				return typeof(MatrixWorld);
			}
			if (outlet is IOutlet<TransitionsList>)
			{
				return typeof(TransitionsList);
			}
			if (outlet is IOutlet<SplineSys>)
			{
				return typeof(SplineSys);
			}
			return GetGenericType(outlet.GetType());
		}

		public static Type GetGenericType(IInlet<object> inlet)
		{
			if (inlet is IInlet<MatrixWorld>)
			{
				return typeof(MatrixWorld);
			}
			if (inlet is IInlet<TransitionsList>)
			{
				return typeof(TransitionsList);
			}
			if (inlet is IInlet<SplineSys>)
			{
				return typeof(SplineSys);
			}
			return GetGenericType(inlet.GetType());
		}

		public virtual (string, int) GetCodeFileLine()
		{
			return GetCodeFileLineBase("A:\\Nomad Drive Folder\\NomadDrive\\Assets\\Excluded\\ThirdParty\\MapMagic\\Nodes\\Generator.cs", 353);
		}

		public (string, int) GetCodeFileLineBase([CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
		{
			return (sourceFilePath, sourceLineNumber);
		}
	}
}
