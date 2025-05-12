using Neverway.Framework.LogicValueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextObjectHolder : MonoBehaviour
{
    public LogicOutput<string> heldText = new("");

    public TextObject heldObject;
    public TextObject cooldownObject;

    public void Update()
    {
        if (heldObject is not null)
        {
            if (heldObject.GetComponent<Object_Grabbable>().isHeld || 
                (heldObject.transform.position - transform.position).magnitude > 0.3f)
            {
                heldObject.GetComponent<Rigidbody>().isKinematic = false;
                
                cooldownObject = heldObject;
                heldObject = null;
                heldText.Set("");
            }
            return;
        }

        TextObject[] objs = FindObjectsByType<TextObject>(FindObjectsSortMode.InstanceID);

        foreach(TextObject textObj in objs)
        {
            Object_Grabbable grabbable = textObj.GetComponent<Object_Grabbable>();
            if ((textObj.transform.position - transform.position).magnitude < 1f &&
                grabbable != null && !grabbable.isHeld)
            {
                if (textObj == cooldownObject)
                    continue;

                heldObject = textObj;
                heldText.Set(heldObject.text);

                textObj.GetComponent<Rigidbody>().isKinematic = true;
                textObj.transform.position = transform.position;
            }
            else if (textObj == cooldownObject)
                cooldownObject = null;
        }
    }
}
