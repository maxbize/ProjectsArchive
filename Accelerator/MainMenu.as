class MainMenu extends MovieClip
{
	function onLoad()
	{
		_root.mainMenu.PlayButton.onPress = function()
		{
			_root.gotoAndStop(2);
		};
	}
}