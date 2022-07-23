using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.AddressableAssets;
using Cysharp.Threading.Tasks;
using UniRx;

public class SoundManager : SingletonMonoBehaviour<SoundManager>
{
    public enum SoundType
    {
        SE,
        BGM,
    }

    private AudioSource SESource;
    private AudioSource BGMSource;
    public bool CanPlayable;
    private Dictionary<SoundDef, AudioClip> SEClips = new Dictionary<SoundDef, AudioClip>();
    private Dictionary<MusicDef, AudioClip> BGMClips = new Dictionary<MusicDef, AudioClip>();

    private MusicDef? _lastPlayedBGM = null;

    //BGM再生
    public void PlayBgm(MusicDef target,float volume = 1f,bool isLoop = false,float fadeTime = 0f)
    {
        BGMSource.clip = BGMClips[target];
        BGMSource.loop = isLoop;
        BGMSource.volume = volume;
        BGMSource.Play();
        _lastPlayedBGM = target;
    }

    public void StopBgm()
    {
        BGMSource.Stop();
        BGMSource.clip = null;
    }

    //SE再生
    public void PlaySe(SoundDef target,float volume = 1f)
    {
        SESource.PlayOneShot(SEClips[target],volume);
    }

    public void StopSe()
    {
        SESource.Stop();
        SESource.clip = null;
    }

    // Start is called before the first frame update
    async void Awake()
    {
        SESource = gameObject.AddComponent<AudioSource>();
        SESource.playOnAwake = false;
        BGMSource = gameObject.AddComponent<AudioSource>();
        BGMSource.playOnAwake = false;
        Array soundDef = Enum.GetValues(typeof(SoundDef));

        for(int i = 0; i < soundDef.Length; i++)
        {
            object Def = soundDef.GetValue(i);
            AudioClip Clip = await Addressables.LoadAssetAsync<AudioClip>("SE/" + Def.ToString()).NotNullTask();
            SEClips.Add((SoundDef)i, Clip);
        }
        Array musicDef = Enum.GetValues(typeof(MusicDef));
        for (int i = 0; i < musicDef.Length; i++)
        {
            object Def = musicDef.GetValue(i);
            AudioClip Clip = await Addressables.LoadAssetAsync<AudioClip>("BGM/" + Def.ToString()).NotNullTask();
            BGMClips.Add((MusicDef)i, Clip);
        }
        CanPlayable = true;
    }

    private void Start()
    {
        // TODO: 暫定
        this.ObserveEveryValueChanged(x => x.BGMSource.isPlaying)
            .Where(isPlaying => !isPlaying)
            .Where(_ => _lastPlayedBGM == MusicDef.First)
            .Where(_ => BGMSource.clip != null)
            .Subscribe(_ =>
            {
                PlayBgm(MusicDef.Loop, isLoop: true);
            })
            .AddTo(this);
    }

    public bool IsPlaying(SoundType soundType)
    {
        bool isPlaying = soundType switch
        {
            SoundType.SE  => SESource.isPlaying,
            SoundType.BGM => BGMSource.isPlaying,
        };

        return isPlaying;
    }
}
