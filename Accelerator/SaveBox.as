class SaveBox extends MovieClip
{
	var savefile;
	var highestLevelBeaten;

	//Comments indicate change from original file in 4-way sitelock class
	function onLoad()
	{
		//_root.backgroundMusic = new Sound(_root.siteLock);
		//_root.soundFX = new Sound(_root.soundFXMC);
		
		savefile = SharedObject.getLocal("Vectron");

		loadData();
		checkData();

		var urlStart = _url.indexOf("://") + 3;
		var urlEnd = _url.indexOf("/", urlStart);
		var domain = _url.substring(urlStart, urlEnd);
		var LastDot = domain.lastIndexOf(".") - 1;
		var domEnd = domain.lastIndexOf(".", LastDot) + 1;
		var domain = domain.substring(domEnd, domain.length);

		if (domain == "kongregate.com" || domain == "")
		{
			_root.saveBox._x = -600;
		}
		
		_root.siteLock.urlName.text = domain;

		_root.siteLock.domainText.text = domain;

		var rightClick:ContextMenu = new ContextMenu();//create a new right click menu called rightClick
		rightClick.hideBuiltInItems();//hide the built in options of the right click menu
		_root.menu = rightClick;
		
		/*
		_root.volpePreloader.volpeLink.onPress = function()
		{
			_root.siteLock.getURL("http://www.facebook.com/pages/Volpe-Music/103225076431256","_blank")
		}
		*/
	}

	function saveGame()
	{
		savefile.data.highestLevelBeaten = highestLevelBeaten;
		
		savefile.flush();
	}

	function loadData()
	{
		highestLevelBeaten = savefile.data.highestLevelBeaten
	}

	function checkData()
	{
		if (highestLevelBeaten == undefined)
		{
			highestLevelBeaten = 0;
		}
	}
}