using System.Collections.Generic;
using UnityEngine;

public class FavouritesController : MonoBehaviour {
    private List<ImageEntry> myFavourites;
    
    public List<ImageEntry> GetImages() {
        return myFavourites;
    }
    
    public void AddImage(ImageEntry image) {
        myFavourites.Add(image);
    }
}
