using UnityEngine;

public class Projectile : MonoBehaviour
{
    private string colorHex;
    public float lifeTime = 5f; // Time in seconds before the projectile is destroyed

    void Start()
    {
        // Destroy the projectile after 'lifeTime' seconds
        Destroy(gameObject, lifeTime);
    }

    public void SetColor(Color newColor)
    {
        colorHex = ColorUtility.ToHtmlStringRGB(newColor);
        // Assuming there's a Renderer to set the color visually
        GetComponent<Renderer>().material.color = newColor;
    }

    public string GetColorHex()
    {
        return colorHex;
    }

    private void OnCollisionEnter(Collision collision)
    {
        TargetScript target = collision.gameObject.GetComponent<TargetScript>();
        if (target != null)
        {
            target.HitByProjectile(GetColorHex());
        }
        Destroy(gameObject); // Destroy the projectile after it hits the target
    }
}
