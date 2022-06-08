#include <iostream>
#include <string>
#include "ItemClass.h"
#include "Equipment.h"
#pragma once

using namespace std;

class Weapon
{
public:
	Weapon();
	Weapon(string Name);
	Weapon(int lvl);
	int getDamage();
	string getName();
	void printInfo();
	int getBonus(string bonus);

private:
	int level, strBonus, dexBonus, hpBonus, expBonus, goldBonus, damageBonus, enchants, value, minDamage, maxDamage;
	string name;
};