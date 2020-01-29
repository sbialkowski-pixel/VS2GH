using System;
using System.IO;
using System.Drawing;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Grasshopper.GUI.Canvas;
using Rhino.Geometry;
using ScriptComponents;

// In order to load the result of this wizard, you will also need to
// add the output bin/ folder of this project to the list of loaded
// folder in Grasshopper.
// You can use the _GrasshopperDeveloperSettings Rhino command for that.

namespace VS2GH
{
    public class VS2GHComponent : GH_Component
    {
        public VS2GHComponent() : base("VS2GH ", "VS2GH", "Uses a code file as the code of a scripting component.", "Maths", "Script") { }
        public override GH_Exposure Exposure { get { return GH_Exposure.tertiary | GH_Exposure.obscure; } }
        public override Guid ComponentGuid { get { return new Guid("{10880653-4b44-4ec6-ba7d-95d7e9babfbb}"); } }
        protected override System.Drawing.Bitmap Icon { get { return Properties.Resources.icon; } }
        public override void CreateAttributes()
        {
            base.CreateAttributes();
            base.m_attributes = new Attributes_Custom(this);
        }
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("File", "F", "Path to the code file. Use the File Path parameter with the Syncronize option enabled.", GH_ParamAccess.item);
        }
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager) { }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string file = string.Empty;
            DA.GetData(0, ref file);

            Rectangle bounds = Rectangle.Ceiling(this.Attributes.Bounds);
            var point = new System.Drawing.Point(bounds.X + bounds.Width / 2, bounds.Y - 40);
            var scriptComponent = (Component_CSNET_Script)Grasshopper.Instances.ActiveCanvas.Document.FindComponent(point);

            if (scriptComponent == null)
            {
                this.Message = string.Empty;
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, string.Format("No scripting component attached."));
            }
            else
                this.Message = scriptComponent.Name;

            try
            {
                string code;
                using (StreamReader sr = new StreamReader(file))
                    code = sr.ReadToEnd();

                if (scriptComponent != null)
                {
                    scriptComponent.SourceCodeChanged(new Grasshopper.GUI.Script.GH_ScriptEditor(Grasshopper.GUI.Script.GH_ScriptLanguage.CS));
                    var splitLines = new List<string>();
                    splitLines.Add("// <Custom using>");
                    splitLines.Add("// </Custom using>");
                    splitLines.Add("// <Custom code>");
                    splitLines.Add("// </Custom code>");
                    splitLines.Add("// <Custom additional code>");
                    splitLines.Add("// </Custom additional code>");

                    string[] codes = code.Split(splitLines.ToArray(), StringSplitOptions.None);
                    scriptComponent.ScriptSource.UsingCode = codes[1];
                    scriptComponent.ScriptSource.ScriptCode = codes[3];
                    scriptComponent.ScriptSource.AdditionalCode = codes[5];
                    scriptComponent.ExpireSolution(true);
                }
            }
            catch (Exception e)
            {
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, e.Message);
            }
        }
    }
    public class Attributes_Custom : Grasshopper.Kernel.Attributes.GH_ComponentAttributes
    {
        public Attributes_Custom(GH_Component owner) : base(owner) { }

        protected override void Layout()
        {
            base.Layout();
        }

        protected override void Render(GH_Canvas canvas, System.Drawing.Graphics graphics, GH_CanvasChannel channel)
        {
            base.Render(canvas, graphics, channel);

            if (channel == GH_CanvasChannel.Objects)
            {
                Rectangle rectangle = new Rectangle();
                int diameter = 30;

                rectangle.Y = (int)Bounds.Y - 40 - diameter / 2;
                rectangle.X = (int)Bounds.X + (int)Bounds.Width / 2 - diameter / 2;
                rectangle.Width = diameter;
                rectangle.Height = diameter;

                Pen pen = new Pen(Color.Black, 1);
                pen.DashPattern = new float[] { 1.0f, 3.0f };
                graphics.DrawArc(pen, rectangle, 0, 360);

                var font = new Font(FontFamily.GenericSansSerif, 4.0f, FontStyle.Regular);
                StringFormat format = new StringFormat();
                format.LineAlignment = StringAlignment.Center;
                format.Alignment = StringAlignment.Center;
                graphics.DrawString("place\r\ncomponent\r\nhere", font, Brushes.Black, rectangle, format);
            }
        }
    }
}
