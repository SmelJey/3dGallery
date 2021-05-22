using UnityEngine;
using UnityEngine.UI;

public class ImageViewer : MonoBehaviour {
    [SerializeField] private Image image;
    [SerializeField] private RectTransform infoPanel;
    [SerializeField] private Text infoLabel;
    
    private ImageEntry myCurrentImage;
    private Vector2 originalSize;

    private FavouritesController myFavouritesController;
    private bool isInfo;
    
    private void Awake() {
        myFavouritesController = FindObjectOfType<FavouritesController>();
        image.preserveAspect = true;
        isInfo = false;
        originalSize = new Vector2(image.rectTransform.rect.width, image.rectTransform.rect.height);
        Close();
    }

    public void SetImage(ImageEntry newImage) {
        myCurrentImage = newImage;
        image.material = newImage.Image;
        if (newImage.Image == null) {
            Application.Quit();
            return;
        }
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

        gameObject.SetActive(true);
    }

    public void Close() {
        isInfo = false;
        infoPanel.gameObject.SetActive(isInfo);
        gameObject.SetActive(false);
        
        image.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originalSize.x);
        image.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, originalSize.y);
    }

    public void AddFavourite() {
        myFavouritesController.AddImage(myCurrentImage);
    }

    public void SwitchInfoPanel() {
        isInfo = !isInfo;
        infoLabel.text = $"Image info \n" +
                         $"Size: {myCurrentImage.Properties.width} x {myCurrentImage.Properties.height} \n";
        infoPanel.gameObject.SetActive(isInfo);
    }

    public bool IsShowing => gameObject.activeInHierarchy;
}
