class Controller extends MovieClip
{
	var dyHider;
	var dyMain;
	var hiderMoveSpeed;
	var hiderMoveAccel;
	var mainMoveSpeed;
	var mainMoveAccel;
	var hiderCurrent;
	var mainCurrent;
	var hiderTarget;
	var mainTarget;
	
	//MC shifter variables
	var dxTargetMC;
	var targetMC;
	var targetMCPosition;
	var targetMCCurrent;
	var targetMCMoveSpeed;
	

	function onLoad()
	{
		dyHider = 0;
		dxTargetMC = 0;
		
		//onRollOver will make actions happen when mouse is over button
		_root.blueButton.onRelease = function()
		{
			_root.actions.shiftMenus(600,900);
		};
		_root.redButton.onRelease = function()
		{
			_root.actions.shiftMenus(800,1500);
		};
		_root.greenButton.onRelease = function()
		{
			_root.actions.shiftMenus(1000,2100);
		};
		_root.main.page1.onRelease = function()
		{
			_root.actions.shiftMC(_root.main.blueMC,0);
		};
		_root.main.page2.onRelease = function()
		{
			_root.actions.shiftMC(_root.main.blueMC,-720);
		};
	}

	function shiftMenus(hTarget, mTarget)
	{
		if (dyHider == 0)
		{
			hiderTarget = hTarget;
			mainTarget = mTarget;
			hiderCurrent = _root.hider._y;
			mainCurrent = _root.main._y;
			dyHider = hTarget - hiderCurrent;
			dyMain = mTarget - mainCurrent;
			hiderMoveSpeed = dyHider / 10;
			mainMoveSpeed = dyMain / 10;
		}
		//_root.hider._y += dyHider; 
		//_root.main._y += dyMain;
	}
	
	function shiftMC(MC,xTarget)
	{
		if (dxTargetMC == 0)
		{
			targetMC = MC;
			targetMCPosition = xTarget;
			targetMCCurrent = MC._x;
			dxTargetMC = targetMCPosition - targetMCCurrent;
			targetMCMoveSpeed = dxTargetMC / 10;
		}
	}

	function onEnterFrame()
	{
		if (dyHider != 0)
		{
			_root.hider._y += hiderMoveSpeed;
			_root.main._y += mainMoveSpeed;
			if (_root.hider._y >= hiderTarget && dyHider > 0)
			{
				dyHider = 0;
			}
			if (_root.hider._y <= hiderTarget && dyHider < 0)
			{
				dyHider = 0;
			}
		}
		
		if (dxTargetMC != 0)
		{
			targetMC._x += targetMCMoveSpeed;
			if(targetMC._x >= targetMCPosition && dxTargetMC > 0)
			{
				dxTargetMC = 0;
			}
			
			if(targetMC._x <= targetMCPosition && dxTargetMC < 0)
			{
				dxTargetMC = 0;
			}
		}
	}

}