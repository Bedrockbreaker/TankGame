using AI;
using AI.StateMachine;

using Util;

/**
 * <summary>
 * A curious AI controller
 * </summary>
 */
public class CuriousAIController : AIController {

	public StateMachine stateMachine;
	public Blackboard blackboard = new(Blackboard.GLOBAL);

	public CuriousAIController() {

		IdleState idle = new(blackboard);
		InvestigateState investigate = new(blackboard);

		idle.Transitions.Add(
			new StateTransition(investigate, new CanHear(Optional<Controller>.None, 5f))
		);

		// TODO: sense memory
		investigate.Transitions.Add(
			new StateTransition(idle, new IsPlayerNear(10f).Inverse())
		);

		stateMachine = new(idle, this);
	}

	protected override void HandleInput() {
		stateMachine.Tick(this);
	}
}