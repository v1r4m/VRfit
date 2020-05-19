using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Valve.VR;


public class VRInputModule : BaseInputModule
{
    public Camera camera;
    public SteamVR_Input_Sources m_TargetSource;
    public SteamVR_Action_Boolean m_ClickAction;

    private GameObject m_currentObject = null;
    private PointerEventData m_Data = null;
    protected override void Awake()
    {
        base.Awake();

        m_Data = new PointerEventData(eventSystem);
    }

    public override void Process()
    {
        m_Data.Reset();
        m_Data.position = new Vector2(camera.pixelWidth / 2, camera.pixelHeight / 2);

        eventSystem.RaycastAll(m_Data, m_RaycastResultCache);
        m_Data.pointerCurrentRaycast = FindFirstRaycast(m_RaycastResultCache);
        m_currentObject = m_Data.pointerCurrentRaycast.gameObject;

        m_RaycastResultCache.Clear();

        HandlePointerExitAndEnter(m_Data, m_currentObject);


        if (m_ClickAction.GetLastStateDown(m_TargetSource))
            ProcessPress(m_Data);

        if (m_ClickAction.GetLastStateUp(m_TargetSource))
            ProcessRelease(m_Data);
    }


    public PointerEventData GetData()
    {
        return m_Data;
    }
    private void ProcessPress(PointerEventData data)
    {

    }
    private void ProcessRelease(PointerEventData data)
    {

    }

}