using UnityEngine;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class DrawerHazard : MonoBehaviour
{
    [Header("Safety Tip")]
    public GameObject safetyTipPanel;
    public TMP_Text titleText;
    public TMP_Text bodyText;

    [Header("Warning Icon")]
    public GameObject warningIcon;

    [Header("Visual - Optional")]
    public Renderer drawerRenderer;
    public Material warningMaterial;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip warningClip;

    [Header("Interaction")]
    public XRGrabInteractable grabInteractable;
    public Transform closedTarget;
    public float closeDistance = 0.15f;

    [Header("Progress")]
    public ProgressManager progressManager;

    private Material originalMaterial;

    private bool identified = false;
    private bool resolved = false;


    private void Start()
    {
        // Save original material only if color changing is being used
        if (drawerRenderer != null)
        {
            originalMaterial = drawerRenderer.sharedMaterial;
        }

        // Hide warning icon at the beginning
        if (warningIcon != null)
        {
            warningIcon.SetActive(false);
        }

        // Listen for Grab and Release
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.AddListener(OnGrabbed);
            grabInteractable.selectExited.AddListener(OnReleased);
        }
    }


    private void OnDestroy()
    {
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.RemoveListener(OnGrabbed);
            grabInteractable.selectExited.RemoveListener(OnReleased);
        }
    }


    // Called when the player grabs the drawer
    private void OnGrabbed(SelectEnterEventArgs args)
    {
        IdentifyHazard();
    }


    // Called when the player releases the drawer
    private void OnReleased(SelectExitEventArgs args)
    {
        CheckIfClosed();
    }


    private void IdentifyHazard()
    {
        if (resolved)
            return;

        // Only trigger warning once
        if (identified)
            return;

        identified = true;

        Debug.Log("Open drawer hazard identified!");


        // Show warning icon
        if (warningIcon != null)
        {
            warningIcon.SetActive(true);
        }


        // Show Safety Tip
        if (safetyTipPanel != null)
        {
            safetyTipPanel.SetActive(true);
        }


        if (titleText != null)
        {
            titleText.text = "Open Drawer";
        }


        if (bodyText != null)
        {
            bodyText.text =
                "An open drawer can cause trips and injuries.\n\n" +
                "Push the drawer closed.";
        }


        // Optional yellow highlight
        // If Drawer Renderer and Warning Material are None,
        // this part is simply skipped.
        if (drawerRenderer != null && warningMaterial != null)
        {
            drawerRenderer.sharedMaterial = warningMaterial;
        }


        // Play warning audio
        if (audioSource != null && warningClip != null)
        {
            audioSource.PlayOneShot(warningClip);
        }
    }

    private void CheckIfClosed()
{
    if (!identified || resolved || closedTarget == null)
        return;

    float distance = Vector3.Distance(
        transform.position,
        closedTarget.position
    );

    Debug.Log("Drawer distance from closed target: " + distance);

    if (distance <= closeDistance)
    {
        Rigidbody rb = GetComponent<Rigidbody>();

        if (rb != null)
        {
            if (!rb.isKinematic)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }

            rb.isKinematic = true;
        }

        transform.SetPositionAndRotation(
            closedTarget.position,
            closedTarget.rotation
        );

        ResolveHazard();
    }
}
   


    private void ResolveHazard()
    {
        if (resolved)
            return;

        resolved = true;

        Debug.Log("Drawer hazard resolved!");


        // Restore original material if color change was used
        if (drawerRenderer != null && originalMaterial != null)
        {
            drawerRenderer.sharedMaterial = originalMaterial;
        }


        // Hide exclamation mark
        if (warningIcon != null)
        {
            warningIcon.SetActive(false);
        }


        // Hide Safety Tip
        if (safetyTipPanel != null)
        {
            safetyTipPanel.SetActive(false);
        }


        // Increase progress only once
        if (progressManager != null)
        {
            progressManager.HazardCompleted();
        }


        // Prevent the player from grabbing the drawer again
        if (grabInteractable != null)
        {
            grabInteractable.enabled = false;
        }
    }
}