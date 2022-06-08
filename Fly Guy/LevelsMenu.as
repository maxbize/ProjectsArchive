class LevelsMenu extends MovieClip
{
	var highestLevelAvailable;

	function onLoad()
	{
		_root.newEnemy.invisButton.onRelease = function()
		{
			_root.newEnemy._visible = false;
			_root.flyGuy.nextLevel(_root.newEnemy._currentframe);
		}
		
		_root.levelsMenu._visible = false;
		
		_root.levelsMenu.levelButton1.onPress = function()
		{
			_root.levelsMenu.playLevel(1);
		}
		
		_root.levelsMenu.levelButton2.onPress = function()
		{
			_root.levelsMenu.playLevel(2);
		}
		
		_root.levelsMenu.levelButton3.onPress = function()
		{
			_root.levelsMenu.playLevel(3);
		}
		
		_root.levelsMenu.levelButton4.onPress = function()
		{
			_root.levelsMenu.playLevel(4);
		}
		
		_root.levelsMenu.levelButton5.onPress = function()
		{
			_root.levelsMenu.playLevel(5);
		}
		
		_root.levelsMenu.levelButton6.onPress = function()
		{
			_root.levelsMenu.playLevel(6);
		}
		
		_root.levelsMenu.levelButton7.onPress = function()
		{
			_root.levelsMenu.playLevel(7);
		}
		
		_root.levelsMenu.levelButton8.onPress = function()
		{
			_root.flyGuy.nextLevel(8);
		}
		
		_root.levelsMenu.levelButton9.onPress = function()
		{
			_root.flyGuy.nextLevel(9);
		}
		
		_root.levelsMenu.levelButton10.onPress = function()
		{
			_root.flyGuy.nextLevel(10);
		}
		
		_root.levelsMenu.levelButtonSurvival.onPress = function()
		{
			_root.flyGuy.nextLevel(11);
		}
		
		_root.levelsMenu.levelsMainMenuButton.onPress = function()
		{
			_root.levelsMenu._visible = false;
			_root.mainMenu._visible = true;
		}
		_root.levelComplete.retry.onPress = function()
		{
			_root.backgroundMusic.stop();
			_root.flyGuy.nextLevel(_root.flyGuy.currentLevel);
			_root.levelComplete._visible = false;
		}
		_root.levelComplete.menuButton.onPress = function()
		{
			_root.userInterface._visible = false;
			_root.mainMenu._visible = true;
			_root.hideGame._alpha = 100;
			_root.backgroundMusic.stop();
			_root.levelComplete._visible = false;
		}
		_root.levelComplete.nextLevelButton.onPress = function()
		{
			_root.userInterface._visible = false;
			_root.levelComplete._visible = false;
			_root.levelsMenu.playLevel((_root.flyGuy.currentLevel + 1));
		}
	}
	function playLevel(level)
	{
		if(_root.flyGuy.highestLevelCompleted >= level - 1)
		{
		_root.levelsMenu._visible = false;
		_root.newEnemy._visible = true;
		_root.newEnemy.gotoAndStop(level);
		}
	}
}