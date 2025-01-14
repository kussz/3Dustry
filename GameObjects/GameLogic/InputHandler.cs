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
        public bool Left { get; private set; }
        public bool Right { get; private set; }
        public bool Forward { get; private set; }
        public bool Backward { get; private set; }
        public bool LeftMouseClick { get; private set; }
        public bool RightMouseClick { get; private set; }
        public bool Up { get; private set; }
        public bool Down { get; private set; }
        public bool Shift { get; private set; }
        public bool R { get; private set; }
        private bool _wasRDown = false;
        public float MouseX { get; private set; }
        public float MouseY { get; private set; }
        public bool Escape { get; private set; } = false;
        public float prevTime = Environment.TickCount;
        public float newTime = Environment.TickCount;
        public float DeltaTime { get { return newTime - prevTime; } }
        public bool SelectionChanged { get; set; } = false;
        public int HotbarSelection { get; private set; } = 1;
        private Key[] _hotbarKeys = { Key.D1, Key.D2, Key.D3, Key.D4, Key.D5, Key.D6, Key.D7, Key.D8 };

        public InputHandler()
        {
            directInput = new DirectInput();
            keyboard = new Keyboard(directInput);
            keyboard.Acquire();
            keyboardState = keyboard.GetCurrentState();
            mouse = new Mouse(directInput);
            mouse.Acquire();
        }

        public void Update()
        {
            LeftMouseClick = false;
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
            if (keyboardState.IsPressed(Key.R) && !_wasRDown)
            {
                R = true; // Считаем щелчком, если кнопка была нажата сейчас
                _wasRDown = true;
            }
            else if (!keyboardState.IsPressed(Key.R))
            {
                _wasRDown = false;
                R = false;
            }
            else
                R = false;
            if (keyboardState.IsPressed(Key.Escape))
                Escape = true;
            for (int i = 0; i < _hotbarKeys.Length; i++)
            {
                if (keyboardState.IsPressed(_hotbarKeys[i]))
                {
                    SelectionChanged = true;
                    HotbarSelection = i + 1; // Увеличиваем на 1, чтобы соответствовать индексу
                    break; // Выход из цикла после обнаружения нажатия
                }
            }

            MouseState state = mouse.GetCurrentState();
            LeftMouseClick = state.Buttons[0]; // 0 - левая кнопка мыши
            RightMouseClick = state.Buttons[1]; // 0 - левая кнопка мыши
            //if (isMouseButtonDown && !_wasMouseButtonDown)
            //{
            //    LeftMouseClick = true; // Считаем щелчком, если кнопка была нажата сейчас
            //    //_wasMouseButtonDown = true;
            //}
            //else if (!isMouseButtonDown)
            //    _wasMouseButtonDown = false;
            MouseX += state.X/100f;
            MouseY += state.Y/100f;
            MouseY = (float)Math.Min(Math.Max(-_border, MouseY), _border);
            if (MouseX > MathUtil.Pi) MouseX -= MathUtil.TwoPi;
            else if (MouseX < -MathUtil.Pi) MouseX += MathUtil.TwoPi;
        }
    }
}
