using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace ENBpresetAssistant.Pages.Transitions.PresetAdd
{
    /// <summary>
    /// AddPresetIntro.xaml 的交互逻辑
    /// </summary>
    public partial class AddPresetIntro : Window
    {
        public AddPresetIntro()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 窗口与拖动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ColorZone_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        /// <summary>
        /// 窗口关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
