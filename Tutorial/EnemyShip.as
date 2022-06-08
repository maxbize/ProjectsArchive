class EnemyShip extends MovieClip
{
	var speed;
	var shootTimer;
	
	function onLoad()
	{
		_x = 700;
		_y = Math.random()*200 +50;
		speed = Math.random()*5 + 5;
		shootTimer = 0;
	}
	
	function onEnterFrame()
	{
		shootTimer++;
		_x -= speed;
		
		if(_x < -100)
		{
			this.removeMovieClip();
		}
		
		if(this.hitTest(_root.ship))
		{
			explode();   
			_root.ship.updateHealth(-20);
		}
		
		if(shootTimer > 30)
		{
			shootTimer = 0;
			var missile = _root.attachMovie("EnemyMissile","EnemyMissile" + _root.getNextHighestDepth(),_root.getNextHighestDepth());
			missile._x = _x - 90;
			missile._y = _y + 2;
			
		}
	}
	
	function explode()
	{
		var explosion = _root.attachMovie( "Explosion" , "Explosion" + 	_root.getNextHighestDepth(), _root.getNextHighestDepth() );
		explosion._x = _x;
		explosion._y = _y;
		this.removeMovieClip();
		_root.ship.UpdateScore(50);
	}
}