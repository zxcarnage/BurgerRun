using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingFood : MonoBehaviour
{
    [SerializeField] private Vector3 _rotation = new Vector3 (0, 60, 5);

    private void Update()
    {
        transform.Rotate(_rotation * Time.deltaTime);
    }

    private void Start()
    {
        Sequence rotatingSequence = DOTween.Sequence();
        rotatingSequence.SetLoops(-1);

        rotatingSequence.Append(transform.DOMoveY(transform.position.y + 0.35f, 2));
        rotatingSequence.Append(transform.DOMoveY(transform.position.y, 2));
    }
}
