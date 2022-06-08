#include "BeingClass.h"

Being createEnemy(int lvl)
{
	int str = lvl * 3;
	int dex = lvl * 3;
	int hp = lvl * 30;
	string nameList[] = {"Ogre","rat","Dwarf","mage","spider"}; // 5 elements
	string name = nameList[rand() % 5];
	Being enemy(str,dex,hp,lvl, name);

	return enemy;
}