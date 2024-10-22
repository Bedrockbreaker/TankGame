namespace AI {
	/**
	 * <summary>
	 * A key in a blackboard with an enforced type
	 * </summary>
	 */
	public struct BlackboardKey<T> {

		public string name;

		public static implicit operator BlackboardKey<T>(string name) {
			return new BlackboardKey<T> { name = name };
		}
	}
}