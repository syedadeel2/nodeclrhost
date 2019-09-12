using System;
using System.Runtime.InteropServices;

namespace NodeHostEnvironment.NativeApi
{
    public sealed class DelegateBasedNativeApi
    {
        public DelegateBasedNativeApi(GetContext getContext,
            ReleaseContext releaseContext,
            PostCallback postCallback,
            GetMember getMember,
            SetMember setMember,
            Invoke invoke,
            CreateObject createObject,
            Release release)
        {
            GetContext = getContext;
            ReleaseContext = releaseContext;
            PostCallback = postCallback;
            GetMember = getMember;
            SetMember = setMember;
            Invoke = invoke;
            CreateObject = createObject;
            Release = release;
        }

        public GetContext GetContext { get; }
        public ReleaseContext ReleaseContext { get; }
        public PostCallback PostCallback { get; }
        public GetMember GetMember { get; }
        public SetMember SetMember { get; }
        public Invoke Invoke { get; }
        public CreateObject CreateObject { get; }
        public Release Release { get; }

    }

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate IntPtr GetContext();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void ReleaseContext(IntPtr context);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int PostCallback(IntPtr context, NodeCallback callback, IntPtr data);

    // Get a handle, ownerHandler == Zero => Global
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate JsValue GetMember(IntPtr context, JsValue ownerHandle, [MarshalAs(UnmanagedType.LPStr)] string name); // A zero handle uses the global object.

    // Convert handles to primitives can be done in managed code based on JsType
    // ATTENTION: 32bit node exists :(

    // Set a member
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate JsValue SetMember(IntPtr context, JsValue ownerHandle, [MarshalAs(UnmanagedType.LPStr)] string name, DotNetValue value);

    // Invoke handles that represent functions
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate JsValue Invoke(IntPtr context, JsValue handle, JsValue receiverHandle, int argc, DotNetValue[] argv);
    /*
            // Create a JSON object
            [DllImport("coreclr-hosting.node", CallingConvention = CallingConvention.Cdecl)]
            public extern static JsHandle CreateJsonObject(int argc, [MarshalAs(UnmanagedType.LPStr)] string[] argn, DotNetValue[] argv);
            
            [DllImport("coreclr-hosting.node", CallingConvention = CallingConvention.Cdecl)]
            public extern static JsHandle CreateJsonObject(string json);
             */

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate JsValue CreateObject(IntPtr context, JsValue constructor, int argc, DotNetValue[] argv); // We use SetMember to define members

    // Release a handle
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void Release(JsValue handle);
}