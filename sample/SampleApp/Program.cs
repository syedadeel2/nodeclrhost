﻿namespace SampleApp
{
    using System.Threading.Tasks;
    using System;
    using NodeHostEnvironment;
    
    class Program
    {
        static int Main(string[] args)
        {
            var host = NodeHost.InProcess();
            var console = host.Global.console;
            console.log("Starting timeout");
            host.Global.setTimeout(new Action(() => 
                                    {
                                        console.log("Timeout from node");
                                        host.Dispose();
                                    }),
                                   1500);
            //RunAsyncApp(host);
            return 5;
        }

        private static async void RunAsyncApp(NodeHost host)
        {
            try
            {
                host.Global.console.log($"Hello world from pid:{host.Global.process.pid}!");

                await host.Run(async() =>
                {
                    var global = host.Global;
                    var console = global.console;

                    console.log("Dynamic log from .Net is ", true);

                    console.log("TestClass", global.TestClass.CreateNewInstance("Hallo ctor argument"));

                    global.testCallback(new Func<string, string, string>(MarshalledDelegate), "SecondArg", "ThirdArg");
                    //global.gc();

                    await Task.Delay(100);

                    console.log("DELAYED");

                    var dynInstance = host.New();
                    dynInstance.dynamicProperty1 = "DynProp1";
                    dynInstance.dynamicProperty2 = new Func<string, string, string>(MarshalledDelegate2);
                    // TODO: Why can we not read from the dynamic properties? e.g. dynInstance.dynamicProperty1

                    //global.gc();

                    global.testCallback(new Func<string, string, string>(MarshalledDelegate2), dynInstance, "ThirdArg2");

                    await Task.Delay(100);
                    //global.gc();

                    global.testCallback(new Func<string, string, string>(MarshalledDelegate), "3", dynInstance);
                    //global.gc();

                    global.testCallback(new Func<string, string, string>((a,b) => { console.log("asdas"); return null;}), "3", dynInstance);

                    var tcs = new TaskCompletionSource<object>();
                    global.callLater(new Action(() => { console.log("We have been called later"); tcs.SetResult(null); }));

                    Console.WriteLine($"Int from JS {(int)global.testAddon.a}");
                    await tcs.Task;
                });

            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: {0}", e);
            }
            finally
            {
                // This will lead to node closing and future callbacks being rejected
                host.Dispose();
            }
        }

        private static string MarshalledDelegate(string a, string b)
        {
            return $".NET has been called with {a ?? "null"} & {b}";
        }

        private static string MarshalledDelegate2(string a, string b)
        {
            return $"2: We have been called with {a ?? "null"} & {b}";
        }
    }
}