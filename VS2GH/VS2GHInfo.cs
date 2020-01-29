using System;
using System.Drawing;
using Grasshopper.Kernel;

namespace VS2GH
{
    public class VS2GHInfo : GH_AssemblyInfo
    {
        public override string Name
        {
            get
            {
                return "VS2GH";
            }
        }
        public override Bitmap Icon
        {
            get
            {
                //Return a 24x24 pixel bitmap to represent this GHA library.
                return null;
            }
        }
        public override string Description
        {
            get
            {
                //Return a short string describing the purpose of this GHA library.
                return "";
            }
        }
        public override Guid Id
        {
            get
            {
                return new Guid("794e0429-5dbb-4f74-84a4-ab5ebf1c3b8a");
            }
        }

        public override string AuthorName
        {
            get
            {
                //Return a string identifying you or your company.
                return "";
            }
        }
        public override string AuthorContact
        {
            get
            {
                //Return a string representing your preferred contact details.
                return "";
            }
        }
    }
}
