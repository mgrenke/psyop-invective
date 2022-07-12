using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(MeshRenderer))]
public class Switch : Interactable
{
    public TriggerAction action = TriggerAction.Trigger;
    public Triggerable[] targets;

    // Fix repetitive code (go to Trigger.cs)
    [Header("Item Required To Trigger")]
    public string requiredItemName = "";
    public bool takeItem;

    public override void Interact()
    {
        Inventory inv = Inventory.instance;
        if (inv != null && inv.HasItem(requiredItemName, takeItem))
            TriggerTargets(action);
    }

    public void TriggerTargets (TriggerAction action)
    {
        foreach (Triggerable t in targets)
        {
            // Check in case a target is destroyed
            if (t != null)
            {
                t.Trigger(action);
            }
        }
    }

    void OnDrawGizmosSelected ()
    {
        Gizmos.color = Color.green;

        // null check to avoid editor error when creating switch
        if (targets != null)
        {
            foreach (Triggerable t in targets)
            {
                if (t != null)
                {
                    Gizmos.DrawLine(transform.position, t.transform.position);
                }
            }
        }      
    }
}