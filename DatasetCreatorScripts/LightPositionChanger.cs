using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightPositionChanger : MonoBehaviour
{
    //LIGHT POSITION INFO
    // private readonly string[] positionNames = new string[] { "Center", "Right", "Left", "Up", "Down", "Directional" };
    [SerializeField] private GameObject pointLight;
    [SerializeField] private GameObject directionalLight;
    private Vector3[] lightPosition = new Vector3[] 
    {
        new Vector3(0, 0, 0.3f),
        new Vector3(1, 0, 0.3f),
        new Vector3(-1, 0, 0.3f),
        new Vector3(0, 0.4f, 0.3f),
        new Vector3(0, -0.4f, 0.3f), 
    };
    private const int maxPositions = 6;
    private int counterPosition = 0;
    public int CurrentName { get; private set; }
    public bool Finished { get => counterPosition == maxPositions; }

    public void SetNextLightPosition()
    {
        if(counterPosition == maxPositions - 1)
        {
            //last one with directional Light
            directionalLight.SetActive(true);
            pointLight.SetActive(false);
        }
        else
        {
            pointLight.transform.localPosition = lightPosition[counterPosition];
        }
        CurrentName = counterPosition; //positionNames[counterPosition];
        counterPosition++;
    }
    public void ResetCounter()
    {
        counterPosition = 0;
        pointLight.SetActive(true);
        directionalLight.SetActive(false);
    }
}
