using UnityEngine;

public class ImageEntry {
    public ImageEntry(Material image, Vector2 size) {
        Image = image;
        Size = size;
    }
        
    public Material Image { get; }
    public Vector2 Size { get; }
}