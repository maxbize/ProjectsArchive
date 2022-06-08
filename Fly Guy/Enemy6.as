// Purple. Small, fast, doesn't hurt as much
class Enemy6 extends Enemy
{
	function onLoad()
	{
		this.spawn();
		speed += 15;
		crashDamage = 0.5;
		health = 20 * _root.flyGuy.currentLevel;
	}
	
	function onEnterFrame()
	{
		this.logic();
	}
}