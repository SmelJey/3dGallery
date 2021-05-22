using System.Collections.Generic;
using UnityEngine;

public class FavouritesController : MonoBehaviour {
    private List<ImageEntry> myFavourites = new List<ImageEntry>();
    
    public List<ImageEntry> GetImages() {
        return myFavourites;
    }
    
    public void AddImage(ImageEntry image) {
        myFavourites.Add(image);
    }
}
