using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [Header("Follow Stats")]

    [Tooltip("Current object camera is following.")]
    public GameObject Target;

    [SerializeField]
    [Tooltip("Speed of lerping")]
    private float speed = 2.0f;

    [Header("Zoom Stats")]

    [SerializeField]
    private float zoomSize = 2.5f;

    [SerializeField]
    [Tooltip("Interval of zoom speed time")]
    private float zoomTime = .01f;

    [SerializeField]
    [Tooltip("Amount zoomed per frame")]
    private float zoomAmount = 50f;

    [SerializeField]
    [Tooltip("Time to pause")]
    private float pauseTime = .5f;

    Camera cam;

    private void Start()
    {
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = new Vector3(Target.transform.position.x, Target.transform.position.y + 2, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, pos, speed * Time.deltaTime);
    }

    public void ZoomCamera()
    {
        
        StartCoroutine(ZoomCoroutine());
    }

    IEnumerator ZoomCoroutine()
    {
        yield return StartCoroutine(CoroutineUtil.WaitForRealSeconds(.05f));

        Vector3 pos = new Vector3(Target.transform.position.x, Target.transform.position.y, transform.position.z);
        transform.position = pos;

        Time.timeScale = 0.05f;
        float camOriginalSize = cam.orthographicSize;
        float zoomRate = (cam.orthographicSize - zoomSize) / zoomAmount;
        while (cam.orthographicSize > zoomSize)
        {

            cam.orthographicSize -= zoomRate;
            yield return StartCoroutine(CoroutineUtil.WaitForRealSeconds(zoomTime));
        }

        yield return StartCoroutine(CoroutineUtil.WaitForRealSeconds(pauseTime));

        while (cam.orthographicSize < camOriginalSize)
        {
            cam.orthographicSize += zoomRate;
            yield return StartCoroutine(CoroutineUtil.WaitForRealSeconds(zoomTime));
        }

        Time.timeScale = 1f;

    }
}
