using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRotation : MonoBehaviour
{
    [SerializeField]
    private float speed = 6;

    [SerializeField]
    private Vector3 rotationValue;

    private Tween t;

    private void OnEnable()
    {
        t = transform.DORotate(rotationValue, speed, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1);
    }

    private void OnDisable()
    {
        t.Kill();
    }
}
