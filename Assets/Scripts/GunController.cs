using UnityEngine;
using System.Collections;

public class GunController : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform shootPoint; // This is where projectiles will spawn
    public float projectileSpeed = 20f;
    public float rotationSpeed = 5f; // Reduced rotation speed for smoother and smaller movement
    public float returnDelay = 1f;  // Delay before returning to default position
    public float maxRotationAngleX = 5f; // Reduced maximum rotation angle on X-axis (up and down)
    public float maxRotationAngleY = 15f; // Reduced maximum rotation angle on Y-axis (left and right)
    public float aimThreshold = 1f; // Threshold for aiming accuracy
    private Quaternion defaultRotation;
    private Coroutine returnRoutine;
    private Coroutine shootRoutine;
    private Quaternion targetRotation;
    private bool hasShotThisTap = false;
    private Color selectedColor = Color.clear;
    private UIManager uiManager;

    void Start()
    {
        if (shootPoint == null)
        {
            Debug.LogError("ShootPoint is not assigned.");
            return;
        }

        defaultRotation = transform.localRotation;
        targetRotation = defaultRotation;

        uiManager = FindObjectOfType<UIManager>();

        if (uiManager == null)
        {
            Debug.LogError("UIManager not found. Make sure there is a UIManager script in the scene.");
        }
    }

    void Update()
    {
        HandleTouchInput();
        SmoothRotateGun();
    }

    void HandleTouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                hasShotThisTap = false; // Reset shot flag at the beginning of the tap
            }

            if (uiManager.IsTouchOverUIButton(touch.position))
            {
                // Do nothing if the touch is over a button
                return;
            }

            if ((touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved) && !hasShotThisTap && selectedColor != Color.clear)
            {
                RotateGunTowards(touch.position);
                if (shootRoutine != null)
                {
                    StopCoroutine(shootRoutine);
                }
                shootRoutine = StartCoroutine(ShootWhenAimed());
                hasShotThisTap = true; // Mark as shot for this tap
                // Stop any ongoing return routine if the player is currently tapping
                if (returnRoutine != null)
                {
                    StopCoroutine(returnRoutine);
                    returnRoutine = null;
                }
            }
        }
        else
        {
            // Start the coroutine to return to default rotation after a delay
            if (returnRoutine == null)
            {
                returnRoutine = StartCoroutine(ReturnToDefaultRotationAfterDelay());
            }
        }
    }

    void RotateGunTowards(Vector2 touchPosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(touchPosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 direction = (hit.point - shootPoint.position).normalized;

            // Calculate rotation towards the tapped position
            Quaternion lookRotation = Quaternion.LookRotation(direction, Vector3.up);
            lookRotation = ClampRotation(lookRotation);
            targetRotation = lookRotation;
        }
    }

    void SmoothRotateGun()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }

    Quaternion ClampRotation(Quaternion rotation)
    {
        Vector3 euler = rotation.eulerAngles;
        if (euler.x > 180) euler.x -= 360;
        if (euler.y > 180) euler.y -= 360;
        euler.x = Mathf.Clamp(euler.x, -maxRotationAngleX, maxRotationAngleX);
        euler.y = Mathf.Clamp(euler.y, -maxRotationAngleY, maxRotationAngleY);
        return Quaternion.Euler(euler);
    }

    IEnumerator ShootWhenAimed()
    {
        // Wait until the gun is sufficiently close to the target rotation
        while (Quaternion.Angle(transform.rotation, targetRotation) > aimThreshold)
        {
            yield return null;
        }

        ShootProjectile();
    }

    void ShootProjectile()
    {
        if (projectilePrefab == null || shootPoint == null)
        {
            Debug.LogError("ProjectilePrefab or ShootPoint is not assigned.");
            return;
        }

        GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, shootPoint.rotation);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 direction = shootPoint.forward;
            rb.velocity = direction * projectileSpeed;
        }
        else
        {
            Debug.LogError("Projectile does not have a Rigidbody component.");
        }

        // Set the color of the projectile
        Projectile projectileScript = projectile.GetComponent<Projectile>();
        if (projectileScript != null)
        {
            projectileScript.SetColor(selectedColor);
        }

        // Destroy the projectile after 5 seconds
        Destroy(projectile, 5f);
    }

    public void SetProjectileColor(string colorHex)
    {
        if (ColorUtility.TryParseHtmlString("#" + colorHex, out selectedColor))
        {
            Debug.Log("Selected Color: " + selectedColor);
        }
        else
        {
            Debug.LogError("Invalid color hex value: " + colorHex);
        }
    }

    IEnumerator ReturnToDefaultRotationAfterDelay()
    {
        yield return new WaitForSeconds(returnDelay);

        targetRotation = defaultRotation;

        while (Quaternion.Angle(transform.rotation, defaultRotation) > 0.01f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, defaultRotation, Time.deltaTime * rotationSpeed);
            yield return null;
        }

        transform.rotation = defaultRotation;
        returnRoutine = null;
    }
}
