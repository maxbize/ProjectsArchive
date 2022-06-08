class Wall extends MovieClip
{
	var insideWall; // Var declared but never defined. Need this to bypass error for inside wall hit detection
	
	function onEnterFrame()
	{
		checkCollisions();
	}
	
	function checkCollisions()
	{
		//Hit test 3. Test for direction it's coming from and alter velocity only if the velocity is not pushing the ball away
		// Also tests for inner wall hit
		for(var i in _root.game.particleList)
		{
			if(this.hitTest(_root.game.particleList[i]))
			   {
				   var dy = _root.game.particleList[i]._y - _y; // positive if particle below wall
				   var dx = _root.game.particleList[i]._x - _x; // positive if particle to the right of wall
				   var offsety = _root.game.particleList[i].velocity_y + _root.game.particleList[i].accel_y; //+ dy - _height / 2; // Offset boundary box for flip
				   var offsetx = _root.game.particleList[i].velocity_x + _root.game.particleList[i].accel_x; //+ dx - _height / 2;
				   if(dy < -_height / 2 + offsety && _root.game.particleList[i].velocity_y > 0) // Hitting from above
				   {
					   _root.game.particleList[i].velocity_y *= -0.95;
					  // _root.game.particleList[i]._y = -1*(_y + _height + _root.game.particleList[i]._height) / 2
				   }
				   else if(dy > _height / 2 + offsety && _root.game.particleList[i].velocity_y < 0) // Hitting from above
				   {
					   _root.game.particleList[i].velocity_y *= -0.95;
					  // _root.game.particleList[i]._y = (_y - _height - _root.game.particleList[i]._height) / 2
				   }
				   //Hitting from left
				   else if(dx < -_width / 2 + offsetx && _root.game.particleList[i].velocity_x > 0)
				   {
					   _root.game.particleList[i].velocity_x *= -0.95;
					   //_root.game.particleList[i]._x = -1*(_width + _root.game.particleList[i]._width) / 2
				   }
				   //Hitting from right
				   else if(dx > (_width / 2 + offsetx) && _root.game.particleList[i].velocity_x < 0)
				   {
					   _root.game.particleList[i].velocity_x *= -0.95;
					   //_root.game.particleList[i]._x = (_width + _root.game.particleList[i]._width) / 2
				   }
				   
				   if(this.insideWall.hitTest(_root.game.particleList[i]) && _root.game.particleList[i].accel_x != 0)
				   {
					   _root.game.particleList[i].respawn();
					   _root.game.particleList[i].existance = 0;
				   }
			   }
		}
	}
}