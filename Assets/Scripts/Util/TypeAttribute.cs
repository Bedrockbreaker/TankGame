using System;

using UnityEngine;

/**
 * <summary>
 * An attribute for marking a serializable field as a type
 * </summary>
 */
public class TypeAttribute : PropertyAttribute {

	public Type type;
	public bool allowNull;

	public TypeAttribute(Type type, bool allowNull = true) {
		this.type = type;
		this.allowNull = allowNull;
	}
}