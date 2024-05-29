using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuckCoreScript : MonoBehaviour
{
    public string currentLayerName;
    public int currentValue;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collided");
        if (other.gameObject.layer != 3 || other.gameObject.tag == "Puck") return;
        currentLayerName = other.gameObject.name;
        currentValue = other.gameObject.GetComponent<ValueArea>().value;
    }
}
