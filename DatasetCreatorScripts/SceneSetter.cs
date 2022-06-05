using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneSetter : MonoBehaviour
{
    [SerializeField] private GameObject[] gameObjects;
    [SerializeField] private GameObject sceneContent;
    [SerializeField] private GameObject floorObject;
    private System.Random rand = new System.Random();
    private const int maxNumberOfObjects = Constants.maxNumberOfObjects;
    private const int multiplier = Constants.scenesPerObjectsKoeff;
    public int CurrentNumberOfObjects { get; private set; } = 1;
    public int CounterScenes { get; private set; } = 0;
    private int CurrentScenesLimit { get => CurrentNumberOfObjects * multiplier; }
    public bool WasLastScene { get => CounterScenes == CurrentScenesLimit; }

    public void SetNextScene()
    {
        ClearScene();
        floorObject.GetComponent<Renderer>().material.SetColor("_Color", new Color((float)rand.NextDouble(), (float)rand.NextDouble(), (float)rand.NextDouble()));
        for (int i = 0; i < CurrentNumberOfObjects; i++)
        {            
            GameObject mesh = Instantiate(gameObjects[rand.Next(0, gameObjects.Length)], sceneContent.transform);
            mesh.transform.localScale = new Vector3(
                mesh.transform.localScale.x + 0.5f * (float)rand.NextDouble(),
                mesh.transform.localScale.y + 0.5f * (float)rand.NextDouble(),
                mesh.transform.localScale.z + 0.5f * (float)rand.NextDouble());
            mesh.transform.localRotation = Quaternion.Euler(new Vector3(
                -90, //180 * (float)rand.NextDouble() - 90,
                360 * (float)rand.NextDouble(),
                0//180 * (float)rand.NextDouble() - 90
                 ));
            mesh.transform.localPosition = new Vector3(
                1.4f * (float)rand.NextDouble() - 0.7f,
                0.1f, 
                -1f * (float)rand.NextDouble());
            mesh.transform.GetChild(0).GetComponent<Renderer>().material.SetColor("_Color", new Color((float)rand.NextDouble(), (float)rand.NextDouble(), (float)rand.NextDouble()));
        }
        CounterScenes++;
    }
    private void ClearScene()
    {
        foreach (Transform child in sceneContent.transform)
        {
            Destroy(child.gameObject);
        }
    }
    public bool TryIncreaseNumberOfObjects()
    {
        if (CurrentNumberOfObjects + 1 > maxNumberOfObjects)
        {
            return false;
        }
        else
        {
            CurrentNumberOfObjects++;
            CounterScenes = 0;
            return true;
        }
    }
}
