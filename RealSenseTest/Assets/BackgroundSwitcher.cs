using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundSwitcher : MonoBehaviour
{
    [SerializeField] private GameObject normalBackground;
    [SerializeField] private GameObject videoBackground;

    public void ToggleBackground()
    {
        if (normalBackground.activeInHierarchy)
        {
            normalBackground.SetActive(false);
            videoBackground.SetActive(true);
        }
        else
        {
            normalBackground.SetActive(true);
            videoBackground.SetActive(false);
        }
    }
}
