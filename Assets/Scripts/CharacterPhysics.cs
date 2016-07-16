using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public class CharacterPhysics : MonoBehaviour {

    public LayerMask collideMask;

    BoxCollider2D collider;
    private RaysCast rayCast;
    public Collisions collisions;

    public int horizRays = 3;
    public int vertiRays = 3;

    private float horizRaySpacing;
    private float vertiRaySpacing;

    private float space = .005f;

    public bool onGround;


	void Start() {
		collider = GetComponent<BoxCollider2D>();
        CalculateSpacing();
	}

    void UpdateRays()
    {
        Bounds bounds = collider.bounds;
        bounds.Expand(space * -2);

        rayCast.bLeft = new Vector2(bounds.min.x, bounds.min.y);
        rayCast.bRight = new Vector2(bounds.max.x, bounds.min.y);
        rayCast.tLeft = new Vector2(bounds.min.x, bounds.max.y);
        rayCast.tRight = new Vector2(bounds.max.x, bounds.max.y);

    }

    void CalculateSpacing()
    {
        Bounds bounds = collider.bounds;
        bounds.Expand(space * -2);

        horizRays = Mathf.Clamp(horizRays, 2, int.MaxValue);
        vertiRays = Mathf.Clamp(vertiRays, 2, int.MaxValue);

        horizRaySpacing = bounds.size.y / (horizRays - 1);
        vertiRaySpacing = bounds.size.x / (vertiRays - 1);
    }

    struct RaysCast
    {
        public Vector2 tLeft, tRight;
        public Vector2 bLeft, bRight; 
    }

    public struct Collisions
    {
        public bool up, down;
        public bool left, right;

        public void Reset()
        {
            up = false;
            down = false;
            left = false;
            right = false;
        }
    }

    public void CheckHCollisions(ref Vector3 velocity)
    {
        float directionX= Mathf.Sign(velocity.x);
        float rayLength = Mathf.Abs(velocity.x) + space;

        for (int i = 0; i < horizRays; i++)
        {
            Vector2 raysOrigin;
            if (directionX == -1)
            {
                raysOrigin = rayCast.bLeft;
            }
            else
            {
                raysOrigin = rayCast.bRight;
            }

            raysOrigin += Vector2.up * (horizRaySpacing * i);

            RaycastHit2D hit = Physics2D.Raycast(raysOrigin, Vector2.right * directionX, rayLength, collideMask);
            if (hit)
            {
                velocity.x = (hit.distance - space) * directionX;
                rayLength = hit.distance;

                collisions.left = (directionX == -1);
                collisions.right = (directionX == 1);
            }
        }
    }

    public void CheckVCollisions(ref Vector3 velocity)
    {
        float directionY = Mathf.Sign(velocity.y);
        float rayLength = Mathf.Abs(velocity.y) + space;

        for (int i = 0; i < vertiRays; i++)
        {
            Vector2 raysOrigin;
            if (directionY == -1)
            {
                raysOrigin = rayCast.bLeft;
            }
            else
            {
                raysOrigin = rayCast.tLeft;
            }

            raysOrigin += Vector2.right * (vertiRaySpacing * i + velocity.x);

            RaycastHit2D hit = Physics2D.Raycast(raysOrigin, Vector2.up * directionY, rayLength, collideMask);
            if (hit)
            {
                velocity.y = (hit.distance - space) * directionY;
                rayLength = hit.distance;

                collisions.up = (directionY == 1);
                collisions.down = (directionY == -1);
            }
        }
    }
	public void Move(Vector3 velocity) {

        UpdateRays();
        collisions.Reset();

        if (velocity.x != 0)
        {
            CheckHCollisions(ref velocity);
        }

        if (velocity.y != 0)
        {
            CheckVCollisions(ref velocity);
        }

        transform.Translate(velocity);

        /*
		float dY = moveAmt.y;
        float dX = moveAmt.x;
        Vector2 pos = transform.position;

        onGround = false;
        for (int i = 0; i < 3; i++)
        {
            float direction = Mathf.Sign(dY);

            //go from left to center then to right
            float x = (pos.x + colliderCenter.x - (colliderSize.x / 2)) + (colliderSize.x / 2) * i;

            //bottom of the collider
            float y = pos.y + colliderCenter.y + (colliderSize.y / 2) * direction;

            ray = new Ray2D(new Vector2(x, y), new Vector2(0, direction));

            Debug.DrawRay(ray.origin, ray.direction);

            hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Abs(dY), collideMask);

            //check if character collided with ground
            if (hit != null && hit.collider != null)
            {
                //distance between character and ground
                float dist = Vector3.Distance(ray.origin, hit.point);

                //stop character from moving down after getting within space of collision
                if (dist > space)
                {
                    dY = dist * direction + space;
                }
                else
                {
                    dY = 0;
                }

                onGround = true;
                break;
            }
		}
        Vector2 targetPos = new Vector2(dX, dY);
        */

    }

}
