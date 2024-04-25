using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardGame.Utils
{
    public class AbstractScriptableManagerBase : ScriptableObject
    {
        public virtual void Initialize()
        {
        }

        public virtual void Destroy()
        {
        }
    }
}



