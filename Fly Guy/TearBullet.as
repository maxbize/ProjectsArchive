class TearBullet extends MovieClip
{
	// V^2 = Vo^2 + 2ax
	// V = Vo + at
	
	var VelY;
	var AccelY;
	
	var VelX;
	var timeTimer;
	var time;
	
	var Dir;
	
	// Need to give velocity and acceleration
	function onLoad()
	{
		timeTimer = 0;
		time = 1;
		Dir = Math.random();
			if(Dir > 0.5)
			{
				Dir = -1;
			}
			else
			{
				Dir = 1;
			}
		VelX = Math.random() * 4 + 6 * Dir;
		VelY = Math.random() * -4 - 2;
		AccelY = Math.random();
	}
	
	// Need to control movement and rotation
	function onEnterFrame()
	{
		timeTimer++;
		
		if(timeTimer > 30)
		{
			timeTimer = 0;
			time++;
		}
		
		VelY += time * AccelY;
		
		// Alter the x and y position
		_x += VelX;
		_y += VelY;
		
		_rotation = Math.atan(VelY/VelX) * 180 / 3.14159 + 90;
		if (Dir == -1)
		{
			_rotation += 180;
		}
		
		if(this.hitTest(_root.flyGuy.base))
		{
			this.removeMovieClip()
			_root.flyGuy.flightTimerIncrement *= 1.1;
		}
		if(_y > 650)
		{
			this.removeMovieClip();
		}
	}
}