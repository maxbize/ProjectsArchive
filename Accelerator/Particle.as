class Particle extends MovieClip
{
	var accel_x;
	var accel_y;
	
	var velocity_x;
	var velocity_y;
	
	var existance; // Handles various game elements
	
	function onLoad()
	{
		velocity_x = 0;
		velocity_y = 0;
		existance = 100;
	}
	
	function onEnterFrame()
	{
		//checkExits(); // Now handled by the game class
		checkFields();
		moveParticle();
		checkBoundaries();
		limitSpeed();
		existance++;
		_alpha = existance;
	}
	
	function checkFields()
	{
		accel_x = 0;
		accel_y = 0;
		
		for(var i in _root.game.fieldList)
		{
			var field = _root.game.fieldList[i];
			if(field.hitTest(_x,_y,true))
			{
				accel_x += (field._xscale / 400) * Math.cos(field._rotation * 0.0174532925);
				accel_y += (field._xscale / 400) * Math.sin(field._rotation * 0.0174532925);
			}
		}
	}
	
	function moveParticle()
	{
		velocity_x += accel_x;
		velocity_y += accel_y;
		
		velocity_y += .02; // gravity
		
		_x += velocity_x;
		_y += velocity_y;
	}
	
	function checkBoundaries()
	{
		if(_x < 10)
		{
			velocity_x *= -0.8;
			_x = 10.1;
		}
		if(_x > 490)
		{
			velocity_x *= -0.8;
			_x = 489.9;
		}
		if(_y < 10)
		{
			velocity_y *= -0.8;
			_y = 10.1;
		}
		if(_y > 490)
		{
			velocity_y *= -0.8;
			_y = 489.9;
		}
	}
	
	function limitSpeed()
	{
		{
			var limit = 6;
			
			if(velocity_x > limit || velocity_x < -limit || velocity_y > limit || velocity_y < -limit)
			{
				velocity_x *= 0.95;
				velocity_y *= 0.95;
			}
		}
	}
	
	function refreshParticleArray()
	{
		for(var i = _root.game.particleList.length; i >= 0; i--)
		{
			if(_root.game.particleList[i]._x == _x && _root.game.particleList[i]._y)
			{
				_root.game.particleList.splice(i,1);
			}
		}
		if(_root.game.particleList.length == 0)
		{
			_root.game.levelComplete();
		}
	}
	
	function respawn()
	{
		flashAnim(1);
		_x = _root.respawn._x;
		_y = _root.respawn._y;
		_root.respawn.gotoAndPlay(1);
	}
	
	function flashAnim(anim)
	{
		var clip;
		switch(anim)
		{
			case 1:
				clip = _root.attachMovie("respawnFlash","respawnFlash"+_root.getNextHighestDepth(),_root.getNextHighestDepth());
				break;
			case 2:
				clip = _root.attachMovie("Flash2","Flash2"+_root.getNextHighestDepth(),_root.getNextHighestDepth());
				break;
		}
		clip._x = _x;
		clip._y = _y;
	}
	
	function collisionAnim()
	{
		if(existance > 0)
		{
			existance = (existance > 100 ? 100 : existance);
			existance -= 5;
			var clip = _root.attachMovie("CollisionAnimation","CollisionAnimation"+_root.getNextHighestDepth(),_root.getNextHighestDepth())
			clip._x = _x;
			clip._y = _y;
			clip._xscale = existance;
			clip._yscale = clip._xscale;
		}
		else( existance = 20);
	}
}