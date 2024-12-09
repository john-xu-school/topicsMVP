using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public PlayerController pc;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        transform.position = Vector3.Lerp(transform.position, new Vector3(pc.playerPos.x, pc.playerPos.y, transform.position.z), Time.deltaTime);
    }
}
