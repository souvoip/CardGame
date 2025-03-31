/**
 * 声音管理类
 * 使用方法：
 *     1、编辑界面FrameWork->Create FrameWork Object建立框架对象
 *     2、使用SoundManager.GetSingleton()获取单例
 *     3、调用你要使用的方法
 */

using UnityEngine;
using System.Collections;
using System;
using Unity.VisualScripting;
using System.Collections.Generic;
using System.Linq;


public class MusicItem
{
    public string musicName;
    public AudioSource audioSource;
    public float volume = 1f;
}


public class SoundItem
{
    private static int count = 0;

    public int _id;
    public string soundName;
    public AudioSource audioSource;
    public float volume = 1f;

    public SoundItem(AudioSource audio)
    {
        _id = count;
        count++;
        audioSource = audio;
    }
}


public class SoundManager : MonoBehaviour
{
    private static SoundManager mng;
    public static SoundManager GetSingleton()
    {
        return mng;
    }

    private float musicVolume = 0.2f;
    /// <summary>
    /// 音乐音量
    /// </summary>
    public float MusicVolume
    {
        get { return musicVolume; }
    }
    private float allSoundVolume = 1f;
    /// <summary>
    /// 音效音量
    /// </summary>
    public float SoundVolume
    {
        get { return allSoundVolume; }
    }
    [SerializeField]
    private int maxAudioSourceNum = 5;

    private Transform soundObj;
    private Transform musicObj;
    private List<SoundItem> soundItems;
    private List<AudioSource> soundSources;
    private MusicItem musicItem;

    private void Awake()
    {
        if (mng == null)
        {
            DontDestroyOnLoad(gameObject);
            mng = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (transform.Find("SoundObj") == null)
        {
            soundObj = new GameObject("SoundObj").transform;
            soundObj.gameObject.AddComponent<AudioSource>();
            soundObj.SetParent(transform);
        }
        else
        {
            soundObj = transform.Find("SoundObj");
            if (soundObj.GetComponent<AudioSource>() == null)
            {
                soundObj.gameObject.AddComponent<AudioSource>();
            }
        }

        if (transform.Find("MusicObj") == null)
        {
            musicObj = new GameObject("MusicObj").transform;
            musicObj.gameObject.AddComponent<AudioSource>();
            musicObj.SetParent(transform);
        }
        else
        {
            musicObj = transform.Find("MusicObj");
            if (musicObj.GetComponent<AudioSource>() == null)
            {
                musicObj.gameObject.AddComponent<AudioSource>();
            }
        }
        soundItems = new List<SoundItem>();
        soundSources = soundObj.GetComponents<AudioSource>().ToList();
        musicItem = new MusicItem();
        musicItem.audioSource = musicObj.GetComponent<AudioSource>();
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    private void EventPlaySound(string path)
    {
        if (PlaySound(path) == -1)
        {
            Debug.LogWarning("音乐播放失效=>" + path);
        }
    }

    //获取可以播放的AudioSource
    private SoundItem GetFreeAudioSource()
    {
        if (soundSources == null || soundSources.Count == 0) { return null; }

        AudioSource source = null;
        for (int i = 0; i < soundSources.Count; i++)
        {
            if (!soundSources[i].isPlaying)
            {
                source = soundSources[i];
                break;
            }
        }
        if (source == null)
        {
            source = soundObj.AddComponent<AudioSource>();
            soundSources.Add(source);
        }
        SoundItem item = new SoundItem(source);
        soundItems.Add(item);
        return item;
    }

    /// <summary>
    /// 等待音效结束
    /// </summary>
    /// <param name="soundItem"></param>
    /// <param name="func">回调函数</param>
    /// <returns></returns>
    private IEnumerator WaitPlayEnd(SoundItem soundItem, Action func)
    {
        yield return new WaitUntil(() => { return !soundItem.audioSource.isPlaying; });

        if (soundSources.Count > maxAudioSourceNum)
        {
            soundSources.Remove(soundItem.audioSource);
            Destroy(soundItem.audioSource);
        }
        soundItems.Remove(soundItem);
        soundItem = null;

        func?.Invoke();
    }

    /// <summary>
    /// 播放背景音乐，如果已经在播放，替换原背景
    /// </summary>
    /// <param name="path">音频文件在Resource目录下的路径</param>
    /// <param name="isLoop">是否循环</param>
    /// <param name="isRestart">如果音乐名字相同是否重新开始播放</param>
    public bool PlayMusic(string path, bool isLoop = true, bool isRestart = false)
    {
        if (musicItem.audioSource == null) { return false; }
        if (isRestart && musicItem.musicName == path) { return false; }
        musicItem.audioSource.clip = Resources.Load<AudioClip>(path);
        if (musicItem.audioSource.clip == null) { return false; }
        else
        {
            musicItem.audioSource.loop = isLoop;
            musicItem.audioSource.Play();
            musicItem.audioSource.volume = musicVolume;
            musicItem.musicName = path;
            return true;
        }
    }

    /// <summary>
    /// 停止背景音乐
    /// </summary>
    public bool StopMusic()
    {
        if (musicItem.audioSource == null) { return false; }
        musicItem.audioSource.Stop();
        return true;
    }

    /// <summary>
    /// 播放音效
    /// </summary>
    /// <param name="path">音频文件在Resource目录下的路径</param>
    /// <param name="volume">设置音量，默认为1</param>
    /// <param name="action">音效播放完成回调</param>
    /// <returns>播放音效的ID，可用于停止播放音效, -1表示失败</returns>
    public int PlaySound(string path, float volume = 1f, Action action = null, bool isStopAll = true)
    {
        if (isStopAll)
        {
            StopAllSound();
        }
        AudioClip audio = Resources.Load<AudioClip>(path);
        if (audio == null) { return -1; }
        SoundItem soundItem = GetFreeAudioSource();
        if (soundItem == null) { return -1; }

        soundItem.soundName = path;
        float volumeTemp = volume * allSoundVolume;
        soundItem.audioSource.clip = audio;
        soundItem.volume = volumeTemp;
        soundItem.audioSource.volume = volumeTemp;
        soundItem.audioSource.Play();
        StartCoroutine(WaitPlayEnd(soundItem, action));
        return soundItem._id;
    }

    /// <summary>
    /// 停止音效
    /// </summary>
    /// <param name="name">音效名字 默认为音频路径</param>
    public void StopSound(string name)
    {
        if (name == null) { return; }
        Debug.Log("Stop");
        Debug.Log(soundItems.Count);
        for (int i = 0; i < soundItems.Count; i++)
        {
            Debug.Log(soundItems[i].soundName + " :----:----: " + name);
            if (soundItems[i].soundName == name)
            {
                soundItems[i].audioSource.Stop();
            }
        }

        //foreach (SoundItem item in soundItems)
        //{
        //    if (item.soundName == name)
        //    {
        //        item.audioSource.Stop();
        //    }
        //}
    }

    /// <summary>
    /// 停止音效
    /// </summary>
    /// <param name="id"></param>
    public void StopSound(int id)
    {
        if (id < 0) { return; }

        foreach (SoundItem item in soundItems)
        {
            if (item._id == id)
            {
                item.audioSource.Stop();
            }
        }
    }

    /// <summary>
    /// 设置音乐音量
    /// </summary>
    /// <param name="volume">音量</param>
    public bool SetMusicVolume(float volume)
    {
        if (musicItem.audioSource == null || volume < 0 || volume > 1)
            return false;

        musicVolume = volume;
        musicItem.audioSource.volume = volume;
        return true;
    }

    /// <summary>
    /// 设置所有音频音量
    /// </summary>
    /// <param name="volume">音量</param>
    public bool SetAllSoundVolume(float volume)
    {
        allSoundVolume = volume;
        if (soundItems == null || soundItems.Count == 0)
            return false;

        for (int i = 0; i < soundItems.Count; i++)
        {
            soundItems[i].audioSource.volume = volume * soundItems[i].volume;
        }
        return true;
    }

    /// <summary>
    /// 停止所有音效
    /// </summary>
    public void StopAllSound()
    {
        for (int i = 0; i < soundItems.Count; i++)
        {
            if (soundItems[i].soundName != "Audio/correct" && soundItems[i].soundName != "Audio/error")
            {
                soundItems[i].audioSource.Stop();
            }
        }
    }

    /// <summary>
    /// 获取音效时间
    /// </summary>
    /// <param name="path">路径</param>
    /// <returns>时间</returns>
    public float GetSoundTime(string path)
    {
        AudioClip audio = Resources.Load<AudioClip>(path);
        if (audio == null)
            return -1;
        return audio.length;
    }
}
