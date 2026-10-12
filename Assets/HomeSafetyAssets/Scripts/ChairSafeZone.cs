using UnityEngine;

public class ChairSafeZone : MonoBehaviour
{
    public Transform chairTarget;

    private void OnTriggerStay(Collider other)
    {
        ChairHazard hazard =
            other.GetComponentInParent<ChairHazard>();

        if (hazard == null)
            return;

        // 抓着时不能完成
        if (hazard.IsSelected())
            return;

        // 松手后吸附到最终位置
        if (chairTarget != null)
{
    Rigidbody rb = hazard.GetComponent<Rigidbody>();

    if (rb != null)
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true;

        rb.position = chairTarget.position;
        rb.rotation = chairTarget.rotation;
    }
    else
    {
        hazard.transform.SetPositionAndRotation(
            chairTarget.position,
            chairTarget.rotation
        );
    }
}

hazard.ResolveHazard();
    }
}