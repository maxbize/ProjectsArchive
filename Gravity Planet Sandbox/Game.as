class Game extends MovieClip
{
	var planetsList:Array;
	var spawnTimer:Number;
	var numberOfBalls:Number;

	var test:Number;
	var collision:Number;
	var collision2:Number;
	var gravity:Number;
	var attract:Number;
	var tracers:Number;
	
	var arrowsList:Array;

	var playTime;
	
	function onLoad()
	{
		_root.pauseMenu._visible = false;
		numberOfBalls = 0;
		spawnTimer = 0;
		test = 0;
		collision = 1;
		collision2 = 1;
		gravity = -1;
		attract = 1;
		tracers = 1;
		playTime = 0;
		planetsList = [];
		arrowsList = [];
		createButtons();

		_root.kongregateServices.connect();
	}

	function onEnterFrame()
	{
		playTime++;
		spawnTimer++;
		test++;
		//trace(test);
		
		if (Key.isDown(80) && test > 10)
		{
			test = 0;
			if(_root.pauseMenu._visible == false)
			{
				pauseGame();
			}
			else
			{
				unPauseGame();
			}
		}
		if (Key.isDown(Key.SPACE))
		{
			if (spawnTimer > 30)
			{
				if(_root.pauseMenu._visible == false)
				{
					spawnPlanet();
				}

				spawnTimer = 0;


			}
		}
		if (Key.isDown(Key.ENTER))
		{
			clearPlanets();
		}
		
		//trace(planetsList[0].collisionBlacklist);
		
		for (var i in planetsList)
		{
			for (var j in planetsList)
			{
				if (i < j)
				{
					var xDistance = Math.abs(planetsList[i]._x - planetsList[j]._x);
					var yDistance = Math.abs(planetsList[i]._y - planetsList[j]._y);
					var r = Math.sqrt(xDistance * xDistance + yDistance * yDistance);
					if ( (r - planetsList[i]._xscale / 2 - planetsList[j]._xscale / 2 <= 0) && collision2 == 1 && !checkBlacklist(planetsList[i],j))
					{
						manage_bounce(planetsList[i],planetsList[j]);
						//planetsList[i].collisionBlacklist.push(j);
					}
					else if( (r - planetsList[i]._width / 2 - planetsList[j]._width / 2 > 0) && checkBlacklist(planetsList[i],j))
					{
						removeBlacklist(planetsList[i],j);
					}
				}
			}
		}
	}

	function spawnPlanet()
	{
		_root.kongregateStats.submit("Play time (min)",(Math.round(playTime / 30 / 60)));
		_root.kongregateStats.submit("Most planets on screen",planetsList.length);
		var planet = _root.attachMovie("Planet", "Planet" + _root.getNextHighestDepth(), _root.getNextHighestDepth());
		planet._x = Math.random() * 600;
		planet._y = Math.random() * 600;
		planetsList.push(planet);
		numberOfBalls++;
	}

	function manage_bounce(ball, ball2)
	{
		var dx = ball._x - ball2._x;
		var dy = ball._y - ball2._y;
		var collisionision_angle = Math.atan2(dy, dx);
		var magnitude_1 = Math.sqrt(ball.xVel * ball.xVel + ball.yVel * ball.yVel);
		var magnitude_2 = Math.sqrt(ball2.xVel * ball2.xVel + ball2.yVel * ball2.yVel);
		var direction_1 = Math.atan2(ball.yVel, ball.xVel);
		var direction_2 = Math.atan2(ball2.yVel, ball2.xVel);
		var new_xVel_1 = magnitude_1 * Math.cos(direction_1 - collisionision_angle);
		var new_yVel_1 = magnitude_1 * Math.sin(direction_1 - collisionision_angle);
		var new_xVel_2 = magnitude_2 * Math.cos(direction_2 - collisionision_angle);
		var new_yVel_2 = magnitude_2 * Math.sin(direction_2 - collisionision_angle);
		var final_xVel_1 = ((ball.mass - ball2.mass) * new_xVel_1 + (ball2.mass + ball2.mass) * new_xVel_2) / (ball.mass + ball2.mass);
		var final_xVel_2 = ((ball.mass + ball.mass) * new_xVel_1 + (ball2.mass - ball.mass) * new_xVel_2) / (ball.mass + ball2.mass);
		var final_yVel_1 = new_yVel_1;
		var final_yVel_2 = new_yVel_2;
		ball.xVel = Math.cos(collisionision_angle) * final_xVel_1 + Math.cos(collisionision_angle + Math.PI / 2) * final_yVel_1;
		ball.yVel = Math.sin(collisionision_angle) * final_xVel_1 + Math.sin(collisionision_angle + Math.PI / 2) * final_yVel_1;
		ball2.xVel = Math.cos(collisionision_angle) * final_xVel_2 + Math.cos(collisionision_angle + Math.PI / 2) * final_yVel_2;
		ball2.yVel = Math.sin(collisionision_angle) * final_xVel_2 + Math.sin(collisionision_angle + Math.PI / 2) * final_yVel_2;
	}
	
	
	//check if planet2 is in planet1's blacklist
	function checkBlacklist(planet1,planet2)
	{
		for(var i in planet1.collisionBlacklist)
		{
			if( planet1.collisionBlacklist[i] == planet2 )
			{
				return true;
			}
			else
			{
				return false;
			}
		}
	}
	
	function removeBlacklist(planet1,planet2)
	{
		for(var i in planet1.collisionBlacklist)
		{
			if( planet1.collisionBlacklist[i] == planet2 )
			{
				planet1.collisionBlacklist.splice(i,1);
				trace(planet1.collisionBlacklist);
			}
				
		}
	}
	
	function pauseGame()
	{
		_root.pauseMenu._visible = true;
		drawArrows();
	}
	
	function unPauseGame()
	{
		_root.pauseMenu._visible = false;
		removeArrows();
	}
	
	function drawArrows()
	{
		for(var i in planetsList)
		{
			var planet = planetsList[i];
			var Arrow = _root.attachMovie("VelArrow","VelArrow"+_root.getNextHighestDepth(),_root.getNextHighestDepth());
			Arrow._x = planet._x;
			Arrow._y = planet._y;
			arrowsList.push(Arrow);
			Arrow._yscale = planet._width * 2;
			Arrow._xscale = 30 * Math.sqrt(planet.xVel * planet.xVel + planet.yVel * planet.yVel);
			Arrow._rotation = 180 / 3.14 * Math.atan(planet.yVel/planet.xVel);
			if(planet.xVel < 0)
			{
				Arrow._rotation -= 180;
			}
			planetsList[i].myArrow = Arrow;
		}
	}
	
	function removeArrows()
	{
		for(var i in planetsList)
		{
			planetsList[i].readArrow();
			planetsList[i].myArrow.removeMovieClip();
		}
		
		arrowsList = [];
	}
	
	function createButtons()
	{
		var numOfButtons = 7;
		for(var i = 0; i < numOfButtons; i++)
		{
			var button = _root.attachMovie("MyButton","MyButton"+_root.getNextHighestDepth(),_root.getNextHighestDepth());
			button._y = 578;
			button._x = 50 + i * 90;
			button.setModifier(i);
			if(i > 5)
			{
				button._x = 50;
				button._y = 578 - 35 * (i - 5);
			}
			switch(i)
			{
				case 0:
				button.textBox.text = "gravity";
				break;
				case 1:
				button.textBox.text = "sticky";
				break;
				case 2:
				button.textBox.text = "collision";
				break;
				case 3:
				button.textBox.text = "repel";
				break;
				case 4:
				button.textBox.text = "planet ++";
				break;
				case 5:
				button.textBox.text = "clear";
				break;
				case 6:
				button.textBox.text = "trails";
				break;
			}
		}
	}
	
	function clearPlanets()
	{
		removeArrows();
		for (var i in planetsList)
			{
				var planet = planetsList[i];
				planet.removeMovieClip();
			}
			planetsList = [];
	}
}