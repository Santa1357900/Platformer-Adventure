using UnityEngine;

public class Attack : MonoBehaviour
{
    public int attackDamage = 10;
    public Vector2 knockback = Vector2.zero;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        // See if it can be hit
        damageable damageable = collision.GetComponent<damageable>();

        if (damageable != null)
        {
            Vector2 deliverKnockback = knockback;

            // Calculate the direction from the attacker to the target
            Vector2 attackDirection = (collision.transform.position - transform.position).normalized;

            // Apply knockback in the direction of the attack
            deliverKnockback.x *= attackDirection.x; // Adjust X component based on direction
            deliverKnockback.y *= attackDirection.y; // Adjust Y component based on direction

            // Hits target
            bool gotHit = damageable.Hit(attackDamage, deliverKnockback);

            if (gotHit)
                Debug.Log(collision.name + " hits for " + attackDamage);
        }
    }
}
