class Ship extends MovieClip
{
	var velocity;
	var shootLimiter;
	var enemyTimer;
	var enemies;
	var score;
	var health;
	var deathTimer;
	
	function onLoad()
	{
		health = 100;
		velocity = 10;
		shootLimiter = 0;
		enemyTimer = 0;
		enemies = [];
		resetScore();
		_root.gameOverMenu._visible = false;
		deathTimer = 0;
		//resetHealth();

		_root.gameOverMenu.playAgainButton.onPress = function()
		{
			_root.ship.newGame();
		}
	}

	function onEnterFrame()
	{
		if (_visible == true)
		{
			shootLimiter++;
			deathTimer++;
			
			if (deathTimer > 150)
			{
				updateHealth(-100+health)
			}

			if (Key.isDown(Key.RIGHT))
			{
				_x = _x + velocity;
			}
			if (Key.isDown(Key.LEFT))
			{
				_x = _x - velocity;
			}
			if (Key.isDown(Key.UP))
			{
				_y = _y - velocity;
			}
			if (Key.isDown(Key.DOWN))
			{
				_y = _y + velocity;
			}
			if (Key.isDown(Key.SPACE) && shootLimiter > 8)
			{
				shootLimiter = 0;
				var missile = _root.attachMovie("Missile", "Missile" + _root.getNextHighestDepth(), _root.getNextHighestDepth());
				missile._x = _x + 110;
				missile._y = _y - 1;
			}
			enemyTimer += 1;
			if (enemyTimer > 60)
			{
				enemyTimer = 0;
				var enemy = _root.attachMovie("EnemyShip", "EnemyShip" + _root.getNextHighestDepth(), _root.getNextHighestDepth());
				enemies.push(enemy);
			}
		}
	}

	function UpdateScore(points)
	{
		score += points;
		_root.scoreText.text = score;
	}

	function resetScore()
	{
		score = 0;
		_root.scoreText.text = score;
	}

	function updateHealth(points)
	{
		health += points;
		if (health < 1)
		{
			health = 0;
			_root.gameOverMenu._visible = true;
			explode();
		}
		_root.healthMeter.bar._xscale = health;
	}

	function explode()
	{
		this._visible = false;
		var explosion = _root.attachMovie("Explosion", "Explosion" + _root.getNextHighestDepth(), _root.getNextHighestDepth());
		explosion._x = _x;
		explosion._y = _y;
		for (var i in enemies)
		{
			enemies[i].explode();
		}
	}

	function newGame()
	{
		this._visible = true;
		_root.gameOverMenu._visible = false;
		resetHealth();
		resetScore();
	}

	function resetHealth()
	{
		health = 100;
		_root.healthMeter.bar._xscale = health;
	}
}