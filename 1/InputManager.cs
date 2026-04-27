using Microsoft.Xna.Framework.Input;

namespace FinalProject
{
    public static class InputManager
    {
        private static KeyboardState _currentKey, _prevKey;

        public static void Update()
        {
            _prevKey = _currentKey;
            _currentKey = Keyboard.GetState();
        }

        public static bool IsKeyPressed(Keys key)
        {
            return _currentKey.IsKeyDown(key) && _prevKey.IsKeyUp(key);
        }

        public static bool IsKeyDown(Keys key)
        {
            return _currentKey.IsKeyDown(key);
        }
    }
}