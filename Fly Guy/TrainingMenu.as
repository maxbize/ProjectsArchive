class TrainingMenu extends MovieClip
{
	var views;
	var xpRequired;
	var xpArray;
	
	function onLoad()
	{
		_root.trainingMenu.augmentRate.onPress = function()
		{
			//10.236x6 - 220.32x5 + 1870.7x4 - 7064.9x3 + 12527x2 - 7616.7x + 159.72
			views = _root.trainingMenu.getViews("rate");
			xpRequired = _root.trainingMenu.getXPrequired("rate",views);
			if (_root.flyGuy.experience >= xpRequired)
			{
				_root.flyGuy.experience -= xpRequired;
				_root.flyGuy.shootSpeed--;
				views = _root.trainingMenu.getViews("rate");
				xpRequired = _root.trainingMenu.getXPrequired("rate",views);
			}
			_root.trainingMenu.rateXp.text = xpRequired;
			_root.trainingMenu.rateViews.text = views;
			_root.trainingMenu.xpText.text = _root.flyGuy.experience;
		};
		_root.trainingMenu.augmentSpeed.onPress = function()
		{
			views = _root.trainingMenu.getViews("speed");
			xpRequired = _root.trainingMenu.getXPrequired("speed",views);
			if (_root.flyGuy.experience >= xpRequired)
			{
				_root.flyGuy.experience -= xpRequired;
				_root.flyGuy.bulletSpeed++;
				views = _root.trainingMenu.getViews("speed");
				xpRequired = _root.trainingMenu.getXPrequired("speed",views);
			}
			_root.trainingMenu.speedXp.text = xpRequired;
			_root.trainingMenu.speedViews.text = views;
			_root.trainingMenu.xpText.text = _root.flyGuy.experience;
		};
		_root.trainingMenu.augmentPower.onPress = function()
		{
			views = _root.trainingMenu.getViews("power");
			xpRequired = _root.trainingMenu.getXPrequired("power",views);
			if (_root.flyGuy.experience >= xpRequired)
			{
				_root.flyGuy.experience -= xpRequired;
				_root.flyGuy.Damage += 20;
				views = _root.trainingMenu.getViews("power");
				xpRequired = _root.trainingMenu.getXPrequired("power",views);
			}
			_root.trainingMenu.powerXp.text = xpRequired;
			_root.trainingMenu.powerViews.text = views;
			_root.trainingMenu.xpText.text = _root.flyGuy.experience;
		};
		_root.trainingMenu.augmentEnduranceTotal.onPress = function()
		{
			views = _root.trainingMenu.getViews("total");
			xpRequired = _root.trainingMenu.getXPrequired("total",views);
			if (_root.flyGuy.experience >= xpRequired)
			{
				_root.flyGuy.experience -= xpRequired;
				_root.flyGuy.flightTimerMAX *= 1.5;
				views = _root.trainingMenu.getViews("total");
				xpRequired = _root.trainingMenu.getXPrequired("total",views);
			}
			_root.trainingMenu.enduranceTotalXp.text = xpRequired;
			_root.trainingMenu.enduranceTotalViews.text = views;
			_root.trainingMenu.xpText.text = _root.flyGuy.experience;
		};
		_root.trainingMenu.augmentEnduranceIncrement.onPress = function()
		{
			views = _root.trainingMenu.getViews("increment");
			xpRequired = _root.trainingMenu.getXPrequired("increment",views);
			if (_root.flyGuy.experience >= xpRequired)
			{
				_root.flyGuy.experience -= xpRequired;
				_root.flyGuy.flightTimerIncrementBase--;
				views = _root.trainingMenu.getViews("increment");
				xpRequired = _root.trainingMenu.getXPrequired("increment",views);
			}
			_root.trainingMenu.enduranceIncrementXp.text = xpRequired;
			_root.trainingMenu.enduranceIncrementViews.text = views;
			_root.trainingMenu.xpText.text = _root.flyGuy.experience;
		};
		_root.trainingMenu.augmentHealth.onPress = function()
		{
			views = _root.trainingMenu.getViews("health");
			xpRequired = _root.trainingMenu.getXPrequired("health",views);
			if (_root.flyGuy.experience >= xpRequired)
			{
				_root.flyGuy.experience -= xpRequired;
				_root.flyGuy.MAXHEALTH += 0.4;
				views = _root.trainingMenu.getViews("health");
				xpRequired = _root.trainingMenu.getXPrequired("health",views);
			}
			_root.trainingMenu.healthXp.text = xpRequired;
			_root.trainingMenu.healthViews.text = views;
			_root.trainingMenu.xpText.text = _root.flyGuy.experience;
		};
		_root.trainingMenu.trainingMainMenuButton.onPress = function()
		{
			_root.trainingMenu._visible = false;
			_root.mainMenu._visible = true;
		};
	}
	
	function getXPrequired(upgrade, views)
	{
		switch(upgrade)
		{
			case "rate":
				if(views < 20)
				{
				return 10 * Math.pow(2,views);
				}
				else
				{
					return 9999999999999999999999999999999999999999;
				}
				break;
			case "speed":
				return 50 * Math.pow(4,views);
				break;
			case "power":
				return 20 * Math.round(Math.pow(2.7,views));
				break;
			case "increment":
				if(views < 10)
				{
				return 10 * Math.pow(2,views);
				}
				else
				{
					return 9999999999999999999999999999999999999999;
				}
				break;
			case "total":
				return 500 * Math.round(Math.pow(1.5,views));
				break;
			case "health":
				return 1000 * Math.round(Math.pow(1.5,views));
				break;
		}
	}
	function getViews(upgrade):Number
	{
		switch(upgrade)
		{
			case "rate":
				if(views <= 19)
				return 20 - _root.flyGuy.shootSpeed;
				break;
			case "speed":
				return _root.flyGuy.bulletSpeed - 6;
				break;
			case "power":
				views = 0;
				for (var i = 100; i != _root.flyGuy.Damage; i += 20)
				{
					views++;
				}
				return views;
				break;
			case "increment":
				return 10 - _root.flyGuy.flightTimerIncrementBase;
				break;
			case "total": 
				views = 0;
				for (var i = 3000; i != _root.flyGuy.flightTimerMAX; i *= 1.5)
				{
					views++;
				}
				return views;
				break;
			case "health":
				return (_root.flyGuy.MAXHEALTH - 3) / 0.4;
				break;
		}
	}
}