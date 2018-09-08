using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private float scrollForce;
    [SerializeField]
    private float zoomForce;
    [SerializeField]
    private float minimumZoomValue;
    [SerializeField]
    private float maximumZoomValue;
    [SerializeField]
    private Rigidbody rb;
    [SerializeField]
    private Camera camera;

    private float currentZoomValue;

    public void Start()
    {
        this.camera.transform.position = new Vector3(0.0f, 300.0f, 0.0f);
        this.currentZoomValue = this.maximumZoomValue - this.minimumZoomValue;
        this.camera.orthographicSize = this.currentZoomValue;
    }

    public void Scroll(Vector3 force)
    {
        this.rb.AddForce(force * -this.scrollForce, ForceMode.Impulse);
    }

    public void Zoom(float value)
    {
        this.currentZoomValue += value * this.zoomForce;

        if(this.currentZoomValue < this.minimumZoomValue)
        {
            this.currentZoomValue = this.minimumZoomValue;
        }
        else if(this.currentZoomValue > this.maximumZoomValue)
        {
            this.currentZoomValue = this.maximumZoomValue;
        }

        this.camera.orthographicSize = this.currentZoomValue;
    }
}
