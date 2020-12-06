using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace TestConsoleApp
{
    class Program
    {
        // static uint White = 0x00FFFFFF;

        public static byte[] Encode (Bitmap bmp, bool zeroIsWhite) {
            var bits = bmp.LockBits (new System.Drawing.Rectangle (0, 0, bmp.Width, bmp.Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format1bppIndexed);
            var bytes = Encode (bmp.Width, bmp.Height, bits.Stride, zeroIsWhite, bits.Scan0);
            bmp.UnlockBits (bits);
            return bytes;
        }

        private static byte[] Encode (int width, int height, int stride, bool zeroIsWhite, IntPtr b) {
            int l = 0;
            var r = NativeMethods.jbig2_encode (width, height, stride, zeroIsWhite, b, ref l);
            byte[] result = new byte[l];
            Marshal.Copy (r, result, 0, l);
            NativeMethods.release (r);
            return result;
        }

        class NativeMethods
        {
            [DllImport ("./jbig2enc.dll")]
            internal static extern IntPtr jbig2_encode (int width, int height, int stride, bool zeroIsWhite, IntPtr data, ref int length);

            [DllImport ("./jbig2enc.dll")]
            internal static extern IntPtr release (IntPtr data);
        }
        static void Main(string[] args){
            using( Bitmap bm = new Bitmap("C:\\Users\\Cong Van\\Downloads\\MCN\\TestConsoleApp\\black.bmp",true)){
                Encode(bm,true);
                bm.Save("result.bmp");
            };
        }
    }
}