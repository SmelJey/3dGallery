using System;
using UnityEngine;
using UnityEngine.UI;

public class ImageViewer : MonoBehaviour {
    [SerializeField] private Image image;

    private ImageEntry myCurrentImage;
    private Vector2 originalSize;

    private FavouritesController myFavouritesController;
    
    private void Awake() {
        myFavouritesController = FindObjectOfType<FavouritesController>();
        Close();
    }

    private void Start() {
        originalSize = image.transform.localScale;
    }

    public void SetImage(ImageEntry newImage) {
        myCurrentImage = newImage;
        image.material = newImage.Image;
        image.transform.localScale = newImage.Size / Mathf.Max(newImage.Size.x, newImage.Size.z);
        gameObject.SetActive(true);
    }

    public void Close() {
        gameObject.SetActive(false);
        image.transform.localScale = originalSize;
    }

    public void AddFavourite() {
        myFavouritesController.AddImage(myCurrentImage);
    }

    public bool IsShowing => gameObject.activeInHierarchy;
}
