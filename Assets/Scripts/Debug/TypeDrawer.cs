using System;
using System.Linq;
using System.Reflection;

using UnityEditor;

using UnityEngine;

[CustomPropertyDrawer(typeof(TypeAttribute))]
public class TypeDrawer : PropertyDrawer {

	public override void OnGUI(
		Rect position,
		SerializedProperty property,
		GUIContent label
	) {
		if (property.propertyType != SerializedPropertyType.String) {
			EditorGUI.LabelField(position, label.text, "Use [Type] with string.");
			return;
		}

		TypeAttribute typeAttribute = (TypeAttribute)attribute;
		Type type = typeAttribute.type;

		Type[] types = Assembly
			.GetAssembly(type)
			.GetTypes()
			.Where(t => type.IsAssignableFrom(t) && !t.IsAbstract)
			.ToArray();

		int allowNull = typeAttribute.allowNull ? 1 : 0;

		string[] typeNames = new string[types.Length + allowNull];
		typeNames[0] = "None";
		for (int i = 0; i < types.Length; i++) {
			typeNames[i + allowNull] = types[i].FullName;
		}

		int currentIndex = string.IsNullOrEmpty(property.stringValue)
			? 0
			: Array.IndexOf(typeNames, property.stringValue);
		int selectedIndex = EditorGUI.Popup(position, label.text, currentIndex, typeNames);

		if (selectedIndex == 0 && allowNull == 1) {
			property.stringValue = string.Empty;
		} else if (selectedIndex >= 0 && selectedIndex < typeNames.Length) {
			property.stringValue = typeNames[selectedIndex];
		}
	}
}