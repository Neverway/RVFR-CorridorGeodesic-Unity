using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using static Thry.GradientEditor;
using static Thry.TexturePacker;

namespace Thry
{
    public class ThryToggleUIDrawer : ThryToggleDrawer
    {
        public ThryToggleUIDrawer()
        {
        }

        //the reason for weird string thing here is that you cant have bools as params for drawers
        public ThryToggleUIDrawer(string keywordLeft)
        {
            if (keywordLeft == "true") left = true;
            else if (keywordLeft == "false") left = false;
            else keyword = keywordLeft;
        }

        public ThryToggleUIDrawer(string keyword, string left)
        {
            this.keyword = keyword;
            this.left = left == "true";
        }
    }

}