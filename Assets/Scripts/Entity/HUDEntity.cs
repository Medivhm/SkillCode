using Constant;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools;
using UnityEngine;
using UnityEngine.UI;

namespace QEntity
{
    public class HUDEntity : MonoBehaviour
    {
        public Text nameText;
        public Image fillImg;
        public GameObject talkRectGo;
        public Text talkText;
        public Transform jumpNumScroll;
        [NonSerialized]
        new public string name;
        [NonSerialized]
        public float maxBlood;
        [NonSerialized]
        public float nowBlood;
        [NonSerialized]
        public Transform Owner;
        private float offsetY = 0;
        private float offsetZ = 0;
        private TalkRect talkRect;
        private JumpNumClass jumpNum;

        private QuickTimer quickTimer;

        public void Init(Transform Owner, string name, float maxBlood, float nowBlood)
        {
            this.Owner = Owner;
            Refresh(nowBlood, name, maxBlood);

            quickTimer = new QuickTimer();
            quickTimer.Init();
            talkRect = new TalkRect();
            talkRect.Init(talkRectGo, talkText, quickTimer);

            jumpNum = new JumpNumClass();
            jumpNum.Init(jumpNumScroll);
        }

        public void SetName(string name)
        {
            this.name = name;
            nameText.text = name;
        }

        public void Refresh(float nowBlood, string name = null, float maxBlood = -1)
        {
            this.nowBlood = nowBlood;
            if (maxBlood > 0) this.maxBlood = maxBlood;
            if (name != null)
            {
                SetName(name);
            }
            fillImg.fillAmount = this.nowBlood / this.maxBlood;
        }

        public void Speak(string content)
        {
            if (gameObject.activeSelf)
            {
                StartCoroutine(ShowTalkNextFrame());
                talkRect.Speak(content);
            }
        }

        public void Speak(string content, float time)
        {
            if (gameObject.activeSelf)
            {
                StartCoroutine(ShowTalkNextFrame());
                talkRect.Speak(content, time);
            }
        }

        public void JumpNum(string content)
        {
            if (gameObject.activeSelf)
            {
                JumpNum(content, Color.white);
            }
        }

        public void JumpNum(string content, Color color)
        {
            if (gameObject.activeSelf)
            {
                jumpNum.JumpNum(content, color);
            }
        }

        public void RefreshPosition()
        {
            if (Owner == null) return;

            Vector3 pos = Owner.position;
            pos.y += HUDConstant.HUDHeight + offsetY;
            pos.z += offsetZ;
            SetPosition(pos);
            transform.LookAt(Main.MainCamera.transform);
        }

        public void SetPosition(Vector3 pos)
        {
            this.transform.position = pos;
        }

        // 防止看到对话框大小变化过程
        Vector3 talkLocalPos;
        Vector3 talkHideLocalPos = new Vector3(9999, 9999, 9999);
        IEnumerator ShowTalkNextFrame()
        {
            talkLocalPos = talkRectGo.transform.localPosition;
            talkRectGo.transform.localPosition = talkHideLocalPos;
            talkRectGo.SetActive(true);
            yield return null;
            talkRectGo.transform.localPosition = talkLocalPos;
        }

        public void Destroy()
        {
            quickTimer.DestroyTimers();
            GameObject.Destroy(this.gameObject);
        }
    }

    public class TalkRect
    {
        GameObject root;
        Text talkText;
        QuickTimer quickTimer;

        int speakTimerID = -1;

        internal void Init(GameObject root, Text talkText, QuickTimer quickTimer)
        {
            this.root = root;
            this.talkText = talkText;
            this.quickTimer = quickTimer;
        }

        internal void Speak(string content)
        {
            talkText.text = content;
        }

        internal void Speak(string content, float time)
        {
            talkText.text = content;
            SetSpeakTimer(time, () =>
            {
                root.SetActive(false);
            });
        }

        private void SetSpeakTimer(float time, Action action)
        {
            if (speakTimerID > 0)
            {
                quickTimer.RemoveTimer(speakTimerID);
                speakTimerID = -1;
            }
            speakTimerID = quickTimer.AddTimer(time, () =>
            {
                action();
                speakTimerID = -1;
            });
        }
    }

    public class JumpNumClass
    {
        private Transform JumpNumScroll;
        private JumpNumDir dir;

        internal void Init(Transform scroll)
        {
            JumpNumScroll = scroll;
            dir = JumpNumDir.Right;
        }

        public void ChangeJumpNumDir()
        {
            dir = dir == JumpNumDir.Left ? JumpNumDir.Right : JumpNumDir.Left;
        }

        public void JumpNum(string content, Color color)
        {
            GameObject go = CreateJumpNumCell();
            go.transform.SetParent(JumpNumScroll);
            JumpNumCell script = go.GetComponent<JumpNumCell>();
            ChangeJumpNumDir();
            script.Init(content, color, dir);
        }

        GameObject CreateJumpNumCell()
        {
            return LoadTool.LoadUI("JumpNumCell");
        }
    }
}
