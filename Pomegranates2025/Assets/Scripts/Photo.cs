using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UI;


public class Photo : MonoBehaviour
{
    public RenderTexture phoneRenderTexture;
    int frameCount = 0;

    public RawImage displayImage;
    public List<Texture2D> photos = new List<Texture2D>();
    private int currentPhotoIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if (photos.Count == 0) return;

        if (Input.GetKeyDown(KeyCode.Z))
        {
            Debug.Log("hit mouse");
            CaptureFrame();
        }

        if(Input.GetKeyDown (KeyCode.E))
        {
            Debug.Log("right arrow");
            currentPhotoIndex = (currentPhotoIndex + 1) % photos.Count;
            ShowPhoto(currentPhotoIndex);
        }
        if(Input.GetKeyDown(KeyCode.Q))
        {
            currentPhotoIndex = (currentPhotoIndex - 1) % photos.Count % photos.Count;
            ShowPhoto(currentPhotoIndex);
        }
    }

    private void CaptureFrame()
    {
        //string path = Application.dataPath + "/StreamingAssets/SavedFrames/";
        //tell unity where to go
        string path = Path.Combine(Application.streamingAssetsPath, "SavedFrames");

        //if it doesnt exist then create it
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
            Debug.Log("made parth");
        }
        else
        {
            Debug.Log("not found");
        }

        //setting our current render var to whatever is the active one
        RenderTexture currentRT = RenderTexture.active;
        RenderTexture.active = phoneRenderTexture;

        //creates a blank new image in memory (texture2D) to hold the pixel data
        //RGB24 means no alpha just RGB
        //make sure size matches the render
        Texture2D image = new Texture2D(phoneRenderTexture.width, phoneRenderTexture.height, TextureFormat.RGB24, false);
        //Reads the pixels from the active render texture into the image
        //rect (0, 0....) means read the full image
        image.ReadPixels(new Rect(0, 0, phoneRenderTexture.width, phoneRenderTexture.height), 0, 0);
        //updates the texture 2d so we can use/encode it
        image.Apply();
        //converts image into a png and stores it as byte array
        //can now be written to a file
        byte[] bytes = image.EncodeToPNG();

        //creates a fukename like frame_0000.prng, frame_0001.png\
        //D04 means pad the number 4 digits
        //combines that with the full folder path
        string filePath = Path.Combine(path, $"frame_{frameCount:D04}.png");
        //saves png byte array to disk as actual image file
        File.WriteAllBytes(filePath, bytes);

        //add to gallery
        photos.Add(image);
        currentPhotoIndex = photos.Count - 1;
        ShowPhoto(currentPhotoIndex);


        //string[] files = Directory.GetFiles(filePath, "*.png");
        //Debug.Log("save");
        //increase so next screen shot gets new name
        frameCount++;

        //restores whatever render texture unity was using before
        RenderTexture.active = currentRT;

      
    }

    void LoadPhotos()
    {
        string folderPath = Path.Combine(Application.streamingAssetsPath, "SavedPhotos");
        if (!Directory.Exists(folderPath)) return;

        string[] files = Directory.GetFiles(folderPath, "*.png");
        foreach (string file in files)
        {
            byte[] bytes = File.ReadAllBytes(file);
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(bytes);
            photos.Add(tex);
        }
    }
    
    void ShowPhoto(int index)
    {
        if(index < 0 || index >= photos.Count) return;
        displayImage.texture = photos[index];
    }
}
