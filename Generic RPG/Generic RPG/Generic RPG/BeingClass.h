#include <iostream>
#include <string>
#include "Inventory.h"
#include "Weapon.h"
//#include "MyFunctions.h"
#pragma once

using namespace std;

class Being
{
	
public: 
	Being();
	Being(int str, int dex, int hp, int lvl, string Name);
	void displayStats();
	void gainExp(int exp, bool displayText);
	int getDexterity();
	int getStrength();
	int getHealth();
	int getLevel();
	int getExp();
	int getMAXhealth();
	void loseHealth( int damage );
	string getName();
	void displayInventory();
	void displayUsableInventory();
	void obtainObject(Item item);
	void obtainObject(Weapon weapon);
	void useItem(int invIndex);
	int getInventorySize();
	int getGold();
	void gainGold(int amount);
	void equipWeapon(int invSlot);
	int getDamage();
	void displayEquipment();

private:
	int strength, dexterity, health, MAXhealth, exp, level, experience, expReq, gold, strBonus, dexBonus, hpBonus, expBonus, goldBonus;
	string name;
	Inventory inventory;
	Weapon equippedWeapon;

	void levelUp();
};