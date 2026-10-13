using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class DrawerSlider : MonoBehaviour
{
    public XRGrabInteractable grabInteractable;
    public Transform closedTarget;
    public float maxOpenDistance = 0.5f;

    private Transform currentHand;

    private Vector3 closedLocalPosition;
    private Vector3 openDirection;

    private Vector3 handStartLocalPosition;
    private float drawerStartDistance;

    void Start()
    {
        closedLocalPosition = closedTarget.localPosition;

        // 自动判断“抽屉打开方向”
        openDirection =
            (transform.localPosition - closedLocalPosition).normalized;

        grabInteractable.selectEntered.AddListener(OnGrabbed);
        grabInteractable.selectExited.AddListener(OnReleased);
    }

    private void OnGrabbed(SelectEnterEventArgs args)
    {
        currentHand = args.interactorObject.transform;

        handStartLocalPosition =
            transform.parent.InverseTransformPoint(currentHand.position);

        drawerStartDistance =
            Vector3.Dot(
                transform.localPosition - closedLocalPosition,
                openDirection
            );
    }

    private void OnReleased(SelectExitEventArgs args)
    {
        currentHand = null;
    }

    void Update()
    {
        if (currentHand == null)
            return;

        Vector3 currentHandLocal =
            transform.parent.InverseTransformPoint(currentHand.position);

        Vector3 handMovement =
            currentHandLocal - handStartLocalPosition;

        float movementAlongDrawer =
            Vector3.Dot(handMovement, openDirection);

        float newDistance =
            Mathf.Clamp(
                drawerStartDistance + movementAlongDrawer,
                0f,
                maxOpenDistance
            );

        transform.localPosition =
            closedLocalPosition +
            openDirection * newDistance;
    }
}