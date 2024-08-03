using UnityEngine;

public class TargetScript : MonoBehaviour
{
    public int hitsToDestroy = 3; // Number of hits to destroy
    private int hitCount = 0; // Count of hits from projectiles
    private string targetColorHex; // Hexadecimal color value of the target

    void Start()
    {
        // Assuming the target's color is set via its Renderer component
        targetColorHex = ColorUtility.ToHtmlStringRGB(GetComponent<Renderer>().material.color);
    }

    public void HitByProjectile(string projectileColorHex)
    {
        Debug.Log("Hit by color: " + projectileColorHex); // Debug log to check color

        if (projectileColorHex == targetColorHex)
        {
            hitCount++;
            Debug.Log("Hits: " + hitCount);

            if (hitCount >= hitsToDestroy)
            {
                DestroyTarget();
            }
        }
    }

    private void DestroyTarget()
    {
        // Add any additional destruction effects here (e.g., animations, sounds)
        Debug.Log("Target destroyed.");
        Destroy(gameObject);
    }
}
