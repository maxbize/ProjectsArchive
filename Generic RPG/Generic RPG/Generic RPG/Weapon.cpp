// Weapon should be a child of equipment so that weapon and armor can be defined by the same class

#include "Weapon.h"

Weapon::Weapon() {}

Weapon::Weapon(string Name)
{
	name = Name;
	level = 1;
	enchants = 0;
	strBonus = 0;
	dexBonus = 0;
	hpBonus = 0;
	expBonus = 0;
	goldBonus = 0;
	damageBonus = 0;
	value = level * (enchants + 1) / 4 * 100 + rand() % 10;
	minDamage = level * 4 + damageBonus;
	maxDamage = minDamage + (3 * level) + (4 * level - 1) + damageBonus;
}

Weapon::Weapon(int lvl)
{
	level = lvl;
	enchants = 0;
	strBonus = 0;
	dexBonus = 0;
	hpBonus = 0;
	expBonus = 0;
	goldBonus = 0;
	damageBonus = 0;

	// Roll for enchants
	int enchanted;
	do
	{
		enchanted = rand() % 101 + 1;
		if(enchanted > 90)
		{
			enchants++;
		}
	}
	while (enchanted > 90);

	// Set enchants
	for(int i = 0; i < enchants; i++)
	{
		int enchant = rand() % 6;
		switch(enchant)
		{
		case 0:
			strBonus += level;
			break;
		case 1:
			dexBonus += level;
			break;
		case 2:
			hpBonus += level * 10;
			break;
		case 3:
			goldBonus += 10;
			break;
		case 4:
			expBonus += 10;
			break;
		case 5:
			damageBonus += level * 2;
		}
	}

	// Generate name
	string nameList[] = {"club", "sword", "axe", "dagger", "bow", "mace"}; // 6 elements
	string prefixList[] = {"great ", "swift ", "sturdy ", "beligerent ", "powerful "}; // 5 elements
	string suffixList[] = {" of the mason", " of the destroyer", " of the eagle", " of fire", " of meaning", " of kings"}; // 6 elements
	int pre = -1;
	int suf = -1;
	int mid = rand() % 6;
	int option = rand() % 2;
	for(int i = 0; i < enchants; i++)
	{
		switch(option)
		{
		case 0:
			pre = rand() % 5;
			break;
		case 1:
			suf = rand() % 6;
			break;
		}
		if ( enchants > 1 )
		{
			option = abs(1 - option);
		}
	}
	name = ( pre == -1 ? "" : prefixList[pre] ) + nameList[mid] + ( suf == -1 ? "" : suffixList[suf] );

	// Generate value
	value = level * (enchants + 1) / 4 * 100 + rand() % 10;

	// Find min and max damage
	minDamage = level * 4 + damageBonus;
	maxDamage = minDamage + (3 * level) + (4 * level - 1) + damageBonus;
}

int Weapon::getDamage()
{
	int damage = 0;
	for(int i = 0; i < level; i++)
	{
		damage += rand() % 4 + 1;
	}
	damage += rand() % (4 * level);
	damage += level * 3;
	damage += damageBonus;
	return damage;
}

string Weapon::getName()
{
	return name;
}

void Weapon::printInfo()
{
	cout << "Name:\t" << name << endl;
	cout << "Damage:\t" << minDamage << " to " << maxDamage << endl;
	if(strBonus > 0)
	{
		cout << "Str:\t+" << strBonus << endl;
	}
	if(dexBonus > 0)
	{
		cout << "Dex:\t+" << dexBonus << endl;
	}
	if(hpBonus > 0)
	{
		cout << "Health:\t+" << hpBonus << endl;
	}
	if(expBonus > 0)
	{
		cout << "Exp:\t+" << expBonus << "%" << endl;
	}
	if(goldBonus > 0)
	{
		cout << "Gold:\t+" << goldBonus << "%" << endl;
	}
	cout << "Value:\t" << value << " gold" << endl;
	cout << endl;
}

int Weapon::getBonus(string bonus)
{
	if(bonus == "dex")
	{
		return dexBonus;
	}
	else if (bonus == "str")
	{
		return strBonus;
	}
	else if (bonus == "health")
	{
		return hpBonus;
	}
	else if (bonus == "exp")
	{
		return expBonus;
	}
	else if (bonus == "gold")
	{
		return goldBonus;
	}
	else
	{
		cout << "ERROR! See Weapon.cpp get bonus function";
		return 0;
	}
}