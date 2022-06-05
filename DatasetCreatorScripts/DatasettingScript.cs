using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class DatasettingScript : MonoBehaviour
{
    private LightPositionChanger lightChanger;
    private SceneSetter sceneSetter;
    private Camera Cam;
    private Material Mat;
    
    private RenderTexture textureToSave;

    private Dictionary<int, string[]> shaderState = Constants.shaders;
    private int counterShaderState = 0;
    private int maxShaderState = Constants.shaders.Count;
    private bool isDatasetCompleted = false;
    private string path;
    private void Awake()
    {
        path = Path.Combine(Application.dataPath, @"..\") + Constants.saveDirectory;
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        lightChanger = GetComponent<LightPositionChanger>();
        sceneSetter = GetComponent<SceneSetter>();
    }
    void Start()
    {
        textureToSave = new RenderTexture(256, 128, 16, RenderTextureFormat.ARGBHalf);
        textureToSave.Create();
        if (Cam == null)
        {
            Cam = GetComponent<Camera>();
            lightChanger.SetNextLightPosition();
            sceneSetter.SetNextScene();
            Cam.depthTextureMode = DepthTextureMode.DepthNormals;
        }
        if (Mat == null)
        {
            Mat = new Material(Shader.Find(shaderState[counterShaderState][1]));
        }
    }
    private void OnPreRender()
    {
        Shader.SetGlobalMatrix(Shader.PropertyToID("UNITY_MATRIX_IV"), Cam.cameraToWorldMatrix);
    }
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (!isDatasetCompleted)
        {
            SaveTexture(source, destination);
            if (counterShaderState == 0 || counterShaderState == 1)
            {
                lightChanger.ResetCounter(); //don't need normal and depth with different light positions
                counterShaderState++;
                return;
            }
            if (lightChanger.Finished)
            {
                lightChanger.ResetCounter();
                if (counterShaderState + 1 == maxShaderState)
                {
                    counterShaderState = 0;
                }
                else
                {
                    counterShaderState++;
                }
                if (sceneSetter.WasLastScene && !sceneSetter.TryIncreaseNumberOfObjects())
                {
                    isDatasetCompleted = true;
                    return;
                }
                else
                {
                    //Debug.Log("New scene");
                    sceneSetter.SetNextScene();
                }
            }
            else
            {
                lightChanger.SetNextLightPosition();
            }
            
        }
    }

    private void SaveTexture(RenderTexture source, RenderTexture destination)
    {
        string lightNamePostfix = counterShaderState == 0 || counterShaderState == 1 ? "" : lightChanger.CurrentName.ToString();
        string filename = sceneSetter.CurrentNumberOfObjects.ToString() + "_"
                + sceneSetter.CounterScenes + "_"
                + shaderState[counterShaderState][0] + "_"
                + lightNamePostfix;
        string shaderName = shaderState[counterShaderState][1];
        Mat = new Material(Shader.Find(shaderName));
        Graphics.Blit(source, destination, Mat);
        Graphics.Blit(null, textureToSave);
        ExportTextureToExr(textureToSave, filename);
    }
    private void ExportTextureToExr(RenderTexture textureToSave, string endName)
    {
        if (textureToSave != null)
        {
            //if (SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGBHalf)){
            int width = textureToSave.width;
            int height = textureToSave.height;

            //Debug.Log("width = " + width.ToString() + ", height = " + height.ToString()); //delete later
            Texture2D tex = new Texture2D(width, height, TextureFormat.RGBAHalf, false);

            // Read screen contents into the texture
            Graphics.SetRenderTarget(textureToSave);
            tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            tex.Apply();

            // Encode texture into the EXR
            byte[] bytes = tex.EncodeToEXR(Texture2D.EXRFlags.CompressZIP);
            File.WriteAllBytes(path + "/SavedRenderTexture" + endName + ".exr", bytes);

            DestroyImmediate(tex);
        }
    }
}

