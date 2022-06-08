#include "BeingClass.h"

void gotoHealer(Being & player)
{
	int heal = player.getMAXhealth() - player.getHealth();
	int cost = (double)heal * 0.8;
	cout << endl << "Welcome to the healer. " << endl;
	if(player.getHealth() != player.getMAXhealth())
	{
		cout << " It will cost " << cost << " gold to heal " << heal << " health points" << endl;
		cout << " You currently have " << player.getGold() << " gold";
		cout << " Pay? (Y)es / (N)o : ";
		char input;
		cin >> input;
		if (input == 'y')
		{
			if (player.getGold() >= cost)
			{
				player.loseHealth(-heal);
				player.gainGold(-cost);
				cout << " You have been healed" << endl << endl;
			}
			else
			{
				cout << " You don't have enough money" << endl << endl;
			}
		}
		else
		{
			cout << endl;
		}
	}
	else
	{
		cout << " You already have full health" << endl << endl;
	}
}