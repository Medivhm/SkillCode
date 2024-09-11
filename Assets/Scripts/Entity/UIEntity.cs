using Manager;
using System;
using UnityEngine;

namespace QEntity
{
    public class UIEntity : MonoBehaviour
    {
        [NonSerialized]
        public RectTransform rectTrans;
        public bool IsShow;
        private bool isDestroy;

        public bool IsDestroy
        {
            get { return isDestroy; }
            set { isDestroy = value; }
        }

        private void Awake()
        {
            rectTrans = this.gameObject.GetComponent<RectTransform>();
            IsDestroy = false;
        }

        private void Start()
        {
            // 留着，不知道为什么Awake里赋值了，这里还是null
            if(rectTrans.IsNull()) rectTrans = this.gameObject.GetComponent<RectTransform>();
            ResetTransform();
        }

        // UI初始化，数据清零
        // 不要自己Init自己，规范只能Init自己的子UI
        public virtual void Init()
        {
            IsDestroy = false;
        }

        // UI刷新
        public virtual void RefreshUI()
        {
        }

        public virtual void ResetTransform()
        {
            ResetScale();
        }

        public virtual void ResetScale()
        {
            rectTrans.localScale = Vector2.one;
        }

        public virtual void ResetAnchoredPosition()
        {
            rectTrans.anchoredPosition = Vector2.zero;
        }

        public virtual void ResetSizeDelta()
        {
            this.rectTrans.sizeDelta = Vector2.zero;
        }

        public virtual void Show()
        {
            this.gameObject.SetActive(true);
            IsShow = true;
            //UIManager.UIPush(this);
            //Ctrl.UseMouse();
        }

        public virtual void Hide()
        {
            this.gameObject.SetActive(false);
            IsShow = false;
            //UIManager.UIPop();
            //Ctrl.UnUseMouse();
        }

        public void PureShow()
        {
            this.gameObject.SetActive(true);
            IsShow = true;
        }

        public void PureHide()
        {
            this.gameObject.SetActive(false);
            IsShow = false;
        }

        public virtual void Destroy()
        {
            //onPointerClick = null;
            //onPointerDown = null;
            //onPointerEnter = null;
            //onPointerExit = null;
            //onPointerUp = null;
            if (IsShow) Hide();
            isDestroy = true;
            PoolManager.GetUIPool().UnSpawn(gameObject, name);
        }


        //////////////////////////////   事件方法

        //private bool pierceTouch = false;
        //public Action<PointerEventData> onPointerClick;
        //public Action<PointerEventData> onPointerDown;
        //public Action<PointerEventData> onPointerEnter;
        //public Action<PointerEventData> onPointerExit;
        //public Action<PointerEventData> onPointerUp;

        //// 设置穿透触摸
        //public void SetPierce(bool state)
        //{
        //    pierceTouch = state;
        //}

        //public void OnPointerClick(PointerEventData eventData)
        //{
        //    if (pierceTouch)
        //    {
        //        GetRaycastResults(eventData);
        //    }
        //    onPointerClick?.Invoke(eventData);
        //    if (pierceTouch)
        //    {
        //        this.OnExecute(eventData, ExecuteEvents.pointerClickHandler);
        //    }
        //}

        //public void OnPointerDown(PointerEventData eventData)
        //{
        //    if (pierceTouch)
        //    {
        //        GetRaycastResults(eventData);
        //    }
        //    onPointerDown?.Invoke(eventData);
        //    if (pierceTouch)
        //    {
        //        this.OnExecute(eventData, ExecuteEvents.pointerDownHandler);
        //    }
        //}

        //public void OnPointerEnter(PointerEventData eventData)
        //{
        //    if (pierceTouch)
        //    {
        //        GetRaycastResults(eventData);
        //    }
        //    onPointerEnter?.Invoke(eventData);
        //    if (pierceTouch)
        //    {
        //        this.OnExecute(eventData, ExecuteEvents.pointerEnterHandler);
        //    }
        //}

        //public void OnPointerExit(PointerEventData eventData)
        //{
        //    if (pierceTouch)
        //    {
        //        GetRaycastResults(eventData);
        //    }
        //    onPointerExit?.Invoke(eventData);
        //    if (pierceTouch)
        //    {
        //        this.OnExecute(eventData, ExecuteEvents.pointerExitHandler);
        //    }
        //}

        //public void OnPointerUp(PointerEventData eventData)
        //{
        //    if (pierceTouch)
        //    {
        //        GetRaycastResults(eventData);
        //    }
        //    onPointerUp?.Invoke(eventData);
        //    if (pierceTouch)
        //    {
        //        this.OnExecute(eventData, ExecuteEvents.pointerUpHandler);
        //    }
        //}


        ////事件穿透传递
        //private List<RaycastResult> m_raycastResults = null;

        //private void GetRaycastResults(PointerEventData eventData)
        //{
        //    if (m_raycastResults == null) m_raycastResults = new List<RaycastResult>();
        //    EventSystem.current.RaycastAll(eventData, m_raycastResults);
        //}
        //private void OnExecute<T>(PointerEventData eventData, ExecuteEvents.EventFunction<T> func) where T : IEventSystemHandler
        //{
        //    int len = m_raycastResults.Count;
        //    for (var i = 0; i < len; i++)
        //    {
        //        if (m_raycastResults[i].gameObject == this.gameObject) continue;
        //        ExecuteEvents.Execute(m_raycastResults[i].gameObject, eventData, func);
        //    }
        //    m_raycastResults.Clear();
        //}
    }
}
