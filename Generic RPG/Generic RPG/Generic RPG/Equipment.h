#include <string>

using namespace std;

class Equipment
{
public:
	string getName();
	void printInfo();
	int getBonus(string bonus);

private:
	int level, strBonus, dexBonus, hpBonus, expBonus, goldBonus, damageBonus, enchants, value;
	string name;
};