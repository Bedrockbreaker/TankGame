using System;
using System.Collections.Generic;

using Unity.VisualScripting;

using UnityEngine;

namespace Util {
	/**
	 * <summary>
	 * An optional value
	 * </summary>
	 */
	[Serializable]
	public struct Optional<T> : ISerializationCallbackReceiver {

		[SerializeField]
		private T value;
		public T Value {
			readonly get {
				System.Diagnostics.Debug.Assert(hasValue, "Optional has no value.");
				return value;
			}
			set {
				if (this.value is UnityEngine.Object prevUnityObject) {
					DestroyListener listener = prevUnityObject
						.GetComponent<DestroyListener>();
					if (listener != null) {
						listener.OnDestroyed -= Clear;
					}
				}

				this.value = value;
				hasValue = value != null;

				if (
					value is UnityEngine.Object unityObject
					&& unityObject != null
				) {
					DestroyListener listener = unityObject
						.GetOrAddComponent<DestroyListener>();
					listener.OnDestroyed += Clear;
				}
			}
		}

		private bool hasValue;
		public readonly bool HasValue {
			get {
				if (!hasValue) return false;

				if (value is UnityEngine.Object unityObject) {
					return unityObject != null;
				}

				return true;
			}
		}

		public Optional(T value) {
			this.value = value;
			hasValue = value != null;
			// Use the setter to validate the value.
			Value = value;
		}

		public static Optional<T> None => new();

		public readonly T ValueOrDefault() {
			return hasValue ? value : default;
		}

		public readonly T ValueOrDefault(T defaultValue) {
			return hasValue ? value : defaultValue;
		}

		public void Clear() {
			Value = default;
			hasValue = false;
		}

		public readonly Optional<T> Where(Func<T, bool> predicate)
			 => HasValue && predicate(Value) ? this : None;

		public readonly Optional<TResult> Then<TResult>(Func<T, TResult> func)
			=> HasValue ? func(Value) : Optional<TResult>.None;

		public readonly Optional<TResult> Then<TResult>(Func<T, Optional<TResult>> func)
			=> HasValue ? func(Value) : Optional<TResult>.None;

		public readonly Optional<T> Then(Action<T> func) {
			if (!HasValue) return None;
			func(Value);
			return this;
		}

		public readonly void OnBeforeSerialize() { }

		public void OnAfterDeserialize() {
			hasValue = value != null;
		}

		public readonly override string ToString() {
			return $"Optional<{typeof(T).Name}>"
				+ (hasValue ? $" {{{value}}}" : ".None");
		}

		public override readonly bool Equals(object obj) {
			return obj is Optional<T> other && this == other;
		}

		public override readonly int GetHashCode() {
			return HasValue ? EqualityComparer<T>.Default.GetHashCode(Value) : 0;
		}

		public static explicit operator T(Optional<T> optional) {
			return optional.Value;
		}

		public static implicit operator Optional<T>(T value) {
			return new Optional<T>(value);
		}

		public static implicit operator bool(Optional<T> optional) {
			return optional.HasValue;
		}

		public static bool operator true(Optional<T> optional) {
			return optional.HasValue;
		}

		public static bool operator false(Optional<T> optional) {
			return !optional.HasValue;
		}

		public static bool operator ==(Optional<T> a, Optional<T> b) {
			return a.HasValue && b.HasValue
				&& EqualityComparer<T>.Default.Equals(a.Value, b.Value);
		}

		public static bool operator !=(Optional<T> a, Optional<T> b) {
			return !(a == b);
		}

		public static bool operator ==(Optional<T> a, T b) {
			return a.HasValue && EqualityComparer<T>.Default.Equals(a.Value, b);
		}

		public static bool operator !=(Optional<T> a, T b) {
			return !(a == b);
		}

		public static Optional<T> operator |(Optional<T> a, Optional<T> b) => a ? a : b;

		public static Optional<T> operator |(Optional<T> a, T b) => a ? a : b;

		public static Optional<T> operator &(Optional<T> a, Optional<T> b) => a ? b : a;

		public static Optional<T> operator &(Optional<T> a, T b) => a ? b : a;
	}
}