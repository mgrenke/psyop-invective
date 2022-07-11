using System.Collections;
using System.Collections.Generic;
using DialogueEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    public Camera cam;
    private PlayerMotor _motor;
    public Interactable focus;

    void Start()
    {
        cam = Camera.main;
        _motor = GetComponent<PlayerMotor>();
    }

    // Update is called once per frame
    void Update()
    {
        // LMB is responsible for movement
        if (Input.GetMouseButtonDown(0))
        {
            if(EventSystem.current.IsPointerOverGameObject())
                return;
            
            if(ConversationManager.Instance.IsConversationActive)
                ConversationManager.Instance.EndConversation();
            
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                _motor.MoveToPoint(hit.point);
                RemoveFocus();
            }
        }
        
        // RMB is responsible for interaction
        if (Input.GetMouseButtonDown(1))
        {
            if(EventSystem.current.IsPointerOverGameObject())
                return;

            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Interactable interactable = hit.collider.GetComponent<Interactable>();
                if (interactable != null)
                    SetFocus(interactable);
            }
        }
    }

    void SetFocus(Interactable newFocus)
    {
        if (newFocus != focus)
        {
            if(focus != null)
                focus.OnDefocused();
            
            focus = newFocus;
            _motor.FollowTarget(newFocus);
        }

        newFocus.OnFocused(transform);
    }

    void RemoveFocus()
    {
        if(focus != null)
            focus.OnDefocused();
        
        focus = null;
        _motor.StopFollowingTarget();
    }
}
