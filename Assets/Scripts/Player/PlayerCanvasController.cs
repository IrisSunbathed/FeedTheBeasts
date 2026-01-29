using FeedTheBeasts.Scripts;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class PlayerCanvasController : MonoBehaviour
{
    RectTransform rectTransform;

    Transform traPlayer;

    CamerasManager camerasManager;
    float orthographicSize;
    float lengthCam;
    float boundsX;
    float boundsZ;
    float boundsXSign;
    Vector3 bounds;

    void Start()
    {
        camerasManager = CamerasManager.Instance;
        orthographicSize = camerasManager.OrthographicSize;
        lengthCam = camerasManager.GetCameraLength();
    }

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        traPlayer = GetComponentInParent<Transform>();

        boundsZ = rectTransform.sizeDelta.y / 2 + traPlayer.position.z;
        boundsX = rectTransform.sizeDelta.x / 2 + traPlayer.position.x;
        bounds = new Vector3(boundsX, 0, boundsZ);

        // CheckOutOfBoundsX();

        // Debug.Log($"boundsX: {boundsX}");
        // Debug.Log($"boundsY: {boundsZ}");
        // Debug.Log($"traPlayer.position.x: {traPlayer.position.x}");
        // Debug.Log($"traPlayer.position.z: {traPlayer.position.z}");
        
        
        
    }

    void Update()
    {
        var values = camerasManager.IsOutOfBounds(traPlayer.position, bounds);
        Debug.Log($"values.Item1: {values.Item1}"); //Okay
        Debug.Log($"values.Item2: {values.Item2}"); //Okay
        Debug.Log($"values.Item3: {values.Item3}");
        Debug.Log($"values.Item4: {values.Item4}");
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void CheckOutOfBoundsZ()
    {

        if (transform.position.z < -orthographicSize
            | transform.position.z > orthographicSize)
        {
            transform.position = new Vector3(transform.position.x
                                           , transform.position.y, orthographicSize * Mathf.Sign(transform.position.z));
        }
    }

    private void CheckOutOfBoundsX()
    {
        boundsXSign = boundsX * Mathf.Sign(transform.position.x);
        if (transform.position.x < -lengthCam + boundsXSign
            | transform.position.x > lengthCam + boundsXSign)
        {
            transform.position = new Vector3(lengthCam
                                             * Mathf.Sign(transform.position.x) + boundsXSign
                                            , transform.position.y, transform.position.z);
        }
    }
}
