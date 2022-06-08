// Parent class for all of the non-boss enemies
// Need to define HP within each specific enemy

class Enemy extends MovieClip
{
	var speed;
	var level;
	var health;
	var crashDamage;
	var expGained;
	var blacklistInstance;

	function spawn()
	{
		expGained = Math.round((Math.random() * 2 * _root.flyGuy.currentLevel)) + 3 * _root.flyGuy.currentLevel;
		level = _root.flyGuy.currentLevel;
		speed = Math.random() * 2 + 5;
		_x = Math.random() * 400 + 50;
		_y = -100;
		crashDamage = 1;
	}

	function logic()
	{
		_y += speed;
		if (_y > 650)
		{
			this.removeMovieClip();
		}

		if (_x < 30)
		{
			_x = 30;
		}
		if (_x > 470)
		{
			_x = 470;
		}

		if (this.hitTest(_root.flyGuy.base))
		{
			_root.flyGuy.health -= crashDamage;
			_root.flyGuy.flightTimerIncrement += 500 * level;
			this.removeMovieClip();
		}

		if (health < 1)
		{
			var pickup = Math.round(Math.random() * _root.flyGuy.luck);
			switch (pickup)
			{
				case 1 :
					pickup = _root.attachMovie("FlightOrb", "FlightOrb" + _root.getNextHighestDepth(), _root.getNextHighestDepth());
					_root.flyGuy.pickupsList.push(pickup);
					break;
				case 2 :
					pickup = _root.attachMovie("Coin", "Coin" + _root.getNextHighestDepth(), _root.getNextHighestDepth());
					_root.flyGuy.pickupsList.push(pickup);
					break;
			}
			pickup._x = _x;
			pickup._y = _y;
			_root.flyGuy.kills++;
			_root.flyGuy.checkAchievements("kills");
			_root.flyGuy.experience += expGained;
			_root.userInterface.xpText.text = _root.flyGuy.experience;
			var reward = _root.attachMovie("ExperienceRewardText","ExperienceRewardText" + _root.getNextHighestDepth(), _root.getNextHighestDepth());
			reward._x = _x;
			reward._y = _y;
			reward.field.text = "+" + expGained;
			this.removeMovieClip();
		}
	}

	function seekObject(seekedObject, seekSpeed)
	{
		if (this._x < seekedObject._x)
		{
			this._x += seekSpeed;
		}
		else
		{
			this._x -= seekSpeed;
		}
	}
}