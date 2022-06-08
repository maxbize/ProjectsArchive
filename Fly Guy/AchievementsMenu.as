class AchievementsMenu extends MovieClip
{
	function onLoad()
	{
		//Hide all achievements
		_root.achievementsMenu.FiftyKillsEarned._visible = false;
		
		_root.achievementsMenu.achievementsMainMenuButton.onPress = function()
		{
			_root.achievementsMenu._visible = false;
			_root.mainMenu._visible = true;
		}
	}
	
	function updateAchievementsDisplay()
	{
		if(_root.flyGuy.kills > 50)
		{
			_root.achievementsMenu.FiftyKillsUnearned._visible = false;
			_root.achievementsMenu.FiftyKillsEarned._visible = true;
		}
	}
}