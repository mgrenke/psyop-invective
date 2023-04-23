using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Door : Triggerable
{
    public Transform hinge;
    public bool slidingDoor = false;
    public bool doorIsOpen = false;
    public float currentAngle = 0f;
    public float endAngle = 90f;
    public float swingSpeed = 40f;

    [Header("Movement")]
    public float moveSpeed = 5f;
    public Vector3 moveOffset;

    private Vector3 _startPosition;
    private Vector3 _endPosition;
    private Vector3 _targetPosition;
    private Coroutine _update;
    private Rigidbody _rigidbody;

    void Awake ()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.isKinematic = true;
        hinge = GetComponentInChildren<Hinge>().transform;

        Validate();
    }

    void OnValidate ()
    {
        Validate();
    }

    void Validate ()
    {
        // Transform the offset so that it works even when the door is rotated
        Vector3 offsetLocal = transform.TransformVector(moveOffset);

        _startPosition = transform.position;
        _endPosition = _startPosition + offsetLocal;
    }

    public override void Trigger (TriggerAction action)
    {
        // Support the door opening and closing
        if (action == TriggerAction.Toggle)
        {
            _targetPosition = (_targetPosition == _endPosition) ? _startPosition : _endPosition; 
        }
        else
        {
            _targetPosition = (action == TriggerAction.Deactivate) ? _startPosition : _endPosition;
        }

        if (_update != null)
        {
            StopCoroutine(_update);
            _update = null;
        }

        if(!slidingDoor)
        {
            _update = StartCoroutine(SwingOpen());
        }

        else
        {
            _update = StartCoroutine(MoveToTarget());
        }
    }

    // The door only needs to update when opening or closing
    // Should be used in case of a sliding door.
    IEnumerator MoveToTarget ()
    {
        while (true)
        {
            Vector3 offset = _targetPosition - transform.position;
            float distance = offset.magnitude;
            float moveDistance = moveSpeed * Time.deltaTime;

            // Keep moving towards target until we are close enough
            if (moveDistance < distance)
            {
                Vector3 move = offset.normalized * moveDistance;
                _rigidbody.MovePosition(transform.position + move);
                yield return null;
            }
            else
            {
                break;
            }
        }

        _rigidbody.MovePosition(_targetPosition);
        _update = null;
    }

IEnumerator SwingOpen ()
    {
        currentAngle = transform.rotation.y;        

        if (!doorIsOpen)
        {
            endAngle = 90f;
            while (true)
            {
                // Keep moving towards target until we are close enough
                if (currentAngle < endAngle)
                {
                    currentAngle = transform.rotation.eulerAngles.y + (swingSpeed * Time.deltaTime);
                    if (currentAngle > 360f) currentAngle -= 360f;
                    transform.RotateAround(hinge.position, Vector3.up, swingSpeed * Time.deltaTime);
                    yield return null;
                }
                else
                {
                    doorIsOpen = !doorIsOpen;
                    break;
                }
            }
            //transform.RotateAround(hinge.position, Vector3.up, swingSpeed * Time.deltaTime);       
            _update = null;
        }
        
        else
        {
            endAngle = 0f;
            while (true)
            {
                // Keep moving towards target until we are close enough
                if (currentAngle > endAngle)
                {
                    currentAngle = transform.rotation.eulerAngles.y - (swingSpeed * Time.deltaTime);
                    if (currentAngle > 360f) currentAngle -= 360f;
                    transform.RotateAround(hinge.position, Vector3.up, -1f * swingSpeed * Time.deltaTime);
                    yield return null;
                }
                else
                {
                    doorIsOpen = !doorIsOpen;
                    break;
                }
            }
            //transform.RotateAround(hinge.position, Vector3.up, -1f * swingSpeed * Time.deltaTime);       
            _update = null;
        }

    }

    // This will make setting up the door movement in editor much easier
    void OnDrawGizmosSelected ()
    {
        Gizmos.matrix = transform.localToWorldMatrix;

        MeshFilter mf = GetComponent<MeshFilter>();
        if (mf != null)
        {
            Gizmos.DrawWireMesh(mf.sharedMesh, moveOffset);
        }
        else
        {
            Gizmos.DrawLine(Vector3.zero, moveOffset);
            Gizmos.DrawWireSphere(moveOffset, 0.05f);
        }
    }
}
