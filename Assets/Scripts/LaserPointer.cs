using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class LaserPointer : MonoBehaviour
{
    public float m_defaultLength = 5.0f;

    public SteamVR_Input_Sources m_TargetSource;
    public SteamVR_Action_Boolean m_ClickAction;

    LineRenderer m_LineRenderer;
    // Start is called before the first frame update
    void Awake()
    {
        m_LineRenderer = GetComponent<LineRenderer>();
    }
    float coolDown = 0;
    // Update is called once per frame
    void Update()
    {
        float targetLength = m_defaultLength;

        RaycastHit hit = CreateRaycast(targetLength);

        Vector3 endPos = transform.position + transform.forward * targetLength;

        if (hit.collider != null)
            endPos = hit.point;

        coolDown -= Time.deltaTime;
        m_LineRenderer.enabled = coolDown > 0;

        m_LineRenderer.SetPosition(0, transform.position);
        m_LineRenderer.SetPosition(1, endPos);



        if (m_ClickAction.GetLastStateDown(m_TargetSource))
        {
            
            ButtonFunctions bf;

            if (coolDown > 0)
                if (hit.collider != null)
                {
                    Debug.Log("Click Hit " + hit.collider.gameObject.name);
                    if ((bf = hit.collider.GetComponent<ButtonFunctions>()) != null)
                    {
                        Debug.Log("Click Sent " + hit.collider.gameObject.name);
                        bf.OnClick();
                    }
                    
                }
                    

            coolDown = 5;
        }

        if (m_ClickAction.GetLastStateUp(m_TargetSource))
        {
            //ProcessRelease(m_Data);
        }

    }
    private RaycastHit CreateRaycast(float length)
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.forward);
        Debug.DrawRay(transform.position, transform.forward);
        Physics.Raycast(ray, out hit, m_defaultLength);
        return hit;
    }
}
