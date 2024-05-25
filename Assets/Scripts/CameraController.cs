using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private Camera myCamera;
    [SerializeField]
    private float zoomAmplifier = 1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        float xAxisValue = Input.GetAxis("Horizontal");
        float zAxisValue = Input.GetAxis("Vertical");
        transform.Translate(new Vector3(xAxisValue * Time.deltaTime * speed,zAxisValue * Time.deltaTime * speed,0f));

        float mouseScroll = Input.mouseScrollDelta.y * -1;
        mouseScroll *= zoomAmplifier;

        if (myCamera.orthographicSize >= 5f && mouseScroll < 0f)
        {
            myCamera.orthographicSize += mouseScroll;
        }
        else if (myCamera.orthographicSize <= 12f && mouseScroll > 0f)
        {
            myCamera.orthographicSize += mouseScroll;
        }

    }
}
