class Ball extends MovieClip
{
	var xSpeed;
	var ySpeed;
	var moving;
	var aro;
	var collidingHorizontal;
	var collidingVertical;
	var accel;


	function onLoad()
	{
		setSpeed();
		spawn();
	}

	function onEnterFrame()
	{
		if (moving == 1)
		{
			moveBall();
			checkBorders();
			checkPadCollisions();
		}
		else
		{
			_xscale += 1;
			_yscale += 1;
			if (aro._currentframe == undefined)
			{
				moving = 1;
			}
		}
	}

	function setSpeed()
	{
		xSpeed = Math.random() * 3 + 3;
		ySpeed = Math.random() * 3 + 3;
		if (Math.random() > 0.5)
		{
			xSpeed *= -1;
		}
		if (Math.random() > 0.5)
		{
			ySpeed *= -1;
		}
		accel = 1.001;
	}

	function moveBall()
	{
		_x += xSpeed;
		_y += ySpeed;
		xSpeed *= accel;
		ySpeed *= accel;
	}

	function checkBorders()
	{
		if (_x < -50 || _x > 600 || _y < -50 || _y > 600)
		{
			this.removeMovieClip();
			_root.game.ballsOnScreen -= 1;
			if(_root.game.ballsOnScreen < 1)
			{
				_root.game.gameOver();
			}
			_root.game.updateCombo(-_root.game.combo/2);
		}
	}

	function checkPadCollisions()
	{
		if (this.hitTest(_root.padTop.base) || this.hitTest(_root.padBottom.base) )
		{
			if (collidingVertical == 0)
			{
				ySpeed *= -1;
				collidingVertical = 1;
				_root.game.updateCombo(1);
				_root.game.score += 50 * _root.game.combo * _root.game.ballsOnScreen * _root.game.ballsOnScreen;
				_root.soundFX.attachSound("ballBounce.wav");
				_root.soundFX.start();
			}
		}
		else
		{
			collidingVertical = 0;
		}

		if (this.hitTest(_root.padLeft.base) || this.hitTest(_root.padRight.base))
		{
			if (collidingHorizontal == 0)
			{
				xSpeed *= -1;
				collidingHorizontal = 1;
				if (_root._currentframe == 3)
				{
					_root.game.updateCombo(1);
					_root.game.score += 50 * _root.game.combo * _root.game.ballsOnScreen * _root.game.ballsOnScreen;
					_root.soundFX.attachSound("ballBounce.wav");
					_root.soundFX.start();
				}
			}
		}
		else
		{
			collidingHorizontal = 0;
		}
	}

	function spawn()
	{
		moving = -1;
		aro = _root.attachMovie("Arrow", "Arrow" + _root.getNextHighestDepth(), _root.getNextHighestDepth());
		aro._x = _x;
		aro._y = _y;
		aro._rotation = Math.atan(ySpeed / xSpeed) * 180 / 3.14;
		if (xSpeed < 0)
		{
			aro._rotation += 180;
		}
		_xscale = 25;
		_yscale = 25;
	}
}


// For angled shots
/*

if (this.hitTest(_root.padTop.base))
		{
			if (collidingVertical == 0)
			{
				var dx = _root.padTop._x - _x;
				var speed = Math.sqrt(xSpeed * xSpeed + ySpeed * ySpeed);
				xSpeed = speed * Math.sin(dx*3.14/180) * -1;
				ySpeed = speed * Math.cos(dx*3.14/180);
				collidingVertical = 1;
				_root.game.updateCombo(1);
				_root.game.score += 50 * _root.game.combo * _root.game.ballsOnScreen * _root.game.ballsOnScreen;
			}
		}
		else if(this.hitTest(_root.padBottom.base))
		{
			if (collidingVertical == 0)
			{
				dx = _root.padBottom._x - _x;
				speed = Math.sqrt(xSpeed * xSpeed + ySpeed * ySpeed);
				xSpeed = speed * Math.sin(dx*3.14/180) * -1;
				ySpeed = speed * Math.cos(dx*3.14/180) * -1;
				collidingVertical = 1;
				_root.game.updateCombo(1);
				_root.game.score += 50 * _root.game.combo * _root.game.ballsOnScreen * _root.game.ballsOnScreen;
			}
		}
		else
		{
			collidingVertical = 0;
		}
		
*/