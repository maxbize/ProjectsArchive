#pragma once
#include <string>
#include <iostream>
//#include "BeingClass.h"

using namespace std;

class Item
{
public:
	Item() {}
	Item(string Name)
	{
		name = Name;
		if(name == "random")
		{
			string nameList[] = {"small health potion","medium health potion","large health potion"}; // 3 elements
			name = nameList[rand() % 3];
		}
	}

	string getName()
	{
		return name;
	}

	void printInfo()
	{
		if(name == "small health potion")
		{
			cout << "A small health potion that heals 30 health." << endl;
		}
		else if(name == "medium health potion")
		{
			cout << "A medium health potion that heals 75 health." << endl;
		}
		else if(name == "large health potion")
		{
			cout << "A large health potion that heals 150 health." << endl;
		}
	}

private:
	//string name;

protected:
	string name;
};