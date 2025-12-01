using NUnit.Framework;
using UnityEngine;

public class GameObjectToCanvas : MonoBehaviour
{

    [SerializeField] RectTransform rectTransform;
    Vector3 newPosition;
    Camera cam;
    // Update is called once per frame

    void Awake()
    {
        Assert.IsNotNull(rectTransform, "ERROR: RectTransform is not added");

        cam = Camera.main;
    }
    void Update()
    {
        newPosition = cam.ScreenToWorldPoint(rectTransform.position);
        transform.position = newPosition;
    }
}
