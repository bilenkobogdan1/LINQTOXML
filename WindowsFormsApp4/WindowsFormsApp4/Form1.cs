using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Xml.Linq;
using System.Xml;
using System.Text.RegularExpressions;

namespace WindowsFormsApp4
{ 
    public partial class Form1 : Form
    { 
        public Form1()
        {
            InitializeComponent();

        }
        private string Perform(string text)
        {
            text = Regex.Replace(text, "^(.*?)$", "<h2><p>$1</p><h2>\r\n", RegexOptions.Multiline);
            return text;
        }
        private string wayUpgreate(string text)
        {
            text = Regex.Replace(text, "xml", "html", RegexOptions.Multiline);
            return text;
        }
        class User
        {
        public string Name { get; private set; }
        public int Age { get; private set; }
        public DateTime Registered { get; private set; }
        public string Faculty { get; private set; }
        public string Residence { get; private set; }
        public int Course { get; private set; }
        
            public User(string name, int age, DateTime registered,string faculty,string residence,int course)
        {
            Name = name;
            Age = age;
            Registered = registered;
            Faculty = faculty;
            Residence = residence;
            Course = course;

        }

        public override string ToString()
        {
            return String.Format("{0}, вік {1}, дата заселення {2}, місце заселення{3} \n факультет {4}, курс {5}", Name, Age, Registered.ToShortDateString(), Residence, Faculty,Course);
        }

    }
    string textXml = "";
     string way;
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog OPF = new OpenFileDialog();
            //OPF.Multiselect = true;
           
            OPF.Filter = "Файлы html|*.html|Файлы xml|*.xml";
            if (OPF.ShowDialog() == DialogResult.OK)
            {
                foreach (string file in OPF.FileNames)
                {
                    MessageBox.Show(file);
                   
                    way = file;
                    StreamReader streamReader = new StreamReader(file);
                    
                   while (!streamReader.EndOfStream)
                   {
                       textXml = textXml+streamReader.ReadLine()+ "\r\n";
                      
                        
                    }
                   
                    textBox1.Text =way;

                }
            }
           

        }

        private void button2_Click(object sender, EventArgs e)
        {
                 List<User> users = new List<User>();

            using (XmlReader xr = XmlReader.Create(way))
            {
                string faculty="";
                string name = "";
                string residence="";
                int age = 0;
                int course = 0;
                DateTime registered = DateTime.Now;
                string element = "";
                while (xr.Read())
                {
                    // reads the element
                    if (xr.NodeType == XmlNodeType.Element)
                    {
                        element = xr.Name; // the name of the current element
                        if (element == "user")
                        {
                            age = int.Parse(xr.GetAttribute("age"));
                        }
                    }
                    // reads the element value
                    else if (xr.NodeType == XmlNodeType.Text)
                    {
                        switch (element)
                        {
                            case "name":
                                name = xr.Value;
                                break;
                            case "registered":
                                registered = DateTime.Parse(xr.Value);
                                break;
                            case "faculty":
                                faculty = xr.Value;
                                break;
                            case "residence":
                                residence = xr.Value;
                                break;
                            case "course":
                                course = int.Parse(xr.Value);
                                break;
                        }
                    }
                    // reads the closing element
                    else if ((xr.NodeType == XmlNodeType.EndElement) && (xr.Name == "user"))
                        users.Add(new User(name, age, registered,faculty, residence,course));
                    string filenew = "";
                    foreach (User u in users)
                    {
                        filenew += u+ "\r\n";                  
                    }
                    textBox1.Text = filenew;
                }
               
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Text = Perform(textBox1.Text);
            File.WriteAllText(wayUpgreate(way), textBox1.Text);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox1.Text += "LINQ\r\n";
            XDocument xdoc = XDocument.Load(way);
            foreach (XElement phoneElement in xdoc.Element("users").Elements("user"))
            {
                XAttribute ageAttribute = phoneElement.Attribute("age");
                XElement nameElement = phoneElement.Element("name");
                XElement registeredElement = phoneElement.Element("registered");
                XElement facultyElement = phoneElement.Element("faculty");
                XElement courseElement = phoneElement.Element("course");
                XElement residenceElement = phoneElement.Element("residence");


                if (ageAttribute != null && nameElement != null && registeredElement != null && facultyElement!= null && courseElement != null &&  residenceElement != null)
                {
                   
                    textBox1.Text += ($"Компания: {nameElement.Value}");
                    textBox1.Text += ($"дата заселення {ageAttribute.Value}");
                    textBox1.Text += ($"місце заселення \r\n {registeredElement.Value}");
                    textBox1.Text += ($"дата заселення {residenceElement.Value}");
                    textBox1.Text += ($"факультет {facultyElement.Value}");
                    textBox1.Text += ($"курс {courseElement.Value}");
                    
                }
                
            }
        }
    }
}
