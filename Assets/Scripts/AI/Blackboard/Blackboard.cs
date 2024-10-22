using System;
using System.Collections.Generic;

using Util;

namespace AI {
	// TODO: blackboard factory?
	/**
	 * <summary>
	 * A blackboard for AI
	 * </summary>
	 */
	public class Blackboard {

		public static Blackboard GLOBAL { get; } = new();

		private Optional<Blackboard> parent = Optional<Blackboard>.None;
		private readonly Dictionary<string, object> data = new();

		public Action<string, object> OnValueSet;
		public Action<string, object> OnValueChanged;

		public Blackboard() { }

		public Blackboard(Optional<Blackboard> parent) {
			this.parent = parent;
		}

		/**
		 * <summary>
		 * Add or set a value
		 * </summary>
		 */
		public void Set<T>(string key, T value) {
			bool hasKey = HasKey(key);
			bool differentValue = !hasKey || (
				data[key] is T typedValue
				&& !EqualityComparer<T>.Default.Equals(typedValue, value)
			);

			data[key] = value;

			if (hasKey) {
				OnValueSet?.Invoke(key, value);
			}
			if (differentValue) {
				OnValueChanged?.Invoke(key, value);
			}
		}

		/**
		 * <summary>
		 * Add or set a value
		 * </summary>
		 */
		public void Set<T>(BlackboardKey<T> key, T value) {
			Set(key.name, value);
		}

		/**
		 * <summary>
		 * Get a value, or the default value if it doesn't exist
		 * </summary>
		 */
		public T Get<T>(string key) {
			if (HasKey(key)) {
				if (data.TryGetValue(key, out object value) && value is T typedValue) {
					return typedValue;
				}
			} else if (parent) {
				return parent.Value.Get<T>(key);
			}

			return default;
		}

		/**
		 * <summary>
		 * Get the value, or the default value if it doesn't exist
		 * </summary>
		 */
		public T Get<T>(BlackboardKey<T> key) {
			return Get<T>(key.name);
		}

		/**
		 * <summary>
		 * Check if the blackboard has a key
		 * </summary>
		 */
		public bool HasKey(string key) {
			return data.ContainsKey(key);
		}

		/**
		 * <summary>
		 * Check if the blackboard has a key.<br/>
		 * Does not type check
		 * </summary>
		 */
		public bool HasKey<T>(BlackboardKey<T> key) {
			return HasKey(key.name);
		}
	}
}