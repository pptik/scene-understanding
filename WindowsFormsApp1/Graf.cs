using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alturos.Yolo;
using Alturos.Yolo.Model;
using System.Windows.Forms;
using System.Drawing;

namespace WindowsFormsApp1
{
    class Graf
    {
        public void makeH(List<YoloItem> items)
        {
            System.Windows.Forms.Form form = new System.Windows.Forms.Form();
            Microsoft.Msagl.GraphViewerGdi.GViewer viewer = new Microsoft.Msagl.GraphViewerGdi.GViewer();
            Microsoft.Msagl.Drawing.Graph graph = new Microsoft.Msagl.Drawing.Graph("graph");

            foreach (var item in items)
            {
                String result = item.Type + " : " + item.Confidence.ToString("#0.##");
                graph.AddEdge("Tframe",result );
                graph.AddEdge("Tframe-H",item.Type).Attr.Color = Microsoft.Msagl.Drawing.Color.Green;
                graph.FindNode("Tframe-H").Attr.FillColor = Microsoft.Msagl.Drawing.Color.GreenYellow;
            }
            viewer.Graph = graph;
            form.SuspendLayout();
            viewer.Dock = System.Windows.Forms.DockStyle.Fill;
            form.Controls.Add(viewer);
            form.Text = "Graf H : OBSERVASI";
            form.StartPosition = FormStartPosition.CenterScreen;
            form.ResumeLayout();
            form.ShowDialog();
        }
        public void makeHostGraf(List<YoloItem> items, string scene)
        {
            System.Windows.Forms.Form formHost = new System.Windows.Forms.Form();
            Microsoft.Msagl.GraphViewerGdi.GViewer viewer = new Microsoft.Msagl.GraphViewerGdi.GViewer();
            Microsoft.Msagl.Drawing.Graph graph = new Microsoft.Msagl.Drawing.Graph("Host graph");
            graph.AddNode(scene);
            foreach (var item in items)
            {

                String box ="BOX : " + item.X + "," + item.Y;
                graph.AddEdge(scene, box);
                Point p = item.Center();
                String O = item.Type + " : " + p.X + "," + p.Y;
                graph.AddEdge(box, O);
                //graph.AddEdge(item.Type, "Tframe-H").Attr.Color = Microsoft.Msagl.Drawing.Color.Green;
                //graph.FindNode("Tframe-H").Attr.FillColor = Microsoft.Msagl.Drawing.Color.GreenYellow;
            }
            viewer.Graph = graph;
            formHost.SuspendLayout();
            viewer.Dock = System.Windows.Forms.DockStyle.Fill;
            formHost.Controls.Add(viewer);
            formHost.Text = "Graf Spasial";
            formHost.StartPosition = FormStartPosition.CenterScreen;
            formHost.ResumeLayout();
            formHost.ShowDialog();

        }
        public void makeProduction(List<Arah> arah_n, List<YoloItem> items, string scenename)
        {
            Arah a = new Arah();
            Stack<NodeArah> nodearahstack = new Stack<NodeArah>();
            List<NodeArah> nodearah = new List<NodeArah>();
            // List<double> distance = new List<double>();
            //string cek_distance = "";
            NodeArah node_spasial = new NodeArah();
            double min_distance = 0;
            Point pusat_arah = new Point();
            Point pusat_object = new Point();
            double[] distance = new double[arah_n.Count];

            System.Windows.Forms.Form form = new System.Windows.Forms.Form();
            Microsoft.Msagl.GraphViewerGdi.GViewer viewer = new Microsoft.Msagl.GraphViewerGdi.GViewer();
            Microsoft.Msagl.Drawing.Graph graph = new Microsoft.Msagl.Drawing.Graph("graph");
           
            foreach (var item in items)
            {
                int i = -1;
                foreach (var arah in arah_n)
                {
                    i++;
                    pusat_arah = arah.Pusat();
                    pusat_object = item.Center();
                    distance[i] = Math.Sqrt(Math.Pow((pusat_arah.X - pusat_object.X), 2) + Math.Pow((pusat_arah.Y - pusat_object.Y), 2));
                    if (i == 0)
                    {
                        min_distance = distance[i];
                        a = arah;
                    }
                    if (min_distance > distance[i])
                    {
                        min_distance = distance[i];
                        a = arah;
                    }
                   
                }
                node_spasial.object_node = item;
                 node_spasial.arahnode = a;
                // TIDAK KE PUSH PADA STACK
                //nodearah.
                nodearahstack.Push(node_spasial);
                //graph.AddEdge(scenename, node_spasial.arahnode.ArahSpasial);
                Point P = item.Center();
                String result = item.Type + " : " + P.X + "-" + P.Y;
                graph.AddEdge(scenename, node_spasial.arahnode.ArahSpasial).Attr.Color = Microsoft.Msagl.Drawing.Color.Green;
                graph.AddEdge(node_spasial.arahnode.ArahSpasial, result).Attr.Color = Microsoft.Msagl.Drawing.Color.Green;
            }
            viewer.Graph = graph;
            form.SuspendLayout();
            viewer.Dock = System.Windows.Forms.DockStyle.Fill;
            form.Controls.Add(viewer);
            form.Text = "Graf Spasial : Tata Letak";
            form.StartPosition = FormStartPosition.CenterScreen;
            form.ResumeLayout();
            form.ShowDialog();
            

        }

    }
}
