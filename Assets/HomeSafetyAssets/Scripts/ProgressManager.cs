using UnityEngine;
using TMPro;

public class ProgressManager : MonoBehaviour
{
    public TMP_Text progressText;

    public int totalHazards = 5;
    private int hazardsFound = 0;

    void Start()
    {
        UpdateProgressText();
    }

    public void HazardCompleted()
    {
        if (hazardsFound < totalHazards)
        {
            hazardsFound++;
        }

        UpdateProgressText();

        Debug.Log("Hazards Found: " + hazardsFound + "/" + totalHazards);
    }

    private void UpdateProgressText()
    {
        if (progressText != null)
        {
            progressText.text =
                "Hazards Found: " + hazardsFound + "/" + totalHazards;
        }
    }
}