// Orange Guy. Attracted to pickups
class Enemy7 extends Enemy
{
	var dx;
	var dy;
	var distance;
	
	function onLoad()
	{
		this.stop();
		this.spawn();
		health = 100 + 30 * _root.flyGuy.currentLevel;
	}

	function onEnterFrame()
	{
		this.logic();
		
		var pickup = _root.flyGuy.pickupsList[_root.flyGuy.pickupsList.length - 1];
		
		if (pickup._x > 0)
		{
			gotoAndStop(2);
			dx = pickup._x - _x;
			dy = pickup._y - _y;
			distance = Math.sqrt((dx*dx)+(dy*dy));
			_x += dx / distance * speed * 2;
			_y += dy / distance * speed * 2;
			
			if(this.hitTest(pickup))
			{
				pickup.removeMovieClip();
			}
		}
		else
		{
			gotoAndStop(1);
		}
	}
}