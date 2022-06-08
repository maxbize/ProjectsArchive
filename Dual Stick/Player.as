class Player extends MovieClip
{
	var health:Number;
	var damage:Number;
	var accel:Number;
	var deltax:Number;
	var deltay:Number;
	var bulletList:Array;
	var enemyList:Array;
	var bulletSpeed:Number;
	var xVel:Number;
	var yVel:Number;
	var maxSpeed:Number;
	var shooting:Boolean;
	var shootTimer:Number;
	var shootSpeed:Number;
	var accuracy:Number;
	var enemyTimer:Number;
	var MAXhealth:Number;
	var HUDhearts:Array;
	
	function onLoad()
	{
		MAXhealth = 1;
		health = MAXhealth;
		accuracy = 1; // 10
		shooting = false;
		accel = .1;
		xVel = 0;
		yVel = 0;
		maxSpeed = 10;
		deltax = 0;
		deltay = 0;
		bulletList = [];
		enemyList = [];
		bulletSpeed = 10;
		shootTimer = 0;
		shootSpeed = 1; // 10
		enemyTimer = 0;
		damage = 10;
		alterHUD("reset");
	}
	
	function onEnterFrame()
	{
		deltax = _root._xmouse - _x;
		deltay = _root._ymouse - _y;
		checkMovement();
		rotate();
		checkBulletArray();
		checkEnemyArray();
		checkBorders();
		shoot();
		spawnEnemy();
	}
	
	function spawnEnemy()
	{
		enemyTimer--;
		if(enemyTimer < 0)
		{
			enemyTimer = 60;
			var choice = Math.ceil((Math.random() * 2));
			//choice = 2;
			var enemy;
			switch(choice)
			{
				case 1:
				enemy = _root.attachMovie("Enemy1","Enemy1"+_root.getNextHighestDepth(),_root.getNextHighestDepth());
				break;
				case 2:
				enemy = _root.attachMovie("Enemy2","Enemy2"+_root.getNextHighestDepth(),_root.getNextHighestDepth());
				break;
			}
			enemyList.push(enemy);
			trace(enemyList);
		}
	}
	
	function checkMovement()
	{
		// W - 87, A - 65, S - 83, D - 68
		if((Key.isDown(Key.LEFT) && Key.isDown(Key.UP)) || (Key.isDown(87) && Key.isDown(65)))
		{
			xVel -= accel / 1.414;
			yVel -= accel / 1.414;
		}
		else if((Key.isDown(Key.LEFT) && Key.isDown(Key.DOWN)) || (Key.isDown(65) && Key.isDown(83)))
		{
			xVel -= accel / 1.414;
			yVel += accel / 1.414;
		}
		else if((Key.isDown(Key.RIGHT) && Key.isDown(Key.UP)) || (Key.isDown(68) && Key.isDown(87)))
		{
			xVel += accel / 1.414;
			yVel -= accel / 1.414;
		}
		else if((Key.isDown(Key.RIGHT) && Key.isDown(Key.DOWN)) || (Key.isDown(68) && Key.isDown(83)))
		{
			xVel += accel / 1.414;
			yVel += accel / 1.414;
		}		
		else if(Key.isDown(Key.DOWN) || Key.isDown(83))
		{
			yVel += accel;
		}
		else if(Key.isDown(Key.RIGHT) || Key.isDown(68))
		{
			xVel += accel;
		}
		else if(Key.isDown(Key.UP) || Key.isDown(87))
		{
			yVel -= accel;
		}
		else if(Key.isDown(Key.LEFT) || Key.isDown(65))
		{
			xVel -= accel;
		}
		else
		{
			xVel *= 1 - (accel / 10);
			yVel *= 1 - (accel / 10);
		}
		
		//Limit speed
		if(xVel > maxSpeed || yVel > maxSpeed || xVel < -maxSpeed || yVel < -maxSpeed)
		{
			var vel = Math.sqrt(xVel * xVel + yVel * yVel);
			xVel = xVel / vel * maxSpeed;
			yVel = yVel / vel * maxSpeed;
		}
		
		// Move player
		_x += xVel;
		_y += yVel;
	}
	
	function checkBorders()
	{
		if(_x > 530)
		{
			xVel *= -1;
			_x = 529
		}
		else if(_x < 20)
		{
			xVel *= -1;
			_x = 21;
		}
		else if(_y < 20)
		{
			yVel *= -1;
			_y = 21;
		}
		else if(_y > 380)
		{
			yVel *= -1;
			_y = 379;
		}
	}
	
	function rotate()
	{
		_rotation = Math.atan(((deltay) / (deltax))) * 180 / 3.14 - 90;
		if(deltax > 0)
		{
			_rotation += 180;
		}
	}
	
	function shoot()
	{
		shootTimer--;
		if(shooting && shootTimer < 0)
		{
			shootTimer = shootSpeed;
			var distanceToGun = 30;
			var bullet = _root.attachMovie("bullet","bullet" + _root.getNextHighestDepth(),_root.getNextHighestDepth());
			var xOffset = distanceToGun * Math.cos((_rotation - 90) / 180 * 3.14);
			var yOffset = distanceToGun * Math.sin((_rotation - 90) / 180 * 3.14);
			bullet._x = _x + xOffset;
			bullet._y = _y + yOffset;
			bullet.dx = deltax;
			bullet.dy = deltay;
			bullet.mspeed = bulletSpeed;
			bullet.normalizeSpeed();
			bulletList.push(bullet);
		}
	}
	
	function checkBulletArray()
	{
		for(var i in bulletList)
		{
			if(bulletList[i]._x < -20 || bulletList[i]._x > 570 || bulletList[i]._y < -20 || bulletList[i]._y > 420 || bulletList[i]._xscale < 0)
			{
				bulletList[i].removeMovieClip();
				bulletList.splice(i,1);
			}
			for(var j in enemyList)
			{
				if(bulletList[i].hitTest(enemyList[j]) && enemyList[j].health > 0)
				{
					enemyList[j].health -= damage * bulletList[i]._xscale / 100;
					bulletList[i].removeMovieClip();
					bulletList.splice(i,1);
					if(enemyList[j].health < 0)
					{
						enemyList[j].death();
						enemyList.splice(j,1);
					}
				}
			}
		}
	}
	
	function checkEnemyArray()
	{
		for(var i in enemyList)
		{
			if(enemyList[i]._x < -20 || enemyList[i]._x > 570 || enemyList[i]._y < -20 || enemyList[i]._y > 420)
			{
				enemyList[i].removeMovieClip();
				enemyList.splice(i,1);
			}
			else if(enemyList[i].hitTest(this))
			{
				enemyList[i].removeMovieClip();
				enemyList.splice(i,1);
				loseHealth();
			}
		}
	}
	
	function alterHUD(parameter)
	{
		if(parameter == "reset")
		{
			HUDhearts = [];
			for(var i = 0; i < MAXhealth; i++)
			{
				var heart = _root.attachMovie("Heart","Heart"+_root.getNextHighestDepth(),_root.getNextHighestDepth());
				heart._x = 30 + i * 40;
				heart._y = 375;
				HUDhearts.push(heart);
			}
		}
	}
	
	function loseHealth()
	{
		health -= 1;
		HUDhearts[HUDhearts.length - (MAXhealth - health)].removeMovieClip();
		if(health < 1)
		{
			_root.gotoAndPlay(1); // Death event
		}
	}
}