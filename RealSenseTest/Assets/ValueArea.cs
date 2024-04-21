using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PointArea
{
    One,
    Two,
    Three,
    Four
}

public class ValueArea : MonoBehaviour
{
    public DataCollector collector;
    public PointArea area;
    [SerializeField] private float currentPointX;
    [SerializeField] private float nextPointX;
    public GameObject SquareLeft;
    public GameObject SquareRight;
    private RectTransform footageArea;
    private float height;
    private float width;

    [Header("PointArea Colors")]
    public Material area1Mat;
    public Material area2Mat;
    public Material area3Mat;
    public Material area4Mat;

    private void Start()
    {
        width = collector.screenWidth;
        height = collector.screenHeight;
        Debug.Log($"Footage height: {height}");
        
        switch (area)
        {
            case PointArea.One:
                currentPointX = collector.pointArea1;
                nextPointX = collector.pointArea2;
                break;
            case PointArea.Two:
                currentPointX = collector.pointArea2;
                nextPointX = collector.pointArea3;
                break;
            case PointArea.Three:
                currentPointX = collector.pointArea3;
                nextPointX = collector.pointArea4;
                break;
            case PointArea.Four:
                currentPointX = collector.pointArea4;
                nextPointX = 0;
                break;
        }
    }

    void Update()
    {
    
    }
}
