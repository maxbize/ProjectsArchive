import flash.utils.getTimer;
class Planet extends MovieClip
{
	var radius:Number;
	var mass:Number;

	var xAccel:Number;
	var yAccel:Number;
	var xVel:Number;
	var yVel:Number;
	var spin:Number;
	var fixed:Boolean;
	var dragging:Boolean;
	var dragOffsetx:Number;
	var dragOffsety:Number;
	var dragStartTime:Number;
	
	var myArrow;

	var Gconst:Number = 0.0000000000667;

	var collision:Number;

	var collisionBlacklist:Array;// Is it colliding already?

	function onLoad()
	{
		fixed = false;
		dragging = false;
		spin = Math.random() * 6 - 3;
		var frame = Math.ceil(Math.random() * 6);
		gotoAndStop(frame);
		_rotation = Math.random() * 360
		
		collisionBlacklist = [];
		
		radius = Math.random();
		if(radius < 0.21)
		{
			radius = 0.2;
		}
		mass = 500000000000000;//Math.random() * 1;
		mass *= radius;

		_xscale *= radius;
		_yscale *= radius;

		xAccel = 0;
		yAccel = 0;
		xVel = Math.random() * 5;
		yVel = Math.random() * 5;
	}

	function onEnterFrame()
	{
		if(_root.pauseMenu._visible == false)
		{
		calculateAcceleration();
		bounceOnSides();
		speedLimit(10);
		drawTracer();
		checkDragging();

		xVel += (_root.game.attract == 1 ? xAccel : -xAccel);
		yVel += (_root.game.attract == 1 ? yAccel : -yAccel);

		if(!fixed)
		{
			_x += xVel;
			_y += yVel;
		}
		
		//_rotation += spin;
		}
	}

	function calculateAcceleration()
	{
		// F = Gmm/r^2
		// F = ma
		var force:Number;
		var Fx:Number;
		var Fy:Number;
		var FxSum:Number;
		var FySum:Number;
		var r:Number;
		var dx:Number;
		var dy:Number;
		var theta:Number;
		var otherxVel:Number;
		var otheryVel:Number;
		
		FxSum = 0;
		FySum = 0;

		// Get all the Fx and Fy
		for (var i in _root.game.planetsList)
		{
			var otherPlanet = _root.game.planetsList[i];
			// Make sure it's not calculating gravity with itself
			if (_x != otherPlanet._x && _y != otherPlanet._y)
			{

				dx = otherPlanet._x - _x;
				dy = _y - otherPlanet._y;
				if (dx != 0)
				{
					theta = Math.atan(dy / dx);// radians
				}
				else
				{
					theta = 3.14159 / 2;
				}
				if (dx < 0)
				{
					theta += 3.14159;
				}
				r = Math.sqrt(dx * dx + dy * dy);
				if (_root.game.gravity == -1)
				{
					force = Gconst * mass * otherPlanet.mass / r / r;
				}
				else
				{
					force = 0;
				}
				Fx = force * Math.cos(theta);
				Fy = force * Math.sin(theta) * -1;

				if (r - _xscale / 2 - otherPlanet._xscale / 2 <= 1)
				{
					if (_root.game.collision == 1)
					{
						Fx = 0;
						Fy = 0;
					}
				}
				FxSum += Fx;
				FySum += Fy;
			}
		}

		xAccel = FxSum / mass;
		yAccel = FySum / mass;
	}

	function bounceOnSides()
	{
		var size:Number = 100;
		if (_x < size / 2)
		{
			xVel *= -1;
			_x = size / 2 + .001;
		}
		if (_x > 600 - size / 2)
		{
			xVel *= -1;
			_x = 599.99 - size / 2;
		}
		if (_y < size / 2)
		{
			yVel *= -1;
			_y = size / 2 + .001;
		}
		if (_y > 600 - size / 2)
		{
			yVel *= -1;
			_y = 599.99 - size / 2;
		}
	}
	
	function speedLimit(limit)
	{
		if (xVel > limit)
		{
			xVel = limit;
		}
		if (yVel > limit)
		{
			yVel = limit;
		}
		if (xVel < -limit)
		{
			xVel = -limit;
		}
		if (yVel < -limit)
		{
			yVel = -limit;
		}
	}
	
	function checkDragging()
	{
		if(dragging)
		{
			_x = _root._xmouse - dragOffsetx;
			_y = _root._ymouse - dragOffsety;
		}
		
	}
	
	function readArrow()
	{
		var hyp = myArrow._xscale / 30;
		xVel = hyp * Math.cos(myArrow._rotation / 180 * 3.14);
		yVel = hyp * Math.sin(myArrow._rotation / 180 * 3.14);
	}
	
	function drawTracer()
	{
		if(_root.game.tracers == 1 && !fixed)
		{
			var tracer = _root.attachMovie("Tracer","Tracer"+_root.getNextHighestDepth(),_root.getNextHighestDepth());
			tracer._x = _x;
			tracer._y = _y;
			tracer._rotation = Math.atan2(yVel,xVel) * 180 / 3.14;
		}
	}
	
	function onPress()
	{
		//fixed = !fixed;
		dragging = !dragging;
		dragStartTime = getTimer();
		dragOffsetx = _root._xmouse - _x;
		dragOffsety = _root._ymouse - _y;
	}
	
	function onRelease()
	{
		dragging = !dragging;
		if (getTimer() - dragStartTime < 100)
		{
			fixed = !fixed;
		}
	}
}