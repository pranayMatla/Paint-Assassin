using UnityEngine;

public class TargetScript : MonoBehaviour
{
    public int hitsToDestroy = 3;
    private int hitCount = 0;
    private string targetColorHex;

    public float speed = 2f;
    public float attackRange = 2f;

    private Vector3 direction;
    private Animator targetAnimator;
      private bool isAttacking = false;

    void Start()
    {
        targetColorHex = ColorUtility.ToHtmlStringRGB(GetComponent<Renderer>().material.color);

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            direction = (player.transform.position - transform.position).normalized;
        }

        targetAnimator = GetComponent<Animator>();
        if (targetAnimator == null)
        {
            Debug.LogError("Target Animator component not found on the target.");
        }

        
    }

    void Update()
    {
        if (!isAttacking)
        {
            MoveTowardsPlayer();
        }
    }

    void MoveTowardsPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        if (distanceToPlayer > attackRange)
        {
            transform.position += direction * speed * Time.deltaTime;
            targetAnimator.SetBool("isAttacking", false);
                   }
        else
        {
            // Stop moving and start attacking
            isAttacking = true;
            targetAnimator.SetBool("isAttacking", true);
            
        }
    }

    public void HitByProjectile(string projectileColorHex)
    {
        if (projectileColorHex == targetColorHex)
        {
            hitCount++;
            if (hitCount >= hitsToDestroy)
            {
                DestroyTarget();
            }
        }
    }

    private void DestroyTarget()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Start the attack animation and stop moving
            isAttacking = true;
            targetAnimator.SetBool("isAttacking", true);
           

            // Optionally, deal damage to the player
            PlayerScript playerScript = collision.gameObject.GetComponent<PlayerScript>();
            if (playerScript != null)
            {
                playerScript.TakeDamage(1);
            }
        }
    }
}
