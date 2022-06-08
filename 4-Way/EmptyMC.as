class EmptyMC extends MovieClip
{
	onClipEvent (load) {
_root.soundHolder.loadMovie("externalSoundObjects.swf");
_root.volpePreloader.loadBar._xscale = 0;
}
//
onClipEvent (enterFrame) {
if (_root.loadSound == 1) {
    soundsBytesTotal = _root.soundHolder.getBytesTotal();
    soundsBytesLoaded = _root.soundHolder.getBytesLoaded();
    percentLoaded = Math.round((soundsBytesLoaded/soundsBytesTotal)*100);
if (percentLoaded>0) {
    _root.percentLoadedText = percentLoaded+"%";
    // Updates text box with the percent loaded.
    _root.loadBar._xscale = percentLoaded;
    // Causes movie clip "loadBar" to scale horizontally equal to the percentLoaded.
}
//
if (percentLoaded>99 && doOnce !=1) {
    doOnce = 1;
    _root.gotoAndStop("startMovie"); // startMovie is the name of the frame you want to go to.
}
}
}
}