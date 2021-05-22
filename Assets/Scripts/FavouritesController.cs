using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FavouritesController : MonoBehaviour {
    private List<ImageEntry> myFavourites = new List<ImageEntry>();
    private HashSet<int> myFavouritesSet = new HashSet<int>();
    
    public List<ImageEntry> GetImages() {
        return myFavourites;
    }
    
    public void AddImage(ImageEntry image) {
        if (myFavouritesSet.Contains(image.Id)) {
            return;
        }
        myFavourites.Add(image);
        myFavouritesSet.Add(image.Id);
    }

    public void RemoveImage(ImageEntry image) {
        foreach (var img in myFavourites.ToList()) {
            if (img.Id == image.Id) {
                myFavourites.Remove(img);
                myFavouritesSet.Remove(image.Id);
                break;
            }
        }
    }

    public bool IsFav(ImageEntry image) {
        return myFavouritesSet.Contains(image.Id);
    }
}
