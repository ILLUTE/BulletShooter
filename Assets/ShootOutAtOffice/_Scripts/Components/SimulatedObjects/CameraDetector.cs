using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraDetector : MonoBehaviour
{
    public float waitTime;

    public Transform cameraTransform;

    public Vector3 startPosition;
    public Vector3 endPosition;

    private void Start()
    {
        Sequence mSequence = DOTween.Sequence();
        mSequence.AppendInterval(waitTime);
        mSequence.Append(cameraTransform.DOLocalRotate(endPosition, waitTime));
        mSequence.SetLoops(-1, LoopType.Yoyo);
    }
}
