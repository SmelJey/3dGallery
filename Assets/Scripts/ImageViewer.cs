using UnityEngine;
using UnityEngine.UI;

public class ImageViewer : MonoBehaviour {
    [SerializeField] private Image image;
    [SerializeField] private RectTransform infoPanel;
    [SerializeField] private Text infoLabel;
    [SerializeField] private Text favBtnLabel;
    
    private ImageEntry myCurrentImage;
    private Vector2 originalSize;

    private FavouritesController myFavouritesController;
    private bool isInfo;
    private Vector3 initialPosition;
    
    private void Awake() {
        myFavouritesController = FindObjectOfType<FavouritesController>();
        image.preserveAspect = true;
        isInfo = false;
        originalSize = new Vector2(image.rectTransform.rect.width, image.rectTransform.rect.height);
        initialPosition = image.transform.position;
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

        favBtnLabel.text = myFavouritesController.IsFav(newImage) ? "Unfav" : "Fav";
        
        gameObject.SetActive(true);
    }

    public void Close() {
        isInfo = false;
        infoPanel.gameObject.SetActive(isInfo);
        gameObject.SetActive(false);
        image.transform.localScale = new Vector3(1, 1, 1);
        image.transform.SetPositionAndRotation(initialPosition, Quaternion.identity);
        
        image.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originalSize.x);
        image.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, originalSize.y);
    }

    public void AddFavourite() {
        if (myFavouritesController.IsFav(myCurrentImage)) {
            myFavouritesController.RemoveImage(myCurrentImage);
            favBtnLabel.text = "Fav";
        } else {
            myFavouritesController.AddImage(myCurrentImage);
            favBtnLabel.text = "Unfav";
        }
        
    }

    public void SwitchInfoPanel() {
        isInfo = !isInfo;
        infoLabel.text = $"Image info \n" +
                         $"Size: {myCurrentImage.Properties.width} x {myCurrentImage.Properties.height} \n";
        infoPanel.gameObject.SetActive(isInfo);
    }

    public Vector3 Scale {
        get {
            return image.transform.localScale;
        }
        set {
            if (!IsShowing) {
                return;
            }

            image.transform.localScale = value;
        }
    }

    public void Translate(Vector3 delta, Quaternion rotation) {
        image.rectTransform.SetPositionAndRotation(image.transform.position + delta, rotation);
    }

    public bool IsShowing => gameObject.activeInHierarchy;
}
