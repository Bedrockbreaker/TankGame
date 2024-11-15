using System;

public class TemplatedTypeAttribute : TypeAttribute {

	public string templateField;

	public TemplatedTypeAttribute(
		Type type,
		string templateField,
		bool allowNull = false
	) : base(type, allowNull) {
		this.templateField = templateField;
	}
}