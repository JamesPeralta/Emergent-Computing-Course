using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;

    private Vector3 offset;
    private int pos;
    private Vector3[] cameraAngles = {new Vector3(0, 60, -225), new Vector3(0, 60, 225), new Vector3(225, 60, 0), new Vector3(-225, 60, 0)};

    // Start is called before the first frame update
    void Start()
    {
        pos = 0;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        this.gameObject.transform.position = cameraAngles[pos % 4];
        if (Input.GetKeyDown("space"))
        {
            pos += 1;
        }

        Vector3 target = LocateGoldenSnitch();
        transform.LookAt(target);
    }

    // AI Functions
    Vector3 LocateGoldenSnitch()
    {
        var pos = GameObject.Find("Golden-Snitch").transform.position;

        return pos;
    }
}
