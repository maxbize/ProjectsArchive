class Game extends MovieClip
{
	var fieldList:Array;
	var particleList:Array;
	
	var listener;
	
	function onLoad()
	{
		listener = 0;
		fieldList = [];
		particleList = [];
	}
	
	function onEnterFrame()
	{
		listener += 1;
		if(Key.isDown(Key.SPACE) && listener > 5)
		{
			var p = _root.attachMovie("Particle","Particle"+_root.getNextHighestDepth(),_root.getNextHighestDepth());
			p._x = Math.random()*500;
			p._y = Math.random()*500;
			particleList.push(p);
			listener = 0;
		}
		if(Key.isDown(Key.UP))
		{
			clearFields();
		}
		checkCollisions();
		_root.arrayCount.text = particleList.length;
		checkExits();
	}
	
	function checkCollisions()
	{
		for (var i = 0; i < particleList.length; i++)
		{
			for( var j = (i + 1); j < particleList.length; j++)
			{
				//trace("i: " + i + "\t j: " + j);
				
				// p1, p2 = particle1, particle2
				var p1 = particleList[i];
				var p2 = particleList[j];
				
				var dx = p2._x - p1._x;
				var dy = p2._y - p1._y;
				var dist = Math.sqrt(dx * dx + dy * dy);
				
				if(dist <= p1._width)
				{
					manage_bounce(p1,p2);
				}
			}
		}
	}
	
	function manage_bounce(ball, ball2)
	{
		var dx = ball._x - ball2._x;
		var dy = ball._y - ball2._y;
		var collisionision_angle = Math.atan2(dy, dx);
		var magnitude_1 = Math.sqrt(ball.velocity_x * ball.velocity_x + ball.velocity_y * ball.velocity_y);
		var magnitude_2 = Math.sqrt(ball2.velocity_x * ball2.velocity_x + ball2.velocity_y * ball2.velocity_y);
		var direction_1 = Math.atan2(ball.velocity_y, ball.velocity_x);
		var direction_2 = Math.atan2(ball2.velocity_y, ball2.velocity_x);
		var new_velocity_x_1 = magnitude_1 * Math.cos(direction_1 - collisionision_angle);
		var new_velocity_y_1 = magnitude_1 * Math.sin(direction_1 - collisionision_angle);
		var new_velocity_x_2 = magnitude_2 * Math.cos(direction_2 - collisionision_angle);
		var new_velocity_y_2 = magnitude_2 * Math.sin(direction_2 - collisionision_angle);
		//var final_velocity_x_1 = (new_velocity_x_2);
		//var final_velocity_x_2 = (new_velocity_x_1);
		//var final_velocity_y_1 = new_velocity_y_1;
		//var final_velocity_y_2 = new_velocity_y_2;
		ball.velocity_x = (Math.cos(collisionision_angle) * new_velocity_x_2 + Math.cos(collisionision_angle + Math.PI / 2) * new_velocity_y_1) * 0.99;
		ball.velocity_y = (Math.sin(collisionision_angle) * new_velocity_x_2 + Math.sin(collisionision_angle + Math.PI / 2) * new_velocity_y_1) * 0.99;
		ball2.velocity_x = (Math.cos(collisionision_angle) * new_velocity_x_1 + Math.cos(collisionision_angle + Math.PI / 2) * new_velocity_y_2) * 0.99;
		ball2.velocity_y = (Math.sin(collisionision_angle) * new_velocity_x_1 + Math.sin(collisionision_angle + Math.PI / 2) * new_velocity_y_2) * 0.99;
	
		ball.collisionAnim();
		ball2.collisionAnim();
	}
	
	function clearFields()
	{
		for (var i in fieldList)
		{
			var field = fieldList[i];
			field.removeMovieClip();
		}
		fieldList = [];
	}
	
	//Spawns a box of balls. x and y define the top left corner
	//Acceptable amounts are 1, 4, 9, 16, 25
	function spawnBox(xSpawn,ySpawn,amount)
	{
		var xStart = xSpawn;
		var offset = 15; //Sets how much each ball offsets from the last
		var rows = Math.sqrt(amount);
		for(var i = 0; i < rows; i++)
		{
			if(i % 2 != 0)
			{
				xSpawn -= offset / 2;
			}
			for (var j = 0; j < rows; j++)
			{
				var spawn = _root.attachMovie("Particle","Particle"+_root.getNextHighestDepth(),_root.getNextHighestDepth());
				spawn._x = xSpawn;
				spawn._y = ySpawn;
				particleList.push(spawn);
				xSpawn += offset;
			}
			xSpawn = xStart;
			ySpawn += offset;
		}
	}
	
	function levelComplete()
	{
		var offset = 3;
		var menu = 2;
		clearFields();
		Mouse.show();
		_root.customCursor.removeMovieClip();
		if(_root.saveBox.highestLevelBeaten < _root._currentframe - offset)
		{
			_root.saveBox.highestLevelBeaten = _root._currentframe - offset;
		}
		_root.gotoAndStop(menu);
		_root.saveBox.saveGame();
	}
	
	function putBallsOnTop()
	{
		for(var i in particleList)
		{
			particleList[i].swapDepths(_root.getNextHighestDepth());
		}
	}
	
	function checkExits()
	{
		for(var i in particleList)
		{
			if(particleList[i].hitTest(_root.Exit)) // Run an easier hit test first for efficiency
			{
				if(_root.Exit.hitTest(particleList[i]._x,particleList[i]._y,true) && particleList[i].existance > 97)
				{
					particleList[i].flashAnim(2);
					particleList[i].removeMovieClip();
					refreshParticleArray(i);
				}
			}
		}
	}
	
	function refreshParticleArray(i)
	{
		particleList.splice(i,1);
		if(particleList.length == 0)
		{
			levelComplete();
		}
	}
}