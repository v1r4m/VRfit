using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserPointer : MonoBehaviour
{
    public float m_defaultLength = 5.0f;

    LineRenderer m_LineRenderer;
    // Start is called before the first frame update
    void Awake()
    {
        m_LineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        float targetLength = m_defaultLength;

        RaycastHit hit = CreateRaycast(targetLength);

        Vector3 endPos = transform.position + transform.forward * targetLength;

        if (hit.collider != null)
            endPos = hit.point;


        m_LineRenderer.SetPosition(0, transform.position);
        m_LineRenderer.SetPosition(0, endPos);

    }
    private RaycastHit CreateRaycast(float length)
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.forward);
        Physics.Raycast(ray, out hit, m_defaultLength);
        return hit;
    }
}
