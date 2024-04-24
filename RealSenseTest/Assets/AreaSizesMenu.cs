using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaSizesMenu : MonoBehaviour
{
    public GameObject AreaMenuObj;
    public GameObject BorderMenuObj;
    public void ToggleMenu()
    {
        AreaMenuObj.SetActive(!(AreaMenuObj.activeInHierarchy));
    }

    public void ToggleBorderMenu()
    {
        BorderMenuObj.SetActive(!(BorderMenuObj.activeInHierarchy));
    }
}
