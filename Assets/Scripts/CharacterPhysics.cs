using UnityEngine;
using System.Collections;

public class CharacterPhysics : MonoBehaviour {

	private BoxCollider collider;
	private Vector3 colliderSize;
	private Vector3 colliderCenter;

	void start() {
		collider = GetComponent<BoxCollider> ();
		colliderSize = collider.size;
		colliderCenter = collider.center;
	}
	public void Move(Vector2 moveAmt) {

		float dY = moveAmt.y;
		transform.Translate (moveAmt);

		for (int i = 0; i < 3; i++) {

		}
	}

}
