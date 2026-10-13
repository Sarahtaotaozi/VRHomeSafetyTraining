using UnityEngine;

public class TrashBinSafeZone : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        TrashBinHazard hazard =
            other.GetComponentInParent<TrashBinHazard>();

        if (hazard == null)
            return;

        if (!hazard.IsSelected())
        {
            hazard.ResolveHazard();
        }
    }
}