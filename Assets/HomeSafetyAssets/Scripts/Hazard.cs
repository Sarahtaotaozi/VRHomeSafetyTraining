using UnityEngine;
using TMPro;

public class Hazard : MonoBehaviour
{
    // Safety Tip UI
    public GameObject safetyTipPanel;
    public TMP_Text titleText;
    public TMP_Text bodyText;

    // Highlight
    public Renderer hazardRenderer;
    public Material highlightMaterial;

    // Audio
    public AudioSource audioSource;
    public AudioClip warningClip;

    // Progress
    public ProgressManager progressManager;

    private Material originalMaterial;

    private bool identified = false;
    private bool resolved = false;

    void Start()
    {
        // Save original rug material
        if (hazardRenderer != null)
        {
            originalMaterial = hazardRenderer.sharedMaterial;
        }
    }

    public void IdentifyHazard()
    {
        // Do nothing if already resolved
        if (resolved)
        {
            return;
        }

        // Show Safety Tip
        if (safetyTipPanel != null)
        {
            safetyTipPanel.SetActive(true);
        }

        if (titleText != null)
        {
            titleText.text = "Loose Rug";
        }

        if (bodyText != null)
        {
            bodyText.text =
                "Loose rugs can slip and cause you to fall.\n\n" +
                "Move the rug to a safe location.";
        }

        // Highlight rug
        if (hazardRenderer != null && highlightMaterial != null)
        {
            hazardRenderer.sharedMaterial = highlightMaterial;
        }

        // Play audio only the first time
        if (!identified)
        {
            identified = true;

            if (audioSource != null && warningClip != null)
            {
                audioSource.PlayOneShot(warningClip);
            }
        }

        Debug.Log("Loose Rug hazard identified!");
    }

    public void RemoveHighlight()
    {
        if (hazardRenderer != null && originalMaterial != null)
        {
            hazardRenderer.sharedMaterial = originalMaterial;
        }
    }

    public void ResolveHazard()
    {
        // Prevent duplicate completion
        if (resolved)
        {
            return;
        }

        resolved = true;

        Debug.Log("Loose Rug resolved!");

        // Restore original material
        RemoveHighlight();

        // Close Safety Tip
        if (safetyTipPanel != null)
        {
            safetyTipPanel.SetActive(false);
        }

        // Increase progress by 1
        if (progressManager != null)
        {
            progressManager.HazardCompleted();
        }
    }
}