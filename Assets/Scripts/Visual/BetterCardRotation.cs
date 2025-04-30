using UnityEngine;
using System.Collections;


[ExecuteInEditMode]
public class BetterCardRotation : MonoBehaviour
{
    public RectTransform CardFront;
    public RectTransform CardBack;
    public Transform targetFacePoint;
    public Collider col;
    private bool showingBack = false;
    void Update()
    {

        RaycastHit[] hits;
        hits = Physics.RaycastAll(origin: Camera.main.transform.position,
                                  direction: (-Camera.main.transform.position + targetFacePoint.position).normalized,
            maxDistance: (-Camera.main.transform.position + targetFacePoint.position).magnitude);
        bool passedThroughColliderOnCard = false;
        foreach (RaycastHit h in hits)
        {
            if (h.collider == col)
                passedThroughColliderOnCard = true;
        } 
        if (passedThroughColliderOnCard != showingBack)
        {
            showingBack = passedThroughColliderOnCard;
            if (showingBack)
            {
                CardFront.gameObject.SetActive(false);
                CardBack.gameObject.SetActive(true);
            }
            else
            {
                CardFront.gameObject.SetActive(true);
                CardBack.gameObject.SetActive(false);
            }

        }

    }

    //For testing raycasting
    //shows the exact line being raycasted
    void OnDrawGizmos()
    {
        if (targetFacePoint != null && Camera.main != null)
        {
            Gizmos.color = UnityEngine.Color.green;
            Vector3 origin = Camera.main.transform.position;
            Vector3 direction = (targetFacePoint.position - origin).normalized;
            float maxDistance = (targetFacePoint.position - origin).magnitude;

            Gizmos.DrawLine(origin, origin + direction * maxDistance);

            Gizmos.color = UnityEngine.Color.red;
            Gizmos.DrawSphere(targetFacePoint.position, 0.05f);
        }
    }
}
