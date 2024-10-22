using AI;
using AI.StateMachine;

/**
 * <summary>
 * A shy AI controller
 * </summary>
 */
public class ShyAIController : AIController {

	public StateMachine stateMachine;
	public Blackboard blackboard = new(Blackboard.GLOBAL);

	public ShyAIController() {

		IdleState idle = new(blackboard);
		FleeState flee = new(blackboard);

		idle.Transitions.Add(
			new StateTransition(flee, new IsPlayerNear(5f))
		);

		flee.Transitions.Add(
			new StateTransition(idle, new IsPlayerNear(10f).Inverse())
		);

		stateMachine = new(idle, this);
	}

	protected override void HandleInput() {
		stateMachine.Tick(this);
	}
}