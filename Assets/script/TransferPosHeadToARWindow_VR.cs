using UnityEngine;

public class TransferPosHeadToARWindow_VR : MonoBehaviour
{
    [Header("Head reference (CenterEyeAnchor del OVRCameraRig)")]
    public Transform headAnchor;

    [Header("Modelo anatómico")]
    public LayerMask modelLayerMask;
    public float maxDistance = 5f;
    public float defaultDistance = 1f;

    [Header("Suavizado (recomendado para confort VR)")]
    public float smoothSpeed = 12f;

    void Update()
    {
        if (headAnchor == null) return;

        Vector3 targetPos;
        Ray ray = new Ray(headAnchor.position, headAnchor.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, modelLayerMask))
        {
            targetPos = hit.point;
        }
        else
        {
            targetPos = headAnchor.position + headAnchor.forward * defaultDistance;
        }

        transform.position = Vector3.Lerp(
            transform.position,
            targetPos,
            Time.deltaTime * smoothSpeed
        );
    }
}