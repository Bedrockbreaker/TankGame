using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using UnityEngine;

using Util;

[Serializable]
public class Template : ISerializationCallbackReceiver {
	private readonly Dictionary<string, object> parameters = new();

	[SerializeField]
	private string stringType;
	[SerializeField]
	private List<string> keys = new();
	[SerializeField]
	private List<string> values = new();
	[SerializeField]
	private List<string> types = new();

	private object GetDefaultValue(Type type) {
		if (type.IsValueType) return Activator.CreateInstance(type);
		return null;
	}

	public void Mold(Type type) {
		stringType = type.FullName;

		parameters.Clear();

		ConstructorInfo constructor = type.GetConstructors()
			.FirstOrDefault(ctor => {
				return ctor.GetCustomAttribute<TemplateAttribute>() != null;
			});

		foreach (ParameterInfo parameter in constructor.GetParameters()) {
			parameters[parameter.Name] = GetDefaultValue(parameter.ParameterType);
		}
	}

	public object CreateInstance() {
		Type type = Type.GetType(stringType);
		ConstructorInfo constructor = type.GetConstructors()
			.FirstOrDefault(ctor => {
				return ctor.GetCustomAttribute<TemplateAttribute>() != null;
			});

		object[] args = new object[constructor.GetParameters().Length];

		for (int i = 0; i < args.Length; i++) {
			string parameterName = constructor.GetParameters()[i].Name;
			args[i] = parameters[parameterName];
		}

		return constructor.Invoke(args);
	}

	public object GetParameter(string name) => parameters.ContainsKey(name)
		? parameters[name]
		: null;

	public void SetParameter(string name, object value) {
		if (parameters.ContainsKey(name)) {
			parameters[name] = value;
		}
	}

	public IEnumerable<string> GetParameterNames() => parameters.Keys;

	public void OnBeforeSerialize() {
		keys.Clear();
		values.Clear();
		types.Clear();

		foreach (KeyValuePair<string, object> parameter in parameters) {
			keys.Add(parameter.Key);
			values.Add(parameter.Value.ToString());
			types.Add(parameter.Value.GetType().AssemblyQualifiedName);
		}
	}

	public void OnAfterDeserialize() {
		parameters.Clear();

		for (int i = 0; i < keys.Count; i++) {
			Type type = Type.GetType(types[i]);
			if (type.IsValueType) {
				parameters[keys[i]] = GetDefaultValue(type);
			} else {
				parameters[keys[i]] = Convert.ChangeType(values[i], type);
			}
		}
	}
}