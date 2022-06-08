// Nervous guy who dodges you

class Enemy2 extends Enemy
{
	function onLoad()
	{
		borderSpawn();
		borderVelocity();
		health = 5;
		xAccel /= 2;
		yAccel /= 2;
	}
	
	function onEnterFrame()
	{
		if(_y < _root.player._y)
		{
			yVel -= yAccel;
		}
		else if(_y > _root.player._y)
		{
			yVel += yAccel;
		}
		
		if(_x < _root.player._x)
		{
			xVel -= xAccel;
		}
		else if(_x > _root.player._x)
		{
			xVel += xAccel;
		}
		
		_x += xVel;
		_y += yVel;
	}
}