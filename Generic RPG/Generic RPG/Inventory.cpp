#include "Inventory.h"

//Array size must start at 1 and have a blank 0th element because C++ does not support 0 element arrays
Inventory::Inventory()
{
	weaponAmount = 1;
	itemAmount = 1;
	itemInv = new Item[itemAmount];
	weaponInv = new Weapon[weaponAmount];
}

// Contains memory leak. Need to delete the old inv but C++ won't let me do delete inv
void Inventory::addObject(Item item)
{
	int newSize = itemAmount + 1;
	Item * newInv = new Item[newSize];
	for (int i = 0; i < itemAmount; i++)
	{
		newInv[i] = itemInv[i];
	}
	newInv[itemAmount] = item;
	itemAmount++;
	delete[] itemInv;
	itemInv = newInv;
}

void Inventory::addObject(Weapon weapon)
{
	int newSize = weaponAmount + 1;
	Weapon * newInv = new Weapon[newSize];
	for (int i = 0; i < weaponAmount; i++)
	{
		newInv[i] = weaponInv[i];
	}
	newInv[weaponAmount] = weapon;
	weaponAmount++;
	delete[] weaponInv;
	weaponInv = newInv;
}

void Inventory::printAllInv()
{
	cout << endl << " - - = Inventory = - - " << endl;
	for (int i = 1; i < itemAmount; i++)
	{
		cout << "  " << i << ": " << itemInv[i].getName() << endl;
	}
	for (int i = 1; i < weaponAmount; i++)
	{
		cout << "  " << i + itemAmount - 1 << ": " << weaponInv[i].getName() << endl;		
	}
	if(itemAmount + weaponAmount == 2)
	{
		cout << "\tEmpty =(" << endl;
	}
	cout << endl;
}

void Inventory::printItemInv()
{
	cout << endl << " - - = Items = - - " << endl;
	for (int i = 1; i < itemAmount; i++)
	{
		cout << "  " << i << ": " << itemInv[i].getName() << endl;
	}
	if(itemAmount == 1)
	{
		cout << "\tNo usable items =(" << endl;
	}
	cout << endl;
}

Item Inventory::getItem(int invIndex)
{
	if(invIndex <= itemAmount)
	{
		return itemInv[invIndex];
	}
}

Weapon Inventory::getWeapon(int invIndex)
{
	if(invIndex <= weaponAmount)
	{
		return weaponInv[invIndex];
	}
}	

// Currently returns item inv only
int Inventory::getInvSize()
{
	return itemAmount;// + weaponAmount;
}

void Inventory::removeWeapon(int index)
{
	int newIndex = 0;
	if (index <= weaponAmount)
	{
		int newSize = weaponAmount - 1;
		Weapon * newInv = new Weapon[newSize];
		for (int i = 0; i < weaponAmount; i++)
		{
			if(i != index)
			{
				newInv[newIndex] = weaponInv[i];
				newIndex++;
			}
		}
		weaponAmount--;
		delete[] weaponInv;
		weaponInv = newInv;
	}
}

void Inventory::removeItem(int index)
{
	int newIndex = 0;
	if(index <= itemAmount)
	{
		int newSize = itemAmount - 1;
		Item * newInv = new Item[newSize];
		for (int i = 0; i < itemAmount; i++)
		{
			if(i != index)
			{
				newInv[newIndex] = itemInv[i];
				newIndex++;
			}
		}
		itemAmount--;
		delete[] itemInv;
		itemInv = newInv;
	}
}

int Inventory::getWepAmount()
{
	return weaponAmount;
}

int Inventory::getItemAmount()
{
	return itemAmount;
}

void Inventory::printObjectInfo(int index)
{
	if(index < itemAmount)
	{
		itemInv[index].printInfo();
	}
	else if (index < itemAmount + weaponAmount)
	{
		index -= itemAmount - 1;
		weaponInv[index].printInfo();
	}
}