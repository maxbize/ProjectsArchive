// Regular guy who flies across the screen

class Enemy1 extends Enemy
{
	function onLoad()
	{
		borderSpawn();
		borderVelocity();
		health = 100;
		type = 1;
		_rotation = Math.atan2(yVel,xVel) * 180 / 3.14;
	}
	
	function onEnterFrame()
	{
		_x += xVel;
		_y += yVel;
	}
}