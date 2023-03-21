using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;

namespace KhushbuPlugin
{
    public class UtilitySound : MonoBehaviour
    {
        public static UtilitySound Instance;
        public bool ismusic;
        [System.Serializable]
        public class Sound
        {
            public AudioClip clip;
            [HideInInspector]
            public int simultaneousPlayCount = 0;
        }
        private int maxSimultaneousSounds = 10;
        public Sound Background;
        public Sound ButtonSound;
        public Sound addbet;
        public Sound addchip;
        public Sound VSsound;
        public Sound TigerRoar;
        public Sound DragonRoar;
        public Sound Cardflip;

        public AudioSource AudioSource;
        public AudioSource OtherAudioSource;
        private void Awake()
        {
            Instance = this;
        }
        private void Start()
        {
            if (ismusic)
            {
                SetMusic(UtilityModel.GetMusic());
                PlayMusic(Background);
            }
        }      
        public void PlaySound(Sound sound, bool autoScaleVolume = true, float maxVolumeScale = 1f)
        {
            if (!UtilityModel.GetSound())
            {
                return;
            }
            StartCoroutine(CRPlaySound(sound, autoScaleVolume, maxVolumeScale));
        }
        public void PlayClockSound(Sound sound, bool isLoop = false)
        {
            if (!UtilityModel.GetSound())
            {
                return;
            }
            OtherAudioSource.loop = isLoop;
            OtherAudioSource.clip = sound.clip;
            OtherAudioSource.Play();
        }
        public void StopClockSound()
        {
            OtherAudioSource.Stop();
        }
        IEnumerator CRPlaySound(Sound sound, bool autoScaleVolume = true, float maxVolumeScale = 1f)
        {
            if (sound.simultaneousPlayCount >= maxSimultaneousSounds)
            {
                yield break;
            }
            sound.simultaneousPlayCount++;
            float vol = maxVolumeScale;
            if (autoScaleVolume && sound.simultaneousPlayCount > 0)
            {
                vol = vol / (float)(sound.simultaneousPlayCount);
            }
            Debug.Log("Sound Clip Name " + sound.clip.name);
            OtherAudioSource.PlayOneShot(sound.clip, vol);
            float delay = sound.clip.length * 0.7f;
            yield return new WaitForSeconds(delay);
            sound.simultaneousPlayCount--;
        }
        public void PlayMusic(Sound music, bool loop = true)
        {
            if (AudioSource.clip != null)
            {
                if (AudioSource.clip.name == music.clip.name)
                {
                    return;
                }
            }
            AudioSource.clip = music.clip;
            AudioSource.loop = loop;
            AudioSource.Play();
        }
        public void SetMixer(bool isSet)
        {
            //if (isSet)
            //{
            //    AudioMixer mixer = Resources.Load("MainAudioMixer") as AudioMixer;
            //    if (mixer != null)
            //    {
            //        string _OutputMixer = "Master";
            //        GetComponent<AudioSource>().outputAudioMixerGroup = mixer.FindMatchingGroups(_OutputMixer)[0];
            //    }
            //    else
            //    {
            //        GetComponent<AudioSource>().outputAudioMixerGroup = null;
            //    }
            //}
            //else
            //{
            //    GetComponent<AudioSource>().outputAudioMixerGroup = null;
            //}
        }
        public void ToggleSound()
        {
            bool sound = UtilityModel.GetSound();
            UtilityModel.SetSound(!sound);
        }
        public void ToggleVibration()
        {
            bool Vibration = UtilityModel.GetSound();
            UtilityModel.SetVibration(!Vibration);
        }
        public void ToggleMusic()
        {
            bool music = UtilityModel.GetMusic();
            UtilityModel.SetMusic(!music);
            SetMusic(!music);
        }
        void SetMusic(bool isMuted)
        {
            AudioSource.mute = isMuted;
            if (!isMuted)
            {
                AudioSource.Play();
            }
        }

        //All Sound Play Event Here ----------------------------------------------------------------------------
        public void ButtonClickSound()
        {
            //PlaySound(ButtonSound);
        }
        public void addbetsound()
        {
            PlaySound(addbet);
        }
        public void addchipsound()
        {
            PlaySound(addchip);
        }
        public void VSsoundbtn()
        {
            PlaySound(VSsound);
        }
        public void TigerRoarsound()
        {
            PlaySound(TigerRoar);
        }
        public void DragonRoarsound()
        {
            PlaySound(DragonRoar);
        }
        public void Cardflipsound()
        {
            PlaySound(Cardflip);
        }
        public void SetVolume(float highvol = 1)
        {
            AudioSource.volume = highvol;
            OtherAudioSource.volume = highvol;
        }
    }
}
