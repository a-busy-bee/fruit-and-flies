using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Object_Manager_Immune : Object_Manager_Base
{
    [SerializeField] private Object_Info_Immune objectInfo;

    override public void OnPointerClick(PointerEventData data)
    {
        AudioManager.instance.PlaySound(objectInfo.soundTypeDefault, true);
    }
}
