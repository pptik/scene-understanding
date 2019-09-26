using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    class TrainSUN12CVR13
    {
        public string[] LabelScene { get; set; }
        public string[] LabelObject { get; set; }
        public string[] CTrain { get; set; }

        public TrainSUN12CVR13()
        {
            CTrain = getTrain();
        }
        public string[] getTrain()
        {
            string[] dataC = null;
            dataC = System.IO.File.ReadAllLines(@"Train/trainbagiobject.txt");
            return dataC;
        }
        public string[] getTLabelObject()
        {
            string[] objectL = System.IO.File.ReadAllLines(@"Train/objectcvr13.txt"); ;
            return objectL;
        }
        public string[] getLabelScene()
        {
            string[] scene = System.IO.File.ReadAllLines(@"Train/scenecvr13.txt");
            return scene;
        }
    }
}
