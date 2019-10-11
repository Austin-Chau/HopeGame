using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [Tooltip("Current object camera is following.")]
    public GameObject Target;

    [SerializeField]
    [Tooltip("Allows camera to only follow if object moves out of a certain bound of camera.")]
    private bool ClampMovementAllowed = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!ClampMovementAllowed)
        {
            Vector3 pos = new Vector3(Target.transform.position.x, Target.transform.position.y + 2, transform.position.z);
            transform.position = pos;
        }
    }

    void FollowUnclamped()
    {

    }


}
