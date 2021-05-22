using UnityEngine;
using UnityEngine.UI;

public class ImageViewer : MonoBehaviour {
    [SerializeField] private Image image;
    [SerializeField] private Text textLabel;
    
    private ImageEntry myCurrentImage;
    private Vector2 originalSize;

    private FavouritesController myFavouritesController;
    
    private void Awake() {
        myFavouritesController = FindObjectOfType<FavouritesController>();
        image.preserveAspect = true;
        //Close();
    }
    
    private int[] scaleResolution(int width, int heigth, int maxWidth, int maxHeight)
    {
        int new_width = width;
        int new_height = heigth;
 
        if (width > heigth){
            new_width = maxWidth;
            new_height = (new_width * heigth) / width;
        }
        else
        {
            new_height = maxHeight;
            new_width = (new_height * width) / heigth;
        }
 
        int[] dimension = { new_width, new_height };
        return dimension;
    }
    
    private static Vector2 SizeToParent(RawImage image, float padding = 0) {
        float w = 0, h = 0;
        var parent = image.transform.parent.GetComponent<RectTransform>();
        var imageTransform = image.GetComponent<RectTransform>();
 
        // check if there is something to do
        if (image.texture != null) {
            if (!parent) { return imageTransform.sizeDelta; } //if we don't have a parent, just return our current width;
            padding = 1 - padding;
            float ratio = image.texture.width / (float)image.texture.height;
            var bounds = new Rect(0, 0, parent.rect.width, parent.rect.height);
            if (Mathf.RoundToInt(imageTransform.eulerAngles.z) % 180 == 90) {
                //Invert the bounds if the image is rotated
                bounds.size = new Vector2(bounds.height, bounds.width);
            }
            //Size by height first
            h = bounds.height * padding;
            w = h * ratio;
            if (w > bounds.width * padding) { //If it doesn't fit, fallback to width;
                w = bounds.width * padding;
                h = w / ratio;
            }
        }
        imageTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, w);
        imageTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, h);
        return imageTransform.sizeDelta;
    }
    

    private void Start() {
        originalSize = new Vector2(image.rectTransform.rect.width, image.rectTransform.rect.height);
    }

    public void SetImage(ImageEntry newImage) {
        myCurrentImage = newImage;
        image.material = newImage.Image;
        image.preserveAspect = true;

        float aspectX = newImage.Properties.width / originalSize.x;
        float aspectY = newImage.Properties.height / originalSize.y;

        Vector2 normalizedSize = new Vector2(aspectX, aspectY) / aspectY;
        
        if (aspectX > aspectY) {
            normalizedSize = new Vector2(aspectX, aspectY) / aspectX;
        }

        image.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originalSize.x * normalizedSize.x);
        image.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, originalSize.y * normalizedSize.y);
        image.rectTransform.ForceUpdateRectTransforms();

        textLabel.text = $"{normalizedSize}, {originalSize}";
        
        // image.rectTransform.sizeDelta =
        //     new Vector2(originalSize.x * normalizedSize.x, originalSize.y * normalizedSize.z);
        // textLabel.text = $"{normalizedSize} / {originalSize} / {image.rectTransform.sizeDelta}";
        gameObject.SetActive(true);
    }

    public void Close() {
        gameObject.SetActive(false);
        image.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originalSize.x);
        image.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, originalSize.y);
    }

    public void AddFavourite() {
        myFavouritesController.AddImage(myCurrentImage);
    }

    public bool IsShowing => gameObject.activeInHierarchy;
}
