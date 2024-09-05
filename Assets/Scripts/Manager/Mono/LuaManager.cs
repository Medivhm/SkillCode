using Tools;
//using XLua;

namespace Manager
{
    class LuaManager : MonoSingleton<LuaManager>
    {
        //static LuaEnv luaEnv;

        public void Init()
        {
            //    luaEnv = new LuaEnv();
            //    luaEnv.AddLoader(Loader);
            //    DoLua("Main");
        }

        //byte[] Loader(ref string name)
        //{
        //    name = name.Replace('.', '/');
        //    return LoadTool.LoadLua(name);
        //}

        //void Update()
        //{
        //    if (luaEnv.IsNotNull())
        //    {
        //        luaEnv.Tick();
        //    }
        //}

        //void OnDestroy()
        //{
        //    if (luaEnv.IsNotNull())
        //    {
        //        luaEnv.Dispose();
        //        luaEnv = null;
        //    }
        //}

        //static void DoLua(string luaName)
        //{
        //    if (luaEnv.IsNotNull())
        //    {
        //        luaEnv.DoString(string.Format("require '{0}'", luaName));
        //    }
        //}
    }
}
