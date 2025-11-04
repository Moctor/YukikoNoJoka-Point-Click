using UnityEngine.Audio;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public Dictionary<string, int> sources = new Dictionary<string, int>();

    private void Awake()
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            sounds[i].source = gameObject.AddComponent<AudioSource>();
            sounds[i].source.clip = sounds[i].clip;

            sounds[i].source.volume = sounds[i].volume;
            sounds[i].source.pitch = sounds[i].pitch;

            sources.Add(sounds[i].name, i);
        }
        /*foreach (Sound s in sounds)
        {
            sounds[i].source = gameObject.AddComponent<AudioSource>();
            sounds[i].source.clip = sounds[i].clip;

            sounds[i].source.volume = sounds[i].volume;
            sounds[i].source.pitch = sounds[i].pitch;

            sources.Add(sounds[i].name, );
        }*/
    }

    public IEnumerator PlaySound(string name)
    {

        //Sound s = Array.Find(sounds, sound => sound.name == name);
        Sound s = sounds[sources[name]];
        /*for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == name)
            {
                s = sounds[i];
            }

        }*/

        if (s == null)
        {
            Debug.Log("Isnull");
            yield break;
           
        }
        if (s.isAccess)
        {
            s.source.Play();
            s.isAccess = false;
            yield return new WaitForSeconds(s.clip.length);
            s.isAccess = true;
        }
        
    }
}
