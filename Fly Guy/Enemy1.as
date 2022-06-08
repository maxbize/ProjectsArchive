class Enemy1 extends Enemy // Yellow sad guy
{
	var avoidSpeed;
	
	function onLoad()
	{
		this.spawn();
		avoidSpeed = 2;
		this.health = 100 + level * 50;
	}
	
	function onEnterFrame()
	{
		this.logic();
		
		if( _x > _root.flyGuy._x && _x < _root.flyGuy._x + 100 )
		{
			_x += avoidSpeed;
		}
		
		if( _x < _root.flyGuy._x && _x > _root.flyGuy._x - 100 )
		{
			_x -= avoidSpeed;
		}
	}
}

/*var speed;
	var avoidSpeed;
	var level;
	var health;
	
	function onLoad()
	{
		level = _root.flyGuy.enemyLevel;
		avoidSpeed = 2;
		speed = Math.random()*5 + 2;
		_x = Math.random()*400 + 50;
		_y = -100;
		health = 100 + level * 50;
	}
	
	function onEnterFrame()
	{
		_y += speed
		
		if(_y > 600)
		{
			this.removeMovieClip();
		}
		
		if( _x > _root.flyGuy._x && _x < _root.flyGuy._x + 100 )
		{
			_x += avoidSpeed;
		}
		
		if( _x < _root.flyGuy._x && _x > _root.flyGuy._x - 100 )
		{
			_x -= avoidSpeed;
		}
		
		if(_x < 30) {_x = 30}
		if(_x > 470) {_x = 470}
		
		if(this.hitTest(_root.flyGuy.base))
		{
			this.removeMovieClip();
			_root.flyGuy.health -= 20;
			_root.flyGuy.flightTimerIncrement += 10;
		}
		
		if(health < 1)
		{
			var pickup = Math.round(Math.random());
			switch(pickup)
			{
				case 0:
					pickup = _root.attachMovie("FlightOrb","FlightOrb" + _root.getNextHighestDepth(), _root.getNextHighestDepth());
					break;
				case 1:
					pickup = _root.attachMovie("Coin","Coin" + _root.getNextHighestDepth(), _root.getNextHighestDepth());
					break;
			}
			pickup._x = _x;
			pickup._y = _y;
			_root.flyGuy.pickupsList.push(pickup);
			_root.flyGuy.kills++;
			this.removeMovieClip();
		}
	}*/