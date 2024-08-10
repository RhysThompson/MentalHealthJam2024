using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
/// <summary>
/// used by the player's head to look at objects tagged with "objectOfInterest"
/// </summary>
public class LookAtObjects : MonoBehaviour
{
    public Transform LookTarget;
    public GameObject neck;
    public bool lookingAtObject;
    public float XLookOffset = 0.5f;
    public float maxHeadTurn = 90f;
    public float minHeadTurn = -90f;
    private void LateUpdate()
    {
        if (lookingAtObject)
        {
            Vector3 lookRotation = LookTarget.position - neck.transform.position;
            lookRotation.x += XLookOffset;
            //neck.transform.DORotate(lookRotation, 0.5f); this doesn't work because animator moves the neck back to its original position every frame
            neck.transform.LookAt(LookTarget);
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "ObjectOfInterest" && other.gameObject != gameObject)
        {
            LookTarget = other.transform;
            Vector3 lookRotation = (LookTarget.position - neck.transform.position).normalized;
            float angle = Vector3.SignedAngle(lookRotation, neck.transform.forward, Vector3.up);
            if (angle <= maxHeadTurn && angle >= minHeadTurn)
                lookingAtObject = true;
            else
                lookingAtObject = false;
            
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "ObjectOfInterest" && other.gameObject != gameObject)
        {
            lookingAtObject = false;
            LookTarget = null;
        }
    }
}
