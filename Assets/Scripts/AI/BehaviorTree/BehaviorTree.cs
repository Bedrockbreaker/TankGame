namespace AI.BehaviorTree {
	// TODO: behavior tree factory?
	/**
	 * <summary>
	 * A tree of behavior nodes
	 * </summary>
	 */
	public class BehaviorTree {

		protected Blackboard blackboard;
		protected Node root;

		public BehaviorTree(Blackboard blackboard, Node root) {
			this.blackboard = blackboard;
			this.root = root;
			this.root.Blackboard = blackboard;
		}

		/**
		 * <summary>
		 * Initialise the behavior tree
		 * </summary>
		 */
		public void EnterTree(AIController controller) {
			root.EnterTree(controller);
		}

		/**
		 * <summary>
		 * Start the behavior tree
		 * </summary>
		 */
		public void Start(AIController controller) {
			root.Start(controller);
		}

		/**
		 * <summary>
		 * Tick the behavior tree
		 * </summary>
		 */
		public void Tick(AIController controller) {
			root.Tick(controller);
		}

		/**
		 * <summary>
		 * Stop the behavior tree
		 * </summary>
		 */
		public void Stop(AIController controller) {
			root.Stop(controller);
		}

		/**
		 * <summary>
		 * Tear down the behavior tree
		 * </summary>
		 */
		public void ExitTree(AIController controller) {
			root.ExitTree(controller);
		}
	}
}