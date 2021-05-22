using UnityEngine;

public class ImageEntry {
    public ImageEntry(Material image, NativeGallery.ImageProperties properties) {
        Image = image;
        Properties = properties;
    }
        
    public Material Image { get; }
    public NativeGallery.ImageProperties Properties { get; }
}