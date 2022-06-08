class BulletRegen extends Bullet
{
	
	function onLoad()
	{
		this.spawn();
	}
	
	function onEnterFrame()
	{
		this.logic();
		this.hit('regen');	
	}
}