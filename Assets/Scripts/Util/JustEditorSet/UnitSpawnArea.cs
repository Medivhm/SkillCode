using QEntity;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts.Util.JustEditorSet
{
    public class UnitSpawnArea : MonoBehaviour
    {
        public GameObject UnitPrefab;

        public float Radius = 10f;
        public float TimeDieAndBorn = 3f;
        public int maxNum = 5;

        private List<UnitEntity> entities;

        private void Awake()
        {
            entities = new List<UnitEntity>();
        }

        private void Start()
        {
            StartCoroutine(LaterInit());
        }

        IEnumerator LaterInit()
        {
            yield return new WaitForSeconds(1f);
            for (int i = 0; i < maxNum; i++)
            {
                BornUnit();
                yield return null;
            }
        } 

        float randomX;
        float randomZ;
        Vector3 pos;
        private void BornUnit()
        {
            randomX = QUtil.GetRandom(0f, Radius);
            randomZ = QUtil.GetRandom(0f, Radius);
            pos = new Vector3(this.transform.position.x + randomX, this.transform.position.y + 1f, this.transform.position.z + randomZ);
            GameObject go = GameObject.Instantiate(UnitPrefab);
            UnitEntity unit = go.GetComponent<UnitEntity>();
            entities.Add(unit);
            unit.Position = pos;
            unit.DeadEvent += () =>
            {
                entities.Remove(unit);
                TimerManager.Add(TimeDieAndBorn, () =>
                {
                    BornUnit();
                    GUI.ActionInfoLog("史莱姆死了，造新的史莱姆");
                });
            };
        }
    }
}