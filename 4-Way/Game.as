class Game extends MovieClip
{
	var padSpeed:Number;
	var score:Number;
	var ballsOnScreen:Number;
	var combo:Number;
	var ballTimer:Number;
	var pickupTimer:Number;
	var ballsList:Array;
	var difficulty:String;
	var pickupsList:Array;
	var padBoostTimer:Number;
	var half:Number;
	var mostOnScreen:Number;
	var biggestCombo:Number;
	var accel:Number;

	function onLoad()
	{
		newGameStats();
		playBackgroundMusic();
		_root.kongregateServices.connect();
	}

	function onEnterFrame()
	{
		checkMovement();
		spawnBall();
		spawnPickup();
		updateScore();
		checkBoosts();
	}

	function spawnBall()
	{
		ballTimer--;
		if (ballTimer < 0)
		{
			if (_root._currentframe < 5)
			{
				ballTimer = 340;
				var ball = _root.attachMovie("Ball", "Ball" + _root.getNextHighestDepth(), _root.getNextHighestDepth());
				ball._x = Math.random() * 200 + 175;
				ball._y = Math.random() * 200 + 175;
				ballsList.push(ball);
				ballsOnScreen += 1;
				if (ballsOnScreen > mostOnScreen)
				{
					mostOnScreen = ballsOnScreen;
				}
				updateCombo(15 * ballsOnScreen);
				_root.soundFX.attachSound("ballSpawn.wav");
				_root.soundFX.start();
			}
			else
			{
				ballTimer = 999999;
			}
		}
	}

	function spawnPickup()
	{
		pickupTimer--;
		if (pickupTimer < 0)
		{
			pickupTimer = Math.random() * 350 + 350;
			var pickup = _root.attachMovie("Pickup", "Pickup" + _root.getNextHighestDepth(), _root.getNextHighestDepth());
			pickup._x = Math.random() * 200 + 175;
			pickup._y = Math.random() * 200 + 175;
			_root.soundFX.attachSound("pspawn.wav");
			_root.soundFX.start();
			pickupsList.push(pickup);
		}
	}

	function newGameStats()
	{
		ballTimer = 34;
		pickupTimer = 100;
		ballsList = [];
		pickupsList = [];
		padSpeed = 10;
		score = 0;
		combo = 0;
		ballsOnScreen = 0;
		updateCombo(0);
		half = 24;
		mostOnScreen = 0;
		biggestCombo = 0;
		accel = 0.1;
	}

	function checkMovement()
	{
		if (Key.isDown(Key.UP))
		{
			_root.padLeft._y -= padSpeed;
			_root.padRight._y += padSpeed;
			if (_root.padLeft._y < half + 75)
			{
				_root.padLeft._y = half + 75;
				_root.padRight._y = 475 - half;
			}
			if (_root.padRight._y < half + 75)
			{
				_root.padRight._y = half + 75;
				_root.padLeft._y = 475 - half;
			}
		}

		if (Key.isDown(Key.DOWN))
		{
			_root.padLeft._y += padSpeed;
			_root.padRight._y -= padSpeed;
			if (_root.padRight._y < half + 75)
			{
				_root.padRight._y = half + 75;
				_root.padLeft._y = 475 - half;
			}
			if (_root.padLeft._y < half + 75)
			{
				_root.padLeft._y = half + 75;
				_root.padRight._y = 475 - half;
			}
		}

		if (Key.isDown(Key.RIGHT))
		{
			_root.padTop._x += padSpeed;
			_root.padBottom._x -= padSpeed;
			if (_root.padBottom._x < half + 75)
			{
				_root.padBottom._x = half + 75;
				_root.padTop._x = 475 - half;
			}
			if (_root.padTop._x < half + 75)
			{
				_root.padTop._x = half + 75;
				_root.padBottom._x = 475 - half;
			}
		}

		if (Key.isDown(Key.LEFT))
		{
			_root.padBottom._x += padSpeed;
			_root.padTop._x -= padSpeed;
			if (_root.padTop._x < half + 75)
			{
				_root.padTop._x = half + 75;
				_root.padBottom._x = 475 - half;
			}
			if (_root.padBottom._x < half + 75)
			{
				_root.padBottom._x = half + 75;
				_root.padTop._x = 475 - half;
			}
		}
	}

	function updateScore()
	{
		score += 20 * ballsOnScreen + combo / 2;
		score = Math.floor(score);
		_root.scoreText.text = score;
	}

	function updateCombo(modifier)
	{
		combo += modifier;
		combo = Math.floor(combo);
		if (combo < 0)
		{
			combo = 0;
		}
		_root.combo.comboText.text = combo + "x";
		_root.combo._xscale = combo + 50;
		_root.combo._yscale = combo + 50;
		if (combo > biggestCombo)
		{
			biggestCombo = combo;
		}
	}

	function checkBoosts()
	{
		padBoostTimer--;
		if (padBoostTimer == 0)
		{
			half = 24;
			_root.padTop._xscale = 100;
			_root.padBottom._xscale = 100;
			if (_root._currentframe == 3)
			{
				_root.padRight._xscale = 100;
				_root.padLeft._xscale = 100;
			}
		}
	}

	function gameOver()
	{
		if (_root._currentframe == 3)
		{
			_root.siteLock.lastHard = score;
			if (score > _root.siteLock.bestHard)
			{
				_root.siteLock.bestHard = score;
			}
			_root.kongregateStats.submit("Hard High Score",_root.siteLock.bestHard);
			_root.kongregateStats.submit("Hard Last Score",score);
			_root.kongregateStats.submit("Hard Most on Screen",mostOnScreen);
			_root.kongregateStats.submit("Hard Biggest Combo",biggestCombo);
		}
		if (_root._currentframe == 4)
		{
			_root.siteLock.lastEasy = score;
			if (score > _root.siteLock.bestEasy)
			{
				_root.siteLock.bestEasy = score;
			}
			_root.kongregateStats.submit("Easy High Score",_root.siteLock.bestEasy);
			_root.kongregateStats.submit("Easy Last Score",score);
			_root.kongregateStats.submit("Easy Most on Screen",mostOnScreen);
			_root.kongregateStats.submit("Easy Biggest Combo",biggestCombo);
		}
		_root.siteLock.saveGame();
		_root.backgroundMusic.stop();
		_root.soundFX.stop();
		for (var i in pickupsList)
		{
			pickupsList[i].removeMovieClip();
		}
		_root.gotoAndStop(2);
	}

	function playBackgroundMusic()
	{
		_root.backgroundMusic.attachSound("Code Red (Game Edit).mp3");
		_root.backgroundMusic.start(0,1000);
	}
}