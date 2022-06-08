#include "BeingClass.h"
#include "MyFunctions.h"

using namespace std;

void playerTurn();
void enemyTurn();
void loseBattle();
void attack(Being &, Being &);
void winBattle();
void useItem();

char input;
int state;

Being player, enemy;

// Battle uses a finite state machine structure
void battle(Being & getPlayer, Being & getEnemy)
{
	state = 1;
	player = getPlayer;
	enemy = getEnemy;

	cout << endl;
	cout << "-- You encountered a level " << enemy.getLevel() << " " << enemy.getName() << "! --" << endl << endl;

	while (state != 0)
	{
		switch (state)
		{
		case 1:
			playerTurn();
			break;
		case 2:
			enemyTurn();
			break;
		case 3:
			loseBattle();
			break;
		case 4:
			winBattle();
			break;
		case 5:
			useItem();
			break;
		}
	}
	getPlayer = player;
};

void playerTurn()
{
	cout << "Your turn: (a)ttack, use (i)tem, or (f)lee? : ";
	cin >> input;
	cout << endl;
	switch(toupper(input))
	{
	case 'A':
		attack(player,enemy);
		if(enemy.getHealth() > 0) { state = 2; }
		else (state = 4);
		break;
	case 'F':
		state = 3;
		break;
	case 'I':
		state = 5;
		break;
	default:
		errorText();
		break;
	}

}

void enemyTurn()
{
	attack(enemy,player);
	if(player.getHealth() > 0) { state = 1; }
	else (state = 3);
}

void attack( Being & attacker, Being & defender)
{
	int damage = attacker.getDamage();
	defender.loseHealth(damage);
	cout << attacker.getName() << " attacked " << defender.getName() << " for " << damage << " damage points!" << endl;
	cout << "  " << defender.getName() << " has " << defender.getHealth() << " health left" << endl << endl;
}

void loseBattle()
{
	cout << "-+-+-  Battle lost!!  -+-+-" << endl << endl;
	player.gainExp(-player.getExp() / 2 , true);
	player.loseHealth((-player.getMAXhealth() / 2) + player.getHealth());
	state = 0;
}

void winBattle()
{
	cout << "Battle won!!" << endl << endl;
	player.gainGold(enemy.getGold());
	cout << "You got " << enemy.getGold() << " gold." << endl;
	int chance = rand() % 101 + 1;
	if(chance > 85)
	{
		Item itemFound("random");
		player.obtainObject(itemFound);
		cout << "You found a " << itemFound.getName() << "." << endl;
	}
	else if (chance <= 10)
	{
		Weapon weaponFound(enemy.getLevel());
		player.obtainObject(weaponFound);
		cout << "You found a " << weaponFound.getName() << "." << endl;
	}
	player.gainExp(enemy.getExp() , true);
	state = 0;
}

void useItem()
{
	cout << "Which item would you like to use? (0 to quit) : " << endl;
	player.displayUsableInventory();
	int choice;
	cout << " > " ;
	cin >> choice;
	cout << endl;
	if(choice > 0 && choice < player.getInventorySize() && player.getInventorySize() != 1)
	{
		player.useItem(choice);
		state = 2;
	}
	else if(choice == 0)
	{
		state = 1;
	}
	else
	{
		cout << "You wish you had an item there" << endl << endl;
	}
}