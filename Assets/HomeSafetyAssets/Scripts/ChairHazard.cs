using UnityEngine;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class ChairHazard : MonoBehaviour
{
    [Header("Safety Tip")]
    public GameObject safetyTipPanel;
    public TMP_Text titleText;
    public TMP_Text bodyText;

    [Header("Visual Accessibility")]
    public Renderer chairRenderer;
    public Renderer legsRenderer;
    public Material warningMaterial;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip warningClip;

    [Header("Progress")]
    public ProgressManager progressManager;

    private XRGrabInteractable grabInteractable;

    private Material originalChairMaterial;
    private Material originalLegsMaterial;

    private bool identified = false;
    private bool resolved = false;

    void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
    }

    void Start()
    {
        if (chairRenderer != null)
            originalChairMaterial = chairRenderer.sharedMaterial;

        if (legsRenderer != null)
            originalLegsMaterial = legsRenderer.sharedMaterial;

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

        if (safetyTipPanel != null)
            safetyTipPanel.SetActive(true);

        if (titleText != null)
            titleText.text = "Chair Blocking Walkway";

        if (bodyText != null)
        {
            bodyText.text =
                "A chair blocking the walkway can cause trips and falls.\n\n" +
                "Move the chair back to a safe location near the dining table.";
        }

        // High-contrast visual warning
        if (warningMaterial != null)
        {
            if (chairRenderer != null)
                chairRenderer.sharedMaterial = warningMaterial;

            if (legsRenderer != null)
                legsRenderer.sharedMaterial = warningMaterial;
        }

        if (!identified)
        {
            identified = true;

            if (audioSource != null && warningClip != null)
                audioSource.PlayOneShot(warningClip);
        }

        Debug.Log("Chair hazard identified!");
    }

    public void ResolveHazard()
    {
        if (resolved || !identified)
            return;

        resolved = true;

        if (chairRenderer != null && originalChairMaterial != null)
            chairRenderer.sharedMaterial = originalChairMaterial;

        if (legsRenderer != null && originalLegsMaterial != null)
            legsRenderer.sharedMaterial = originalLegsMaterial;

        if (safetyTipPanel != null)
            safetyTipPanel.SetActive(false);

        if (progressManager != null)
            progressManager.HazardCompleted();

        Debug.Log("Chair hazard resolved!");
    }

    public bool IsSelected()
    {
        return grabInteractable != null && grabInteractable.isSelected;
    }
}