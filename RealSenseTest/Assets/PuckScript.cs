using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class PuckScript : MonoBehaviour
{
    public int currentValue = 0;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entered trigger");
        if (other.gameObject.layer != 3) return;
        ValueArea valueArea = other.gameObject.GetComponent<ValueArea>();
        currentValue = valueArea.value;
    }

    
}

