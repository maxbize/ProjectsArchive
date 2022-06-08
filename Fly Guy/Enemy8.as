// Cry Guy
class Enemy8 extends Enemy
{
	var shootTimer;
	
	function onLoad()
	{
		this.spawn();
		health = 100 + 40 * _root.flyGuy.currentLevel;
		shootTimer = 0;
	}
	
	function onEnterFrame()
	{
		this.logic();
		shootTimer++;
		
		if (shootTimer > 10)
		{
			shootTimer = 0;
			var tear = _root.attachMovie("TearBullet", "TearBullet" + _root.getNextHighestDepth(), _root.getNextHighestDepth())
			tear._x = this._x;
			tear._y = this._y;
			_root.flyGuy.bulletsList.push(tear);
		}
	}
}