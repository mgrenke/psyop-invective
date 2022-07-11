using System;
using UnityEngine;
using UnityEngine.AI;

/*	
	This component is for all objects that the player can
	interact with such as enemies, items etc. It is meant
	to be used as a base class.
*/

public class Interactable : MonoBehaviour {

    public float radius = 3f;
    public Transform interactionTransform;

    bool _isFocus;	// Is this interactable currently being focused?
    Transform _player;		// Reference to the player transform

    bool _hasInteracted;	// Have we already interacted with the object?

    private void Start()
    {
        if (interactionTransform == null)
            interactionTransform = transform;
    }

    void Update ()
    {
        if (_isFocus)	// If currently being focused
        {
            float distance = Vector3.Distance(_player.position, interactionTransform.position);
            // If we haven't already interacted and the player is close enough
            if (!_hasInteracted && distance <= radius)
            {
                // Interact with the object
                _hasInteracted = true;
                ItemPickup item = GetComponent<ItemPickup>();
                item.Interact();
            }
        }
    }

    // Called when the object starts being focused
    public void OnFocused (Transform playerTransform)
    {
        _isFocus = true;
        _hasInteracted = false;
        _player = playerTransform;
    }

    // Called when the object is no longer focused
    public void OnDefocused ()
    {
        _isFocus = false;
        _hasInteracted = false;
        _player = null;
    }

    // This method is meant to be overwritten
    public virtual void Interact ()
    {
		
    }

    void OnDrawGizmosSelected ()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(interactionTransform.position, radius);
    }

}