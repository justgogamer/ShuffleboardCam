using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ValueAreas
{
    Area1,
    Area2, 
    Area3, 
    Area4,
}

public class VariableGameZones : MonoBehaviour
{
    [Header("ValueArea transforms")]
    public Transform valueArea0Trans;
    public Transform valueArea1Trans;
    public Transform valueArea2Trans;
    public Transform valueArea3Trans;
    public Transform valueArea4Trans;

    [Header("ValueArea Original Sizes")]
    public float totalSizeAreas;
    public float valueArea0Size;
    public float valueArea1Size;
    public float valueArea2Size;
    public float valueArea3Size;
    public float valueArea4Size;

    private void Start()
    {
        valueArea0Size = valueArea0Trans.localScale.x;
        valueArea1Size = valueArea1Trans.localScale.x;
        valueArea2Size = valueArea2Trans.localScale.x;
        valueArea3Size = valueArea3Trans.localScale.x;
        valueArea4Size = valueArea4Trans.localScale.x;
        totalSizeAreas = valueArea0Size + valueArea1Size + valueArea2Size + valueArea3Size + valueArea4Size;
    }

    public void ChangeAreaSize(ValueAreas area, float sizeChange)
    {
        switch (area)
        {
            case ValueAreas.Area1:
                valueArea1Trans.localScale += new Vector3(sizeChange, 0, 0);
                valueArea1Trans.position += new Vector3(sizeChange / 2, 0, 0);
                valueArea0Trans.localScale -= new Vector3(sizeChange, 0, 0);
                valueArea0Trans.position += new Vector3(sizeChange, 0, 0);
                break;
            case ValueAreas.Area2:
                valueArea2Trans.localScale += new Vector3(sizeChange, 0, 0);
                valueArea2Trans.position += new Vector3(sizeChange / 2, 0, 0);
                valueArea0Trans.localScale -= new Vector3(sizeChange, 0, 0);
                valueArea0Trans.position += new Vector3(sizeChange / 2, 0, 0);
                valueArea1Trans.position += new Vector3(sizeChange, 0, 0);
                break;
            case ValueAreas.Area3:
                valueArea3Trans.localScale += new Vector3(sizeChange, 0, 0);
                valueArea3Trans.position += new Vector3(sizeChange / 2, 0, 0);
                valueArea0Trans.localScale -= new Vector3(sizeChange, 0, 0);
                valueArea0Trans.position += new Vector3(sizeChange, 0, 0);
                valueArea1Trans.position += new Vector3(sizeChange, 0, 0);
                valueArea2Trans.position += new Vector3(sizeChange, 0, 0);
                break;
            case ValueAreas.Area4:
                valueArea4Trans.localScale += new Vector3(sizeChange, 0, 0);
                valueArea4Trans.position += new Vector3(sizeChange / 2, 0, 0);
                valueArea0Trans.localScale -= new Vector3(sizeChange, 0, 0);
                valueArea0Trans.position += new Vector3(sizeChange, 0, 0);
                valueArea1Trans.position += new Vector3(sizeChange, 0, 0);
                valueArea2Trans.position += new Vector3(sizeChange, 0, 0);
                valueArea3Trans.position += new Vector3(sizeChange, 0, 0);
                break;
        }
    }
}
