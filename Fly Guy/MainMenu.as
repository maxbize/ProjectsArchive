class MainMenu extends MovieClip
{
	var views;
	var xpRequired;
	
	function onLoad()
	{
		_root.mainMenu._visible = false;
		_root.mainMenu.accessShopButton.onPress = function()
		{
			_root.shopMenu.goldText.text = _root.flyGuy.gold;
			_root.shopMenu._visible = true;
			_root.mainMenu._visible = false;
		}
		_root.mainMenu.accessTrainingButton.onPress = function()
		{
			_root.trainingMenu.xpText.text = _root.flyGuy.experience;
			
			views = _root.trainingMenu.getViews("rate");
			xpRequired = _root.trainingMenu.getXPrequired("rate",views);
			_root.trainingMenu.rateXp.text = xpRequired;
			_root.trainingMenu.rateViews.text = views;
		
			views = _root.trainingMenu.getViews("speed");
			xpRequired = _root.trainingMenu.getXPrequired("speed",views);
			_root.trainingMenu.speedXp.text = xpRequired;
			_root.trainingMenu.speedViews.text = views;
		
			views = _root.trainingMenu.getViews("power");
			xpRequired = _root.trainingMenu.getXPrequired("power",views);
			_root.trainingMenu.powerXp.text = xpRequired;
			_root.trainingMenu.powerViews.text = views;
		
			views = _root.trainingMenu.getViews("total");
			xpRequired = _root.trainingMenu.getXPrequired("total",views);
			_root.trainingMenu.enduranceTotalXp.text = xpRequired;
			_root.trainingMenu.enduranceTotalViews.text = views;
		
			views = _root.trainingMenu.getViews("increment");
			xpRequired = _root.trainingMenu.getXPrequired("increment",views);
			_root.trainingMenu.enduranceIncrementXp.text = xpRequired;
			_root.trainingMenu.enduranceIncrementViews.text = views;
		
			views = _root.trainingMenu.getViews("health");
			xpRequired = _root.trainingMenu.getXPrequired("health",views);
			_root.trainingMenu.healthXp.text = xpRequired;
			_root.trainingMenu.healthViews.text = views;
			
			_root.trainingMenu._visible = true;
			_root.mainMenu._visible = false;
		}
		_root.mainMenu.accessLevelsButton.onPress = function()
		{
			_root.levelsMenu._visible = true;
			_root.mainMenu._visible = false;
		}
		_root.mainMenu.accessAchievementsButton.onPress = function()
		{
			_root.achievementsMenu._visible = true;
			_root.achievementsMenu.updateAchievementsDisplay();
			_root.mainMenu._visible = false;
		}
	}
}