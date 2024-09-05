using System;

namespace Manager
{
    // UnityEngine.UnityException: Load can only be called from the main thread.
    class MainThreadManager : MonoSingleton<MainThreadManager>
    {
        static Action NextDo;

        public void Init()
        {
            
        }

        private void Update()
        {
            if (NextDo.IsNotNull())
            {
                NextDo.Invoke();
                NextDo = null;
            }
        }

        public static void Add(Action action)
        {
            NextDo += action;
        }
    }
}
