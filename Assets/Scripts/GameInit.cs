using Manager;
using System.Collections;
using Tools;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameInit : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(Init());
    }

    IEnumerator Init()
    {
        InitMonoManager(GameObject.Find("GameInit"));
        yield return null;
        InitCommonManager();
        Main.DontDestroyTemp = GameObject.Find("DontDestroyTemp").transform;
        Main.EventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        yield return null;
        GameState.ChangeState(GameStateEnum.Start);

        StartCoroutine(Test());
    }


    IEnumerator Test()
    {
        yield return new WaitForSeconds(0.5f);
        GameObject player = GameObject.Instantiate(LoadTool.LoadPrefab("Player/Player"));
        player.transform.position = new Vector3(0, 10, 0);


        //GameObject creature = GameObject.Instantiate(LoadTool.LoadPrefab("Creature/Goblin"));
    }

    void InitMonoManager(GameObject GameInit)
    {
        GameInit.AddComponent<QuickKeyManager>();
        GameInit.AddComponent<SceneManager>();
        GameInit.AddComponent<PoolManager>();
        GameInit.AddComponent<ChunkManager>();
        GameInit.AddComponent<TickHelper>();
        GameInit.AddComponent<ChunkColliderBuilder>();
        //GameInit.AddComponent<MainThreadManager>();
        //GameInit.AddComponent<LuaManager>();

    }

    void InitCommonManager()
    {
        StartCoroutine(DataManager.Instance.Init());

        PoolManager.Instance.Init();
        AudioManager.Instance.Init();
        CarrierManager.Instance.Init();
        FxManager.Instance.Init();
        //EnemyManager.Instance.Init();
        FunctionManager.Instance.Init();
        GameSaveManager.Instance.Init();
        ItemManager.Instance.Init();
        TextureManager.Instance.Init();
        UIManager.Instance.Init();
        ChunkManager.Instance.Init();
        MainMouseController.Instance.Init();
        //UserManager.Instance.Init();
        HUDManager.Instance.Init();
        //BRGManager.Instance.Init();


        MagicSkillManager.Instance.Init();
    }
}
