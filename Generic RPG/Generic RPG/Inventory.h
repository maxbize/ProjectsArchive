#include "ItemClass.h"
#include "Weapon.h"
#include <iostream>
#pragma once

using namespace std;

class Inventory
{
public:
	Inventory();
	void addObject(Item item);
	void addObject(Weapon weapon);
	void printAllInv();
	void printItemInv();
	Item getItem(int invIndex);
	Weapon getWeapon(int invIndex);
	int getInvSize();
	int getWepAmount();
	int getItemAmount();
	void removeItem(int index);
	void removeWeapon(int index);
	void printObjectInfo(int index);

private:
	int itemAmount, weaponAmount;

protected:
	Item * itemInv;
	Weapon * weaponInv;
};