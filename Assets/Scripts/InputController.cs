using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    [SerializeField]
    private float scrollingThreshold;
    [SerializeField]
    private CameraController cameraController;
    private bool checkForDrag = false;
    private Vector3 cursorPosThisFrame;
    private Vector3 cursorPosLastFrame;
    private float cursorPosDifference;
    private Vector3 cursorDragDirection;

    private void Update()
    {
        //LMB Up
        if (Input.GetMouseButtonUp(0))
        {

        }

        //RMB Down
        if (Input.GetMouseButtonDown(1))
        {
            
        }

        //RMB Up
        if (Input.GetMouseButtonUp(1))
        {
            this.checkForDrag = false;
            this.cursorPosThisFrame = Vector3.zero;
            this.cursorPosLastFrame = Vector3.zero;
        }

        //RMB Hold
        if (Input.GetMouseButton(1))
        {
            if (this.checkForDrag)
            {
                this.cursorPosThisFrame = Input.mousePosition;
                this.cursorPosDifference = Vector3.Distance(this.cursorPosThisFrame, this.cursorPosLastFrame);

                if(this.cursorPosDifference > this.scrollingThreshold)
                {
                    this.cursorDragDirection = this.cursorPosThisFrame - this.cursorPosLastFrame;
                    this.cameraController.Scroll(new Vector3(this.cursorDragDirection.x, this.cursorDragDirection.z, this.cursorDragDirection.y)); //Flip y and z axis because camera is using 3D space.
                }
            }
            else
            {
                this.checkForDrag = true;
                this.cursorPosThisFrame = Input.mousePosition;
            }

            this.cursorPosLastFrame = this.cursorPosThisFrame;
        }

        //MouseWheel
        if(Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            this.cameraController.Zoom(Input.GetAxis("Mouse ScrollWheel"));
        }
    }
}
