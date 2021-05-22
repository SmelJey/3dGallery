using UnityEngine;
using UnityEngine.UI;

public class ImageViewer : MonoBehaviour {
    [SerializeField] private Image myImage;

    private void Awake() {
        Close();
    }

    public void SetImage(Material newImage) {
        myImage.material = newImage;
        gameObject.SetActive(true);
    }

    public void Close() {
        gameObject.SetActive(false);
    }

    public bool IsShowing => gameObject.activeInHierarchy;
}
