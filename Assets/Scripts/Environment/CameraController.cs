using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private Camera myCamera;
    [SerializeField]
    private float zoomAmplifier = 1f;

    [SerializeField]
    private float xMin = 0f;
    [SerializeField]
    private float xMax = 30f;
    [SerializeField]
    private float yMin = 0f;
    [SerializeField]
    private float yMax = 30f;


    private void Awake()
    {
        AudioListener.volume = .2f;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        float xAxisValue = Input.GetAxis("Horizontal");
        if (xAxisValue < 0f && transform.position.x <= xMin)
        {
            xAxisValue = 0f;
        }
        else if (xAxisValue > 0f && transform.position.x >= xMax)
        {
            xAxisValue = 0f;
        }

        float zAxisValue = Input.GetAxis("Vertical");
        if (zAxisValue < 0f && transform.position.y <= yMin)
        {
            zAxisValue = 0f;
        }
        else if (zAxisValue > 0f && transform.position.y >= yMax)
        {
            zAxisValue = 0f;
        }

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
