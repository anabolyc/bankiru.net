using System;
using System.Net;
using System.Reflection;
using NUnit.Framework;

namespace www.BankBals.Tests {

    public static class Tools {

        public static HttpStatusCode GetHeaders(string url, string Method) {
            Console.WriteLine("Fetching: {0}: {1}", Method, url);
            HttpStatusCode result = default(HttpStatusCode);

            var request = HttpWebRequest.Create(url);
            request.Method = Method;
            request.ContentLength = 0;
            using (var response = request.GetResponse() as HttpWebResponse) {
                if (response != null) {
                    result = response.StatusCode;
                    response.Close();
                }
            }

            return result;
        }

        public static HttpStatusCode GetHeaders(string url) {
            return GetHeaders(url, WebRequestMethods.Http.Head);
        }

        public static void TestResultCode(Type myType) {
            MethodInfo[] methodInfos = myType.GetMethods(BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Instance);
            foreach (MethodInfo mi in methodInfos) {
                string controllerPath = myType.Name.Replace("Controller", String.Empty);
                string url = String.Format("{0}/{1}/{2}/", AccountTest.SERVER_ADDRESS, controllerPath, mi.Name);
                Console.WriteLine("{0}.{1}", myType.Name, mi.Name);
                
                string param = String.Empty;
                foreach (ParameterInfo pi in mi.GetParameters()) {
                    if (!pi.IsOptional) {
                        Console.WriteLine("\t({1})\t{0} ", pi.Name, pi.ParameterType);
                        string testValue = string.Empty;
                        switch (pi.Name.ToLower()) { 
                            case "viewid":
                            case "bankid":
                                testValue = "1";
                                break;
                            case "viewitemticker":
                                testValue = "As:Cash";
                                break;
                            case "aggitemid":
                            case "viewitem":
                                testValue = "1620";
                                break;
                            case "type":
                                testValue = "text";
                                break;
                            case "id":
                                testValue = "20202";
                                break;
                            case "dateid":
                                testValue = "469";
                                break;
                            default:
                                break;
                        }
                        param += String.Format("{0}={1}&", pi.Name, testValue);
                    }
                }
                if (param != String.Empty) {
                    url += "?" + param.Substring(0, param.Length - 1);
                }

                if (mi.GetCustomAttributes(typeof(System.Web.Mvc.HttpPostAttribute), true).Length > 0)
                    Assert.AreEqual(HttpStatusCode.OK, Tools.GetHeaders(url, WebRequestMethods.Http.Post), mi.Name);
                else
                    Assert.AreEqual(HttpStatusCode.OK, Tools.GetHeaders(url, WebRequestMethods.Http.Get), mi.Name);
            }
        }
    }
}




