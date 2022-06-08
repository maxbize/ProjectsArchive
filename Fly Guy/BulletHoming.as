class BulletHoming extends Bullet
{
	var target;
	var dx;
	var dy;
	var distance;
	var vx;
	var vy;
	var velocity;
	var turning;

	function onLoad()
	{
		this.spawn();
		target = _root.flyGuy.enemiesList[_root.flyGuy.enemiesList.length - 1];
		vx = 0;
		vy = 0;
		turning = 5;
	}

	function onEnterFrame()
	{
		if (target._y > 0)
		{
			dx = target._x - _x;
			dy = target._y - _y;
			distance = Math.sqrt((dx * dx) + (dy * dy));
			dx /= distance;
			dy /= distance;
			
			vx += dx * turning;
			vy += dy * turning;

			velocity = Math.sqrt((vx * vx) + (vy * vy));

			if (velocity > Speed)
			{
				vx = (vx * Speed) / velocity;
				vy = (vy * Speed) / velocity;
			}

			_x += vx;
			_y += vy;
			
			_rotation = Math.atan(vy/vx) * 180 / 3.14159 + 90;
			
			if(vx < 0)
			{
				_rotation += 180;
			}
		}
		else
		{
			_y -= Speed;
			_rotation = 0;
		}
		this.hit('homing');
		
		if( _y < -50 )
		{
			this.removeMovieClip();
		}
	}
}