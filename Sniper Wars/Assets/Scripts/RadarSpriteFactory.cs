using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class RadarSpriteFactory : MonoBehaviour {

    public Transform blipPrefab;
    public Sprite enemySprite, allySprite;

    public enum blipType { enemy, ally };
    public Dictionary<blipType, Sprite> spriteMap;

    void Start() {
        spriteMap = new Dictionary<blipType, Sprite>() {
            {blipType.enemy, enemySprite},
            {blipType.ally, allySprite}
        };
    }

    public GameObject makeNewSpriteObj(blipType type) {
        GameObject blipObj = Instantiate(blipPrefab).gameObject;
        blipObj.GetComponent<Image>().sprite = spriteMap[type];
        return blipObj;
    }
}
