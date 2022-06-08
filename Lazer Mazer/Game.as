// Hit test solution: Add a bit to the leading edge of the bar and hit test that instead of the bar

class Game extends MovieClip
{
	var gridSize:Number;
	var hWalls:Array;
	var vWalls:Array;
	var walls:Array;
	var lazerBits:Array;
	var lazerSpeed:Number;
	var shooting:Boolean;
	var lazerLength:Number;
	var shots:Number;
	var ammo:Number;
	
	var lazerBars:Array;
	
	function onLoad()
	{
		resetVars();
		//loadGrid(hWalls, false);
		//loadGrid(vWalls, true);
		//generateMaze();
		loadLevel(1);
	}
	
	function onEnterFrame()
	{
		if(!shooting)
		{
			rotateCannon();
		}
		
		checkShoot();
		moveLazer();
		checkWalls();
		
	}
	
	function resetVars()
	{
		lazerBars = [];
		lazerBits = [];
		walls = [];
		lazerSpeed = 3;
		lazerLength = 20;
		ammo = 1;
		shots = lazerLength;
		gridSize = 10;
	}
	
	function rotateCannon()
	{
		_root.cannon._rotation = Math.atan2( _root._ymouse - _root.cannon._y, _root._xmouse - _root.cannon._x ) * 180 / 3.14;
	}
	
	function checkShoot()
	{
		if(shooting && shots > 0)
		{
			shootBar();
			//shootBit();
			//shots--;
			shooting = false;
		}
		else if(shots == 0)
		{
			shots = lazerLength;
			shooting = false;
		}
	}
	
	function shoot()
	{
		shooting = true;
	}
	
	function shootBit()
	{
		var bit = _root.attachMovie("Bit","Bit"+_root.getNextHighestDepth(),_root.getNextHighestDepth());
		bit._x = _root.cannon._x + _root.cannon._width / 2 * Math.cos(_root.cannon._rotation / 180 * 3.14);
		bit._y = _root.cannon._y + _root.cannon._width / 2 * Math.sin(_root.cannon._rotation / 180 * 3.14);
		bit.ySpeed = lazerSpeed * Math.sin(_root.cannon._rotation / 180 * 3.14);
		bit.xSpeed = lazerSpeed * Math.cos(_root.cannon._rotation / 180 * 3.14);
		bit._rotation = _root.cannon._rotation;
		lazerBits.push(bit);
	}
	
	function shootBar()
	{
		var bar = _root.attachMovie("Bit","Bit"+_root.getNextHighestDepth(),_root.getNextHighestDepth());
		bar._x = _root.cannon._x + _root.cannon._width / 2 * Math.cos(_root.cannon._rotation / 180 * 3.14);
		bar._y = _root.cannon._y + _root.cannon._width / 2 * Math.sin(_root.cannon._rotation / 180 * 3.14);
		bar.ySpeed = lazerSpeed * Math.sin(_root.cannon._rotation / 180 * 3.14);
		bar.xSpeed = lazerSpeed * Math.cos(_root.cannon._rotation / 180 * 3.14);
		bar._rotation = _root.cannon._rotation;
		bar.growing = true;
		lazerBars.push(bar);
	}
	
	function moveLazer()
	{
		/*
		for (var i in lazerBits)
		{
			lazerBits[i]._x += lazerBits[i].xSpeed;
			lazerBits[i]._y += lazerBits[i].ySpeed;
		}
		*/
		for(var i in lazerBars)
		{
			if (lazerBars[i].growing == true)
			{
				if (lazerBars[i]._xscale < lazerLength)
				{
					lazerBars[i]._xscale += 100 / 3 * lazerSpeed;
				}
				else
				{
					lazerBars[i].growing = false;
				}
			}
			else
			{
				lazerBars[i]._x += lazerBars[i].xSpeed;
				lazerBars[i]._y += lazerBars[i].ySpeed;
			}
		}
	}
	
	function createWall(len,rot,posx,posy)
	{
		var spacing:Number = 500 / (gridSize + 1);
		var wall = _root.attachMovie("Wall","Wall"+_root.getNextHighestDepth(),_root.getNextHighestDepth());
		wall._x = posx * spacing;
		wall._y = posy * spacing;
		//trace(gridSize);
		//wall.number = num;
		//num++;
		wall._xscale = len * spacing * 20;
		wall._rotation = rot;
		walls.push(wall);
	}
	
	function generateMaze() // (size)
	{		
		var slots = [];
		for(var i = 0; i < gridSize; i++)
		{
			slots.push(i);
		}
		for(var i in slots)
		{
			slots[i] = [];
			for(var j = 0; j < gridSize; j++)
			{
				slots[i].push(j);
			}
		}
		
	}
	
	function loadGrid(grid,vertical)
	{
		grid = [];
		for(var i = 0; i < gridSize; i++)
		{
			grid.push(i);
		}
		for(var i in grid)
		{
			grid[i] = [];
			for(var j = 0; j < gridSize; j++)
			{
				grid[i].push(j);
			}
		}
		var num = 0;
		for(var i = 1; i < (vertical ? gridSize + 1 : gridSize); i++)
		{
			for(var j = 1; j < (!vertical ? gridSize + 1 : gridSize); j++)
			{
				var spacing:Number = 500 / (gridSize + 1);
				var wall = _root.attachMovie("Wall","Wall"+_root.getNextHighestDepth(),_root.getNextHighestDepth());
				wall._x = i * spacing;
				wall._y = j * spacing;
				wall.number = num;
				num++;
				wall._xscale = 910;
				if(vertical)
				{
					wall._rotation += 90;
					wall._y += wall._height / 2;
				}
				else
				{
					wall._x += wall._width / 2;
				}
				grid[i - 1][j - 1] = wall
			}
		}
		if(vertical)
		{
			vWalls = grid;
		}
		else
		{
			hWalls = grid;
		}
	}
	
	function loadLevel(level)
	{
		switch (level)
		{
			case 1:
				createWall(10,30,4,10);
				createWall(10,30,4,1);
				createWall(12,95,1,5);
				createWall(12,95,5,5);
				break;
		}
		
	}
	
	function checkWalls()
	{
		for(var lb in lazerBars)
		{
			for (var w in walls)
			{
				if (walls[w].hitTest( lazerBars[lb]._x,lazerBars[lb]._y,true ))
				{
					//trace("lazer initial " + lazerBars[lb]._rotation);
					//trace("wall " + walls[w]._rotation);
					var angle_incidence = Math.abs(lazerBars[lb]._rotation - walls[w]._rotation);
					var angle_rotated = ( lazerBars[lb]._rotation > walls[w]._rotation ? lazerBars[lb]._rotation - 2 * angle_incidence : lazerBars[lb]._rotation + 2 * angle_incidence);
					lazerBars[lb].xSpeed = lazerSpeed * Math.cos(angle_rotated/180*3.14);
					lazerBars[lb].ySpeed = lazerSpeed * Math.sin(angle_rotated/180*3.14);
					lazerBars[lb]._rotation = Math.atan2(lazerBars[lb].ySpeed,lazerBars[lb].xSpeed) * 180 / 3.14;
					//trace("lazer final " + lazerBars[lb]._rotation);
				}
			}
		}
	}
}