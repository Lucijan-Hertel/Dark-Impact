using System;
using Microsoft.Toolkit.HighPerformance;
using System.Numerics;
using static Raylib_CsLo.Raylib;
using Raylib_CsLo.InternalHelpers;

namespace PixelJump.InterfaceObjects
{
	public class Button
	{
        Vector2 position;
        Vector2 size;
        string textOnButton;
        string screenShouldBeDisplayedOn;
        string keyShouldSwitchTo;
        Screen screenShouldBeSwitchedTo;
        bool buttonPressed;

        public Button(Vector2 position, Vector2 size, string textOnButton, string screenShouldBeDisplayedOn, Screen screenShouldBeSwitchedTo, bool buttonPressed)
        {
            this.position = position;
            this.size = size;
            this.textOnButton = textOnButton;
            this.screenShouldBeDisplayedOn = screenShouldBeDisplayedOn;
            this.screenShouldBeSwitchedTo = screenShouldBeSwitchedTo;
            this.buttonPressed = buttonPressed;
        }

        public Button(Vector2 position, Vector2 size, string textOnButton, string screenShouldBeDisplayedOn, string keyShouldSwitchTo, bool buttonPressed)
        {
            this.position = position;
            this.size = size;
            this.textOnButton = textOnButton;
            this.screenShouldBeDisplayedOn = screenShouldBeDisplayedOn;
            this.keyShouldSwitchTo = keyShouldSwitchTo;
            this.buttonPressed = buttonPressed;
        }

        public void CreateButton(List<Button> buttons, Vector2 position, Vector2 size, string textOnButton, string screenShouldBeDisplayedOn, Screen screenShouldBeSwitchedTo, string keyShouldSwitchTo)
        {
            if(screenShouldBeSwitchedTo == null)
            {
                buttons.Add(new Button(position, size, textOnButton, screenShouldBeDisplayedOn, keyShouldSwitchTo, false));
            }
            else if(keyShouldSwitchTo == null)
            {
                buttons.Add(new Button(position, size, textOnButton, screenShouldBeDisplayedOn, screenShouldBeSwitchedTo, false));
            }
        }

        public void UpdateButton(List<Button> buttons)
        {
            Vector2 mousePosition = GetMousePosition();

            foreach (Button button in buttons)
            {
                if (mousePosition.X >= button.position.X
                 && mousePosition.X <= button.position.X + button.size.X
                 && mousePosition.Y >= button.position.Y
                 && mousePosition.Y <= button.position.Y + button.position.Y
                 && IsMouseButtonPressed(MOUSE_LEFT_BUTTON))
                {
                    button.buttonPressed = true;
                }
            }
        }

        public void drawButton(List<Button> buttons, string screenShouldBeDisplayedOn)
        {
            foreach(Button button in buttons)
            {
                if(screenShouldBeDisplayedOn == button.ScreenShouldBeDisplayedOn)
                {
                    DrawRectangle((int)button.position.X, (int)button.position.Y, (int)button.size.X, (int)button.size.Y, RED);
                    DrawText(button.textOnButton, button.position.X + (int) (0.5 * button.size.X - 0.5 * (0.25 * button.size.Y * button.textOnButton.Length)), button.position.Y + (int) (0.3 * button.size.Y), (int) (0.4 * button.size.Y), BLACK);
                }
            }
        }

        public Vector2 Position { get => position; set => position = value; }
        public Vector2 Size { get => size; set => size = value; }
        public string TextOnButton { get => textOnButton; set => textOnButton = value; }
        public string ScreenShouldBeDisplayedOn { get => screenShouldBeDisplayedOn; set => screenShouldBeDisplayedOn = value; }
        public Screen ScreenShouldBeSwitchedTo { get => screenShouldBeSwitchedTo; set => screenShouldBeSwitchedTo = value; }
        public string KeyShouldSwitchTo { get => keyShouldSwitchTo; set => keyShouldSwitchTo = value; }
        public bool ButtonPressed { get => buttonPressed; set => buttonPressed = value; }
    }
}

