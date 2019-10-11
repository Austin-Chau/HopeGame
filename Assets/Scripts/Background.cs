using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    public float backgroundSize;

    [SerializeField]
    [Tooltip("The speed at which the background scrolls relative to the camera")]
    float speedRatio = 1.0f;

    Transform[] images;
    Vector3 camPos;
    float viewZone = 10;
    int leftIndex;
    int rightIndex;


    // Start is called before the first frame update
    void Start()
    {
        images = new Transform[transform.childCount];
        for(int i = 0; i < transform.childCount; i++)
        {
            images[i] = transform.GetChild(i);
        }
        camPos = Camera.main.transform.position;

        leftIndex = 0;
        rightIndex = images.Length - 1;
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 newPos = Camera.main.transform.position;
        newPos = newPos - camPos;
        newPos.x = newPos.x * speedRatio;
        transform.position += newPos;
        camPos = Camera.main.transform.position;
        
        if (camPos.x < (images[leftIndex].transform.position.x + viewZone))
            ScrollLeft();
        else if (camPos.x > (images[rightIndex].transform.position.x - viewZone))
            ScrollRight();
    }

    void ScrollLeft()
    {
        int lastRight = rightIndex;
        Vector3 newPos = images[rightIndex].position;
        newPos.x = images[leftIndex].position.x - backgroundSize;
        images[rightIndex].position = newPos;

        leftIndex = rightIndex;
        rightIndex--;
        if (rightIndex < 0)
            rightIndex = images.Length - 1;
    }

    void ScrollRight()
    {
        int lastLeft = leftIndex;
        Vector3 newPos = images[leftIndex].position;
        newPos.x = images[rightIndex].position.x + backgroundSize;
        images[leftIndex].position = newPos;

        rightIndex = leftIndex;
        leftIndex++;
        if (leftIndex == images.Length)
            leftIndex = 0;
    }
}
