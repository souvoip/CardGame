using UnityEngine;
using System;

namespace TimerTools 
{
    public static class Timer
    {
        static private TimerObj timerObj;

        /// <summary>
        /// 单次触发计时器
        /// </summary>
        /// <param name="delay">延迟时间</param>
        /// <param name="callback">回调</param>
        /// <returns>计时器数据，里面有计时器的基本数据，可用于结束计时器</returns>
        static public TimerData Once(float delay, Action callback)
        {
            if (timerObj == null) { timerObj = new GameObject("Timer").AddComponent<TimerObj>(); }
            return timerObj.Once(delay, callback);
        }

        /// <summary>
        /// 循环计时器
        /// </summary>
        /// <param name="delay">延迟时间</param>
        /// <param name="count">循环次数，小于等于0无限循环</param>
        /// <param name="loopCallback">单词循环回调，返回当前循环次数</param>
        /// <param name="completeCallback">全部完成时的回调</param>
        /// <returns>计时器数据，里面有计时器的基本数据，可用于结束计时器</returns>
        static public TimerData Loop(float delay, int count, Action<int> loopCallback, Action completeCallback)
        {
            if (timerObj == null) { timerObj = new GameObject("Timer").AddComponent<TimerObj>(); }
            return timerObj.Loop(delay, count, loopCallback, completeCallback);
        }

        /// <summary>
        /// 单次触发帧计时器
        /// </summary>
        /// <param name="delay">延迟帧数</param>
        /// <param name="callback">回调</param>
        /// <returns>计时器数据，里面有计时器的基本数据，可用于结束计时器</returns>
        static public TimerData FrameOnce(float delay, Action callback)
        {
            if (timerObj == null) { timerObj = new GameObject("Timer").AddComponent<TimerObj>(); }
            return timerObj.FrameOnce(delay, callback);
        }

        /// <summary>
        /// 循环帧计时器
        /// </summary>
        /// <param name="delay">延迟帧数</param>
        /// <param name="count">循环次数，小于等于0无限循环</param>
        /// <param name="loopCallback">单词循环回调，返回当前循环次数</param>
        /// <param name="completeCallback">全部完成时的回调</param>
        /// <returns>计时器数据，里面有计时器的基本数据，可用于结束计时器</returns>
        static public TimerData FrameLoop(float delay, int count, Action<int> loopCallback, Action completeCallback)
        {
            if (timerObj == null) { timerObj = new GameObject("Timer").AddComponent<TimerObj>(); }
            return timerObj.FrameLoop(delay, count, loopCallback, completeCallback);
        }

        /// <summary>
        /// 停止计时器
        /// </summary>
        /// <param name="data">计时器数据</param>
        static public void RemoveTimer(TimerData data)
        {
            if (data == null) return;
            if (timerObj == null) { timerObj = new GameObject("Timer").AddComponent<TimerObj>(); }
            timerObj.RemoveTimer(data);
        }
    }
}