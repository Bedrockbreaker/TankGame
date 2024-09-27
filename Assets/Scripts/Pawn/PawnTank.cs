/**
 * <summary>
 * A tank pawn
 * </summary>
 */
public class PawnTank : Pawn {

	public override void Move(float distance) {
		Movement.Move(transform.forward, distance * linearSpeed);
	}

	public override void Rotate(float radians) {
		Movement.Rotate(radians * angularSpeed);
	}
}