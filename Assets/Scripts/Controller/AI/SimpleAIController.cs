using AI;
using AI.StateMachine;

/**
 * <summary>
 * A simple AI controller
 * </summary>
 */
public class SimpleAIController : AIController {

	public StateMachine stateMachine;
	public Blackboard blackboard = new(Blackboard.GLOBAL);

	public SimpleAIController() {

		IdleState idle = new(blackboard);
		AttackState attack = new(blackboard);

		idle.Transitions.Add(
			new StateTransition(attack, new CanSeePlayer(45f, 10f))
		);

		attack.Transitions.Add(
			new StateTransition(idle, new CanSeePlayer(45f, 15f).Inverse())
		);

		stateMachine = new(idle, this);
	}

	protected override void HandleInput() {
		stateMachine.Tick(this);
	}
}