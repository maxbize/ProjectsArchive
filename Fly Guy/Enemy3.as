// Green Guy. Flies towards Fly Guy
class Enemy3 extends Enemy
{
	var attackSpeed;
	
	function onLoad()
	{
		this.spawn();
		attackSpeed = 3;
		health = level * 40;
	}
	
	function onEnterFrame()
	{
		this.logic();
		this.seekObject(_root.flyGuy,3);
	}
}