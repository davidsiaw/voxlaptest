#include "voxlap5.h"
#include <math.h>

DLLEXPORT long getVSID() { return VSID; }
DLLEXPORT void setMaxScanDistToMax() { vx5.maxscandist = VSID*sqrt(2); }
DLLEXPORT void setMipUse(long amount) { vx5.vxlmipuse = amount; }

DLLEXPORT void setRectOneColor(lpoint3d* hit1, lpoint3d* hit2, long ARGB)
{
	vx5.curcol = ARGB;
	vx5.colfunc = curcolfunc ;
	setrect(hit1, hit2, 0);
}

DLLEXPORT void setRectWoodColor(lpoint3d* hit1, lpoint3d* hit2, long ARGB)
{
	vx5.curcol = ARGB;
	vx5.colfunc = woodcolfunc ;
	setrect(hit1, hit2, 0);
}

static char curbrightness = 128;

long setbrightnessfunc (lpoint3d *p) {
	return getcube(p->x, p->y, p->z) & 0xffffff | (curbrightness << 24);
}

DLLEXPORT void setRectBrightness(lpoint3d* hit1, lpoint3d* hit2, char brightness)
{
	curbrightness = brightness;
	vx5.colfunc = setbrightnessfunc;
	setrect(hit1, hit2, 0);
}

DLLEXPORT void printString(long x, long y, long fcol, long bcol, const char* str){
	print6x8(x,y,fcol,bcol,"%s",str);
}
