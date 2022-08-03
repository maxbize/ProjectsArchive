using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{

    public enum clips { startPlay, win };

    public AudioClip startPlayClip;
    public AudioClip winClip;

    private Dictionary<clips, AudioClip> clipMap;

    // Use this for initialization
    void Start() {
        clipMap = new Dictionary<clips, AudioClip>() {
            {clips.startPlay, startPlayClip},
            {clips.win, winClip}
        };
    }

    // Update is called once per frame
    void Update() {

    }

    public void playClip(clips clip) {
        AudioSource.PlayClipAtPoint(clipMap[clip], Vector3.zero, 0.1f);
    }
}
