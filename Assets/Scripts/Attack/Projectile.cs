using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private bool shouldBeginMovement = false;
    private GameObject target;
    private IAttack attack;

    public float moveSpeed = 5f; // The speed at which to move towards the target

    private float lifeTimer = 0f;

    private void Update()
    {
        if (shouldBeginMovement == true && target != null)
        {
            // Get the direction vector from current position to the target
            Vector2 direction = (target.transform.position - transform.position).normalized;

            // Calculate the movement amount based on moveSpeed and deltaTime
            float movementAmount = moveSpeed * Time.deltaTime;

            // Move the object towards the target
            transform.Translate(direction * movementAmount, Space.World);

            //Rotate the projectile to target
            // Get the direction vector from this object to the targe
            Vector3 dir = target.transform.position - transform.position;
            // Calculate the angle from the direction vector (in radians)
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            // Rotate the object to face the target
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            if (CalculateDistance(transform.position.x, transform.position.y, target.transform.position.x, target.transform.position.y) < .2f)
            {
                CollidedWithTarget();
            }

        }

        lifeTimer += Time.deltaTime;
        if (lifeTimer > 4f || target == null)
        {
            Destroy(gameObject);
        }
    }

    public void Init(GameObject target,IAttack attack)
    {
        this.target = target;
        shouldBeginMovement = true;
        this.attack = attack;
    }

    private void CollidedWithTarget()
    {
        EntityStatus targetStatus = target.GetComponent<EntityStatus>();
        if (targetStatus != null)
        {
            attack.AffectTarget(targetStatus);
        }
        Destroy(gameObject);
    }

    private float CalculateDistance(float x1, float y1, float x2, float y2)
    {
        // Calculate the squared differences
        float dx = x2 - x1;
        float dy = y2 - y1;
        float squaredDistance = dx * dx + dy * dy;

        // Return the square root of the squared distance
        return Mathf.Sqrt(squaredDistance);
    }

}
