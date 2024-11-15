using System;
using System.Linq;
using System.Reflection;

using UnityEditor;

using UnityEngine;

[CustomPropertyDrawer(typeof(TypeAttribute))]
public class TypeDrawer : PropertyDrawer {

	protected Type[] assignableTypes;
	protected Type selectedType;
	protected int selectedTypeIndex = -1;

	protected virtual Type[] GetTypes(Type type) {
		return Assembly
			.GetAssembly(type)
			.GetTypes()
			.Where(t => type.IsAssignableFrom(t) && !t.IsAbstract)
			.ToArray();
	}

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

		assignableTypes ??= GetTypes(type);

		int allowNull = typeAttribute.allowNull ? 1 : 0;

		string[] typeNames = new string[assignableTypes.Length + allowNull];
		typeNames[0] = "None";
		for (int i = 0; i < assignableTypes.Length; i++) {
			typeNames[i + allowNull] = assignableTypes[i].FullName;
		}

		int currentIndex = string.IsNullOrEmpty(property.stringValue)
			? 0
			: Array.IndexOf(typeNames, property.stringValue);

		int selectedIndex = EditorGUI.Popup(
			new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight),
			label.text,
			currentIndex,
			typeNames
		);

		if (selectedIndex == 0 && allowNull == 1) {
			property.stringValue = string.Empty;
			selectedType = null;
			selectedTypeIndex = -1;
		} else if (selectedIndex >= 0 && selectedIndex < typeNames.Length) {
			selectedType = assignableTypes[selectedIndex];
			property.stringValue = typeNames[selectedIndex];
			selectedTypeIndex = selectedIndex;
		}
	}
}