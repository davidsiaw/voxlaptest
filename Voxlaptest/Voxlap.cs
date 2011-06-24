using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;

namespace Voxlaptest
{
    /// <summary>
    /// Contains the interop bridge
    /// </summary>
    static unsafe class Voxlap
    {
        public struct Orientation
        {
            public dpoint3d ipo, ist, ihe, ifo;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct vx5sprite
        {
            public point3d p; //position in VXL coordinates
            public int flags; //flags bit 0:0=use normal shading, 1=disable normal shading
            //flags bit 1:0=points to kv6data, 1=points to kfatype
            //flags bit 2:0=normal, 1=invisible sprite
            public point3d s;

            public IntPtr voxnum; //pointer to KV6 voxel data (bit 1 of flags = 0)

            public point3d h;
            public int kfatim;        //time (in milliseconds) of KFA animation
            public point3d f;
            public int okfatim;       //make vx5sprite exactly 64 bytes :)
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct dpoint3d
        {
            public double x, y, z;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct lpoint3d
        {
            public int x, y, z;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct point3d
        {
            public float x, y, z;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct point4d
        {
            public float x, y, z, z2;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        struct kv6voxtype
        {
            public int col;
            public ushort zpublic;
            public char vis, dir;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        struct kv6data
        {
            public int leng, xsiz, ysiz, zsiz;
            public float xpiv, ypiv, zpiv;
            public uint numvoxs;
            public int namoff;
            public kv6data* lowermip;
            public kv6voxtype* vox;      //numvoxs*sizeof(kv6voxtype)
            public uint* xlen;  //xsiz*sizeof(long)
            public ushort* ylen; //xsiz*ysiz*sizeof(short)
        }

        [DllImport("voxlap")]
        extern static int initvoxlap();

        [DllImport("voxlap")]
        extern static void uninitvoxlap();

        //extern static  long loadsxl (const char *, char **, char **, char **);
        //extern static  char *parspr (vx5sprite *, char **);

        [DllImport("voxlap")]
        extern static  void loadnul (out dpoint3d ipo, out dpoint3d ist, out dpoint3d ihe, out dpoint3d ifo);

        //extern static  long loaddta (const char *, dpoint3d *, dpoint3d *, dpoint3d *, dpoint3d *);
        //extern static long loadpng (const char *, dpoint3d *, dpoint3d *, dpoint3d *, dpoint3d *);
        //extern static void loadbsp (const char *, dpoint3d *, dpoint3d *, dpoint3d *, dpoint3d *);

        [DllImport("voxlap")]
        extern static int loadvxl([MarshalAs(UnmanagedType.LPStr)] string filename, out dpoint3d ipo, out dpoint3d ist, out dpoint3d ihe, out dpoint3d ifo);

        //extern static long savevxl (const char *, dpoint3d *, dpoint3d *, dpoint3d *, dpoint3d *);

        [DllImport("voxlap")]
        extern static long loadsky (string filename);



        [DllImport("voxlap")]
        extern static void voxsetframebuffer(IntPtr p, int b, int x, int y);

        //extern static void setsideshades (char, char, char, char, char, char);
        [DllImport("voxlap")]
        extern static void setcamera(ref dpoint3d ipo, ref dpoint3d ist, ref dpoint3d ihe, ref dpoint3d iho, float dahx, float dahy, float dahz);
        [DllImport("voxlap")]
        extern static void opticast();
        //extern static void drawpoint2d (long, long, long);
        //extern static void drawpoint3d (float, float, float, long);
        //extern static void drawline2d (float, float, float, float, long);
        //extern static void drawline3d (float, float, float, float, float, float, long);
        //extern static long project2d (float, float, float, float *, float *, float *);
        //extern static void drawspherefill (float, float, float, float, long);
        //extern static void drawpicinquad (long, long, long, long, long, long, long, long, float, float, float, float, float, float, float, float);
        //extern static void drawpolyquad (long, long, long, long, float, float, float, float, float, float, float, float, float, float, float, float, float, float, float, float, float, float);
        //extern static void print4x6 (long, long, long, long, const char *, ...);
        //extern static void print6x8 (long, long, long, long, const char *, ...);
        //extern static void drawtile (long, long, long, long, long, long, long, long, long, long, long, long);
        //extern static long screencapture32bit (const char *);
        //extern static long surroundcapture32bit (dpoint3d *, const char *, long);

        [DllImport("voxlap")]
        extern static kv6data* getkv6([MarshalAs(UnmanagedType.LPStr)] string filename);
        //extern static kfatype *getkfa (const char *);
        //extern static void freekv6 (kv6data *kv6);
        //extern static void savekv6 (const char *, kv6data *);
        //extern static void getspr (vx5sprite *, const char *);

        [DllImport("voxlap")]
        extern static kv6data *genmipkv6 (kv6data *vxl);
        //extern static char *getkfilname (long);
        //extern static void animsprite (vx5sprite *, long);

        [DllImport("voxlap")]
        extern static void drawsprite(ref vx5sprite sprite);
        //extern static long meltsphere (vx5sprite *, lpoint3d *, long);
        //extern static long meltspans (vx5sprite *, vspans *, long, lpoint3d *);

        //extern static void orthonormalize (point3d *, point3d *, point3d *);
        //extern static void dorthonormalize (dpoint3d *, dpoint3d *, dpoint3d *);
        //extern static void orthorotate (float, float, float, point3d *, point3d *, point3d *);
        //extern static void dorthorotate (double, double, double, dpoint3d *, dpoint3d *, dpoint3d *);

        [DllImport("voxlap")]
        extern static void axisrotate (ref point3d vector, ref point3d axis, float angle);

        //extern static void slerp (point3d *, point3d *, point3d *, point3d *, point3d *, point3d *, point3d *, point3d *, point3d *, float);
        //extern static long cansee (point3d *, point3d *, lpoint3d *);

        public enum Face
        {
            Xdown,
            Xup,
            Ydown,
            Yup,
            Zdown,
            Zup,
        }

        [DllImport("voxlap")]
        extern static void hitscan(ref dpoint3d startpos, ref dpoint3d direction, out lpoint3d coordOfVoxelHit, out IntPtr color, out Face face);

        //extern static void sprhitscan (dpoint3d *, dpoint3d *, vx5sprite *, lpoint3d *, kv6voxtype **, float *vsc);
        //extern static double findmaxcr (double, double, double, double);
        
        [DllImport("voxlap")]
        extern static void clipmove (ref dpoint3d inout, ref dpoint3d movement, double radius);

        //extern static long triscan (point3d *, point3d *, point3d *, point3d *, lpoint3d *);
        //extern static void estnorm (long, long, long, point3d *);

        //extern static long isvoxelsolid (long, long, long);
        //extern static long anyvoxelsolid (long, long, long, long);
        //extern static long anyvoxelempty (long, long, long, long);
        //extern static long getfloorz (long, long, long);
        //extern static long getcube (long, long, long);

        //extern static void setcube (long, long, long, long);
        //extern static void setsphere (lpoint3d *, long, long);
        //extern static void setellipsoid (lpoint3d *, lpoint3d *, long, long, long);
        //extern static void setcylinder (lpoint3d *, lpoint3d *, long, long, long);
        
        [DllImport("voxlap")]
        extern static void setrect (ref lpoint3d hit, ref lpoint3d hit2, int dacol);
        //extern static void settri (point3d *, point3d *, point3d *, long);
        //extern static void setsector (point3d *, long *, long, float, long, long);
        //extern static void setspans (vspans *, long, lpoint3d *, long);
        //extern static void setheightmap (const unsigned char *, long, long, long, long, long, long, long);
        //extern static void setkv6 (vx5sprite *, long);

        //extern static void sethull3d (point3d *, long, long, long);
        //extern static void setlathe (point3d *, long, long, long);
        //extern static void setblobs (point3d *, long, long, long);
        //extern static void setfloodfill3d (long, long, long, long, long, long, long, long, long);
        //extern static void sethollowfill ();
        //extern static void setkvx (const char *, long, long, long, long, long);
        //extern static void setflash (float, float, float, long, long, long);
        //extern static void setnormflash (float, float, float, long, long);

        //extern static void updatebbox (long, long, long, long, long, long, long);

        [DllImport("voxlap")]
        extern static void updatevxl ();

        [DllImport("voxlap")]
        extern static void genmipvxl (long x0, long y0, long x1, long y1);
        //extern static void updatelighting (long, long, long, long, long, long);

        //extern static void checkfloatinbox (long, long, long, long, long, long);
        //extern static void startfalls ();
        //extern static void dofall (long);
        //extern static long meltfall (vx5sprite *, long, long);
        //extern static void finishfalls ();

        //extern static long curcolfunc (lpoint3d *);
        //extern static long floorcolfunc (lpoint3d *);
        //extern static long jitcolfunc (lpoint3d *);
        //extern static long manycolfunc (lpoint3d *);
        //extern static long sphcolfunc (lpoint3d *);
        //extern static long woodcolfunc (lpoint3d *);
        //extern static long pngcolfunc (lpoint3d *);
        //extern static long kv6colfunc (lpoint3d *);

        //extern static void voxbackup (long, long, long, long, long);
        //extern static void voxdontrestore ();
        //extern static void voxrestore ();
        //extern static void voxredraw ();

        //extern static void kpzload (const char *, long *, long *, long *, long *);
        //extern static void kpgetdim (const char *, long, long *, long *);
        //extern static long kprender (const char *, long, long, long, long, long, long, long);

        [DllImport("voxlap")]
        extern static int kzaddstack([MarshalAs(UnmanagedType.LPStr)] string filename);

        [DllImport("voxlap")]
        extern static void kzuninit();
        //extern static long kzopen (const char *);
        //extern static long kzread (void *, long);
        //extern static long kzfilelength ();
        //extern static long kzseek (long, long);
        //extern static long kztell ();
        //extern static long kzgetc ();
        //extern static long kzeof ();
        //extern static void kzclose ();

        //extern static void kzfindfilestart (const char *); //pass wildcard string
        //extern static long kzfindfile (char *); //you alloc buf, returns 1:found,0:~found


        // Other exports
        [DllImport("voxlap")]
        extern static int getVSID();

        [DllImport("voxlap")]
        extern static void setMaxScanDistToMax();

        [DllImport("voxlap")]
        extern static void setMipUse(int amount);

        [DllImport("voxlap")]
        extern static void setRectOneColor(ref lpoint3d p1, ref lpoint3d p2, int c);

        [DllImport("voxlap")]
        extern static void setRectWoodColor(ref lpoint3d p1, ref lpoint3d p2, int c);

        [DllImport("voxlap")]
        extern static void setRectBrightness(ref lpoint3d p1, ref lpoint3d p2, byte brightness);

        public enum LightingType
        {
            NoLighting = 0,
            NormalLighting = 1,
            Multipointsource = 2,
        }

        [DllImport("voxlap")]
        extern static void setLightingMode(LightingType type);

        [DllImport("voxlap")]
        extern static void printString(int x, int y, int fcol, int bcol, string str);

        public static int Initialize()
        {
            return initvoxlap();
        }

        public static int AddStack(string filename)
        {
            return kzaddstack(filename);
        }

        public static Orientation LoadNull()
        {
            dpoint3d ipo, ist, ihe, ifo;
            loadnul(out ipo, out ist, out ihe, out ifo);

            return new Orientation()
            {
                ipo = ipo,
                ist = ist,
                ihe = ihe,
                ifo = ifo,
            };
        }

        public static void LoadSky(string filename){
            loadsky(filename);
        }

        public static Orientation LoadVoxel(string filename)
        {
            dpoint3d ipo, ist, ihe, ifo;
            int ret = loadvxl(filename, out ipo, out ist, out ihe, out ifo);
            if (ret == -1)
            {
                throw new Exception();
            }
            return new Orientation()
            {
                ipo = ipo,
                ist = ist,
                ihe = ihe,
                ifo = ifo,
            };
        }

        public static IntPtr GetKV6(string filename)
        {
            return (IntPtr)getkv6(filename);
        }

        public static void SetFrameBuffer(IntPtr topLeft, int pitch, int xdim, int ydim)
        {
            voxsetframebuffer(topLeft, pitch, xdim, ydim);
        }

        public static void SetCamera(dpoint3d ipo, dpoint3d ist, dpoint3d ihe, dpoint3d ifo, float dahx, float dahy, float dahz)
        {
            setcamera(ref ipo, ref ist, ref ihe, ref ifo, dahx, dahy, dahz);
        }

        public static void Opticast()
        {
            opticast();
        }

        public static void DrawSprite(vx5sprite sprite)
        {
            drawsprite(ref sprite);
        }

        public static void SetRect(lpoint3d p1, lpoint3d p2, Color c)
        {
            setRectOneColor(ref p1, ref p2, c.ToArgb());
        }

        public static void SetRectWithWoodTexture(lpoint3d p1, lpoint3d p2, Color c, byte brightness = 128)
        {
            setRectWoodColor(ref p1, ref p2, c.ToArgb() & 0xffffff | (brightness << 24));
        }

        public static void SetRectBrightness(lpoint3d p1, lpoint3d p2, byte brightness)
        {
            setRectBrightness(ref p1, ref p2, brightness);
        }

        public static void ClearRect(lpoint3d p1, lpoint3d p2)
        {
            setrect(ref p1, ref p2, -1);
        }

        public static dpoint3d ClipMove(dpoint3d start, dpoint3d movementVector, double objectRadius)
        {
            clipmove(ref start, ref movementVector, objectRadius);
            return start;
        }

        public static point3d Rotate(point3d vector, point3d axis, float angle)
        {
            axisrotate(ref vector, ref axis, angle);
            return vector;
        }

        public static void UpdateVxl()
        {
            updatevxl();
        }

        public static void GenMipVxlFirst()
        {
            genmipvxl(0, 0, getVSID(), getVSID());
        }

        public static void GenAllMipsKV6(IntPtr kv6)
        {
            for (IntPtr cur = kv6; cur == IntPtr.Zero; )
            {
                cur = (IntPtr)genmipkv6((kv6data*)cur);
            }
        }

        public static void SetLightingMode(LightingType type)
        {
            setLightingMode(type);
        }

        public static void SetMaxScanDistToMax()
        {
            setMaxScanDistToMax();
        }

        public static void SetMipUse(int amount)
        {
            setMipUse(amount);
        }

        public enum HitStatus
        {
            Hit,
            Nohit,
        }

        public static HitStatus Hitscan(dpoint3d start, dpoint3d direction, out lpoint3d voxel, out Color color, out Face face)
        {
            IntPtr ind;
            hitscan(ref start, ref direction, out voxel, out ind, out face);

            if (ind == IntPtr.Zero)
            {
                color = Color.Transparent;
                return HitStatus.Nohit;
            }

            color = Color.FromArgb(Marshal.ReadInt32(ind));
            return HitStatus.Hit;
        }

        public static void Print(int x, int y, Color fg, Color bg, string format, params object[] parms)
        {
            printString(x, y, fg.ToArgb() & 0x00ffffff, bg == Color.Transparent ? -1 : bg.ToArgb() & 0x00ffffff, string.Format(format, parms));
        }
    }
}
