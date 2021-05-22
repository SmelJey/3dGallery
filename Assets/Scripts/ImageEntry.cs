using UnityEngine;

public class ImageEntry {
    public ImageEntry(int id, Material image, NativeGallery.ImageProperties properties, Vector3 localScale) {
        Image = image;
        Properties = properties;
        Id = id;

        LocalScale = new Vector3(localScale.x, 1.0f, localScale.x * properties.height / properties.width);
    }

    public int Id { get; }
    public Material Image { get; }
    public NativeGallery.ImageProperties Properties { get; }
    public Vector2 LocalScale { get; }
}