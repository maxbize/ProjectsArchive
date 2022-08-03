using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    private const string MONEY_KEY_STR = "MONEY";

    private int money;

    public InputManager inputManager;

    // Use this for initialization
    void Start() {
        load();

    }

    // Update is called once per frame
    void Update() {

    }

    public bool requestMoney(int amount) {
        if (money >= amount) {
            money -= amount;
            inputManager.updateMoneyText(money);
            save();
            return true;
        }
        return false;
    }

    public void acceptMoney(int amount) {
        money += amount;
        inputManager.updateMoneyText(money);
        save();
    }

    private void load() {
        money = PlayerPrefs.GetInt(MONEY_KEY_STR, 5000);
        // DEBUG
        if (money < 100) {
            money = 5000;
        }
        inputManager.updateMoneyText(money);
    }

    private void save() {
        PlayerPrefs.SetInt(MONEY_KEY_STR, money);
        PlayerPrefs.Save();
    }
}
