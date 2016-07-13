using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterPhysics))]

public class CharacterControls : MonoBehaviour {

	//Character Handling
	public float speed = 15;
	public float acceleration = 1000;
	public float gravity = -20;

	private float currentSpeed;
	private float targetSpeedHorizontal;
	private Vector2 amountToMove;

	private CharacterPhysics phys;

	// Use this for initialization
	void Start () {
		phys = GetComponent<CharacterPhysics> ();
	}
	
	// Update is called once per frame
	void Update () {
	
		targetSpeedHorizontal = Input.GetAxisRaw ("Horizontal") * speed;
		currentSpeed = IncrementSpeed (currentSpeed, targetSpeedHorizontal, acceleration);

		amountToMove.x = currentSpeed;
		amountToMove.y += gravity * Time.deltaTime;
		phys.Move (amountToMove * Time.deltaTime);
	}

	private float IncrementSpeed(float currentVel, float targetVel, float acc) {

		if (currentVel == targetVel)
			return currentVel;
		
		else {
			//determine direction of acceleration
			float currDirection = Mathf.Sign (currentVel);
			float targetDirection = Mathf.Sign (targetVel);

			//if the acceleration is in the same direction, give
			//a delay
			if  ((currentVel == 0))
				currentVel += (acc * Time.deltaTime * targetDirection);
			else if ((targetDirection == 0) || (targetDirection != currDirection))
				currentVel = 0;
			else 				
				currentVel += (acc * Time.deltaTime * targetDirection);

			//currentVel = targetVel;
			//check if the current is now greater target
			if (Mathf.Abs(currentVel) < Mathf.Abs(targetVel))
				return currentVel;
			else
				return targetVel;		
		}
	}
}
