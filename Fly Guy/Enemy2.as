class Enemy2 extends Enemy
{
	function onLoad()
	{
		this.spawn();
		this.health = 100 + level * 20;
	}
	
	function onEnterFrame()
	{
		this.logic();
	}
}