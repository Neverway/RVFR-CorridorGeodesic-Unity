//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose: Data classes for conversations in a textbox
// Notes:
//
//=============================================================================

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Neverway.Framework.LogicSystem
{
    [Serializable]
    public class Dialogue
    {
        public string name;
        [TextArea] public string text;
        public float textSpeed=1f;
        public float endDelay=1f;
    }

    [Serializable]
    public class DialogueEvent
    {
        public List<Dialogue> dialogue;
    }
}