using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default
{
    public class UseableTrigger : MonoBehaviour, Useable
    {
        void Useable.Use()
        {
            transform.parent.SendMessage("Use");
        }

        void Useable.LookingAt()
        {
            transform.parent.SendMessage("LookingAt");
        }
    }
}
