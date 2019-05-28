using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] float damping = 1f;
    Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - player.transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 newPosition = player.transform.position + offset;
        Vector3 position = Vector3.Lerp
            (transform.position, newPosition, damping * Time.deltaTime);
        transform.position = position;

        //transform.LookAt(player.transform.position);
    }
}
