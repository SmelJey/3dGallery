using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    [SerializeField] public GameObject leftWall, rightWall, forwardWall, backWall;

    [SerializeField] public MeshRenderer imageF1, imageF2, imageF3, imageB1, imageB2, imageB3;

    private NativeGallery.ImageProperties[] myImageProps = new NativeGallery.ImageProperties[6];
    
    public MeshRenderer[] GetImages() => new[] {imageF1, imageF2, imageF3, imageB1, imageB2, imageB3};
    public NativeGallery.ImageProperties GetImageProp(int idx) => myImageProps[idx];

    public NativeGallery.ImageProperties GetImageProp(MeshRenderer image) {
        var images = GetImages();
        for (int i = 0; i < 6; i++) {
            if (images[i] == image) {
                return GetImageProp(i);
            }
        }

        return new NativeGallery.ImageProperties();
    }

    public void SetupImage(int idx, string path) {
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

        myImageProps[idx] = imageProp;
    }
}