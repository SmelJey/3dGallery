using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldController : MonoBehaviour
{
    [SerializeField] private GameObject roomPrefab;

    private RoomController _oldRoom;
    private RoomController _currentRoom;

    [SerializeField] private GameObject defaultGameObject;
    private GameObject favoriteGameObject;
    private FavouritesController favouritesController;

    void Start()
    {
        favouritesController = FindObjectOfType<FavouritesController>();

        NativeGallery.GetImagesFromGallery(paths =>
        {
            if (paths == null || paths.Length == 0)
            {
                _currentRoom = Instantiate(roomPrefab).GetComponent<RoomController>();
                return;
            }

            for (var i = 0; i < paths.Length; i++)
            {
                if (i % 6 == 0)
                {
                    _oldRoom = _currentRoom;
                    _currentRoom = Instantiate(roomPrefab).GetComponent<RoomController>();
                    if (_oldRoom != null)
                    {
                        _oldRoom.rightWall.SetActive(false);
                        _currentRoom.transform.position = _oldRoom.transform.position + Vector3.forward * 10;
                        _currentRoom.leftWall.SetActive(false);
                    }
                }

                var material = new Material(Shader.Find("UI/Default"))
                {
                    mainTexture = NativeGallery.LoadImageAtPath(paths[i])
                };
                _currentRoom.GetImages()[i % 6].material = material;
                _currentRoom.GetImages()[i % 6].gameObject.tag = "Image";
                var imageProp = NativeGallery.GetImageProperties(paths[i]);
                var imageTransform = _currentRoom.GetImages()[i % 6].transform;
                var localScale = imageTransform.localScale;
                imageTransform.parent = defaultGameObject.transform;
                localScale = new Vector3(localScale.x, 1.0f, localScale.x * imageProp.height / imageProp.width);
                imageTransform.localScale = localScale;
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
            return;
        }

        for (var i = 0; i < imageEntries.Count; i++)
        {
            if (i % 6 == 0)
            {
                _oldRoom = _currentRoom;
                _currentRoom = Instantiate(roomPrefab).GetComponent<RoomController>();
                if (_oldRoom != null)
                {
                    _oldRoom.rightWall.SetActive(false);
                    _currentRoom.transform.position = _oldRoom.transform.position + Vector3.forward * 10;
                    _currentRoom.leftWall.SetActive(false);
                }
            }

            _currentRoom.GetImages()[i % 6].material = imageEntries[i].Image;
            _currentRoom.GetImages()[i % 6].gameObject.tag = "Image";
            _currentRoom.GetImages()[i % 6].transform.localScale = imageEntries[i].Size;
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

    void Update()
    {
    }
}