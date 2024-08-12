using UnityEngine;
/// <summary>
/// used by the player's head to look at objects tagged with "objectOfInterest"
/// </summary>
public class LookAtObjects : MonoBehaviour
{
    public Transform LookTarget;
    public GameObject head;
    public bool lookingAtObject;
    public float maxHeadTurn = 80f;
    public float minHeadTurn = -80f;
    private Animator animator;
    public float lookSpeed = 0.5f;
    private float turnProgress;
    private float turnAwayProgress;
    public float lookWeight = 1;
    private bool lookingAway;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (lookingAtObject)
        {
            turnProgress += Time.deltaTime * lookSpeed;
            
            Vector3 modifiedLookTarget = Vector3.Lerp(head.transform.position + head.transform.forward, LookTarget.position, turnProgress);
            animator.SetLookAtPosition(modifiedLookTarget);
            animator.SetLookAtWeight(lookWeight);
        }
        else if (lookingAway)
        {
            turnAwayProgress += Time.deltaTime * lookSpeed;

            Vector3 modifiedLookTarget = Vector3.Lerp(LookTarget.position, head.transform.position + head.transform.forward, turnAwayProgress);
            animator.SetLookAtPosition(modifiedLookTarget);
            animator.SetLookAtWeight(lookWeight/2);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "ObjectOfInterest" && other.gameObject != gameObject)
        {
            LookTarget = other.transform;
            turnProgress = 0;
        }
    }
    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "ObjectOfInterest" && other.gameObject != gameObject)
        {
            Vector3 lookRotation = (LookTarget.position - head.transform.position);
            float angle = Vector3.SignedAngle(lookRotation, head.transform.parent.forward, Vector3.up);
            if (angle <= maxHeadTurn && angle >= minHeadTurn)
            {
                lookingAtObject = true;
                turnAwayProgress =0;
            }
            else
            {
                lookingAtObject = false;
                lookingAway = true;
                turnProgress = 0;
            }
            
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "ObjectOfInterest" && other.gameObject != gameObject)
        {
            lookingAtObject = false;
            LookTarget = null;
            turnProgress = 0;
            lookingAway = false;
        }
    }


}
