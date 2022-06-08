class SiteLock extends MovieClip
{
	function onLoad()
	{
		var urlStart = _url.indexOf("://") + 3;
		var urlEnd = _url.indexOf("/", urlStart);
		var domain = _url.substring(urlStart, urlEnd);
		var LastDot = domain.lastIndexOf(".") - 1;
		var domEnd = domain.lastIndexOf(".", LastDot) + 1;
		var domain = domain.substring(domEnd, domain.length);

		if (domain == "dropbox.com" || domain == "")
		{
			_root.siteLock._X = -600;
		}
		
		_root.siteLock.domainText.text = domain;
		
		var rightClick:ContextMenu = new ContextMenu();//create a new right click menu called rightClick
		rightClick.hideBuiltInItems();//hide the built in options of the right click menu
		_root.menu = rightClick;
	}
}