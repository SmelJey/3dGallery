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

    public void SetupImage(int idx, string path) {
        var material = new Material(Shader.Find("UI/Default"))
        {
            mainTexture = NativeGallery.LoadImageAtPath(path)
        };
        
        GetImages()[idx % 6].material = material;
        GetImages()[idx % 6].gameObject.tag = "Image";

        var imageProp = NativeGallery.GetImageProperties(path);
        var imageTransform = GetImages()[idx % 6].transform;
        var localScale = imageTransform.localScale;
        localScale = new Vector3(localScale.x, 1.0f, localScale.x * imageProp.height / imageProp.width);
        imageTransform.localScale = localScale;

        myImageEntries[idx % 6] = new ImageEntry(idx, material, imageProp, localScale);
    }

    public void SetupImage(int idx, ImageEntry image) {
        GetImages()[idx % 6].material = image.Image;
        GetImages()[idx % 6].gameObject.tag = "Image";

        var imageTransform = GetImages()[idx % 6].transform;
        var localScale = imageTransform.localScale;
        localScale = new Vector3(localScale.x, 1.0f, localScale.x * image.Properties.height / image.Properties.width);
        imageTransform.localScale = localScale;

        myImageEntries[idx % 6] = new ImageEntry(image.Id, image.Image, image.Properties, localScale);
    }
    
    public void SetupEmpty(int idx) {
        var material = new Material(Shader.Find("UI/Default"));
        GetImages()[idx % 6].gameObject.tag = "Image";

        var prop = new NativeGallery.ImageProperties(100, 100, "RGB24", NativeGallery.ImageOrientation.Normal);
        myImageEntries[idx % 6] = new ImageEntry(idx, material, prop, Vector3.one);
    }
}