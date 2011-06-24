using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using Tao.Sdl;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace Voxlaptest
{
    static class Program
    {

        const int SCREEN_WIDTH = 1024;
        const int SCREEN_HEIGHT = 768;
        static IntPtr g_pDisplaySurface;
        static Sdl.SDL_Surface g_DisplaySurface;
        static Sdl.SDL_Event g_Event;

        static float vertAngle = 0;


        const int jumpamount = 20;
        const int jumprate = 4;
        static int zlevel = 0;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            Sdl.SDL_Init(Sdl.SDL_INIT_VIDEO);

            g_pDisplaySurface = Sdl.SDL_SetVideoMode(SCREEN_WIDTH, SCREEN_HEIGHT, 32, Sdl.SDL_DOUBLEBUF);
            g_DisplaySurface = (Sdl.SDL_Surface)Marshal.PtrToStructure(g_pDisplaySurface, typeof(Sdl.SDL_Surface));

            Init();


            Sdl.SDL_ShowCursor(0);

            for (; ; )
            {
                if (Sdl.SDL_PollEvent(out g_Event) == 0)
                {
                    DoFrame();
                    Sdl.SDL_Flip(g_pDisplaySurface);

                    int forward = 0;
                    int strafe = 0;

                    int numkeys;
                    byte[] keys = Sdl.SDL_GetKeyState(out numkeys);
                    if (keys[Sdl.SDLK_w] != 0) { forward = 10; }
                    if (keys[Sdl.SDLK_a] != 0) { strafe = -10; }
                    if (keys[Sdl.SDLK_s] != 0) { forward = -10; }
                    if (keys[Sdl.SDLK_d] != 0) { strafe = 10; }

                    int x, y;
                    byte state = Sdl.SDL_GetMouseState(out x, out y);

                    Voxlap.dpoint3d vec = new Voxlap.dpoint3d()
                    {
                        x = forward * or.ifo.x + strafe * or.ist.x,
                        y = forward * or.ifo.y + strafe * or.ist.y,
                        z = 0
                    };

                    or.ipo = Voxlap.ClipMove(or.ipo, vec, 8.0);

                    //or.ipo.x += forward * or.ifo.x;
                    //or.ipo.y += forward * or.ifo.y;

                    //or.ipo.x += strafe * or.ist.x;
                    //or.ipo.y += strafe * or.ist.y;

                }
                else
                {
                    if (g_Event.type == Sdl.SDL_MOUSEMOTION)
                    {
                        or.ifo = TodPoint3d(Voxlap.Rotate(
                            ToPoint3d(or.ifo),
                            new Voxlap.point3d() { z = 1 },
                            (float)g_Event.motion.xrel / 100f));

                        or.ihe = TodPoint3d(Voxlap.Rotate(
                            ToPoint3d(or.ihe),
                            new Voxlap.point3d() { z = 1 },
                            (float)g_Event.motion.xrel / 100f));

                        or.ist = TodPoint3d(Voxlap.Rotate(
                            ToPoint3d(or.ist),
                            new Voxlap.point3d() { z = 1 },
                            (float)g_Event.motion.xrel / 100f));


                        float newangle = vertAngle + (float)-g_Event.motion.yrel / 100f;

                        if (newangle < Math.PI / 2 && newangle > -Math.PI / 2) 
                        {
                            or.ifo = TodPoint3d(Voxlap.Rotate(
                                ToPoint3d(or.ifo),
                                ToPoint3d(or.ist),
                                (float)-g_Event.motion.yrel / 100f));

                            or.ihe = TodPoint3d(Voxlap.Rotate(
                                ToPoint3d(or.ihe),
                                ToPoint3d(or.ist),
                                (float)-g_Event.motion.yrel / 100f));

                            or.ist = TodPoint3d(Voxlap.Rotate(
                                ToPoint3d(or.ist),
                                ToPoint3d(or.ist),
                                (float)-g_Event.motion.yrel / 100f));

                            vertAngle = newangle;
                        }

                        Sdl.SDL_EventState(Sdl.SDL_MOUSEMOTION, Sdl.SDL_IGNORE);
                        Sdl.SDL_WarpMouse(SCREEN_WIDTH >> 1, SCREEN_HEIGHT >> 1);
                        Sdl.SDL_EventState(Sdl.SDL_MOUSEMOTION, Sdl.SDL_ENABLE);

                    }

                    if (g_Event.type == Sdl.SDL_MOUSEBUTTONDOWN)
                    {
                        if (g_Event.button.button == Sdl.SDL_BUTTON_WHEELUP)
                        {
                            or.ipo.z -= 10;
                        }

                        if (g_Event.button.button == Sdl.SDL_BUTTON_WHEELDOWN)
                        {
                            or.ipo.z += 10;
                        }
                    }                    

                    if (g_Event.type == Sdl.SDL_KEYDOWN)
                    {
                        if (g_Event.key.keysym.sym == Sdl.SDLK_ESCAPE)
                        {
                            break;
                        }
                    }

                    if (g_Event.type == Sdl.SDL_QUIT) break;

                }
            }

            Sdl.SDL_Quit();
        }

        private static Voxlap.point3d ToPoint3d(Voxlap.dpoint3d p)
        {
            return new Voxlap.point3d()
            {
                x = (float)p.x,
                y = (float)p.y,
                z = (float)p.z,
            };
        }

        private static Voxlap.dpoint3d TodPoint3d(Voxlap.point3d p)
        {
            return new Voxlap.dpoint3d()
            {
                x = (double)p.x,
                y = (double)p.y,
                z = (double)p.z,
            };
        }

        static Voxlap.Orientation or;
        static void Init()
        {
            int a = Voxlap.Initialize();
            Voxlap.AddStack("voxdata.zip");
            or = Voxlap.LoadNull();
            Voxlap.LoadSky("png/TOONSKY.JPG");
            //or = Voxlap.LoadVoxel("vxl/untitled.vxl");
            Voxlap.SetLightingMode(Voxlap.LightingType.Multipointsource);

            or = new Voxlap.Orientation()
            {
                ifo = new Voxlap.dpoint3d() { x = 1, y = 0, z = 0 },
                ihe = new Voxlap.dpoint3d() { x = 0, y = 0, z = 1 },
                ist = new Voxlap.dpoint3d() { x = 0, y = 1, z = 0 },
                ipo = new Voxlap.dpoint3d() { x = 50, y = 50, z = 256-200}
            };

            Voxlap.ClearRect(

                new Voxlap.lpoint3d()
                {
                    x = 0,
                    y = 0,
                    z = 0
                },

                new Voxlap.lpoint3d()
                {
                    x = 2047,
                    y = 2047,
                    z = 255
                });

            Voxlap.UpdateVxl();

            Voxlap.SetRectWithWoodTexture(

                new Voxlap.lpoint3d()
                {
                    x = 0,
                    y = 0,
                    z = 256-8
                },

                new Voxlap.lpoint3d()
                {
                    x = 2047,
                    y = 2047,
                    z = 255
                },
                Color.BurlyWood);

            Voxlap.SetRectWithWoodTexture(

                new Voxlap.lpoint3d()
                {
                    x = 1000,
                    y = 2047,
                    z = -100
                },

                new Voxlap.lpoint3d()
                {
                    x = 2047,
                    y = 2047,
                    z = 255
                },
                Color.BurlyWood);

            Voxlap.SetMaxScanDistToMax();
            Voxlap.SetMipUse(10);
            Voxlap.UpdateVxl();

            //or.ipo.z -= 70;
        }

        static Voxlap.lpoint3d p1 = new Voxlap.lpoint3d() { x = 356, y = 256, z = 216 };
        static Voxlap.lpoint3d p2 = new Voxlap.lpoint3d() { x = 263, y = 263, z = 223 };

        static Stopwatch sw = new Stopwatch();
        static void DoFrame()
        {

            sw.Start();

            bool hit = false;
            Voxlap.lpoint3d loc;
            Color voxcolor;
            Voxlap.Face face;
            int x, y;
            Voxlap.ClearRect(p1, p2);
            Voxlap.SetRectWithWoodTexture(p1, p2, Color.BurlyWood);
            if (Voxlap.Hitscan(or.ipo, or.ifo, out loc, out voxcolor, out face) == Voxlap.HitStatus.Hit)
            {

                double xdist = or.ipo.x - loc.x;
                double ydist = or.ipo.y - loc.y;
                double zdist = or.ipo.z - loc.z;
                double dist = Math.Sqrt(xdist * xdist + ydist * ydist + zdist * zdist);
                if (dist < 40)
                {
                    p1 = new Voxlap.lpoint3d() { x = loc.x & 0x7ffffff8, y = loc.y & 0x7ffffff8, z = loc.z & 0x7ffffff8 };
                    p2 = new Voxlap.lpoint3d() { x = p1.x + 7, y = p1.y + 7, z = p1.z + 7 };
                    Voxlap.ClearRect(p1, p2);

                    Voxlap.SetRectWithWoodTexture(p1, p2, Color.BurlyWood, 200);

                }
                hit = true;
            }
            Voxlap.UpdateVxl();


            Sdl.SDL_LockSurface(g_pDisplaySurface);


            Voxlap.SetFrameBuffer(g_DisplaySurface.pixels, g_DisplaySurface.pitch, g_DisplaySurface.w, g_DisplaySurface.h);
            Voxlap.SetCamera(
                or.ipo, 
                or.ist, 
                or.ihe, 
                or.ifo, 
                g_DisplaySurface.w * .5f, 
                g_DisplaySurface.h * .5f, 
                g_DisplaySurface.w * .5f);
            Voxlap.Opticast();

            sw.Stop();


            Voxlap.Print(10, 10, Color.White, Color.Transparent, "x={0} y={1} z={2}", or.ipo.x, or.ipo.y, or.ipo.z);
            Voxlap.Print(10, 20, Color.White, Color.Transparent, "{0} fps", sw.ElapsedMilliseconds == 0 ? "Infinity" : (1000 / sw.ElapsedMilliseconds).ToString());
            if (hit)
            {
                Voxlap.Print(10, 30, Color.White, Color.Transparent, "HIT x={0} y={1} z={2} face={3}", loc.x, loc.y, loc.z, face);
            }
            sw.Reset();

            Sdl.SDL_UnlockSurface(g_pDisplaySurface);

            SdlGfx.lineColor(g_pDisplaySurface, SCREEN_WIDTH / 2 - 10, SCREEN_HEIGHT / 2, SCREEN_WIDTH / 2 + 10, SCREEN_HEIGHT / 2, -1);
            SdlGfx.lineColor(g_pDisplaySurface, SCREEN_WIDTH / 2, SCREEN_HEIGHT / 2 - 10, SCREEN_WIDTH / 2, SCREEN_HEIGHT / 2 + 10, -1);
        }
    }
}
