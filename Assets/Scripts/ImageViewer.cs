using UnityEngine;
using UnityEngine.UI;

public class ImageViewer : MonoBehaviour {
    [SerializeField] private Image image;

    private ImageEntry myCurrentImage;

    private FavouritesController myFavouritesController;
    
    private void Awake() {
        myFavouritesController = FindObjectOfType<FavouritesController>();
        Close();
    }

    public void SetImage(ImageEntry newImage) {
        myCurrentImage = newImage;
        image.material = newImage.Image;
        gameObject.SetActive(true);
    }

    public void Close() {
        gameObject.SetActive(false);
    }

    public void AddFavourite() {
        myFavouritesController.AddImage(myCurrentImage);
    }

    public bool IsShowing => gameObject.activeInHierarchy;
}
