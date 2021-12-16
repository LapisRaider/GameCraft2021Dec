using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactiables : MonoBehaviour
{
    protected bool m_interactionFinish = true;

    public virtual void Start()
    {
        m_interactionFinish = true;
    }

    public virtual void Interact()
    {
        //TODO:: maybe can have some UI or something
    }

    public virtual bool InteractionFinish()
    {
        return m_interactionFinish;
    }
}
