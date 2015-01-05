using System;
using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Hardware;
using Android.Media;
using Android.Util;

[assembly: UsesFeature("android.hardware.camera")]

namespace xamavision
{

	[Activity (Label = "mycamera", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity, TextureView.ISurfaceTextureListener, Camera.IPictureCallback, Camera.IPreviewCallback
	{
		int count = 1;
		Android.Hardware.Camera _camera;
		TextureView _textureView;
		ImageView _imageView;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView (Resource.Layout.Main);

			Button button = FindViewById<Button> (Resource.Id.myButton);
			
			button.Click += delegate {
				//_camera.TakePicture(null,  null, null,new DelegatePictureCallback());;
				_camera.SetOneShotPreviewCallback(this);
				button.Text = string.Format ("{0} clicks!", count++);
			};

			_imageView = FindViewById<ImageView> (Resource.Id.imageView1);
			_textureView = FindViewById<TextureView> (Resource.Id.myTextureView);
			_textureView.SurfaceTextureListener = this;

		}

		#region ISurfaceTextureListener

		public void OnSurfaceTextureAvailable (Android.Graphics.SurfaceTexture surface, int w, int h)
		{
			_camera = Camera.Open ();

			try {
				_camera.SetPreviewTexture (surface);
				var p = _camera.GetParameters();
				//p.PreviewFormat = Android.Graphics.ImageFormatType.Nv21;
				_camera.SetParameters(p);
				_camera.StartPreview ();

			} catch (Java.IO.IOException ex) {
				Console.WriteLine (ex.Message);
			}
		}

		public bool OnSurfaceTextureDestroyed (Android.Graphics.SurfaceTexture surface)
		{
			_camera.StopPreview ();
			_camera.Release ();

			return true;
		}

		public void OnSurfaceTextureSizeChanged (Android.Graphics.SurfaceTexture surface, int width, int height)
		{
			// camera takes care of this
		}

		public void OnSurfaceTextureUpdated (Android.Graphics.SurfaceTexture surface)
		{
			count++;
			Console.WriteLine (count);
		}

		#endregion
			
		#region IPictureCallback

		public void OnPictureTaken(byte[] data, Camera camera)
		{
			int[] aa = new int[640*480];
			Android.Graphics.Bitmap bmp = Android.Graphics.BitmapFactory.DecodeByteArray(data, 0, data.Length);
			bmp.GetPixels (aa, 0, 640, 0, 0, 640, 480);
			camera.StartPreview ();

		}
		#endregion

		#region IPreviewCallback

		private int[] decodeYUV420SP(byte[] yuv420sp, int width, int height) {

			int[] res = new int[width*height];
			int frameSize = width * height;

			for (int j = 0, yp = 0; j < height; j++) {
				int uvp = frameSize + (j >> 1) * width, u = 0, v = 0;
				for (int i = 0; i < width; i++, yp++) {
					int y = (0xff & ((int) yuv420sp[yp])) - 16;
					if (y < 0) y = 0;
					if ((i & 1) == 0) {
						try{
							v = (0xff & yuv420sp[uvp++]) - 128;
							u = (0xff & yuv420sp[uvp++]) - 128;
						}catch(Exception e){

						}
					}
					int y1192 = 1192 * y;
					int r = (y1192 + 1634 * v);
					int g = (y1192 - 833 * v - 400 * u);
					int b = (y1192 + 2066 * u);
					if (r < 0) r = 0; else if (r > 262143) r = 262143;
					if (g < 0) g = 0; else if (g > 262143) g = 262143;
					if (b < 0) b = 0; else if (b > 262143) b = 262143;
					res[yp] =(int)(0xff000000 | ((r << 6) & 0xff0000) | ((g >> 2) & 0xff00) | ((b >> 10) & 0xff));
				}
			}
			return res;
		}   

		public void OnPreviewFrame(byte[] data, Camera camera)
		{
			int mWidth = camera.GetParameters().PreviewSize.Width;
			int mHeight = camera.GetParameters().PreviewSize.Height;
			int[] mIntArray;// = new int[mWidth * mHeight];

			mIntArray = decodeYUV420SP(data, mWidth, mHeight);

			Android.Graphics.Bitmap bitmap = Android.Graphics.Bitmap.CreateBitmap(mWidth, mHeight, Android.Graphics.Bitmap.Config.Argb4444);
			bitmap.SetPixels (mIntArray, 0, mWidth, 0 , 0 , mWidth, mHeight);

			_imageView.SetImageBitmap (bitmap);
		}

		#endregion
	}

}


