class LevelsMenu extends MovieClip
{
	var offset:Number;
	var buttonList:Array;
	
	function onLoad()
	{
		offset = 3;
		buttonList = [];
		constructMenu()
	}
	
	function playLevel(level)
	{
		if(_root.saveBox.highestLevelBeaten >= level - 1)
		{
			clearButtons();
			_root.gotoAndStop(level + offset);
		}
	}
	
	function clearButtons()
	{
		for ( var i in buttonList)
		{
			buttonList[i].removeMovieClip();
		}
		buttonList = [];
	}
	
	function constructMenu()
	{
		var startx = -170;
		var starty = -150;
		var dx = 113;
		var dy = 100;
		var buttonLvl = 1;
		
		for(var j = 1; j <= 4; j++)
		{
			for(var i = 1; i <= 4; i++)
			{
				var clip = _root.attachMovie("LevelButton","LevelButton"+_root.getNextHighestDepth(),_root.getNextHighestDepth());
				buttonList.push(clip);
				clip._x = i * dx - 35;
				clip._y = j * dy;
				clip.Level = buttonLvl;
				clip.levelText.text = buttonLvl;
				colorClip(clip);
				buttonLvl++;
			}
		}
	}
	
	function colorClip(clip)
	{
		var black = 0x000000
		var blue = 0x0400D9;
		var red = 0xCB0004
		var yellow = 0xCBFF04;
		var color = new Color(clip);
		if(_root.saveBox.highestLevelBeaten >= clip.Level)
		{
			color.setRGB(blue);
		}
		else if (_root.saveBox.highestLevelBeaten + 1 == clip.Level)
		{
			color.setRGB(yellow);
		}
		else
		{
			color.setRGB(red);
		}
	}
}