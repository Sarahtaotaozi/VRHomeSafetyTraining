using UnityEngine;

public class SafeZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Hazard hazard = other.GetComponent<Hazard>();

        if (hazard != null)
        {
            hazard.ResolveHazard();
        }
    }
}