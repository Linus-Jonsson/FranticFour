public static class StringManager
{
    public struct Controllers
    {
        public const string Xbox = "xbox";
        public const string PS4 = "wireless controller";
        public const string Switch = "Switch"; //Todo Switch support
    }
    public struct Inputs
    {
        public const string Horizontal = "Horizontal"; //Left stick | XBOX, PS4
        public const string Vertical = "Vertical"; //Left stick | XBOX, PS4

        public const string RightHorizontal = "RightHorizontal"; //Right stick | XBOX
        public const string RightVertical = "RightVertical"; //Right stick | XBOX

        public const string RightHorizontalPS4 = "RightHorizontalPS4"; //Right stick X | PS4
        public const string RightVerticalPS4 = "RightVerticalPS4"; //Right stick Y | PS4

        public const string HorizontalKeyboard = "HorizontalKeyboard"; //a,d ←, →
        public const string VerticalKeyboard = "VerticalKeyboard"; //w,s ↑, ↓

        public const string Action1 = "Action1"; //RB | XBOX
        public const string Action1PS4 = "Action1PS4"; //R2 | PS4
        public const string Action1Keyboard = "Action1Keyboard"; //Keyboard

        public const string Jump = "Jump"; //R1 | XBOX, PS4
        public const string JumpKeboard = "JumpKeyboard"; //Keyboard
    }

    public struct ButtonInputs
    {
        public const string A_Button = "ButtonA"; //XBOX
        public const string X_ButtonPS4 = "ButtonXPS4"; //PS4
        public const string A_ButtonKeyboard = "JumpKeyboard"; //Keyboard
    }
}