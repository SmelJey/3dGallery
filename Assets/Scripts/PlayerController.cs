using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour {
    [SerializeField] private Joystick moveJoystick;
    [SerializeField] private Joystick sightJoystick;

    [SerializeField] private float movespeed;
    [SerializeField] private Vector2 cameraSpeed;

    [SerializeField] private ImageViewer imageViewer;

    private CharacterController myCharacterController;
    private Camera myCamera;
    private bool isAfterFixed = false;
    private Material myDefaultMaterial;
    private float initialFingersDistance;
    private Vector3 initialScale;

    private void Awake() {
        myCharacterController = GetComponent<CharacterController>();
        myCamera = GetComponentInChildren<Camera>();
        var primitive = GameObject.CreatePrimitive(PrimitiveType.Plane);
        primitive.SetActive(false);
        myDefaultMaterial = primitive.GetComponent<MeshRenderer>().material;
    }

    public void Teleport(Vector3 position) {
        myCharacterController.enabled = false;
        transform.position = position;
        myCharacterController.enabled = true;
    }

    private void Update() {
        var movement = Vector3.zero;
        movement += transform.right * moveJoystick.Horizontal;
        movement += transform.forward * moveJoystick.Vertical;
        Move(movement.normalized);

        if (Input.GetMouseButtonDown(0)) {
            if (EventSystem.current.IsPointerOverGameObject() || imageViewer.IsShowing) {
                return;
            }

            var ray = myCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Physics.Raycast(ray, out hit);

            if (hit.collider == null || !hit.collider.CompareTag("Image")) {
                return;
            }

            var meshRenderer = hit.collider.GetComponent<MeshRenderer>();
            if (meshRenderer.material != myDefaultMaterial) {
                imageViewer.SetImage(hit.collider.GetComponentInParent<RoomController>().GetImageEntry(meshRenderer));
            }
        }

        Touch t1 = Input.touches[0];
        Touch t2 = Input.touches[1];

        if (t1.phase == TouchPhase.Began || t2.phase == TouchPhase.Began) {
            initialFingersDistance = Vector2.Distance(t1.position, t2.position);
            initialScale = imageViewer.Scale;
        } else if (t1.phase == TouchPhase.Moved || t2.phase == TouchPhase.Moved) {
            var currentFingersDistance = Vector2.Distance(t1.position, t2.position);
            var scaleFactor = currentFingersDistance / initialFingersDistance;
            imageViewer.Scale = initialScale * scaleFactor;

            float Dx = t1.position.x - transform.position.x;
            float Dy = t1.position.y - transform.position.y;


            //var cameraTransform = Camera.main.transform.InverseTransformPoint(0, 0, 0);
            //transform.position = Camera.main.ScreenToWorldPoint(new Vector3(t2.position.x, t2.position.y, cameraTransform.z -0.5f));


         //    Vector3 pos = t2.position;
         //
         //    //Vector3 pos = t1.position - t2.position;
         //    Vector3 touchedPos = Camera.main.ScreenToWorldPoint(new Vector3(pos.x, pos.y, 10));
         //    transform.position = Vector3.Lerp(transform.position, touchedPos, Time.deltaTime * 4);
         // //   transform.position += touchedPos;
         //
         //    float pinchAmount = 0;
         //    Quaternion desiredRotation = transform.rotation;
         //
         //    TouchLogic.Calculate();
         //
         //    if (Mathf.Abs(TouchLogic.pinchDistanceDelta) > 0)
         //    { // zoom
         //        pinchAmount = TouchLogic.pinchDistanceDelta;
         //    }
         //
         //    if (Mathf.Abs(TouchLogic.turnAngleDelta) > 0)
         //    { // rotate
         //        Vector3 rotationDeg = Vector3.zero;
         //        rotationDeg.z = -TouchLogic.turnAngleDelta;
         //        desiredRotation *= Quaternion.Euler(rotationDeg);
         //    }
         //
         //    // not so sure those will work:
         //    transform.rotation = desiredRotation;
         //    transform.position += Vector3.forward * pinchAmount;
        }
    }

    private void Move(Vector3 direction) {
        if (myCharacterController.isGrounded) {
            direction = direction.normalized;
            myCharacterController.SimpleMove(new Vector3(direction.x, 0, direction.z) * movespeed);
        } else {
            myCharacterController.SimpleMove(new Vector3(0, Physics.gravity.y, 0) * movespeed);
        }
       
    }

    private void FixedUpdate() {
        isAfterFixed = true;
    }

    void LateUpdate()
    {
        if (!isAfterFixed) return;
        isAfterFixed = false;

        #region Camera movement

        var oldCameraRotation = myCamera.transform.rotation;

        myCamera.transform.Rotate(Vector3.left, sightJoystick.Vertical * cameraSpeed.y);

        var cameraAngleX = Vector3.SignedAngle(Vector3.up, myCamera.transform.forward, myCamera.transform.right);
        if (cameraAngleX < 0.0f)
        {
            myCamera.transform.rotation = oldCameraRotation;
        }

        transform.Rotate(transform.up, sightJoystick.Horizontal * cameraSpeed.x);

        #endregion Camera movement
    }
}
