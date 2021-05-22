using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldController : MonoBehaviour
{
    [SerializeField] private GameObject roomPrefab;
    [SerializeField] private Text favBtnText;

    private RoomController _oldRoom;
    private RoomController _currentRoom;
    private PlayerController myPlayerController;

    [SerializeField] private GameObject defaultGameObject;
    private GameObject favoriteGameObject;
    private FavouritesController favouritesController;
    private bool isFav = false;

    void Start() {
        myPlayerController = FindObjectOfType<PlayerController>();
        favouritesController = FindObjectOfType<FavouritesController>();

        NativeGallery.GetImagesFromGallery(paths =>
        {
            if (paths == null || paths.Length == 0)
            {
                for (int i = 0; i < 12; i++) {
                    if (i % 6 == 0) {
                        _oldRoom = _currentRoom;
                        _currentRoom = Instantiate(roomPrefab).GetComponent<RoomController>();
                        _currentRoom.transform.SetParent(defaultGameObject.transform);
                        if (_oldRoom != null) {
                            _oldRoom.rightWall.SetActive(false);
                            _currentRoom.transform.position = _oldRoom.transform.position + Vector3.forward * 10;
                            _currentRoom.leftWall.SetActive(false);
                        }
                    }

                    _currentRoom.SetupEmpty(i);
                }
                // _currentRoom.transform.parent = defaultGameObject.transform;
                _currentRoom.transform.SetParent(defaultGameObject.transform);
                return;
            }

            for (var i = 0; i < paths.Length; i++)
            {
                if (i % 6 == 0)
                {
                    _oldRoom = _currentRoom;
                    _currentRoom = Instantiate(roomPrefab).GetComponent<RoomController>();
                    _currentRoom.transform.SetParent(defaultGameObject.transform);
                    if (_oldRoom != null)
                    {
                        _oldRoom.rightWall.SetActive(false);
                        _currentRoom.transform.position = _oldRoom.transform.position + Vector3.forward * 10;
                        _currentRoom.leftWall.SetActive(false);
                    }
                }
                
                _currentRoom.SetupImage(i, paths[i]);
            }
        });

        _oldRoom = null;
        _currentRoom = null;
    }

    public void ToFavorite() {
        isFav = true;
        _currentRoom = null;
        favoriteGameObject = new GameObject("Favorite");

        var imageEntries = favouritesController.GetImages();
        if (imageEntries == null || imageEntries.Count == 0)
        {
            _currentRoom = Instantiate(roomPrefab).GetComponent<RoomController>();
            _currentRoom.transform.SetParent(favoriteGameObject.transform);
            return;
        }

        for (var i = 0; i < imageEntries.Count; i++)
        {
            if (i % 6 == 0)
            {
                _oldRoom = _currentRoom;
                _currentRoom = Instantiate(roomPrefab).GetComponent<RoomController>();
                _currentRoom.transform.SetParent(favoriteGameObject.transform);
                if (_oldRoom != null)
                {
                    _oldRoom.rightWall.SetActive(false);
                    _currentRoom.transform.position = _oldRoom.transform.position + Vector3.forward * 10;
                    _currentRoom.leftWall.SetActive(false);
                }
            }

            _currentRoom.SetupImage(i, imageEntries[i]);
        }

        _oldRoom = null;
        _currentRoom = null;
        defaultGameObject.SetActive(false);
    }

    public void ToDefault() {
        isFav = false;
        defaultGameObject.SetActive(true);
        favoriteGameObject.SetActive(false);
        
        Destroy(favoriteGameObject);
    }

    public void SwitchMode() {
        myPlayerController.Teleport(new Vector3(0, 2, 0));
        if (!isFav) {
            ToFavorite();
            favBtnText.text = "All";
        } else {
            ToDefault();
            favBtnText.text = "Fav";
        }
        myPlayerController.Teleport(new Vector3(0, 2, 0));
    }
}