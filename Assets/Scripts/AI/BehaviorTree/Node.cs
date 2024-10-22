namespace AI.BehaviorTree {
	/**
	 * <summary>
	 * Base class for behavior tree nodes
	 * </summary>
	 */
	public abstract class Node {

		public virtual Blackboard Blackboard { protected get; set; }

		/**
		 * <summary>
		 * Called when the behavior tree is first started.<br/>
		 * Initialise the node
		 * </summary>
		 */
		public virtual void EnterTree(AIController controller) { }

		/**
		 * <summary>
		 * Start the node's processing
		 * </summary>
		 */
		public virtual void Start(AIController controller) { }

		/**
		 * <summary>
		 * Tick the node. Behavior may optionally continue after the node stops.
		 * </summary>
		 */
		public abstract NodeState Tick(AIController controller);

		/**
		 * <summary>
		 * Stop the node's processing.
		 * </summary>
		 */
		public virtual void Stop(AIController controller) { }

		/**
		 * <summary>
		 * Tear down the node. Called when the behavior tree is stopped.
		 * </summary>
		 */
		public virtual void ExitTree(AIController controller) { }
	}
}