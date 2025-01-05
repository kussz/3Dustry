using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.DirectInput;

namespace GameObjects.GameLogic
{
    public class InputHandler
    {
        private DirectInput directInput;
        private Keyboard keyboard;
        private Mouse mouse;
        private KeyboardState keyboardState;
        private double _border = Math.PI/2;
        private bool _wasMouseButtonDown = false;
        public bool Left { get; private set; }
        public bool Right { get; private set; }
        public bool Forward { get; private set; }
        public bool Backward { get; private set; }
        public bool MouseClick { get; private set; }
        public bool Up { get; private set; }
        public bool Down { get; private set; }
        public bool Shift { get; private set; }
        public bool Aplus { get; private set; }
        public bool Aminus { get; private set; }
        public bool Bplus { get; private set; }
        public bool Bminus { get; private set; }
        public float MouseX { get; private set; }
        public float MouseY { get; private set; }
        public bool Escape { get; private set; } = false;
        public float prevTime = Environment.TickCount;
        public float newTime = Environment.TickCount;
        public float DeltaTime { get { return newTime - prevTime; } }

        public InputHandler()
        {
            directInput = new DirectInput();
            keyboard = new Keyboard(directInput);
            keyboard.Acquire();
            mouse = new Mouse(directInput);
            mouse.Acquire();
        }

        public void Update()
        {
            MouseClick = false;
            prevTime = newTime;
            newTime = Environment.TickCount;
            // Обновляем состояние клавиатуры
            keyboardState = keyboard.GetCurrentState();

            if (keyboardState.IsPressed(Key.W))
                Forward = true;
            else
                Forward = false;
            if (keyboardState.IsPressed(Key.A))
                Left = true;
            else
                Left = false;
            if (keyboardState.IsPressed(Key.D))
                Right = true;
            else
                Right = false;
            if (keyboardState.IsPressed(Key.S))
                Backward = true;
            else
                Backward = false;
            if (keyboardState.IsPressed(Key.Space))
                Up = true;
            else
                Up = false;
            if (keyboardState.IsPressed(Key.C))
                Down = true;
            else
                Down = false;
            if (keyboardState.IsPressed(Key.LeftShift))
                Shift = true;
            else
                Shift = false;
            if (keyboardState.IsPressed(Key.NumberPad8))
                Aplus = true;
            else
                Aplus = false;
            if (keyboardState.IsPressed(Key.NumberPad2))
                Aminus = true;
            else
                Aminus = false;
            if (keyboardState.IsPressed(Key.NumberPad6))
                Bplus = true;
            else
                Bplus = false;
            if (keyboardState.IsPressed(Key.NumberPad4))
                Bminus = true;
            else
                Bminus = false;
            if (keyboardState.IsPressed(Key.Escape))
                Escape = true;
            MouseState state = mouse.GetCurrentState();
            bool isMouseButtonDown = state.Buttons[0]; // 0 - левая кнопка мыши
            if (isMouseButtonDown && !_wasMouseButtonDown)
            {
                MouseClick = true; // Считаем щелчком, если кнопка была нажата сейчас
                _wasMouseButtonDown = true;
            }
            else if (!isMouseButtonDown)
                _wasMouseButtonDown = false;
            MouseX += state.X/100f;
            MouseY += state.Y/100f;
            MouseY = (float)Math.Min(Math.Max(-_border, MouseY), _border);
            if (MouseX > MathUtil.Pi) MouseX -= MathUtil.TwoPi;
            else if (MouseX < -MathUtil.Pi) MouseX += MathUtil.TwoPi;
        }
    }
}
