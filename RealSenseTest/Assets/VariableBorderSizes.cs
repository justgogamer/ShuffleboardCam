using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BorderAreas
{
    Top,
    Bottom,
    Side
}

public class VariableBorderSizes : MonoBehaviour
{
    [Header("Border transforms")]
    public Transform sideBorderTrans;
    public Transform topBorderTrans;
    public Transform bottomBorderTrans;
    public Transform gameFieldTrans;

    public void ChangeAreaSize(BorderAreas area, float sizeChange)
    {
        switch (area)
        {
            case BorderAreas.Side:
                sideBorderTrans.localScale += new Vector3(sizeChange, 0, 0);
                sideBorderTrans.position += new Vector3(sizeChange / 2, 0, 0);
                float toGameFieldValues = sizeChange / 1125;
                gameFieldTrans.localScale -= new Vector3(toGameFieldValues, 0, 0);
                gameFieldTrans.position += new Vector3(sizeChange, 0, 0);
                break;
            case BorderAreas.Top:
                topBorderTrans.localScale += new Vector3(sizeChange, 0, 0);
                topBorderTrans.position += new Vector3(sizeChange / 2, 0, 0);
                gameFieldTrans.localScale -= new Vector3(0, sizeChange, 0);
                gameFieldTrans.position -= new Vector3(0, sizeChange, 0);
                break;
            case BorderAreas.Bottom:
                bottomBorderTrans.localScale += new Vector3(sizeChange, 0, 0);
                bottomBorderTrans.position += new Vector3(sizeChange / 2, 0, 0);
                gameFieldTrans.localScale -= new Vector3(0, sizeChange, 0);
                gameFieldTrans.position += new Vector3(0, sizeChange, 0);
                break;
        }
    }
}
