#include <iostream>
#include <time.h>
#include "BeingClass.h"
#include "Weapon.h"
#include "MyFunctions.h"

using namespace std;

// What needs to be added to the game:
//  -Shop
//  -Armor

// What needs to be changed in the game:
//  -Balancing

// Current temporary test changes:
//  -Weapon drop rate
//  -Weapon enchant rate

int main()
{
	char input;
	srand(time(NULL));

	cout << "--- Welcome to Max's generic RPG, \n created to give him experience with arrays and classes! ---" << endl << endl;

	Being player(5,5,50, 1, "Max");
	player.gainExp(10000,false);
	
	cout << "You found a weapon on the ground!" << endl << endl;

	while(player.getHealth() > 0)
	{
		Item testItem("small health potion");
		Item testItem2("medium health potion");		
		cout << "(F)ight, (S)tats, (I)nventory, (P)riest, (E)quipment, (T)est ... " ;
		cin >> input;
		switch(toupper(input))
		{
		case 'F':
		{
			Being enemy = createEnemy(player.getLevel() - 1);
			battle(player, enemy);
			break;
		}
		case 'S':
			player.displayStats();
			break;
		case 'I':
			player.displayInventory();
			break;
		case 'T':
			player.obtainObject(testItem);
			player.obtainObject(testItem2);
			break;
		case 'P':
			gotoHealer(player);
			break;
		case 'E':
			player.displayEquipment();
			break;
		default :
			errorText();
			break;
		}
	}
	
	return 0;
}