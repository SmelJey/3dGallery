using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldController : MonoBehaviour
{
    [SerializeField] private GameObject roomPrefab;

    private RoomController _oldRoom;
    private RoomController _currentRoom;
    private PlayerController myPlayerController;

    [SerializeField] private GameObject defaultGameObject;
    private GameObject favoriteGameObject;
    private FavouritesController favouritesController;

    void Start() {
        myPlayerController = FindObjectOfType<PlayerController>();
        favouritesController = FindObjectOfType<FavouritesController>();

        NativeGallery.GetImagesFromGallery(paths =>
        {
            if (paths == null || paths.Length == 0)
            {
                _currentRoom = Instantiate(roomPrefab).GetComponent<RoomController>();
                for (int i = 0; i < 6; i++) {
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
                
                _currentRoom.SetupImage(i % 6, paths[i]);
            }
        });

        _oldRoom = null;
        _currentRoom = null;
    }

    public void ToFavorite()
    {
        defaultGameObject.SetActive(false);
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

            _currentRoom.GetImages()[i % 6].material = imageEntries[i].Image;
            _currentRoom.GetImages()[i % 6].gameObject.tag = "Image";

            _currentRoom.GetImages()[i % 6].transform.localScale = imageEntries[i].LocalScale;
        }

        _oldRoom = null;
        _currentRoom = null;
    }

    public void ToDefault()
    {
        favoriteGameObject.SetActive(false);
        defaultGameObject.SetActive(true);
        Destroy(favoriteGameObject);
    }

    public void SwitchMode() {
        myPlayerController.Teleport(new Vector3(0, 2, 0));
        if (defaultGameObject.activeInHierarchy) {
            ToFavorite();
        } else {
            ToDefault();
        }
    }
}