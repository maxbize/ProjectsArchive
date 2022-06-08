class MainMenu extends MovieClip
{

	function onLoad()
	{
		checkSlashes();
		loadButtons();
		loadScoresText();
	}

	function onEnterFrame()
	{
		if (Key.isDown(69))
		{
			_root.gotoAndStop(4);
		}

		if (Key.isDown(72))
		{
			_root.gotoAndStop(3);
		}
	}

	function loadButtons()
	{
		_root.mainMenu.easyButton.onPress = function()
		{
			_root.gotoAndStop(4);
		};
		_root.mainMenu.hardButton.onPress = function()
		{
			_root.play();
		};
		_root.mainMenu.tutorialButton.onPress = function()
		{
			_root.gotoAndStop(5);
		};

		_root.mainMenu.muteMusic.onRelease = function()
		{
			if (_root.mainMenu.musicSlash._currentframe == 1)
			{
				_root.backgroundMusic.setVolume(0);
				_root.mainMenu.musicSlash.play();
			}
			if (_root.mainMenu.musicSlash._currentframe == 17)
			{
				_root.backgroundMusic.setVolume(100);
				_root.mainMenu.musicSlash.play();
			}
		};
		
		_root.mainMenu.muteFX.onRelease = function()
		{
			if (_root.mainMenu.FXSlash._currentframe == 1)
			{
				_root.soundFX.setVolume(0);
				_root.mainMenu.FXSlash.play();
			}
			if (_root.mainMenu.FXSlash._currentframe == 17)
			{
				_root.soundFX.setVolume(100);
				_root.mainMenu.FXSlash.play();
			}
		};
		
		_root.mainMenu.volpeLink.onRelease = function()
		{
			_root.siteLock.getURL("http://www.facebook.com/pages/Volpe-Music/103225076431256","_blank")
		}
	}

	function checkSlashes()
	{
		if(_root.backgroundMusic.getVolume() == 0)
		{
			_root.mainMenu.musicSlash.gotoAndStop(17);
		}
		if(_root.soundFX.getVolume() == 0)
		{
			_root.mainMenu.FXSlash.gotoAndStop(17);
		}
	}

	function loadScoresText()
	{
		_root.mainMenu.lastEasy.text = _root.siteLock.lastEasy;
		_root.mainMenu.lastHard.text = _root.siteLock.lastHard;
		_root.mainMenu.bestEasy.text = _root.siteLock.bestEasy;
		_root.mainMenu.bestHard.text = _root.siteLock.bestHard;
	}
}