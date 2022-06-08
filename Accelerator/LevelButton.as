class LevelButton extends MovieClip
{
	var Level;
	var levelText;
	
	function onPress()
	{
		_root.levelsMenu.playLevel(Level);
	}
	
	function onRollOver()
	{
		_alpha = 60;
	}
	
	function onRollOut()
	{
		_alpha = 100;
	}
}