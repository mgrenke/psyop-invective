using UnityEngine;

public class CameraParallax : MonoBehaviour
{
    private Vector3 pz;
    private Quaternion StartRot;

    public float moveModifier;

    void Start()
    {
        StartRot = transform.rotation;
    }

    void Update()
    {
        var pz = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        pz.z = 0;
        gameObject.transform.rotation = Quaternion.identity;
        transform.rotation = Quaternion.Euler(StartRot.x - (pz.y * moveModifier), StartRot.y + (pz.x * moveModifier), 0);
    }
}