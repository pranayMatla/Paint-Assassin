using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Button redButton;
    public Button blueButton;
    public Button yellowButton;
    private GunController gunController;

    void Start()
    {
        gunController = FindObjectOfType<GunController>();
        if (gunController == null)
        {
            Debug.LogError("GunController not found in the scene.");
            return;
        }

        redButton.onClick.AddListener(() => gunController.SetProjectileColor("C185E8"));
        blueButton.onClick.AddListener(() => gunController.SetProjectileColor("00FFFF"));
        yellowButton.onClick.AddListener(() => gunController.SetProjectileColor("FFFF4F"));
    }

    public bool IsTouchOverUIButton(Vector2 touchPosition)
    {
        // Check if the touch is over a UI button
        return RectTransformUtility.RectangleContainsScreenPoint(redButton.GetComponent<RectTransform>(), touchPosition, Camera.main) ||
               RectTransformUtility.RectangleContainsScreenPoint(blueButton.GetComponent<RectTransform>(), touchPosition, Camera.main) ||
               RectTransformUtility.RectangleContainsScreenPoint(yellowButton.GetComponent<RectTransform>(), touchPosition, Camera.main);
    }
}
