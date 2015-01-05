package mycamera;


public class DelegatePictureCallback
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		android.hardware.Camera.PictureCallback,
		android.hardware.Camera.PreviewCallback
{
	static final String __md_methods;
	static {
		__md_methods = 
			"n_onPictureTaken:([BLandroid/hardware/Camera;)V:GetOnPictureTaken_arrayBLandroid_hardware_Camera_Handler:Android.Hardware.Camera/IPictureCallbackInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"n_onPreviewFrame:([BLandroid/hardware/Camera;)V:GetOnPreviewFrame_arrayBLandroid_hardware_Camera_Handler:Android.Hardware.Camera/IPreviewCallbackInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"";
		mono.android.Runtime.register ("mycamera.DelegatePictureCallback, mycamera, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", DelegatePictureCallback.class, __md_methods);
	}


	public DelegatePictureCallback () throws java.lang.Throwable
	{
		super ();
		if (getClass () == DelegatePictureCallback.class)
			mono.android.TypeManager.Activate ("mycamera.DelegatePictureCallback, mycamera, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onPictureTaken (byte[] p0, android.hardware.Camera p1)
	{
		n_onPictureTaken (p0, p1);
	}

	private native void n_onPictureTaken (byte[] p0, android.hardware.Camera p1);


	public void onPreviewFrame (byte[] p0, android.hardware.Camera p1)
	{
		n_onPreviewFrame (p0, p1);
	}

	private native void n_onPreviewFrame (byte[] p0, android.hardware.Camera p1);

	java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
