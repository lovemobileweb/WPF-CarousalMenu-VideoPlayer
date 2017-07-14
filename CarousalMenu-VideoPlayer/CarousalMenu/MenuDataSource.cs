using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows.Media;

namespace CarousalMenu.Library
{
    public class MenuDataSource
    {
        public MenuDataSource() : this("", 0, Color.FromArgb(255, 238, 239, 241), Colors.Black, "")
        {
        }

        public MenuDataSource(string name) : this(name, 0, Color.FromArgb(255, 238, 239, 241), Colors.Black, "")
        {
        }

        public MenuDataSource(string name, double paddingLeft, Color bgColor, Color fgColor, string url = "")
        {
            Name = name;
            PaddingLeft = paddingLeft;
            BackgroundColor = bgColor;
            ForegroundColor = fgColor;
            Url = url;
        }

        #region Properties
        public string Name { get; set; }
        public double PaddingLeft { get; set; }
        public Color BackgroundColor { get; set; }
        public Color ForegroundColor { get; set; }
        public string Url { get; set; }

        ObservableCollection<MenuDataSource> _items = null;

        public ObservableCollection<MenuDataSource> Items
        {
            get
            {

                if (_items == null) _items = new ObservableCollection<MenuDataSource>();
                return _items;
            }
            set { _items = value; }
        }
        #endregion

        #region static method

        public static ObservableCollection<MenuDataSource> GetSampleList()
        {

            ObservableCollection<MenuDataSource> listToReturn = new ObservableCollection<MenuDataSource>();

            MenuDataSource subject = null;
            MenuDataSource capter = null;
            MenuDataSource topic = null;

            // Program_001 
            subject = new MenuDataSource("Program_001", 30, Color.FromArgb(255, 11, 134, 194), Color.FromArgb(255, 255, 249, 244));
            {
                capter = new MenuDataSource("Capter 1", 30, Color.FromArgb(255, 3, 91, 137), Color.FromArgb(255, 255, 249, 244));
                {
                    subject.Items.Add(capter);
                    topic = new MenuDataSource("Topic 1", 39, Color.FromArgb(255, 102, 176, 213), Color.FromArgb(255, 255, 249, 244));
                    {
                        capter.Items.Add(topic);
                        topic.Items.Add(new MenuDataSource("Leaf 1", 50, Color.FromArgb(255, 238, 239, 241), Color.FromArgb(255, 0, 0, 0), @".\Videos\Wildlife.mp4"));
                        topic.Items.Add(new MenuDataSource("Leaf 3", 50, Color.FromArgb(255, 238, 239, 241), Color.FromArgb(255, 0, 0, 0), @".\Videos\Wildlife.mp4"));
                    }
                    topic = new MenuDataSource("Topic 2", 39, Color.FromArgb(255, 102, 176, 213), Color.FromArgb(255, 255, 249, 244));
                    {
                        capter.Items.Add(topic);
                        topic.Items.Add(new MenuDataSource("Leaf 1", 50, Color.FromArgb(255, 238, 239, 241), Color.FromArgb(255, 0, 0, 0), @".\Videos\Wildlife.mp4"));
                        topic.Items.Add(new MenuDataSource("Leaf 2", 50, Color.FromArgb(255, 238, 239, 241), Color.FromArgb(255, 0, 0, 0), @".\Videos\Wildlife.mp4"));
                        topic.Items.Add(new MenuDataSource("Leaf 3", 50, Color.FromArgb(255, 238, 239, 241), Color.FromArgb(255, 0, 0, 0), @".\Videos\Wildlife.mp4"));
                        topic.Items.Add(new MenuDataSource("Leaf 4", 50, Color.FromArgb(255, 238, 239, 241), Color.FromArgb(255, 0, 0, 0), @".\Videos\Wildlife.mp4"));
                        topic.Items.Add(new MenuDataSource("Leaf 5", 50, Color.FromArgb(255, 238, 239, 241), Color.FromArgb(255, 0, 0, 0), @".\Videos\Wildlife.mp4")); 
                    }
                }
            }
            listToReturn.Add(subject);

            // Program_001 
            subject = new MenuDataSource("Program_002", 30, Color.FromArgb(255, 11, 134, 194), Color.FromArgb(255, 255, 249, 244));
            {
                capter = new MenuDataSource("Capter 1", 30, Color.FromArgb(255, 3, 91, 137), Color.FromArgb(255, 255, 249, 244));
                {
                    subject.Items.Add(capter);
                    topic = new MenuDataSource("Topic 1", 39, Color.FromArgb(255, 102, 176, 213), Color.FromArgb(255, 255, 249, 244));
                    {
                        capter.Items.Add(topic);
                        topic.Items.Add(new MenuDataSource("Leaf 1", 50, Color.FromArgb(255, 238, 239, 241), Color.FromArgb(255, 0, 0, 0), @".\Videos\Wildlife.mp4"));
                        topic.Items.Add(new MenuDataSource("Leaf 2", 50, Color.FromArgb(255, 238, 239, 241), Color.FromArgb(255, 0, 0, 0), @".\Videos\Wildlife.mp4"));
                        topic.Items.Add(new MenuDataSource("Leaf 3", 50, Color.FromArgb(255, 238, 239, 241), Color.FromArgb(255, 0, 0, 0), @".\Videos\Wildlife.mp4"));
                    }
                }
            }
            listToReturn.Add(subject);
            return listToReturn;
        }

        #endregion

        public override string ToString()
        {
           return base.ToString();
        }

    }
}
