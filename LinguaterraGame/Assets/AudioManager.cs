using UnityEngine;
using System.Collections.Generic; // Important for Dictionary

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public SoundEffect[] soundEffectsArray; // Keep the array for editor setup

    private Dictionary<string, AudioClip> soundEffects = new Dictionary<string, AudioClip>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);

        // Populate the dictionary for faster lookup
        foreach (SoundEffect sfx in soundEffectsArray)
        {
            if (!soundEffects.ContainsKey(sfx.name)) // Prevent duplicates
            {
                soundEffects.Add(sfx.name, sfx.clip);
            }
            else
            {
                Debug.LogError("Duplicate sound effect name: " + sfx.name);
            }
        }
    }

    public void PlaySoundEffect(string soundName)
    {
        if (soundEffects.ContainsKey(soundName))
        {
            AudioSource.PlayClipAtPoint(soundEffects[soundName], transform.position);
        }
        else
        {
            Debug.LogWarning("Sound effect not found: " + soundName);
        }
    }

    [System.Serializable]
    public struct SoundEffect
    {
        public string name;
        public AudioClip clip;
    }
}