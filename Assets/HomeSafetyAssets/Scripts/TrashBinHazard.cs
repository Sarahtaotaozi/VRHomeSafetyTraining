using UnityEngine;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class TrashBinHazard : MonoBehaviour
{
    [Header("Warning Icon")]
    public GameObject warningIcon;

    [Header("Safety Tip UI")]
    public GameObject safetyTipPanel;
    public TMP_Text titleText;
    public TMP_Text bodyText;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip warningClip;

    [Header("Progress")]
    public ProgressManager progressManager;

    private XRGrabInteractable grabInteractable;

    private bool identified = false;
    private bool resolved = false;

    void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
    }

    void Start()
    {
        // Warning icon should be hidden at the beginning
        if (warningIcon != null)
            warningIcon.SetActive(false);

        // Detect when user selects/grabs the trash bin
        if (grabInteractable != null)
            grabInteractable.selectEntered.AddListener(OnSelected);
    }

    void OnDestroy()
    {
        if (grabInteractable != null)
            grabInteractable.selectEntered.RemoveListener(OnSelected);
    }

    private void OnSelected(SelectEnterEventArgs args)
    {
        IdentifyHazard();
    }

    public void IdentifyHazard()
    {
        if (resolved)
            return;

        // Show large warning icon
        if (warningIcon != null)
            warningIcon.SetActive(true);

        // Show Safety Tip
        if (safetyTipPanel != null)
            safetyTipPanel.SetActive(true);

        if (titleText != null)
            titleText.text = "Trash Bin in Walkway";

        if (bodyText != null)
        {
            bodyText.text =
                "A trash bin blocking the walkway can cause trips and falls.\n\n" +
                "Move it away from the walking path.";
        }

        // Only play the warning once
        if (!identified)
        {
            identified = true;

            if (audioSource != null && warningClip != null)
                audioSource.PlayOneShot(warningClip);
        }

        Debug.Log("Trash Bin hazard identified!");
    }

    public void ResolveHazard()
    {
        if (resolved)
            return;

        resolved = true;

        // Remove warning icon
        if (warningIcon != null)
            warningIcon.SetActive(false);

        // Close Safety Tip
        if (safetyTipPanel != null)
            safetyTipPanel.SetActive(false);

        // Update progress
        if (progressManager != null)
            progressManager.HazardCompleted();

        Debug.Log("Trash Bin hazard resolved!");
    }

    public bool IsSelected()
    {
        return grabInteractable != null && grabInteractable.isSelected;
    }
}