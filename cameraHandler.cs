using UnityEngine;
using System.Collections;

public class cameraHandler : MonoBehaviour
{

    public Texture2D crosshairImage;
    public bool lockCursor = true;
    public Transform target;
    public float distance = 4f;
    public Vector3 cameraOffset;
    [Range(0.1f, 10f)] public float mouseSensitivity = 3.2f;
    public Vector2 tiltMinMax = new Vector2(-40f, 85f);
    public float zoomFOV = 45f;
    public float defaultFOV = 60f;

    public float smoothTime = 0.2f;
    Vector3 smoothTemp;
    float smoothTempf;

    float rotX;
    float rotY;
    Vector3 rot;
    float d = 2.5f;
    Camera cam;

    // Use this for initialization
    void Start()
    {
        d = distance;
        cam = GetComponent<Camera>();
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        tilt();
        zoom();
        offset();
    }

    void tilt()
    {
        rotX -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        rotY += Input.GetAxis("Mouse X") * mouseSensitivity;
        rotX = Mathf.Clamp(rotX, tiltMinMax.x, tiltMinMax.y);

        rot = Vector3.SmoothDamp(rot, new Vector3(rotX, rotY), ref smoothTemp, smoothTime);
        transform.eulerAngles = rot;
    }

    void zoom()
    {
        if (Input.GetButton("Fire2"))
        {
            cam.fieldOfView = Mathf.SmoothDamp(cam.fieldOfView, zoomFOV, ref smoothTempf, smoothTime);
        }
        else
        {
            cam.fieldOfView = Mathf.SmoothDamp(cam.fieldOfView, defaultFOV, ref smoothTempf, smoothTime);
        }
    }

    void offset()
    {
        d -= Input.GetAxis("Mouse ScrollWheel");
        d = Mathf.Clamp(d, 2.5f, distance);
        transform.position = target.position - transform.forward * d;
        Debug.Log(transform.forward);
        //transform.position = target.TransformPoint(target.localPosition+cameraOffset) - transform.forward * d;
    }

    void OnGUI()
    {
        GUI.DrawTexture(new Rect((Screen.width - crosshairImage.width) / 2f, (Screen.height - crosshairImage.height) / 2f, crosshairImage.width, crosshairImage.height), crosshairImage);
    }
}
