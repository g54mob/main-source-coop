using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using MessagePack.Internal;

namespace MessagePack
{
	public class MessagePackSecurity
	{
		private class HashResistantCache<T>
		{
			internal static readonly IEqualityComparer<T>? EqualityComparer;

			static HashResistantCache()
			{
				object equalityComparer2;
				if (!(typeof(T) == typeof(bool)))
				{
					if (!(typeof(T) == typeof(char)))
					{
						if (!(typeof(T) == typeof(sbyte)))
						{
							if (!(typeof(T) == typeof(byte)))
							{
								if (!(typeof(T) == typeof(short)))
								{
									if (!(typeof(T) == typeof(ushort)))
									{
										if (!(typeof(T) == typeof(int)))
										{
											if (!(typeof(T) == typeof(uint)))
											{
												if (!(typeof(T) == typeof(long)))
												{
													if (!(typeof(T) == typeof(ulong)))
													{
														if (!(typeof(T) == typeof(Guid)))
														{
															if (!(typeof(T) == typeof(float)))
															{
																if (!(typeof(T) == typeof(double)))
																{
																	if (!(typeof(T) == typeof(string)))
																	{
																		if (!(typeof(T) == typeof(DateTime)))
																		{
																			if (!(typeof(T) == typeof(DateTimeOffset)))
																			{
																				if (typeof(T).GetTypeInfo().IsEnum)
																				{
																					Type enumUnderlyingType = typeof(T).GetTypeInfo().GetEnumUnderlyingType();
																					if ((object)enumUnderlyingType != null)
																					{
																						if (!(enumUnderlyingType == typeof(byte)))
																						{
																							if (!(enumUnderlyingType == typeof(sbyte)))
																							{
																								if (!(enumUnderlyingType == typeof(ushort)))
																								{
																									if (!(enumUnderlyingType == typeof(short)))
																									{
																										if (!(enumUnderlyingType == typeof(uint)))
																										{
																											if (!(enumUnderlyingType == typeof(int)))
																											{
																												if (!(enumUnderlyingType == typeof(ulong)))
																												{
																													IEqualityComparer<T> equalityComparer = ((enumUnderlyingType == typeof(long)) ? CollisionResistantEnumHasher<T, long>.Instance : null);
																													equalityComparer2 = equalityComparer;
																												}
																												else
																												{
																													IEqualityComparer<T> equalityComparer = CollisionResistantEnumHasher<T, ulong>.Instance;
																													equalityComparer2 = equalityComparer;
																												}
																											}
																											else
																											{
																												IEqualityComparer<T> equalityComparer = CollisionResistantEnumHasher<T, int>.Instance;
																												equalityComparer2 = equalityComparer;
																											}
																										}
																										else
																										{
																											IEqualityComparer<T> equalityComparer = CollisionResistantEnumHasher<T, uint>.Instance;
																											equalityComparer2 = equalityComparer;
																										}
																									}
																									else
																									{
																										IEqualityComparer<T> equalityComparer = CollisionResistantEnumHasher<T, short>.Instance;
																										equalityComparer2 = equalityComparer;
																									}
																								}
																								else
																								{
																									IEqualityComparer<T> equalityComparer = CollisionResistantEnumHasher<T, ushort>.Instance;
																									equalityComparer2 = equalityComparer;
																								}
																							}
																							else
																							{
																								IEqualityComparer<T> equalityComparer = CollisionResistantEnumHasher<T, sbyte>.Instance;
																								equalityComparer2 = equalityComparer;
																							}
																						}
																						else
																						{
																							IEqualityComparer<T> equalityComparer = CollisionResistantEnumHasher<T, byte>.Instance;
																							equalityComparer2 = equalityComparer;
																						}
																						goto IL_03dd;
																					}
																				}
																				equalityComparer2 = null;
																			}
																			else
																			{
																				equalityComparer2 = (IEqualityComparer<T>)DateTimeOffsetEqualityComparer.Instance;
																			}
																		}
																		else
																		{
																			equalityComparer2 = (IEqualityComparer<T>)DateTimeEqualityComparer.Instance;
																		}
																	}
																	else
																	{
																		equalityComparer2 = (IEqualityComparer<T>)StringEqualityComparer.Instance;
																	}
																}
																else
																{
																	equalityComparer2 = (IEqualityComparer<T>)DoubleEqualityComparer.Instance;
																}
															}
															else
															{
																equalityComparer2 = (IEqualityComparer<T>)SingleEqualityComparer.Instance;
															}
														}
														else
														{
															equalityComparer2 = (IEqualityComparer<T>)CollisionResistantHasherUnmanaged<Guid>.Instance;
														}
													}
													else
													{
														equalityComparer2 = (IEqualityComparer<T>)CollisionResistantHasherUnmanaged<ulong>.Instance;
													}
												}
												else
												{
													equalityComparer2 = (IEqualityComparer<T>)CollisionResistantHasherUnmanaged<long>.Instance;
												}
											}
											else
											{
												equalityComparer2 = (IEqualityComparer<T>)CollisionResistantHasherUnmanaged<uint>.Instance;
											}
										}
										else
										{
											equalityComparer2 = (IEqualityComparer<T>)CollisionResistantHasherUnmanaged<int>.Instance;
										}
									}
									else
									{
										equalityComparer2 = (IEqualityComparer<T>)CollisionResistantHasherUnmanaged<ushort>.Instance;
									}
								}
								else
								{
									equalityComparer2 = (IEqualityComparer<T>)CollisionResistantHasherUnmanaged<short>.Instance;
								}
							}
							else
							{
								equalityComparer2 = (IEqualityComparer<T>)CollisionResistantHasherUnmanaged<byte>.Instance;
							}
						}
						else
						{
							equalityComparer2 = (IEqualityComparer<T>)CollisionResistantHasherUnmanaged<sbyte>.Instance;
						}
					}
					else
					{
						equalityComparer2 = (IEqualityComparer<T>)CollisionResistantHasherUnmanaged<char>.Instance;
					}
				}
				else
				{
					equalityComparer2 = (IEqualityComparer<T>)CollisionResistantHasherUnmanaged<bool>.Instance;
				}
				goto IL_03dd;
				IL_03dd:
				EqualityComparer = (IEqualityComparer<T>?)equalityComparer2;
			}
		}

		private abstract class CollisionResistantHasher<T> : IEqualityComparer<T>, IEqualityComparer
		{
			public bool Equals(T? x, T? y)
			{
				return EqualityComparer<T>.Default.Equals(x, y);
			}

			bool IEqualityComparer.Equals(object? x, object? y)
			{
				return ((IEqualityComparer)EqualityComparer<T>.Default).Equals(x, y);
			}

			public int GetHashCode(object obj)
			{
				return GetHashCode((T)obj);
			}

			public abstract int GetHashCode(T value);
		}

		private class CollisionResistantHasherUnmanaged<T> : CollisionResistantHasher<T> where T : unmanaged
		{
			internal static readonly CollisionResistantHasherUnmanaged<T> Instance = new CollisionResistantHasherUnmanaged<T>();

			public override int GetHashCode(T value)
			{
				return SecureHash(value);
			}
		}

		private class ObjectFallbackEqualityComparer : IEqualityComparer<object>, IEqualityComparer
		{
			private static readonly Lazy<MethodInfo> GetHashCollisionResistantEqualityComparerOpenGenericMethod = new Lazy<MethodInfo>(() => typeof(MessagePackSecurity).GetTypeInfo().DeclaredMethods.Single((MethodInfo m) => m.Name == "GetHashCollisionResistantEqualityComparer" && m.IsGenericMethod));

			private readonly MessagePackSecurity security;

			private readonly ThreadsafeTypeKeyHashTable<IEqualityComparer> equalityComparerCache = new ThreadsafeTypeKeyHashTable<IEqualityComparer>();

			internal ObjectFallbackEqualityComparer(MessagePackSecurity security)
			{
				this.security = security ?? throw new ArgumentNullException("security");
			}

			bool IEqualityComparer<object>.Equals(object? x, object? y)
			{
				return EqualityComparer<object>.Default.Equals(x, y);
			}

			bool IEqualityComparer.Equals(object? x, object? y)
			{
				return ((IEqualityComparer)EqualityComparer<object>.Default).Equals(x, y);
			}

			public int GetHashCode(object value)
			{
				if (value == null)
				{
					return 0;
				}
				Type type = value.GetType();
				if (type == typeof(object))
				{
					return value.GetHashCode();
				}
				if (!equalityComparerCache.TryGetValue(type, out IEqualityComparer value2))
				{
					try
					{
						value2 = (IEqualityComparer)GetHashCollisionResistantEqualityComparerOpenGenericMethod.Value.MakeGenericMethod(type).Invoke(security, Array.Empty<object>());
					}
					catch (TargetInvocationException ex) when (ex.InnerException != null)
					{
						ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
						throw null;
					}
					equalityComparerCache.TryAdd(type, value2);
				}
				return value2.GetHashCode(value);
			}
		}

		private class SingleEqualityComparer : CollisionResistantHasherUnmanaged<float>
		{
			internal new static readonly SingleEqualityComparer Instance = new SingleEqualityComparer();

			public override int GetHashCode(float value)
			{
				float value2 = ((value == 0f) ? 0f : ((!float.IsNaN(value)) ? value : float.NaN));
				return base.GetHashCode(value2);
			}
		}

		private class DoubleEqualityComparer : CollisionResistantHasherUnmanaged<double>
		{
			internal new static readonly DoubleEqualityComparer Instance = new DoubleEqualityComparer();

			public override int GetHashCode(double value)
			{
				double value2 = ((value == 0.0) ? 0.0 : ((!double.IsNaN(value)) ? value : double.NaN));
				return base.GetHashCode(value2);
			}
		}

		private class DateTimeEqualityComparer : CollisionResistantHasherUnmanaged<DateTime>
		{
			internal new static readonly DateTimeEqualityComparer Instance = new DateTimeEqualityComparer();

			public override int GetHashCode(DateTime value)
			{
				return SecureHash(value.Ticks);
			}
		}

		private class DateTimeOffsetEqualityComparer : CollisionResistantHasherUnmanaged<DateTimeOffset>
		{
			internal new static readonly DateTimeOffsetEqualityComparer Instance = new DateTimeOffsetEqualityComparer();

			public override int GetHashCode(DateTimeOffset value)
			{
				return SecureHash(value.UtcDateTime.Ticks);
			}
		}

		private class StringEqualityComparer : CollisionResistantHasher<string>
		{
			internal static readonly StringEqualityComparer Instance = new StringEqualityComparer();

			public override int GetHashCode(string value)
			{
				return SecureHash(MemoryMarshal.Cast<char, byte>(value.AsSpan()));
			}
		}

		private class CollisionResistantEnumHasher<TEnum, TUnderlying> : IEqualityComparer<TEnum>, IEqualityComparer where TUnderlying : unmanaged
		{
			internal static readonly CollisionResistantEnumHasher<TEnum, TUnderlying> Instance = new CollisionResistantEnumHasher<TEnum, TUnderlying>();

			public bool Equals(TEnum? x, TEnum? y)
			{
				return EqualityComparer<TEnum>.Default.Equals(x, y);
			}

			public int GetHashCode(TEnum obj)
			{
				return SecureHash(Unsafe.As<TEnum, TUnderlying>(ref obj));
			}

			bool IEqualityComparer.Equals(object? x, object? y)
			{
				if (x is TEnum x2 && y is TEnum y2)
				{
					return Equals(x2, y2);
				}
				return false;
			}

			int IEqualityComparer.GetHashCode(object obj)
			{
				return GetHashCode((TEnum)obj);
			}
		}

		public static readonly MessagePackSecurity TrustedData = new MessagePackSecurity
		{
			HashCollisionResistant = false,
			MaximumObjectGraphDepth = 500
		};

		public static readonly MessagePackSecurity UntrustedData = new MessagePackSecurity
		{
			HashCollisionResistant = true,
			MaximumObjectGraphDepth = 500
		};

		private static readonly SipHash Hash = new SipHash();

		private readonly ObjectFallbackEqualityComparer objectFallbackEqualityComparer;

		public bool HashCollisionResistant { get; private set; }

		public int MaximumObjectGraphDepth { get; private set; } = 500;

		private MessagePackSecurity()
		{
			objectFallbackEqualityComparer = new ObjectFallbackEqualityComparer(this);
		}

		protected MessagePackSecurity(MessagePackSecurity copyFrom)
			: this()
		{
			if (copyFrom == null)
			{
				throw new ArgumentNullException("copyFrom");
			}
			HashCollisionResistant = copyFrom.HashCollisionResistant;
			MaximumObjectGraphDepth = copyFrom.MaximumObjectGraphDepth;
		}

		public MessagePackSecurity WithMaximumObjectGraphDepth(int maximumObjectGraphDepth)
		{
			if (MaximumObjectGraphDepth == maximumObjectGraphDepth)
			{
				return this;
			}
			MessagePackSecurity messagePackSecurity = Clone();
			messagePackSecurity.MaximumObjectGraphDepth = maximumObjectGraphDepth;
			return messagePackSecurity;
		}

		public MessagePackSecurity WithHashCollisionResistant(bool hashCollisionResistant)
		{
			if (HashCollisionResistant == hashCollisionResistant)
			{
				return this;
			}
			MessagePackSecurity messagePackSecurity = Clone();
			messagePackSecurity.HashCollisionResistant = hashCollisionResistant;
			return messagePackSecurity;
		}

		public IEqualityComparer<T> GetEqualityComparer<T>()
		{
			if (!HashCollisionResistant)
			{
				return EqualityComparer<T>.Default;
			}
			return GetHashCollisionResistantEqualityComparer<T>();
		}

		public IEqualityComparer GetEqualityComparer()
		{
			if (!HashCollisionResistant)
			{
				return EqualityComparer<object>.Default;
			}
			return GetHashCollisionResistantEqualityComparer();
		}

		protected virtual IEqualityComparer<T> GetHashCollisionResistantEqualityComparer<T>()
		{
			IEqualityComparer<T> equalityComparer = HashResistantCache<T>.EqualityComparer;
			if (equalityComparer != null)
			{
				return equalityComparer;
			}
			if (typeof(T) == typeof(object))
			{
				return (IEqualityComparer<T>)objectFallbackEqualityComparer;
			}
			throw new TypeAccessException($"No hash-resistant equality comparer available for type: {typeof(T)}");
		}

		public void DepthStep(ref MessagePackReader reader)
		{
			if (reader.Depth >= MaximumObjectGraphDepth)
			{
				throw new InsufficientExecutionStackException($"This msgpack sequence has an object graph that exceeds the maximum depth allowed of {MaximumObjectGraphDepth}.");
			}
			checked
			{
				reader.Depth++;
			}
		}

		protected virtual IEqualityComparer GetHashCollisionResistantEqualityComparer()
		{
			return (IEqualityComparer)GetHashCollisionResistantEqualityComparer<object>();
		}

		protected virtual MessagePackSecurity Clone()
		{
			return new MessagePackSecurity(this);
		}

		private static int SecureHash<T>(T value) where T : unmanaged
		{
			Span<T> span = stackalloc T[1];
			span[0] = value;
			return (int)Hash.Compute(MemoryMarshal.Cast<T, byte>(span));
		}

		private static int SecureHash(ReadOnlySpan<byte> data)
		{
			return (int)Hash.Compute(data);
		}
	}
}
