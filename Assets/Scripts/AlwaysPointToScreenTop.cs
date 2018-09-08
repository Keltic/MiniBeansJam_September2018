using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlwaysPointToScreenTop : MonoBehaviour
{
    public void Update()
    {
        this.transform.localRotation = Quaternion.Euler(90.0f, 0.0f, this.transform.parent.transform.localRotation.eulerAngles.y);
    }
}
