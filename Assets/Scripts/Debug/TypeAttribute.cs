using System;

using UnityEngine;

public class TypeAttribute : PropertyAttribute {

	public Type type;
	public bool allowNull;

	public TypeAttribute(Type type, bool allowNull = true) {
		this.type = type;
		this.allowNull = allowNull;
	}
}