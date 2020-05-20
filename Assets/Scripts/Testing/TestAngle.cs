using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAngle : MonoBehaviour
{
    [SerializeField]
    private Transform other;

    [SerializeField]
    private float angle = 45;

    private bool inside = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnDrawGizmos()
    {
        if (inside)
            Gizmos.color = Color.red;
        else
            Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, new Vector3(Mathf.Sin(Mathf.Deg2Rad * angle), 0, Mathf.Cos(Mathf.Deg2Rad * angle)) * 4);
        Gizmos.DrawRay(transform.position, new Vector3(Mathf.Sin(Mathf.Deg2Rad * -angle), 0, Mathf.Cos(Mathf.Deg2Rad * -angle)) * 4);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 vector = other.position - transform.position;
        inside = Vector3.Angle(transform.forward, vector) > -45 && Vector3.Angle(transform.forward, vector) < 45;
    }
}
