using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OutlineOnHover : MonoBehaviour
{
    private Outline outlineScript;
    public UnityEvent OnClick;
    // Start is called before the first frame update
    void Start()
    {
        outlineScript = GetComponent<Outline>();
    }

    void OnMouseEnter ()
    {
        outlineScript.enabled = true;
    }

    void OnMouseExit()
    {
        outlineScript.enabled = false;
    }

    private void OnMouseDown()
    {
        OnClick.Invoke();
    }
}
