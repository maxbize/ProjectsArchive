using UnityEngine;
using System.Collections;

public static class SoundManager
{

    private static AudioClip moveClip = Resources.Load<AudioClip>("sounds/move1");
    private static AudioClip moveClip2 = Resources.Load<AudioClip>("sounds/move2");
    private static AudioClip powerUpClip = Resources.Load<AudioClip>("sounds/powerUpPickup");
    private static AudioClip endClip = Resources.Load<AudioClip>("sounds/end");
    private static AudioClip drillClip = Resources.Load<AudioClip>("sounds/drill");


    public static void playMoveSound(Int3 pos) {
        switch ((int)Random.Range(0, 2)) {
            case 0:
                AudioSource.PlayClipAtPoint(moveClip, pos, 0.5F);
                break;
            case 1:
                AudioSource.PlayClipAtPoint(moveClip2, pos, 0.5F);
                break;
        }
    }

    public static void playPowerUpPickupSound(Int3 pos) {
        AudioSource.PlayClipAtPoint(powerUpClip, pos);
    }

    public static void playEndSound(Int3 pos) {
        AudioSource.PlayClipAtPoint(endClip, pos);
    }

    public static void playDrillSound(Int3 pos) {
        AudioSource.PlayClipAtPoint(drillClip, pos);
    }

}
