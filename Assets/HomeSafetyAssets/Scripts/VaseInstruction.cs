using TMPro;
using UnityEngine;

public class VaseInstruction : MonoBehaviour
{
    public GameObject popupPanel;
    public TMP_Text instructionText;
    public Transform safeVaseLocation;

    public float safeDistance = 0.5f;

    [TextArea]
    public string instruction =
        "Drag the vase to the marked area in the middle.";

    private bool isDragging;
    private bool isSafe;
    private float vaseHeight;
    private Vector3 dragOffset;
    private Plane dragPlane;

    void Start()
    {
        vaseHeight = transform.position.y;

        if (popupPanel != null)
        {
            popupPanel.SetActive(false);
        }
    }

    void OnMouseDown()
    {
        if (isSafe)
        {
            return;
        }

        isDragging = true;

        dragPlane = new Plane(
            Vector3.up,
            new Vector3(0, vaseHeight, 0)
        );

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (dragPlane.Raycast(ray, out float distance))
        {
            dragOffset = transform.position - ray.GetPoint(distance);
        }

        ShowMessage(instruction);
    }

    void OnMouseDrag()
    {
        if (!isDragging || isSafe)
        {
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (dragPlane.Raycast(ray, out float distance))
        {
            Vector3 newPosition = ray.GetPoint(distance) + dragOffset;
            newPosition.y = vaseHeight;
            transform.position = newPosition;
        }
    }

    void OnMouseUp()
    {
        if (!isDragging || isSafe)
        {
            return;
        }

        isDragging = false;

        if (safeVaseLocation != null)
        {
            Vector3 currentPosition = transform.position;
            Vector3 targetPosition = safeVaseLocation.position;

            currentPosition.y = 0;
            targetPosition.y = 0;

            if (Vector3.Distance(currentPosition, targetPosition) <= safeDistance)
            {
                transform.position = safeVaseLocation.position;
                isSafe = true;

                ShowMessage("The vase is in a safe location.");
            }
        }
    }

    private void ShowMessage(string message)
    {
        if (popupPanel != null)
        {
            popupPanel.SetActive(true);
        }

        if (instructionText != null)
        {
            instructionText.text = message;
        }
    }

    public void ClosePopup()
    {
        if (popupPanel != null)
        {
            popupPanel.SetActive(false);
        }
    }
}