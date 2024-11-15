using System;

namespace Util {
	/**
	* <summary>
	* An attribute for marking a constructor as serializable for use with TypeAttribute
	* </summary>
	*/
	[AttributeUsage(AttributeTargets.Constructor)]
	public class TemplateAttribute : Attribute { }
}