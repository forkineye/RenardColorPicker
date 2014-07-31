using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Xceed.Wpf.Toolkit;

// my stuff
using System.IO.Ports;

namespace RenardColorPicker_WPF
{
    public partial class MainWindow : Window {
        static SerialPort   serial;
        static String       port = null;
        static UInt32       baud = 0;
        static UInt32       channels = 0;
        static Boolean      isEnabled = false;
        static Int32        r, g, b;

        public MainWindow() {
            InitializeComponent();
            serial = new SerialPort();
            serial.DataBits = 8;
            serial.Parity = Parity.None;
            serial.StopBits = StopBits.One;
            serial.Handshake = Handshake.None;
            serial.ReadTimeout = 500;
            serial.WriteTimeout = 500;
        }
        
        private void update(Color color) {
            if (isEnabled) {
                byte[] packet;
                packet = Renard.PacketBuilder(color.R, color.G, color.B, channels, cboxTemplate.Text);
                serial.Write(packet, 0, packet.Length);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) {
            foreach (string port in SerialPort.GetPortNames())
                cboxPort.Items.Add(port);
            
            colorcanvas.SelectedColor = Color.FromRgb(255, 255, 255);
            // default to white
            //colorcanvas.R = 255;
            //colorcanvas.G = 255;
            //colorcanvas.B = 255;

            // force button off
            btnEnable.IsChecked = false;
            btnEnable.Focusable = false;
        }

        private void cboxPort_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e) {
            port = cboxPort.Text;
        }

        private void cboxBaud_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e) {
            ComboBoxItem item = (ComboBoxItem)cboxBaud.SelectedItem;
            baud = Convert.ToUInt32(item.Content.ToString());
        }

        private void numChannels_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e) {
            channels = Convert.ToUInt32(numChannels.Value);
        }

        private void btnEnable_Checked(object sender, RoutedEventArgs e) {
            if (!(channels > 0) || !(baud > 0) || (port == null)) {
                btnEnable.IsChecked = false;
                return;
            } else {
                serial.PortName = cboxPort.Text;
                serial.BaudRate = Convert.ToInt32(cboxBaud.Text);
                serial.Open();
                isEnabled = true;
                update(colorcanvas.SelectedColor);
            }
        }

        private void btnEnable_Unchecked(object sender, RoutedEventArgs e) {
            isEnabled = false;
            if (serial.IsOpen) {
                byte[] packet;
                packet = Renard.PacketBuilder(0, 0, 0, channels, cboxTemplate.Text);
                serial.Write(packet, 0, packet.Length);
                serial.Close();
            }
        }

        private void colorcanvas_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<System.Windows.Media.Color> e) {
            update(e.NewValue);
        }

        private void colorcanvas_MouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e) {

        }

    }
}
