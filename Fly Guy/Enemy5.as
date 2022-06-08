// Black shield Guy. Infinite health and higher crash damage
class Enemy5 extends Enemy
{
	function onLoad()
	{
		this.spawn();
		crashDamage = 2;
		speed += 3;
		_x = _root.flyGuy._x;
	}
	
	function onEnterFrame()
	{
		this.logic();
	}
}