using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public float interactionRadius = 3f;
    public Transform interactionTransform;
    
    private bool _isFocus = false;
    private Transform _player;

    private bool _hasInteracted = false;

    public virtual void Interact()
    {
        // This method is meant to be overwritten
    }
    
    private void Update()
    {
        if (_isFocus && !_hasInteracted)
        {
            float distance = Vector3.Distance(_player.position, interactionTransform.position);
            if (distance <= interactionRadius)
            {
                Interact();
                _hasInteracted = true;
            }
        }
    }

    public void OnFocus(Transform playerTransform)
    {
        _isFocus = true;
        _player = playerTransform;
        _hasInteracted = false;
    }
    
    public void OnDefocus()
    {
        _isFocus = false;
        _player = null;
        _hasInteracted = false;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(interactionTransform.position, interactionRadius);
    }
}
