using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using UnityEditor;

using UnityEngine;

using Util;

[CustomPropertyDrawer(typeof(TemplatedTypeAttribute))]
public class TemplatedTypeDrawer : TypeDrawer {

	protected Template templateReference;

	private void DrawTemplateFields(Template template, Rect position) {
		position.y += EditorGUIUtility.singleLineHeight;

		IEnumerable<string> parameterNames = template.GetParameterNames();

		for (int i = 0; i < parameterNames.Count(); i++) {
			string parameterName = parameterNames.ElementAt(i);
			Rect fieldRect = new(
				position.x,
				position.y,
				position.width,
				EditorGUIUtility.singleLineHeight
			);

			object value = template.GetParameter(parameterName);
			object newValue;

			if (value is int intValue) {
				newValue = EditorGUI.IntField(fieldRect, parameterName, intValue);
			} else if (value is float floatValue) {
				newValue = EditorGUI.FloatField(fieldRect, parameterName, floatValue);
			} else if (value is bool boolValue) {
				newValue = EditorGUI.Toggle(fieldRect, parameterName, boolValue);
			} else if (value is string stringValue) {
				newValue = EditorGUI.TextField(fieldRect, parameterName, stringValue);
			} else {
				EditorGUI.LabelField(fieldRect, parameterName, "Unsupported type.");
				newValue = value;
			}

			template.SetParameter(parameterName, newValue);
			position.y += EditorGUIUtility.singleLineHeight;
		}
	}

	protected override Type[] GetTypes(Type type) {
		return base.GetTypes(type).Where(t => {
			return t.GetConstructors()
				.Any(ctor => ctor.GetCustomAttribute<TemplateAttribute>() != null);
		}).ToArray();
	}

	public override void OnGUI(
		Rect position,
		SerializedProperty property,
		GUIContent label
	) {
		if (property.propertyType != SerializedPropertyType.String) {
			EditorGUI.LabelField(
				position,
				label.text,
				"Use [TemplatedType] with string."
			);
			return;
		}

		base.OnGUI(position, property, label);

		if (selectedType != null) {
			TemplatedTypeAttribute thisAttribute = (TemplatedTypeAttribute)attribute;
			SerializedProperty templateProperty = property.serializedObject
				.FindProperty(thisAttribute.templateField);

			if (templateProperty == null) {
				EditorGUI.LabelField(
					position,
					label.text,
					$"Error: Field '{thisAttribute.templateField}' not found."
				);
				return;
			}

			UnityEngine.Object targetObject = property.serializedObject.targetObject;
			FieldInfo templateFieldInfo = targetObject.GetType().GetField(
				thisAttribute.templateField,
				BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
			);

			if (templateFieldInfo.GetValue(targetObject) is not Template template) {
				template = new Template();
				templateFieldInfo.SetValue(targetObject, template);
			}
			template.Mold(selectedType);

			EditorGUI.indentLevel++;
			DrawTemplateFields(template, position);
			EditorGUI.indentLevel--;

			templateReference = template;
		}
	}

	public override float GetPropertyHeight(
		SerializedProperty property,
		GUIContent label
	) {
		float height = EditorGUIUtility.singleLineHeight;

		if (templateReference != null) {
			height += EditorGUIUtility.singleLineHeight
				* templateReference.GetParameterNames().Count();
		}

		return height;
	}
}