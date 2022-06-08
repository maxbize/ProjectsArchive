class SiteLock extends MovieClip
{
	var savefile;
	var lastEasy;
	var lastHard;
	var bestEasy;
	var bestHard;

	function onLoad()
	{
		_root.backgroundMusic = new Sound(_root.siteLock);
		_root.soundFX = new Sound(_root.soundFXMC);
		
		savefile = SharedObject.getLocal("4-Way");

		loadData();
		checkData();

		/*var urlStart = _url.indexOf("://") + 3;
		var urlEnd = _url.indexOf("/", urlStart);
		var domain = _url.substring(urlStart, urlEnd);
		var LastDot = domain.lastIndexOf(".") - 1;
		var domEnd = domain.lastIndexOf(".", LastDot) + 1;
		var domain = domain.substring(domEnd, domain.length);*/

		var urlStart = _url.indexOf("://") + 3;
		var urlEnd = _url.indexOf("/", urlStart);
		var domain = _url.substring(urlStart, urlEnd);
		var LastDot = domain.lastIndexOf(".") - 1;
		var domEnd = domain.lastIndexOf(".", LastDot) + 1;
		var domain = domain.substring(domEnd, domain.length);

		if (domain == "kongregate.com" || domain == "")
		{
			_root.siteLock._x = -600;
		}
		
		_root.siteLock.urlName.text = domain;

		_root.siteLock.domainText.text = domain;

		var rightClick:ContextMenu = new ContextMenu();//create a new right click menu called rightClick
		rightClick.hideBuiltInItems();//hide the built in options of the right click menu
		_root.menu = rightClick;
		
		_root.volpePreloader.volpeLink.onPress = function()
		{
			_root.siteLock.getURL("http://www.facebook.com/pages/Volpe-Music/103225076431256","_blank")
		}
	}

	function saveGame()
	{
		savefile.data.lastEasy = lastEasy;
		savefile.data.lastHard = lastHard;
		savefile.data.bestEasy = bestEasy;
		savefile.data.bestHard = bestHard;
		
		savefile.flush();
	}

	function loadData()
	{
		lastEasy = savefile.data.lastEasy;
		lastHard = savefile.data.lastHard;
		bestEasy = savefile.data.bestEasy;
		bestHard = savefile.data.bestHard;
	}

	function checkData()
	{
		if (lastEasy == undefined)
		{
			lastEasy = 0;
		}
		if (lastHard == undefined)
		{
			lastHard = 0;
		}
		if (bestEasy == undefined)
		{
			bestEasy = 0;
		}
		if (bestHard == undefined)
		{
			bestHard = 0;
		}
	}
}