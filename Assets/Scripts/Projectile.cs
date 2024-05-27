using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private bool shouldBeginMovement = false;
    private GameObject target;

    public float moveSpeed = 5f; // The speed at which to move towards the target

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
        }
    }

    public void Init(GameObject target)
    {
        this.target = target;
        shouldBeginMovement = true;
    }

}
