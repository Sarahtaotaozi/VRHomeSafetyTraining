using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProgressManager : MonoBehaviour
{
    public TMP_Text progressText;
    public Slider progressBar;

    public int totalHazards = 5;
    private int hazardsFound = 0;

    void Start()
    {
        progressBar.minValue = 0;
        progressBar.maxValue = totalHazards;
        progressBar.value = hazardsFound;

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

        if (progressBar != null)
        {
            progressBar.value = hazardsFound;
        }
    }
}
