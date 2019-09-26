using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Alturos.Yolo;
using Alturos.Yolo.Model;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

namespace WindowsFormsApp1
{
    public partial class SUN : Form
    {
        VideoCapture cap;
        YoloWrapper yoloWrapper;
        VideoWriter videoWriter;
       String fileName = String.Format("video_out.mp4");
        int fourcc;
        int width;
        int height;
        
        TrainSUN12CVR13 train;
        List<string> objectL;
        List<string> sceneL;
        List<string> phiLabel;
        List<double> phi = new List<double>();
        double[,] Cmat = new double[49, 3];

        public SUN()
        {
            InitializeComponent();
            var configurationDetector = new ConfigurationDetector();
            yoloWrapper = new YoloWrapper("yolov3.cfg", "yolov3.weights", "coco.names");
            train = new TrainSUN12CVR13();
            setUpData();
        }
        private void setUpData()
        {
            List<string> result = null;
            string[] C = train.getTrain();
            string[] objectLabel = train.getTLabelObject();
            string[] sceneLabel = train.getLabelScene();
            objectL = objectLabel.ToList();
            sceneL = sceneLabel.ToList();

            for (int i = 0; i < 49; i++)
            {
                result = C[i].Split(',').ToList();
                Cmat[i, 0] = Convert.ToDouble(result[0]);
                Cmat[i, 1] = Convert.ToDouble(result[1]);
                Cmat[i, 2] = Convert.ToDouble(result[2]);
                result.Clear();
            }

        }
        private List<Arah> initialisaiArah(Image a)
        {
            List<Arah> arah9 = new List<Arah>();
            int xarah = a.Width / 3;
            int yarah = a.Height / 3;
            Arah barat_laut = new Arah();
            barat_laut.ArahSpasial = "Top Left";
            barat_laut.X = 0; barat_laut.Y = 0;
            barat_laut.LebarArea = xarah; barat_laut.PanjangArea = yarah;
            arah9.Add(barat_laut);

            Arah utara = new Arah();
            utara.ArahSpasial = "TOP";
            utara.X = xarah; utara.Y = 0;
            utara.LebarArea = xarah; utara.PanjangArea = yarah;
            arah9.Add(utara);

            Arah timur_laut = new Arah();
            timur_laut.ArahSpasial = "Top Right";
            timur_laut.X = xarah * 2; timur_laut.Y = 0;
            timur_laut.LebarArea = xarah; timur_laut.PanjangArea = yarah;
            arah9.Add(timur_laut);

            Arah barat = new Arah();
            barat.ArahSpasial = "LEFT";
            barat.X = 0; barat.Y = yarah;
            barat.LebarArea = xarah; barat.PanjangArea = yarah;
            arah9.Add(barat);

            Arah simpul = new Arah();
            simpul.ArahSpasial = "CENTRAL";
            simpul.X = xarah; simpul.Y = yarah;
            simpul.LebarArea = xarah; simpul.PanjangArea = yarah;
            arah9.Add(simpul);

            Arah timur = new Arah();
            timur.ArahSpasial = "RIGHT";
            timur.X = xarah * 2; timur.Y = yarah;
            timur.LebarArea = xarah; timur.PanjangArea = yarah;
            arah9.Add(timur);

            Arah barat_daya = new Arah();
            barat_daya.ArahSpasial = "Bottom Left";
            barat_daya.X = 0; barat_daya.Y = yarah * 2;
            barat_daya.LebarArea = xarah; barat_daya.PanjangArea = yarah;
            arah9.Add(barat_daya);

            Arah selatan = new Arah();
            selatan.ArahSpasial = "BOTTOM";
            selatan.X = xarah; selatan.Y = yarah * 2;
            selatan.LebarArea = xarah; selatan.PanjangArea = yarah;
            arah9.Add(selatan);

            Arah tenggara = new Arah();
            tenggara.ArahSpasial = "Bottom Right";
            tenggara.X = xarah * 2; tenggara.Y = yarah * 2;
            tenggara.LebarArea = xarah; tenggara.PanjangArea = yarah;
            arah9.Add(tenggara);

            return arah9;
        }

        private void bntOpen_Click(object sender, EventArgs e)
        {
            Graf g = new Graf();
            Stack<NodeArah> node = new Stack<NodeArah>();
            OpenFileDialog ofd = new OpenFileDialog();
            if(ofd.ShowDialog() == DialogResult.OK)
            {
      
                pBox.Image = Image.FromFile(ofd.FileName);
                List<YoloItem> items = yoloWrapper.Detect(ofd.FileName).ToList();
                g.makeH(items);              
                phi = parsingObject(items);
                normalPHI(phi);
                string scene=interpretasikan(phi, phiLabel, Cmat);
                DrawBorder2Citra(items,ofd.FileName);
                //MessageBox.Show("SCENE LABLE : "+scene);
                //parsing2GraphH();
                //List<Arah> sarah = this.initialisaiArah(pBox.Image);
                List<Arah> sarah = this.initialisaiArah(Image.FromFile(ofd.FileName));
                //DrawBorder2Arah(sarah, ofd.FileName, scene);
                MessageBox.Show("SCENE LABLE : " + scene);
                DrawBorder2Canvas(items, ofd.FileName,scene);
                MessageBox.Show("Informasi                                                      Spasial ");
                g.makeHostGraf(items, scene);
                g.makeProduction(sarah, items, scene);
                //node=inisialisasiGraphSpasial(sarah, items,scene);
               
            }
        }
        
        private Stack<NodeArah> inisialisasiGraphSpasial(List<Arah> arah_n, List<YoloItem> items, string scenename)
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
           // foreach (var arah in arah_n)
           // {
            //    graph.AddEdge(scenename, arah.ArahSpasial).Attr.Color = Microsoft.Msagl.Drawing.Color.Green;

           // }

                //for (int i=
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
                    //if (string.Equals("TENGAH", a.ArahSpasial, StringComparison.OrdinalIgnoreCase))
                    //{
                       // a.ArahSpasial = a.ArahSpasial + " : " + scenename;
                    //}
                }
                    node_spasial.object_node = item;
                    node_spasial.arahnode = a;
                    // TIDAK KE PUSH PADA STACK
                    //nodearah.
                    nodearahstack.Push(node_spasial);
                //graph.AddEdge(scenename, node_spasial.arahnode.ArahSpasial);
                Point P = item.Center(); 
                    String result = item.Type + " : " + P.X+"-"+P.Y;
              graph.AddEdge(scenename, node_spasial.arahnode.ArahSpasial).Attr.Color = Microsoft.Msagl.Drawing.Color.Green;
              graph.AddEdge(node_spasial.arahnode.ArahSpasial, result).Attr.Color = Microsoft.Msagl.Drawing.Color.Green;
                // graph.AddEdge(node_spasial.arahnode.ArahSpasial, item.Type).Attr.Color = Microsoft.Msagl.Drawing.Color.Green;
                //MessageBox.Show(node_spasial.object_node.Type+" terdapat pada "+ node_spasial.arahnode.ArahSpasial);
                //Graf Spasial


                /* foreach (var item in items)
                 {
                     String result = item.Type + " : " + item.Confidence.ToString("#0.##");
                     graph.AddEdge(result, "Tframe");
                     graph.AddEdge(item.Type, "Tframe-H").Attr.Color = Microsoft.Msagl.Drawing.Color.Green;
                     graph.FindNode("Tframe-H").Attr.FillColor = Microsoft.Msagl.Drawing.Color.GreenYellow;
                 }*/

                //end Graf

            }
                viewer.Graph = graph;
                form.SuspendLayout();
                viewer.Dock = System.Windows.Forms.DockStyle.Fill;
                form.Controls.Add(viewer);
                form.Text = "Graf H : OBSERVASI";
                form.StartPosition = FormStartPosition.CenterScreen;
                form.ResumeLayout();
                form.ShowDialog();
                return nodearahstack;
            
        }
        private async void btnPlay_Click(object sender, EventArgs e)
        {
            btnClose.Enabled = true;
            btnPlay.Enabled = false;
            if (radioVidieo.Checked)
            {
                OpenFileDialog ofd = new OpenFileDialog();
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    cap = new VideoCapture(ofd.FileName);
                } 
            }else if (radioKamera.Checked)
            {
                cap = new VideoCapture(0);
            }
            prosesData();
            
        }

        private async void prosesData()
        {
            fourcc = Convert.ToInt32(cap.GetCaptureProperty(CapProp.FourCC));
            width = Convert.ToInt32(cap.GetCaptureProperty(CapProp.FrameWidth));
            height = Convert.ToInt32(cap.GetCaptureProperty(CapProp.FrameHeight));

            videoWriter = new VideoWriter(fileName, fourcc, 25, new Size(width, height), true);
            while (cap != null)
            {
                Mat mat = new Mat();

                cap.Read(mat);


                if (!mat.IsEmpty)
                {
                    pBox.Image = mat.Bitmap;

                    var ms = new MemoryStream();
                    pBox.Image.Save(ms, ImageFormat.Jpeg);

                    var items = yoloWrapper.Detect(ms.ToArray()).ToList();
                    
                    DrawBorder2Image(items, mat);

                    double fps = cap.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.Fps);
                    label2.Text = "FPS: " + fps;

                    await Task.Delay(1000 / (int)fps);

                }
                else
                {
                    videoWriter.Dispose();
                    break;
                }
            }
        }

        private Pen GetBrush(double confidence, int width)
        {
            var size = width / 100;

            if (confidence > 0.5)
            {
                return new Pen(Brushes.GreenYellow, size);
            }
            else if (confidence > 0.45 && confidence <= 0.5)
            {
                return new Pen(Brushes.Orange, size);
            }

            return new Pen(Brushes.DarkRed, size);
        }

        private void DrawBorder2Image(List<YoloItem> items,Mat mat, YoloItem selectedItem = null)
        {
            var image = mat.Bitmap;
            
            using (var canvas = Graphics.FromImage(image))
            {

                foreach (var item in items)
                {
                    var x = item.X;
                    var y = item.Y;
                    var width = item.Width;
                    var height = item.Height;
                    
                    using (var overlayBrush = new SolidBrush(Color.FromArgb(150, 255, 255, 102)))
                    using (var pen = this.GetBrush(item.Confidence, image.Width))
                    {
                        if (item.Equals(selectedItem))
                        {
                            canvas.FillRectangle(overlayBrush, x, y, width, height);
                            
                        }
                        //MessageBox.Show(x + "," + y + " " + item.Type + "  Jumlah Object " + items.Count);
                        canvas.DrawRectangle(pen, x, y, width, height);
                        String result = item.Type + " : " + item.Confidence.ToString("#0.##");
                        System.Drawing.Font drawFont = new System.Drawing.Font("Arial", 16);

                        StringFormat drawFormat = new StringFormat();
                        drawFormat.Alignment = StringAlignment.Center;
                        canvas.DrawString(result, drawFont, Brushes.Red, x, y);
                        canvas.Flush();
                    }
                }
            }
            pBox.Image = mat.Bitmap;
            videoWriter.Write(mat);
        }

        private void DrawBorder2Arah(List<Arah> items, string filename,string scene)
        {
            var image = Image.FromFile(filename);
            using (var canvas = Graphics.FromImage(image))
            {
                //canvas.Clear(Color.White);
                //MessageBox.Show("Jumlah :" + items.Count);
                foreach (var item in items)
                {
                    var x = item.X;
                    var y = item.Y;
                    var width = item.LebarArea;
                    var height = item.PanjangArea;
                    using (var overlayBrush = new SolidBrush(Color.FromArgb(150, 255, 255, 102)))
                    using (var pen = this.GetBrush(0.60, image.Width))
                    {

                        // canvas.DrawRectangle(new Pen(Brushes.DarkRed, image.Width / 100), image.Width / 2, image.Height / 2, 5, 5);
                        var font = new Font("Arial", 10, FontStyle.Bold);
                        StringFormat drawFormat = new StringFormat();
                        drawFormat.Alignment = StringAlignment.Center;
                        canvas.DrawRectangle(pen, x, y, width, height);
                        canvas.DrawRectangle(pen, x + width / 2, y + height / 2, 5, 5);
                        if (string.Equals("SIMPUL", item.ArahSpasial, StringComparison.OrdinalIgnoreCase))
                        {
                            canvas.DrawString(scene.ToUpper(), font, Brushes.White, x, height+height/2);
                        }
                        else
                        {
                            canvas.DrawString(item.ArahSpasial, font, Brushes.Red, x, y);
                        }
                            
                        canvas.Flush();
                    }
                }
            }
            var oldImage = this.pBox.Image;
            this.pBox.Image = image;
            oldImage?.Dispose();
        }

        private void DrawBorder2Canvas(List<YoloItem> items, string fname, string sname)
        {

            var image = Image.FromFile(fname);
            int xBidang = image.Width / 3;
            int yBidang = image.Height / 3;
            using (var canvas = Graphics.FromImage(image))
            {
                canvas.Clear(Color.White);
                var font = new Font("Arial", 10, FontStyle.Bold);
                StringFormat drawFormat = new StringFormat();
                drawFormat.Alignment = StringAlignment.Center;
                canvas.DrawString(sname, font, Brushes.Red, image.Width / 2-20, image.Height / 2-20);
                int i = 0;
                foreach (var item in items)
                {
                    i++;
                    var x = item.X;
                    var y = item.Y;
                    var width = item.Width;
                    var height = item.Height;
                    canvas.DrawLine(new Pen(Brushes.BlueViolet, 1), xBidang, 0, xBidang, image.Height);
                    canvas.DrawLine(new Pen(Brushes.BlueViolet, 1), xBidang * 2, 0, xBidang * 2, image.Height);
                    canvas.DrawLine(new Pen(Brushes.BlueViolet, 1), 0, yBidang, image.Width, yBidang);
                    canvas.DrawLine(new Pen(Brushes.BlueViolet, 1), 0, yBidang * 2, image.Width, yBidang * 2);
                    using (var overlayBrush = new SolidBrush(Color.FromArgb(150, 255, 255, 102)))
                    using (var pen = this.GetBrush(item.Confidence, image.Width))
                    { 
                        canvas.DrawRectangle(new Pen(Brushes.DarkRed, image.Width / 100), image.Width / 2, image.Height / 2, 5, 5);
                        //var font = new Font("Arial", image.Width / 20, FontStyle.Bold);
                        //StringFormat drawFormat = new StringFormat();
                        //drawFormat.Alignment = StringAlignment.Center;
                        //canvas.DrawString("TFrame", font, Brushes.Red, x = image.Width / 2, y = image.Height / 2);

                        String result = "O" + i;
                        System.Drawing.Font drawFont = new System.Drawing.Font("Arial", 10);

                        //StringFormat drawFormat = new StringFormat();
                        

                        //canvas.DrawRectangle(pen, x, y, width, height);
                        canvas.DrawRectangle(pen, x + width / 2, y + height / 2, 5, 5);
                        canvas.DrawLine(new Pen(Brushes.DarkRed, 2), x + width / 2, y + height / 2, image.Width / 2, image.Height / 2);
                        //Ukuran biasa
                        //canvas.DrawLine(new Pen(Brushes.DarkRed, image.Width / 100), x + width / 2, y + height / 2, image.Width / 2, image.Height / 2);
                        drawFormat.Alignment = StringAlignment.Center;
                        canvas.DrawString(result, drawFont, Brushes.Black, x + width / 2, y + height / 2);
                        canvas.Flush();
                    }
                }
            }
            var oldImage = this.pBox.Image;
            this.pBox.Image = image;
            oldImage?.Dispose();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            cap = null;
            videoWriter.Dispose();
            videoWriter = null;
            radioKamera.Checked = false; radioVidieo.Checked = false;
            btnPlay.Enabled = false;btnClose.Enabled = false;
            pBox.Image = WindowsFormsApp1.Properties.Resources.Arah;
        }

        private void SUN_FormClosing(object sender, FormClosingEventArgs e)
        {
            yoloWrapper.Dispose();
        }

        private void radioVidieo_CheckedChanged(object sender, EventArgs e)
        {
            btnPlay.Enabled = true;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            btnPlay.Enabled = true;
        }
        private void DrawBorder2Citra(List<YoloItem> items, string name, YoloItem selectedItem = null)
        {
            //var imageInfo = this.GetCurrentImage();
            //Load the image(probably from your stream)
            var image = Image.FromFile(name);
            //MessageBox.Show("x:" + image.Size.ToString());
            using (var canvas = Graphics.FromImage(image))
            {
                //canvas.DrawImage
                foreach (var item in items)
                {
                    var x = item.X;
                    var y = item.Y;
                    var width = item.Width;
                    var height = item.Height;

                    using (var overlayBrush = new SolidBrush(Color.FromArgb(150, 255, 255, 102)))
                    using (var pen = this.GetBrush(item.Confidence, image.Width))
                    {
                        if (item.Equals(selectedItem))
                        {
                            canvas.FillRectangle(overlayBrush, x, y, width, height);
                        }

                        canvas.DrawRectangle(pen, x, y, width, height);
                        String result = item.Type + " : " + item.Confidence.ToString("#0.##");
                        //System.Drawing.Font drawFont = new System.Drawing.Font("Arial", 16);
                        var font = new Font("Arial", image.Width / 50, FontStyle.Bold);
                        StringFormat drawFormat = new StringFormat();
                        drawFormat.Alignment = StringAlignment.Center;
                        canvas.DrawString(result, font, Brushes.Red, x, y);

                        canvas.Flush();
                    }
                }
            }
            var oldImage = this.pBox.Image;
            this.pBox.Image = image;
            oldImage?.Dispose();
        }

        private List<double> parsingObject(List<YoloItem> resultDetection)
        {
            //dari observasi terbentuk H
            List<string> objectFound = new List<string>();
            foreach (var item in resultDetection)
                objectFound.Add(item.Type);

            objectFound.Sort((x, y) => y.CompareTo(x));
            int i = 0, z;
            string name = objectFound[0];
            Stack<string> st = new Stack<string>();
            st.Push(name);
            foreach (var item in objectFound)
            {
                if (string.Equals(name, item, StringComparison.OrdinalIgnoreCase))
                {
                    i++;
                }
                else
                {
                    st.Push(item);
                    name = item;
                }
            }
            List<double> l = new List<double>();
            int indeks;
            foreach (var itemst in st)
            {
                string cari = itemst;
                indeks = 0;
                foreach (var item in objectFound)
                {
                    if (string.Equals(cari, item, StringComparison.OrdinalIgnoreCase))
                        indeks++;
                }
                l.Add(Convert.ToDouble(indeks));
            }
            string[] s = st.ToArray();
            phiLabel = s.ToList();
            return l;
        }

        private void normalPHI(List<double> phinorm)
        {
            if (phinorm.Count == 0)
            {
                throw new InvalidOperationException("Empty list");
            }
            double maxv = Double.MinValue;
            double total = 0;
            foreach (var p in phinorm)
            {
                /*if (p > maxv)
                {
                    maxv = p;
                }*/
                total = total + p;

            }
            //MessageBox.Show("Nilai Terbesar: " + maxv);
            for (int i = 0; i < phi.Count; i++)
            {
                phi[i] = phi[i] / total;
            }
            //string test = "";
            //foreach (var p in phi)
                //test = test + p + "\t";
            //MessageBox.Show("PHi: " + test);
        }
        private string interpretasikan(List<double> observasi, List<string> target, double[,] C)
        {
            double[,] cTarget = new double[observasi.Count, 3];
            for (int i = 0; i < target.Count; i++)
            {
                int indeks = objectL.FindIndex(x => x.Equals(target[i], StringComparison.OrdinalIgnoreCase));
                if (indeks >= 0)
                {
                    cTarget[i, 0] = C[indeks, 0];
                    cTarget[i, 1] = C[indeks, 1];
                    cTarget[i, 2] = C[indeks, 2];
                }
                else
                {
                    cTarget[i, 0] = 0;
                    cTarget[i, 1] = 0;
                    cTarget[i, 2] = 0;
                }

            }
            int argmax = cariR(observasi, cTarget);
            return sceneL[argmax];
        }
        private int cariR(List<double> observasi, double[,] cTarget)
        {
            double[] R = new double[3];
            double dummy = 0;
            for (int indeksR = 0; indeksR < R.Length; indeksR++)
            {
                dummy = 0;
                for (int baris = 0; baris < observasi.Count; baris++)
                {
                    dummy = dummy + observasi[baris] * cTarget[baris, indeksR];
                }
                R[indeksR] = dummy;
            }
           
            double maxv = 0;
            int argmax = -1;
            for (int x = 0; x < R.Length; x++)
            {
                if (R[x] > maxv)
                {
                    maxv = R[x];
                    argmax = x;
                }
            }
            return argmax;

            //foreach (var p in phiLabel)
            //    Console.Write(p + "\t");
        }

    }
}
