class Bullet extends MovieClip
{
	var Speed;
	var alreadyHit;
	var splitAmount;
	var blacklist;
	
	function spawn()
	{
		this.Speed = _root.flyGuy.bulletSpeed;
	}
	
	
	function logic()
	{
		_y -= this.Speed;
		
		if(_y < -100)
		{
			this.removeMovieClip();
		}
	}
	
	function hit(effect)
	{
		for(var i = _root.flyGuy.enemiesList.length - 20; i < _root.flyGuy.enemiesList.length; i++) // Hit test the last twenty enemies
		//for(var i in _root.flyGuy.enemiesList)
		{
			if(_root.flyGuy.enemiesList[i]._y > 0)
			{
				if(this.hitTest(_root.flyGuy.enemiesList[i]))
				{
					switch(effect)
					{
						case 'regen':
							_root.flyGuy.enemiesList[i].health -= _root.flyGuy.Damage;
							_root.flyGuy.flightTimerIncrement -= 100;
							if(_root.flyGuy.capesPurchased[10][2] == 1)
							{
								_root.flyGuy.health += 0.02;
							}
							this.removeMovieClip();
							break;
						case 'splitter':
							if(blacklist != _root.flyGuy.enemiesList[i].blacklistInstance)
							{
								_root.flyGuy.enemiesList[i].health -= _root.flyGuy.Damage;
								if(splitAmount != 2)
								{
									var bullet1 = _root.attachMovie("BulletSplitter","BulletSplitter" + _root.getNextHighestDepth(), _root.getNextHighestDepth());
									var bullet2 = _root.attachMovie("BulletSplitter","BulletSplitter" + _root.getNextHighestDepth(), _root.getNextHighestDepth());
									bullet1._x = this._x;
									bullet2._x = this._x;
									bullet1._y = this._y - 45;
									bullet2._y = this._y - 45;
									bullet1.speedx = Speed;
									bullet2.speedx = -Speed;
									bullet1._rotation = 45;
									bullet2._rotation = -45;
									_root.flyGuy.bulletsList.push(bullet1);
									_root.flyGuy.bulletsList.push(bullet2);
									bullet1.blacklist = _root.flyGuy.enemiesList[i].blacklistInstance;
									bullet2.blacklist = _root.flyGuy.enemiesList[i].blacklistInstance;
									bullet1.splitAmount = splitAmount + 1;
									bullet2.splitAmount = splitAmount + 1;
								}
								this.removeMovieClip();
							}
							break;
						case 'homing':
						case 'normal':
							_root.flyGuy.enemiesList[i].health -= _root.flyGuy.Damage;
							this.removeMovieClip();
							break;
					}
				}
			}
		}
	}
}