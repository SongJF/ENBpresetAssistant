using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


using ENBpresetAssistant.Tools;
using MaterialDesignThemes.Wpf.Transitions;

namespace ENBpresetAssistant.Pages.Transitions.PresetAdd
{
    /// <summary>
    /// Intro1.xaml 的交互逻辑
    /// </summary>
    public partial class Slider1_Intro : UserControl
    {
        public Slider1_Intro()
        {
            InitializeComponent();

            DataContext = new Data.GlobalVariables_Preset();

            DirectotyTree_Load();
            Init_ComboBox();
        }

        private void NextBtn_Click(object sender, RoutedEventArgs e)
        {

            Transitioner.MoveNextCommand.Execute(null, null);
        }

        private void Init_ComboBox()
        {
            var Cores = CoreHelper.GetCoresFromJson();
            if(Cores!=null)
            {
                foreach (var item in Cores)
                {
                    ComboBoxItem comboBoxItem = new ComboBoxItem()
                    {
                        Content = item.CoreVersion
                    };
                    CoreSelection.Items.Add(comboBoxItem);
                }
            }
        }

        private void DirectotyTree_Load()
        {
            string Path = Directory.GetCurrentDirectory() + Data.ID.Dir_Temp;

            DirectotyTree.Items.Clear();
            DirectotyTree.Items.Add(TreeHelper.GetTreeViewItem(Path,Data.GlobalVariables_Preset.ZipName));
        }
    }
}
