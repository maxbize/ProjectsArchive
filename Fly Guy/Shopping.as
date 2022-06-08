class ShopMenu extends MovieClip
{
	var stuff;
	
	function onLoad()
	{
		stuff = 0;
		_root.shopMenu.shopNewGame.onPress = function()
		{
			_root.flyGuy.nextLevel(1);
			_root.mainMenu._visible = false;
		}
		_root.shopMenu.cape1Button.onPress = function()
		{
			buyCape1();
		}
	}
	
	function onEnterFrame()
	{
		
	}
	
	function buyCape1()
	{
		if(_root.flyGuy.gold >= 100)
		{
			_root.flyGuy.gold -= 100;
			_root.flyGuy.cape = 1;
			_root.shopMenu.cape1Button._visible = false;
		}
	}
}