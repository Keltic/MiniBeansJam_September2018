using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputController : MonoBehaviour
{
    public enum UpgradeModes
    {
        None,
        Runner,
        Shooter,
        Bomber,
        Trickster
    }

    [SerializeField]
    private float scrollingThreshold;
    [SerializeField]
    private CameraController cameraController;
    private bool checkForDrag = false;
    private Vector3 cursorPosThisFrame;
    private Vector3 cursorPosLastFrame;
    private float cursorPosDifference;
    private Vector3 cursorDragDirection;
    private bool wasDrag = false;
    private UpgradeModes currentUpgradeMode = UpgradeModes.None;
    private GameObject markedCharacter = null;

    public void Update()
    {
        if (!EventSystem.current.IsPointerOverGameObject() && EventSystem.current.currentSelectedGameObject == null)
        {
            RaycastHit hitInfo;
            Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo, 500.0f);

            if (hitInfo.collider != null && hitInfo.collider.gameObject != null)
            {
                this.ReleaseMarkedCharacter();

                this.markedCharacter = hitInfo.collider.gameObject;
                Transform child = this.markedCharacter.transform.Find("Sprite");
                if(child != null)
                {
                    SpriteRenderer sr = child.gameObject.GetComponent<SpriteRenderer>();
                    if(sr != null)
                    {
                        sr.color = Color.cyan;
                    }
                }
            }
            else
            {
                this.ReleaseMarkedCharacter();
            }
        }
        else
        {
            this.ReleaseMarkedCharacter();
        }

        //LMB Up
        if (Input.GetMouseButtonUp(0))
        {
            if(this.markedCharacter != null)
            {
                ClickableComponent clickable = this.markedCharacter.GetComponent<ClickableComponent>();
                if (clickable != null)
                {
                    clickable.OnClick(this.currentUpgradeMode);
                }
            }
            
        }

        //RMB Down
        if (Input.GetMouseButtonDown(1))
        {
            
        }

        //RMB Up
        if (Input.GetMouseButtonUp(1))
        {
            if (!this.wasDrag)
            {
                GameGuiController gui = GameObject.Find("Canvas_Game").GetComponent<GameGuiController>();
                if (gui != null)
                {
                    gui.ShowViewRadiusMarker(false);
                    gui.ShowShootRadiusMarker(false);
                    gui.ShowTextAtMouse(false);
                }

                this.SwitchToUpgradeMode(UpgradeModes.None);
            }

            this.wasDrag = false;
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
                    this.wasDrag = true;
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
            this.cameraController.Zoom(Input.GetAxis("Mouse ScrollWheel") * -1);
        }
    }

    public void SwitchToUpgradeMode(UpgradeModes newMode)
    {
        this.currentUpgradeMode = newMode;
    }

    private void ReleaseMarkedCharacter()
    {
        if (this.markedCharacter != null)
        {
            Transform child = this.markedCharacter.transform.Find("Sprite");
            if (child != null)
            {
                SpriteRenderer spriteRenderer = child.gameObject.GetComponent<SpriteRenderer>();
                if (spriteRenderer != null)
                {
                    spriteRenderer.color = Color.white;
                }
            }

            this.markedCharacter = null;
        }
    }
}
