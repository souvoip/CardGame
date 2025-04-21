using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TimerTools 
{
    public class TimerObj : MonoBehaviour
    {
        private int nowTimerID = 0;
        private Dictionary<TimerData, Coroutine> coroutineDic;
        void Awake()
        {
            coroutineDic = new Dictionary<TimerData, Coroutine>();
            DontDestroyOnLoad(gameObject);
        }

        /// <summary>
        /// 单次触发计时器
        /// </summary>
        /// <param name="delay">延迟时间</param>
        /// <param name="callback">回调</param>
        /// <returns>计时器数据，里面有计时器的基本数据，可用于结束计时器</returns>
        public TimerData Once(float delay, Action callback)
        {
            TimerData data = new TimerData(nowTimerID++, 0, delay, false, false);
            Coroutine coroutine = StartCoroutine(IEOnce(data, delay, callback));
            coroutineDic.Add(data, coroutine);
            return data;
        }

        /// <summary>
        /// 循环计时器
        /// </summary>
        /// <param name="delay">延迟时间</param>
        /// <param name="count">循环次数，小于等于0无限循环</param>
        /// <param name="loopCallback">单词循环回调，返回当前循环次数</param>
        /// <param name="completeCallback">全部完成时的回调</param>
        /// <returns>计时器数据，里面有计时器的基本数据，可用于结束计时器</returns>
        public TimerData Loop(float delay, int count, Action<int> loopCallback, Action completeCallback)
        {
            TimerData data = new TimerData(nowTimerID++, count, delay, true, false);
            Coroutine coroutine = StartCoroutine(IELoop(data, delay, count, loopCallback, completeCallback));
            coroutineDic.Add(data, coroutine);
            return data;
        }

        /// <summary>
        /// 单次触发帧计时器
        /// </summary>
        /// <param name="delay">延迟帧数</param>
        /// <param name="callback">回调</param>
        /// <returns>计时器数据，里面有计时器的基本数据，可用于结束计时器</returns>
        public TimerData FrameOnce(float delay, Action callback)
        {
            TimerData data = new TimerData(nowTimerID++, 0, delay, false, true);
            Coroutine coroutine = StartCoroutine(IEFrameOnce(data, delay, callback));
            coroutineDic.Add(data, coroutine);
            return data;
        }

        /// <summary>
        /// 循环帧计时器
        /// </summary>
        /// <param name="delay">延迟帧数</param>
        /// <param name="count">循环次数，小于等于0无限循环</param>
        /// <param name="loopCallback">单词循环回调，返回当前循环次数</param>
        /// <param name="completeCallback">全部完成时的回调</param>
        /// <returns>计时器数据，里面有计时器的基本数据，可用于结束计时器</returns>
        public TimerData FrameLoop(float delay, int count, Action<int> loopCallback, Action completeCallback)
        {
            TimerData data = new TimerData(nowTimerID++, count, delay, true, true);
            Coroutine coroutine = StartCoroutine(IEFrameLoop(data, delay, count, loopCallback, completeCallback));
            coroutineDic.Add(data, coroutine);
            return data;
        }

        /// <summary>
        /// 停止计时器
        /// </summary>
        /// <param name="data">计时器数据</param>
        public void RemoveTimer(TimerData data)
        {
            if (coroutineDic.ContainsKey(data))
            {
                StopCoroutine(coroutineDic[data]);
                coroutineDic.Remove(data);
            }
        }

        private IEnumerator IEOnce(TimerData data, float delay, Action callback)
        {
            yield return new WaitForSeconds(delay);
            if (callback != null) { callback.Invoke(); }
            data.IsComplete = true;
            if (coroutineDic.ContainsKey(data)) { coroutineDic.Remove(data); }
        }

        private IEnumerator IELoop(TimerData data, float delay, int count, Action<int> loopCallback, Action completeCallback)
        {
            while (true)
            {
                yield return new WaitForSeconds(delay);
                data.NowCount++;
                if (loopCallback != null) { loopCallback.Invoke(data.NowCount); }
                if (count > 0 && data.NowCount >= count) { break; }
            }
            if (completeCallback != null) { completeCallback.Invoke(); }
            data.IsComplete = true;
            if (coroutineDic.ContainsKey(data)) { coroutineDic.Remove(data); }
        }

        private IEnumerator IEFrameOnce(TimerData data, float delay, Action callback)
        {
            for (int i = 0; i < delay; i++) { yield return 0; }
            if (callback != null) { callback.Invoke(); }
            data.IsComplete = true;
            if (coroutineDic.ContainsKey(data)) { coroutineDic.Remove(data); }
        }

        private IEnumerator IEFrameLoop(TimerData data, float delay, int count, Action<int> loopCallback, Action completeCallback)
        {
            while (true)
            {
                for (int i = 0; i < delay; i++) { yield return 0; }
                data.NowCount++;
                if (loopCallback != null) { loopCallback.Invoke(data.NowCount); }
                if (count > 0 && data.NowCount >= count) { break; }
            }
            if (completeCallback != null) { completeCallback.Invoke(); }
            data.IsComplete = true;
            if (coroutineDic.ContainsKey(data)) { coroutineDic.Remove(data); }
        }
    }

    public class TimerData
    {
        public int NowCount { get; set; }
        public bool IsComplete { get; set; }
        public int ID { get; private set; }
        public int MaxCount { get; private set; }
        public float Delay { get; private set; }
        public bool IsLoop { get; private set; }
        public bool IsFrame { get; private set; }

        public TimerData(int id, int maxCount, float delay, bool isLoop, bool isFrame)
        {
            ID = id;
            MaxCount = maxCount;
            Delay = delay;
            IsLoop = isLoop;
            IsFrame = isFrame;
            NowCount = 0;
            IsComplete = false;
        }
    }
}
