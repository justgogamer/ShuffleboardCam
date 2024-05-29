using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValueArea : MonoBehaviour
{
    public int value;
    public float middleX;

    private void Start()
    {
        middleX = GetComponent<BoxCollider>().bounds.center.x;
    }
}
