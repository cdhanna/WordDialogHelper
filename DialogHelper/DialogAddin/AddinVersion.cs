using System;

namespace DialogAddin
{
    public static class AddinVersion
    {
        public static string VERSION { get
            {
                return "no version number";

                //Assembly ass = Assembly.GetAssembly(typeof(AddinVersion));
                //string version;
                //if (ass != null)
                //{
                //    FileVersionInfo FVI = FileVersionInfo.GetVersionInfo(ass.Location);
                //    version = String.Format("{0} Version ({1:0}.{2:0})",
                //                  FVI.ProductName,
                //                  FVI.FileMajorPart.ToString(),
                //                  FVI.FileMinorPart.ToString());
                //}
                //else
                //{
                //    version = "Unknown";
                //}
                //return version;
            }
        }
    }
}
