using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StringManager
{
    public struct Inputs
    {
        public static readonly string horizontal = "Horizontal"; //Left stick
        public static readonly string vertical = "Vertical"; //Left stick

        public static readonly string rightHorizontal = "RightHorizontal"; //Right stick
        public static readonly string rightVertical = "RightVertical"; //Right stick

        public static readonly string rightHorizontalPS4 = "RightHorizontalPS4"; //Right stick X for ps4
        public static readonly string rightVerticalPS4 = "RightVerticalPS4"; //Right stick Y for ps4
        
        public static readonly string horizontalKeyboard = "HorizontalKeyboard"; //a,d ←, →
        public static readonly string verticalKeyboard = "VerticalKeyboard"; //w,s ↑, ↓

        public static readonly string action1 = "Action1"; //R2
        public static readonly string action1PS4 = "Action1PS4"; //R2
        public static readonly string action1Keyboard = "Action1Keyboard"; //Keyboard
        
        public static readonly string jump = "Jump"; //R1
        public static readonly string jumpKeboard = "JumpKeyboard"; //Keyboard
    }
}