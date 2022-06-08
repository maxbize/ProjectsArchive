class BulletSplitter extends Bullet
{
	var power;
	var speedx;

	function onLoad()
	{
		this.spawn();
	}

	function onEnterFrame()
	{
		this.logic();
		this.hit('splitter');
		_x += speedx;
	}

}