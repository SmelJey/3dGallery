using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    [SerializeField] public GameObject leftWall, rightWall, forwardWall, backWall;

    [SerializeField] public MeshRenderer imageF1, imageF2, imageF3, imageB1, imageB2, imageB3;

    private ImageEntry[] myImageEntries = new ImageEntry[6];
    
    public MeshRenderer[] GetImages() => new[] {imageF1, imageF2, imageF3, imageB1, imageB2, imageB3};

    public ImageEntry GetImageEntry(int idx) => myImageEntries[idx];

    public ImageEntry GetImageEntry(MeshRenderer image) {
        var images = GetImages();
        for (int i = 0; i < 6; i++) {
            if (images[i] == image) {
                return GetImageEntry(i);
            }
        }

        return null;
    }

    public void SetupImage(int idx, string path, GameObject defaultGameObject) {
        var material = new Material(Shader.Find("UI/Default"))
        {
            mainTexture = NativeGallery.LoadImageAtPath(path)
        };
        
        GetImages()[idx].material = material;
        GetImages()[idx].gameObject.tag = "Image";

        var imageProp = NativeGallery.GetImageProperties(path);
        var imageTransform = GetImages()[idx].transform;
        var localScale = imageTransform.localScale;
        localScale = new Vector3(localScale.x, 1.0f, localScale.x * imageProp.height / imageProp.width);
        imageTransform.localScale = localScale;
        imageTransform.parent = defaultGameObject.transform;

        myImageEntries[idx] = new ImageEntry(material, imageProp, localScale);
    }
}