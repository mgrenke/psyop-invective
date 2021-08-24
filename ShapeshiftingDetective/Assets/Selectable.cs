using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selectable : MonoBehaviour
{
    [SerializeField]
    private LayerMask layerMask;
    private Collider _selection;

    // Update is called once per frame
    void Update()
    {
        if(_selection != null)
        {
            var outline = _selection.GetComponent<Outline>();
            outline.enabled = false;
            _selection = null;
        }

        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            var selection = hit.collider;
            var outline = selection.GetComponent<Outline>();
            if(outline != null) outline.enabled = true;

            _selection = selection;
        }
    }
}
