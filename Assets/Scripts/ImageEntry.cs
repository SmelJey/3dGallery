using UnityEngine;

public class ImageEntry {
    public ImageEntry(Material image, Vector3 size) {
        Image = image;
        Size = size;
    }
        
    public Material Image { get; }
    public Vector3 Size { get; }
}