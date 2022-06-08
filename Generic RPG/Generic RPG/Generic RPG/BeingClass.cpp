#include "BeingClass.h"

using namespace std;

Being::Being() {}

Being::Being(int str, int dex, int hp, int lvl, string Name)
{
	Weapon startingWep(lvl);
	equippedWeapon = startingWep;
	strength = str;
	dexterity = dex;
	health = hp;
	level = lvl;
	experience = lvl * 20 + rand() % lvl * 10 + rand() % 10;
	MAXhealth = health;
	expReq = lvl * 100;
	name = Name;
	gold = lvl * 20 + rand() % (lvl * 10) + rand() % 10;
}

void Being::displayStats()
{
	cout << endl << "- - - STATS - - -" << endl;
	cout << " Level:\t\t" << level << endl;
	cout << " Dexterity:\t" << dexterity << endl;
	cout << " Strength:\t" << strength << endl;
	cout << " Health:\t" << health << " / " << MAXhealth << endl;
	cout << " Gold:\t\t" << gold << endl;
	cout << " Experience: \t" << experience << " / " << expReq << endl << endl;
}

void Being::gainExp(int exp, bool displayText)
{
	if (exp > 0 && displayText)
	{
	cout << "You gained " << exp << " experience." << endl << endl;
	}
	else if (exp < 0 && displayText)
	{
		cout << "You lost " << -exp << " experience." << endl << endl;
	}
	experience += exp;
	if(experience >= expReq)
	{
		levelUp();
	}
}

int Being::getDexterity()
{
	return dexterity;
}

int Being::getStrength()
{
	return strength;
}

int Being::getHealth()
{
	return health;
}

int Being::getLevel()
{
	return level;
}

int Being::getExp()
{
	return experience;
}

int Being::getMAXhealth()
{
	return MAXhealth;
}

void Being::loseHealth( int damage )
{
	health -= damage;
	if(health > MAXhealth)
	{
		health = MAXhealth;
	}
	else if(health < 0)
	{
		health = 0;
	}
}

string Being::getName()
{
	return name;
}

void Being::levelUp()
{
	level++;
	int points = 3;
	char input;
	experience = 0;
	expReq = level * 200;
	cout << "-+-+-  You leveled up!!  -+-+- " << endl;
	while(points > 0)
	{
		cout << endl <<"You have " << points << (points > 1 ? " points" : " point") << " to spend. Put one in:" << endl;
		cout << " > (D)exterity (" << dexterity << ")" << endl;
		cout << " > (S)trength (" << strength << ")" << endl;
		cout << " > (H)ealth (" << MAXhealth << ")" << endl;
		cout << " >>> ";
		cin >> input;
		switch(toupper(input))
		{
		case 'D':
			dexterity++;
			points--;
			break;
		case 'S':
			strength++;
			points--;
			break;
		case 'H':
			MAXhealth += 25;
			points--;
			break;
		default:
			cout << "You can't upgrade that stat..." << endl;
		}
	}
	cout << endl;
	health = MAXhealth;
}

void Being::displayInventory()
{
	inventory.printAllInv();
	if(inventory.getItemAmount() + inventory.getWepAmount() > 2)
	{
		cout << "Learn more about (0 to quit): " ;
		int input;
		cin >> input;
		if(input < inventory.getItemAmount() && input > 0)
		{
			inventory.printObjectInfo(input);
			cout << "Use item? Y/N : ";
			char choice;
			cin >> choice;
			cout << endl;
			if(toupper(choice) == 'Y')
			{
				useItem(input);
			}
		}
		else if (input < inventory.getItemAmount() + inventory.getWepAmount() - 1 && input > 0)
		{
			inventory.printObjectInfo(input);
			cout << "Equip? Y/N : ";
			char choice;
			cin >> choice;
			cout << endl;
			if(toupper(choice) == 'Y')
			{
				equipWeapon(input - inventory.getItemAmount() + 1);
			}
		}
		else if (input != 0)
		{
			cout << "An empty space in your backpack..." << endl << endl;
		}
		else
		{
			cout << endl;
		}
	}
}

void Being::displayUsableInventory()
{
	inventory.printItemInv();
}

void Being::obtainObject(Item theItem)
{
	inventory.addObject(theItem);
}

void Being::obtainObject(Weapon theWeapon)
{
	inventory.addObject(theWeapon);
}

void Being::useItem(int invIndex)
{
	Item item = inventory.getItem(invIndex);
	if(item.getName() == "small health potion")
	{
		int heal = 30;
		cout << name << " used a " << item.getName() << " and recovered " << heal << " health." << endl;
		loseHealth(-heal);
	}
	else if(item.getName() == "medium health potion")
	{
		int heal = 75;
		cout << name << " used a " << item.getName() << " and recovered " << heal << " health." << endl;
		loseHealth(-heal);
	}
	else if(item.getName() == "large health potion")
	{
		int heal = 150;
		cout << name << " used a " << item.getName() << " and recovered " << heal << " health." << endl;
		loseHealth(-heal);
	}
	cout << endl;
	inventory.removeItem(invIndex);
}

int Being::getInventorySize()
{
	return inventory.getInvSize();
}

int Being::getGold()
{
	return gold;
}

void Being::gainGold(int amount)
{
	gold += amount;
}

void Being::equipWeapon(int invSlot)
{
	Weapon oldWep = equippedWeapon;
	equippedWeapon = inventory.getWeapon(invSlot);
	inventory.removeWeapon(invSlot);
	inventory.addObject(oldWep);
	cout << name << " equipped a " << equippedWeapon.getName() << endl << endl;
}

int Being::getDamage()
{
	int damage = equippedWeapon.getDamage();
	damage *= 1 + ((double)strength / 10);
	return damage;
}

void Being::displayEquipment()
{
	cout << endl;
	cout << " - - = Equipment = - - " << endl;
	cout << " 1) Weapon: " << equippedWeapon.getName() << endl;

	cout << endl << "More info on (0 to quit) : " ;
	int choice;
	cin >> choice;
	switch(choice)
	{
	case 0:
		cout << endl;
		break;
	case 1:
		equippedWeapon.printInfo();
		break;
	default:
		cout << "You don't have any equipment there..." << endl;
		break;
	}
}