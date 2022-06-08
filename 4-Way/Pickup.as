class Pickup extends MovieClip
{
	function onEnterFrame()
	{
		for(var i in _root.game.ballsList)
		{
			if(this.hitTest(_root.game.ballsList[i]))
			{
				effect();
				this.removeMovieClip();
			}
		}
	}
	
	function effect()
	{
		_root.soundFX.attachSound("coin1.wav");
		_root.soundFX.start();
		
		var type = Math.random()
		
		//combo boost 30% chance
		if(type < .3)
		{
			var upgrade = Math.round(type * 150) + 10;
			_root.game.updateCombo(upgrade);
			spawnText("+ " + upgrade + " Combo!");
		}
		
		//double combo 10% chance
		else if(type < .4)
		{
			_root.game.updateCombo(_root.game.combo);
			spawnText("Combo doubled!");
		}
		
		//switch keys 10% chance
		else if(type < .5)
		{
			_root.game.padSpeed *= -1;
			spawnText("PAD FLIP!");
		}
		
		//slow down balls 10% chance
		else if(type < .6)
		{
			for(var i in _root.game.ballsList)
			{
				_root.game.ballsList[i].xSpeed /= 2;
				_root.game.ballsList[i].ySpeed /= 2;
			}
			spawnText("Balls slowed!");
		}
		
		//Speed up balls 10% chance
		else if(type < .7)
		{
			for(var i in _root.game.ballsList)
			{
				_root.game.ballsList[i].xSpeed *= 1.5;
				_root.game.ballsList[i].ySpeed *= 1.5;
			}
			spawnText("Balls speed UP!");
		}
		
		//Temporary Bigger pads 10% chance
		else if(type < .8)
		{
			_root.game.half = 74;
			_root.game.padBoostTimer = 525;
			_root.padTop._xscale = 200;
			_root.padBottom._xscale = 200;
			if(_root._currentframe == 3)
			{
				_root.padRight._xscale = 200;
				_root.padLeft._xscale = 200;
			}
			spawnText("Big Pads!");
		}
		
		// Temporary Gravity Well 10% chance
		else if(type < .9)
		{
			var well = _root.attachMovie("GravityWell","GravityWell"+_root.getNextHighestDepth(),_root.getNextHighestDepth());
			well._x = 550/2;
			well._y = 550/2;
			spawnText("Gravity Well!");
			_root.soundFX.attachSound("ballBounce.wav");
			_root.soundFX.start();
			_root.game.pickupsList.push(well);
		}
		
		//Ball flip 10% chance
		else if(type < 1)
		{
			for(var i in _root.game.ballsList)
			{
				_root.game.ballsList[i].xSpeed *= -1;
				_root.game.ballsList[i].ySpeed *= -1;
			}
			spawnText("Ball flip!");
		}
	}
	
	function spawnText(field)
	{
		var popup = _root.attachMovie("RewardText","RewardText"+_root.getNextHighestDepth(),_root.getNextHighestDepth())
		popup._x = _x;
		popup._y = _y;
		popup.field.text = field;
	}
}