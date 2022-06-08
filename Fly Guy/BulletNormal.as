class BulletNormal extends Bullet
{
	function onLoad()
	{
		this.spawn();
	}
	
	function onEnterFrame()
	{
		this.logic();
		this.hit('normal');
	}
}