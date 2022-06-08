class FPSDisplay extends MovieClip
{
	var fps;
	var timeinit;
	var lasttime;
	var timepassed;
	
	function onLoad()
	{
		fps=30;
		lasttime=timeinit.getMilliseconds();
		timeinit = new Date;
	}
	
	function onEnterFrame()
	{
	var time:Date = new Date;
	//on each frame, figure out how much time has passed by comparing the milliseconds of the last frame to
	//the milliseconds of the current frame
	timepassed=((time.getMilliseconds()-lasttime)>=0)?(time.getMilliseconds()-lasttime):(1000+(time.getMilliseconds()-lasttime));
	//convert the time passed between frames to frames per second. Round it to the nearest tenth.
	fps=Math.round(1000/timepassed);
	//set last time for the next frame.
	lasttime=time.getMilliseconds();
	_root.fpsDisplay.field.text = fps;
	}
}