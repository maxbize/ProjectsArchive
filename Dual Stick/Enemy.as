// Blue spinning diamond - just moves across the screen
class Enemy extends MovieClip
{
	var xVel:Number;
	var yVel:Number;
	var xAccel:Number;
	var yAccel:Number;
	var health:Number;
	var type:Number;
	
	function borderSpawn()
	{
		var spawnLocation = Math.random();
		if(spawnLocation < 0.25) // spawn left
		{
			_x = -10;
			_y = Math.random() * 380 + 10;
		}
		else if(spawnLocation < 0.5) // spawn right
		{
			_x = 560;
			_y = Math.random() * 380 + 10;
		}
		else if(spawnLocation < 0.75) // Spawn bottom
		{
			_y = 410;
			_x = Math.random() * 530 + 10;
		}
		else // spawn top
		{
			_y = -10;
			_x = Math.random() * 530 + 10;
		}
	}
	
	function borderVelocity()
	{
		xAccel = 0.1;
		yAccel = 0.1;
		
		if(_x < 0)
		{
			xVel = Math.random() * 3 + 1;
			yVel = Math.random() * 4 - 2;
		}
		else if (_x > 550)
		{
			xVel = -Math.random() * 3 - 1;
			yVel = Math.random() * 4 - 2;
		}
		else if (_y < 0)
		{
			yVel = Math.random() * 3 + 1;
			xVel = Math.random() * 4 - 2;
		}
		else
		{
			yVel = -Math.random() * 3 - 1;
			xVel = Math.random() * 4 - 2;
		}
	}
	
	function death()
	{
		switch(type)
		{
			case 1:
			this.gotoAndPlay(121);
		}
	}
}