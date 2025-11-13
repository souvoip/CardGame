using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MusicMgr : MonoBehaviour
{
    //唯一的背景音乐组件
    private AudioSource bkMusic = null;
    //音乐大小
    private float bkValue = 0.3f;

    //音效依附对象
    private GameObject soundObj = null;
    //音效列表
    private List<AudioSource> soundList = new List<AudioSource>();
    //音效大小
    private float soundValue = 1;

    public bool soundIsOn = true;

    public bool bkMusicIsOn = true;

    //public MusicMgr()
    //{
    //    MonoMgr.GetInstance().AddUpdateListener(Update);
    //}

    private static MusicMgr instance;

    private void Awake()
    {
        // 检查是否已经存在一个MusicPlayer实例，如果存在，则销毁当前实例
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        // 如果不存在MusicPlayer实例，将当前实例设置为全局实例，并标记为不销毁
        instance = this;
        DontDestroyOnLoad(this.gameObject);
        PlayBGMusic("Music1");
    }
    public static MusicMgr GetInstance()
    {
        return instance;
    }

    private void Update()
    {
        for (int i = soundList.Count - 1; i >= 0; --i)
        {
            if (!soundList[i])
            {
                soundList.RemoveAt(i);
                continue;
            }
            if (!soundList[i].isPlaying)
            {
                GameObject.Destroy(soundList[i]);
                soundList.RemoveAt(i);
            }
        }
    }

    /// <summary>
    /// 播放背景音乐
    /// </summary>
    /// <param name="name"></param>
    public void PlayBGMusic(string name)
    {
        //if(!bkMusicIsOn) return;
        if (bkMusic == null)
        {
            GameObject obj = new GameObject();
            obj.name = "BgMusic";
            bkMusic = obj.AddComponent<AudioSource>();
            //DontDestroyOnLoad(bkMusic);
            Debug.Log("添加背景音乐");
        }
        if (bkMusic.clip && bkMusic.clip.name == name)
        {
            Debug.Log("播放同名音乐，");
            return;
        }
        //异步加载背景音乐 加载完成后 播放
        LoadAsync<AudioClip>("Music/Bgm/" + name, (clip) =>
        {
            bkMusic.clip = clip;
            bkMusic.loop = true;
            bkMusic.volume = bkValue;// *0;
            if (bkMusicIsOn) bkMusic.Play();
            Debug.Log("播放了：" + name);
        });

        //AudioClip clip = Resources.Load<AudioClip>("Music/Bg/" + name);
        //if (clip != null)
        //{
        //    bkMusic.clip = clip;
        //    bkMusic.loop = true;
        //    bkMusic.volume = bkValue;
        //    bkMusic.Play();
        //    Debug.Log("~~~~~~~~~~~~~~~~~~!!!!!!!!");
        //}


    }

    /// <summary>
    /// 暂停背景音乐
    /// </summary>
    public void PauseBKMusic()
    {
        if (bkMusic == null)
            return;
        bkMusic.Pause();
    }

    public void ResumeBKMusic()
    {
        if (bkMusic == null)
            return;
        bkMusic.Play();
    }

    /// <summary>
    /// 停止背景音乐
    /// </summary>
    public void StopBKMusic()
    {
        if (bkMusic == null)
            return;
        bkMusic.Stop();
    }

    /// <summary>
    /// 改变背景音乐 音量大小
    /// </summary>
    /// <param name="v"></param>
    public void ChangeBKValue(float v)
    {
        bkValue = v;
        if (bkMusic == null)
            return;
        bkMusic.volume = bkValue;
    }

    /// <summary>
    /// 播放音效
    /// </summary>
    public void PlaySFX(string name, bool isLoop = false, UnityAction<AudioSource> callBack = null)
    {
        //return; //暂时不加音效
        if (!soundIsOn) return;
        if (soundObj == null)
        {
            soundObj = new GameObject();
            soundObj.name = "SFX";
        }
        //当音效资源异步加载结束后 再添加一个音效
        LoadAsync<AudioClip>("Music/SFX/" + name, (clip) =>
        {
            AudioSource source = soundObj.AddComponent<AudioSource>();
            source.clip = clip;
            source.loop = isLoop;
            source.volume = soundValue;
            source.Play();
            soundList.Add(source);
            callBack?.Invoke(source);
        });

        //AudioClip clip = Resources.Load<AudioClip>("Music/Sound/"+name);
        //AudioSource source = soundObj.AddComponent<AudioSource>();
        //source.clip = clip;
        //source.loop = isLoop;
        //source.volume = soundValue;
        //source.Play();
        //soundList.Add(source);
        //if (callBack != null)
        //    callBack(source);
        // Debug.Log("播放sound");
    }

    /// <summary>
    /// 改变音效声音大小
    /// </summary>
    /// <param name="value"></param>
    public void ChangeSoundValue(float value)
    {
        soundValue = value;
        for (int i = 0; i < soundList.Count; ++i)
            soundList[i].volume = value;
    }

    /// <summary>
    /// 停止音效
    /// </summary>
    public void StopSound(AudioSource source)
    {
        if (soundList.Contains(source))
        {
            soundList.Remove(source);
            source.Stop();
            Destroy(source);
        }
    }

    //异步加载资源
    public void LoadAsync<T>(string name, UnityAction<T> callback) where T : Object
    {
        //开启异步加载的协程
        StartCoroutine(ReallyLoadAsync(name, callback));
    }

    //真正的协同程序函数  用于 开启异步加载对应的资源
    private IEnumerator ReallyLoadAsync<T>(string name, UnityAction<T> callback) where T : Object
    {
        ResourceRequest r = Resources.LoadAsync<T>(name);
        yield return r;

        if (r.asset is GameObject)
            callback(Instantiate(r.asset) as T);
        else
            callback(r.asset as T);
    }
}
