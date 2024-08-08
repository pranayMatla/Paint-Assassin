using UnityEngine;

public class PlayerScript : MonoBehaviour

{
    public int health = 3; // Player health

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // Add death logic here (e.g., restart level, show game over screen)
        Debug.Log("Player has died.");
        Destroy(gameObject);
    }
}
