using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    [SerializeField] public GameObject leftWall, rightWall, forwardWall, backWall;

    [SerializeField] public MeshRenderer imageF1, imageF2, imageF3, imageB1, imageB2, imageB3;

    public MeshRenderer[] GetImages() => new[] {imageF1, imageF2, imageF3, imageB1, imageB2, imageB3};
}