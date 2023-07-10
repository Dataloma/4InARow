using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour
{
    [SerializeField] List<GameObject> waypoints;
    [SerializeField] float speed = 2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        int index = 0;
        Vector3 newPos = Vector3.MoveTowards(transform.position, waypoints[index].transform.position, speed*Time.deltaTime);
        transform.position = newPos;
    }
}
