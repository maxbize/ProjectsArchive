class OneWayWall extends MovieClip
{
	function onEnterFrame()
	{
		checkCollisions();
	}
	
	function checkCollisions()
	{
		//Hit test 3. Test for direction it's coming from and alter velocity only if the velocity is not pushing the ball away
		
		for(var i in _root.game.particleList)
		{
			if(this.hitTest(_root.game.particleList[i]))
			   {
				   var dy = _root.game.particleList[i]._y - _y; // negative if above
				   var dx = _root.game.particleList[i]._x - _x;
				   var offsety = _root.game.particleList[i].velocity_y + dy - _height / 2; // Offset boundary box for flip
				   var offsetx = _root.game.particleList[i].velocity_x + dx - _height / 2;
				   if(dy < -_height / 2 - offsety && _root.game.particleList[i].velocity_y > 0) // Hitting from above
				   {
					   _root.game.particleList[i].velocity_y *= -1.1;
				   }
				   else if(dy > _height / 2 + offsety && _root.game.particleList[i].velocity_y < 0) // Hitting from above
				   {
					   _root.game.particleList[i].velocity_y *= -1.1;
				   }
				   if(dx < _width / 2 + offsetx && _root.game.particleList[i].velocity_x < 0)
				   {
					   _root.game.particleList[i].velocity_x *= -1.1;
				   }
				   else if(dx > -_width / 2 - offsetx && _root.game.particleList[i].velocity_x > 0)
				   {
					   _root.game.particleList[i].velocity_x *= -1.1;
				   }
			   }
		}
	}
}