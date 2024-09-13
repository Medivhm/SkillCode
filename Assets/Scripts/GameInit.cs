using Creature;
using Manager;
using System.Collections;
using Tools;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameInit : MonoBehaviour
{
    void Awake()
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
        Ctrl.SetQuickKey(KeyCode.G, () =>
        {
            Ctrl.GMActive();
        });

        Ctrl.SetQuickKey(KeyCode.B, () =>
        {
            Ctrl.OpenBag();
        });

        Main.MainCanvas = GameObject.Find("MainCanvas").GetComponent<Canvas>();
        yield return new WaitForSeconds(0.3f);


        GameObject creature = GameObject.Instantiate(LoadTool.LoadPrefab("Creature/Goblin"));
        yield return null;
        creature.transform.position = new Vector3(10f, 10f, 4.6f);

        GameObject creature2 = GameObject.Instantiate(LoadTool.LoadPrefab("Creature/Goblin"));
        creature2.transform.position = new Vector3(10f, 13, 10f);
        creature2.GetComponent<Goblin>().SetNoMove(true);

        GameObject creature3 = GameObject.Instantiate(LoadTool.LoadPrefab("Creature/Goblin"));
        creature3.transform.position = new Vector3(-4.8f, 13f, 89.2f);
        creature3.GetComponent<Goblin>().SetNoMove(true);

        GameObject creature4 = GameObject.Instantiate(LoadTool.LoadPrefab("Creature/Goblin"));
        creature4.transform.position = new Vector3(4.74f, 0f, 52f);
        creature4.GetComponent<Goblin>().SetNoMove(true);


        GameObject player = GameObject.Instantiate(LoadTool.LoadPrefab("Player/Player"));
        yield return null;
        player.transform.position = new Vector3(4.7f, 11f, -7f);

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
        //CreatureManager.Instance.Init();
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
