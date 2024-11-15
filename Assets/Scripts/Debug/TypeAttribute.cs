using System;

using UnityEngine;

[AttributeUsage(AttributeTargets.Field, Inherited = true)]
public class TypeAttribute : PropertyAttribute {

	public Type type;
	public bool allowNull;

	public TypeAttribute(Type type, bool allowNull = true) {
		this.type = type;
		this.allowNull = allowNull;
	}
}