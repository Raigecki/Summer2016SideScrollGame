using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterPhysics))]

public class CharacterControls : MonoBehaviour {

	private Animator animator;

	//Character Handlling

	float jumpHeight = 8;
	float timeToJumpApex = .4f;

	public float speed = 8;
	public float acceleration = 1000;
	public float gravity;
	public float jumpVelocity;

    Vector3 velocity;

	//private float currentSpeed;
	//private float targetSpeedHorizontal;
	//private Vector2 amountToMove;

	private CharacterPhysics phys;

	// Use this for initialization
	void Start () {
		phys = GetComponent<CharacterPhysics> ();
		gravity = -2 * jumpHeight / Mathf.Pow (timeToJumpApex, 2);
		jumpVelocity = -gravity * timeToJumpApex;

		//animator
		animator = GetComponent<Animator>();

	}
	
	// Update is called once per frame
	void Update () {
	
        if (phys.collisions.up || phys.collisions.down)
        {
            velocity.y = 0;
        }

		//detecting left and right
        Vector2 inputMove = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

		//detecting jump
		if (Input.GetKeyDown (KeyCode.Space) && phys.collisions.down) {
			velocity.y = jumpVelocity;
		}

        velocity.x = inputMove.x * speed;
        velocity.y += gravity * Time.deltaTime;
        phys.Move(velocity * Time.deltaTime);
	
		//animations
		onRun();

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

	protected void onRun() {

		if (velocity.x > 0) {
			animator.SetTrigger ("char_run");
		} else if (velocity.x == 0) {
			animator.SetTrigger ("char_idle");
		}
        else if (velocity.x < 0)
        {
            animator.SetTrigger("char_run_left");
        }
	}
}
